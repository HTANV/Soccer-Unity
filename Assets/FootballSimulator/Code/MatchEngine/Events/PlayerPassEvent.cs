using FStudio.Events;
using FStudio.MatchEngine.Players;

namespace FStudio.MatchEngine.Events {
    public class PlayerPassEvent : AbstractPlayerEvent {
        public readonly float Power;

        public PlayerPassEvent (PlayerBase player, float power) : base (player) {
            this.Power = power;
        }
    }
}
