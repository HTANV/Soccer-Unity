using FStudio.Events;
using FStudio.MatchEngine.Events;
using FStudio.MatchEngine.Players;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FStudio.MatchEngine.Statistics {
    public class Passing : AbstractPlayerStatistics, IDisposable {
        private int passerTeamId = -1;
        private int passerId = -1;

        public readonly int[] SuccesfulPasses = new int[2];
        public readonly int[] Passes = new int[2];
        public readonly int[] PassingPercentage = new int[2];

        public readonly Dictionary<int, int> PlayerPassing;
        public readonly Dictionary<int, int> PlayerSuccesfulPasses;

        public Passing (in int[] players) {
            EventManager.Subscribe<PlayerPassEvent>(PassEvent);
            EventManager.Subscribe<PlayerCrossEvent>(CrossEvent);
            EventManager.Subscribe<PlayerControlBallEvent>(BallHoldEvent);

            InitList(ref PlayerSuccesfulPasses, players);
            InitList(ref PlayerPassing, players);
        }

        private void CrossEvent (PlayerCrossEvent crossEvent) {
            ProcessPass(crossEvent.Player);
        }

        private void PassEvent(PlayerPassEvent passEvent) {
            ProcessPass(passEvent.Player);
        }

        private void ProcessPass (PlayerBase player) {
            passerTeamId = player.GameTeam.TeamId;
            passerId = player.MatchPlayer.Player.id;

            Debug.Log("Process Pass: " + passerId);

            try {
                Passes[passerTeamId]++;
                PlayerPassing[passerId]++;
            } catch {
                Debug.LogError("Pass failed.");

                Debug.LogError("Passer id : " + passerId);

                foreach (var playerId in PlayerPassing) {
                    Debug.LogError(playerId.Key);
                }
            }
        }

        private void BallHoldEvent(PlayerControlBallEvent holdEvent) {
            try {
                if (holdEvent.Player.GameTeam.TeamId == passerTeamId) {
                    SuccesfulPasses[passerTeamId]++;
                    PlayerSuccesfulPasses[passerId]++;
                }

                for (int i = 0; i < 2; i++) {
                    PassingPercentage[i] = Mathf.RoundToInt((float)SuccesfulPasses[i] * 100 / Passes[i]);
                }

                passerTeamId = -1;
                passerId = -1;
            } catch { }
        }

        public void Dispose() {
            EventManager.UnSubscribe<PlayerCrossEvent>(CrossEvent);
            EventManager.UnSubscribe<PlayerPassEvent>(PassEvent);
            EventManager.UnSubscribe<PlayerControlBallEvent>(BallHoldEvent);
        }
    }
}
