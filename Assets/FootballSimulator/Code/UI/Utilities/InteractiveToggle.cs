using FStudio.UI.GamepadInput;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FStudio.UI.Utilities {
    [RequireComponent(typeof(Toggle))]
    public class InteractiveToggle : MonoBehaviour, 
        IPointerEnterHandler, 
        IPointerExitHandler, 
        ISnappable {

        public Toggle Toggle;

        public Action onClick;

        public UnityEvent onFalse, onTrue;

        public Vector3 position => transform.position;

        public bool blockHorizontalNavigation => false;

        [SerializeField] private UnityEvent onSnap, onSnapLeft;

        public GameObject gObject => gameObject;

        public bool isSnappable => enabled &&
            gameObject.activeInHierarchy &&
            IsInteractable;

        private void Awake() {
            Toggle.onValueChanged.AddListener((val) => {
                if (val) {
                    onTrue?.Invoke();
                } else {
                    onFalse?.Invoke();
                }
            });
        }

        public bool IsInteractable => true;

        private void OnValidate() {
            this.Toggle = GetComponent<Toggle>();
        }

        public ScrollRect OnSnap() {
            onSnap?.Invoke();

            var scrollView = GetComponentsInParent<ScrollRect>(true).FirstOrDefault();
            return scrollView;
        }

        public void OnSnapLeft() {
            onSnapLeft?.Invoke();
        }

        public void OnClick() {
            Toggle.isOn = !Toggle.isOn;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            OnSnap();
        }

        public void OnPointerExit(PointerEventData eventData) {
            OnSnapLeft();
        }
    }
}
