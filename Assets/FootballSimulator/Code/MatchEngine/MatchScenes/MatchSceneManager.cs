
using MatchEngine.MatchScenes.Implementations;

namespace MatchEngine.MatchScenes {
    public class MatchSceneManager {
        private IMatchScene activeScene = null;

        private readonly IMatchScene[] matchScenes = new IMatchScene[] {
            new GoToDressingRoomScene(),
            new GoalCelebrationScene(),
        };

        /// <summary>
        /// Can player move?
        /// </summary>
        /// <param name="tactics"></param>
        /// <returns></returns>
        public ESceneResult UpdateScenes () {
            if (activeScene != null) {
                var result = activeScene.Update();

                if (result != ESceneResult.Failed) {
                    return result;
                } else {
                    activeScene = null;
                }
            }

            foreach (var scene in matchScenes) {
                var result = scene.Update();
                if (result != ESceneResult.Failed) {
                    activeScene = scene;
                    return result;
                } 
            }

            return ESceneResult.Failed;
        }

        public void Dispose () {
            foreach (var scene in matchScenes) {
                scene.Dispose();
            }
        }
    }
}
