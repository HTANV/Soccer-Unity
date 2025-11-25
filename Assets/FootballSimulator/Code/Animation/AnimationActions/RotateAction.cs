using UnityEngine;

namespace FStudio.Animation {
    public class RotateAction : BaseAction {
        private readonly Quaternion startRotation;
        private readonly Quaternion targetRotation;
        private readonly float speed;
        private readonly AnimationCurve curve;
        private readonly IRotatable rotatable;

        public RotateAction(IRotatable rotatable, Quaternion targetRotation, float speed, AnimationCurve curve) {
            startRotation = rotatable.GetRotation();

            this.targetRotation = targetRotation;
            this.speed = speed;
            this.curve = curve;
            this.rotatable = rotatable;
        }

        public sealed override bool Update(in float deltaTime) {
            Progress = Mathf.Min(Progress + deltaTime * speed, 1);

            var newRotation = Quaternion.Slerp (startRotation, targetRotation, curve.Evaluate (Progress));

            rotatable.SetRotation(newRotation);

            return Progress == 1;
        }

        public override void Finish() {
            rotatable.SetRotation(targetRotation);
        }
    }
}
