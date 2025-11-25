
using System.Linq;
using UnityEngine;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class GKDegage : GKPassBehaviour {

        /// <summary>
        /// This cannot be bigger than Ball.IGNORE_COLLISION_TIME_FOR_BALL_HITTER
        /// </summary>
        private const float BALL_HIT_DELAYER = 0.45f;
        private float timer;
        private bool hitter;
        private PlayerBase target;

        public override bool Behave(bool isAlreadyActive) {
            if (Player.IsGoalKickHolder) {
                return false;
            }

            if (!Player.IsHoldingBall && !hitter) {
                return false;
            }

            if (!isAlreadyActive) {
                if (!Player.IsGKUntouchable) {
                    return false;
                } else {
                    isAlreadyActive = true;
                }
            }

            if (!isAlreadyActive) {
                return false;
            }

            if (Player.MoveSpeed > 0.1f) { // stop before degage.
                Player.Stop(in deltaTime);
                return true;
            }

            if (!hitter) {
                timer = time + BALL_HIT_DELAYER;
                ball.Release();

                // we will force this behaviour,
                // this will stop others, and keep this working.
                ForceBehaviour = true; // forcing enabled.

                hitter = true;
            }

            if (hitter) {
                if (timer > time) {
                    return true;
                }

                if (target == null) {
                    target = FindAOpponentToPass();
                }

                if (target == null) {
                    return false;
                }

                var dir = (target.Position - Player.Position).normalized;

                if (Player.LookTo (in deltaTime, dir)) {

                    hitter = false;
                    timer = 0;
                    ForceBehaviour = false; // forcing disabled.

                    // hold the ball again (we cannot hit the ball without holding)
                    ball.Hold(Player);
                    Player.Cross(target.Position + dir * DEGAGE_SPEED_MOD);
                    //

                    target = null;
                }

                return true;
            }

            return true;
        }
    }
}
