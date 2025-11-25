using FStudio.MatchEngine.Players.PlayerController;
using FStudio.Scriptables;

namespace FStudio.MatchEngine.Players
{
    public class PlayerControllerPrefab : SerializedSingletonScriptable<PlayerControllerPrefab> {
        public CodeBasedController PlayerController;
    }
}