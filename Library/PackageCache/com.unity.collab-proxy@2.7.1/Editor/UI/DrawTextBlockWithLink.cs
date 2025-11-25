using System;
using System.Text.RegularExpressions;

using UnityEditor;
using UnityEngine;

using Codice.Client.Common;

namespace Unity.PlasticSCM.Editor.UI
{
    internal static class DrawTextBlockWithLink
    {
        internal static void ForExternalLink(
            ExternalLink externalLink,
            string explanation,
            GUIStyle textBlockStyle)
        {
            GUILayout.Label(explanation, textBlockStyle);

            GUIStyle linkStyle = new GUIStyle(UnityStyles.LinkLabel);
            linkStyle.fontSize = textBlockStyle.fontSize;
            linkStyle.stretchWidth = false;

            if (GUILayout.Button(externalLink.Label, linkStyle))
                Application.OpenURL(externalLink.Url);

            EditorGUIUtility.AddCursorRect(
                GUILayoutUtility.GetLastRect(), MouseCursor.Link);
        }

        internal static void ForMultiLinkLabel(MultiLinkLabelData data)
        {
            GUIStyle labelStyle = new GUIStyle(UnityStyles.Paragraph);
            labelStyle.margin = labelStyle.padding = new RectOffset(0, 0, 0, 0);

            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                GUILayout.Label(string.Format(data.Text, data.LinkNames.ToArray()), labelStyle);
                return;
            }

            string[] labels = Regex.Split(data.Text, @"\{\d+\}");

            GUIStyle linkStyle = new GUIStyle(UnityStyles.LinkLabel);
            linkStyle.fontSize = labelStyle.fontSize;
            linkStyle.stretchWidth = false;
            linkStyle.margin = linkStyle.padding = new RectOffset(0, 0, 0, 0);

            using (new EditorGUILayout.HorizontalScope())
            {
                for (int i = 0; i < labels.Length; i++)
                {
                    GUILayout.Label(labels[i], labelStyle);

                    if (data.LinkNames.Count <= i)
                        break;

                    bool buttonResult = GUILayout.Button(data.LinkNames[i], linkStyle);

                    EditorGUIUtility.AddCursorRect(
                        GUILayoutUtility.GetLastRect(), MouseCursor.Link);

                    if (buttonResult)
                        ((Action)data.LinkActions[i]).Invoke();
                }

                GUILayout.FlexibleSpace();
            }
        }
    }
}
