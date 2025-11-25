using UnityEditor;
using UnityEngine;

namespace FStudio.Database {
    [CustomEditor(typeof(PlayerEntry))]
    [CanEditMultipleObjects]
    public class PlayerEntryEditor : Editor {
        public override void OnInspectorGUI() {
            var t = target as PlayerEntry;
            GUILayout.Label($"Overall {t.Overall}");

            base.OnInspectorGUI();
        }
    }
}
