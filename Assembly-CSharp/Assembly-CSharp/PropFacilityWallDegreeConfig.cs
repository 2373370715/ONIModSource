using System.Collections.Generic;
using TUNING;
using UnityEngine;
using BUILDINGS = STRINGS.BUILDINGS;

public class PropFacilityWallDegreeConfig : IEntityConfig {
    public string[] GetDlcIds() { return DlcManager.AVAILABLE_ALL_VERSIONS; }

    public GameObject CreatePrefab() {
        var    id    = "PropFacilityWallDegree";
        string name  = BUILDINGS.PREFABS.PROPFACILITYWALLDEGREE.NAME;
        string desc  = BUILDINGS.PREFABS.PROPFACILITYWALLDEGREE.DESC;
        var    mass  = 50f;
        var    tier  = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
        var    tier2 = NOISE_POLLUTION.NOISY.TIER0;
        var gameObject = EntityTemplates.CreatePlacedEntity(id,
                                                            name,
                                                            desc,
                                                            mass,
                                                            Assets.GetAnim("gravitas_degree_kanim"),
                                                            "off",
                                                            Grid.SceneLayer.Building,
                                                            2,
                                                            2,
                                                            tier,
                                                            PermittedRotations.R90,
                                                            Orientation.Neutral,
                                                            tier2,
                                                            SimHashes.Creature,
                                                            new List<Tag> { GameTags.Gravitas });

        var component = gameObject.GetComponent<PrimaryElement>();
        component.SetElement(SimHashes.Granite);
        component.Temperature = 294.15f;
        gameObject.AddOrGet<Demolishable>();
        gameObject.GetComponent<OccupyArea>().objectLayers = new[] { ObjectLayer.Building };
        return gameObject;
    }

    public void OnPrefabInit(GameObject inst) { }

    public void OnSpawn(GameObject inst) {
        var component = inst.GetComponent<OccupyArea>();
        var cell      = Grid.PosToCell(inst);
        foreach (var offset in component.OccupiedCellsOffsets)
            Grid.GravitasFacility[Grid.OffsetCell(cell, offset)] = true;
    }
}