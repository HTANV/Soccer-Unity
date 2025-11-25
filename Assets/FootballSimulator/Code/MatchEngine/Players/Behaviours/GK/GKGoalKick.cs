
namespace FStudio.MatchEngine.Players.Behaviours {
    public class GKGoalKick : GKPassBehaviour {
        public override bool Behave(bool isAlreadyActive) {
            if (!Player.IsGoalKickHolder) {
                return false;
            }

            var target = FindAOpponentToPass();
            if (target != null) {

                var dir = (target.Position - Player.Position).normalized;

                Player.Cross(target.Position + dir * DEGAGE_SPEED_MOD);
            }

            return true;
        }
    }
}
