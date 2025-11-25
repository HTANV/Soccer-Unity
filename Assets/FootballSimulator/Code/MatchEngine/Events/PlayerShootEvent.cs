using FStudio.Events;
using FStudio.MatchEngine.Players;

namespace FStudio.MatchEngine.Events {
    public class PlayerShootEvent : AbstractPlayerEvent {
        public readonly float Power;

        public PlayerShootEvent(PlayerBase player, float power) : base (player) {
            this.Power = power;
        }
    }
}
