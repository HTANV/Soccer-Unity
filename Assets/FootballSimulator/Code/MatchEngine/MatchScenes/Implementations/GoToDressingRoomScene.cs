using FStudio.Events;
using FStudio.MatchEngine;
using FStudio.MatchEngine.Balls;
using FStudio.MatchEngine.Events;
using FStudio.MatchEngine.Players.PlayerController;
using UnityEngine;

namespace MatchEngine.MatchScenes.Implementations {
    public class GoToDressingRoomScene : IMatchScene {
        private bool isMatchCompleted;

        public GoToDressingRoomScene() {
            EventManager.Subscribe<FinalWhistleEvent>(FinalWhistle);
        }

        public void Dispose() {
            EventManager.UnSubscribe<FinalWhistleEvent>(FinalWhistle);
        }

        public ESceneResult Update() {
            if (isMatchCompleted) {
                GoToDressingRoom();
            }

            return isMatchCompleted ? ESceneResult.BlockLogic : ESceneResult.Failed;
        }

        private void FinalWhistle(FinalWhistleEvent _) {
            isMatchCompleted = true;
        }

        private void GoToDressingRoom() {
            var fieldSize = MatchManager.Current.SizeOfField;
            var fieldEndX = fieldSize.x;
            var ball = Ball.Current;
            var allPlayers = MatchManager.AllPlayers;

            // move all players to the dressing room.
            var dressingRoomPoint = new Vector3(fieldEndX / 2, 0, -50);

            var dT = Time.deltaTime;
            var time = Time.time;

            ball.Release();

            foreach (var player in allPlayers) {
                player.PlayerController.IsPhysicsEnabled = true;

                if (!player.MoveTo(in dT, dressingRoomPoint, true, MovementType.Relax)) {
                    player.ProcessMovement(in time, in dT);
                }
            }
        }
    }
}
