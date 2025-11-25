using UnityEngine;

namespace FStudio.Animation {
    public class ScaleAction : BaseAction {
        private readonly Vector3 startScale, targetScale;
        private readonly float speed;
        private readonly AnimationCurve scaleCurve;
        private readonly IScalable scalable;

        public ScaleAction(IScalable scalable, Vector3 targetScale, float speed, AnimationCurve scaleCurve) {
            startScale = scalable.GetScale();

            this.scalable = scalable;
            this.targetScale = targetScale;
            this.speed = speed;
            this.scaleCurve = scaleCurve;
        }

        public sealed override bool Update(in float deltaTime) {
            Progress = Mathf.Min(Progress + deltaTime * speed, 1);

            var newScale = Vector3.Lerp(startScale, targetScale, scaleCurve.Evaluate(Progress));

            scalable.SetScale (newScale);

            return Progress == 1;
        }

        public override void Finish() {
            scalable.SetScale (targetScale);
        }
    }
}
