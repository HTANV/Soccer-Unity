
using FStudio.MatchEngine.Players.Behaviours;

namespace FStudio.MatchEngine.Players.InputBehaviours {
    public class InputBlockRestBehaviour : BaseBehaviour {
        public override bool Behave(bool isAlreadyActive) {
            if (Player.isInputControlled) {
                return true;
            }

            return false;
        }
    }
}
