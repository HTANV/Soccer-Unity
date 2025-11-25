using FStudio.Events;
using UnityEngine;

namespace FStudio.MatchEngine.Events {
    public class OutEvent : IBaseEvent {
        public readonly int Part;

        public OutEvent(int part) {
            this.Part = part;
        }
    }
}
