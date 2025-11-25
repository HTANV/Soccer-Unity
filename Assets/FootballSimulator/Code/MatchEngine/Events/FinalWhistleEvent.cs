using FStudio.Database;
using FStudio.Events;
using Shared.Responses;

namespace FStudio.MatchEngine.Events {
    public class FinalWhistleEvent : IBaseEvent {
        public readonly TeamEntry HomeTeam, AwayTeam;

        public FinalWhistleEvent (TeamEntry homeTeam, TeamEntry awayTeam) {
            this.HomeTeam = homeTeam;
            this.AwayTeam = awayTeam;
        }
    }
}
