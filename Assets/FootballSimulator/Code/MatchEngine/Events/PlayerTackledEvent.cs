using FStudio.Events;
using FStudio.MatchEngine.Players;

namespace FStudio.MatchEngine.Events {
    public class PlayerTackledEvent : AbstractPlayerEvent {
        public PlayerTackledEvent(PlayerBase player) : base(player) { }
    }
}
