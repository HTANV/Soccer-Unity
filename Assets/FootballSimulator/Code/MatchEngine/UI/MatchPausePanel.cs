using FStudio.Events;
using FStudio.MatchEngine.Cameras;
using FStudio.MatchEngine.Events;
using FStudio.UI;
using FStudio.UI.Events;
using TMPro;
using UnityEngine;

namespace FStudio.MatchEngine.UI {
    internal class MatchPausePanel : EventPanel<MatchPauseEvent> {
        private const string SETTING_QUALITY = "SETTING_QUALITY";
        private const string SETTING_CAMERA = "SETTING_CAMERA";

        private const int DEFAULT_QUALIY =
#if UNITY_STANDALONE
2
#else
            1
#endif
;

        private const int DEFAULT_CAMERA = 1;


        [SerializeField] private Selector qualitySelector;
        [SerializeField] private Selector matchCameraSelector;
        [SerializeField] private TextMeshProUGUI qualityText, cameraText;

        private readonly string[] cameraTypes = new string[4] {
            "Broadcast",
            "Stadium",
            "Tele",
            "StadiumHigh"
        };

        private void Awake() {
            var qNames = QualitySettings.names;

            qualitySelector.Max = qNames.Length;
            matchCameraSelector.Max = cameraTypes.Length;

            matchCameraSelector.OnSelectionUpdate += async (val) => {
                var cam = cameraTypes[val];
                await CameraSystem.Current.SwitchCamera(cam);

                cameraText.text = cam;

                PlayerPrefs.SetInt(SETTING_CAMERA, val);
            };

            qualitySelector.OnSelectionUpdate += (val) => {
                QualitySettings.SetQualityLevel(val);

                qualityText.text = QualitySettings.names[val];

                PlayerPrefs.SetInt(SETTING_QUALITY, val);

                Shader.SetGlobalFloat("_SHADER_LAYER_COUNT", 2 + val * 3 + val);
            };

            var qSetting = PlayerPrefs.GetInt(SETTING_QUALITY, DEFAULT_QUALIY);
            var cSetting = PlayerPrefs.GetInt(SETTING_CAMERA, DEFAULT_CAMERA);

            qualitySelector.SetSelected(qSetting);
            matchCameraSelector.SetSelected(cSetting);
        }

        protected override void OnDisappearing() {
            base.OnDisappearing();
            GameInput.SwitchToMatchEngine();
            MatchPause.Pause();
        }

        protected override void OnEventCalled(MatchPauseEvent eventObject) {
            if (eventObject == null) {
                Disappear();
                return;
            }

            GameInput.SwitchToUI();

            Appear();
        }

        public async void LeaveMatch() {
            EventManager.Trigger(new LoadingEvent());

            if (MatchEngineLoader.Current != null) {
                await MatchEngineLoader.Current.UnloadMatch();
            }

            EventManager.Trigger(new CloseAllPanelsEvent());
            EventManager.Trigger(new MainMenuEvent());
        }
    }
}
