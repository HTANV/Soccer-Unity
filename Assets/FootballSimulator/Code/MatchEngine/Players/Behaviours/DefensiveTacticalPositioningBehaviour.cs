
using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.FieldPositions;
using FStudio.MatchEngine.Players.PlayerController;
using FStudio.Data;
using System.Linq;
using UnityEngine;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class DefensiveTacticalPositioningBehaviour : AbstractTacticalPositioning {
        private const float OFFSIDE_LINE_X_DIFF_BETWEEN_BALL = -2;

        private const float KEEP_DIST_BETWEEN_BALL_ON_X = 5;

        private const float LEAK_CLOSER_0_1 = 0.5f;

        public override bool Behave(bool isAlreadyActive) {
            Player.CurrentAct = Acts.DefensiveTacticalPositioningBehaviour;

            var ballPosition = ball.transform.position;

            var tacticalPosition = Player.GetFieldPosition(
                ball.HolderPlayer?.GameTeam == Player.GameTeam,
                teamBehaviour,
                in fieldEndX,
                in fieldEndY,
                ballPosition,
                Player.MarkingTarget,
                in offsideLine,
                goalNet,
                targetGoalNet);

            bool noOffsideCheck = false;

            if (ball.HolderTeam != Player.GameTeam) {
                var ballX = ballPosition.x;
                // opponent has the ball. we should keep the the X position.
                if (Player.toGoalXDirection > 0 && tacticalPosition.x > ballX - KEEP_DIST_BETWEEN_BALL_ON_X) {
                    noOffsideCheck = true;
                    tacticalPosition.x = Mathf.Lerp(tacticalPosition.x, ballX - KEEP_DIST_BETWEEN_BALL_ON_X, LEAK_CLOSER_0_1);
                } else if (Player.toGoalXDirection < 0 && tacticalPosition.x < ballX + KEEP_DIST_BETWEEN_BALL_ON_X) {
                    noOffsideCheck = true;
                    tacticalPosition.x = Mathf.Lerp (tacticalPosition.x, ballX + KEEP_DIST_BETWEEN_BALL_ON_X, LEAK_CLOSER_0_1);
                }
            }

            var ballPos = ball.transform.position;

            if (!noOffsideCheck) {
                // pick the line holder.
                var possibleLineHolders = teammates.Where(x => !x.IsGK && x != Player && x.IsFrontOfMe(ballPos, OFFSIDE_LINE_X_DIFF_BETWEEN_BALL));

                PlayerBase lineHolder = null;

                if (Player.toGoalXDirection > 0) {
                    lineHolder = possibleLineHolders.OrderBy(x => x.Position.x).FirstOrDefault();
                } else {
                    lineHolder = possibleLineHolders.OrderByDescending(x => x.Position.x).FirstOrDefault();
                }

                if (lineHolder != null && Player.IsFrontOfMe(lineHolder.Position)) {
                    tacticalPosition.x = Mathf.Lerp(tacticalPosition.x, lineHolder.Position.x, 1);
                }
            }

            var movementMode = RequiredMovementType(Player.Position, tacticalPosition);

            if (teamBehaviour == Tactics.TeamBehaviour.WaitingForTeamGK) {
                movementMode = MovementType.Normal;
            }

            if (Player.CaughtInOffside) {
                movementMode = MovementType.Relax;
            }

            //Player.AvoidMarkers(teammates, ref tacticalPosition, 3);

            targetPosition = tacticalPosition;

            Player.MoveTo(in deltaTime, targetPosition, false, movementMode);
            Player.FocusToBall(in deltaTime, ball);

            return true;
        }
    }
}
