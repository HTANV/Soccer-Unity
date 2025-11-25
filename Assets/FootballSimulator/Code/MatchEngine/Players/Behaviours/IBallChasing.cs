

using FStudio.MatchEngine.Players;

namespace FStudio.Players.Behaviours {
    internal interface IBallChasing {
        public float ChasingDistance { get; }

        public PlayerBase ActivePlayer { get; }
    }
}
