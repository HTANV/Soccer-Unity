using FStudio.Events;
using FStudio.MatchEngine.Players;

namespace FStudio.MatchEngine.Events {
    public class PlayerCrossEvent : AbstractPlayerEvent {
        public readonly float Power;

        public PlayerCrossEvent(PlayerBase player, float power) : base(player) {
            this.Power = power;
        }
    }
}
