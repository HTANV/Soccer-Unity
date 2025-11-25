using UnityEngine;

namespace FStudio.Animation {
    public class DynamicFloatAction : BaseAction {
        private readonly float start, end;
        private readonly float speed;
        private readonly AnimationCurve curve;
        private readonly IDynamicFloat dynamicFloat;

        public DynamicFloatAction(IDynamicFloat dynamicFloat, float start, float end, float speed, AnimationCurve curve) {
            this.dynamicFloat = dynamicFloat;
            this.start = start;
            this.end = end;
            this.speed = speed;
            this.curve = curve;
        }

        public sealed override bool Update(in float deltaTime) {
            Progress = Mathf.Min(Progress + deltaTime * speed, 1);

            var newFloat = Mathf.Lerp(start, end, curve.Evaluate(Progress));

            dynamicFloat.SetFloat(newFloat);

            return Progress == 1;
        }

        public override void Finish() {
            dynamicFloat.SetFloat(end);
        }
    }
}
