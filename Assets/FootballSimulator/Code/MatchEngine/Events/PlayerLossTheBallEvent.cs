using FStudio.Events;
using FStudio.MatchEngine.Players;

namespace FStudio.MatchEngine.Events {
    public class PlayerLossTheBallEvent : AbstractPlayerEvent {
        public PlayerLossTheBallEvent(PlayerBase player) : base(player) { }
    }
}
