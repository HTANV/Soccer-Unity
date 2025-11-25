using FStudio.Events;
using FStudio.MatchEngine.Events;
using FStudio.MatchEngine.Players;
using System;
using System.Collections.Generic;

namespace FStudio.MatchEngine.Statistics {
    public class BallLosing : AbstractPlayerStatistics, IDisposable {
        public readonly int[] Losing = new int[2];

        public readonly Dictionary<int, int> PlayerBallLosing;

        public BallLosing(in int[] players) {

            EventManager.Subscribe<PlayerLossTheBallEvent>(LossTheBall);

            InitList(ref PlayerBallLosing, players);
        }

        public void Dispose() {
            EventManager.UnSubscribe<PlayerLossTheBallEvent>(LossTheBall);
        }

        private void LossTheBall(PlayerLossTheBallEvent passEvent) {
            try {
                Losing[passEvent.Player.GameTeam.TeamId]++;
                PlayerBallLosing[passEvent.Player.MatchPlayer.Player.id]++;
            } catch { }
        }
    }
}
