using FStudio.MatchEngine.Enums;
using UnityEngine;
using FStudio.MatchEngine.Players.PlayerController;
using FStudio.MatchEngine.Players.InputBehaviours;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class InputTackleBehaviour : BaseBehaviour, IInputBehaviour {
        public MovementType MovementType { get; private set; }
        public bool IsTriggered { set; private get; }
        public Vector3 InputDirection { set; private get; }

        public override bool Behave(bool isAlreadyActive) {
            if (!IsTriggered && !isAlreadyActive) {
                return false;
            }

            if (!Player.isInputControlled) {
                IsTriggered = false;
                return false;
            }

            if (Player.IsThrowHolder) {
                IsTriggered = false;
                return false;
            }

            if (Player.IsCornerHolder) {
                IsTriggered = false;
                return false;
            }

            if (Player.IsHoldingBall) {
                IsTriggered = false;
                return false;
            }

            if (teamBehaviour == Tactics.TeamBehaviour.Attacking) {
                IsTriggered = false;
                return false;
            }

            var playerPos = Player.Position;

            Vector3 targetPos;

            var holderPlayer = ball.HolderPlayer;
            if (holderPlayer != null) {
                if (holderPlayer.GameTeam == Player.GameTeam) {
                    return false;
                }

                var holderPlayerPos = ball.HolderPlayer.Position;

                var holderDir = ball.HolderPlayer.Direction.normalized;

                var dot = Vector3.Dot(holderDir, (holderPlayerPos - playerPos).normalized);

                targetPos = PlayerBase.Predicter(Player, ball.HolderPlayer);

                // different distance check for tackling.
                if (dot < -0.25f && 
                    Vector3.Distance(targetPos, playerPos) < PlayerBase.TACKLING_DISTANCE / 1.5f) {
                    Player.Tackle(ball);
                    return true;
                }
            } else {
                targetPos = ball.BallPosition(Player);
            }

            Player.CurrentAct = Acts.GoingToTackle;

            Player.AvoidMarkers(teammates, ref targetPos, 3);

            // we need to go for tackling.
            Player.MoveTo(deltaTime, targetPos, true);

            return true;
        }
    }
}
