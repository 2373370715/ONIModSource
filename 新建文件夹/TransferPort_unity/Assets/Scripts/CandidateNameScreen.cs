using RsLib.Adapter;
using RsLib.Components;
using UnityEngine;

namespace RsTransferPort
{
    public class CandidateNameScreen : MonoBehaviour
    {
        [SerializeField]
        protected RsHierarchyReferences rowPrefab;
        
        [SerializeField]
        protected GameObject listContainer;

        [SerializeField]
        protected MultiToggleAdapter supplyToggle;
        [SerializeField]
        protected MultiToggleAdapter temperatureToggle;
    }
}