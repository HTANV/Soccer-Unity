
using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.FieldPositions;
using FStudio.MatchEngine.Players.PlayerController;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class StrikerTacticalPositioningBehaviour : AbstractTacticalPositioning {
        public override bool Behave(bool isAlreadyActive) {
            var customTactic = teamBehaviour;

            if (customTactic != Tactics.TeamBehaviour.Attacking 
                || 
                ball.HolderPlayer == Player) {
                return false;
            }

            Player.CurrentAct = Acts.StrikerTacticalBehaviour;

            var tacticalPosition = Player.GetFieldPosition(
                ball.HolderPlayer?.GameTeam == Player.GameTeam,
                customTactic,
                in fieldEndX,
                in fieldEndY,
                ball.transform.position,
                null,
                in offsideLine,
                goalNet,
                targetGoalNet);

            var ballX = ball.transform.position.x - goalNet.Direction.x;
            // dont stay behind the ball.
            if (goalNet.Direction.x > 0 && tacticalPosition.x < ballX) {
                tacticalPosition.x = ballX;
            } else if (goalNet.Direction.x < 0 && tacticalPosition.x > ballX) {
                tacticalPosition.x = ballX;
            }

            targetPosition = tacticalPosition;

            var movementMode = MovementType.Relax;

            if (teamBehaviour == Tactics.TeamBehaviour.Attacking) {
                if (Player.IsFrontOfMe (ball.transform.position)) {
                    movementMode = MovementType.BestHeCanDo;
                } else {
                    movementMode = MovementType.Normal;
                }
            } else if (teamBehaviour == Tactics.TeamBehaviour.WaitingForTeamGK) {
                movementMode = MovementType.BestHeCanDo;
            }
            
            if (movementMode != MovementType.BestHeCanDo) {
                movementMode = RequiredMovementType(Player.Position, targetPosition);
            }

            if (Player.CaughtInOffside) {
                movementMode = MovementType.Relax;
            }
            
            var target = targetPosition;

            Player.AvoidMarkers(teammates, ref target, 3);

            Player.MoveTo(in deltaTime, target, false, movementMode); // we are defending. we should move relaxed.

            Player.FocusToBall(in deltaTime, ball);

            return true;
        }
    }
}
