
using FStudio.MatchEngine.Players.PlayerController;
using UnityEngine;

namespace FStudio.MatchEngine.Players.Behaviours {
    internal class IsInOffsideBehaviour : BaseBehaviour {
        public override bool Behave (bool isAlreadyActive) {
            if (Player.CaughtInOffside) {
                var myZ = Player.Position.z;
                var target = Quaternion.Euler (0, myZ > fieldEndY/2 ? 90 : -90, 0) * ball.Velocity.normalized;

                var final = Player.Position + target * 10;

                KeepInField(ref final);

                Player.MoveTo(in deltaTime, Player.Position + target * 10, true, MovementType.Relax);

                return true;
            }

            return false;
        }
    }
}
