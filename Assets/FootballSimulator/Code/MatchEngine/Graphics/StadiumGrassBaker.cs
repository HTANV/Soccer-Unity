

using System.Threading.Tasks;
using UnityEngine;

namespace FStudio.MatchEngine.Graphics {
    /// <summary>
    /// This is a monobehaviour derived class which helps us to bake grass textures from realtime shader.
    /// </summary>
    internal class StadiumGrassBaker : MonoBehaviour {
        [SerializeField] private string savePath = "Arts/";

        [SerializeField] private int textureQuality = 2;

        private async void Start () {
            await Task.Delay(100); // wait a bit

            ScreenCapture.CaptureScreenshot($"{savePath}screen.png", textureQuality);

            await Task.Delay(100); // wait a bit
        }
    }
}
