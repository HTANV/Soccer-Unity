using FStudio.Database;
using FStudio.Events;

namespace FStudio.MatchEngine.Events {
    public struct GoalScoredEvent : IBaseEvent {
        public readonly bool Side;
        public readonly PlayerEntry Scorer;
        public readonly int Minute;

        public GoalScoredEvent(bool side, PlayerEntry playerResponse, int minute) {
            this.Side = side;
            this.Scorer = playerResponse;
            this.Minute = minute;
        }
    }
}
