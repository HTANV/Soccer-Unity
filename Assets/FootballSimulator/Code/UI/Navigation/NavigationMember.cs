using UnityEngine;

using FStudio.UI.Animators;

using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

namespace FStudio.UI.Navigation {
    public class NavigationMember : InteractiveUIElement {

        [SerializeField] private UIAnimation[] onActiveAnimation;
        [SerializeField] private UIAnimation[] onDeactiveAnimation;

        [SerializeField] private UnityEvent onActive, onDeactive;

        public bool disableSingleForcing;

        public bool IsCurrentlySelected { private set; get; }

        public Action OnNavigationMemberClick;

        public void SetAsActiveMember () {
            if (!disableSingleForcing)
                IsInteractable = false;

            blockAnimations = false;

            StopAll(true);

            IsCurrentlySelected = true;

            onActive?.Invoke();

            foreach (var animation in onActiveAnimation) {
                animation.Play (this);
            }

            foreach (var animation in pointerExitAnimation) {
                animation.Play(this);
            }
        }

        public void NoLongerActive () {
            onDeactive?.Invoke();

            StopAll(true);

            if (!gameObject.activeInHierarchy) {
                return;
            }


            IsInteractable = true;

            IsCurrentlySelected = false;
            foreach (var animation in onDeactiveAnimation) {
                animation.Play(this);
            }
        }

        public override void OnPointerClick(PointerEventData eventData) {
            base.OnPointerClick(eventData);

            if (!IsInteractable) {
                return;
            }

            OnNavigationMemberClick?.Invoke();
        }
    }
}
