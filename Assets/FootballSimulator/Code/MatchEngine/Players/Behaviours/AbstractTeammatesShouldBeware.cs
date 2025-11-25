
using System.Linq;
using UnityEngine;

namespace FStudio.MatchEngine.Players.Behaviours {
    public abstract class AbstractTeammatesShouldBeware : BallChasingBehaviour {
        private const int MAX_MARKING = 3;

        public (bool shouldI, PlayerBase target) ShouldIBeware (bool ignoreOffside = false) {
            var goalNetPosition = goalNet.Position;

            var orderedTeammates = teammates.Where(x => !x.IsGK).
                Select(x => (x, x.PlayerFieldProgress));

            var opponentsBehinds = opponents.Where(x => !x.IsGK && (ignoreOffside || !x.IsInOffside)).
            OrderByDescending (x=>x.PlayerFieldProgress);

            int opponentsBehindCount = Mathf.Min (MAX_MARKING, opponentsBehinds.Count());

            var teammatesShouldBeBeware = orderedTeammates.OrderBy(x => x.Item2).Take(opponentsBehindCount).Select(x => x.x).ToList ();

            if (teammatesShouldBeBeware.Contains(Player)) {
                return (true, opponentsBehinds.ElementAt(teammatesShouldBeBeware.FindIndex(x => x == Player)));
            } else {
                return default;
            }
        }
    }
}
