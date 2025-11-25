using FStudio.Events;
using FStudio.MatchEngine.Players;

namespace FStudio.MatchEngine.Events {
    public abstract class AbstractPlayerEvent : IBaseEvent {
        public readonly PlayerBase Player;

        public AbstractPlayerEvent(PlayerBase player) {
            this.Player = player;
        }
    }
}
