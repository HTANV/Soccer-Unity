

using FStudio.Scriptables;
using System;
using UnityEngine;

namespace FStudio.MatchEngine.EngineOptions {
    [Serializable]
    public class EngineOptions_BallDropPredictionForCrosses : SerializedSingletonScriptable<EngineOptions_BallDropPredictionForCrosses> {
        [Header("Ball drop prediction")]
        public float Angle_Pow = 2;
        public float Angle_Mod = 0.01f;
        public float DistToAngle_Pow = 1.2f;

        public float CrossHeight_Pow = 0.01f;
        public float CrossHeight_Mod = 1;

        public float DistanceToBallPow = 1;
        public float DistanceToBallMod = 1;

        public float PlayerVelocityDivider = 7f;
        public float PlayerVelocityMul = 2f;

        [Header("Ball drop prediction Height Mod")]
        public float Velocity_Height_Mod_As_Minus = 0.1f;
    }
}
