using FStudio.Input;
using UnityEngine.InputSystem;

namespace FStudio.MatchEngine.Input {
    internal class GeneralUserInput : InputListener {
        public GeneralUserInput(string actionMap, int playerIndex, int layer = 0) : base(actionMap, playerIndex, layer) {
            RegisterAction("Pause", PauseInput);
        }

        ~GeneralUserInput() {
            Clear();
        }

        private bool PauseInput(InputAction.CallbackContext ctx) {
            MatchPause.Pause();

            return true;
        }
    }
}
