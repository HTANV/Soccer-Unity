using FStudio.MatchEngine.Tactics;
using System;
using UnityEngine;

namespace FStudio.MatchEngine.AIManager {
    internal class ConsiderPossessioning : IManagerBehaviour {
        private readonly AIManagerHandler handler;

        public ConsiderPossessioning(AIManagerHandler handler) {
            this.handler = handler;
        }

        public float GetOffensiveness() {
            var statistics = MatchManager.Statistics;
            var teamPos = statistics.possesioning.TeamPositioning[handler.homeOrAway ? 1 : 0];
            return Mathf.Max (teamPos, 20) / 100;
        }
    }
}
