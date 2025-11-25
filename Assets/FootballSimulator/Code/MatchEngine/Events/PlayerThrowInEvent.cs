using FStudio.Events;
using FStudio.MatchEngine.Players;

namespace FStudio.MatchEngine.Events {
    public class PlayerThrowInEvent : AbstractPlayerEvent {

        public PlayerThrowInEvent(PlayerBase player) : base(player) { }
    }
}
