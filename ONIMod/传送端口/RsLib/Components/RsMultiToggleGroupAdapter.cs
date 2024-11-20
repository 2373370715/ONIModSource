using System;
using UnityEngine;

namespace RsLib.Adapter;

public class RsMultiToggleGroupCom : MonoBehaviour {
    [SerializeField]
    public MultiToggle[] toggles;

    public int               selected { get; private set; } = -1;
    public event Action<int> onSelected;

    protected void Awake() {
        for (var i = 0; i < toggles.Length; i++) {
            var toggle = toggles[i];
            var i1     = i;
            toggle.onClick += () => { OnToggleClick(i1); };
        }
    }

    public void Select(int index, bool triggerOnSelected = false) {
        if (selected == index) return;

        SelectNoCheck(index, triggerOnSelected);
    }

    protected void SelectNoCheck(int index, bool triggerOnSelected = false) {
        selected = index;
        for (var i = 0; i < toggles.Length; i++) {
            var toggle = toggles[i];
            if (index != i)
                toggle.ChangeState(0);
            else
                toggle.ChangeState(1);
        }

        if (triggerOnSelected) onSelected?.Invoke(index);
    }

    protected void OnToggleClick(int index) { Select(index, true); }
}