using UnityEngine;
using UnityEngine.UI;

using FStudio.Utilities;
using FStudio.Animation;

using System;

namespace FStudio {
    public class CameraTransition : SceneObjectSingleton <CameraTransition>, IDynamicFloat  {
        [SerializeField] private RawImage rawImage = default;
        [SerializeField] private CanvasGroup canvasGroup = default;
        [SerializeField] private AnimationCurve fadeOutCurve = default;
        [SerializeField] private float fadeOutSpeed = default;

        public void StartTransition (Action onCompleted = null) {
            TakePicture();
            SetFloat(1);

            //Fade out.
            new DynamicFloatAction(this, 1, 0, fadeOutSpeed, fadeOutCurve).
                GetQuery ().
                Start (this, onCompleted);
        }

        private void TakePicture () {
            RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 8);
            Camera.main.targetTexture = renderTexture;
            RenderTexture.active = renderTexture;
            Camera.main.Render();
            Camera.main.targetTexture = null;
            rawImage.texture = renderTexture;
        }

        public float GetFloat() {
            return canvasGroup.alpha;
        }

        public void SetFloat(float dynamicFloat) {
            canvasGroup.alpha = dynamicFloat;
        }
    }
}
