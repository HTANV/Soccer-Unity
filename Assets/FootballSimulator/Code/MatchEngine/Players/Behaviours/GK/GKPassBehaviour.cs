
using System.Linq;

namespace FStudio.MatchEngine.Players.Behaviours {
    public abstract class GKPassBehaviour : BaseBehaviour {
        protected const float DEGAGE_SPEED_MOD = 15f;

        protected PlayerBase FindAOpponentToPass() {
            var possibles = teammates.Where (x=>x != Player);
            if (Player.toGoalXDirection > 0) {
                possibles = possibles.OrderByDescending(x => x.Position.x);
            } else {
                possibles = possibles.OrderBy(x => x.Position.x);
            }
            
            return possibles.
                Take (3).
                OrderBy(x => System.Guid.NewGuid()).
                FirstOrDefault();
        }
    }
}
