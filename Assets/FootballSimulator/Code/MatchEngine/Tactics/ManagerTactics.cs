using FStudio.Utilities;
using UnityEngine;

namespace FStudio.MatchEngine.Tactics {
    public class ManagerTactics : ScriptableObject {
        public SerializableCollection<TacticPresetTypes, TacticPreset> Presets;
    }
}
