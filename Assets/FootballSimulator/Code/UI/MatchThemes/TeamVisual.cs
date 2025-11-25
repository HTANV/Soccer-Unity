
using FStudio.Database;
using FStudio.UI.Graphics;
using FStudio.UI.MatchThemes.TeamPresentation;
using FStudio.UI.Utilities;
using FStudio.Data;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FStudio.UI.MatchThemes {
    public class TeamVisual : MonoBehaviour {
        public KitRenderer.KitSolver KitSolver;

        [SerializeField] private Image logoImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private ImageFiller overallFiller;
        [SerializeField] private TextMeshProUGUI overallText;

        [SerializeField] private TeamSquadPresentation squadPresentation;

        public async Task SetTeam (TeamEntry teamEntry, Formations formation, PlayerEntry[] players) {
            nameText.text = teamEntry.TeamName;

            var overall = teamEntry.Overall;

            int rangeMin = 60, rangeMax = 84;
            var clamped = Mathf.Clamp(teamEntry.Overall, rangeMin, rangeMax);
            var fill = (clamped - rangeMin) / 3;
            var result = 0.2f + 0.01f + fill * 0.1f;

            overallFiller.Image.fillAmount = 0;
            overallFiller.FillTo(result);
            overallText.text = overall.ToString();

            logoImage.gameObject.SetActive(false);
            logoImage.material = TeamLogoMaterial.Current.GetColoredMaterial(teamEntry.TeamLogo);
            logoImage.gameObject.SetActive(true);

            var squadPlayers = TeamSquadPresentation.PlayersToSquadMembers(players, formation, teamEntry);
            await squadPresentation.SetSquadMembers(squadPlayers);
        }
    }
}
