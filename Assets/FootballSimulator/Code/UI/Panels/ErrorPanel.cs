using TMPro;
using FStudio.UI.Events;

using UnityEngine;

using FStudio.Events;

namespace FStudio.UI {
    public class ErrorPanel : EventPanel<ErrorEvent> {
        [SerializeField] private TextMeshProUGUI errorMessageText;
        [SerializeField] private InteractiveUIElement closeButton;

        private void Awake() {
            closeButton.onClick.AddListener(() => {
                EventManager.Trigger<ErrorEvent>(null); // close.
            });
        }

        protected override void OnEventCalled(ErrorEvent eventObject) {
            if (eventObject == null) {
                Disappear();
            } else {
                Appear();
            }

            if (eventObject == null) {
                return;
            }

            errorMessageText.text = eventObject.Error;
        }
    }
}


