using FStudio.Animation;

using UnityEngine;

namespace FStudio.UI.Animators {
    public class UIRectRotateAnimation : IRotatable {
        private readonly RectTransform rectTransform;

        public UIRectRotateAnimation(RectTransform rectTransform) {
            this.rectTransform = rectTransform;
        }

        public Quaternion GetRotation () => rectTransform.localRotation;

        public void SetRotation (Quaternion rotation) => rectTransform.localRotation = rotation;
    }
}
