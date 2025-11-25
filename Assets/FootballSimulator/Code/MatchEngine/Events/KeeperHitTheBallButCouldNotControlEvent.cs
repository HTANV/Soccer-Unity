
using FStudio.MatchEngine.Players;

namespace FStudio.MatchEngine.Events {
    /// <summary>
    /// Calls when the ball hits any player and player could not control it.
    /// </summary>
    public class KeeperHitTheBallButCouldNotControlEvent : AbstractPlayerEvent {
        public readonly float Power;

        public KeeperHitTheBallButCouldNotControlEvent(PlayerBase player, float power) : base(player) {
            this.Power = power;
        }
    }
}
