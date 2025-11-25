using FStudio.Events;
using FStudio.MatchEngine.Events;
using FStudio.MatchEngine.Players;
using System;
using System.Collections.Generic;

namespace FStudio.MatchEngine.Statistics {
    public class Saves : AbstractPlayerStatistics, IDisposable {
        public readonly int[] BallSaves = new int[2];

        public readonly Dictionary<int, int> PlayerBallSave;

        public Saves (in int[] players) {
            EventManager.Subscribe<KeeperSavesTheBallEvent>(KeeperSaved);
            EventManager.Subscribe<KeeperHitTheBallButCouldNotControlEvent>(KeeperHit);

            InitList(ref PlayerBallSave, players);
        }

        public void Dispose() {
            EventManager.UnSubscribe<KeeperSavesTheBallEvent>(KeeperSaved);
            EventManager.UnSubscribe<KeeperHitTheBallButCouldNotControlEvent>(KeeperHit);
        }

        private void KeeperSaved(KeeperSavesTheBallEvent saveEvent) {
            try {

                BallSaves[saveEvent.Player.GameTeam.TeamId]++;
                PlayerBallSave[saveEvent.Player.MatchPlayer.Player.id]++;
            } catch { }
        }

        private void KeeperHit(KeeperHitTheBallButCouldNotControlEvent saveEvent) {
            try {

                BallSaves[saveEvent.Player.GameTeam.TeamId]++;
                PlayerBallSave[saveEvent.Player.MatchPlayer.Player.id]++;
            } catch {

            }
        }
    }
}
