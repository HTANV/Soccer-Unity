using FStudio.Events;
using FStudio.MatchEngine.Players;

namespace FStudio.MatchEngine.Events {
    public class PlayerWinTheBallEvent : AbstractPlayerEvent {
        public PlayerWinTheBallEvent(PlayerBase player) : base(player) { }
    }
}
