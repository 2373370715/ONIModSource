using UnityEngine;

public class SleepLocator : IEntityConfig {
    public static readonly string   ID = "SleepLocator";
    public                 string[] GetDlcIds() { return DlcManager.AVAILABLE_ALL_VERSIONS; }

    public GameObject CreatePrefab() {
        var gameObject = EntityTemplates.CreateEntity(ID, ID, false);
        gameObject.AddTag(GameTags.NotConversationTopic);
        gameObject.AddOrGet<Approachable>();
        gameObject.AddOrGet<Sleepable>().isNormalBed = false;
        return gameObject;
    }

    public void OnPrefabInit(GameObject go) { }
    public void OnSpawn(GameObject      go) { }
}