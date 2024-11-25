using TUNING;
using UnityEngine;

public abstract class LogicGateBaseConfig : IBuildingConfig {
    protected abstract CellOffset[] InputPortOffsets   { get; }
    protected abstract CellOffset[] OutputPortOffsets  { get; }
    protected abstract CellOffset[] ControlPortOffsets { get; }

    protected BuildingDef CreateBuildingDef(string ID, string anim, int width = 2, int height = 2) {
        var hitpoints           = 10;
        var construction_time   = 3f;
        var tier                = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
        var refined_METALS      = MATERIALS.REFINED_METALS;
        var melting_point       = 1600f;
        var build_location_rule = BuildLocationRule.Anywhere;
        var none                = NOISE_POLLUTION.NONE;
        var buildingDef = BuildingTemplates.CreateBuildingDef(ID,
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
                                                              none);

        buildingDef.ViewMode            = OverlayModes.Logic.ID;
        buildingDef.ObjectLayer         = ObjectLayer.LogicGate;
        buildingDef.SceneLayer          = Grid.SceneLayer.LogicGates;
        buildingDef.ThermalConductivity = 0.05f;
        buildingDef.Floodable           = false;
        buildingDef.Overheatable        = false;
        buildingDef.Entombable          = false;
        buildingDef.AudioCategory       = "Metal";
        buildingDef.AudioSize           = "small";
        buildingDef.BaseTimeUntilRepair = -1f;
        buildingDef.PermittedRotations  = PermittedRotations.R360;
        buildingDef.DragBuild           = true;
        LogicGateBase.uiSrcData         = Assets.instance.logicModeUIData;
        GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
        return buildingDef;
    }

    protected abstract LogicGateBase.Op                GetLogicOp();
    protected abstract LogicGate.LogicGateDescriptions GetDescriptions();

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
        GeneratedBuildings.MakeBuildingAlwaysOperational(go);
        BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
    }

    public override void DoPostConfigurePreview(BuildingDef def, GameObject go) {
        base.DoPostConfigurePreview(def, go);
        var moveableLogicGateVisualizer = go.AddComponent<MoveableLogicGateVisualizer>();
        moveableLogicGateVisualizer.op                 = GetLogicOp();
        moveableLogicGateVisualizer.inputPortOffsets   = InputPortOffsets;
        moveableLogicGateVisualizer.outputPortOffsets  = OutputPortOffsets;
        moveableLogicGateVisualizer.controlPortOffsets = ControlPortOffsets;
    }

    public override void DoPostConfigureUnderConstruction(GameObject go) {
        base.DoPostConfigureUnderConstruction(go);
        var logicGateVisualizer = go.AddComponent<LogicGateVisualizer>();
        logicGateVisualizer.op                 = GetLogicOp();
        logicGateVisualizer.inputPortOffsets   = InputPortOffsets;
        logicGateVisualizer.outputPortOffsets  = OutputPortOffsets;
        logicGateVisualizer.controlPortOffsets = ControlPortOffsets;
    }

    public override void DoPostConfigureComplete(GameObject go) {
        var logicGate = go.AddComponent<LogicGate>();
        logicGate.op                 = GetLogicOp();
        logicGate.inputPortOffsets   = InputPortOffsets;
        logicGate.outputPortOffsets  = OutputPortOffsets;
        logicGate.controlPortOffsets = ControlPortOffsets;
        go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object) {
                                                         game_object.GetComponent<LogicGate>()
                                                                    .SetPortDescriptions(GetDescriptions());
                                                     };

        go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits);
    }
}