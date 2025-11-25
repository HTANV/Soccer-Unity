using FStudio.Events;

namespace FStudio.MatchEngine.Events {
    public struct GameTimeEvent : IBaseEvent {
        public readonly float GameTime;

        public GameTimeEvent (float gameTime) {
            this.GameTime = gameTime;
        }
    }
}
