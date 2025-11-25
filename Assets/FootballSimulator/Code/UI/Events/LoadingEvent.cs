using FStudio.Events;

namespace FStudio.UI.Events {
    public class LoadingEvent : IBaseEvent {
        public readonly string Message;

        public LoadingEvent(string message) {
            Message = message;
        }

        public LoadingEvent () {
            Message = null;
        }
    }
}
