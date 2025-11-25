
using UnityEngine;
using FStudio.Data;
using FStudio.Scriptables;

namespace FStudio.MatchEngine.Graphics {
    public class SkinColors : SerializedSingletonScriptable<SkinColors> {
        [SerializeField] private Gradient skinColorsGradient = default;

        public Color GetColor(SkinColor skinColor) {
            float total = (float)SkinColor.ParametersCount;
            float point = (int)skinColor / total;

            return skinColorsGradient.Evaluate(point);
        }
    }
}
