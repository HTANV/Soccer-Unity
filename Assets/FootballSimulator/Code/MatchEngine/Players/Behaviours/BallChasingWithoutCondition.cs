

using FStudio.MatchEngine.Enums;
using FStudio.Players.Behaviours;

namespace FStudio.MatchEngine.Players.Behaviours {
    internal class BallChasingWithoutCondition : BallChasingBehaviour {
        private const float FOCUS_TO_BALL_AFTER_HEIGHT = 3f;
        public override bool Behave (bool isAlreadyActive) {
            if (!isAlreadyActive) {
                return false; // cannot work automaticly. It should be manually triggered via ActivateBehaviour.
            }

            if (ball.HolderPlayer != null) {
                return false;
            }

            if (!IsRoughValidated()) {
                return false;
            }

            if (Player.CaughtInOffside) {
                return false;
            }

            chasingAct = AmITheChaser();

            // Chasing ball directly.
            Player.CurrentAct = Acts.GoingToGetTheBall_BallChasing;

            var actualPoint = ball.BallPosition(Player, chasingAct.relaxation);

            var actualBallPos = ball.transform.position;

            bool lookPoint = actualBallPos.y > FOCUS_TO_BALL_AFTER_HEIGHT;

            Player.MoveTo(in deltaTime, actualPoint, !lookPoint);

            if (lookPoint) {
                Player.LookTo(in deltaTime, actualBallPos - Player.Position);
            }

            ChasingDistance = BallChasingDistance(Player);

            return true;
        }
    }
}
