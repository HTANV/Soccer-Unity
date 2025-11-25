using UnityEngine;

using FStudio.Events;

namespace FStudio.UI {
    [RequireComponent (typeof (Canvas))]
    public abstract class EventPanel<T> : Panel where T : IBaseEvent {
        protected override void OnEnable() {
            base.OnEnable();
            
            EventManager.Subscribe<T>(OnEventCalled);
        }

        protected override void OnDisable() {
            base.OnDisable();

            EventManager.UnSubscribe<T>(OnEventCalled);
        }

        protected abstract void OnEventCalled(T eventObject);
    }
}
