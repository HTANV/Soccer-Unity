
using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.FieldPositions;
using FStudio.MatchEngine.Players.PlayerController;
using System.Linq;
using UnityEngine;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class JoinTheAttackBehaviour : AbstractTeammatesShouldBeware {
        public enum ForwardCurve {
            GoalFocused,
            KeepX
        }

        private const float MIN_BALL_PROGRESS = 0.5f;

        private Vector3 tacticalPosition;
        private readonly float joinPower;
        private readonly float ballToOffsideLinePercentage;
        private readonly ForwardCurve curve;

        /// <summary>
        /// Join the attack with power, Power = 1 usually medium joining.
        /// IF you want to make a full attack, you can use a number like 2-3
        /// </summary>
        /// <param name="joinPower"></param>
        /// <param name="ballToOffsideLinePercentage">Between 0 and 1</param>
        public JoinTheAttackBehaviour (ForwardCurve curve, float ballToOffsideLinePercentage, float joinPower) {
            this.joinPower = joinPower;
            this.ballToOffsideLinePercentage = ballToOffsideLinePercentage;
            this.curve = curve;
        }
       

        public override bool Behave (bool isAlreadyActive) {
            if (!ShouldIAttackWhenNotHoldingTheBall ()) {
                return false;
            }

            if (Player.CaughtInOffside) {
                return false;
            }

            if (Player.GameTeam.BallProgress < MIN_BALL_PROGRESS) {
                return false;
            }

            if (!isAlreadyActive) {
                // check offside.
                var offside = PlayerBase.IsPositionOffside(Player.Position + Player.GoalDirection, targetGoalNet, offsideLine);
                if (offside.Item1) {
                    return false;
                }
                //

                tacticalPosition = Player.GetFieldPosition(
                    ball.HolderPlayer?.GameTeam == Player.GameTeam,
                    teamBehaviour,
                    in fieldEndX,
                    in fieldEndY,
                    ball.transform.position,
                    null,
                    in offsideLine,
                    goalNet,
                    targetGoalNet);

                var ballProgress = Player.GameTeam.BallProgress;

                var lerper = EngineSettings.Current.JoinTheAttackCurves[(int)curve].Evaluate(ballProgress);

                // ball distance on x
                var targetX = Mathf.Lerp (ball.transform.position.x - goalNet.Direction.x, offsideLine - goalNet.Direction.x, 
                    ballToOffsideLinePercentage * ballProgress);

                var targetGoalNetPosition = targetGoalNet.Position;

                // do that by progress of the ball position on X
                tacticalPosition.x = Mathf.Lerp(tacticalPosition.x, targetX, 
                    joinPower * 
                    Player.GameTeam.Team.TeamTactics.JoinAttackPowerByFieldProgressCurve.Evaluate (ballProgress));

                tacticalPosition = Vector3.Lerp(tacticalPosition, targetGoalNetPosition, lerper);

                isAlreadyActive = true;
            }

            if (isAlreadyActive) {
                if (!CanIJoinAttack()) {
                    return false;
                }

                if (IsTheBallGoingOutside()) {
                    return false;
                }

                var playerPosition = Player.Position;

                if (playerPosition.x < 1 || playerPosition.x > fieldEndX - 1) {
                    return false;
                }
                //

                MovementType movementType = MovementType.BestHeCanDo;

                if (Player.GameTeam.BallProgress < 0.35f) {
                    movementType = MovementType.Relax;
                } else if (Player.GameTeam.BallProgress < 0.55f) {
                    movementType = MovementType.Normal;
                }

                Player.AvoidMarkers(teammates, ref tacticalPosition, 5);

                Player.CurrentAct = Acts.JoinTheAttack;
                if (Player.MoveTo(in deltaTime, tacticalPosition, true, movementType)) {
                    return false;
                }

                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Amount of the players behind the last player of opponents.
        /// </summary>
        /// <returns></returns>
        private bool CanIJoinAttack () {
            if (AmITheChaser ().result != BallChasingResult.None || 
                (Player.PlayerFieldProgress > 0.35 && ShouldIBeware ().shouldI)) {
                return false;
            }

            return true;
        }
    }
}
