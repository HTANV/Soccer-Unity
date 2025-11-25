using UnityEngine;

namespace FStudio.Utilities {
    public class SceneObjectSingleton<T> : MonoBehaviour where T : Object {
        private static T _Current;
        public static T Current {
            get {
                if (_Current == null) {
                    _Current = Object.FindObjectOfType<T>();
                }

                return _Current;
            }
        }

        protected virtual void OnEnable () {
            if (_Current != null) {
                Debug.LogWarning($"{typeof(T)} has multiple instances. It won't break things, but it doesnt sound good tho.");
            }
        }
    }
}
