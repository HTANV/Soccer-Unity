using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using FStudio.Scriptables;
using FStudio.Database;

namespace FStudio.UI.Graphics {
    public class TeamLogoMaterial : SerializedSingletonScriptable<TeamLogoMaterial> {
        private const string MASK_TEXTURE = "_MaskTexture";
        private const string MASK_COLOR_1 = "_FirstColor";
        private const string MASK_COLOR_2 = "_SecondColor";

        [SerializeField] private AssetReferenceT<Material> ScoreboardMaterial = default;

        [SerializeField] private Material mainMaterial;

        public Material GetColoredMaterial(LogoEntry logoResponse) {
            var result = GetColoredMaterial(logoResponse.TeamLogoMaterial, logoResponse.TeamLogoColor1, logoResponse.TeamLogoColor2);
            return result;
        }

        private Material GetColoredMaterial (Texture texture, Color color1, Color color2) {
            var logoMaterial = new Material(mainMaterial);
            logoMaterial.SetTexture(MASK_TEXTURE, texture);

            logoMaterial.SetColor(MASK_COLOR_1, color1);
            logoMaterial.SetColor(MASK_COLOR_2, color2);

            return logoMaterial;
        }

        public async Task<Material> GetScoreboardMaterial(Color color1, Color color2) {
            var board = Addressables.LoadAssetAsync<Material>(ScoreboardMaterial);

            await board.Task;

            var colored = new Material(board.Result);
            colored.SetColor(MASK_COLOR_1, color1);
            colored.SetColor(MASK_COLOR_2, color2);

            return colored;
        }
    }
}
