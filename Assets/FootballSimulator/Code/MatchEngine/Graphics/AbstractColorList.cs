using UnityEngine;
using FStudio.Scriptables;

namespace FStudio.MatchEngine.Graphics {
    public class AbstractColorList<T, T0> : SerializedSingletonScriptable<T0> where T : System.Enum where T0 : Object {
        [SerializeField] private Color[] colors = default;

        public Color GetColor(T value) {
            
            return colors[(int)(object)value];
        }
    }
}
