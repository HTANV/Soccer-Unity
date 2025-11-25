
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FStudio.Loaders {
    public class DefaultSceneLoader : MonoBehaviour {
        [SerializeField] private AssetReference defaultSceneAsset;

        private async void Start() {
            // go to default scene.
            await SceneLoader.LoadScene(defaultSceneAsset);
        }
    }
}
