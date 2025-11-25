using FStudio.Events;
using UnityEngine;

namespace FStudio.MatchEngine.Events {
    public class CornerEvent : IBaseEvent {
        public readonly int Part;

        public CornerEvent(int part) {
            this.Part = part;
        }
    }
}
