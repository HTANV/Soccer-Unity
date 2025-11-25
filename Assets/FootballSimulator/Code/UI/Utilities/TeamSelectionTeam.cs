
using FStudio.Database;
using FStudio.UI.Graphics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FStudio.UI.Utilities {
    public class TeamSelectionTeam : MonoBehaviour {
        [SerializeField] private Selector selector;
        [SerializeField] private TextMeshProUGUI teamNameText;
        [SerializeField] private TextMeshProUGUI overallText;
        [SerializeField] private Image teamLogoImage;
        [SerializeField] private ImageFiller overallFiller;

        public TeamEntry SelectedTeam { private set; get; }

        private void Awake() {
            var teams = DatabaseService.LoadTeams();

            selector.Max = teams.Length;
            
            selector.OnSelectionUpdate += (value) => {
                SetTeam(teams[value]);    
            };

            selector.SetSelected(Random.Range(0, selector.Max));
        }

        public void SetTeam (int teamIndex) {
            selector.SetSelected(teamIndex);
        }
        private void SetTeam (TeamEntry teamEntry) {
            SelectedTeam = teamEntry;

            teamNameText.text = teamEntry.TeamName;
            teamLogoImage.material = TeamLogoMaterial.Current.GetColoredMaterial(teamEntry.TeamLogo);

            int rangeMin = 60, rangeMax = 84;
            var clamped = Mathf.Clamp (teamEntry.Overall, rangeMin, rangeMax);
            var fill = (clamped - rangeMin) / 3;
            var result = 0.2f + 0.01f + fill * 0.1f;

            overallFiller.FillTo (result);
            overallText.text = teamEntry.Overall.ToString ();
        }
    }
}
