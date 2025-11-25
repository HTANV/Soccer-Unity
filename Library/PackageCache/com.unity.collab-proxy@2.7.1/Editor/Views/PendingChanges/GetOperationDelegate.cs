using System;
using System.Collections.Generic;
using System.Linq;

using Codice.Client.BaseCommands;
using Codice.CM.Common;
using PlasticGui;
using Unity.PlasticSCM.Editor.AssetUtils;
using Unity.PlasticSCM.Editor.UI;

namespace Unity.PlasticSCM.Editor.Views.PendingChanges
{
    internal static class GetOperationDelegate
    {
        internal interface INotifySuccess
        {
            void InStatusBar(string message);
            void InEmptyState(string message);
            void EnableInviteMembersIfOrganizationAdmin(string server);
        }

        internal static Action ForCheckinSuccess(
            WorkspaceInfo wkInfo, bool areAllItemsChecked, INotifySuccess notifySuccess)
        {
            if (areAllItemsChecked)
                return () => NotifyCheckinSuccessInEmptyState(wkInfo, notifySuccess);

            return () => NotifyCheckinSuccessInStatusBar(notifySuccess);
        }

        internal static SuccessOperationDelegateForCreatedChangeset ForPartialCheckinSuccess(
            WorkspaceInfo wkInfo, bool areAllItemsChecked, INotifySuccess notifySuccess)
        {
            if (areAllItemsChecked)
                return (_, __) => NotifyCheckinSuccessInEmptyState(wkInfo, notifySuccess);

            return (_, __) => NotifyCheckinSuccessInStatusBar(notifySuccess);
        }

        internal static SuccessOperationDelegateForCreatedChangeset ForShelveSuccess(
            bool areAllItemsChecked, INotifySuccess notifySuccess)
        {
            if (areAllItemsChecked)
                return (createdChangesetId, areShelvedChangesUndone) =>
                    NotifyShelveSuccess(createdChangesetId, areShelvedChangesUndone, notifySuccess);

            return (createdChangesetId, areShelvedChangesUndone) =>
                NotifyShelveSuccessInStatusBar(createdChangesetId, notifySuccess);
        }

        internal static Action ForUndoEnd(List<ChangeInfo> changesToUndo, bool keepLocalChanges)
        {
            if (keepLocalChanges)
                return null;

            return () =>
            {
                if (changesToUndo.Any(
                        change => AssetsPath.IsPackagesRootElement(change.Path) &&
                        !IsAddedChange(change)))
                {
                    RefreshAsset.UnityAssetDatabaseAndPackageManagerAsync();
                    return;
                }

                RefreshAsset.UnityAssetDatabase();
            };
        }

        static void NotifyCheckinSuccessInEmptyState(
            WorkspaceInfo wkInfo,
            INotifySuccess notifySuccess)
        {
            RepositorySpec repSpec = PlasticGui.Plastic.API.GetRepositorySpec(wkInfo);
            bool isFirstCheckin = !BoolSetting.Load(UnityConstants.FIRST_CHECKIN_SUBMITTED, false);

            if (PlasticGui.Plastic.API.IsCloud(repSpec.Server) && isFirstCheckin)
            {
                BoolSetting.Save(true, UnityConstants.FIRST_CHECKIN_SUBMITTED);
                notifySuccess.EnableInviteMembersIfOrganizationAdmin(repSpec.Server);
            }

            notifySuccess.InEmptyState(PlasticLocalization.Name.CheckinCompleted.GetString());
        }

        static void NotifyCheckinSuccessInStatusBar(
            INotifySuccess notifySuccess)
        {
            notifySuccess.InStatusBar(PlasticLocalization.Name.CheckinCompleted.GetString());
        }

        static void NotifyShelveSuccess(
            long createdChangesetId,
            bool areShelvedChangesUndone,
            INotifySuccess notifySuccess)
        {
            if (areShelvedChangesUndone)
            {
                NotifyShelveSuccessInEmptyState(createdChangesetId, notifySuccess);
                return;
            }

            NotifyShelveSuccessInStatusBar(createdChangesetId, notifySuccess);
        }

        static void NotifyShelveSuccessInEmptyState(
            long createdChangesetId,
            INotifySuccess notifySuccess)
        {
            notifySuccess.InEmptyState(GetShelveCreatedMessage(createdChangesetId));
        }

        static void NotifyShelveSuccessInStatusBar(
            long createdChangesetId,
            INotifySuccess notifySuccess)
        {
            notifySuccess.InStatusBar(GetShelveCreatedMessage(createdChangesetId));
        }

        static bool IsAddedChange(ChangeInfo change)
        {
            return ChangeTypesOperator.ContainsAny(
                change.ChangeTypes, ChangeTypesClassifier.ADDED_TYPES);
        }

        static string GetShelveCreatedMessage(long createdChangesetId)
        {
            return PlasticLocalization.Name.ShelveCreatedMessage.GetString(
                string.Format("{0} {1}",
                    PlasticLocalization.Name.Shelve.GetString(),
                    Math.Abs(createdChangesetId)));
        }
    }
}
