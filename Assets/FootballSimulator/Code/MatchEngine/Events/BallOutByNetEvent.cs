using FStudio.Events;

namespace FStudio.MatchEngine.Events {
    public class BallOutByNetEvent : IBaseEvent {
        public readonly float Power;

        public BallOutByNetEvent(float power) {
            this.Power = power;
        }
    }
}
