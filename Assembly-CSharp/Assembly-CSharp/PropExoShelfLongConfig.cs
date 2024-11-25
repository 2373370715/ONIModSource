using System.Collections.Generic;
using TUNING;
using UnityEngine;
using BUILDINGS = STRINGS.BUILDINGS;

public class PropExoShelfLongConfig : IEntityConfig {
    public string[] GetDlcIds() { return DlcManager.AVAILABLE_DLC_2; }

    public GameObject CreatePrefab() {
        var    id    = "PropExoShelfLong";
        string name  = BUILDINGS.PREFABS.PROPEXOSHELFLONG.NAME;
        string desc  = BUILDINGS.PREFABS.PROPEXOSHELFLONG.DESC;
        var    mass  = 50f;
        var    tier  = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
        var    tier2 = NOISE_POLLUTION.NOISY.TIER0;
        var gameObject = EntityTemplates.CreatePlacedEntity(id,
                                                            name,
                                                            desc,
                                                            mass,
                                                            Assets.GetAnim("poi_shelf_long_kanim"),
                                                            "off",
                                                            Grid.SceneLayer.Building,
                                                            3,
                                                            1,
                                                            tier,
                                                            tier2,
                                                            SimHashes.Creature,
                                                            new List<Tag> { GameTags.Gravitas });

        var component = gameObject.GetComponent<PrimaryElement>();
        component.SetElement(SimHashes.Granite);
        component.Temperature = 294.15f;
        gameObject.AddOrGet<Demolishable>();
        return gameObject;
    }

    public void OnPrefabInit(GameObject inst) {
        inst.GetComponent<OccupyArea>().objectLayers = new[] { ObjectLayer.Building };
    }

    public void OnSpawn(GameObject inst) { }
}