using UnityEngine;

namespace FStudio.Graphics.TimeOfDay {
    public class TimeOfDayTemplate : ScriptableObject {

        [System.Serializable]
        public class TemplateEntry {
            public Color AmbientColor;

            public Color FogColor;
            public float FogStart;
            public float FogEnd;

            public float LightIntensity;

            public Color LightColor;

            public float ShadowStrength;

            public float SpotLightIntensity;

            public Vector3 LightRotation;

            public float PlayerFakeShadowPower = 1;
        }

        public TemplateEntry[] entries = new TemplateEntry[5]; // by quality.
    }
}
