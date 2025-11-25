using FStudio.Events;
using FStudio.MatchEngine.Players;

namespace FStudio.MatchEngine.Events {
    public class KeeperSavesTheBallEvent : AbstractPlayerEvent {
        public readonly float Power;

        public KeeperSavesTheBallEvent(PlayerBase player, float power) : base(player) {
            this.Power = power;
        }
    }
}
