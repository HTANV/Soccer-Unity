using FStudio.Data;
using FStudio.Scriptables;
using FStudio.Utilities;

namespace FStudio.MatchEngine.Tactics {
    public class PlayableFormations : SerializedSingletonScriptable<PlayableFormations> {
        public SerializableAssetCollection<Formations, TeamTactics> Formations;
    }
}
