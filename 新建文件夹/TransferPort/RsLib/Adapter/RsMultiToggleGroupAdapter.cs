using System;
using UnityEngine;

namespace RsLib.Adapter;

public class RsMultiToggleGroupAdapter : MonoBehaviour {
    [SerializeField]
    private int m_selected;

    [SerializeField]
    private MultiToggleAdapter[] toggles;

    public int               selected => m_selected;
    public event Action<int> onSelected;

    protected void Start() {
        for (var i = 0; i < toggles.Length; i++) {
            var toggle = toggles[i];
            var i1     = i;
            toggle.onClick += () => { OnToggleClick(i1); };
        }

        SelectNoCheck(m_selected);
    }

    public void Select(int index) {
        if (selected == index) return;

        SelectNoCheck(index);
    }

    protected void SelectNoCheck(int index) {
        m_selected = index;
        for (var i = 0; i < toggles.Length; i++) {
            MultiToggle toggle = toggles[i];
            if (index != i)
                toggle.ChangeState(0);
            else
                toggle.ChangeState(1);
        }

        onSelected?.Invoke(index);
    }

    protected void OnToggleClick(int index) { Select(index); }
}