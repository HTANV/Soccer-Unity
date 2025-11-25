using UnityEngine;

using UnityEditor;


namespace FStudio.Graphics.TimeOfDay {
    [CustomEditor(typeof(TimeOfDayTemplate))]
    [CanEditMultipleObjects]
    public class TimeOfDayTemplateEditor : Editor {
        public override void OnInspectorGUI () {
            base.OnInspectorGUI();

            var script = (TimeOfDayTemplate) target;

            // put a button to compress / decomress.
            if (GUILayout.Button("Copy Scene settings to this template")) {
                var timeOfDay = GetTimeOfDaySystem();

                var targetEntry = script.entries[QualitySettings.GetQualityLevel()];

                targetEntry.AmbientColor = RenderSettings.ambientLight;
                targetEntry.FogColor = RenderSettings.fogColor;
                targetEntry.FogStart = RenderSettings.fogStartDistance;
                targetEntry.FogEnd = RenderSettings.fogEndDistance;
                targetEntry.LightRotation = timeOfDay.MainLight.transform.rotation.eulerAngles;
                targetEntry.LightColor = timeOfDay.MainLight.color;
                targetEntry.LightIntensity = timeOfDay.MainLight.intensity;
                targetEntry.ShadowStrength = timeOfDay.MainLight.shadowStrength;

                EditorUtility.SetDirty(script);
            }

            if (GUILayout.Button ("Apply this template to the scene")) {
                var timeOfDay = GetTimeOfDaySystem();

                timeOfDay.ApplyTemplate(script);
            }
        }
        
        private TimeOfDaySystem GetTimeOfDaySystem () {
            var timeOfDaySystem = Object.FindObjectOfType<TimeOfDaySystem>();
            if (timeOfDaySystem == null) {
                Debug.LogError("There is no time of day system in the scene.");
            }

            return timeOfDaySystem;
        }
    }
}

