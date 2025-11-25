
using UnityEngine;
using System.Linq;

namespace FStudio.MatchEngine.Players.Behaviours {
    /// <summary>
    /// When we are close to our goal net, if someone is around us.
    /// </summary>
    public class CriticalSendBallToSafe : BallChasingBehaviour {
        private readonly float maxBallProgress = 0.2f;

        private const float RISK_AREA = 3f;

        private readonly float sendAwayPowerMin = 26;
        private readonly float sendAwayPowerMax = 38;

        private Vector3 targetSendAwayPosition;

        public override bool Behave(bool isAlreadyActive) {
            if (!Player.IsHoldingBall) {
                return false;
            }

            if (Player.GameTeam.BallProgress > maxBallProgress) {
                return false;
            }

            if (!isAlreadyActive) {
                // check around.
                var myPos = Player.Position;
                if (
                    opponents.Where(x => x.PlayerController.IsPhysicsEnabled && 
                    Vector3.Distance(x.Position, myPos) < RISK_AREA).Any()) {
                    var forward = new Vector3(Player.toGoalXDirection, 0, 0);

                    var forwardLook = Quaternion.LookRotation(forward);
                    var myLook = Quaternion.LookRotation(Player.Direction);

                    forward = Quaternion.Slerp(myLook, forwardLook, 0.5f) * Vector3.forward;

                    targetSendAwayPosition = Player.Position + forward * Random.Range(sendAwayPowerMin, sendAwayPowerMax);

                    isAlreadyActive = true;
                }
            }

            if (isAlreadyActive) {
                if (Player.LookTo(in deltaTime, targetSendAwayPosition - Player.Position)) {
                    Player.Cross(targetSendAwayPosition);
                }

                return true;
            }

            return false;
        }
    }
}
