using FStudio.Events;

namespace FStudio.MatchEngine.Events {
    public class MatchDetailsEvent : IBaseEvent {
        public readonly MatchManager.MatchDetails matchDetails;

        public MatchDetailsEvent(MatchManager.MatchDetails matchDetails) {
            this.matchDetails = matchDetails;
        }
    }
}
