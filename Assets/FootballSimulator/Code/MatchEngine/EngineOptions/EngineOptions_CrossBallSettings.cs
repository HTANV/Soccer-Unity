

using FStudio.Scriptables;
using System;
using UnityEngine;

namespace FStudio.MatchEngine.EngineOptions {
    [Serializable]
    public class EngineOptions_CrossBallSettings : SerializedSingletonScriptable<EngineOptions_CrossBallSettings> {
        [SerializeField] private AnimationCurve distanceSubtractor;

        [SerializeField] private float dragMinusPerDistance;

        public Vector3 CrossPoint (Vector3 start, Vector3 end) {
            var dir = (end - start);
            var dist = dir.magnitude;
            var m_distanceSubtractor = distanceSubtractor.Evaluate(dist);

            end -= dir.normalized * m_distanceSubtractor;

            return end;
        }

        public Vector3 TargetPointForDrag (Vector3 start, Vector3 end) {
            var dir = (end - start);
            var dist = dir.magnitude;

            end -= dir.normalized * dist * dragMinusPerDistance;

            return end;
        }
    }
}
