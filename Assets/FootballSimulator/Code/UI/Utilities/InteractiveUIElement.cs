using UnityEngine;
using UnityEngine.UI;
using FStudio.UI.Animators;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using FStudio.UI.GamepadInput;
using System.Linq;

namespace FStudio.UI {
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(RectTransform))]
    public class InteractiveUIElement : AbstractInteractiveUI, ISnappable {

        [SerializeField] [HideInInspector] private CanvasGroup canvasGroup;

        [SerializeField] protected RectTransform rectTransform;

        public UIAnimation[] appearAnimation;
        public UIAnimation[] disappearAnimation;
        public UIAnimation[] pointerHoverAnimation;
        public UIAnimation[] pointerExitAnimation;
        public UIAnimation[] pointerClickAnimation;

        public bool blockHorizontalNavigation => false;

        public Vector3 position => transform.position;

        public GameObject gObject => this != null ? gameObject : null;

        [SerializeField] private bool dontWaitAnimationToEnableInteraction;

        public bool isSnappable => this !=null && enabled && gameObject.activeInHierarchy && IsInteractable;

        [SerializeField] private bool keepGameObjectActiveWhenDisappeared;

        [Header("Current interactable status.")]
        [SerializeField] private bool _isInteractable = true;

        public bool IsInteractable {
            get => isInteractionEverEnabled && _isInteractable;
            set {
                _isInteractable = value;
            }
        }

        [Header("Is interaction is possible on this ever.")]
        public bool isInteractionEverEnabled = true;

        [Tooltip("This will be called when user clicks on element.")]
        public UnityEvent onClick = new UnityEvent();

        [Tooltip("This will be called after pointerClickAnimations are completed.")]
        public UnityEvent onLateClick = new UnityEvent();

        public UnityEvent onHover = new UnityEvent();

        public UnityEvent onUnHover = new UnityEvent();

        public UnityEvent onAppeared = new UnityEvent();

        protected bool blockAnimations;

        private bool isHovered;

        private void OnValidate() {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable() {
            if (blockAnimations) {
                // Restore.

                blockAnimations = false;

                foreach (var animation in pointerExitAnimation) {
                    animation.Play(this);
                }
            }
        }

        private void OnDisable() {
            StopAll();
        }

        public virtual void Appear () {
            try {
                if (gameObject == null) {
                    return;
                }
            } catch {
                return;
            }

            gameObject.SetActive(true);

            blockAnimations = false;

            if (!this.isActiveAndEnabled) {
                // direct show.
                canvasGroup.alpha = 1;
                IsInteractable = true;
                onAppeared?.Invoke();
                return;
            }

            if (!isActiveAndEnabled) {
                return;
            }
 
            StopAll();

            if (dontWaitAnimationToEnableInteraction || appearAnimation.Length == 0) {
                IsInteractable = true;
            }

            int value = 0;
            foreach (var animation in appearAnimation) {
                value++;
                animation.Play(this, () => { 
                    if (--value == 0) {
                        IsInteractable = true;

                        onAppeared?.Invoke();
                    }
                });
            }
        }

        public virtual void Disappear () {
            if (!this.isActiveAndEnabled) {
                // direct hide.
                canvasGroup.alpha = 0;
                gameObject.SetActive(false);
                
                return;
            }

            IsInteractable = false;

            StopAll();

            foreach (var animation in pointerExitAnimation) {
                animation.Play(this);
            }

            int value = 0;
            foreach (var animation in disappearAnimation) {
                value++;
                animation.Play(this, () => {
                    if (--value == 0) {
                        if (!keepGameObjectActiveWhenDisappeared) {
                            gameObject.SetActive(false);
                        }
                    }
                });
            }
        }

        protected virtual void StopAll (bool ignoreAppearAnimations = false) {
            if (blockAnimations) {
                Debug.Log("kills.");
            }

            if (!ignoreAppearAnimations) {
                foreach (var animation in appearAnimation) {
                    animation.Stop();
                }

                foreach (var animation in disappearAnimation) {
                    animation.Stop();
                }
            }

            foreach (var animation in pointerHoverAnimation) {
                animation.Stop();
            }

            foreach (var animation in pointerExitAnimation) {
                animation.Stop();
            }

            foreach (var animation in pointerClickAnimation) {
                animation.Stop();
            }
        }

        public void SetRaycast (bool value) {
            canvasGroup.blocksRaycasts = value;
            canvasGroup.interactable = value;
        }

        public override void OnPointerClick(PointerEventData eventData) {
            if (!IsInteractable) {
                return;
            }

            if (blockAnimations) {
                return;
            }

            base.OnPointerClick(eventData);

            StopAll();

            onClick?.Invoke();

            if (pointerClickAnimation.Length > 0) {
                blockAnimations = true;
                int task = 0;
                foreach (var animation in pointerClickAnimation) {
                    task++;

                    animation.Play(this, () => {

                        if (--task == 0) {

                            blockAnimations = false;
                            onLateClick?.Invoke();
                            
                            if (isHovered) {
                                PointerEnter ();
                            } else {
                                PointerExit();
                            }
                        }
                    });
                }
            } else {
                onLateClick.Invoke();
            }
        }

        public override void OnPointerEnter(PointerEventData eventData) {
            if (PointerEnter()) {
                base.OnPointerEnter(eventData);
            }
        }

        private bool PointerEnter () {
            isHovered = true;

            if (!IsInteractable || blockAnimations) {
                return false;
            }

            StopAll(true);

            foreach (var animation in pointerHoverAnimation) {
                animation.Play(this);
            }

            onHover?.Invoke();
            return true;
        }

        public override void OnPointerExit(PointerEventData eventData) {
            if (PointerExit()) {
                base.OnPointerExit(eventData);
            }
        }

        private bool PointerExit () {
            isHovered = false;

            if (!IsInteractable || blockAnimations) {
                return false;
            }

            blockAnimations = false;

            StopAll(true);

            foreach (var animation in pointerExitAnimation) {
                animation.Play(this);
            }

            onUnHover?.Invoke();

            return true;
        }

        public ScrollRect OnSnap() {
            if (isSnappable) {
                OnPointerEnter(new PointerEventData(EventSystem.current));
            }

            var scrollView = GetComponentsInParent<ScrollRect>(true).FirstOrDefault();
            return scrollView;
        }

        public void OnSnapLeft() {
            if (!isSnappable) {
                return;
            }

            OnPointerExit(new PointerEventData(EventSystem.current));
        }

        public void OnClick() {
            if (!isSnappable) {
                return;
            }

            OnPointerClick(new PointerEventData(EventSystem.current));
        }
    }
}
