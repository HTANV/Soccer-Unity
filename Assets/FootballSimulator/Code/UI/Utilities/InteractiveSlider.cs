using FStudio.Input;
using FStudio.UI.GamepadInput;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FStudio.UI.Utilities {
    [RequireComponent(typeof (Slider))]
    public class InteractiveSlider : MonoBehaviour, ISnappable {
        public Slider slider;

        public Vector3 position => transform.position;

        public bool blockHorizontalNavigation => true;

        public GameObject gObject => gameObject;
		
		private const float minInputMagnitude = 0.5f;
        private const float maxYMagnitude = 0.2f;
        private const float inputStep = 0.05f;
        private const float inputDelay = 0.1f;

        private float nextInput;

        public bool isSnappable => enabled && 
            gameObject.activeInHierarchy && 
            IsInteractable;

        private InputListener inputListener;

        private Vector2 currentPointerDirection;

        public bool IsInteractable => true;
		
		private bool isValidInput (Vector3 dir) {
			if (Mathf.Abs(dir.y) > maxYMagnitude ||
                Mathf.Abs(dir.x) < minInputMagnitude) {
                return false;
            }
			
			return true;
		}


        private void OnValidate() {
            slider = GetComponent<Slider>();
        }

        public ScrollRect OnSnap() {
            inputListener = new InputListener("UI", 0);

            inputListener.RegisterAction("Navigate", InputUpdate);

            var scrollView = GetComponentsInParent<ScrollRect>(true).FirstOrDefault ();
            return scrollView;
        }

        public void OnSnapLeft() {
            if (inputListener == null) {
                return;
            }

            inputListener.Clear();

            inputListener = null;
        }

        private void OnDisable() {
            OnSnapLeft();
        }

        public void OnClick() { }

        private bool InputUpdate (InputAction.CallbackContext ctx) {
            currentPointerDirection = ctx.ReadValue<Vector2>();

            Debug.Log("slider update.");

			if (isValidInput (currentPointerDirection)) {
				return true;
			}
			
			return false;
        }

        private void Update() {
            float time = Time.time;
			
			if (!isValidInput (currentPointerDirection)) {
				return;
			}

            if (nextInput < time) {
                float val = slider.value + (slider.wholeNumbers ? 1 : inputStep) * currentPointerDirection.x;
                slider.value = val;
                nextInput = time + inputDelay;
            }
        }
    }
}
