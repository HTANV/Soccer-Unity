using FStudio.Events;

namespace FStudio.MatchEngine.Events {
    public class BallHitTheWoodWorkEvent : IBaseEvent {
        public readonly float Power;

        public BallHitTheWoodWorkEvent(float power) {
            this.Power = power;
        }
    }
}
