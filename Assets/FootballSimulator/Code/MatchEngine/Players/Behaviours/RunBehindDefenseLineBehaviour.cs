using FStudio.MatchEngine.Enums;
using FStudio.Data;
using UnityEngine;

namespace FStudio.MatchEngine.Players.Behaviours {
    /// <summary>
    /// Target man runs to the ball holder teammate to take a pass.
    /// </summary>
    public class RunBehindDefenseLineBehaviour : BaseBehaviour {
        private const float MIN_DISTANCE_TO_HOLDER = 30f;
        private const float MIN_DISTANCE_TO_OFFSIDELINE_TO_RUN = 5f;
        private const float MAX_DISTANCE_TO_OFFSIDELINE_TO_RUN = 10f;

        private Vector3 runDirection;

        public RunBehindDefenseLineBehaviour () {

        }

        public override bool Behave(bool isAlreadyActive) {
            if (ball.HolderPlayer == null || ball.HolderPlayer == Player || ball.HolderPlayer.GameTeam != Player.GameTeam) {
                ForceBehaviour = false;
                return false;
            }

            if (!isAlreadyActive) {
                // check activation.

                if (Player.IsInOffside) {
                    ForceBehaviour = false;
                    return false;
                }

                float distanceToHolder = Vector3.Distance(Player.Position, ball.HolderPlayer.Position);
                float currentDistanceToOffsideLine = Mathf.Abs(Player.Position.x - offsideLine);

                if (distanceToHolder < MIN_DISTANCE_TO_HOLDER &&
                    currentDistanceToOffsideLine > MIN_DISTANCE_TO_OFFSIDELINE_TO_RUN &&
                    currentDistanceToOffsideLine < MAX_DISTANCE_TO_OFFSIDELINE_TO_RUN) {

                    runDirection = (targetGoalNet.Position - Player.Position).normalized * 5;
                    ForceBehaviour = true;
                } else {
                    return false;
                }
            }


            if (Player.IsInOffside) {
                // stop running. we are in offside.
                ForceBehaviour = false;
                return false;
            }

            Player.CurrentAct = Acts.RunningBehindTheDefenseLine;

            var targetPosition = Player.Position + runDirection;

            Player.AvoidMarkers(teammates, ref targetPosition, 5);

            Player.MoveTo(in deltaTime, targetPosition, true);

            return true;
        }
    }
}
