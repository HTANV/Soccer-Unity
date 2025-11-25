using UnityEditor;

namespace FStudio.MatchEngine.FieldPositions {
    [CustomEditor(typeof(KickOffPositioning_Starter))]
    public class KickOffPositioning_StarterEditor : Editor {
        public override void OnInspectorGUI() {
            var script = (KickOffPositioning_Starter)target;

            BasePositionsDataEditor.OnInspectorGUI(script);

            Repaint();

            base.OnInspectorGUI();
        }
    }
}

