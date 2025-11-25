using UnityEditor;

namespace FStudio.MatchEngine.FieldPositions {
    [CustomEditor(typeof(KickOffPositioning))]
    public class KickOffPositioningEditor : Editor {
        public override void OnInspectorGUI() {
            var script = (KickOffPositioning)target;

            BasePositionsDataEditor.OnInspectorGUI(script);

            Repaint();

            base.OnInspectorGUI();
        }
    }
}

