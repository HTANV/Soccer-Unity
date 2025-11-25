
using FStudio.MatchEngine.Players;

namespace FStudio.MatchEngine.Events {
    public class PlayerChipShootEvent : AbstractPlayerEvent {
        public PlayerChipShootEvent(PlayerBase player) : base(player) { }
    }
}
