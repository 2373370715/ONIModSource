using System.Collections.Generic;
using UnityEngine;

public class DebugTool : DragTool {
    public enum Type {
        ReplaceSubstance,
        FillReplaceSubstance,
        Clear,
        AddSelection,
        RemoveSelection,
        Deconstruct,
        Destroy,
        Sample,
        StoreSubstance,
        Dig,
        Heat,
        Cool,
        AddPressure,
        RemovePressure,
        PaintPlant
    }

    public static DebugTool Instance;
    public        Type      type;
    public static void      DestroyInstance() { Instance = null; }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        Instance = this;
    }

    public void Activate() { PlayerController.Instance.ActivateTool(this); }

    public void Activate(Type type) {
        this.type = type;
        Activate();
    }

    protected override void OnDeactivateTool(InterfaceTool new_tool) {
        base.OnDeactivateTool(new_tool);
        PlayerController.Instance.ToolDeactivated(this);
    }

    protected override void OnDragTool(int cell, int distFromOrigin) {
        if (Grid.IsValidCell(cell))
            switch (type) {
                case Type.ReplaceSubstance:
                    DoReplaceSubstance(cell);
                    return;
                case Type.FillReplaceSubstance: {
                    GameUtil.FloodFillNext.Value.Clear();
                    GameUtil.FloodFillVisited.Value.Clear();
                    var elem_hash = Grid.Element[cell].id;
                    GameUtil.FloodFillConditional(cell,
                                                  delegate(int check_cell) {
                                                      var result = false;
                                                      if (Grid.Element[check_cell].id == elem_hash) {
                                                          result = true;
                                                          DoReplaceSubstance(check_cell);
                                                      }

                                                      return result;
                                                  },
                                                  GameUtil.FloodFillVisited.Value);

                    return;
                }
                case Type.Clear:
                    ClearCell(cell);
                    return;
                case Type.AddSelection:
                    DebugBaseTemplateButton.Instance.AddToSelection(cell);
                    return;
                case Type.RemoveSelection:
                    DebugBaseTemplateButton.Instance.RemoveFromSelection(cell);
                    return;
                case Type.Deconstruct:
                    DeconstructCell(cell);
                    return;
                case Type.Destroy:
                    DestroyCell(cell);
                    return;
                case Type.Sample:
                    DebugPaintElementScreen.Instance.SampleCell(cell);
                    return;
                case Type.StoreSubstance:
                    DoStoreSubstance(cell);
                    return;
                case Type.Dig:
                    SimMessages.Dig(cell);
                    return;
                case Type.Heat:
                    SimMessages.ModifyEnergy(cell, 10000f, 10000f, SimMessages.EnergySourceID.DebugHeat);
                    return;
                case Type.Cool:
                    SimMessages.ModifyEnergy(cell, -10000f, 10000f, SimMessages.EnergySourceID.DebugCool);
                    return;
                case Type.AddPressure:
                    SimMessages.ModifyMass(cell,
                                           10000f,
                                           byte.MaxValue,
                                           0,
                                           CellEventLogger.Instance.DebugToolModifyMass,
                                           293f,
                                           SimHashes.Oxygen);

                    return;
                case Type.RemovePressure:
                    SimMessages.ModifyMass(cell,
                                           -10000f,
                                           byte.MaxValue,
                                           0,
                                           CellEventLogger.Instance.DebugToolModifyMass,
                                           0f,
                                           SimHashes.Oxygen);

                    break;
                default:
                    return;
            }
    }

    public void DoReplaceSubstance(int cell) {
        if (!Grid.IsValidBuildingCell(cell)) return;

        var element = DebugPaintElementScreen.Instance.paintElement.isOn
                          ? ElementLoader.FindElementByHash(DebugPaintElementScreen.Instance.element)
                          : ElementLoader.elements[Grid.ElementIdx[cell]];

        if (element == null) element = ElementLoader.FindElementByHash(SimHashes.Vacuum);
        var b = DebugPaintElementScreen.Instance.paintDisease.isOn
                    ? DebugPaintElementScreen.Instance.diseaseIdx
                    : Grid.DiseaseIdx[cell];

        var num = DebugPaintElementScreen.Instance.paintTemperature.isOn
                      ? DebugPaintElementScreen.Instance.temperature
                      : Grid.Temperature[cell];

        var num2 = DebugPaintElementScreen.Instance.paintMass.isOn
                       ? DebugPaintElementScreen.Instance.mass
                       : Grid.Mass[cell];

        var num3 = DebugPaintElementScreen.Instance.paintDiseaseCount.isOn
                       ? DebugPaintElementScreen.Instance.diseaseCount
                       : Grid.DiseaseCount[cell];

        if (num  == -1f) num  = element.defaultValues.temperature;
        if (num2 == -1f) num2 = element.defaultValues.mass;
        if (DebugPaintElementScreen.Instance.affectCells.isOn) {
            SimMessages.ReplaceElement(cell, element.id, CellEventLogger.Instance.DebugTool, num2, num, b, num3);
            if (DebugPaintElementScreen.Instance.set_prevent_fow_reveal) {
                Grid.Visible[cell]               = 0;
                Grid.PreventFogOfWarReveal[cell] = true;
            } else if (DebugPaintElementScreen.Instance.set_allow_fow_reveal && Grid.PreventFogOfWarReveal[cell])
                Grid.PreventFogOfWarReveal[cell] = false;
        }

        if (DebugPaintElementScreen.Instance.affectBuildings.isOn)
            foreach (var gameObject in new List<GameObject> {
                         Grid.Objects[cell, 1],
                         Grid.Objects[cell, 2],
                         Grid.Objects[cell, 9],
                         Grid.Objects[cell, 16],
                         Grid.Objects[cell, 12],
                         Grid.Objects[cell, 16],
                         Grid.Objects[cell, 26]
                     })
                if (gameObject != null) {
                    var component                       = gameObject.GetComponent<PrimaryElement>();
                    if (num > 0f) component.Temperature = num;
                    if (num3 > 0 && b != 255) {
                        component.ModifyDiseaseCount(int.MinValue, "DebugTool.DoReplaceSubstance");
                        component.AddDisease(b, num3, "DebugTool.DoReplaceSubstance");
                    }
                }
    }

    public void DeconstructCell(int cell) {
        var instantBuildMode = DebugHandler.InstantBuildMode;
        DebugHandler.InstantBuildMode = true;
        DeconstructTool.Instance.DeconstructCell(cell);
        if (!instantBuildMode) DebugHandler.InstantBuildMode = false;
    }

    public void DestroyCell(int cell) {
        foreach (var gameObject in new List<GameObject> {
                     Grid.Objects[cell, 2],
                     Grid.Objects[cell, 1],
                     Grid.Objects[cell, 12],
                     Grid.Objects[cell, 16],
                     Grid.Objects[cell, 20],
                     Grid.Objects[cell, 0],
                     Grid.Objects[cell, 26],
                     Grid.Objects[cell, 31],
                     Grid.Objects[cell, 30]
                 })
            if (gameObject != null)
                Util.KDestroyGameObject(gameObject);

        ClearCell(cell);
        if (ElementLoader.elements[Grid.ElementIdx[cell]].id == SimHashes.Void) {
            SimMessages.ReplaceElement(cell, SimHashes.Void, CellEventLogger.Instance.DebugTool, 0f, 0f);
            return;
        }

        SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.DebugTool, 0f, 0f);
    }

    public void ClearCell(int cell) {
        var vector2I   = Grid.CellToXY(cell);
        var pooledList = ListPool<ScenePartitionerEntry, DebugTool>.Allocate();
        GameScenePartitioner.Instance.GatherEntries(vector2I.x,
                                                    vector2I.y,
                                                    1,
                                                    1,
                                                    GameScenePartitioner.Instance.pickupablesLayer,
                                                    pooledList);

        for (var i = 0; i < pooledList.Count; i++) {
            var pickupable = pooledList[i].obj as Pickupable;
            if (pickupable != null && pickupable.GetComponent<MinionBrain>() == null)
                Util.KDestroyGameObject(pickupable.gameObject);
        }

        pooledList.Recycle();
    }

    public void DoStoreSubstance(int cell) {
        if (!Grid.IsValidBuildingCell(cell)) return;

        var gameObject = Grid.Objects[cell, 1];
        if (gameObject == null) return;

        var component = gameObject.GetComponent<Storage>();
        if (component == null) return;

        var element = DebugPaintElementScreen.Instance.paintElement.isOn
                          ? ElementLoader.FindElementByHash(DebugPaintElementScreen.Instance.element)
                          : ElementLoader.elements[Grid.ElementIdx[cell]];

        if (element == null) element = ElementLoader.FindElementByHash(SimHashes.Vacuum);
        var disease_idx = DebugPaintElementScreen.Instance.paintDisease.isOn
                              ? DebugPaintElementScreen.Instance.diseaseIdx
                              : Grid.DiseaseIdx[cell];

        var num = DebugPaintElementScreen.Instance.paintTemperature.isOn
                      ? DebugPaintElementScreen.Instance.temperature
                      : element.defaultValues.temperature;

        var num2 = DebugPaintElementScreen.Instance.paintMass.isOn
                       ? DebugPaintElementScreen.Instance.mass
                       : element.defaultValues.mass;

        if (num  == -1f) num  = element.defaultValues.temperature;
        if (num2 == -1f) num2 = element.defaultValues.mass;
        var disease_count = DebugPaintElementScreen.Instance.paintDiseaseCount.isOn
                                ? DebugPaintElementScreen.Instance.diseaseCount
                                : 0;

        if (element.IsGas) {
            component.AddGasChunk(element.id, num2, num, disease_idx, disease_count, false);
            return;
        }

        if (element.IsLiquid) {
            component.AddLiquid(element.id, num2, num, disease_idx, disease_count);
            return;
        }

        if (element.IsSolid) component.AddOre(element.id, num2, num, disease_idx, disease_count);
    }
}