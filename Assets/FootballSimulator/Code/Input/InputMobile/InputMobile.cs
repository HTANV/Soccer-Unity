using FStudio.MatchEngine;
using UnityEngine;

namespace FStudio.Input {
    internal class InputMobile : MonoBehaviour {
        private const string MATCH_ENGINE_ACTION_MAP = "MatchEngine";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        internal static void Load() {
            var inputMobile = Instantiate(Resources.Load<Transform>("InputMobile"));
            DontDestroyOnLoad(inputMobile);
        }

        [SerializeField] private GameObject cancelButton;
        [SerializeField] private GameObject canvas;

        private void Update() {
            var mainCanvasEnabled = false;
            var cancelCanvasEnabled = false;

#if UNITY_ANDROID || UNITY_IOS
            var matchEngineActive = MatchManager.Current != null;
            var hasUserTeam = MatchManager.Current?.UserTeam != null;

            var activeSchema = GameInput.ActiveSchema;
            if (GameInput.ActiveMap == MATCH_ENGINE_ACTION_MAP &&
                 activeSchema != "GamePad" &&
                 activeSchema != "XBoxController" &&
                 matchEngineActive &&
                 hasUserTeam) {
                mainCanvasEnabled = true;
            }

            if (GameInput.ActiveMap == MATCH_ENGINE_ACTION_MAP &&
                 activeSchema != "GamePad" &&
                 activeSchema != "XBoxController" && matchEngineActive) {
                cancelCanvasEnabled = true;
            }
#endif

            canvas.SetActive (mainCanvasEnabled);
            cancelButton.SetActive(cancelCanvasEnabled);
        }
    }
}
