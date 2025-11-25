using UnityEngine;

using TMPro;

namespace FStudio.UI {
    public class SquadPosition : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI positionText;

        public void SetPosition (FStudio.Data.Positions position) {
            if (position == FStudio.Data.Positions.ParametersCount) {
                positionText.text = ("ALL");
            } else {
                positionText.text = position.ToString();
            }
        }
    }
}
