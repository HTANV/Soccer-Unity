using FStudio.MatchEngine.Enums;
using UnityEngine;
using System.Linq;
using FStudio.Events;
using FStudio.MatchEngine.Events;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class ThrowInBehaviour : BaseBehaviour {
        private PlayerBase target;

        /// <summary>
        /// Player will try to find a target, closer to goal net and not marked.
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="ball"></param>
        /// <param name="targetGoalNet"></param>
        /// <param name="teammates"></param>
        /// <returns></returns>
        public override bool Behave (bool isAlreadyActive) {
            if (!Player.IsThrowHolder) {
                return false;
            }

            if (!isAlreadyActive) {
                var playerPos = Player.Position;

                var targetTeammate = teammates.
                    Where(x => x != Player && !x.IsGK).
                    OrderBy(x=>
                    Vector3.Distance(x.Position, playerPos)).
                    Take (5).
                    OrderBy (x=>System.Guid.NewGuid ()).
                    FirstOrDefault();

                if (targetTeammate != null) {
                    target = targetTeammate;
                    isAlreadyActive = true;
                }
            }

            if (isAlreadyActive) {
                Player.CurrentAct = Acts.ThrowIn;

                var targetPos = target.Position;

                if (Player.LookTo (in deltaTime, targetPos - Player.Position)) {
                    Player.Cross (targetPos);

                    Player.PassingTarget = target;
                    EventManager.Trigger(new PlayerThrowInEvent(Player));
                }
            }

            return true;
        }
    }
}
