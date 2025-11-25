using FStudio.MatchEngine.Players;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FStudio.MatchEngine.Statistics {
    public class Dribbling : AbstractPlayerStatistics, IDisposable {
        private readonly int[] players;
        private readonly Vector3[] lastPositions;
        private readonly int length;

        public readonly Dictionary<int, float> PlayerDribbling;

        public Dribbling(in int[] players) {
            this.players = players;

            this.length = players.Length;
            this.lastPositions = new Vector3[length];

            InitList(ref PlayerDribbling, players);

            // set start positions.
            ResetPositions();
        }

        public void ResetPositions() {
            for (int i = 0; i < length; i++) {
                var player = GetPlayer(players[i]);
                if (player != null) {
                    lastPositions[i] = player.Position;
                }
            }
        }

        public void Update() {
            for (int i = 0; i < length; i++) {
                var player = GetPlayer(players[i]);

                if (player == null) {
                    continue;
                }

                var playerPos = player.Position;
                var distance = (playerPos - lastPositions[i]).magnitude;
                lastPositions[i] = playerPos;

                // gkCheck.
                if (player.IsGK && player.IsGKUntouchable) {
                    continue;
                }

                if (player.IsHoldingBall) {
                    PlayerDribbling[players[i]] += distance;
                }
            }
        }

        public void Dispose() {
        }
    }
}
