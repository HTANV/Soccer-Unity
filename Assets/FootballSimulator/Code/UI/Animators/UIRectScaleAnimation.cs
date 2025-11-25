using FStudio.Animation;

using UnityEngine;

namespace FStudio.UI.Animators {
    public class UIRectScaleAnimation : IScalable {
        private readonly RectTransform rectTransform;

        public UIRectScaleAnimation(RectTransform rectTransform) {
            this.rectTransform = rectTransform;
        }

        public Vector3 GetScale() => rectTransform.localScale;

        public void SetScale(Vector3 scale) => rectTransform.localScale = scale;
    }
}
