
using FStudio.Data;
using UnityEngine;

namespace FStudio.Database {
    public class SkillSchema : PlayerEntry {
        [SerializeField] private Positions position;
        public override Positions Position => position;
    }
}
