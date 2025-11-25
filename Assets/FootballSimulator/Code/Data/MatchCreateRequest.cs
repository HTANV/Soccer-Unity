using FStudio.Database;
using FStudio.Data;
using FStudio.MatchEngine.Enums;

namespace Shared.Responses {
    public struct MatchCreateRequest {
        public enum UserTeam {
            None,
            Home,
            Away
        }

        public readonly TeamEntry homeTeam;
        public readonly TeamEntry awayTeam;

        public AILevel aiLevel;

        public UserTeam userTeam;

        public DayTimes dayTime;

        public TeamEntry GetTeam(bool homeOrAway) {
            return !homeOrAway ? homeTeam : awayTeam;
        }

        public MatchCreateRequest (TeamEntry homeTeam, TeamEntry awayTeam) {
            this.homeTeam = (TeamEntry) homeTeam.Clone ();
            this.awayTeam = (TeamEntry) awayTeam.Clone ();

            for (int i = 0; i < 11; i++) {
                this.homeTeam.Players[i].id = i;
            }

            for (int i = 0; i < 11; i++) {
                this.awayTeam.Players[i].id = i + 11;
            }

            aiLevel = AILevel.Legendary;
            userTeam = UserTeam.None;
            dayTime = DayTimes.Night;
        }
    }
}