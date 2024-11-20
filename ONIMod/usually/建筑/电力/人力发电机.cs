using HarmonyLib;
using TUNING;

#if usually
[HarmonyPatch]
public class 人力发电机 {
    [HarmonyPatch(typeof(ManualGeneratorConfig), "CreateBuildingDef"), HarmonyPrefix]
    public static bool fix1(ref BuildingDef __result) {
        string            id                  = "ManualGenerator";
        int               width               = 2;
        int               height              = 2;
        string            anim                = "generatormanual_kanim";
        int               hitpoints           = 30;
        float             construction_time   = 30f;
        float[]           tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
        string[]          all_METALS          = MATERIALS.ALL_METALS;
        float             melting_point       = 1600f;
        BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
        EffectorValues    tier2               = NOISE_POLLUTION.NOISY.TIER3;
        BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id,
                                                                      width,
                                                                      height,
                                                                      anim,
                                                                      hitpoints,
                                                                      construction_time,
                                                                      tier,
                                                                      all_METALS,
                                                                      melting_point,
                                                                      build_location_rule,
                                                                      BUILDINGS.DECOR.NONE,
                                                                      tier2);

        buildingDef.GeneratorWattageRating = 2000 * 20000f;
        buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
        buildingDef.RequiresPowerOutput = true;
        buildingDef.PowerOutputOffset = new CellOffset(0, 0);
        buildingDef.ViewMode = OverlayModes.Power.ID;
        buildingDef.AudioCategory = "Metal";
        buildingDef.Breakable = true;
        buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
        buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
        buildingDef.SelfHeatKilowattsWhenActive = 0f;
        __result = buildingDef;
        return false;
    }
}
#endif