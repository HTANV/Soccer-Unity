using FStudio.Data;
using FStudio.MatchEngine.Enums;
using TMPro;
using UnityEngine;

namespace FStudio.UI.Panels {
    internal class MatchSettingsPanel : MonoBehaviour {
        private const string SETTING_AILEVEL = "SETTING_AILEVEL";
        private const string SETTING_DAYTIME = "SETTING_DAYTIME";
        private const string SETTING_SIDE = "SETTING_SIDE";

        public static DayTimes DAYTIMES { private set; get; }
        public static AILevel AILEVEL { private set; get; }
        public static Shared.Responses.MatchCreateRequest.UserTeam SIDE {  private set; get; }


        [SerializeField] private TextMeshProUGUI 
            dayTimeText,
            sideText,
            difficultyText;

        [SerializeField] private Selector 
            dayTimeSelector, 
            sideSelector, 
            aiLevelSelector;

        private void Awake() {
            dayTimeSelector.Max = 3;
            aiLevelSelector.Max = 4;
            sideSelector.Max = 3;

            dayTimeSelector.OnSelectionUpdate += (val) => {
                DAYTIMES = (DayTimes)val;
                dayTimeText.text = DAYTIMES.ToString();
                PlayerPrefs.SetInt(SETTING_DAYTIME, val);
            };

            aiLevelSelector.Max = (int)AILevel.Legendary + 1;
            aiLevelSelector.OnSelectionUpdate = (int val) => {
                AILEVEL = (AILevel)val;
                difficultyText.text = AILEVEL.ToString();

                PlayerPrefs.SetInt(SETTING_AILEVEL, (int)AILEVEL);
            };

            sideSelector.OnSelectionUpdate += (val) => {
                SIDE = (Shared.Responses.MatchCreateRequest.UserTeam) val;
                sideText.text = SIDE.ToString();

                PlayerPrefs.SetInt(SETTING_SIDE, val);
            };

            var aiLevelSetting = PlayerPrefs.GetInt(SETTING_AILEVEL, 4);
            var sideSetting = PlayerPrefs.GetInt(SETTING_SIDE, 0);
            var dayTimeSetting = PlayerPrefs.GetInt(SETTING_DAYTIME, 2);

            sideSelector.   SetSelected(sideSetting);
            aiLevelSelector.SetSelected(aiLevelSetting);
            dayTimeSelector.SetSelected(dayTimeSetting);

            gameObject.SetActive(false);
        }
    }
}
