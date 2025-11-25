using FStudio.Events;
using FStudio.MatchEngine.Players;

namespace FStudio.MatchEngine.Events {
    public class PlayerSlideTackleEvent : AbstractPlayerEvent {
        public PlayerSlideTackleEvent(PlayerBase player) : base (player) { }
    }
}
