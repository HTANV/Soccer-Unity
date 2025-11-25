using FStudio.Animation;

using UnityEngine;

namespace FStudio.UI.Animators {
    public class UIRectMovementAnimation : IMovable {
        private readonly RectTransform rectTransform;

        public UIRectMovementAnimation(RectTransform rectTransform) {
            this.rectTransform = rectTransform;
        }

        public Vector3 GetPosition() => rectTransform.anchoredPosition;

        public Quaternion GetRotation() => rectTransform.localRotation;

        public void SetPosition(Vector3 targetPosition) => rectTransform.anchoredPosition = targetPosition;

        public void SetRotation(Quaternion targetRotation) => rectTransform.localRotation = targetRotation;
    }
}
