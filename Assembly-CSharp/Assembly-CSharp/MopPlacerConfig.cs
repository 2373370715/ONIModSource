using System;
using STRINGS;
using UnityEngine;

public class MopPlacerConfig : CommonPlacerConfig, IEntityConfig {
    public static string ID = "MopPlacer";

    public GameObject CreatePrefab() {
        var gameObject = base.CreatePrefab(ID, MISC.PLACERS.MOPPLACER.NAME, Assets.instance.mopPlacerAssets.material);
        gameObject.AddTag(GameTags.NotConversationTopic);
        var moppable = gameObject.AddOrGet<Moppable>();
        moppable.synchronizeAnims    = false;
        moppable.amountMoppedPerTick = 20f;
        gameObject.AddOrGet<Cancellable>();
        return gameObject;
    }

    public void OnPrefabInit(GameObject go) { }
    public void OnSpawn(GameObject      go) { }

    [Serializable]
    public class MopPlacerAssets {
        public Material material;
    }
}