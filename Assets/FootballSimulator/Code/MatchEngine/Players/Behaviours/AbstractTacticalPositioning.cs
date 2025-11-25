
using FStudio.MatchEngine.Players.PlayerController;
using UnityEngine;

namespace FStudio.MatchEngine.Players.Behaviours {
    public abstract class AbstractTacticalPositioning : BaseBehaviour {
        private const float TARGET_POSITION_MIN_LERP_SPEED = 0.5f;
        private const float TARGET_POSITION_MAX_LERP_SPEED = 1.5f;

        private Vector3 m_TargetPosition;
        protected Vector3 targetPosition {
            get {
                return m_TargetPosition;
            }

            set {
                if (m_TargetPosition == Vector3.zero) {
                    m_TargetPosition = value;
                } else {
                    // Lerp.
                    m_TargetPosition = Vector3.Lerp(
                        m_TargetPosition,
                        value,
                        deltaTime *
                        Random.Range(TARGET_POSITION_MIN_LERP_SPEED, TARGET_POSITION_MAX_LERP_SPEED));
                }
            }
        }

        public MovementType RequiredMovementType(Vector3 start, Vector3 end) {
            var distance = Vector3.Distance(start, end);

            if (distance > 4) {
                return MovementType.BestHeCanDo;
            } else if (distance > 2) {
                return MovementType.Normal;
            } else {
                return MovementType.Relax;
            }
        }
    }
}
