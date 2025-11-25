using FStudio.Events;
using FStudio.MatchEngine.Players;

namespace FStudio.MatchEngine.Events {
    public class PlayerDisbalancedEvent : AbstractPlayerEvent {

        public PlayerDisbalancedEvent(PlayerBase player) : base(player) { }
    }
}
