using UnityEngine;
using UnityEditor;
using FStudio.MatchEngine.Players;

namespace FStudio.MatchEngine {
    [CustomEditor(typeof(GoalNet))]
    [CanEditMultipleObjects]
    public class GoalNetEditor : Editor {
        public override void OnInspectorGUI() {
            var script = (GoalNet)target;

            base.OnInspectorGUI();
        }
    }
}

