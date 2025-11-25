using FStudio.Input;
using FStudio.UI.GamepadInput;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FStudio.UI.Navigation {
    [RequireComponent(typeof(NavigationManager))]
    [DisallowMultipleComponent]
    public class NavigationGamepadInput : MonoBehaviour {
        [HideInInspector] [SerializeField] private NavigationManager manager;

        private const string LEFT = "Left", RIGHT = "Right";

        [SerializeField] private GameObject navigationHelpers;

        private InputListener inputListener;

        private void Update() {
            navigationHelpers.SetActive(SnapManager.Current?.IsEnabled == true);
        }

        private void OnValidate() {
            manager = GetComponent<NavigationManager>();
        }

        private void OnEnable() {
            inputListener = new InputListener("UI", 0);
            inputListener.RegisterAction(LEFT, Left);
            inputListener.RegisterAction(RIGHT, Right);
        }

        private void OnDisable() {
            inputListener.Clear();
            inputListener = null;
        }

        private bool Left (InputAction.CallbackContext obj) {
            InputUpdate(false);

            return true;
        }

        private bool Right (InputAction.CallbackContext obj) {
            InputUpdate(true);

            return true;
        }

        private async void InputUpdate(bool forward) {
            // check layer.
            var snapLayer = GetComponentInParent<SnapLayer>();
            if (!SnapManager.Current.IsActiveLayer (snapLayer.Layer)) {
                return; // layer doesnt match.
            }

            var cMember = manager.CurrentIndex;
            var length = manager.Length;

            cMember += forward ? 1 : -1;

            if (cMember < 0) {
                cMember = length - 1;
            } else if (cMember >= length) {
                cMember = 0;
            }

            manager.SetIndexSilent (cMember);
            var active = manager.GetActiveMember();
            await SnapManager.AutoSnap(active, true);

            manager.SetIndex(cMember);
        }
    }
}
