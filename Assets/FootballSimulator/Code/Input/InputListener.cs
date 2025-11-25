using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace FStudio.Input {
    public class InputListener {
        public const int MAX_CONTROLLER = 4;

        public readonly PlayerInput PlayerInput;

        private readonly string actionMap;

        public delegate bool InputEntry (InputAction.CallbackContext ctx);

        private readonly Dictionary<string, InputEntry> myRegistrations = new Dictionary<string, InputEntry>();

        /// <summary>
        /// ActionId, value.
        /// </summary>
        private static readonly Dictionary<string, List<InputEntry>> registeredActions = 
            new Dictionary<string, List<InputEntry>>();

        /// <summary>
        /// Create input listener. Device id can be 0 to MAX_CONTROLLER.
        /// This will get the input from the given 
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="receiver"></param>
        public InputListener(string actionMap, int playerIndex, int layer = 0) {
            Debug.LogFormat("[InputListener] Player: {0}", playerIndex);

            if (playerIndex < 0 || playerIndex > MAX_CONTROLLER) {
                Debug.LogErrorFormat ("[InputListener] Maximum {0} players allowed to join the game.", MAX_CONTROLLER);
            }

            this.actionMap = actionMap;

            if (PlayerInput.all.Count > playerIndex) {
                PlayerInput = PlayerInput.all[playerIndex];
            }
        }

        private static void InputCallback (InputAction.CallbackContext callback) {
            var actionName = callback.action.name;

            var actionList = registeredActions[actionName];

            int registeredCount = actionList.Count;

            for (int i = registeredCount - 1; i >= 0; i--) {
                // call from latest to oldest.
                if (actionList[i].Invoke (callback)) {
                    return;
                }
            }
        }

        public void RegisterAction (string actionId, InputEntry callback) {
            if (PlayerInput == null)
                return;

            var action = PlayerInput.actions.FindActionMap(actionMap).FindAction(actionId);
            if (action != null) {
                if (!registeredActions.ContainsKey (actionId)) {
                    registeredActions.Add(actionId, new List<InputEntry>());
                    action.performed += InputCallback;

                    Debug.Log($"[{actionId}] Listening...");
                }

                registeredActions[actionId].Add(callback);
                myRegistrations.Add(actionId, callback);
            } else {
                Debug.LogWarningFormat("[InputListener] Action Id is not found {0}", actionId);
            }
        }

        /// <summary>
        /// UnRegister all of the actions.
        /// </summary>
        public void Clear () {
            if (PlayerInput == null) {
                return;
            }

            /// unregister from the input system.
            foreach (var kvp in myRegistrations) {
                registeredActions[kvp.Key].Remove (kvp.Value);
            }
        }
    }
}
