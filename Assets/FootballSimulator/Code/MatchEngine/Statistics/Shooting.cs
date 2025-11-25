using FStudio.Events;
using FStudio.MatchEngine.Events;
using FStudio.MatchEngine.Players;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FStudio.MatchEngine.Statistics {
    public class Shooting : AbstractPlayerStatistics, IDisposable {
        private int shooterTeamId = -1;
        private int shooterId = -1;

        public readonly int[] AttemptsOnTarget = new int[2];
        public readonly int[] Attempts = new int[2];

        public readonly Dictionary<int, int> PlayerAttempt;
        public readonly Dictionary<int, int> PlayerAttemptOnTarget;
        public readonly Dictionary<int, int> PlayerGoal;

        public Shooting(in int[] players) {
            EventManager.Subscribe<PlayerShootEvent>(ShootEvent);
            EventManager.Subscribe<KeeperHitTheBallButCouldNotControlEvent>(KeeperHit);
            EventManager.Subscribe<KeeperSavesTheBallEvent>(KeeperSaves);
            EventManager.Subscribe<GoalEvent>(Goal);


            InitList(ref PlayerAttempt, players);

            InitList(ref PlayerAttemptOnTarget, players);

            InitList(ref PlayerGoal, players);
        }

        private void ShootEvent (PlayerShootEvent eventObject) {
            try {
                shooterTeamId = eventObject.Player.GameTeam.TeamId;
                shooterId = eventObject.Player.MatchPlayer.Player.id;

                Attempts[shooterTeamId]++;
                PlayerAttempt[shooterId]++;
            } catch { }
        }

        private void KeeperHit (KeeperHitTheBallButCouldNotControlEvent eventObject) {
            ProcessNextStep(eventObject.Player);
        }

        private void KeeperSaves(KeeperSavesTheBallEvent eventObject) {
            ProcessNextStep(eventObject.Player);
        }

        private void Goal(GoalEvent eventObject) {
            try {
                var scorerTeamId = eventObject.HomeOrAway ? 1 : 0;

                var scorer = MatchManager.Current.GoalScorer(scorerTeamId);

                PlayerGoal[scorer.MatchPlayer.Player.id]++;

                if (shooterTeamId == scorerTeamId && shooterTeamId != -1) {
                    AttemptsOnTarget[shooterTeamId]++;
                    PlayerAttemptOnTarget[shooterId]++;

                    shooterTeamId = -1;
                    shooterId = -1;
                }
            }
            catch { }
        }

        private void ProcessNextStep (PlayerBase player) {
            if (player.GameTeam.TeamId != shooterTeamId && shooterTeamId != -1) {
                AttemptsOnTarget[shooterTeamId]++;
                PlayerAttemptOnTarget[shooterId]++;
                shooterTeamId = -1;
                shooterId = -1;
            }
        }

        public void Dispose() {
            EventManager.UnSubscribe<PlayerShootEvent>(ShootEvent);
            EventManager.UnSubscribe<KeeperHitTheBallButCouldNotControlEvent>(KeeperHit);
            EventManager.UnSubscribe<KeeperSavesTheBallEvent>(KeeperSaves);
            EventManager.UnSubscribe<GoalEvent>(Goal);
        }
    }
}
