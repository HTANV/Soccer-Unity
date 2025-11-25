using UnityEditor;
using UnityEngine;

using Codice.CM.Common;
using PlasticGui.WorkspaceWindow.QueryViews.Changesets;
using PlasticGui;
using Unity.PlasticSCM.Editor.Tool;
using Unity.PlasticSCM.Editor.UI;

namespace Unity.PlasticSCM.Editor.Views.Changesets
{
    internal class ChangesetsViewMenu
    {
        internal GenericMenu Menu { get { return mMenu; } }

        public interface IMenuOperations
        {
            void DiffBranch();
            ChangesetExtendedInfo GetSelectedChangeset();
        }

        internal ChangesetsViewMenu(
            WorkspaceInfo wkInfo,
            IChangesetMenuOperations changesetMenuOperations,
            IMenuOperations menuOperations,
            LaunchTool.IShowDownloadPlasticExeWindow showDownloadPlasticExeWindow,
            bool isGluonMode)
        {
            mWkInfo = wkInfo;
            mChangesetMenuOperations = changesetMenuOperations;
            mMenuOperations = menuOperations;
            mShowDownloadPlasticExeWindow = showDownloadPlasticExeWindow;
            mIsGluonMode = isGluonMode;

            BuildComponents();
        }

        internal void Popup()
        {
            mMenu = new GenericMenu();

            UpdateMenuItems(mMenu);

            mMenu.ShowAsContext();
        }

        internal bool ProcessKeyActionIfNeeded(Event e)
        {
            int selectedChangesetsCount = mChangesetMenuOperations.GetSelectedChangesetsCount();

            ChangesetMenuOperations operationToExecute = GetMenuOperations(
                e, selectedChangesetsCount > 1);

            if (operationToExecute == ChangesetMenuOperations.None)
                return false;

            ChangesetMenuOperations operations = ChangesetMenuUpdater.GetAvailableMenuOperations(
                selectedChangesetsCount,
                mIsGluonMode,
                mMenuOperations.GetSelectedChangeset().BranchId,
                mLoadedBranchId,
                false);

            if (!operations.HasFlag(operationToExecute))
                return false;

            ProcessMenuOperation(operationToExecute);
            return true;
        }

        internal void SetLoadedBranchId(long loadedBranchId)
        {
            mLoadedBranchId = loadedBranchId;
        }

        void DiffChangesetMenuItem_Click()
        {
            mChangesetMenuOperations.DiffChangeset();
        }

        void DiffSelectedChangesetsMenuItem_Click()
        {
            mChangesetMenuOperations.DiffSelectedChangesets();
        }

        void RevertToChangesetMenuItem_Click()
        {
            mChangesetMenuOperations.RevertToChangeset();
        }

        void DiffBranchMenuItem_Click()
        {
            mMenuOperations.DiffBranch();
        }

        void SwitchToChangesetMenuItem_Click()
        {
            mChangesetMenuOperations.SwitchToChangeset();
        }

        void MergeChangesetMenuItem_Click()
        {
            mChangesetMenuOperations.MergeChangeset();
        }

        void CreateCodeReviewMenuItem_Click()
        {
            mChangesetMenuOperations.CreateCodeReview();
        }

        void UpdateMenuItems(GenericMenu menu)
        {
            ChangesetExtendedInfo singleSelectedChangeset = mMenuOperations.GetSelectedChangeset();

            ChangesetMenuOperations operations = ChangesetMenuUpdater.GetAvailableMenuOperations(
                mChangesetMenuOperations.GetSelectedChangesetsCount(),
                mIsGluonMode,
                singleSelectedChangeset.BranchId,
                mLoadedBranchId,
                false);

            AddDiffChangesetMenuItem(
                mDiffChangesetMenuItemContent,
                menu,
                singleSelectedChangeset,
                operations,
                DiffChangesetMenuItem_Click);

            AddChangesetsMenuItem(
                mDiffSelectedChangesetsMenuItemContent,
                menu,
                operations,
                ChangesetMenuOperations.DiffSelectedChangesets,
                DiffSelectedChangesetsMenuItem_Click);

            if (!IsOnMainBranch(singleSelectedChangeset))
            {
                menu.AddSeparator(string.Empty);

                AddDiffBranchMenuItem(
                    mDiffBranchMenuItemContent,
                    menu,
                    singleSelectedChangeset,
                    operations,
                    DiffBranchMenuItem_Click);
            }

            menu.AddSeparator(string.Empty);

            AddChangesetsMenuItem(
                mSwitchToChangesetMenuItemContent,
                menu,
                operations,
                ChangesetMenuOperations.SwitchToChangeset,
                SwitchToChangesetMenuItem_Click);

            if (!mIsGluonMode)
            {
                AddChangesetsMenuItem(
                    mRevertToChangesetMenuItemContent,
                    menu,
                    operations,
                    ChangesetMenuOperations.RevertToChangeset,
                    RevertToChangesetMenuItem_Click);

                menu.AddSeparator(string.Empty);

                AddChangesetsMenuItem(
                    mMergeChangesetMenuItemContent,
                    menu,
                    operations,
                    ChangesetMenuOperations.MergeChangeset,
                    MergeChangesetMenuItem_Click);
            }

            menu.AddSeparator(string.Empty);

            AddChangesetsMenuItem(
                mCreateCodeReviewMenuItemContent,
                menu,
                operations,
                ChangesetMenuOperations.CreateCodeReview,
                CreateCodeReviewMenuItem_Click);
        }

        void ProcessMenuOperation(
            ChangesetMenuOperations operationToExecute)
        {
            if (operationToExecute == ChangesetMenuOperations.DiffChangeset)
            {
                DiffChangesetMenuItem_Click();
                return;
            }

            if (operationToExecute == ChangesetMenuOperations.DiffSelectedChangesets)
            {
                DiffSelectedChangesetsMenuItem_Click();
                return;
            }

            if (operationToExecute == ChangesetMenuOperations.MergeChangeset)
            {
                MergeChangesetMenuItem_Click();
                return;
            }

            if (operationToExecute == ChangesetMenuOperations.CreateCodeReview)
            {
                CreateCodeReviewMenuItem_Click();
                return;
            }
        }

        static void AddChangesetsMenuItem(
            GUIContent menuItemContent,
            GenericMenu menu,
            ChangesetMenuOperations operations,
            ChangesetMenuOperations operationsToCheck,
            GenericMenu.MenuFunction menuFunction)
        {
            if (operations.HasFlag(operationsToCheck))
            {
                menu.AddItem(
                    menuItemContent,
                    false,
                    menuFunction);

                return;
            }

            menu.AddDisabledItem(menuItemContent);
        }

        static void AddDiffChangesetMenuItem(
            GUIContent menuItemContent,
            GenericMenu menu,
            ChangesetExtendedInfo changeset,
            ChangesetMenuOperations operations,
            GenericMenu.MenuFunction menuFunction)
        {
            string changesetName =
                changeset != null ?
                changeset.ChangesetId.ToString() :
                string.Empty;

            menuItemContent.text = string.Format("{0} {1}",
                    PlasticLocalization.GetString(
                        PlasticLocalization.Name.AnnotateDiffChangesetMenuItem,
                        changesetName),
                    GetPlasticShortcut.ForDiff());

            if (operations.HasFlag(ChangesetMenuOperations.DiffChangeset))
            {
                menu.AddItem(
                    menuItemContent,
                    false,
                    menuFunction);
                return;
            }

            menu.AddDisabledItem(
                menuItemContent);
        }

        static void AddDiffBranchMenuItem(
            GUIContent menuItemContent,
            GenericMenu menu,
            ChangesetExtendedInfo changeset,
            ChangesetMenuOperations operations,
            GenericMenu.MenuFunction menuFunction)
        {
            string branchName = GetBranchName(changeset);

            menuItemContent.text =
                PlasticLocalization.GetString(PlasticLocalization.Name.AnnotateDiffBranchMenuItem,
                branchName);

            if (operations.HasFlag(ChangesetMenuOperations.DiffChangeset))
            {
                menu.AddItem(
                    menuItemContent,
                    false,
                    menuFunction);
                return;
            }

            menu.AddDisabledItem(
                menuItemContent);
        }

        static string GetBranchName(ChangesetExtendedInfo changesetInfo)
        {
            if (changesetInfo == null)
                return string.Empty;

            string branchName = changesetInfo.BranchName;

            int lastIndex = changesetInfo.BranchName.LastIndexOf("/");

            if (lastIndex == -1)
                return branchName;

            return branchName.Substring(lastIndex + 1);
        }

        static bool IsOnMainBranch(ChangesetExtendedInfo singleSeletedChangeset)
        {
            if (singleSeletedChangeset == null)
                return false;

            return singleSeletedChangeset.BranchName == MAIN_BRANCH_NAME;
        }

        static ChangesetMenuOperations GetMenuOperations(
            Event e, bool isMultipleSelection)
        {
            if (Keyboard.IsControlOrCommandKeyPressed(e) &&
                Keyboard.IsKeyPressed(e, KeyCode.D))
                return isMultipleSelection ?
                    ChangesetMenuOperations.DiffSelectedChangesets :
                    ChangesetMenuOperations.DiffChangeset;

            if (Keyboard.IsControlOrCommandKeyPressed(e) &&
                Keyboard.IsKeyPressed(e, KeyCode.M))
                return ChangesetMenuOperations.MergeChangeset;

            return ChangesetMenuOperations.None;
        }

        void BuildComponents()
        {
            mDiffChangesetMenuItemContent = new GUIContent(string.Empty);
            mDiffSelectedChangesetsMenuItemContent = new GUIContent(string.Format("{0} {1}",
                PlasticLocalization.GetString(PlasticLocalization.Name.ChangesetMenuItemDiffSelected),
                GetPlasticShortcut.ForDiff()));
            mDiffBranchMenuItemContent = new GUIContent();
            mSwitchToChangesetMenuItemContent = new GUIContent(
                PlasticLocalization.GetString(PlasticLocalization.Name.ChangesetMenuItemSwitchToChangeset));
            mRevertToChangesetMenuItemContent = new GUIContent(
                PlasticLocalization.GetString(PlasticLocalization.Name.ChangesetMenuItemRevertToChangeset));
            mMergeChangesetMenuItemContent = new GUIContent(string.Format("{0} {1}",
                PlasticLocalization.GetString(PlasticLocalization.Name.ChangesetMenuItemMergeFromChangeset),
                GetPlasticShortcut.ForMerge()));
            mCreateCodeReviewMenuItemContent = new GUIContent(
                PlasticLocalization.Name.ChangesetMenuCreateANewCodeReview.GetString());
        }

        GenericMenu mMenu;

        GUIContent mDiffChangesetMenuItemContent;
        GUIContent mDiffSelectedChangesetsMenuItemContent;
        GUIContent mDiffBranchMenuItemContent;
        GUIContent mSwitchToChangesetMenuItemContent;
        GUIContent mRevertToChangesetMenuItemContent;
        GUIContent mMergeChangesetMenuItemContent;
        GUIContent mCreateCodeReviewMenuItemContent;

        readonly WorkspaceInfo mWkInfo;
        readonly IChangesetMenuOperations mChangesetMenuOperations;
        readonly IMenuOperations mMenuOperations;
        readonly LaunchTool.IShowDownloadPlasticExeWindow mShowDownloadPlasticExeWindow;
        readonly bool mIsGluonMode;

        long mLoadedBranchId = -1;

        const string MAIN_BRANCH_NAME = "/main";
    }
}
