using TMPro;
using FStudio.UI.Events;
using UnityEngine;
using FStudio.UI.GamepadInput;

namespace FStudio.UI {
    public class LoadingPanel : EventPanel<LoadingEvent> {
        [SerializeField] private TextMeshProUGUI loadingText;

        [SerializeField] private GameObject infoPanel;

        protected override void OnDisappeared () {
            base.OnDisappeared();

            SnapManager.Enable();
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            SnapManager.Disable();
        }

        protected override void OnEventCalled(LoadingEvent eventObject) {
            if (eventObject == null) {
                Debug.Log("closing loading");
                Disappear();
                return;
            } else {
                Debug.Log("opening loading");
                Appear();
            }

            var hasInfo = !string.IsNullOrEmpty(eventObject.Message);

            if (hasInfo) {
                infoPanel.SetActive(true);

                loadingText.text = eventObject.Message;
            } else {
                infoPanel.SetActive(false);
            }
        }
    }
}


