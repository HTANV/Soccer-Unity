using UnityEngine;
using FStudio.UI.Graphics;
using UnityEngine.UI;

namespace FStudio.UI.KitRenderer {
    public class KitSolver : MonoBehaviour {
        [SerializeField] private Image[] kitImages;

        public void SetKit (
            Texture maskTexture, 
            Color color1, 
            Color color2) {
            var uiMaterial = UILargeKitMaterial.Current.GetMaterial(maskTexture, color1, color2);

            foreach (var kit in kitImages) {
                kit.material = uiMaterial;
            }
        }
    }
}
