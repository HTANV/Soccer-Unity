
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FStudio.Graphics.RenderTextureCreators {
    [ExecuteInEditMode]
    public class TextCreator : AbstractRenderTextureCreator<TextCreator> {
        [SerializeField] private TextMeshProUGUI text;
        public RenderTexture Render (string text, Color color, float size = 1) {
            this.text.text = text;
            this.text.color = color;
            this.text.transform.localScale = Vector3.one * size;
            return base.Render();
        }

#if UNITY_EDITOR
        [SerializeField] private RawImage TestImage;
        [SerializeField] private string TestText;
        [SerializeField] private Color TestColor;
        [SerializeField] private float SizeMultiplier;

        [SerializeField] private bool Test;
        private void Update() {
            if (!Test) {
                return;
            }

            Test = false;

            var rd = Render(TestText, TestColor, SizeMultiplier);
            TestImage.texture = rd;
        }
#endif
    }
}