
using FStudio.MatchEngine.Tactics;
using UnityEngine.AI;

namespace FStudio.MatchEngine.AIManager {
    public class BallStatusConsidering : IManagerBehaviour {
        private readonly AIManagerHandler handler;

        public BallStatusConsidering(AIManagerHandler handler) {
            this.handler = handler;
        }

        public float GetOffensiveness () {
            var teamEvent = MatchManager.GetTeamEvent(handler.gameTeam);
            
            switch (teamEvent) {
                case TeamBehaviour.Attacking:
                    return 0.5f;
                case TeamBehaviour.Defending:
                    return 0f;
                case TeamBehaviour.BallChasing:
                    return 0.2f;
                default:
                    return 0.3f;
            }
        }
    }
}
