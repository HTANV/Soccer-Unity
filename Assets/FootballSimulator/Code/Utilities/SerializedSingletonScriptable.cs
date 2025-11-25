using UnityEngine;

namespace FStudio.Scriptables {
    public class SerializedSingletonScriptable<T> : ScriptableObject where T : Object {
        private static string ObjectName {
            get {
                return typeof(T).Name;
            }
        }

        private static T _Current;
        public static T Current {
            get {
                if (_Current == null) {
                    var path = $"Singletons/{ObjectName}";

                    _Current = Resources.Load<T>(path);
                }

                return _Current;
            }
        }
    }
}
