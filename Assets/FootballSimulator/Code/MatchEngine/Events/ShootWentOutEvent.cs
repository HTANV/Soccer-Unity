using FStudio.Events;

namespace FStudio.MatchEngine.Events {
    public class ShootWentOutEvent : IBaseEvent {
        public readonly float Power;

        public ShootWentOutEvent(float power) {
            this.Power = power;
        }
    }
}
