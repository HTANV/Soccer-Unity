using System.Threading.Tasks;
using UnityEngine;

namespace FStudio.Animation {
    public class ColorAction : BaseAction {
        private readonly float speed;
        private readonly Gradient gradient;
        private readonly float intensity;
        private readonly IColorable colorable;
        private readonly string colorName;

        public ColorAction(IColorable colorable, string colorName, float speed, Gradient gradient, float intensity = 1) {
            this.speed = speed;
            this.gradient = gradient;
            this.colorable = colorable;
            this.colorName = colorName;
            this.intensity = intensity;
        }

        public override bool Update(in float deltaTime) {
            Progress = Mathf.Min(Progress + deltaTime * speed, 1);

            var newColor = gradient.Evaluate(Progress);
            float alpha = newColor.a;
            newColor *= intensity;
            newColor.a = alpha;

            colorable.SetColor(colorName, newColor);

            return Progress == 1;
        }

        public override void Finish() {
            colorable.SetColor(colorName, gradient.Evaluate(1));
        }
    }
}
