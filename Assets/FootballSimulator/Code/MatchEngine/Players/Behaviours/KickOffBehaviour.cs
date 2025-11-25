
using FStudio.MatchEngine.Enums;

using UnityEngine;

using System.Linq;

namespace FStudio.MatchEngine.Players.Behaviours {
    public class KickOffBehaviour : BaseBehaviour {
        private PlayerBase teammateToPass;

        public override bool Behave(bool isAlreadyActive) {
            if (matchStatus != MatchStatus.WaitingForKickOff) {
                return false;
            }

            if (ball.HolderPlayer != Player) {
                return false;
            }

            if (!isAlreadyActive) {
                // find a player and pass.
                teammateToPass = 
                    teammates.Where (x=>x!= Player). // from all teammates
                    OrderBy (x=>Vector3.Distance (x.Position, Player.Position)). // order by position
                    Take (4). // take first 4
                    OrderBy (x=>System.Guid.NewGuid ()).FirstOrDefault(); // pick randomly.

                if (teammateToPass != null) {
                    isAlreadyActive = true;
                }
            }

            if (isAlreadyActive) {
                if (Player.PassToTarget(in deltaTime, teammateToPass.Position)) {
                    // set pass target. after pass target player will behave with BallChasingBehaviour.
                    Player.PassingTarget = teammateToPass;
                }
            }

            return true;
        }
    }
}
