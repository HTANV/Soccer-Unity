using FStudio.MatchEngine.Tactics;
using Shared;

namespace FStudio.MatchEngine.AIManager {
    public class ManagerDecision : IDroppable {
        private int mRate;
        public int Rate {
            get => mRate; set => mRate = value;
        }

        public TacticPresetTypes tactic;
    }
}
