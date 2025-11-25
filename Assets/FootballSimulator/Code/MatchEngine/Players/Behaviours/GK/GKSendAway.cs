
namespace FStudio.MatchEngine.Players.Behaviours {
    public class GKSendAway : GKPassBehaviour {
        private PlayerBase target;

        public override bool Behave(bool isAlreadyActive) {
            if (Player.IsGoalKickHolder || Player.IsGKUntouchable || !Player.IsHoldingBall) {
                return false;
            }

            if (!isAlreadyActive) {
                target = FindAOpponentToPass();

                if (target != null) {
                    isAlreadyActive = true;
                }
            }

            if (isAlreadyActive) {
                if (Player.LookTo(in deltaTime, target.Position - Player.Position)) {
                    ball.Hold(Player);
                    Player.Cross(target.Position);
                    target = null;
                }

                return true;
            }

            return false;
        }
    }
}
