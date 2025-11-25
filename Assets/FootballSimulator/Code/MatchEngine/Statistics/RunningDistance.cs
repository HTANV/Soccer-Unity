using System;
using System.Collections.Generic;
using UnityEngine;

namespace FStudio.MatchEngine.Statistics {
    public class RunningDistance : AbstractPlayerStatistics, IDisposable {
        private readonly int[] players;
        private readonly Vector3[] lastPositions;
        private readonly int length;

        public readonly float[] TeamDistances;
        public readonly Dictionary<int, float> PlayerDistances;

        public RunningDistance(in int[] players) {
            this.players = players;

            this.length = players.Length;
            this.TeamDistances = new float[length];
            this.lastPositions = new Vector3[length];

            InitList(ref PlayerDistances, players);

            // set start positions.
            ResetPositions();
        }

        public void ResetPositions () {
            for (int i = 0; i < length; i++) {
                var player = GetPlayer(players[i]);
                if (player != null) {
                    lastPositions[i] = player.Position;
                }
            }
        }

        public void Update () {
            for (int i=0; i<length; i++) {
                var player = GetPlayer(players[i]);

                if (player == null) {
                    continue;
                }

                var playerPos = player.Position;
                var distance = (playerPos - lastPositions[i]).magnitude;
                lastPositions[i] = playerPos;

                TeamDistances[player.GameTeam.TeamId] += distance;

                PlayerDistances[player.MatchPlayer.Player.id] += distance;
            }
        }

        public void Dispose() {
        }
    }
}
