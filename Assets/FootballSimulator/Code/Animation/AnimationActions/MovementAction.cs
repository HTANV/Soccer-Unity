using UnityEngine;

namespace FStudio.Animation {
    public class MovementAction : BaseAction {
        private readonly Vector3 startPosition, targetPosition;
        private readonly float speed;
        private readonly AnimationCurve movementCurve;
        private readonly AnimationCurve heightCurve;
        private readonly bool useHeightCurve;
        private readonly bool lookForward;
        private readonly IMovable movable;

        public MovementAction(IMovable movable, Vector3 targetPosition, float speed, AnimationCurve movementCurve) {
            startPosition = movable.GetPosition();

            this.movable = movable;
            this.targetPosition = targetPosition;
            this.speed = speed;
            this.movementCurve = movementCurve;
        }

        public MovementAction(IMovable movable, Vector3 targetPosition, float speed, AnimationCurve movementCurve, AnimationCurve heightCurve, bool lookForward = false) {
            startPosition = movable.GetPosition();

            this.movable = movable;
            this.targetPosition = targetPosition;
            this.speed = speed;
            this.movementCurve = movementCurve;
            this.heightCurve = heightCurve;
            this.lookForward = lookForward;

            if (heightCurve != null) {
                useHeightCurve = true;
            }
        }

        public sealed override bool Update(in float deltaTime) {
            Progress = Mathf.Min(Progress + deltaTime * speed, 1);

            var newPosition = Vector3.Lerp(startPosition, targetPosition, movementCurve.Evaluate(Progress));
            
            if (useHeightCurve)
                newPosition.y += heightCurve.Evaluate(Progress);

            if (lookForward) {
                movable.SetRotation(Quaternion.LookRotation(newPosition - movable.GetPosition()));
            }

            movable.SetPosition(newPosition);

            return Progress == 1;
        }

        public override void Finish() {
            movable.SetPosition(targetPosition);
        }
    }
}
