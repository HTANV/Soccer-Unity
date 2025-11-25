using FStudio.Utilities;
using System.Threading.Tasks;
using UnityEngine;

using FStudio.Data;

namespace FStudio.Graphics.TimeOfDay {
    public class TimeOfDaySystem : SceneObjectSingleton<TimeOfDaySystem> {
        private const string PLAYER_SHADOW_SHADER_PROPERTY = "_ConstantPower";
        public Light MainLight;

        public Light[] SpotLight;

        [SerializeField] private SerializableAssetCollection<DayTimes, TimeOfDayTemplate> templates;

        private TimeOfDayTemplate currentTemplate;

        public async Task LoadTemplate (DayTimes dayTime) {
            var template = await templates.FindAsync(dayTime);
            ApplyTemplate(template);
        }

        public void RefreshTemplate () {
            if (currentTemplate != null) {
                ApplyTemplate(currentTemplate);
            }
        }

        public void ApplyTemplate (TimeOfDayTemplate targetTemplate) {
            this.currentTemplate = targetTemplate;

            var template = targetTemplate.entries[QualitySettings.GetQualityLevel()];

            RenderSettings.fogColor = template.FogColor;
            RenderSettings.fogStartDistance = template.FogStart;
            RenderSettings.fogEndDistance = template.FogEnd;
            RenderSettings.ambientLight = template.AmbientColor;

            MainLight.intensity = template.LightIntensity;
            MainLight.shadowStrength = template.ShadowStrength;
            MainLight.color = template.LightColor;

            MainLight.transform.rotation = Quaternion.Euler(template.LightRotation);

            foreach (var spot in SpotLight) {
                spot.intensity = template.SpotLightIntensity;
            }

            Shader.SetGlobalFloat(PLAYER_SHADOW_SHADER_PROPERTY, template.PlayerFakeShadowPower);
        }

        public void Clear () {
            templates.Release();
        }
    }
}
