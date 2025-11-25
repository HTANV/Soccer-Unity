using FStudio.Input;
using FStudio.UI.GamepadInput;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using System.Threading.Tasks;

namespace FStudio.UI.Utilities {
    [RequireComponent(typeof(TMP_InputField))]
    public class InteractiveInput : MonoBehaviour, ISnappable {
        public static InteractiveInput Current { get; private set; }

        [SerializeField] private TMP_InputField inputField;

        public Vector3 position => transform.position;

        public GameObject gObject => gameObject;

        public bool isSnappable => 
            enabled &&
            gameObject.activeInHierarchy &&
            IsInteractable;

        public bool IsInteractable => true;

        public bool blockHorizontalNavigation => false;

        private void OnValidate() {
            this.inputField = GetComponent<TMP_InputField>();
        }

        private void Start () {
            // register.
            this.inputField.onSelect.AddListener(OnInputSelect);
            this.inputField.onEndEdit.AddListener(OnInputUnSelect);
        }

        public ScrollRect OnSnap() {
            var scrollView = GetComponentsInParent<ScrollRect>(true).FirstOrDefault();
            return scrollView;
        }

        public void OnSnapLeft () {

        }

        private void OnInputSelect (string _) {
            Current = this;
        }

        private async void OnInputUnSelect (string _) {
            await Task.Delay(100);
            Current = null;
        }

        public void OnClick () {
            if (!inputField.isFocused) {
                inputField.Select();
            } else {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}
