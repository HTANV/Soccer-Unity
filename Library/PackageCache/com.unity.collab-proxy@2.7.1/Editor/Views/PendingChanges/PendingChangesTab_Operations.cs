using System;
using System.Collections.Generic;

using Codice.Client.BaseCommands;
using Codice.Client.Common;
using Codice.Client.Common.EventTracking;
using Codice.Client.Common.Threading;
using Codice.CM.Common;
using GluonGui.WorkspaceWindow.Views.Checkin.Operations;
using PlasticGui;
using Unity.PlasticSCM.Editor.AssetUtils;
using Unity.PlasticSCM.Editor.Settings;
using Unity.PlasticSCM.Editor.Tool;
using Unity.PlasticSCM.Editor.UI;
using Unity.PlasticSCM.Editor.Views.PendingChanges.Dialogs;
using Unity.PlasticSCM.Editor.WebApi;

namespace Unity.PlasticSCM.Editor.Views.PendingChanges
{
    internal partial class PendingChangesTab : GetOperationDelegate.INotifySuccess
    {
        void GetOperationDelegate.INotifySuccess.InStatusBar(string message)
        {
            mStatusBar.Notify(
                message,
                UnityEditor.MessageType.None,
                Images.GetStepOkIcon());
        }

        void GetOperationDelegate.INotifySuccess.InEmptyState(string message)
        {
            mOperationSuccessfulMessage = message;
            mCooldownClearOperationSuccessAction.Ping();
        }

        void GetOperationDelegate.INotifySuccess.EnableInviteMembersIfOrganizationAdmin(string server)
        {
            string organizationName = ServerOrganizationParser.GetOrganizationFromServer(server);

            CurrentUserAdminCheckResponse response = null;

            IThreadWaiter waiter = ThreadWaiter.GetWaiter(50);
            waiter.Execute(
                /*threadOperationDelegate*/
                delegate
                {
                    string authToken = AuthToken.GetForServer(server);

                    if (string.IsNullOrEmpty(authToken))
                        return;

                    response = WebRestApiClient.PlasticScm.IsUserAdmin(organizationName, authToken);
                },
                /*afterOperationDelegate*/
                delegate
                {
                    if (response == null || !response.IsCurrentUserAdmin)
                        return;

                    mCanInviteMembersFromPendingChanges = true;

                    mParentWindow.Repaint();
                });
        }

        void UndoForMode(bool isGluonMode, bool keepLocalChanges)
        {
            List<ChangeInfo> changesToUndo;
            List<ChangeInfo> dependenciesCandidates;

            mPendingChangesTreeView.GetCheckedChanges(
                null, true,
                out changesToUndo,
                out dependenciesCandidates);

            UndoChangesForMode(
                isGluonMode, keepLocalChanges, changesToUndo, dependenciesCandidates);
        }

        void UndoChangesForMode(
            bool isGluonMode,
            bool keepLocalChanges,
            List<ChangeInfo> changesToUndo,
            List<ChangeInfo> dependenciesCandidates)
        {
            if (isGluonMode)
            {
                PartialUndoChanges(changesToUndo, dependenciesCandidates);
                return;
            }

            UndoChanges(changesToUndo, dependenciesCandidates, keepLocalChanges);
        }

        void UndoUnchanged()
        {
            List<ChangeInfo> changesToUndo;
            List<ChangeInfo> dependenciesCandidates;

            mPendingChangesTreeView.GetCheckedChanges(
                null, true,
                out changesToUndo,
                out dependenciesCandidates);

            UndoUnchangedChanges(changesToUndo);
        }

        void CheckinForMode(
            bool isGluonMode,
            bool keepItemsLocked)
        {
            List<ChangeInfo> changesToCheckin;
            List<ChangeInfo> dependenciesCandidates;

            mPendingChangesTreeView.GetCheckedChanges(
                null, false,
                out changesToCheckin,
                out dependenciesCandidates);

            CheckinChangesForMode(
                changesToCheckin, dependenciesCandidates, isGluonMode, keepItemsLocked);
        }

        void CheckinChangesForMode(
            List<ChangeInfo> changesToCheckin,
            List<ChangeInfo> dependenciesCandidates,
            bool isGluonMode,
            bool keepItemsLocked)
        {
            if (isGluonMode)
            {
                PartialCheckinChanges(changesToCheckin, dependenciesCandidates, keepItemsLocked);
                return;
            }

            CheckinChanges(changesToCheckin, dependenciesCandidates);
        }

        void ShelveForMode(
            bool isGluonMode,
            bool keepItemsLocked)
        {
            List<ChangeInfo> changesToShelve;
            List<ChangeInfo> dependenciesCandidates;

            mPendingChangesTreeView.GetCheckedChanges(
                null, false,
                out changesToShelve,
                out dependenciesCandidates);

            ShelveChangesForMode(
                changesToShelve, dependenciesCandidates, isGluonMode, keepItemsLocked);
        }

        void ShelveChangesForMode(
           List<ChangeInfo> changesToShelve,
           List<ChangeInfo> dependenciesCandidates,
           bool isGluonMode,
           bool keepItemsLocked)
        {
            if (isGluonMode)
            {
                PartialCheckinChanges(
                    changesToShelve, dependenciesCandidates, keepItemsLocked, isShelve: true);
                return;
            }

            ShelveChanges(changesToShelve, dependenciesCandidates);
        }

        void PartialCheckinChanges(
            List<ChangeInfo> changesToCheckin,
            List<ChangeInfo> dependenciesCandidates,
            bool keepItemsLocked,
            bool isShelve = false)
        {
            if (CheckEmptyOperation(changesToCheckin))
            {
                ((IProgressControls)mProgressControls).ShowWarning(
                    PlasticLocalization.GetString(PlasticLocalization.Name.NoItemsAreSelected));
                return;
            }

            bool isCancelled;
            SaveAssets.ForChangesWithConfirmation(
                mWkInfo.ClientPath, changesToCheckin, mWorkspaceOperationsMonitor,
                out isCancelled);

            if (isCancelled)
                return;

            CheckinUIOperation checkinOperation = new CheckinUIOperation(
                mWkInfo,
                mViewHost,
                mProgressControls,
                mGuiMessage,
                new LaunchCheckinConflictsDialog(mParentWindow),
                new LaunchDependenciesDialog(
                    PlasticLocalization.GetString(PlasticLocalization.Name.CheckinButton),
                    mParentWindow),
                this,
                mWorkspaceWindow.GluonProgressOperationHandler,
                null);

            bool areAllItemsChecked = mPendingChangesTreeView.AreAllItemsChecked();

            checkinOperation.Checkin(
                changesToCheckin,
                dependenciesCandidates,
                mCommentText,
                keepItemsLocked,
                isShelve,
                isShelve ?
                    OpenPlasticProjectSettings.InShelveAndSwitchFoldout:
                    (Action)null,
                isShelve ?
                    GetOperationDelegate.ForUndoEnd(changesToCheckin, false) :
                    (Action)null,
                isShelve ?
                    (Action)null :
                    RefreshAsset.UnityAssetDatabase,
                isShelve ?
                    GetOperationDelegate.ForShelveSuccess(areAllItemsChecked, this) :
                    GetOperationDelegate.ForPartialCheckinSuccess(mWkInfo, areAllItemsChecked, this));
        }

        void CheckinChanges(
            List<ChangeInfo> changesToCheckin,
            List<ChangeInfo> dependenciesCandidates)
        {
            if (CheckEmptyOperation(changesToCheckin, HasPendingMergeLinks()))
            {
                ((IProgressControls)mProgressControls).ShowWarning(
                    PlasticLocalization.GetString(PlasticLocalization.Name.NoItemsAreSelected));
                return;
            }

            bool isCancelled;
            SaveAssets.ForChangesWithConfirmation(
                mWkInfo.ClientPath, changesToCheckin, mWorkspaceOperationsMonitor,
                out isCancelled);

            if (isCancelled)
                return;

            bool areAllItemsChecked = mPendingChangesTreeView.AreAllItemsChecked();

            mPendingChangesOperations.Checkin(
                changesToCheckin,
                dependenciesCandidates,
                mCommentText,
                null,
                RefreshAsset.UnityAssetDatabase,
                GetOperationDelegate.ForCheckinSuccess(mWkInfo, areAllItemsChecked, this));
        }

        void ShelveChanges(
            List<ChangeInfo> changesToShelve,
            List<ChangeInfo> dependenciesCandidates)
        {
            bool hasPendingMergeLinks = HasPendingMergeLinks();

            if (hasPendingMergeLinks &&
                !UserWantsShelveWithPendingMergeLinks(mGuiMessage))
            {
                return;
            }

            if (CheckEmptyOperation(changesToShelve, hasPendingMergeLinks))
            {
                ((IProgressControls)mProgressControls).ShowWarning(
                    PlasticLocalization.GetString(PlasticLocalization.Name.NoItemsAreSelected));
                return;
            }

            bool isCancelled;
            SaveAssets.ForChangesWithConfirmation(
                mWkInfo.ClientPath, changesToShelve, mWorkspaceOperationsMonitor,
                out isCancelled);

            if (isCancelled)
                return;

            bool areAllItemsChecked = mPendingChangesTreeView.AreAllItemsChecked();

            mPendingChangesOperations.Shelve(
                changesToShelve,
                dependenciesCandidates,
                mCommentText,
                OpenPlasticProjectSettings.InShelveAndSwitchFoldout,
                GetOperationDelegate.ForUndoEnd(changesToShelve, false),
                null,
                GetOperationDelegate.ForShelveSuccess(areAllItemsChecked, this));
        }

        void PartialUndoChanges(
            List<ChangeInfo> changesToUndo,
            List<ChangeInfo> dependenciesCandidates)
        {
            if (CheckEmptyOperation(changesToUndo))
            {
                ((IProgressControls)mProgressControls).ShowWarning(
                    PlasticLocalization.GetString(PlasticLocalization.Name.NoItemsToUndo));
                return;
            }

            SaveAssets.ForChangesWithoutConfirmation(
                mWkInfo.ClientPath, changesToUndo, mWorkspaceOperationsMonitor);

            UndoUIOperation undoOperation = new UndoUIOperation(
                mWkInfo, mViewHost,
                new LaunchDependenciesDialog(
                    PlasticLocalization.GetString(PlasticLocalization.Name.UndoButton),
                    mParentWindow),
                mProgressControls);

            undoOperation.Undo(
                changesToUndo,
                dependenciesCandidates,
                RefreshAsset.UnityAssetDatabase);
        }

        void UndoChanges(
            List<ChangeInfo> changesToUndo,
            List<ChangeInfo> dependenciesCandidates,
            bool keepLocalChanges)
        {
            if (CheckEmptyOperation(changesToUndo, HasPendingMergeLinks()))
            {
                ((IProgressControls)mProgressControls).ShowWarning(
                    PlasticLocalization.GetString(PlasticLocalization.Name.NoItemsToUndo));
                return;
            }

            SaveAssets.ForChangesWithoutConfirmation(
                mWkInfo.ClientPath, changesToUndo, mWorkspaceOperationsMonitor);

            mPendingChangesOperations.Undo(
                changesToUndo,
                dependenciesCandidates,
                mPendingMergeLinks.Count,
                keepLocalChanges,
                GetOperationDelegate.ForUndoEnd(changesToUndo, keepLocalChanges),
                null);
        }

        void UndoUnchangedChanges(List<ChangeInfo> changesToUndo)
        {
            if (CheckEmptyOperation(changesToUndo, HasPendingMergeLinks()))
            {
                ((IProgressControls) mProgressControls).ShowWarning(
                    PlasticLocalization.GetString(PlasticLocalization.Name.NoItemsToUndo));

                return;
            }

            SaveAssets.ForChangesWithoutConfirmation(
                mWkInfo.ClientPath, changesToUndo, mWorkspaceOperationsMonitor);

            mPendingChangesOperations.UndoUnchanged(
                changesToUndo,
                RefreshAsset.UnityAssetDatabase,
                null);
        }

        void UndoCheckoutsKeepingLocalChanges()
        {
            UndoForMode(isGluonMode: false, keepLocalChanges: true);
        }

        void UndoCheckoutChangesKeepingLocalChanges(List<ChangeInfo> changesToUndo)
        {
            UndoChanges(changesToUndo, new List<ChangeInfo>(), keepLocalChanges: true);
        }

        void ShowShelvesView(IViewSwitcher viewSwitcher)
        {
            TrackFeatureUseEvent.For(
                mRepSpec,
                TrackFeatureUseEvent.Features.UnityPackage.ShowShelvesViewFromDropdownMenu);

            viewSwitcher.ShowShelvesView();
        }

        static bool CheckEmptyOperation(List<ChangeInfo> elements)
        {
            return elements == null || elements.Count == 0;
        }

        static bool CheckEmptyOperation(List<ChangeInfo> elements, bool bHasPendingMergeLinks)
        {
            if (bHasPendingMergeLinks)
                return false;

            if (elements != null && elements.Count > 0)
                return false;

            return true;
        }

        static bool UserWantsShelveWithPendingMergeLinks(GuiMessage.IGuiMessage guiMessage)
        {
            return guiMessage.ShowQuestion(
                PlasticLocalization.GetString(PlasticLocalization.Name.ShelveWithPendingMergeLinksRequest),
                PlasticLocalization.GetString(PlasticLocalization.Name.ShelveWithPendingMergeLinksRequestMessage),
                PlasticLocalization.GetString(PlasticLocalization.Name.ShelveButton));
        }
    }
}
