using Codice.CM.Common;
using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer.Explorer;
using Unity.PlasticSCM.Editor.AssetUtils;
using Unity.PlasticSCM.Editor.UI;

namespace Unity.PlasticSCM.Editor.Views.Changesets
{
    internal partial class ChangesetsTab
    {
        void SwitchToChangesetForMode()
        {
            bool isCancelled;
            SaveAssets.UnderWorkspaceWithConfirmation(
                mWkInfo.ClientPath, mWorkspaceOperationsMonitor,
                out isCancelled);

            if (isCancelled)
                return;

            if (mIsGluonMode)
            {
                SwitchToChangesetForGluon();
                return;
            }

            SwitchToChangesetForDeveloper();
        }

        void SwitchToChangesetForDeveloper()
        {
            mChangesetOperations.SwitchToChangeset(
                ChangesetsSelection.GetSelectedRepository(mChangesetsListView),
                ChangesetsSelection.GetSelectedChangeset(mChangesetsListView),
                RefreshAsset.BeforeLongAssetOperation,
                items => RefreshAsset.AfterLongAssetOperation(
                    ProjectPackages.ShouldBeResolvedFromUpdateReport(mWkInfo, items)));
        }

        void SwitchToChangesetForGluon()
        {
            ChangesetExtendedInfo csetInfo = ChangesetsSelection.GetSelectedChangeset(mChangesetsListView);

            new SwitchToUIOperation().SwitchToChangeset(
                mWkInfo,
                PlasticGui.Plastic.API.GetRepositorySpec(mWkInfo),
                csetInfo.BranchName,
                csetInfo.ChangesetId,
                mViewHost,
                mGluonNewIncomingChangesUpdater,
                new UnityPlasticGuiMessage(),
                mProgressControls,
                mWorkspaceWindow.GluonProgressOperationHandler,
                mGluonUpdateReport,
                mWorkspaceWindow,
                mShelvePendingChangesQuestionerBuilder,
                mShelvedChangesUpdater,
                mEnableSwitchAndShelveFeatureDialog,
                RefreshAsset.BeforeLongAssetOperation,
                items => RefreshAsset.AfterLongAssetOperation(
                    ProjectPackages.ShouldBeResolvedFromPaths(mWkInfo, items)));
        }
    }
}
