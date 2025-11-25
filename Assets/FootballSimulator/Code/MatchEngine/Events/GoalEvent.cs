using FStudio.Events;

namespace FStudio.MatchEngine.Events {
    public class GoalEvent : IBaseEvent {
        public readonly bool HomeOrAway;

        public GoalEvent (bool homeOrAway) {
            this.HomeOrAway = homeOrAway;
        }
    }
}
