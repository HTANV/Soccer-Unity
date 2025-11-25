
using FStudio.MatchEngine.Players.PlayerController;
using UnityEngine;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class GKGoToPenaltyPoint : BaseBehaviour {
        private const float
            PENALTY_BOX_RANDOM_MIN = 4,
            PENALTY_BOX_RANDOM_MAX = 4,
            PENALTY_POINT_FROM_GOALNET = 8;

        private bool skipGointToPenaltyPoint = false;

        private MovementType movementType;

        private Vector3? targetPoint;

        public override sealed bool Behave(bool isAlreadyActive) {
            if (!Player.IsGKUntouchable) {
                skipGointToPenaltyPoint = false;
                targetPoint = null;
                return false;
            }

            if (skipGointToPenaltyPoint) {
                targetPoint = null;
                return false;
            }

            if (Player.IsGoalKickHolder) {
                skipGointToPenaltyPoint = false;
                targetPoint = null;
                return false;
            }


            if (!targetPoint.HasValue) {
                targetPoint = goalNet.Position + goalNet.Direction * PENALTY_POINT_FROM_GOALNET;
                targetPoint +=
                    new Vector3(
                        0,
                        0,
                        Random.Range(PENALTY_BOX_RANDOM_MIN, PENALTY_BOX_RANDOM_MAX));

                var randomValue = Random.Range(0, 100);
                if (randomValue < 50) {
                    movementType = MovementType.BestHeCanDo;
                } else {
                    movementType = MovementType.Normal;
                }

                if (!Player.IsFrontOfMe (targetPoint.Value)) {
                    skipGointToPenaltyPoint = true;
                    return false;
                }
            }

            if (!skipGointToPenaltyPoint) {
                // go to penalty box.

                if (Player.MoveTo(in deltaTime, targetPoint.Value, true, movementType)) {
                    skipGointToPenaltyPoint = true; // we have waited to recover team density, but not anymore.
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
