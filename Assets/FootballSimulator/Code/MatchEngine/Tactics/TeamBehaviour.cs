
namespace FStudio.MatchEngine.Tactics {
    /// <summary>
    /// Team events.
    /// </summary>
    public enum TeamBehaviour {
        Defending,
        // Defending means you don't have the ball and trying to get the ball.
        Attacking,
        // Attacking means you have the ball, and the players will get closer to the ball and move forward by the ball.
        BallChasing,
        // Ball is free at the moment.
        WaitingForTeamGK,
        // Our keeper has the ball.
        WaitingForOpponentGK,
        // OpponentGK has the ball.
        TeamThrowIn,
        OpponentThrowIn,
        ParameterCount
    }
}
