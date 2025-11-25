using UnityEngine;

using UnityEditor;

using System;

using FStudio.Loaders;

namespace FStudio.MatchEngine.FieldPositions {
    public class BasePositionsDataEditor {
        private const string FieldTextureName = "BasePositionDataEditor/InspectorField";
        private const string CircleTextureName = "BasePositionDataEditor/PlayerCircle";
        private const string GUISkinName = "BasePositionDataEditor/GUISkin";

        private const float CircleSize = 0.05f;

        private static Vector2 holdMessageOffset = new Vector2(50, 30);
        private static Vector2 holdMessageSize = new Vector2(100, 50);

        private static ResourceLoader<Texture2D> textureLoader = new ResourceLoader<Texture2D>();
        private static ResourceLoader<GUISkin> guiSkinLoader = new ResourceLoader<GUISkin>();

        private static int targetDraggable = -1;

        public static void OnInspectorGUI<T>(BasePositionsData<T> script) where T : BasePositionsData<T> {
            var positions = Enum.GetValues(typeof(FStudio.Data.Positions));
            var positionsCount = positions.Length- 1;

            if (script.FieldPositions == null) {
                script.FieldPositions = new FieldPosition[0];
            }

            var length = script.FieldPositions.Length;

            if (length != positionsCount && script.FixedLength) {
                Debug.LogWarning("You cannot change the number of behaviours since it's fixed by Position enum.");
                Array.Resize(ref script.FieldPositions, positionsCount);

                for (int i = 0; i < positionsCount; i++) {
                    if (string.IsNullOrEmpty (script.FieldPositions[i].Name)) {
                        script.FieldPositions[i] = new FieldPosition((FStudio.Data.Positions)positions.GetValue (i));

                        script.FieldPositions[i].Name = positions.GetValue (i).ToString();
                    }
                }
            }

            // fix enum changes.
            for (int i = 0; i < length; i++) {
                script.FieldPositions[i].Position = (FStudio.Data.Positions)positions.GetValue(i);
                script.FieldPositions[i].Name = script.FieldPositions[i].Position.ToString();
            }
            //

            GUI.skin = guiSkinLoader.Get(GUISkinName);

            var width = Screen.width - 200;
            var height = width / 1.416f;

            // draw field image
            GUI.DrawTexture(new Rect(50, 0, width, height), textureLoader.Get(FieldTextureName));

            Vector2 screenMousePosition = Event.current.mousePosition;

            var circleRadius = width * CircleSize;

            var e = Event.current;

            int index = 0;
            foreach (var fieldPosition in script.FieldPositions) {
                var rect = new Rect(50 +
                    fieldPosition.VerticalPlacement * width - circleRadius / 2,
                    height - (fieldPosition.HorizontalPlacement * height) - circleRadius / 2,
                    circleRadius,
                    circleRadius);

                GUI.DrawTexture(rect,
                    textureLoader.Get(CircleTextureName));

                GUI.Label(rect, positions.GetValue(index).ToString());

                if (targetDraggable == -1 && rect.Contains(screenMousePosition)) {
                    GUI.Label(new Rect(rect.position+ holdMessageOffset, holdMessageSize), "Click & hold to drag");

                    if (e.type == EventType.MouseDown) {
                        targetDraggable = index;
                        Undo.RegisterCompleteObjectUndo(script, script.name);
                    }
                }

                index++;
            }

            if (e.type == EventType.MouseUp) {
                targetDraggable = -1;
            }

            if (targetDraggable != -1) {
                float newX = screenMousePosition.x / (width + circleRadius);
                float newY = screenMousePosition.y / (height - circleRadius / 2);

                var pos = new FieldPosition(script.FieldPositions[targetDraggable]);

                pos.VerticalPlacement = newX;
                pos.HorizontalPlacement = 1 - newY;

                script.FieldPositions[targetDraggable] = pos;
            }

            GUILayout.Space(height + 10);

            script.FixedLength = GUILayout.Toggle(script.FixedLength, "Is length fixed?");

            GUILayout.BeginHorizontal();
            // put a button to compress / decomress.
            if (GUILayout.Button ("Compress Horizontal")) {
                Undo.RegisterCompleteObjectUndo(script, script.name);
                pushHorizontal(-0.1f);
            }

            if (GUILayout.Button("Decompress Horizontal")) {
                Undo.RegisterCompleteObjectUndo(script, script.name);
                pushHorizontal(0.1f);
            }

            if (GUILayout.Button("Compress Vertical")) {
                Undo.RegisterCompleteObjectUndo(script, script.name);
                pushVertical(-0.1f);
            }

            if (GUILayout.Button("Decompress Vertical")) {
                Undo.RegisterCompleteObjectUndo(script, script.name);
                pushVertical(0.1f);
            }

            void pushVertical (float value) {
                int length = script.FieldPositions.Length;
                for (int i=0; i<length;i++) {
                    if (script.FieldPositions[i].Position != FStudio.Data.Positions.GK) {
                        var pos = new FieldPosition(script.FieldPositions[i]);
                        pos.VerticalPlacement += pos.VerticalPlacement * value;
                        script.FieldPositions[i] = pos;
                    }
                }

                EditorUtility.SetDirty(script);
            }

            void pushHorizontal (float value) {
                int length = script.FieldPositions.Length;
                for (int i = 0; i < length; i++) {
                    if (script.FieldPositions[i].Position != FStudio.Data.Positions.GK) {
                        var distToPoint5 = script.FieldPositions[i].HorizontalPlacement - 0.5f;

                        var pos = new FieldPosition(script.FieldPositions[i]);
                        pos.HorizontalPlacement += distToPoint5 * value;
                        script.FieldPositions[i] = pos;
                    }
                }

                EditorUtility.SetDirty(script);
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button ("Set Dirty")) {
                EditorUtility.SetDirty(script);
            }
        }
    }
}

