using FStudio.MatchEngine;
using FStudio.UI.Events;
using FStudio.UI.GamepadInput;
using FStudio.UI.Utilities;
using UnityEngine;

namespace FStudio.UI.Panels {
    public class MainMenuPanel : EventPanel<MainMenuEvent> {
        [SerializeField] private TeamSelectionTeam homeTeam, awayTeam;

        protected override void OnEventCalled(MainMenuEvent eventObject) {
            if (eventObject == null) {
                Disappear();
                return;
            }

            Appear();

            SnapManager.Enable();

            GameInput.SwitchToUI();
        }

        public async void Play() {
            var matchRequest = new Shared.Responses.MatchCreateRequest(
                homeTeam.SelectedTeam, 
                awayTeam.SelectedTeam);

            await MatchEngineLoader.CreateMatch(matchRequest);
        }
    }
}
