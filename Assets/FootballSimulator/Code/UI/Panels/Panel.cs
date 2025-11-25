using UnityEngine;

using FStudio.UI.Animators;

using UnityEngine.Events;
using FStudio.Events;
using FStudio.UI.Events;
using System.Collections.Generic;
using FStudio.Audio;
using FStudio.UI.GamepadInput;

namespace FStudio.UI {
    public class Panel : MonoBehaviour {
        [SerializeField] protected CanvasGroup canvasGroup;

        [SerializeField] private UIAnimation[] appearAnimation;
        [SerializeField] private UIAnimation[] disappearAnimation;

        [SerializeField] private bool dontDisableWhenDisappear;

        [SerializeField] private string appearSoundId, disappearSoundId;

        public IEnumerable<UIAnimation> AppearAnimations => appearAnimation;
        public IEnumerable<UIAnimation> DisappearAnimations => disappearAnimation;

        public UnityEvent AppearedEvent;
        public UnityEvent DisappearEvent;
        public UnityEvent OnAppearingEvent;
        public UnityEvent OnDisappearingEvent;

        public bool IsActive { get; protected set; }

        protected virtual void OnEnable() {
            EventManager.Subscribe<CloseAllPanelsEvent>(CloseAll);
        }

        protected virtual void OnDisable() {
            EventManager.UnSubscribe<CloseAllPanelsEvent>(CloseAll);
        }

        private void CloseAll(CloseAllPanelsEvent eventObject) {
            Disappear();
        }

        protected virtual void OnAppearing() {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;

            OnAppearingEvent?.Invoke();

            if (!string.IsNullOrEmpty(appearSoundId)) {
                UINavigationAudio.Current?.audioManager?.Play(appearSoundId);
            }
        }

        protected virtual void OnDisappearing () {
            OnDisappearingEvent?.Invoke();

            if (!string.IsNullOrEmpty(disappearSoundId)) {
                UINavigationAudio.Current.audioManager.Play(disappearSoundId);
            }
        }
        
        protected virtual void OnAppeared() {
            AppearedEvent?.Invoke();
        }

        protected virtual void Deactivated () {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        protected virtual void OnDisappeared() {
            if (!dontDisableWhenDisappear) 
            canvasGroup.gameObject.SetActive(false);

            DisappearEvent?.Invoke();
        }

        public void Appear () {
            if (IsActive) {
                return;
            }

            IsActive = true;

            if (!canvasGroup.gameObject.activeSelf) {
                canvasGroup.gameObject.SetActive(true);
            }

            if (!isActiveAndEnabled) {
                return;
            }

            foreach (var animation in disappearAnimation) {
                animation.Stop();
            }

            OnAppearing();

            if (appearAnimation.Length == 0) {
                OnAppeared();
            } else {
                int appearing = 0;
                foreach (var animation in appearAnimation) {
                    appearing++;
                    animation.Play(this, () => {
                        if (--appearing == 0) {
                            OnAppeared();
                        }
                    });
                }
            }
        }

        public void Disappear () {
            try {
                if (!IsActive) {
                    return;
                }

                if (!isActiveAndEnabled) {
                    return;
                }
            } catch {
                return;
            }

            IsActive = false;

            Deactivated();

            if (!canvasGroup.gameObject.activeInHierarchy) {
                return;
            }

            foreach (var animation in appearAnimation) {
                animation.Stop();
            }

            OnDisappearing();

            if (disappearAnimation.Length == 0) {
                OnDisappeared();
            } else {
                int appearing = 0;
                foreach (var animation in disappearAnimation) {
                    appearing++;
                    animation.Play(this, () => {
                        if (--appearing == 0) {
                            OnDisappeared();
                        }
                    });
                }
            }
        }


        [SerializeField] private bool PreviewAppear, PreviewDisappear;

        protected virtual void AppearPreview() {
            Appear();
        }

        protected virtual void DisappearPreview() {
            Disappear();
        }

        protected virtual void Update() {
            if (PreviewAppear) {
                PreviewAppear = false;

                AppearPreview();
            }

            if (PreviewDisappear) {
                PreviewDisappear = false;

                DisappearPreview();
            }
        }
    }
}
