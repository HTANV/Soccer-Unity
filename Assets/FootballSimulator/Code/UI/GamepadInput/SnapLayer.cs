
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FStudio.UI.GamepadInput {
    [RequireComponent(typeof(Panel))]
    public class SnapLayer : MonoBehaviour {
        [SerializeField] [HideInInspector] public Panel targetPanel;

        [Header ("Higher values will block the lesser values.")]
        [SerializeField] private byte layer;

        public byte Layer => layer;

        [Header ("Log everything registered/unregistered")]
        [SerializeField] private bool debugger;

        [SerializeField] private GameObject[] snapAtAppears;

        [Tooltip ("Choose this if you wat to use this layer manually. You may need to call RegisterLayer and UnregisterLayer")]
        [SerializeField] private bool disableAutoRegistering;

        private bool updateSnapWhenUnregister = true;

        private void OnEnable () {
            if (disableAutoRegistering) {
                return;
            }

            targetPanel.OnAppearingEvent.AddListener ( RegisterLayer );
            targetPanel.OnDisappearingEvent.AddListener ( UnRegisterLayer );
        }

        private void OnDisable() {
            if (disableAutoRegistering) {
                return;
            }

            targetPanel.OnAppearingEvent.RemoveListener (RegisterLayer);
            targetPanel.OnDisappearingEvent.RemoveListener (UnRegisterLayer);
        }

        private void OnValidate() {
            targetPanel = GetComponent<Panel>();
        }

        private IEnumerable<ISnappable> mySnappables () {
            var snappables = GetComponentsInChildren<ISnappable>(true).AsEnumerable ();

            // if you build something like that;
            // SnapLayer
            // --- Snappable
            // --- Snappable
            // --> SnapLayer
            //     --- Snapable. // THIS CODE WILL IGNORE IT BECAUSE ANOTHER SNAP LAYER HOLDS IT.
            // >>
            snappables = snappables.Where(x => x.gObject.GetComponentInParent<SnapLayer>(true) == this);

            return snappables;
        }

        public void RegisterLayer () {
            Debug.Log($"[SnapLayer] RegisterLayer on {layer} => {this}", this);

            var snappables = mySnappables();

            if (debugger) {
                foreach (var snappable in snappables) {
                    Debug.Log(snappable, snappable.gObject);
                }
            }

            SnapManager.Register(snappables, layer, snapAtAppears.Select (x=>x.GetComponent<ISnappable>()));
        }

        public async void UnRegisterLayer () {
            Debug.Log($"[SnapLayer] UnRegisterLayer {this}", this);

            var snappables = mySnappables();

            if (debugger) {
                foreach (var snappable in snappables) {
                    Debug.Log(snappable, snappable.gObject);
                }
            }

            SnapManager.UnRegister(snappables, layer, updateSnapWhenUnregister);

            if (updateSnapWhenUnregister) {
                if (SnapManager.Current.IsEnabled) {
                    await SnapManager.AutoSnap(null);
                }
            }
        }

        public void SetSnapTarget (GameObject snapTarget) {
            snapAtAppears = new GameObject[1] { snapTarget };
        }

        public void UpdateLayer () {
            updateSnapWhenUnregister = false;
            UnRegisterLayer();
            updateSnapWhenUnregister = true;
            RegisterLayer();
        }
    }
}
