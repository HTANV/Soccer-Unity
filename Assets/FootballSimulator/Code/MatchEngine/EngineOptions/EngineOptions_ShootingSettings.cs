

using FStudio.Scriptables;
using System;
using UnityEngine;

namespace FStudio.MatchEngine.EngineOptions {
    [Serializable]
    public class EngineOptions_ShootingSettings : SerializedSingletonScriptable<EngineOptions_ShootingSettings> {
        public AnimationCurve shootPowerModByAngleFree;
    }
}
