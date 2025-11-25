
using FStudio.Events;
using UnityEngine;
using FStudio.UI.Events;

using UnityEngine.UI;
using FStudio.UI.Graphics;
using FStudio.UI.MatchThemes.MatchEvents;
using TMPro;
using FStudio.MatchEngine.Events;

namespace FStudio.UI.MatchThemes {
    public class LiveScoreboardPanel : ScoreboardPanel {
        [SerializeField] private Image homeTeamColors, awayTeamColors;

        protected override void OnEnable() {
            base.OnEnable();

            EventManager.Subscribe<ShowScoreboardEvent>(ShowScoreboard);
            EventManager.Subscribe<MatchKitsEvent>(MatchKitsUpdate);
        }

        protected override void OnDisable() {
            base.OnDisable();

            EventManager.UnSubscribe<ShowScoreboardEvent>(ShowScoreboard);
            EventManager.UnSubscribe<MatchKitsEvent>(MatchKitsUpdate);
        }

        private void ShowScoreboard (ShowScoreboardEvent eventObject) {
            if (eventObject == null) {
                Disappear();
            } else {
                Appear();
            }
        }

        private async void MatchKitsUpdate (MatchKitsEvent matchKits) {
            Debug.Log("Matchkitsupdate");

            homeTeamColors.material = await TeamLogoMaterial.Current.GetScoreboardMaterial(
                matchKits.HomeTeamKit.Color1,
                 matchKits.HomeTeamKit.Color2);

            awayTeamColors.material = await TeamLogoMaterial.Current.GetScoreboardMaterial(
                matchKits.AwayTeamKit.Color1,
                matchKits.AwayTeamKit.Color2);
        }
    }
}
