
using FStudio.MatchEngine.Players;

namespace FStudio.MatchEngine.Events {
    public class PlayerControlBallEvent : AbstractPlayerEvent {
        public readonly float Power;

        public PlayerControlBallEvent(PlayerBase player, float power) : base(player) {
            this.Power = power;
        }
    }
}
