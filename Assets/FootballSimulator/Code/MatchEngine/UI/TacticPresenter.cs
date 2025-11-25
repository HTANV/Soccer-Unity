using FStudio.Animation;
using FStudio.MatchEngine.Tactics;
using FStudio.UI;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FStudio.MatchEngine.UI {
    public class TacticPresenter : Panel, IMovable, IDynamicFloat {
        [SerializeField] private TextMeshProUGUI tacticText;
        [SerializeField] private Panel tacticTextPanel;

        [SerializeField] private RectTransform imageRect;
        [SerializeField] private Image image;
        [SerializeField] private AnimationCurve movementCurve;
        [SerializeField] private float movementSpeed = 3;

        [SerializeField] private float[] xPoints;

        private AnimationQuery currentAnimQuery;

        private TacticPresetTypes tacticPreset;

        [SerializeField] private Gradient colorGradient;
        [SerializeField] private AnimationCurve colorCurve;
        [SerializeField] private float colorSpeed = 3;

        private float closeIn;

        private bool isAppearing;

        public void Set(TacticPresetTypes tacticPreset) {
            tacticText.text = tacticPreset.ToString();
            this.tacticPreset = tacticPreset;

            if (isAppearing) {
                return;
            }

            if (IsActive) {
                Slide();
            } else {
                Appear();
            }
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            isAppearing = true;
        }

        protected override async void OnAppeared() {
            base.OnAppeared();
            isAppearing = false;
            tacticTextPanel.Appear();
            Slide();

            tacticTextPanel.Appear();

            while (closeIn > Time.time) {
                await Task.Yield();
            }

            tacticTextPanel.Disappear();

            Disappear();
        }

        private async void Slide() {
            closeIn = Time.time + 4;
            
            while (currentAnimQuery != null) {
                await Task.Yield();
            }

            var currentPos = GetPosition();
            var xPoint = (int)tacticPreset;
            currentPos.x = xPoints[xPoint];

            var pPoint = (float)(tacticPreset+1) / (float)TacticPresetTypes.ParameterCount;

            var posAnim = new MovementAction(this, currentPos, movementSpeed, movementCurve);
            currentAnimQuery = posAnim.GetQuery();
            var colorAnim = new DynamicFloatAction (this, colorPoint, pPoint, colorSpeed, colorCurve);
            currentAnimQuery.AddToQuery(colorAnim);

            currentAnimQuery.Start(this, () => { currentAnimQuery = null; });
        }

        public void SetAlpha (float alpha) {
            canvasGroup.alpha = alpha;
        }

        public Vector3 GetPosition() {
            return imageRect.anchoredPosition;
        }

        public void SetPosition(Vector3 targetPosition) {
            imageRect.anchoredPosition = targetPosition;
        }

        public Quaternion GetRotation() {
            return imageRect.rotation;
        }

        public void SetRotation(Quaternion rotation) {
            throw new System.NotImplementedException();
        }

        private float colorPoint;

        public float GetFloat() {
            return colorPoint;
        }
        public void SetFloat(float dynamicFloat) {
            colorPoint = dynamicFloat;

            image.color = colorGradient.Evaluate(dynamicFloat);
        }
    }
}
