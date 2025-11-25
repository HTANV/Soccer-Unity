using FStudio.Data;
using FStudio.Scriptables;
using System.Linq;
using UnityEngine;

namespace FStudio.Database.Rules {
    public class PositionSkillRules : SerializedSingletonScriptable<PositionSkillRules> {
        [SerializeField] private SkillSchema[] PreferredSkillsForOverallCalc;
        
        public PlayerEntry GetDefaultSkillSchema (Positions position) {
            return PreferredSkillsForOverallCalc.FirstOrDefault (x => x.Position.HasFlag(position));
        }
    }
}
