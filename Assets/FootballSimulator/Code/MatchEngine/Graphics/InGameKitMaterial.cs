using UnityEngine;

using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace FStudio.MatchEngine.Graphics {
    public class InGameKitMaterial : KitMaterial<InGameKitMaterial> {
        [SerializeField] private AssetReferenceT<Material> RefereeMaterial = default;

        public async Task<Material> GetRefereeMaterial () {
            var loadSkin = Addressables.LoadAssetAsync<Material>(RefereeMaterial);

            await loadSkin.Task;

            return loadSkin.Result;
        }
    }
}
