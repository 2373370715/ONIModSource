using System;
using RsLib.Components;
using UnityEngine;

namespace RsLib.Adapter
{
    public class RsMultiToggleGroupAdapter : MonoBehaviour
    {
        [SerializeField] private MultiToggleAdapter[] toggles;
        [SerializeField] private int m_selected;
        
        public event Action<int> onSelected;
        
    }
}