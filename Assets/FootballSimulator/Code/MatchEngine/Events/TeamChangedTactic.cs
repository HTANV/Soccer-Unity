

using FStudio.Events;
using FStudio.MatchEngine.Tactics;

namespace FStudio.MatchEngine.Events {
    public class TeamChangedTactic : IBaseEvent {
        public readonly GameTeam Team;
        public readonly TacticPresetTypes TacticPreset;

        public TeamChangedTactic (GameTeam team, TacticPresetTypes tacticPreset) {
            Team = team;
            TacticPreset = tacticPreset;
        }
    }
}
