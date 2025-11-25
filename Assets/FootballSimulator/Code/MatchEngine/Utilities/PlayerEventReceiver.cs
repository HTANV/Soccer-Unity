using UnityEngine;
using FStudio.MatchEngine.Players.PlayerController;

namespace FStudio.MatchEngine.Utilities {
    public class PlayerEventReceiver : MonoBehaviour {

        private IPlayerController parentPlayer;

        private void Start() {
            parentPlayer = GetComponentInParent<IPlayerController>();
        }
        private void BallHitEvent() {
            if (parentPlayer == null) {
                return;
            }

            parentPlayer.BallHitEvent();
        }
    }
}
