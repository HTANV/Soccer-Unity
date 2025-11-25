
using UnityEngine;

namespace FStudio.Graphics.Cameras {
    [RequireComponent (typeof (Canvas))]
    public class UICanvas : MonoBehaviour {
        private void OnEnable () {
            GetComponent<Canvas>().worldCamera = UICamera.Current.Camera;
        }
    }
}
