

using FStudio.MatchEngine.FieldPositions;
using FStudio.MatchEngine.Players.PlayerController;
using Shared;
using UnityEngine;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class OurGKDegageBehaviour : BaseBehaviour {
        private readonly Curve movementCurve = new Curve(new Curve.Point[] {
            new Curve.Point (5, (float)MovementType.Relax),
            new Curve.Point (10, (float)MovementType.Normal),
            new Curve.Point (15, (float)MovementType.BestHeCanDo),
        });

        public override bool Behave(bool isAlreadyActive) {
            if (!IsOurGoalKeeperHasTheBallWithProtection ()) {
                return false;
            }

            var tactic = Tactics.TeamBehaviour.WaitingForTeamGK;

            var tacticalPosition = Player.GetFieldPosition(
                false,
                tactic,
                in fieldEndX,
                in fieldEndY,
                ball.transform.position,
                null,
                in offsideLine,
                goalNet,
                targetGoalNet);

            var distanceToTarget = (tacticalPosition - Player.Position).magnitude;

            var movement = (MovementType)Mathf.RoundToInt(movementCurve.Evaluate(distanceToTarget));

            Player.MoveTo(in deltaTime, tacticalPosition, false, movement);

            Player.FocusToBall(in deltaTime, ball);

            return true;
        }
    }
}
