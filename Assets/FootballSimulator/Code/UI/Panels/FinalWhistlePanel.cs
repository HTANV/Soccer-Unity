using FStudio.UI.Events;

using UnityEngine;

using FStudio.Events;
using FStudio.MatchEngine.Events;
using System.Threading.Tasks;
using FStudio.MatchEngine.UI;

namespace FStudio.UI {
    public class FinalWhistlePanel : MonoBehaviour {
        private const int showStatisticsAfterSeconds = 3000;

        private void OnEnable() {
            EventManager.Subscribe<FinalWhistleEvent>(OnEventCalled);
        }

        private void OnDisable() {
            EventManager.UnSubscribe<FinalWhistleEvent>(OnEventCalled);
        }

        protected async void OnEventCalled(FinalWhistleEvent eventObject) {
            EventManager.Trigger<ShowScoreboardEvent>(null);

            EventManager.Trigger(new InfoboardEvent());

            // show statistics.

            await Task.Delay(showStatisticsAfterSeconds);

            // Show statistics.
            EventManager.Trigger(new MatchStatisticsEvent());
        }
    }
}


