using FStudio.Scriptables;

using System;

using UnityEngine;

namespace FStudio.MatchEngine {
    [Serializable]
    public class GKSettings : SerializedSingletonScriptable<GKSettings> {
        public AnimationCurve shieldSideLengthCurve;
        public AnimationCurve shieldForwardRadiusCurve;
        public AnimationCurve shieldSideByForwardCurve;

        public (float forward, float side) GKPosition(
            Vector3 ballPosition,
            in float teamBallProgress,
            in float fieldYSize) {

            var ballSideProgress = ballPosition.z / fieldYSize;

            var toForward = shieldForwardRadiusCurve.Evaluate(teamBallProgress);
            var toSide = 
                shieldSideLengthCurve.Evaluate(ballSideProgress) * 
                shieldSideByForwardCurve.Evaluate(teamBallProgress) + fieldYSize / 2;

            return (toForward, toSide);
        }
    }
}
