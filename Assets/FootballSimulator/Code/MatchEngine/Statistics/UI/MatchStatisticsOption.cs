
using Shared;

namespace FStudio.MatchEngine.UI {
    public class MatchStatisticsOption : IUnique<MatchStatisticsOption> {
        public int ID { get; set; }
        public string Header { get; set; }

        public bool IsSame(MatchStatisticsOption other) {
            return other.ID == ID;
        }
    }
}
