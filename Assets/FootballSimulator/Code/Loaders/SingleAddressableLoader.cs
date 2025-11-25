
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FStudio.Loaders {
    [Serializable]
    public class SingleAddressableLoader {
        [SerializeField] private AssetReference prefab;
        private GameObject currentInstantiatedTheme;

        private AsyncOperationHandle<GameObject> operation;

        public GameObject CurrentInstantiated => currentInstantiatedTheme;

        private bool isLoading => 
            operation.IsValid() && 
            operation.Status == AsyncOperationStatus.None;

        public void Unload() {
            if (currentInstantiatedTheme != null) {
                prefab.ReleaseInstance(currentInstantiatedTheme);
                currentInstantiatedTheme = null;
            }
        }

        public async Task Load() {
            if (isLoading) {
                return;
            }

            operation = prefab.InstantiateAsync();
            this.currentInstantiatedTheme = await operation.Task;
        }
    }
}
