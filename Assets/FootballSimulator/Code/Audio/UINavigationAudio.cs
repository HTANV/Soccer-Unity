
using FStudio.Events;
using FStudio.UI.Events;

namespace FStudio.Audio {
    public class UINavigationAudio : AudioMaster<UINavigationAudio> {
        protected override void OnEnable  () {
            base.OnEnable();

            EventManager.Subscribe<UIPointerEnterEvent>(PointerEnter);
            EventManager.Subscribe<UIClickEvent>(Click);
        }

        protected override void OnDisable () {
            base.OnDisable();

            EventManager.UnSubscribe<UIPointerEnterEvent>(PointerEnter);
            EventManager.UnSubscribe<UIClickEvent>(Click);
        }

        private void PointerEnter (UIPointerEnterEvent _) {
            audioManager.Play("POINTER_ENTER");
        }

        private void Click (UIClickEvent _) {
            audioManager.Play("POINTER_ENTER");
        }
    }
}

