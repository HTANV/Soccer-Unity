
using FStudio.MatchEngine.Enums;

using UnityEngine;

using System.Linq;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class BecomeAPassOptionBehaviour : BaseBehaviour {
        private const float RANGE_FOR_PASS_OPTION = 10f;
        private static Vector3 BOX_CAST_SIZE_FOR_CHECK = new Vector3(3,1f,3);
        private const float MIN_DISTANCE_FOR_PASS_OPTION = 15f;
        private const float MAX_DISTANCE_FOR_PASS_OPTION = 35f;
        private const float PASS_OPTION_DELAY_AS_SECONDS = 10f;
        private const float MIN_X_DIFFERENCE = 10f;

        private const int DIRECTION_COUNT = 8;

        private static Vector3[] DIRECTIONS = new Vector3[DIRECTION_COUNT] { 
            new Vector3(1,0,0),
            new Vector3(1,0,1),
            new Vector3(1,0,-1),
            new Vector3(-1,0,0),
            new Vector3(-1,0,1),
            new Vector3(-1,0,-1),
            new Vector3(0,0,1),
            new Vector3(0,0,-1),
        };

        private readonly Collider[] m_alloc = new Collider[10];
        private float nextPassOption;
        private Vector3 targetPosition;

        private bool IsInBounds (in Vector3 position) {
            if (position.x <= 0 || position.x >= fieldEndX || position.z >= fieldEndY || position.z < 0) {
                return false;
            }

            return true;
        }

        public override bool Behave(bool isAlreadyActive) {
            if (Player.IsHoldingBall) {
                return false;
            }

            if (!isAlreadyActive) {
                if (ball.HolderPlayer == null || ball.HolderPlayer.GameTeam != Player.GameTeam) {
                    return false;
                }

                if (nextPassOption < time) {
                    // find a spot to run to take the ball.
                    var holderPlayerPosition = ball.HolderPlayer.Position;

                    var closestTeammate = teammates.Where(x => x != ball.HolderPlayer &&
                    !x.IsGK &&
                    Vector3.Distance(holderPlayerPosition, x.Position) < MAX_DISTANCE_FOR_PASS_OPTION &&
                    Vector3.Distance(holderPlayerPosition, x.Position) < MIN_DISTANCE_FOR_PASS_OPTION &&
                    Mathf.Abs (holderPlayerPosition.x - x.Position.x) > MIN_X_DIFFERENCE
                    ).
                    OrderBy(x => Vector3.Distance(x.Position, holderPlayerPosition)).FirstOrDefault();
                    
                    if (closestTeammate != Player) {
                        return false;
                    }

                    var myPosition = Player.Position;

                    // order by closest direction, then take 3 of them.
                    var possibleDirs = DIRECTIONS.OrderBy(x => Vector3.Distance(holderPlayerPosition + x * RANGE_FOR_PASS_OPTION, myPosition)).Take (3).Where (x=> IsInBounds(x));

                    foreach (var dir in possibleDirs) {
                        var checkPos = holderPlayerPosition + dir * RANGE_FOR_PASS_OPTION;

                        int count = Physics.OverlapBoxNonAlloc(
                            checkPos,
                            BOX_CAST_SIZE_FOR_CHECK, m_alloc,
                            Quaternion.identity, 1 << LayerMask.NameToLayer(Tags.PLAYER_LAYER));

                        if (count == 0 || (count == 1 && m_alloc[0].transform == ball.HolderPlayer.PlayerController.UnityObject.transform)) {
                            Debug.Log("[BECOMEAPASSOPTION] Become a pass option started.");

                            isAlreadyActive = true;
                            targetPosition = checkPos;
                            nextPassOption = time + PASS_OPTION_DELAY_AS_SECONDS;

                            Player.CurrentAct = Acts.BecomeAPassOption;
                            break;
                        }
                    }
                }
            }

            if (isAlreadyActive) {
                if (Player.IsInOffside) {
                    return false;
                }

                if (IsTheBallGoingOutside()) {
                    return false;
                }

                Player.AvoidMarkers(teammates, ref targetPosition, 5);

                if (!Player.MoveTo(in deltaTime, targetPosition, false)) {
                    Debug.Log("[BECOMEAPASSOPTION] Become a pass option reached.");
                    return false;
                } else {
                    return true;
                }
            }

            return false;
        }
    }
}
