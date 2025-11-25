using FStudio.Events;
using FStudio.UI.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FStudio.UI {
    public abstract class AbstractInteractiveUI : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler {

        public virtual void OnPointerClick(PointerEventData eventData) {
            EventManager.Trigger(new UIClickEvent ());
        }

        public virtual void OnPointerEnter(PointerEventData eventData) {
            EventManager.Trigger(new UIPointerEnterEvent ());
        }

        public virtual void OnPointerExit(PointerEventData eventData) {
            EventManager.Trigger(new UIPointerExitEvent ());
        }
    }
}
