using UnityEditor;

namespace FStudio.MatchEngine.FieldPositions {
    [CustomEditor(typeof(DefenderCornerPositioning))]
    public class DefenderCornerPositioningEditor : Editor {
        public override void OnInspectorGUI() {
            var script = (DefenderCornerPositioning)target;

            BasePositionsDataEditor.OnInspectorGUI(script);

            Repaint();

            base.OnInspectorGUI();
        }
    }
}

