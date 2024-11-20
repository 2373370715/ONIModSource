using RsLib.Adapter;
using UnityEngine;

namespace RsTransferPort
{
    public class PriorityBar : MonoBehaviour
    {
        [SerializeField]
        private MultiToggleAdapter[] toggles;
        [SerializeField]
        private ToolTipAdapter help;
    }
}