using FStudio.Events;

namespace FStudio.MatchEngine.Events {
    public class BallHitNetEvent : IBaseEvent {
        public readonly float Power;

        public BallHitNetEvent(float power) {
            this.Power = power;
        }
    }
}
