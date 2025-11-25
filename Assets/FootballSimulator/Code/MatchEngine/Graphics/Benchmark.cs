
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FStudio.MatchEngine.Graphics {
    public class Benchmark : MonoBehaviour {
        [SerializeField] private Dropdown benchmarkDropdown;
        [SerializeField] private Dropdown qualityDropdown;

        [SerializeField] private Light[] pointLights;
        [SerializeField] private Light directionalLight;

        [SerializeField] private bool saveScreen;

        private readonly string[] options = new string[] {
            "No point lights",
            "Point lights yes",
            "All ligts on but only dir shadows",
            "All lights & shadows on"
        };

        private readonly string[] qualityOptions = new string[] {
            "No shadows, no aniso, 0.5 scale",
            "Yes aniso, 0.5 scale",
            "Directional shadows yes, aniso, 0.5",
            "Point shadows yes, aniso, 0.5"
        };

        private void Start() {
            benchmarkDropdown.onValueChanged.AddListener(Set);
            qualityDropdown.onValueChanged.AddListener(SetQ);


            qualityDropdown.AddOptions(qualityOptions.ToList()); 
            benchmarkDropdown.AddOptions(options.ToList ());

            benchmarkDropdown.value = 0;
            qualityDropdown.value = 0;
        }

        public void SetQ (int value) {
            QualitySettings.SetQualityLevel(value, true);
        }

        public void Set (int value) {
            switch (value) {
                case 0:
                    foreach (var point in pointLights) {
                        point.enabled = false;
                    }

                    directionalLight.enabled = true;
                    directionalLight.shadows = LightShadows.Soft;
                    break;

                case 1:
                    foreach (var point in pointLights) {
                        point.enabled = true;
                        point.shadows = LightShadows.None;
                    }

                    directionalLight.enabled = true;
                    directionalLight.shadows = LightShadows.None;
                    break;

                case 2:
                    foreach (var point in pointLights) {
                        point.enabled = true;
                        point.shadows = LightShadows.None;
                    }

                    directionalLight.enabled = true;
                    directionalLight.shadows =  LightShadows.Soft;
                    break;

                case 3:
                    foreach (var point in pointLights) {
                        point.enabled = true;
                        point.shadows = LightShadows.Soft;
                    }

                    directionalLight.enabled = true;
                    directionalLight.shadows = LightShadows.Soft;
                    break;
            }
        }

        private void Update() {
            if (saveScreen) {
                saveScreen = false;

                ScreenCapture.CaptureScreenshot(Application.dataPath + "/SCreen.png", 2);
            }
        }
    }
}
