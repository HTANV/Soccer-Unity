
using UnityEngine;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class SendBallToSafe : BallChasingBehaviour {

        private readonly float maxBallProgress = 0.3f;

        private const float ANGLE_MIN = -80;
        private const float ANGLE_MAX = 80;

        private readonly float sendAwayPowerMin = 20;
        private readonly float sendAwayPowerMax = 30;

        private readonly float X_SIZE = 22;

        private Vector3 targetSendAwayPosition;
        
        public override bool Behave(bool isAlreadyActive) {
            if (!Player.IsHoldingBall) {
                return false;
            }

            if (Player.GameTeam.TeamDensity < Player.GameTeam.BallProgress) {
                // we have backup behind. so ignore.
                return false;
            }

            if (Player.GameTeam.BallProgress > maxBallProgress) {
                return false;
            }

            var playerZ = Player.Position.z;

            if (playerZ < (fieldEndY / 2) - X_SIZE || playerZ > (fieldEndY / 2) + X_SIZE) {
                return false;
            }

            if (!isAlreadyActive) {
                var forward = new Vector3 (Player.toGoalXDirection, 0, 0);

                var directionErrY = Random.Range(ANGLE_MIN, ANGLE_MAX);
                forward = Quaternion.Euler(0, directionErrY, 0) * forward;

                targetSendAwayPosition = Player.Position + forward * Random.Range(sendAwayPowerMin, sendAwayPowerMax);
            }

            if (Player.LookTo (in deltaTime, targetSendAwayPosition - Player.Position)) {
                Player.Cross(targetSendAwayPosition);
            }

            return true;
        }
    }
}
