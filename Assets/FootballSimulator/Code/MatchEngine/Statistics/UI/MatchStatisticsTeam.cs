using FStudio.Database;
using FStudio.UI.Graphics;
using FStudio.UI.Utilities;
using Shared.Responses;
using FStudio.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FStudio.MatchEngine.UI {
    [Serializable]
    public class MatchStatisticsTeam : MonoBehaviour {
        [SerializeField] private ImageFiller overallFiller;
        [SerializeField] private TextMeshProUGUI overallText;

        [SerializeField] private Image[] teamLogoImages = new Image[1];

        [SerializeField] private TextMeshProUGUI[] teamNameTexts = new TextMeshProUGUI[1];

        [SerializeField] private TextMeshProUGUI[] scoreTexts = new TextMeshProUGUI[1];

        public void SetTeam(
            TeamEntry teamEntry,
            PlayerEntry[] players,
            Formations formation,
            int score) {

            var logoMaterial = TeamLogoMaterial.Current.GetColoredMaterial(teamEntry.TeamLogo);

            foreach (var teamLogo in teamLogoImages) {
                teamLogo.material = logoMaterial;
            }

            foreach (var teamNameText in teamNameTexts) {
                teamNameText.text = teamEntry.TeamName.ToUpper();
            }

            foreach (var scoreText in scoreTexts) {
                scoreText.text = score.ToString ();
            }

            var teamOverall = teamEntry.Overall;

            overallFiller.Image.fillAmount = 0;
            overallFiller.FillTo(teamOverall / 100f);
            overallText.text = teamOverall.ToString();
        }
    }
}
