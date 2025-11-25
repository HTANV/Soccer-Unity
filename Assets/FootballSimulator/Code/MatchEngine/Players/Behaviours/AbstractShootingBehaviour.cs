
using UnityEngine;

namespace FStudio.MatchEngine.Players.Behaviours {
    public abstract class AbstractShootingBehaviour : BaseBehaviour {
        private const float MIN_Z_DISTANCE_SQR_TO_ANGLE_CHECK = 4;
        private const float MAX_X_DISTANCE_SQR_TO_ANGLE_CHECK = 6;
        private const float MAX_ANGLE = 80;

        protected bool CanShoot () {
            var goalToMe = Player.Position - targetGoalNet.Position;

            if (Mathf.Abs (goalToMe.z) > MIN_Z_DISTANCE_SQR_TO_ANGLE_CHECK && 
                Mathf.Abs (goalToMe.x) <= MAX_X_DISTANCE_SQR_TO_ANGLE_CHECK) {

                var angleBetweenGoal = AngleToGoal (targetGoalNet);

                if (angleBetweenGoal > MAX_ANGLE) {
                    Debug.Log("Cannot shoot.");
                    return false;
                }
            }

            return true;
        }

        protected float AngleToGoal (GoalNet goalNet) {
            var playerPosition = Player.Position;

            var dirToLeft = goalNet.leftLimit.position - playerPosition;
            var dirToRight = goalNet.rightLimit.position - playerPosition;

            var leftAngle = Mathf.Abs (Vector3.SignedAngle(Player.GoalDirection, dirToLeft, Vector3.up));
            var rightAngle = Mathf.Abs(Vector3.SignedAngle(Player.GoalDirection, dirToRight, Vector3.up));

            var final = Mathf.Min (leftAngle, rightAngle);

            return final;
        }
    }
}
