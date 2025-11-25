using UnityEngine;
using FStudio.MatchEngine.Graphics;
using FStudio.Graphics.RenderTextureCreators;
using FStudio.Database;

namespace FStudio.MatchEngine.Players.PlayerController {
    [RequireComponent (typeof (PlayerWeight))]
    public class PlayerGraphic : MonoBehaviour {
        private const string BOOT_COLOR_MATERIAL_PROPERTY = "_BootColor";
        private const string SOCK_ACCESSORIES_COLOR__MATERIAL_PROPERTY = "_SockAccessoriesColor";
        private const string GK_GLOVES_COLOR_MATERIAL_PROPERTY = "_GKGlovesColor";
        private const string HAIR_COLOR_MATERIAL_PROPERTY = "_Color";
        private const string SKIN_COLOR_MATERIAL_PROPERTY = "_SkinColor";
        private const string FAKE_CLOTH_POWER = "_FakeClothPower";

        private const string PLAYER_NAME_RENDER_TEXTURE = "_PlayerNameRenderTexture";
        private const string PLAYER_NUMBER_RENDER_TEXTURE = "_PlayerNumberRenderTexture";
        private const string FIRST_COLOR = "_FirstColor";
        private const string SECOND_COLOR = "_SecondColor";

        [SerializeField] private PlayerWeight playerWeight;

        [SerializeField] private Renderer[] hairRenderers = default;
        [SerializeField] private Renderer[] facialHairRenderers = default;

        [SerializeField] public SkinnedMeshRenderer mainRenderer;

        [SerializeField] private GameObject refereeFlag;

        private void OnValidate () {
            playerWeight = GetComponent<PlayerWeight>();
        }

        public void SetGKGloves (bool value) {
            if (mainRenderer == null) {
                return;
            }

            Debug.Log("[PlayerRenderer] SetGKGloves: _GKGlovesColor " + value);
            var color = mainRenderer.material.GetColor(GK_GLOVES_COLOR_MATERIAL_PROPERTY);
            color.a = value ? 1 : 0;
            mainRenderer.material.SetColor(GK_GLOVES_COLOR_MATERIAL_PROPERTY, color);
        }

        public void SetPlayer  (
            int playerNumber,
            Material kitMaterial, 
            PlayerEntry player) {

            if (mainRenderer == null) {
                return;
            }

            Debug.Log($"SetPlayer {kitMaterial}");

            mainRenderer.material = kitMaterial;
            mainRenderer.material.SetColor(BOOT_COLOR_MATERIAL_PROPERTY, BootColors.Current.GetColor(player.BootColor));
            mainRenderer.material.SetColor(SOCK_ACCESSORIES_COLOR__MATERIAL_PROPERTY, SockAccessoryColors.Current.GetColor(player.SockAccessoryColor));
            mainRenderer.material.SetColor(SKIN_COLOR_MATERIAL_PROPERTY, SkinColors.Current.GetColor(player.SkinColor));

            var textColor = Color.white;
            var colorWeight = Color.Lerp (kitMaterial.GetColor(FIRST_COLOR), kitMaterial.GetColor(SECOND_COLOR), 0.5f);
            if (colorWeight.grayscale > 0.6f){
                textColor = Color.black;
            }

            if (playerNumber > 0) {
                Debug.Log("Player number: " + playerNumber);
                var numberTexture = playerNumber == 0 ? null : TextCreator.Current.Render(playerNumber.ToString(), textColor, 3);
                mainRenderer.material.SetTexture(PLAYER_NUMBER_RENDER_TEXTURE, numberTexture);
                var nameTexture = TextCreator.Current.Render(player.name, textColor, 0.75f);
                mainRenderer.material.SetTexture(PLAYER_NAME_RENDER_TEXTURE, nameTexture);
            }

            foreach (var rend in hairRenderers) {
                rend.gameObject.SetActive(false);
            }

            if (player.HairStyles > 0) {
                var rend = hairRenderers[(int)player.HairStyles - 1];
                rend.gameObject.SetActive(true);

                var renderers = rend.GetComponentsInChildren<Renderer>(true);
                foreach (var renderer in renderers) {
                    renderer.material.SetColor(
                        HAIR_COLOR_MATERIAL_PROPERTY,
                        PlayerHairColors.Current.GetColor(player.HairColor));
                }
            }

            foreach (var rend in facialHairRenderers) {
                rend.gameObject.SetActive(false);
            }

            if (player.FacialHairStyles > 0) {
                var rend = facialHairRenderers[(int)player.FacialHairStyles - 1];
                rend.gameObject.SetActive(true);

                var renderers = rend.GetComponentsInChildren<Renderer>(true);
                foreach (var renderer in renderers) {
                    renderer.material.SetColor(
                        HAIR_COLOR_MATERIAL_PROPERTY,
                        PlayerHairColors.Current.GetColor(player.FacialHairColor));
                }
            }

			var relativeWeight = player.weight * ((float)(player.height - 100) / player.weight);
			
            playerWeight.SetWeight (relativeWeight);
        }

        public void SetRefereeFlag (bool value) {
            if (refereeFlag != null) {
                refereeFlag.SetActive(value);
            }
        }
    }
}
