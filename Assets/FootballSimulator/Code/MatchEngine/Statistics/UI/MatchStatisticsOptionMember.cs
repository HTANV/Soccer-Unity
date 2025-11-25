using FStudio.Loaders;
using FStudio.UI.Navigation;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace FStudio.MatchEngine.UI {
    public class MatchStatisticsOptionMember : MonoBehaviour, IStaticPoolMember<MatchStatisticsOption> {
        public bool IsActive => gameObject.activeSelf;

        public MatchStatisticsOption Member { private set; get; }

        [SerializeField] private TextMeshProUGUI[] labels;

        public void MarkAsActive() {
            gameObject.SetActive(true);
        }

        public void MarkAsDeactive() {
            gameObject.SetActive(false);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task SetMember(MatchStatisticsOption member) {
            this.Member = member;

            foreach (var label in labels) {
                label.text = member.Header;
            }
        }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
