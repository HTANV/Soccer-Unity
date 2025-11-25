using FStudio.Events;
using FStudio.MatchEngine.Events;
using FStudio.UI.MatchThemes.MatchEvents;
using TMPro;
using UnityEngine;

namespace FStudio.UI {
    public class ScoreboardPanel : EventPanel<UpcomingMatchEvent>  {

        [SerializeField] private TextMeshProUGUI homeTeamName;
        [SerializeField] private TextMeshProUGUI awayTeamName;
        [SerializeField] private TextMeshProUGUI timeCounter;

        [SerializeField] private TextMeshProUGUI homeScoreText;
        [SerializeField] private TextMeshProUGUI awayScoreText;

        private int homeScore, awayScore;

        protected override void OnEnable() {
            base.OnEnable();

            EventManager.Subscribe<GameTimeEvent>(GameTimeUpdate);
            EventManager.Subscribe<GoalScoredEvent>(GoalScored);
            EventManager.Subscribe<FirstWhistleEvent>(FirstWhistle);
        }

        private void FirstWhistle(FirstWhistleEvent kickOffEvent) {
            homeScoreText.text = "0";
            awayScoreText.text = "0";
        }

        protected override void OnDisable() {
            base.OnDisable();

            EventManager.UnSubscribe<GameTimeEvent>(GameTimeUpdate);
            EventManager.UnSubscribe<GoalScoredEvent>(GoalScored);
            EventManager.UnSubscribe<FirstWhistleEvent>(FirstWhistle);
        }

        private void GoalScored (GoalScoredEvent goalScoredEvent) {
            if (!goalScoredEvent.Side) {
                homeScoreText.text = (++homeScore).ToString();
            } else {
                awayScoreText.text = (++awayScore).ToString();
            }
        }

        private void GameTimeUpdate(GameTimeEvent gameTimeUpdate) {
            if (timeCounter == null)
                return;

            float time = gameTimeUpdate.GameTime * 60;

            int minutes = (int)time / 60;
            int seconds = (int)time - 60 * minutes;

            var asString = string.Format("{0:00}:{1:00}", minutes, seconds);

            timeCounter.text = asString;
        }

        protected override void OnEventCalled(UpcomingMatchEvent eventObject) {
            timeCounter.text = "";

            if (eventObject == null) {
                return;
            }

            homeTeamName.text = eventObject.details.homeTeam.TeamName.ToUpper();
            awayTeamName.text = eventObject.details.awayTeam.TeamName.ToUpper();

            if (homeScoreText == null || awayScoreText == null)
                return;

            homeScore = 0;
            awayScore = 0;
            homeScoreText.text = "";
            awayScoreText.text = "";
        }
    }
}