using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/AutoDisinfectableManager")]
public class AutoDisinfectableManager : KMonoBehaviour, ISim1000ms {
    public static    AutoDisinfectableManager Instance;
    private readonly List<AutoDisinfectable>  autoDisinfectables = new List<AutoDisinfectable>();

    public void Sim1000ms(float dt) {
        for (var i = 0; i < autoDisinfectables.Count; i++) autoDisinfectables[i].RefreshChore();
    }

    public static void DestroyInstance() { Instance = null; }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        Instance = this;
    }

    public void AddAutoDisinfectable(AutoDisinfectable auto_disinfectable) {
        autoDisinfectables.Add(auto_disinfectable);
    }

    public void RemoveAutoDisinfectable(AutoDisinfectable auto_disinfectable) {
        auto_disinfectable.CancelChore();
        autoDisinfectables.Remove(auto_disinfectable);
    }
}