using FStudio.Input;
using FStudio.UI.GamepadInput;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FStudio.UI.Navigation {
    [DisallowMultipleComponent]
    public class GamepadClick : MonoBehaviour {
        [SerializeField] private string actionMap = "Approve";
        private InputListener inputListener;

        private async void OnEnable() {
            await Task.Yield();

            inputListener = new InputListener("UI", 0, 0);
            inputListener.RegisterAction(actionMap, InputUpdate);
        }

        private async void OnDisable() {
            while (inputListener == null) {
                await Task.Yield();
            }

            inputListener.Clear();
            inputListener = null;
        }

        private bool InputUpdate(InputAction.CallbackContext obj) {
            var inputValue = obj.ReadValue<float>();
            if (inputValue != 1) {
                return false;
            }

            var snappable = GetComponent<ISnappable>();
            var isInLayer = SnapManager.Current.IsInLayer(snappable);
            if (isInLayer) {
                snappable.OnClick();
            }

            return true;
        }
    }
}
