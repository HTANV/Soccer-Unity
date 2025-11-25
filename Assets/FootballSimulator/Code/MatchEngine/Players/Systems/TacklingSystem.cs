using UnityEngine;

using FStudio.MatchEngine.Graphics;
using FStudio.MatchEngine.Enums;
using FStudio.Animation;
using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Events;
using FStudio.Events;
using FStudio.MatchEngine.Players.PlayerController;
using FStudio.MatchEngine.Graphics.EventRenderer;

namespace FStudio.MatchEngine.Players {
    public partial class PlayerBase {
        #region tackling
        public const float TACKLING_DISTANCE = 2f;
        private const float TACKLING_RESPONSE_TIME = 0.25f;
        private const float TACKLING_RECOVERY_TIME_FOR_TACKLER = 1.2f;
        private const float TACKLING_RECOVERY_TIME_FOR_TACKLED = 2.25f;
        #endregion

        private readonly string[] spinBlockers = new string[1] { "Spin" };

        /// <summary>
        /// Do a tackling, if you are far away from the ball, you won't get the ball:=)
        /// </summary>
        /// <param name="ball"></param>
        public void DoTackle (Ball ball) {
            if (IsHoldingBall) {
                return;
            }

            EventManager.Trigger(new PlayerSlideTackleEvent(this));

            CurrentAct = Acts.Tackling;

            PlayerController.IsPhysicsEnabled = false;

            PlayerController.Animator.SetTrigger(PlayerAnimatorVariable.Tackling);

            DirtRenderer.Current.SetPosition(1, Position, Rotation);

            if (ball.HolderPlayer.IsGK) {
                return;
            }

            new TimerAction(TACKLING_RESPONSE_TIME).GetQuery().Start(MatchManager.Current, () => {
                calculateTackling();
            });

            void restoreTackler() {
                // tackler restore.
                new TimerAction(TACKLING_RECOVERY_TIME_FOR_TACKLER).GetQuery().Start(MatchManager.Current, () => {
                    PlayerController.IsPhysicsEnabled = true;
                });
            }

            void calculateTackling() {
                if (ball.HolderPlayer == null || 
                    ball.HolderPlayer.IsGKUntouchable || 
                    ball.HolderPlayer.GameTeam == GameTeam) {

                    restoreTackler();

                    return;
                }

                if (Vector3.Distance(ball.HolderPlayer.Position, Position) < TACKLING_DISTANCE) {
                    var ballKeeping = ball.HolderPlayer.MatchPlayer.ActualBallKeeping;
                    var tackling = MatchPlayer.ActualTackling;

                    bool isTacklingSuccess = Random.Range(ballKeeping * 0.5f, ballKeeping) <
                        Random.Range(tackling / EngineSettings.Current.Tackling_Difficulty, tackling);

                    if (isTacklingSuccess) {
                        restoreTackler();

                        EventManager.Trigger(new PlayerWinTheBallEvent (this));
                        EventManager.Trigger(new PlayerLossTheBallEvent (ball.HolderPlayer));
                        EventManager.Trigger(new PlayerTackledEvent(ball.HolderPlayer));

                        // ball holder 
                        var ballHolder = ball.HolderPlayer;
                        ballHolder.PlayerController.IsPhysicsEnabled = false;
                        ballHolder.ballHitAnimationEvent = BallHitAnimationEvent.None;
                        ballHolder.PlayerController.Animator.SetTrigger(PlayerAnimatorVariable.Tackled);

                        new TimerAction(TACKLING_RECOVERY_TIME_FOR_TACKLED).GetQuery().Start(MatchManager.Current, () => {
                            ballHolder.CurrentAct = Acts.Stunned;

                            ballHolder.PlayerController.IsPhysicsEnabled = true;
                        });

                        ball.Release(); // release the ball from the holder.
                    } else {
                        // spin anim.
                        if (ball.HolderPlayer.Velocity.magnitude > 2 && 
                            ball.HolderPlayer.ballHitAnimationEvent == BallHitAnimationEvent.None && 
                            !ball.HolderPlayer.PlayerController.IsAnimationABlocker (in spinBlockers)) {

                            ball.HolderPlayer.PlayerController.Animator.SetTrigger(PlayerAnimatorVariable.Spin);
                        }

                        CurrentAct = Acts.TacklingFailed;

                        restoreTackler();
                    }
                } else {
                    restoreTackler();
                }
            }
        }

        public void Tackle(Ball ball) {
            // TRY TO TACKLE.
            if (ball.HolderPlayer == null) {
                return; // no one holds the ball. No tackling possible.
            }

            DoTackle(ball);
        }
    }
}
