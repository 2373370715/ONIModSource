using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;

public class DevToolSimDebug : DevTool {
    private const    string                                 INVALID_OVERLAY_MODE_STR = "None";
    public static    DevToolSimDebug                        Instance;
    private          Option<DevToolEntityTarget.ForSimCell> boundBoxSimCellTarget;
    private          Dictionary<SimHashes, double>          elementCounts = new Dictionary<SimHashes, double>();
    private readonly string[]                               elementNames;
    private readonly string[]                               gameGridModes;
    private readonly Dictionary<string, HashedString>       modeLookup;
    private readonly string[]                               overlayModes;
    private readonly Dictionary<HashedString, string>       revModeLookup;
    private          int                                    selectedOverlayMode;
    private          bool                                   shouldDrawBoundingBox = true;
    private          bool                                   showAccessRestrictions;
    private          bool                                   showBuildings;
    private          bool                                   showCavityInfo;
    private          bool                                   showCreatures;
    private          bool                                   showElementData;
    private          bool                                   showGasConduitData;
    private          bool                                   showGridContents;
    private          bool                                   showLayerToggles;
    private          bool                                   showLiquidConduitData;
    private          bool                                   showMouseData = true;
    private          bool                                   showPhysicsData;
    private          bool                                   showPropertyInfo;
    private          bool                                   showScenePartitionerContents;
    private readonly HashSet<ScenePartitionerLayer>         toggledLayers = new HashSet<ScenePartitionerLayer>();
    private          Vector3                                worldPos      = Vector3.zero;
    private          int                                    xBound        = 8;
    private          int                                    yBound        = 8;

    public DevToolSimDebug() {
        elementNames = Enum.GetNames(typeof(SimHashes));
        Array.Sort(elementNames);
        Instance = this;
        var list = new List<string>();
        modeLookup    = new Dictionary<string, HashedString>();
        revModeLookup = new Dictionary<HashedString, string>();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        for (var i = 0; i < assemblies.Length; i++)
            foreach (var type in assemblies[i].GetTypes())
                if (typeof(OverlayModes.Mode).IsAssignableFrom(type)) {
                    var field = type.GetField("ID");
                    if (field != null) {
                        var value = field.GetValue(null);
                        if (value != null) {
                            var hashedString = (HashedString)value;
                            list.Add(type.Name);
                            modeLookup[type.Name]       = hashedString;
                            revModeLookup[hashedString] = type.Name;
                        }
                    }
                }

        foreach (var fieldInfo in typeof(SimDebugView.OverlayModes).GetFields())
            if (fieldInfo.FieldType == typeof(HashedString)) {
                var value2 = fieldInfo.GetValue(null);
                if (value2 != null) {
                    var hashedString2 = (HashedString)value2;
                    list.Add(fieldInfo.Name);
                    modeLookup[fieldInfo.Name]   = hashedString2;
                    revModeLookup[hashedString2] = fieldInfo.Name;
                }
            }

        list.Sort();
        list.Insert(0, "None");
        modeLookup["None"]    = "None";
        revModeLookup["None"] = "None";
        list.RemoveAll(s => s == null);
        overlayModes  = list.ToArray();
        gameGridModes = Enum.GetNames(typeof(SimDebugView.GameGridMode));
    }

    protected override void RenderTo(DevPanel panel) {
        if (Game.Instance == null) return;

        var hashedString = SimDebugView.Instance.GetMode();
        var y            = hashedString;
        if (overlayModes != null) {
            selectedOverlayMode = Array.IndexOf(overlayModes, revModeLookup[hashedString]);
            selectedOverlayMode = selectedOverlayMode == -1 ? 0 : selectedOverlayMode;
            ImGui.Combo("Debug Mode", ref selectedOverlayMode, overlayModes, overlayModes.Length);
            hashedString = modeLookup[overlayModes[selectedOverlayMode]];
            if (hashedString == "None") hashedString = OverlayModes.None.ID;
        }

        if (hashedString != y) SimDebugView.Instance.SetMode(hashedString);
        if (hashedString == OverlayModes.Temperature.ID) {
            ImGui.InputFloat("Min Expected Temp:", ref SimDebugView.Instance.minTempExpected);
            ImGui.InputFloat("Max Expected Temp:", ref SimDebugView.Instance.maxTempExpected);
        } else if (hashedString == SimDebugView.OverlayModes.Mass) {
            ImGui.InputFloat("Min Expected Mass:", ref SimDebugView.Instance.minMassExpected);
            ImGui.InputFloat("Max Expected Mass:", ref SimDebugView.Instance.maxMassExpected);
        } else if (hashedString == SimDebugView.OverlayModes.Pressure) {
            ImGui.InputFloat("Min Expected Pressure:", ref SimDebugView.Instance.minPressureExpected);
            ImGui.InputFloat("Max Expected Pressure:", ref SimDebugView.Instance.maxPressureExpected);
        } else if (hashedString == SimDebugView.OverlayModes.GameGrid) {
            var gameGridMode = (int)SimDebugView.Instance.GetGameGridMode();
            ImGui.Combo("Grid Mode", ref gameGridMode, gameGridModes, gameGridModes.Length);
            SimDebugView.Instance.SetGameGridMode((SimDebugView.GameGridMode)gameGridMode);
        }

        int num;
        int num2;
        Grid.PosToXY(worldPos, out num, out num2);
        var num3 = num2 * Grid.WidthInCells + num;
        ImGui.Checkbox("Draw Bounding Box", ref shouldDrawBoundingBox);
        if (ImGui.CollapsingHeader("Overlay Box") && shouldDrawBoundingBox) {
            if (ImGui.Button("Pick cell"))
                panel.PushDevTool(new DevToolEntity_EyeDrop(delegate(DevToolEntityTarget target) {
                                                                boundBoxSimCellTarget
                                                                    = (DevToolEntityTarget.ForSimCell)target;
                                                            },
                                                            delegate(DevToolEntityTarget uncastTarget) {
                                                                if (!(uncastTarget is DevToolEntityTarget.ForSimCell))
                                                                    return "Target is not a sim cell";

                                                                return Option.None;
                                                            }));

            DrawBoundingBoxOverlay();
        }

        showMouseData = ImGui.CollapsingHeader("Mouse Data");
        if (showMouseData) {
            ImGui.Indent();
            var str    = "WorldPos: ";
            var vector = worldPos;
            ImGui.Text(str + vector);
            ImGui.Unindent();
        }

        if (num3 < 0 || Grid.CellCount <= num3) return;

        if (showMouseData) {
            ImGui.Indent();
            ImGui.Text("CellPos: "                          + num + ", " + num2);
            var num4 = (num2 + 1) * (Grid.WidthInCells + 2) + num + 1;
            if (ImGui.InputInt("Sim Cell:", ref num4)) {
                num      = Mathf.Max(0, num4 % (Grid.WidthInCells + 2) - 1);
                num2     = Mathf.Max(0, num4 / (Grid.WidthInCells + 2) - 1);
                worldPos = Grid.CellToPosCCC(Grid.XYToCell(num, num2), Grid.SceneLayer.Front);
            }

            if (ImGui.InputInt("Game Cell:", ref num3)) {
                num      = num3 % Grid.WidthInCells;
                num2     = num3 / Grid.WidthInCells;
                worldPos = Grid.CellToPosCCC(Grid.XYToCell(num, num2), Grid.SceneLayer.Front);
            }

            var num5 = Grid.WidthInCells / 32;
            var num6 = num               / 32;
            var num7 = num2              / 32;
            var num8 = num7 * num5 + num6;
            ImGui.Text(string.Format("Chunk Idx ({0}, {1}): {2}", num6, num7, num8));
            ImGui.Text("RenderedByWorld: "         + Grid.RenderedByWorld[num3]);
            ImGui.Text("Solid: "                   + Grid.Solid[num3]);
            ImGui.Text("Damage: "                  + Grid.Damage[num3]);
            ImGui.Text("Foundation: "              + Grid.Foundation[num3]);
            ImGui.Text("Revealed: "                + Grid.Revealed[num3]);
            ImGui.Text("Visible: "                 + Grid.Visible[num3]);
            ImGui.Text("DupePassable: "            + Grid.DupePassable[num3]);
            ImGui.Text("DupeImpassable: "          + Grid.DupeImpassable[num3]);
            ImGui.Text("CritterImpassable: "       + Grid.CritterImpassable[num3]);
            ImGui.Text("FakeFloor: "               + Grid.FakeFloor[num3]);
            ImGui.Text("HasDoor: "                 + Grid.HasDoor[num3]);
            ImGui.Text("HasLadder: "               + Grid.HasLadder[num3]);
            ImGui.Text("HasPole: "                 + Grid.HasPole[num3]);
            ImGui.Text("GravitasFacility: "        + Grid.GravitasFacility[num3]);
            ImGui.Text("HasNavTeleporter: "        + Grid.HasNavTeleporter[num3]);
            ImGui.Text("IsTileUnderConstruction: " + Grid.IsTileUnderConstruction[num3]);
            ImGui.Text("LiquidVisPlacers: "        + Game.Instance.liquidConduitSystem.GetConnections(num3, false));
            ImGui.Text("LiquidPhysPlacers: "       + Game.Instance.liquidConduitSystem.GetConnections(num3, true));
            ImGui.Text("GasVisPlacers: "           + Game.Instance.gasConduitSystem.GetConnections(num3, false));
            ImGui.Text("GasPhysPlacers: "          + Game.Instance.gasConduitSystem.GetConnections(num3, true));
            ImGui.Text("ElecVisPlacers: "          + Game.Instance.electricalConduitSystem.GetConnections(num3, false));
            ImGui.Text("ElecPhysPlacers: "         + Game.Instance.electricalConduitSystem.GetConnections(num3, true));
            ImGui.Text("World Idx: "               + Grid.WorldIdx[num3]);
            ImGui.Text("ZoneType: "                + World.Instance.zoneRenderData.GetSubWorldZoneType(num3));
            ImGui.Text("Light Intensity: "         + Grid.LightIntensity[num3]);
            ImGui.Text("Sunlight: "                + Grid.ExposedToSunlight[num3]);
            ImGui.Text("Radiation: "               + Grid.Radiation[num3]);
            showAccessRestrictions = ImGui.CollapsingHeader("Access Restrictions");
            if (showAccessRestrictions) {
                ImGui.Indent();
                Grid.Restriction restriction;
                if (!Grid.DEBUG_GetRestrictions(num3, out restriction))
                    ImGui.Text("No access control.");
                else {
                    ImGui.Text("Orientation: "         + restriction.orientation);
                    ImGui.Text("Default Restriction: " + restriction.DirectionMasksForMinionInstanceID[-1]);
                    ImGui.Indent();
                    foreach (var minionIdentity in Components.LiveMinionIdentities.Items) {
                        var instanceID = minionIdentity.GetComponent<MinionIdentity>()
                                                       .assignableProxy.Get()
                                                       .GetComponent<KPrefabID>()
                                                       .InstanceID;

                        Grid.Restriction.Directions directions;
                        if (restriction.DirectionMasksForMinionInstanceID.TryGetValue(instanceID, out directions))
                            ImGui.Text(minionIdentity.name + " Restriction: " + directions);
                        else
                            ImGui.Text(minionIdentity.name + ": Has No restriction");
                    }

                    ImGui.Unindent();
                }

                ImGui.Unindent();
            }

            showGridContents = ImGui.CollapsingHeader("Grid Objects");
            if (showGridContents) {
                ImGui.Indent();
                for (var i = 0; i < 45; i++) {
                    var gameObject = Grid.Objects[num3, i];
                    ImGui.Text(Enum.GetName(typeof(ObjectLayer), i) +
                               ": "                                 +
                               (gameObject != null ? gameObject.name : "None"));
                }

                ImGui.Unindent();
            }

            showScenePartitionerContents = ImGui.CollapsingHeader("Scene Partitioner");
            if (showScenePartitionerContents) {
                ImGui.Indent();
                if (GameScenePartitioner.Instance != null) {
                    showLayerToggles = ImGui.CollapsingHeader("Layers");
                    if (showLayerToggles) {
                        var flag = false;
                        foreach (var scenePartitionerLayer in GameScenePartitioner.Instance.GetLayers()) {
                            var flag2 = toggledLayers.Contains(scenePartitionerLayer);
                            var flag3 = flag2;
                            ImGui.Checkbox(HashCache.Get().Get(scenePartitionerLayer.name), ref flag3);
                            if (flag3 != flag2) {
                                flag = true;
                                if (flag3)
                                    toggledLayers.Add(scenePartitionerLayer);
                                else
                                    toggledLayers.Remove(scenePartitionerLayer);
                            }
                        }

                        if (flag) {
                            GameScenePartitioner.Instance.SetToggledLayers(toggledLayers);
                            if (toggledLayers.Count > 0)
                                SimDebugView.Instance.SetMode(SimDebugView.OverlayModes.ScenePartitioner);
                        }
                    }

                    var pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
                    foreach (var layer in GameScenePartitioner.Instance.GetLayers()) {
                        pooledList.Clear();
                        GameScenePartitioner.Instance.GatherEntries(num, num2, 1, 1, layer, pooledList);
                        foreach (var scenePartitionerEntry in pooledList) {
                            var gameObject2   = scenePartitionerEntry.obj as GameObject;
                            var monoBehaviour = scenePartitionerEntry.obj as MonoBehaviour;
                            if (gameObject2 != null)
                                ImGui.Text(gameObject2.name);
                            else if (monoBehaviour != null) ImGui.Text(monoBehaviour.name);
                        }
                    }

                    pooledList.Recycle();
                }

                ImGui.Unindent();
            }

            showCavityInfo = ImGui.CollapsingHeader("Cavity Info");
            if (showCavityInfo) {
                ImGui.Indent();
                CavityInfo cavityInfo = null;
                if (Game.Instance != null && Game.Instance.roomProber != null)
                    cavityInfo = Game.Instance.roomProber.GetCavityForCell(num3);

                if (cavityInfo != null) {
                    ImGui.Text("Cell Count: " + cavityInfo.numCells);
                    var room = cavityInfo.room;
                    if (room != null) {
                        ImGui.Text("Is Room: True");
                        showBuildings = ImGui.CollapsingHeader("Buildings (" + room.buildings.Count + ")");
                        if (showBuildings)
                            foreach (var kprefabID in room.buildings)
                                ImGui.Text(kprefabID.ToString());

                        showCreatures = ImGui.CollapsingHeader("Creatures (" + room.cavity.creatures.Count + ")");
                        if (!showCreatures) goto IL_CC0;

                        using (var enumerator4 = room.cavity.creatures.GetEnumerator()) {
                            while (enumerator4.MoveNext()) {
                                var kprefabID2 = enumerator4.Current;
                                ImGui.Text(kprefabID2.ToString());
                            }

                            goto IL_CC0;
                        }
                    }

                    ImGui.Text("Is Room: False");
                } else
                    ImGui.Text("No Cavity Detected");

                IL_CC0:
                ImGui.Unindent();
            }

            showPropertyInfo = ImGui.CollapsingHeader("Property Info");
            if (showPropertyInfo) {
                ImGui.Indent();
                var flag4 = true;
                var b     = Grid.Properties[num3];
                foreach (var obj in Enum.GetValues(typeof(Sim.Cell.Properties)))
                    if ((b & (int)obj) != 0) {
                        ImGui.Text(obj.ToString());
                        flag4 = false;
                    }

                if (flag4) ImGui.Text("No properties");
                ImGui.Unindent();
            }

            ImGui.Unindent();
        }

        if (Grid.ObjectLayers != null) {
            var element = Grid.Element[num3];
            showElementData = ImGui.CollapsingHeader("Element");
            ImGui.SameLine();
            ImGui.Text("[" + element.name + "]");
            ImGui.Indent();
            ImGui.Text("Mass:" + Grid.Mass[num3]);
            if (showElementData) DrawElem(element);
            ImGui.Text("Average Flow Rate (kg/s):" + Grid.AccumulatedFlow[num3] / 3f);
            ImGui.Unindent();
        }

        showPhysicsData = ImGui.CollapsingHeader("Physics Data");
        if (showPhysicsData) {
            ImGui.Indent();
            ImGui.Text("Solid: "                        + Grid.Solid[num3]);
            ImGui.Text("Pressure: "                     + Grid.Pressure[num3]);
            ImGui.Text("Temperature (kelvin -272.15): " + Grid.Temperature[num3]);
            ImGui.Text("Radiation: "                    + Grid.Radiation[num3]);
            ImGui.Text("Mass: "                         + Grid.Mass[num3]);
            ImGui.Text("Insulation: "                   + Grid.Insulation[num3] / 255f);
            ImGui.Text("Strength Multiplier: "          + Grid.StrengthInfo[num3]);
            ImGui.Text("Properties: 0x: "               + Grid.Properties[num3].ToString("X"));
            ImGui.Text("Disease: " +
                       (Grid.DiseaseIdx[num3] == byte.MaxValue
                            ? "None"
                            : Db.Get().Diseases[Grid.DiseaseIdx[num3]].Name));

            ImGui.Text("Disease Count: " + Grid.DiseaseCount[num3]);
            ImGui.Unindent();
        }

        showGasConduitData = ImGui.CollapsingHeader("Gas Conduit Data");
        if (showGasConduitData) DrawConduitFlow(Game.Instance.gasConduitFlow, num3);
        showLiquidConduitData = ImGui.CollapsingHeader("Liquid Conduit Data");
        if (showLiquidConduitData) DrawConduitFlow(Game.Instance.liquidConduitFlow, num3);
    }

    private void DrawElem(Element element) {
        ImGui.Indent();
        ImGui.Text("State: "                  + element.state);
        ImGui.Text("Thermal Conductivity: "   + element.thermalConductivity);
        ImGui.Text("Specific Heat Capacity: " + element.specificHeatCapacity);
        if (element.lowTempTransition != null) {
            ImGui.Text("Low Temperature: "            + element.lowTemp);
            ImGui.Text("Low Temperature Transition: " + element.lowTempTransitionTarget);
        }

        if (element.highTempTransition != null) {
            ImGui.Text("High Temperature: "                + element.highTemp);
            ImGui.Text("HighTemp Temperature Transition: " + element.highTempTransitionTarget);
            if (element.highTempTransitionOreID != 0)
                ImGui.Text("HighTemp Temperature Transition: " + element.highTempTransitionOreID);
        }

        ImGui.Text("Light Absorption Factor: "     + element.lightAbsorptionFactor);
        ImGui.Text("Radiation Absorption Factor: " + element.radiationAbsorptionFactor);
        ImGui.Text("Radiation Per 1000 Mass: "     + element.radiationPer1000Mass);
        ImGui.Text("Sublimate ID: "                + element.sublimateId);
        ImGui.Text("Sublimate FX: "                + element.sublimateFX);
        ImGui.Text("Sublimate Rate: "              + element.sublimateRate);
        ImGui.Text("Sublimate Efficiency: "        + element.sublimateEfficiency);
        ImGui.Text("Sublimate Probability: "       + element.sublimateProbability);
        ImGui.Text("Off Gas Percentage: "          + element.offGasPercentage);
        if (element.IsGas)
            ImGui.Text("Default Pressure: " + element.defaultValues.pressure);
        else
            ImGui.Text("Default Mass: " + element.defaultValues.mass);

        ImGui.Text("Default Temperature: " + element.defaultValues.temperature);
        if (element.IsGas) ImGui.Text("Flow: " + element.flow);
        if (element.IsLiquid) {
            ImGui.Text("Max Comp: " + element.maxCompression);
            ImGui.Text("Max Mass: " + element.maxMass);
        }

        if (element.IsSolid) {
            ImGui.Text("Hardness: " + element.hardness);
            ImGui.Text("Unstable: " + element.IsUnstable);
        }

        ImGui.Unindent();
    }

    private void DrawConduitFlow(ConduitFlow flow_mgr, int cell) {
        ImGui.Indent();
        var contents = flow_mgr.GetContents(cell);
        ImGui.Text("Element: " + contents.element);
        ImGui.Text(string.Format("Mass: {0}",         contents.mass));
        ImGui.Text(string.Format("Movable Mass: {0}", contents.movable_mass));
        ImGui.Text("Temperature: " + contents.temperature);
        ImGui.Text("Disease: " +
                   (contents.diseaseIdx == byte.MaxValue ? "None" : Db.Get().Diseases[contents.diseaseIdx].Name));

        ImGui.Text("Disease Count: " + contents.diseaseCount);
        ImGui.Text(string.Format("Update Order: {0}", flow_mgr.ComputeUpdateOrder(cell)));
        flow_mgr.SetContents(cell, contents);
        var permittedFlow = flow_mgr.GetPermittedFlow(cell);
        if (permittedFlow == ConduitFlow.FlowDirections.None)
            ImGui.Text("PermittedFlow: None");
        else {
            var text                                                                                       = "";
            if ((permittedFlow & ConduitFlow.FlowDirections.Up)   != ConduitFlow.FlowDirections.None) text += " Up ";
            if ((permittedFlow & ConduitFlow.FlowDirections.Down) != ConduitFlow.FlowDirections.None) text += " Down ";
            if ((permittedFlow & ConduitFlow.FlowDirections.Left) != ConduitFlow.FlowDirections.None) text += " Left ";
            if ((permittedFlow & ConduitFlow.FlowDirections.Right) != ConduitFlow.FlowDirections.None)
                text += " Right ";

            ImGui.Text("PermittedFlow: " + text);
        }

        ImGui.Unindent();
    }

    private void DrawBoundingBoxOverlay() {
        ImGui.InputInt("Width:",  ref xBound, 2);
        ImGui.InputInt("Height:", ref yBound, 2);
        var vector2I = boundBoxSimCellTarget.HasValue
                           ? Grid.CellToXY(boundBoxSimCellTarget.Unwrap().cellIndex)
                           : Grid.PosToXY(worldPos);

        var vector2I2 = new Vector2I(Math.Max(0, vector2I.x - xBound / 2), Math.Max(0, vector2I.y - yBound / 2));
        var vector2I3 = new Vector2I(Math.Min(vector2I.x + xBound / 2, Grid.WidthInCells),
                                     Math.Min(vector2I.y + yBound / 2, Grid.HeightInCells));

        var screenRect  = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(vector2I2.X, vector2I2.Y)).GetScreenRect();
        var screenRect2 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(vector2I3.X, vector2I3.Y)).GetScreenRect();
        if (screenRect.IsSome() && screenRect2.IsSome())
            for (var i = vector2I2.Y; i <= vector2I3.Y; i++) {
                for (var j = vector2I2.X; j <= vector2I3.X; j++) {
                    var screenRect3 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(j, i)).GetScreenRect();
                    var screenRect4 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(j, i)).GetScreenRect();
                    var screenRect5
                        = new ValueTuple<Vector2, Vector2>(screenRect3.Unwrap().Item1, screenRect4.Unwrap().Item2);

                    var value = Grid.XYToCell(j, i).ToString();
                    DevToolEntity.DrawScreenRect(screenRect5,
                                                 value,
                                                 new Color(1f, 1f, 1f, 0.7f),
                                                 new Color(1f, 1f, 1f, 0.2f),
                                                 new Option<DevToolUtil.TextAlignment>(DevToolUtil.TextAlignment
                                                     .Center));
                }
            }
    }

    public void SetCell(int cell) { worldPos = Grid.CellToPosCCC(cell, Grid.SceneLayer.Move); }
}