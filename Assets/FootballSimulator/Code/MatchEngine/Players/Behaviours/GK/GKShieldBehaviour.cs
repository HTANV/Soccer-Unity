
using UnityEngine;
using FStudio.Utilities;
using FStudio.MatchEngine.Players.PlayerController;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class GKShieldBehaviour : BallChasingBehaviour {
        private const float GOAL_KEEPING_X_DISTANCE = 2f;

        private const float MAX_DOT_PRODUCT_TO_JUMP = 0; 

        private const float MIN_VEL_TO_CHASE_INSTEAD_OF_KEEP = 10f;

        private const float POS_MISTAKE_BY_BALL_DIST = 0.1f;

        private const float GK_POS_MISTAKE_MOD = 0.2f;

        private const float JUMP_BEFORE_SECONDS = 0.6f;

        private const float MIN_Z_DISTANCE_TO_JUMP = 0.25f;
        private const float MAX_Z_DISTANCE_TO_JUMP = 5;

        private const float JUMP_EXPIRE_TIME = 2f;

        private const float JUMPING_SPEED = 2;

        private float expireJump;

        public bool IsOnJump { private set; get; }

        private Vector3 jumpTarget = default;

        private float positionError;

        private readonly AnimationCurve REACTION_ROLL_RATE_BY_SKILL = new AnimationCurve(new Keyframe[] {
             new Keyframe (0, 65f),
             new Keyframe (100, 100f),
        });

        private readonly AnimationCurve POSITIONING_TO_X_MISTAKE = new AnimationCurve(new Keyframe[] {
             new Keyframe (0, 2f),
             new Keyframe (100, 0f),
        });

        private readonly AnimationCurve JUMPING_DISTANCE_BY_SKILL = new AnimationCurve(new Keyframe[] {
             new Keyframe (0, MIN_Z_DISTANCE_TO_JUMP),
             new Keyframe (100, MAX_Z_DISTANCE_TO_JUMP),
        });

        private bool ShouldIJump (Vector3 intersectPoint) {
            // check if its in the goal size.
            var mid = fieldEndY / 2;
            if (intersectPoint.z < mid - 6 || intersectPoint.z > mid + 6) {
                return false;
            }

            var ballVelMag = ball.Velocity.magnitude;
            var distanceToIntersect = Vector3.Distance(ball.transform.position, intersectPoint);

            var pathAsSeconds = distanceToIntersect / (ballVelMag + 1);

            if (pathAsSeconds < JUMP_BEFORE_SECONDS) {
                var xDist = Mathf.Abs(intersectPoint.x - Player.Position.x);

                return xDist >= MIN_Z_DISTANCE_TO_JUMP && xDist < MAX_Z_DISTANCE_TO_JUMP;
            }

            return false;
        }

        public override sealed bool Behave(bool isAlreadyActive) {
            if (Player.IsHoldingBall || Player.IsGoalKickHolder) {
                IsOnJump = false;
                ForceBehaviour = false;

                return false;
            }

            if (expireJump != 0 && expireJump < time) {
                IsOnJump = false;
                ForceBehaviour = false;
            }

            if (IsOnJump) {
                positionError = Mathf.Lerp(positionError, 0, deltaTime);

                var jumpSkill = Player.MatchPlayer.ActualJump / 100f;

                var mistakeOnX = POSITIONING_TO_X_MISTAKE.Evaluate(Player.MatchPlayer.ActualPositioning);
                var finalMistake = Random.Range(-mistakeOnX, mistakeOnX);

                var includeMistake = jumpTarget + Vector3.right * finalMistake;

                var nextPos = Vector3.Lerp(Player.Position, includeMistake, deltaTime * JUMPING_SPEED * jumpSkill);

                Player.PlayerController.SetInstantPosition (nextPos);
                Player.LookTo (in deltaTime, Player.GoalDirection);

                return true;
            }

            if (ball.Velocity.magnitude < MIN_VEL_TO_CHASE_INSTEAD_OF_KEEP) {
                // pass. chase the ball.
                bool isChasing = base.Behave(false);

                if (isChasing) {
                    return true;// chasing activated.
                }
            }
 
            var ballPositionStart = ball.transform.position;
            ballPositionStart.y = 0;
            var ballPositionEnd = ballPositionStart + ball.Velocity;
            ballPositionStart.y = 0;

            var goalLineX = goalNet.Position.x + GOAL_KEEPING_X_DISTANCE * goalNet.Direction.x;

            var goalLineStart = new Vector3(goalLineX, 0, 0);

            var goalLineEnd = new Vector3(goalLineX, 0, fieldEndY);

            var ballS2F = new LineSegment2f(ballPositionStart, ballPositionEnd);
            var goalLine2F = new LineSegment2f(goalLineStart, goalLineEnd);

            Vector3 goToPosition;

            if (ballS2F.TryIntersect(goalLine2F, out var result, out var _, out var _)) {
                goToPosition = result;
                // line intersected, we should catch the ball. (probably shooted)
                            // check dot product.

                if (!IsOnJump && 
                ShouldIJump(goToPosition) && 
                Player.IsFrontOfMe (ballPositionStart) && 
                Vector3.Dot (goalNet.Direction.normalized, ball.Velocity.normalized) < MAX_DOT_PRODUCT_TO_JUMP) {
                    // roll for reaction.
                    if (Random.Range(0, 100) < REACTION_ROLL_RATE_BY_SKILL.Evaluate(Player.MatchPlayer.ActualReaction)) {
                        // i need to jump to that point to catch, including mistake.
                        var catchPoint = goToPosition;

                        var jumpingMax = Random.Range(JUMPING_DISTANCE_BY_SKILL.Evaluate(Player.MatchPlayer.ActualJump), MAX_Z_DISTANCE_TO_JUMP);

                        // clamp catch point by jumping skill.
                        catchPoint = Player.Position + Vector3.ClampMagnitude(catchPoint - Player.Position, jumpingMax);

                        var jumpSkill = Player.MatchPlayer.ActualJump / 100f;

                        // jump activated.
                        IsOnJump = true;
                        jumpTarget = catchPoint;
                        expireJump = time + JUMP_EXPIRE_TIME / jumpSkill;
                        ForceBehaviour = true;
                        // decide for anim. right or left.

                        positionError = 1;

                        var xVal = jumpTarget.z - Player.Position.z;

                        var animParam = PlayerAnimatorVariable.GKJumpLeft;

                        if (xVal < 0) {
                            // jump to right if we are home.
                            if (Player.toGoalXDirection > 0) {
                                animParam = PlayerAnimatorVariable.GKJumpRight;
                            }
                        } else {
                            if (Player.toGoalXDirection < 0) {
                                animParam = PlayerAnimatorVariable.GKJumpRight;
                            }
                        }

                        // activate animation.
                        Player.PlayerController.Animator.SetTrigger(animParam);

                        return true;
                    }
                }
            } else {
                // not shooted.
                if (AmITheChaser().result != BallChasingResult.None) {
                    base.Behave(isAlreadyActive); // base activation here. need to chase the ball.
                    return true;
                } else {
                    goToPosition = ballPositionEnd;

                    var shield = GKSettings.Current.GKPosition(
                        goToPosition,
                        Player.GameTeam.BallProgress,
                        fieldEndY);

                    goToPosition = new Vector3(goalNet.Position.x + shield.forward * goalNet.Direction.x, 0, shield.side);
                }
            }

            var distanceToBall = Vector3.Distance(ballPositionStart, Player.Position);

            goToPosition += Player.PositioningMistake * distanceToBall * POS_MISTAKE_BY_BALL_DIST * GK_POS_MISTAKE_MOD;

            goToPosition.x = Mathf.Clamp(goToPosition.x, 0, fieldEndX);

            Player.FocusToBall(deltaTime, ball);

            Player.MoveTo(in deltaTime, goToPosition, false);

            return true;
        }
    }
}
