using FStudio.Events;
using FStudio.MatchEngine.Events;
using FStudio.UI;
using UnityEngine;
using Shared.Responses;

namespace FStudio.MatchEngine.UI {
    public class TeamTacticsUI : EventPanel<MatchInitializationCompletedEvent> {

        [SerializeField] private TacticPresenter homeTacticPresenter, awayTacticPresenter;

        protected override void OnEnable() {
            base.OnEnable();
            EventManager.Subscribe<FinalWhistleEvent>(FinalWhistle);
            EventManager.Subscribe<TeamChangedTactic>(TeamChangedTactics);
        }

        protected override void OnDisable() {
            base.OnDisable();
            EventManager.UnSubscribe<FinalWhistleEvent>(FinalWhistle);
            EventManager.UnSubscribe<TeamChangedTactic>(TeamChangedTactics);
        }

        private void TeamChangedTactics(TeamChangedTactic teamChangedTactic) {
            if (MatchManager.Current.GameTeam1 == teamChangedTactic.Team) {
                homeTacticPresenter.Set(teamChangedTactic.TacticPreset);
            } else {
                awayTacticPresenter.Set(teamChangedTactic.TacticPreset);
            }
        }

        private void FinalWhistle (FinalWhistleEvent finalWhistle) {
            Disappear();
        }

        protected override void OnEventCalled(MatchInitializationCompletedEvent eventObject) {
            Appear();
        }
    }
}
