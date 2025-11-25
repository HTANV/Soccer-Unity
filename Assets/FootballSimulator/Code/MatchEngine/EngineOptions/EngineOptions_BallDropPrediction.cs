using FStudio.Scriptables;
using System;
using UnityEngine;

namespace FStudio.MatchEngine.EngineOptions {
    [Serializable]
    public class EngineOptions_BallDropPrediction : SerializedSingletonScriptable<EngineOptions_BallDropPrediction> {
        [Header("Ball drop prediction")]
        public float BallPrediction_Velocity_Pow = 1.55f;
        public float BallPrediction_Velocity_Mod = 0.22f;
        public float BallPrediction_Velocity_Per_Distance_Mod = -0.01f;
        public float BallPrediction_Angle_Pow = 0.9f;
        public float BallPrediction_Angle_Mod = 0.0009f;
        public AnimationCurve BallPrediction_Angle_Effect_DistanceCurve;
        public AnimationCurve RelaxationToVelocityCurve;

        [Header("Ball drop prediction Y Mod")]
        public float BallPrediction_Velocity_YPow = 0.75f;
        public float BallPrediction_Velocity_YMod = 0.425f;
        public float BallPrediction_Velocity_Height_Mod_As_Minus = 0.08f;
    }
}
