using FStudio.MatchEngine.Players;
using System.Collections.Generic;
using System.Linq;

namespace FStudio.MatchEngine.Statistics {
    public abstract class AbstractPlayerStatistics {
        public PlayerBase GetPlayer (int id) {
            return MatchManager.AllPlayers.FirstOrDefault(x => x.MatchPlayer.Player.id == id);
        }

        public void InitList <T> (ref Dictionary <int, T> dic, int[] players) {
            var length = players.Length;

            dic = new Dictionary<int, T>();
            for (int i = 0; i < length; i++) {
                dic.Add(players[i], default);
            }
        }
    }
}
