using System.Linq;
using UnityEngine.InputSystem;

namespace FStudio {
    public static class GameInput {
        public static string ActiveMap =>
                PlayerInput.all.First().currentActionMap.name;

        public static string ActiveSchema =>
            PlayerInput.all.First().currentControlScheme;

        public static void SwitchToMatchEngine() {
            // switch to game.
            var allInput = PlayerInput.all;
            foreach (var input in allInput) {
                input.SwitchCurrentActionMap("MatchEngine");
            }
            //
        }

        public static void SwitchToUI() {
            // switch to game.
            var allInput = PlayerInput.all;
            foreach (var input in allInput) {
                input.SwitchCurrentActionMap("UI");
            }
            //
        }
    }
}
