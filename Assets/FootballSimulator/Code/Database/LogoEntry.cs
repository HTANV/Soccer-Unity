using UnityEditor;
using UnityEngine;

namespace FStudio.Database {
    public class LogoEntry : ScriptableObject {
        private const string defaultAssetPath = "Assets/FootballSimulator/Arts/UI/TeamLogos/TeamLogo1.psd";

        public Texture TeamLogoMaterial;
        public Color TeamLogoColor1;
        public Color TeamLogoColor2;
#if UNITY_EDITOR
        public void Initialize () {

            if (TeamLogoMaterial == null) {
                TeamLogoMaterial = AssetDatabase.LoadAssetAtPath<Texture>(defaultAssetPath);
            }

            if (TeamLogoColor1 == default) {
                TeamLogoColor1 = DatabaseService.RandomColor ();
            }

            if (TeamLogoColor2 == default) {
                TeamLogoColor2 = DatabaseService.RandomColor ();
            }
        }
#endif
    }
}

