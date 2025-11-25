
namespace FStudio.MatchEngine.Enums {
    [System.Flags]
    public enum MatchStatus {
        NotPlaying = 1<<0,
        WaitingForKickOff = 1 << 1,
        Playing = 1 << 2 ,
        Freeze = 1 << 3, 
        Special = 1 << 4
    }
}
