namespace FStudio.MatchEngine.Players.PlayerController {
    public enum PlayerAnimatorVariable {
        Horizontal,
        Vertical,
        MoveSpeed,
        /// <summary>
        /// Speed of all actions like passing, shooting.
        /// </summary>
        Agility,
        IsHoldingBall,
        Pass_R,
        LongBall_R,
        Shoot_R,
        Shoot_L,
        Tackling,
        Tackled,
        ThrowInIdle,
        IsHappy,
        Struggle,
        Header_R,
        Volley_R,
        Spin, // Dribblesuccess.
        GroundHeader_R,
        Throw_R,
        GKJumpLeft,
        GKJumpRight,
        GKMiss,
        GKBallSave_Low,
        GKDegage_R,

        // L footed
        Pass_L,
        LongBall_L,
        Header_L,
        GroundHeader_L,
        Volley_L,
        Throw_L,
        GKDegage_L,
        // 

        ParameterCount // Parameter count of the animator.
    }
}
    
    