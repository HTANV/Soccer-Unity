using System;

using UnityEngine;

using Codice.CM.Common;
using PlasticGui.WorkspaceWindow.Merge;

namespace Unity.PlasticSCM.Editor.Views.Merge.Developer
{
    [Serializable]
    internal class BuildMergeTabParameters
    {
        internal RepositorySpec RepSpec
        {
            get { return mRepSpec; }
        }

        internal EnumMergeType MergeType
        {
            get { return mMergeType; }
        }

        internal ShowIncomingChangesFrom From
        {
            get { return mFrom; }
        }

        internal bool IsIncomingMerge
        {
            get { return mIsIncomingMerge; }
        }

        internal bool IsInitialized
        {
            get { return mIsInitialized; }
        }

        internal BuildMergeTabParameters(
            RepositorySpec repSpec,
            ObjectInfo objectInfo,
            ObjectInfo ancestorObjectInfo,
            EnumMergeType mergeType,
            ShowIncomingChangesFrom from,
            bool isIncomingMerge)
        {
            mRepSpec = repSpec;

            SetObjectInfo(objectInfo);
            SetAncestorObjectInfo(ancestorObjectInfo);

            mMergeType = mergeType;
            mFrom = from;
            mIsIncomingMerge = isIncomingMerge;

            mIsInitialized = true;
        }

        internal ObjectInfo GetObjectInfo()
        {
            if (mBranchInfo.Id != -1)
                return mBranchInfo;

            if (mChangesetInfo.Id != -1)
                return mChangesetInfo;

            if (mLabelInfo.Id != -1)
                return mLabelInfo;

            return null;
        }

        internal ObjectInfo GetAncestorObjectInfo()
        {
            if (mAncestorBranchInfo.Id != -1)
                return mAncestorBranchInfo;

            if (mAncestorChangesetInfo.Id != -1)
                return mAncestorChangesetInfo;

            if (mAncestorLabelInfo.Id != -1)
                return mAncestorLabelInfo;

            return null;
        }

        void SetObjectInfo(ObjectInfo objectInfo)
        {
            if (objectInfo is BranchInfo)
            {
                mBranchInfo = (BranchInfo)objectInfo;
                return;
            }

            if (objectInfo is ChangesetInfo)
            {
                mChangesetInfo = (ChangesetInfo)objectInfo;
                return;
            }

            if (objectInfo is MarkerInfo)
            {
                mLabelInfo = (MarkerInfo)objectInfo;
                return;
            }
        }

        void SetAncestorObjectInfo(ObjectInfo objectInfo)
        {
            if (objectInfo is BranchInfo)
            {
                mAncestorBranchInfo = (BranchInfo)objectInfo;
                return;
            }

            if (objectInfo is ChangesetInfo)
            {
                mAncestorChangesetInfo = (ChangesetInfo)objectInfo;
                return;
            }

            if (objectInfo is MarkerInfo)
            {
                mAncestorLabelInfo = (MarkerInfo)objectInfo;
                return;
            }
        }

        [SerializeField]
        RepositorySpec mRepSpec;

        [SerializeField]
        BranchInfo mBranchInfo;
        [SerializeField]
        ChangesetInfo mChangesetInfo;
        [SerializeField]
        MarkerInfo mLabelInfo;

        [SerializeField]
        BranchInfo mAncestorBranchInfo;
        [SerializeField]
        ChangesetInfo mAncestorChangesetInfo;
        [SerializeField]
        MarkerInfo mAncestorLabelInfo;

        [SerializeField]
        EnumMergeType mMergeType;
        [SerializeField]
        ShowIncomingChangesFrom mFrom;
        [SerializeField]
        bool mIsIncomingMerge;

        [SerializeField]
        bool mIsInitialized;
    }
}
