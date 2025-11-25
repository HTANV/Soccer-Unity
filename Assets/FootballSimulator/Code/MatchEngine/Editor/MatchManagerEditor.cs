using UnityEngine;
using UnityEditor;

namespace FStudio.MatchEngine {
    [CustomEditor(typeof(MatchManager))]
    [CanEditMultipleObjects]
    public class MatchManagerEditor : Editor {
        public override void OnInspectorGUI() {
            var script = (MatchManager)target;

            GUILayout.Label("Match Engine Editor");

            for (int i=0; i<4;i++) {
                if (GUILayout.Button(string.Format ("Create Corner with Index {0}", i))) {
                    script.Corner(i);
                }
            }

            for (int i = 0; i < 4; i++) {
                if (GUILayout.Button(string.Format("Create GoalKick with Index {0}", i))) {
                    script.GoalKick(i);
                }
            }

            base.OnInspectorGUI();
        }
    }
}

