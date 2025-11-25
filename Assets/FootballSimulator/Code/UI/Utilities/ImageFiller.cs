using UnityEngine;
using UnityEngine.UI;

using FStudio.Animation;
using TMPro;

namespace FStudio.UI.Utilities {
    public class ImageFiller : MonoBehaviour, IDynamicFloat {
        [SerializeField] private Image fillerImage;

        [SerializeField] private AnimationCurve fillCurve;
        [SerializeField] private float fillSpeed;

        [SerializeField] private Gradient fillAmountGradient;

        public Image Image => fillerImage;

        private AnimationQuery currentAnimation;

        public void FillTo(float target) {
            if (currentAnimation != null) {
                currentAnimation.Stop();
            }

            var current = fillerImage.fillAmount;

            if (!isActiveAndEnabled) {
                SetFloat(target);
            } else {
                currentAnimation = new DynamicFloatAction(this, current, target, fillSpeed, fillCurve).GetQuery();

                currentAnimation.Start(this, () => {
                    currentAnimation = null;
                });
            }
        }

        public float GetFloat() => fillerImage.fillAmount;

        public void SetFloat(float dynamicFloat) {
            fillerImage.fillAmount = dynamicFloat;
            fillerImage.color = fillAmountGradient.Evaluate(dynamicFloat);
        }

#if UNITY_EDITOR
        [SerializeField] private bool enableDebugging;
        [Range(0f, 1f)]
        [SerializeField] private float debugFill;
        private void Update() {
            if (enableDebugging) {
                FillTo(debugFill);
                enableDebugging = false;
            }
        }
#endif
    }
}
