using FStudio.Events;
using FStudio.MatchEngine.Events;
using FStudio.MatchEngine.Tactics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FStudio.MatchEngine.AIManager {
    public class AIManagerHandler : IManager {
        private const float UPDATE_TIME_OFFSET = 1;

        public readonly GameTeam gameTeam;
        public readonly GameTeam opponent;
        public readonly bool homeOrAway;

        private readonly IManagerBehaviour[] managerBehaviours;

        private float nextChange;

        private readonly HashSet<int> subbed = new HashSet<int>();

        private readonly AnimationCurve subChanceCurve = new AnimationCurve(
            new Keyframe(0, 0),
            new Keyframe (40, 0.1f),
            new Keyframe (50, 0.2f),
            new Keyframe (60, 0.5f),
            new Keyframe (70, 0.8f),
            new Keyframe (80, 3f),
            new Keyframe (90, 5f)
            );

        public AIManagerHandler(bool homeOrAway, GameTeam gameTeam, GameTeam opponent) {
            this.gameTeam = gameTeam;
            this.opponent = opponent;
            this.homeOrAway = homeOrAway;

            managerBehaviours = new IManagerBehaviour[] {
                new ConsiderPossessioning(this),
                new OverallCheck (this),
                new ScoreCheck (this)
            };
        }

        public void Run () {
            var time = Time.time;

            if (nextChange > time) {
                return;
            }

            nextChange = time + UPDATE_TIME_OFFSET;

            float minute = MatchManager.Current.minutes;

            var totalOffensiveness = managerBehaviours.Select(x => x.GetOffensiveness()).Sum();
            var realOffensiveness = totalOffensiveness / managerBehaviours.Length;

            var paramCount = (float)TacticPresetTypes.ParameterCount * realOffensiveness;

            var userTactic = (TacticPresetTypes) Mathf.RoundToInt (paramCount);

            if (gameTeam.Team.TacticPresetType == userTactic) {
                return;
            }

            gameTeam.Team.TacticPresetType = userTactic;

            EventManager.Trigger(new TeamChangedTactic(gameTeam, userTactic));
        }
    }
}
