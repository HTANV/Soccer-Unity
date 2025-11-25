
using FStudio.MatchEngine.Players.PlayerController;
using UnityEngine;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class MarkTheLastGuy : AbstractTeammatesShouldBeware {
        private const float TO_GOAL_NET_FACTOR = 5f;

        private const float MIN_BALL_PROGRESS = .5f;

        private PlayerBase targetPlayer;

        public override bool Behave(bool isAlreadyActive) {
            if (Player.IsHoldingBall) {
                return false;
            }

            if (!ShouldIAttackWhenNotHoldingTheBall()) {
                return false;
            }

            if (Player.GameTeam.BallProgress < MIN_BALL_PROGRESS) {
                return false;
            }

            if (!isAlreadyActive) {
                var bewareCheck = ShouldIBeware(false);

                if (bewareCheck.shouldI) {
                    targetPlayer = bewareCheck.target;

                    isAlreadyActive = true;
                }
            }

            if (isAlreadyActive) {
                Player.CurrentAct = Enums.Acts.MarkTheLastGuy;

                var targetPlayerPos = targetPlayer.Position;

                var target = targetPlayerPos + (goalNet.Position - targetPlayerPos).normalized * TO_GOAL_NET_FACTOR;

                Player.AvoidMarkers(teammates, ref target, 3);

                Player.FocusToBall(in deltaTime, ball);
                Player.MoveTo(in deltaTime, target, false, MovementType.BestHeCanDo);
                return true;
            }

            return false;
        }
    }
}
