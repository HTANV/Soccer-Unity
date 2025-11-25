using FStudio.Events;
using FStudio.MatchEngine.Events;
using FStudio.UI;
using FStudio.UI.Events;
using FStudio.UI.GamepadInput;
using System;
using TMPro;
using UnityEngine;

namespace FStudio.MatchEngine.UI {
    public class MatchStatistics : EventPanel<MatchStatisticsEvent> {
        [SerializeField] private GameObject quitMatchButton;

        [SerializeField] private MatchStatisticsTeam homeTeam, awayTeam;

        private bool isInitialized = false;

        // general statistics, home and away;
        [SerializeField]
        private TextMeshProUGUI[]
            possessionText,

            passingCount,
            succesfulPassingCountText,
            passingPercentageText,

            attemptCountText,
            succesfulAttemptCountText,

            totalDistanceText,

            cornersCountText,
            ballWinningCountText;
        //

        protected override void OnAppearing() {
            base.OnAppearing();
            Initialize();

            GameInput.SwitchToUI();
        }

        public static string LocalizedDistance(in float distance) {
            const float divider = 100f;

            double final = Math.Round(distance / divider, 2);
            var distParser = "KM";

            if (distance < divider) {
                distParser = "METERS";
                final = Math.Round(distance * 1000 / divider, 0);
            }

            return final.ToString() + " " + distParser;
        }

        private void Initialize() {
            if (isInitialized) {
                return;
            }

            EventManager.Trigger(new LoadingEvent());

            // build general statistics.
            var statistics = MatchManager.Statistics;

            for (int i = 0; i < 2; i++) {
                possessionText[i].text = $"%{statistics.possesioning.TeamPositioning[i]}";

                passingCount[i].text = $"{statistics.passing.Passes[i]}";
                succesfulPassingCountText[i].text = $"{statistics.passing.SuccesfulPasses[i]}";
                passingPercentageText[i].text = $"%{statistics.passing.PassingPercentage[i]}";

                attemptCountText[i].text = $"{statistics.shooting.Attempts[i]}";
                succesfulAttemptCountText[i].text = $"{statistics.shooting.AttemptsOnTarget[i]}";

                totalDistanceText[i].text = $"{LocalizedDistance(statistics.runningDistance.TeamDistances[i])}";

                cornersCountText[i].text = $"{statistics.corners.CornerCount[i]}";
                ballWinningCountText[i].text = $"{statistics.ballWinning.Winnings[i]}";
            }

            EventManager.Trigger<LoadingEvent>(null);

            isInitialized = true;
        }

        protected override void OnEventCalled(MatchStatisticsEvent eventObject) {
            if (eventObject == null) {
                Disappear();
            } else {
                var matchDetails = MatchManager.CurrentMatchDetails;

                homeTeam.SetTeam(matchDetails.homeTeam, matchDetails.homeTeam.Players, matchDetails.homeTeam.Formation, MatchManager.Current.homeTeamScore);
                awayTeam.SetTeam(matchDetails.awayTeam, matchDetails.awayTeam.Players, matchDetails.awayTeam.Formation, MatchManager.Current.awayTeamScore);

                Appear();
            }
        }

        protected override void OnEnable() {
            base.OnEnable();

            EventManager.Subscribe<FinalWhistleEvent>(OnFinalWhistle);
        }

        protected override void OnDisable() {
            base.OnDisable();

            EventManager.UnSubscribe<FinalWhistleEvent>(OnFinalWhistle);
        }

        private void OnFinalWhistle (FinalWhistleEvent _) {
            quitMatchButton.SetActive(true);

            GetComponentInParent<SnapLayer>().UpdateLayer();
        }

        public void GoToMatchRewards () {
            Disappear();
        }

        protected override void OnDisappeared() {
            base.OnDisappeared();

            EventManager.Trigger(new MatchCompleteEvent());
        }

        public async void QuitMatch () {
            if (!MatchEngineLoader.Current) {
                return;
            }

            await MatchEngineLoader.Current.UnloadMatch();

            // go back to main menu.
            EventManager.Trigger<InfoboardEvent>(null);
            EventManager.Trigger<MatchCompleteEvent>(null);
            EventManager.Trigger(new BigLoadingEvent());

            EventManager.Trigger(new MainMenuEvent());

            EventManager.Trigger<BigLoadingEvent>(null);
        }
    }
}
