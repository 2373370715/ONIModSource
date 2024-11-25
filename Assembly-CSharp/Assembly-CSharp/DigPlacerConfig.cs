using System;
using STRINGS;
using UnityEngine;

public class DigPlacerConfig : CommonPlacerConfig, IEntityConfig {
    public static string ID = "DigPlacer";

    public GameObject CreatePrefab() {
        var gameObject
            = base.CreatePrefab(ID, MISC.PLACERS.DIGPLACER.NAME, Assets.instance.digPlacerAssets.materials[0]);

        var diggable = gameObject.AddOrGet<Diggable>();
        diggable.workTime         = 5f;
        diggable.synchronizeAnims = false;
        diggable.workAnims        = new HashedString[] { "place", "release" };
        diggable.materials        = Assets.instance.digPlacerAssets.materials;
        diggable.materialDisplay  = gameObject.GetComponentInChildren<MeshRenderer>(true);
        gameObject.AddOrGet<CancellableDig>();
        return gameObject;
    }

    public void OnPrefabInit(GameObject go) { }
    public void OnSpawn(GameObject      go) { }

    [Serializable]
    public class DigPlacerAssets {
        public Material[] materials;
    }
}