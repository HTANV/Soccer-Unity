using FStudio.Events;
using System;

namespace FStudio.MatchEngine.Statistics {
    public class Corners : IDisposable {
        public readonly int[] CornerCount = new int[2];
        public Corners () {
            EventManager.Subscribe<Events.CornerEvent>(CornerEvent);
        }

        public void Dispose() {
            EventManager.UnSubscribe<Events.CornerEvent>(CornerEvent);
        }

        private void CornerEvent(Events.CornerEvent cornerEvent) {
            CornerCount[cornerEvent.Part < 2 ? 1 : 0]++;
        }
    }
}
