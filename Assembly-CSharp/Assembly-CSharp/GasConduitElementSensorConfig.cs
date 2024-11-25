using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class GasConduitElementSensorConfig : ConduitSensorConfig {
    public static      string      ID = "GasConduitElementSensor";
    protected override ConduitType ConduitType => ConduitType.Gas;

    public override BuildingDef CreateBuildingDef() {
        var result = base.CreateBuildingDef(ID,
                                            "gas_element_sensor_kanim",
                                            BUILDINGS.CONSTRUCTION_MASS_KG.TIER0,
                                            MATERIALS.REFINED_METALS,
                                            new List<LogicPorts.Port> {
                                                LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID,
                                                                           new CellOffset(0, 0),
                                                                           STRINGS.BUILDINGS.PREFABS
                                                                               .GASCONDUITELEMENTSENSOR.LOGIC_PORT,
                                                                           STRINGS.BUILDINGS.PREFABS
                                                                               .GASCONDUITELEMENTSENSOR
                                                                               .LOGIC_PORT_ACTIVE,
                                                                           STRINGS.BUILDINGS.PREFABS
                                                                               .GASCONDUITELEMENTSENSOR
                                                                               .LOGIC_PORT_INACTIVE,
                                                                           true)
                                            });

        GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, ID);
        return result;
    }

    public override void DoPostConfigureComplete(GameObject go) {
        base.DoPostConfigureComplete(go);
        go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Gas;
        var conduitElementSensor = go.AddOrGet<ConduitElementSensor>();
        conduitElementSensor.manuallyControlled = false;
        conduitElementSensor.conduitType        = ConduitType;
        conduitElementSensor.defaultState       = false;
        go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits);
    }
}