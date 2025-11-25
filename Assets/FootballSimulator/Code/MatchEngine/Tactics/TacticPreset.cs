using FStudio.Utilities;

namespace FStudio.MatchEngine.Tactics {
    [System.Serializable]
    public class TacticPreset {
        public SerializableCollection<TeamBehaviour, BehaviourPreset> behaviours;
    }
}