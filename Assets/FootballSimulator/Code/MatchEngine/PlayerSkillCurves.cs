using UnityEngine;

using System;

using FStudio.Scriptables;

namespace FStudio.MatchEngine {
    /// <summary>
    /// Player skills will be rearranged by these curves.
    /// <para>Lets say a player has 60 dribble speed skill, but you don't want to see a much difference with a player with 80 dribble speed skill. So you can arrange the curve to make the gameplay better.</para>
    /// </summary>
    [Serializable]
    public class PlayerSkillCurves : SerializedSingletonScriptable<PlayerSkillCurves> {
        public AnimationCurve SpeedCurve;
        public AnimationCurve PassingCurve;
        public AnimationCurve ShootingCurve;
        public AnimationCurve DribbleSpeedCurve;
        public AnimationCurve AgilityCurve;
        public AnimationCurve StrengthCurve;
        public AnimationCurve TacklingCurve;
        public AnimationCurve PositioningCurve;
        public AnimationCurve ReactionCurve;
        public AnimationCurve BallControlCurve;
        public AnimationCurve JumpingCurve;
    }
}
