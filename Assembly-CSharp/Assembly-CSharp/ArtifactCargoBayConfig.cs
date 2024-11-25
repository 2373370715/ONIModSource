using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class ArtifactCargoBayConfig : IBuildingConfig {
    public const    string   ID = "ArtifactCargoBay";
    public override string[] GetRequiredDlcIds() { return DlcManager.EXPANSION1; }

    public override BuildingDef CreateBuildingDef() {
        var id                  = "ArtifactCargoBay";
        var width               = 3;
        var height              = 1;
        var anim                = "artifact_transport_module_kanim";
        var hitpoints           = 1000;
        var construction_time   = 60f;
        var hollow_TIER         = BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER1;
        var refined_METALS      = MATERIALS.REFINED_METALS;
        var melting_point       = 9999f;
        var build_location_rule = BuildLocationRule.Anywhere;
        var tier                = NOISE_POLLUTION.NOISY.TIER2;
        var buildingDef = BuildingTemplates.CreateBuildingDef(id,
                                                              width,
                                                              height,
                                                              anim,
                                                              hitpoints,
                                                              construction_time,
                                                              hollow_TIER,
                                                              refined_METALS,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.NONE,
                                                              tier);

        BuildingTemplates.CreateRocketBuildingDef(buildingDef);
        buildingDef.SceneLayer          = Grid.SceneLayer.Building;
        buildingDef.Invincible          = true;
        buildingDef.OverheatTemperature = 2273.15f;
        buildingDef.Floodable           = false;
        buildingDef.AttachmentSlotTag   = GameTags.Rocket;
        buildingDef.ObjectLayer         = ObjectLayer.Building;
        buildingDef.RequiresPowerInput  = false;
        buildingDef.attachablePosition  = new CellOffset(0, 0);
        buildingDef.CanMove             = true;
        buildingDef.Cancellable         = false;
        buildingDef.ShowInBuildMenu     = false;
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
        go.AddOrGet<LoopingSounds>();
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
        go.AddOrGet<BuildingAttachPoint>().points = new[] {
            new BuildingAttachPoint.HardPoint(new CellOffset(0, 1), GameTags.Rocket, null)
        };
    }

    public override void DoPostConfigureComplete(GameObject go) {
        BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MAJOR);
        go.AddOrGet<Storage>()
          .SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>(new[] {
              Storage.StoredItemModifier.Seal, Storage.StoredItemModifier.Preserve
          }));

        Prioritizable.AddRef(go);
        var artifactModule = go.AddOrGet<ArtifactModule>();
        artifactModule.AddDepositTag(GameTags.PedestalDisplayable);
        artifactModule.occupyingObjectRelativePosition = new Vector3(0f, 0.5f, -1f);
        go.AddOrGet<DecorProvider>();
        go.AddOrGet<ItemPedestal>();
        go.AddOrGetDef<ArtifactHarvestModule.Def>();
    }
}