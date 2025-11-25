using UnityEditor;

namespace FStudio.MatchEngine.FieldPositions {
    [CustomEditor(typeof(FormalPositioning))]
    public class FormalPositioningEditor : Editor {
        public override void OnInspectorGUI() {
            var script = (FormalPositioning)target;

            BasePositionsDataEditor.OnInspectorGUI(script);

            Repaint();

            base.OnInspectorGUI();
        }
    }
}

