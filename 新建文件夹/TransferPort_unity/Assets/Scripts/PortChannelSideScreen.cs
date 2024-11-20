using RsLib.Adapter;
using RsLib.Components;
using UnityEngine;

namespace RsTransferPort
{
    public class PortChannelSideScreen : MonoBehaviour
    {
        [SerializeField]
        protected string titleKey;
        [SerializeField]
        private MultiToggleAdapter detailLevelToggle;
        [SerializeField]
        private MultiToggleAdapter batchRenameToggle;
        [SerializeField]
        private MultiToggleAdapter globalToggle;
        [SerializeField]
        private MultiToggleAdapter openCandidateNameToggle;
        [SerializeField]
        private KInputTextFieldAdapter channelNameInputField;
        [SerializeField]
        private GameObject listContainer;
        [SerializeField]
        private LocTextAdapter headerLabel;
        [SerializeField]
        private LocTextAdapter warningLabel;
        [SerializeField]
        private PriorityBar priorityBar;
        
        [SerializeField]
        private RsHierarchyReferences row1Prefab;
        [SerializeField]
        private RsHierarchyReferences row2Prefab;
        [SerializeField]
        private LocTextAdapter infoLocTextPrefab;
        [SerializeField]
        private RsHierarchyReferences worldInfoPrefab;


        [SerializeField]
        private CandidateNameScreen candidateNameScreenPrefab;
    }
}