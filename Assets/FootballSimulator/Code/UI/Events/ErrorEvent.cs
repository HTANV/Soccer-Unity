using FStudio.Events;

namespace FStudio.UI.Events {
    public class ErrorEvent : IBaseEvent {
        public readonly string Error;

        public ErrorEvent (string error) {
            Error = error;
        }
    }
}
