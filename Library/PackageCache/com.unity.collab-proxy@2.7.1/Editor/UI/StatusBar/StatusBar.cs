using UnityEditor;
using UnityEngine;

using PlasticGui;
using PlasticGui.WorkspaceWindow;
using Unity.PlasticSCM.Editor.Developer;

namespace Unity.PlasticSCM.Editor.UI.StatusBar
{
    internal class StatusBar
    {
        internal interface IIncomingChangesNotification
        {
            bool HasNotification { get; }
            void OnGUI();
        }

        internal class IncomingChangesNotificationData
        {
            internal string InfoText { get; private set; }
            internal string ActionText { get; private set; }
            internal string TooltipText { get; private set; }
            internal bool HasUpdateAction { get; private set; }
            internal PlasticNotification.Status Status { get; private set; }

            internal void UpdateData(
                string infoText,
                string actionText,
                string tooltipText,
                bool hasUpdateAction,
                PlasticNotification.Status status)
            {
                InfoText = infoText;
                ActionText = actionText;
                TooltipText = tooltipText;
                HasUpdateAction = hasUpdateAction;
                Status = status;
            }

            internal void Clear()
            {
                InfoText = string.Empty;
                ActionText = string.Empty;
                TooltipText = string.Empty;
                HasUpdateAction = false;
                Status = PlasticNotification.Status.None;
            }
        }

        internal interface IShelvedChangesNotification :
            CheckShelvedChanges.IUpdateShelvedChangesNotification
        {
            bool HasNotification { get; }
            void SetWorkspaceWindow(
                WorkspaceWindow workspaceWindow);
            void SetShelvedChangesUpdater(
                IShelvedChangesUpdater shelvedChangesUpdater);
            void OnGUI();
        }

        internal void Notify(string message, MessageType type, Texture2D image)
        {
            mNotification = new Notification(message, type, image);
            mCooldownNotificationClearAction.Ping();
        }
        internal NotificationBar NotificationBar { get; private set; }

        internal StatusBar()
        {
            mCooldownNotificationClearAction = new CooldownWindowDelayer(
                DelayedClearNotification,
                UnityConstants.NOTIFICATION_CLEAR_INTERVAL);

            NotificationBar = new NotificationBar();
        }

        internal void Initialize(
            WorkspaceWindow workspaceWindow,
            IIncomingChangesNotification incomingChangesNotification,
            IShelvedChangesNotification shelvedChangesNotification)
        {
            mWorkspaceWindow = workspaceWindow;
            mIncomingChangesNotification = incomingChangesNotification;
            mShelvedChangesNotification = shelvedChangesNotification;

            if (incomingChangesNotification is IncomingChangesNotification)
                ((IncomingChangesNotification)incomingChangesNotification).SetWorkspaceWindow(workspaceWindow);

            shelvedChangesNotification.SetWorkspaceWindow(workspaceWindow);
        }

        void DelayedClearNotification()
        {
            mNotification = null;
        }

        internal void OnGUI()
        {
            if (NotificationBar.HasNotification &&
                NotificationBar.IsVisible)
            {
                BeginDrawBar();
                NotificationBar.OnGUI();
                EndDrawBar();
            }

            BeginDrawBar();

            if (NotificationBar.HasNotification)
            {
                DrawNotificationAvailablePanel(NotificationBar);
            }

            if (mIncomingChangesNotification.HasNotification)
            {
                mIncomingChangesNotification.OnGUI();
            }

            if (mShelvedChangesNotification.HasNotification)
            {
                if (mIncomingChangesNotification.HasNotification)
                    EditorGUILayout.Space(15);

                mShelvedChangesNotification.OnGUI();
            }

            if (mNotification != null)
                DrawNotification(mNotification);

            GUILayout.FlexibleSpace();

            DrawWorkspaceStatus(mWorkspaceWindow);

            EndDrawBar();
        }

        internal static void DrawNotificationLabel(GUIContent label)
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUILayout.Label(
                label,
                UnityStyles.StatusBar.NotificationLabel);

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        internal static bool DrawButton(GUIContent content)
        {
            GUIStyle buttonStyle = new GUIStyle(EditorStyles.miniButton);

            Rect rt = GUILayoutUtility.GetRect(
                content,
                buttonStyle,
                GUILayout.Width(60));

            return GUI.Button(
                rt,
                content,
                buttonStyle);
        }

        static void DrawNotificationAvailablePanel(
            NotificationBar notificationBar)
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(PlasticLocalization.GetString(
                    notificationBar.IsVisible ?
                        PlasticLocalization.Name.HideNotification :
                        PlasticLocalization.Name.ShowNotification)))
            {
                notificationBar.SetVisibility(!notificationBar.IsVisible);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        static void DrawNotification(Notification notification)
        {
            DrawIcon(notification.Image);
            DrawNotificationLabel(new GUIContent(notification.Message));
        }

        static void DrawWorkspaceStatus(WorkspaceWindow workspaceWindow)
        {
            DrawIcon(Images.GetBranchIcon());

            if (workspaceWindow.WorkspaceStatus == null)
                return;

            DrawLabel(string.Format(
                "{0}@{1}@{2}",
                workspaceWindow.WorkspaceStatus.ObjectSpec,
                workspaceWindow.WorkspaceStatus.RepositoryName,
                workspaceWindow.ServerDisplayName));
        }

        internal static void DrawIcon(Texture2D icon, int size = UnityConstants.STATUS_BAR_ICON_SIZE)
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUILayout.Label(
                icon,
                UnityStyles.StatusBar.Icon,
                GUILayout.Height(size),
                GUILayout.Width(size));

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        static void DrawLabel(string label)
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            GUILayout.Label(
                label,
                UnityStyles.StatusBar.Label);

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        static void BeginDrawBar()
        {
            EditorGUILayout.BeginVertical(
                GetBarStyle(),
                GUILayout.Height(UnityConstants.STATUS_BAR_HEIGHT));
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal();
        }

        static void EndDrawBar()
        {
            EditorGUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();
        }

        static GUIStyle GetBarStyle()
        {
            if (sBarTexture == null)
                sBarTexture = new Texture2D(1, 1);

            if (sBarStyle == null)
                sBarStyle = new GUIStyle();

            sBarTexture.SetPixel(0, 0, UnityStyles.Colors.BackgroundBar);
            sBarTexture.Apply();
            sBarStyle.normal.background = sBarTexture;

            return sBarStyle;
        }

        class Notification
        {
            internal string Message { get; private set; }
            internal MessageType MessageType { get; private set; }
            internal Texture2D Image { get; private set; }

            internal Notification(string message, MessageType messageType, Texture2D image)
            {
                Message = message;
                MessageType = messageType;
                Image = image;
            }
        }

        Notification mNotification;
        WorkspaceWindow mWorkspaceWindow;
        IIncomingChangesNotification mIncomingChangesNotification;
        IShelvedChangesNotification mShelvedChangesNotification;

        readonly CooldownWindowDelayer mCooldownNotificationClearAction;

        static Texture2D sBarTexture;
        static GUIStyle sBarStyle;
    }
}
