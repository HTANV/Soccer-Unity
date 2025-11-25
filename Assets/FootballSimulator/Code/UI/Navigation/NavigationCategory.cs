using UnityEngine;

using FStudio.UI;

namespace FStudio.UI.Navigation {
    public class NavigationCategory : MonoBehaviour {
        [SerializeField] private NavigationManager navigationManager;
        [SerializeField] private Panel[] panels;

        private void OnEnable () {
            navigationManager.OnNavigationUpdate += NavUpdate;
        }

        private void OnDisable() {
            navigationManager.OnNavigationUpdate -= NavUpdate;
        }

        private void NavUpdate (int index) {
            Debug.Log($"Catergory Update {index}");

            for (int i=0, length = panels.Length; i<length; i++) {
                if (i != index) {
                    if (panels[i] != null) panels[i].Disappear();
                }
            }

            if (index >= 0 && panels.Length > index) {
                if (panels[index] != null) panels[index].Appear();
            }
        }
    }
}
