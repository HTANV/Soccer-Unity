using FStudio.Events;
using FStudio.Loaders;
using FStudio.UI;
using FStudio.UI.Events;
using UnityEngine;

namespace FStudio {
    public class Boot : MonoBehaviour {
        private async void Start() {
            await UILoader.Current.GeneralUILoader.Load();
            await SceneLoader.LoadDefaultScene();

            // show main menu.
            EventManager.Trigger(new MainMenuEvent());
        }
    }
}
