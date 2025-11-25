using UnityEngine;

using System.Threading.Tasks;

using FStudio.Data;

using UnityEngine.AddressableAssets;
using FStudio.Scriptables;
using FStudio.Utilities;

namespace FStudio.MatchEngine.Graphics {
    public abstract class KitMaterial<T> : SerializedSingletonScriptable<T> where T : Object {
        private const string MASK_TEXTURE = "_MaskTexture";
        private const string KIT_FIRST_COLOR = "_FirstColor";
        private const string KIT_SECOND_COLOR = "_SecondColor";

        [SerializeField] private Material Material = default;

        public Material GetMaterial(
            Texture maskTexture, 
            Color color1, Color color2) {
            var newMaterial = new Material(Material);

            newMaterial.SetTexture(MASK_TEXTURE, maskTexture);

            newMaterial.SetColor(KIT_FIRST_COLOR, color1);

            newMaterial.SetColor(KIT_SECOND_COLOR, color2);

            return newMaterial;
        }
    }
}
