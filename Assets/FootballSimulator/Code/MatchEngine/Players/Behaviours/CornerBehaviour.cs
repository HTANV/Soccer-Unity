using FStudio.MatchEngine.Enums;
using UnityEngine;
using System.Linq;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class CornerBehaviour : BaseBehaviour {
        private readonly Vector2 cornerBox = new Vector2 (8, 16);

        private const float TO_PENALTY_BOX = 9.15f;

        private const float TO_TARGET = 0.8f;

        private const float DIRECTION_MULTIPLIER = 7;

        private Vector3 target;

        /// <summary>
        /// Player will try to find a target, closer to goal net and not marked.
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="ball"></param>
        /// <param name="targetGoalNet"></param>
        /// <param name="teammates"></param>
        /// <returns></returns>
        public override bool Behave(bool isAlreadyActive) {
            if (!Player.IsCornerHolder) {
                return false;
            }

            if (!isAlreadyActive) {
                var penaltyPoint = targetGoalNet.Position;
                penaltyPoint -= Player.GoalDirection * TO_PENALTY_BOX;

                target = penaltyPoint + new Vector3(
                    Random.Range(-cornerBox.x, cornerBox.x), 
                    0,
                    Random.Range(-cornerBox.y, cornerBox.y)
                    );

                var closest = teammates.OrderBy(x => Vector3.Distance(x.Position, target)).FirstOrDefault ();

                target = Vector3.Lerp(target, closest.Position, TO_TARGET) + DIRECTION_MULTIPLIER * (closest.Position - Player.Position).normalized;

                Debug.Log($"Corner target is {target}");

                isAlreadyActive = true;
            }
            
            if (isAlreadyActive) {
                Player.CurrentAct = Acts.ThrowIn;

                Player.Cross(target);

                // teammates chase the ball directly.
                foreach (var e in Player.GameTeam.GamePlayers) {
                    if (e.PlayerFieldProgress > 0.8f) {
                        e.ActivateBehaviour("BallChasingWithoutCondition");
                    }
                }
            }

            return true;
        }
    }
}
