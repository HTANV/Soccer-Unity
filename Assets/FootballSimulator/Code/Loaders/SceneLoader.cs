using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;

using UnityEngine.SceneManagement;

namespace FStudio.Loaders {
    public static class SceneLoader {
        private const string DEFAULT_SCENE_NAME = "DefaultScene";

        public static async Task LoadDefaultScene () {
            var loader = SceneManager.LoadSceneAsync(DEFAULT_SCENE_NAME, LoadSceneMode.Single);
            
            while (!loader.isDone) {
                await Task.Yield();
            }
        }

        public static async Task<SceneInstance> LoadScene (AssetReference sceneAsset) {
            Debug.Log($"[SceneLoader] Load scene {sceneAsset}");

            var loader = await Addressables.LoadSceneAsync(sceneAsset, UnityEngine.SceneManagement.LoadSceneMode.Single).Task;
            var activator = loader.ActivateAsync();

            while (!activator.isDone) {
                await Task.Yield();
            }

            return loader;
        }
    }
}
