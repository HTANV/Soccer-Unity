using UnityEngine;
using FStudio.MatchEngine.Enums;
using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Tactics;

using System.Collections.Generic;
using FStudio.MatchEngine.Players.Behaviours;
using System.Linq;
using FStudio.MatchEngine.FieldPositions;
using FStudio.MatchEngine.Players.PlayerController;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FStudio.MatchEngine.Players.Positions {
    [ExecuteInEditMode]
    public class GK : PlayerBase {

        private const float SECURE_X_SIZE = 14f;
        private const float SECURE_Z_SIZE = 34.5f;
		
		private const float GK_BALL_CONTROL_IMPULSE_POW = 3;

        private const float NEXT_BALL_TOUCH_OFFSET = 1;

        private float nextBallTouch;

        private const float MIN_Y_FOR_REACT_BALL_MISS = 0.5f;

        private const float BALL_HEIGHT_TO_IMPACT_MOD = 10;

        private const float MAX_BALL_HEIGHT_FOR_LOW_SAVE = 0.6f;

        private readonly AnimationCurve ballHeightToImpactCurve = new AnimationCurve(
                new Keyframe (0,2f),
                new Keyframe (0.5f, 10f),
                new Keyframe (1, 25f),
                new Keyframe (1.5f, 60f),
                new Keyframe (2f, 120f)
            );

        private bool canWeSecureTheBall;

        private Rect secureAreaRect;

        private readonly IEnumerable<BaseBehaviour> m_behaviours = new BaseBehaviour[] {

            new GKGoalKick (),
            new GKBallHoldStay (),
            new GKGoToPenaltyPoint (),
            new GKDegage (),

            // Stop unnecessary headers.
            new RunForwardWithBallBehaviour(0f,
                RunForwardWithBallBehaviour.ForwardCurve.Wingman,
                RunForwardWithBallBehaviour.BewareMod.Normal,
                false,
                0.8f, // activate on ball height 0.8f
                0.65f), // activate before ball progress 0.65),

            new GKSendAway (),
            new BallChasingWithoutCondition (),
            new TryToTackleBehaviour (),
            new GKShieldBehaviour()
        };

        public GK(GameTeam gameTeam, MatchPlayer matchPlayer, Material kitMaterial) : base(gameTeam, matchPlayer, kitMaterial)
        {
        }

        public override IEnumerable<BaseBehaviour> Behaviours => m_behaviours;

        public override Vector3 GetFieldPosition(
            in bool teammateHasTheBall,
            in TeamBehaviour teamBehaviour,
            in int fieldEndX,
            in int fieldEndY,
            in Vector3 ballPosition,
            in PlayerBase markingTarget,
            in float offsideLine,
            GoalNet goalNet,
            GoalNet targetGoalNet) {
            return goalNet.Position + goalNet.Direction;
        }

        public override sealed void Behave(
            in bool isInputControlled,
            in float time,
            in float deltaTime,
            in int fieldEndX,
            in int fieldEndY,
            in MatchStatus matchStatus,
            in TeamBehaviour tactics,
            in float offsideLine,
            Ball ball, 
            GoalNet goalNet, 
            GoalNet targetGoalNet, 
            in PlayerBase[] teammates, 
            in PlayerBase[] opponents) {

            ProcessMovement(in time, in deltaTime);

            base.Behave(in isInputControlled,
                in time,
                in deltaTime,
                in fieldEndX,
                in fieldEndY,
                in matchStatus,
                in tactics,
                in offsideLine,
                ball,
                goalNet,
                targetGoalNet,
                in teammates,
                in opponents);

            // calculate if we can secure the ball.
            if (goalNet.Direction.x > 0) {
                secureAreaRect = new Rect(
                goalNet.Position.x,
                goalNet.Position.z - SECURE_Z_SIZE / 2,
                SECURE_X_SIZE,
                SECURE_Z_SIZE);
            } else {
                secureAreaRect = new Rect(
                goalNet.Position.x - SECURE_X_SIZE,
                goalNet.Position.z - SECURE_Z_SIZE / 2,
                SECURE_X_SIZE,
                SECURE_Z_SIZE);
            }

            var myPosition = new Vector2(Position.x, Position.z);
            canWeSecureTheBall = secureAreaRect.Contains(myPosition);
        }

        public override void OnBallHold() {
            base.OnBallHold();

            Debug.Log("Ball hold " + canWeSecureTheBall);
            Debug.Log("Last holder " + Ball.Current.LastHolder);

            if ((Ball.Current.LastHolder == null || Ball.Current.LastHolder.GameTeam != GameTeam) && canWeSecureTheBall) {
                IsGKUntouchable = true; // we secure the ball here.
                                        // activate GK holding.

                if (!IsGoalKickHolder) {
                    PlayerController.Animator.GKIKController.LerpTo(1);

                    if (Ball.Current.transform.position.y < MAX_BALL_HEIGHT_FOR_LOW_SAVE) {
                        var shieldBehave = Behaviours.Where(x => x is GKShieldBehaviour).Cast<GKShieldBehaviour>().FirstOrDefault();

                        if (!shieldBehave.IsOnJump) {
                            PlayerController.Animator.SetTrigger(PlayerAnimatorVariable.GKBallSave_Low);
                        }
                    }
                }
            }
        }

        public override bool OnBallTouch(float touchHeight, float impulse, Ball ball) {
            float time = Time.time;

            if (nextBallTouch > time) {
                return false;
            }

            nextBallTouch = time + NEXT_BALL_TOUCH_OFFSET;

			impulse = Mathf.Pow (impulse, GK_BALL_CONTROL_IMPULSE_POW);

            var shieldBehave = Behaviours.Where(x => x is GKShieldBehaviour).Cast<GKShieldBehaviour>().FirstOrDefault();

            if (shieldBehave.IsOnJump) {
                impulse += ballHeightToImpactCurve.Evaluate(ball.transform.position.y) * BALL_HEIGHT_TO_IMPACT_MOD;
            }

            var isHolded = base.OnBallTouch(touchHeight, impulse, ball);

            if (!isHolded) {
                if (!shieldBehave.IsOnJump && ball.transform.position.y >= MIN_Y_FOR_REACT_BALL_MISS) {
                    PlayerController.Animator.SetTrigger(PlayerAnimatorVariable.GKMiss);
                }
            }

            return isHolded;
        }

        public override void OnBallRelease() {
            base.OnBallRelease();

            PlayerController.Animator.GKIKController.LerpTo(0);

            Debug.Log("Ball released.");
        }
    }
}