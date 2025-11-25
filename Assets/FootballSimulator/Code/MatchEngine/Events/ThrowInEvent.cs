using FStudio.Events;
using UnityEngine;

namespace FStudio.MatchEngine.Events {
    public class ThrowInEvent : IBaseEvent {
        public readonly Vector3 Position;

        public ThrowInEvent (Vector3 position) {
            this.Position = position;
        }
    }
}
