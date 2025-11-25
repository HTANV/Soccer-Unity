
using UnityEngine;

using System.Linq;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class GKBallHoldStay : BaseBehaviour {
        private const float BALL_HOLD_TIME_MIN = 1.25f, BALL_HOLD_TIME_MAX = 3f;

        private const float CLOSER_OPPONENT_RADIUS = 10f;

        private float timer;

        private bool expired;

        public override sealed bool Behave(bool isAlreadyActive) {
            if (!Player.IsGKUntouchable || !Player.IsHoldingBall) {
                timer = 0f;
                expired = false;
                return false;
            }

            if (expired) {
                return false;
            }

            if (timer == 0f) {
                timer = time + Random.Range(BALL_HOLD_TIME_MIN, BALL_HOLD_TIME_MAX);
            }

            var isBlocked = opponents.
                Where(x => 
                Vector3.Distance(x.Position, Player.Position) < CLOSER_OPPONENT_RADIUS || 
                !Player.IsFrontOfMe (x.Position)).Any ();

            if (isBlocked) {
                timer = time + 1;
            }

            Player.Stop(in deltaTime);
            Player.LookTo(in deltaTime, goalNet.Direction); // look to forward.

            expired = !(timer > time);

            return !expired;
        }
    }
}
