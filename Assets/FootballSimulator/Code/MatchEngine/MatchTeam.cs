using FStudio.Database;
using FStudio.MatchEngine.Tactics;
using FStudio.Data;
using FStudio.MatchEngine.Enums;

namespace FStudio.MatchEngine {
    public class MatchTeam {
        public TeamEntry Team;
        public Formations Formation;
        public MatchPlayer[] Players;
        public TeamTactics TeamTactics;
        public TacticPresetTypes TacticPresetType = TacticPresetTypes.ParameterCount;
        public bool Kit;
        public AILevel AILevel;
    }
}
