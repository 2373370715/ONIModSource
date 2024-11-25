using System.Collections.Generic;
using TUNING;
using UnityEngine;
using BUILDINGS = STRINGS.BUILDINGS;

public class PropSurfaceSatellite2Config : IEntityConfig {
    public static string   ID = "PropSurfaceSatellite2";
    public        string[] GetDlcIds() { return DlcManager.AVAILABLE_ALL_VERSIONS; }

    public GameObject CreatePrefab() {
        var    id    = ID;
        string name  = BUILDINGS.PREFABS.PROPSURFACESATELLITE2.NAME;
        string desc  = BUILDINGS.PREFABS.PROPSURFACESATELLITE2.DESC;
        var    mass  = 50f;
        var    tier  = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
        var    tier2 = NOISE_POLLUTION.NOISY.TIER0;
        var gameObject = EntityTemplates.CreatePlacedEntity(id,
                                                            name,
                                                            desc,
                                                            mass,
                                                            Assets.GetAnim("satellite2_kanim"),
                                                            "off",
                                                            Grid.SceneLayer.Building,
                                                            4,
                                                            4,
                                                            tier,
                                                            tier2,
                                                            SimHashes.Creature,
                                                            new List<Tag> { GameTags.Gravitas });

        var component = gameObject.GetComponent<PrimaryElement>();
        component.SetElement(SimHashes.Unobtanium);
        component.Temperature = 294.15f;
        var workable = gameObject.AddOrGet<Workable>();
        workable.synchronizeAnims    = false;
        workable.resetProgressOnStop = true;
        var setLocker = gameObject.AddOrGet<SetLocker>();
        setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
        setLocker.dropOffset   = new Vector2I(0, 1);
        setLocker.numDataBanks = new[] { 4, 9 };
        LoreBearerUtil.AddLoreTo(gameObject);
        gameObject.AddOrGet<Demolishable>();
        return gameObject;
    }

    public void OnPrefabInit(GameObject inst) {
        var component = inst.GetComponent<SetLocker>();
        component.possible_contents_ids = PropSurfaceSatellite1Config.GetLockerBaseContents();
        component.ChooseContents();
        inst.GetComponent<OccupyArea>().objectLayers = new[] { ObjectLayer.Building };
        var radiationEmitter = inst.AddOrGet<RadiationEmitter>();
        radiationEmitter.emitType                 = RadiationEmitter.RadiationEmitterType.Constant;
        radiationEmitter.radiusProportionalToRads = false;
        radiationEmitter.emitRadiusX              = 12;
        radiationEmitter.emitRadiusY              = 12;
        radiationEmitter.emitRads                 = 2400f / (radiationEmitter.emitRadiusX / 6f);
    }

    public void OnSpawn(GameObject inst) {
        var component = inst.GetComponent<RadiationEmitter>();
        if (component != null) component.SetEmitting(true);
    }
}