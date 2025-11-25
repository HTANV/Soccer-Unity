
using System.Collections.Generic;
using UnityEngine;
using static FStudio.MatchEngine.Players.PlayerBase;

namespace FStudio.MatchEngine {
    [System.Serializable]
    public class ThroughtPassOption {
        public string Name;

        public bool DisableCrossing;

        public bool EnableUserInput;

        public float TakerVelocityMultiplier;

        [Range (0,1)]
        public float ToTargetGoalNetOnXAxis;

        [Range(0, 1)]
        public float ToTargetGoalNetOnZAxis;

        [Range(0, 1)]
        public float ToAttackMultiplier;

        public int Priority;

        public bool OnlyWhenRunningForward;

        public bool IsEnabled;

        public List<PassType> PassTypes = new List<PassType>() { PassType.ShortPass, PassType.LongPass };

        public Color DebugColor;
    }

    public struct ThroughtPassPoint {
        public readonly Vector3 Position;
        public readonly int Priority;
        public readonly bool EnableUserInput;
        public readonly bool OnlyWhenRunningForward;
        public readonly bool DisableCrossing;
        public readonly string OptionName;
        public readonly Color DebugColor;
        public readonly List<PassType> PassTypes;

        public ThroughtPassPoint(ThroughtPassOption option,
            Vector3 position,
            Vector3 toVelocity,
            Vector3 targetGoalNet) {

            this.OptionName = option.Name;

            this.DisableCrossing = option.DisableCrossing;

            this.EnableUserInput = option.EnableUserInput;

            this.PassTypes = option.PassTypes;

            position.x = Mathf.Lerp(position.x, targetGoalNet.x, option.ToTargetGoalNetOnXAxis);
            position.z = Mathf.Lerp(position.z, targetGoalNet.z, option.ToTargetGoalNetOnZAxis);

            this.OnlyWhenRunningForward = option.OnlyWhenRunningForward;

            this.Position = Vector3.Lerp (
                position + toVelocity * option.TakerVelocityMultiplier,
                targetGoalNet, option.ToAttackMultiplier);

            this.Priority = option.Priority;

            DebugColor = option.DebugColor;
        }
    }
}
