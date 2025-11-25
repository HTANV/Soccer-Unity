
using FStudio.Loaders;
using FStudio.Data;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FStudio.UI.MatchThemes.TeamPresentation {
    public class TeamPresentationSquadPlayer : MonoBehaviour, IStaticPoolMember<TeamSquadMember> {
        public bool IsActive => gameObject.activeSelf;

        [SerializeField] private SquadPosition squadPosition;

        [SerializeField] private TextMeshProUGUI nameText, overallText;

        [SerializeField] private Outline[] overallOutlines;
        [SerializeField] private Gradient overallColorGradient;
        [SerializeField] private AnimationCurve overallOutlineCurve;
        [SerializeField] private float overallOutlineSize;

        public TeamSquadMember Member { get; set; }

        public void MarkAsActive() {
            gameObject.SetActive(true);
        }

        public void MarkAsDeactive() {
            gameObject.SetActive(false);
        }

#pragma warning disable CS1998
        public async Task SetMember(TeamSquadMember member) {
            this.Member = member;

            if (this.Member == null) {
                return;
            }

            overallText.text = member.player.Overall.ToString ();
            nameText.text = member.player.Name;
            squadPosition.SetPosition(PositionRules.GetBasePosition (member.position));

            float overall = member.player.Overall / 100f;
            var overallColor = overallColorGradient.Evaluate(overall);
            var overallOutline = overallOutlineCurve.Evaluate(overall) * overallOutlineSize;
            var effectDist = new Vector2(overallOutline, -overallOutline);

            foreach (var outline in overallOutlines) {
                outline.effectColor = overallColor;
                outline.effectDistance = effectDist;
            }
        }
#pragma warning restore CS1998
    }
}
