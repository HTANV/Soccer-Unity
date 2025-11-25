using FStudio.Utilities;
using UnityEngine;

namespace FStudio.Graphics.Cameras {
    [RequireComponent (typeof (Camera))]
    public abstract class AbstractCamera<T> : SceneObjectSingleton<T> where T : Object {
        public Camera Camera;

        protected virtual void OnValidate () {
            this.Camera = GetComponent<Camera>();
        }
    }
}
