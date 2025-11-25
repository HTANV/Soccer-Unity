using UnityEditor;
using UnityEngine;

namespace FStudio.Database {
    [CustomEditor(typeof(SkillSchema))]
    [CanEditMultipleObjects]
    public class SkillSchemaEditor : Editor {
        SerializedProperty position;
        SerializedProperty strength;
        SerializedProperty acceleration;
        SerializedProperty topSpeed;
        SerializedProperty dribbleSpeed;
        SerializedProperty jump;
        SerializedProperty tackling;
        SerializedProperty ballKeeping;
        SerializedProperty passing;
        SerializedProperty longBall;
        SerializedProperty agility;
        SerializedProperty shooting;
        SerializedProperty shootPower;
        SerializedProperty positioning;
        SerializedProperty reaction;
        SerializedProperty ballControl;
        void OnEnable() {
            position =                  serializedObject.FindProperty("position");
            strength =                  serializedObject.FindProperty("strength");
            acceleration=               serializedObject.FindProperty("acceleration");
            topSpeed=                   serializedObject.FindProperty("topSpeed");
            dribbleSpeed=               serializedObject.FindProperty("dribbleSpeed");
            jump=                       serializedObject.FindProperty("jump");
            tackling=                   serializedObject.FindProperty("tackling");
            ballKeeping=                serializedObject.FindProperty("ballKeeping");
            passing=                    serializedObject.FindProperty("passing");
            longBall=                   serializedObject.FindProperty("longBall");
            agility=                    serializedObject.FindProperty("agility");
            shooting=                   serializedObject.FindProperty("shooting");
            shootPower=                 serializedObject.FindProperty("shootPower");
            positioning=                serializedObject.FindProperty("positioning");
            reaction=                   serializedObject.FindProperty("reaction");
            ballControl =               serializedObject.FindProperty("ballControl");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.PropertyField(position);

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            EditorGUILayout.PropertyField(strength);
            EditorGUILayout.PropertyField(acceleration);
            EditorGUILayout.PropertyField(topSpeed);
            EditorGUILayout.PropertyField(dribbleSpeed);
            EditorGUILayout.PropertyField(jump);
            EditorGUILayout.PropertyField(tackling);
            EditorGUILayout.PropertyField(ballKeeping);
            EditorGUILayout.PropertyField(passing);
            EditorGUILayout.PropertyField(longBall);
            EditorGUILayout.PropertyField(agility);
            EditorGUILayout.PropertyField(shooting);
            EditorGUILayout.PropertyField(shootPower);
            EditorGUILayout.PropertyField(positioning);
            EditorGUILayout.PropertyField(reaction);
            EditorGUILayout.PropertyField(ballControl);


            serializedObject.ApplyModifiedProperties();
        }
    }
}
