

namespace FStudio.MatchEngine.AIManager {
    internal class OverallCheck : IManagerBehaviour {
        private readonly AIManagerHandler handler;

        public OverallCheck(AIManagerHandler handler) {
            this.handler = handler;
        }

        public float GetOffensiveness () {
            var myTeamOverall = handler.gameTeam.Team.Team.Overall;
            var opponentOverall = handler.opponent.Team.Team.Overall;

            var difference = myTeamOverall - opponentOverall;

            return difference/100f;
        }
    }
}
