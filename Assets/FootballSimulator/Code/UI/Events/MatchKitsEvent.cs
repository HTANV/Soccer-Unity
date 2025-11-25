using FStudio.Database;
using FStudio.Events;

namespace FStudio.UI.Events {
    public class MatchKitsEvent : IBaseEvent {
        public readonly KitEntry HomeTeamKit;
        public readonly KitEntry AwayTeamKit;

        public MatchKitsEvent (
            KitEntry homeTeamKit,
            KitEntry awayTeamKit) {

            HomeTeamKit = homeTeamKit;
            AwayTeamKit = awayTeamKit;
        }
    }
}
