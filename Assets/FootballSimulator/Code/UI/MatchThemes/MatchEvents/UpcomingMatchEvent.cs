using FStudio.Events;
using Shared.Responses;
using FStudio.Data;

namespace FStudio.UI.MatchThemes.MatchEvents {
    public class UpcomingMatchEvent : IBaseEvent {
        public MatchCreateRequest details;

        public UpcomingMatchEvent (MatchCreateRequest fullMatchResponse) {
            this.details = fullMatchResponse;
        }
    }
}
