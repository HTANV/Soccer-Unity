using FStudio.Data;
using FStudio.UI.Graphics;
using UnityEditor;
using UnityEngine;

namespace FStudio.Database {
    public class KitEntry : ScriptableObject {
        private const string defaultPreviewAssetPath = "Assets/FootballSimulator/Arts/FootballPlayer/PlayerModel/UIKits/KitMask1.png";
        private const string defaultAssetPath = "Assets/FootballSimulator/Arts/FootballPlayer/PlayerModel/Textures/KitSchemas/KitMask1.png";

        public Texture PreviewTexture;

        public Texture KitMaterial;
        public Color Color1;
        public Color Color2;
        public Color TextColor;

        public Texture GKKitMaterial;
        public Color GKColor1;
        public Color GKColor2;
        public Color GKTextColor;

#if UNITY_EDITOR
        public void Initialize() {

            if (PreviewTexture == null) {
                PreviewTexture = AssetDatabase.LoadAssetAtPath<Texture>(defaultPreviewAssetPath);
            }

            if (KitMaterial == null) {
                KitMaterial = AssetDatabase.LoadAssetAtPath<Texture>(defaultAssetPath);
            }

            if (Color1 == default) {
                Color1 = DatabaseService.RandomColor();
            }

            if (Color2 == default) {
                Color2 = DatabaseService.RandomColor();
            }

            if (GKKitMaterial == null) {
                GKKitMaterial = AssetDatabase.LoadAssetAtPath<Texture>(defaultAssetPath);
            }

            if (GKColor1 == default) {
                GKColor1 = DatabaseService.RandomColor();
            }

            if (GKColor2 == default) {
                GKColor2 = DatabaseService.RandomColor();
            }
        }
#endif
    }
}

