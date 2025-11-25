using FStudio.Animation;

using UnityEngine;

namespace FStudio.UI.Animators {
    public class UIAlphaAnimation : IDynamicFloat {
        private readonly CanvasGroup canvasGroup;

        public UIAlphaAnimation(CanvasGroup canvasGroup) {
            this.canvasGroup = canvasGroup;
        }

        public float GetFloat() => canvasGroup.alpha;

        public void SetFloat(float dynamicFloat) {
            canvasGroup.alpha = dynamicFloat;
        }
    }
}
