using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.Tactics;
using FStudio.Utilities;
using UnityEngine;

namespace FStudio.MatchEngine.Players.Behaviours {
    public abstract class BaseBehaviour {
        public PlayerBase Player { protected set; get; }
        protected bool isInputControlled;
        protected float time;
        protected float deltaTime;
        protected int fieldEndX;
        protected int fieldEndY;
        protected MatchStatus matchStatus;
        protected TeamBehaviour teamBehaviour;
        protected float offsideLine;
        protected float ourOffsideLine;
        protected Ball ball;
        protected GoalNet goalNet;
        protected GoalNet targetGoalNet;
        protected PlayerBase[] teammates;
        protected PlayerBase[] opponents;

        /// <summary>
        /// If this is enabled, this behaviour will run with the other forceds, but all non forceds will be ignored.
        /// When you don't want to force anymore, turn this off to unblock others.
        /// </summary>
        public bool ForceBehaviour;

        public abstract bool Behave(bool isAlreadyActive);

        /// <summary>
        /// Chekck if we are in the rough gameplay.
        /// </summary>
        /// <returns></returns>
        protected bool IsRoughValidated () {
            if (teamBehaviour != TeamBehaviour.BallChasing &&
                 teamBehaviour != TeamBehaviour.Attacking &&
                 teamBehaviour != TeamBehaviour.Defending) {
                return false;
            }

            return true;
        }
        public void SetBehaviour(
                PlayerBase player,
                in bool isInputControlled,
                in float time,
                in float deltaTime,
                in int fieldEndX,
                in int fieldEndY,
                in MatchStatus matchStatus,
                in TeamBehaviour teamBehaviour,
                in float offsideLine,
                in float ourOffsideLine,
                Ball ball,
                GoalNet goalNet,
                GoalNet targetGoalNet,
                in PlayerBase[] teammates,
                in PlayerBase[] opponents
        ) {
            this.Player = player;
            this.isInputControlled = isInputControlled;
            this.time = time;
            this.deltaTime = deltaTime;
            this.fieldEndX = fieldEndX;
            this.fieldEndY = fieldEndY;
            this.matchStatus = matchStatus;
            this.teamBehaviour = teamBehaviour;
            this.offsideLine = offsideLine;
            this.ourOffsideLine = ourOffsideLine;
            this.ball = ball;
            this.goalNet = goalNet;
            this.targetGoalNet = targetGoalNet;
            this.teammates = teammates;
            this.opponents = opponents;
        }

        /// <summary>
        /// Checks if a position is in the field.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected bool IsPositionInField (in Vector3 position, float offset = 0) {
            if (position.x < -offset || position.x > fieldEndX + offset || position.z < -offset || position.z > fieldEndY + offset) {
                return false;
            }

            return true;
        }

        protected void KeepInField (ref Vector3 position) {
            position.x = Mathf.Clamp(position.x, 0, fieldEndX);
            position.z = Mathf.Clamp(position.z, 0, fieldEndY);
        }

        protected bool IsTheBallGoingOutside () {
            const float MIN_BALL_VEL = 5;
            const float BALL_VELOCITY_MUL = 3;
            const float DISTANCE_OFFSET = 12;

            var ballPosition = ball.transform.position;
            var ballVel = ball.Velocity * BALL_VELOCITY_MUL;
            var ballPredicted = ballPosition + ballVel;

            if (MIN_BALL_VEL > ball.Velocity.magnitude) {
                return false;
            }

            if (IsPositionInField (in ballPredicted)) {
                return false;
            }

            var myDistance = Vector3.Distance(Player.Position, ballPredicted);
            var ballDistance = ballVel.magnitude;

            if (myDistance > ballDistance + DISTANCE_OFFSET) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if you can attack or not.
        /// </summary>
        /// <returns></returns>
        protected bool ShouldIAttackWhenNotHoldingTheBall() {
            if (ball.HolderPlayer == Player) {
                return false;
            }

            if (ball.HolderPlayer != null &&
                ball.HolderPlayer.GameTeam != Player.GameTeam) {
                return false;
            }

            if (ball.HolderPlayer == null &&
                ball.LastHolder != null &&
                ball.LastHolder.GameTeam != Player.GameTeam) {
                return false;
            }

            return true;
        }

        protected bool IsOurGoalKeeperHasTheBallWithProtection () {
            return ball.HolderPlayer != null &&
                ball.HolderPlayer.IsGK &&
                ball.HolderPlayer.GameTeam == Player.GameTeam &&
                ball.HolderPlayer.IsGKUntouchable;
        }

        protected bool IsOpponentGoalKeeperHasTheBallWithProtection() {
            return ball.HolderPlayer != null &&
                ball.HolderPlayer.IsGK &&
                ball.HolderPlayer.GameTeam != Player.GameTeam &&
                ball.HolderPlayer.IsGKUntouchable;
        }

        protected void Log<T> (T behaviour, string message) where T : BaseBehaviour {
            if (Player.PlayerController.IsDebuggerEnabled) {
                Debug.Log($"[{behaviour}] -> {message}");
            }
        }
    }
}
