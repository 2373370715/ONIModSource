using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class LiquidLogicValveConfig : IBuildingConfig {
    public const  string      ID           = "LiquidLogicValve";
    private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

    public override BuildingDef CreateBuildingDef() {
        var id                  = "LiquidLogicValve";
        var width               = 1;
        var height              = 2;
        var anim                = "valveliquid_logic_kanim";
        var hitpoints           = 30;
        var construction_time   = 10f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
        var refined_METALS      = MATERIALS.REFINED_METALS;
        var melting_point       = 1600f;
        var build_location_rule = BuildLocationRule.Anywhere;
        var tier2               = NOISE_POLLUTION.NOISY.TIER1;
        var buildingDef = BuildingTemplates.CreateBuildingDef(id,
                                                              width,
                                                              height,
                                                              anim,
                                                              hitpoints,
                                                              construction_time,
                                                              tier,
                                                              refined_METALS,
                                                              melting_point,
                                                              build_location_rule,
                                                              BUILDINGS.DECOR.PENALTY.TIER0,
                                                              tier2);

        buildingDef.InputConduitType            = ConduitType.Liquid;
        buildingDef.OutputConduitType           = ConduitType.Liquid;
        buildingDef.Floodable                   = false;
        buildingDef.RequiresPowerInput          = true;
        buildingDef.EnergyConsumptionWhenActive = 10f;
        buildingDef.PowerInputOffset            = new CellOffset(0, 1);
        buildingDef.ViewMode                    = OverlayModes.LiquidConduits.ID;
        buildingDef.AudioCategory               = "Metal";
        buildingDef.PermittedRotations          = PermittedRotations.R360;
        buildingDef.UtilityInputOffset          = new CellOffset(0, 0);
        buildingDef.UtilityOutputOffset         = new CellOffset(0, 1);
        buildingDef.LogicInputPorts = new List<LogicPorts.Port> {
            LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID,
                                      new CellOffset(0, 0),
                                      STRINGS.BUILDINGS.PREFABS.LIQUIDLOGICVALVE.LOGIC_PORT,
                                      STRINGS.BUILDINGS.PREFABS.LIQUIDLOGICVALVE.LOGIC_PORT_ACTIVE,
                                      STRINGS.BUILDINGS.PREFABS.LIQUIDLOGICVALVE.LOGIC_PORT_INACTIVE,
                                      true)
        };

        GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidLogicValve");
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        Object.DestroyImmediate(go.GetComponent<BuildingEnabledButton>());
        BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
        var operationalValve = go.AddOrGet<OperationalValve>();
        operationalValve.conduitType = ConduitType.Liquid;
        operationalValve.maxFlow     = 10f;
    }

    public override void DoPostConfigureComplete(GameObject go) {
        Object.DestroyImmediate(go.GetComponent<ConduitConsumer>());
        Object.DestroyImmediate(go.GetComponent<ConduitDispenser>());
        go.GetComponent<RequireInputs>().SetRequirements(true, false);
        go.AddOrGet<LogicOperationalController>().unNetworkedValue = 0;
        go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
    }
}