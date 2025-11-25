using UnityEngine;

namespace Unity.PlasticSCM.Editor.UI
{
    internal static class DrawUserIcon
    {
        internal static void ForPendingChangesTab()
        {
            Rect rect = BuildUserIconAreaRect(35f);

            GUI.DrawTexture(rect, Images.GetEmptyGravatar());
        }

        static Rect BuildUserIconAreaRect(float sizeOfImage)
        {
            GUIStyle commentTextAreaStyle = UnityStyles.PendingChangesTab.CommentTextArea;

            Rect result = GUILayoutUtility.GetRect(sizeOfImage, sizeOfImage); // Needs to be a square
            result.x = commentTextAreaStyle.margin.left;

            return result;
        }
    }
}
