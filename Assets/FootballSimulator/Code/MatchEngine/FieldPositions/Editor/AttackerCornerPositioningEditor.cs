using UnityEditor;

namespace FStudio.MatchEngine.FieldPositions {
    [CustomEditor(typeof(AttackerCornerPositioning))]
    public class AttackerCornerPositioningEditor : Editor {
        public override void OnInspectorGUI() {
            var script = (AttackerCornerPositioning)target;

            BasePositionsDataEditor.OnInspectorGUI(script);

            Repaint();

            base.OnInspectorGUI();
        }
    }
}

