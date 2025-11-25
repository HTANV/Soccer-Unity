using FStudio.Database;
using FStudio.Loaders;
using FStudio.MatchEngine.FieldPositions;
using FStudio.UI.Graphics;
using FStudio.Data;
using UnityEditor;
using UnityEngine;

namespace FStudio.Database {
    [CustomEditor(typeof(TeamEntry))]
    public class TeamEntryEditor : Editor {
        private const string FieldTextureName = "FormationPreview/InspectorField";
        private const string GUISkinName = "FormationPreview/GUISkin";

        private ResourceLoader<Texture2D> textureLoader = new ResourceLoader<Texture2D>();
        private ResourceLoader<GUISkin> guiSkinLoader = new ResourceLoader<GUISkin>();

        private int currentSelectedPlayerIndex = -1;

        private TeamEntry teamEntry;

        public override void OnInspectorGUI() {
            teamEntry = target as TeamEntry;

            if (!teamEntry.IsValid) {
                if (GUILayout.Button("Initialize")) {
                    teamEntry.Initialize();
                }
                return;
            }

            GUILayout.Label("Team Overall => " + teamEntry.Overall);

            base.OnInspectorGUI();

            DrawFormation();

            GUILayout.Space(Screen.height);

            Repaint();
        }

        public void DrawFormation() {
            var formation = teamEntry.Formation;
            var positions = FormationRules.GetTeamFormation(formation).Positions;

            GUI.skin = guiSkinLoader.Get(GUISkinName);

            var width = Screen.width / 1.5f;
            var height = Screen.height / 2.25f;

            var yStartOffset = 80;

            // draw field image
            GUI.DrawTexture(new Rect(0, yStartOffset, width, height), textureLoader.Get(FieldTextureName));

            var circleSize = new Vector2(50, 20);

            float xOffset = width / 10f;

            var offWidth = width - xOffset * 2;
            int index = 0;
            foreach (var position in positions) {
                var fieldPosition = FormalPositioning.Current.GetPosition(position);

                var rect = new Rect(
                    xOffset + offWidth - (fieldPosition.HorizontalPlacement * offWidth) - circleSize.x / 2,
                    yStartOffset + height - (fieldPosition.VerticalPlacement * height) - circleSize.y / 2,
                    circleSize.x,
                    circleSize.y);

                if (currentSelectedPlayerIndex != -1 && currentSelectedPlayerIndex == index) {
                    var cancelRect = new Rect(rect.position, rect.size);
                    cancelRect.position += cancelRect.size;
                    cancelRect.size = new Vector2(cancelRect.size.y, cancelRect.size.y);

                    if (GUI.Button(cancelRect, "X")) {
                        currentSelectedPlayerIndex = -1;
                    }

                    GUI.color = Color.yellow;
                    GUI.color = Color.white;
                }

                if (GUI.Button (rect, positions.GetValue(index).ToString())) {
                    if (currentSelectedPlayerIndex == -1) {
                        currentSelectedPlayerIndex = index;
                    } else if (currentSelectedPlayerIndex != -1) {
                        // replace.
                        var currentPlayer = teamEntry.Players[currentSelectedPlayerIndex];
                        teamEntry.Players[currentSelectedPlayerIndex] = teamEntry.Players[index];
                        teamEntry.Players[index] = currentPlayer;

                        currentSelectedPlayerIndex = -1;

                        Undo.RegisterCompleteObjectUndo(teamEntry, teamEntry.name);
                        EditorUtility.SetDirty(teamEntry);
                        AssetDatabase.SaveAssets();
                        return;
                    }
                }

                var nameRect = new Rect(rect.position, rect.size);
                nameRect.position -= new Vector2(30, 0);
                nameRect.size += new Vector2(60, 30);

                var name = teamEntry.Players[index].Name ?? "Unnamed";

                GUI.Label(nameRect, name.ToString());
                
                index++;
            }

            GUILayout.Height(1000);
        }
    }
}
