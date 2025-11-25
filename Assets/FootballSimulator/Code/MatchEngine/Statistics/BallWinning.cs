using FStudio.Events;
using FStudio.MatchEngine.Events;
using FStudio.MatchEngine.Players;
using System;
using System.Collections.Generic;

using UnityEngine;

namespace FStudio.MatchEngine.Statistics {
    public class BallWinning : AbstractPlayerStatistics, IDisposable {
        public readonly int[] Winnings = new int[2];

        public readonly Dictionary<int, int> PlayerBallWinning;

        public BallWinning(in int[] players) {
            EventManager.Subscribe<PlayerWinTheBallEvent>(WinTheBall);

            InitList(ref PlayerBallWinning, players);
        }

        public void Dispose() {
            EventManager.UnSubscribe<PlayerWinTheBallEvent>(WinTheBall);
        }

        private void WinTheBall (PlayerWinTheBallEvent ballWinningEvent) {
            try {
                Winnings[ballWinningEvent.Player.GameTeam.TeamId]++;
                PlayerBallWinning[ballWinningEvent.Player.MatchPlayer.Player.id]++;
            } catch {
            }
        }
    }
}
