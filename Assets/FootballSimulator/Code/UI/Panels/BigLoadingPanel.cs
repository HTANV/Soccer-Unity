
using FStudio.UI.Events;
using System.Threading.Tasks;
using UnityEngine;

namespace FStudio.UI {
    public class BigLoadingPanel : EventPanel<BigLoadingEvent> {
        protected override async void OnEventCalled(BigLoadingEvent eventObject) {
            Debug.Log("Big loading : " + (eventObject != null));
            if (eventObject == null) {
                await Task.Delay(1000);
                Disappear();
            } else {
                Appear();
            }
        }
    }
}