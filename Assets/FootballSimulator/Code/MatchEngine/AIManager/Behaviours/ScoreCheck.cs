using FStudio.MatchEngine.Tactics;
using UnityEngine;

namespace FStudio.MatchEngine.AIManager {
    internal class ScoreCheck : IManagerBehaviour {
        private readonly AIManagerHandler handler;

        public ScoreCheck (AIManagerHandler handler) {
            this.handler = handler;
        }

        public float GetOffensiveness () {
            var homeScore = MatchManager.Current.homeTeamScore;
            var awayScore = MatchManager.Current.awayTeamScore;

            var diff = homeScore - awayScore;

            var max = Mathf.Max(Mathf.Abs(diff), 9);
            
            if (!handler.homeOrAway) {
                return 1 - (diff / max);
            } else {
                return 1 - (-diff / max);
            }
        }
    }
}
