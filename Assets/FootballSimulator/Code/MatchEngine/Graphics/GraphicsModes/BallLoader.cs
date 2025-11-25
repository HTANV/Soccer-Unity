
using FStudio.MatchEngine.Balls;
using FStudio.Utilities;
using System.Threading.Tasks;
using UnityEngine;

namespace FStudio.MatchEngine.Graphics.GraphicsModes {
    public class BallLoader : ScriptableObject {
        private GameObject activeBall;
        private int activeBallType;

        [SerializeField] private SerializableAssetCollection<int, GameObject> balls;

        public async Task LoadBall (int ballType, Transform holder) {
            activeBallType = ballType;
            activeBall = await balls.Instantiate(ballType, holder);
        }


        public void UnloadBall() {
            if (activeBall != null) {
                balls.ReleaseInstantiated(activeBallType, activeBall);
            }
        }

        public async Task LoadRandomBall() {
            var max = balls.Entries.Length;
            int randomBall = Random.Range(0, max);
            await LoadBall(randomBall, Ball.Current.ballAssetPoint);
        }
    }
}
