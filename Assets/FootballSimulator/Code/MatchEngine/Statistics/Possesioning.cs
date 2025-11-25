using FStudio.MatchEngine.Balls;
using System;
using UnityEngine;

namespace FStudio.MatchEngine.Statistics {
    public class Possesioning : IDisposable {
        private readonly float[] teams = new float[2];
        private readonly Ball ball;

        public readonly int[] TeamPositioning = new int[2];

        public Possesioning(Ball ball) {
            this.ball = ball;
        }

        public void Update() {
            var lastHolder = ball.LastHolder;
            var holder = ball.HolderPlayer;

            var tPlayer = holder == null ? lastHolder : holder;

            try {
                if (tPlayer == null || tPlayer.GameTeam == null) {
                    return;
                }
            } catch {
                return;
            }

            var teamId = tPlayer.GameTeam.TeamId;

            teams[teamId] += Time.deltaTime;

            var total = teams[0] + teams[1];

            for (int i=0; i<2; i++) {
                TeamPositioning[i] = Mathf.RoundToInt ((teams[i] / total) * 100);
            }
        }

        public void Dispose() {
            
        }
    }
}
