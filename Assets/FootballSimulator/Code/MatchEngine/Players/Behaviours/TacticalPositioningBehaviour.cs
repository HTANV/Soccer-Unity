
using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.FieldPositions;
using FStudio.MatchEngine.Players.PlayerController;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class TacticalPositioningBehaviour : AbstractTacticalPositioning {
        public override bool Behave (bool isAlreadyActive) {
            Player.CurrentAct = Acts.TacticalPositioning;

            var tacticalPosition = Player.GetFieldPosition(
                ball.HolderPlayer?.GameTeam == Player.GameTeam,
                teamBehaviour,
                in fieldEndX,
                in fieldEndY,
                ball.transform.position,
                Player.MarkingTarget,
                in offsideLine,
                goalNet,
                targetGoalNet);

            targetPosition = tacticalPosition;

            var movementMode = MovementType.Normal;

            if (teamBehaviour == Tactics.TeamBehaviour.WaitingForOpponentGK) {
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

            Player.MoveTo(in deltaTime, target, false, movementMode);
            Player.FocusToBall(in deltaTime, ball); 

            return true;
        }
    }
}
