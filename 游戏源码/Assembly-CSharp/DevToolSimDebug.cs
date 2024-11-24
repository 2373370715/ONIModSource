using System;
using System.Collections.Generic;
using System.Reflection;
using ImGuiNET;
using UnityEngine;

public class DevToolSimDebug : DevTool
{
	private Vector3 worldPos = Vector3.zero;

	private string[] elementNames;

	private Dictionary<SimHashes, double> elementCounts = new Dictionary<SimHashes, double>();

	public static DevToolSimDebug Instance;

	private const string INVALID_OVERLAY_MODE_STR = "None";

	private bool shouldDrawBoundingBox = true;

	private Option<DevToolEntityTarget.ForSimCell> boundBoxSimCellTarget;

	private int xBound = 8;

	private int yBound = 8;

	private bool showElementData;

	private bool showMouseData = true;

	private bool showAccessRestrictions;

	private bool showGridContents;

	private bool showScenePartitionerContents;

	private bool showLayerToggles;

	private bool showCavityInfo;

	private bool showPropertyInfo;

	private bool showBuildings;

	private bool showCreatures;

	private bool showPhysicsData;

	private bool showGasConduitData;

	private bool showLiquidConduitData;

	private string[] overlayModes;

	private int selectedOverlayMode;

	private string[] gameGridModes;

	private Dictionary<string, HashedString> modeLookup;

	private Dictionary<HashedString, string> revModeLookup;

	private HashSet<ScenePartitionerLayer> toggledLayers = new HashSet<ScenePartitionerLayer>();

	public DevToolSimDebug()
	{
		elementNames = Enum.GetNames(typeof(SimHashes));
		Array.Sort(elementNames);
		Instance = this;
		List<string> list = new List<string>();
		modeLookup = new Dictionary<string, HashedString>();
		revModeLookup = new Dictionary<HashedString, string>();
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < assemblies.Length; i++)
		{
			Type[] types = assemblies[i].GetTypes();
			foreach (Type type in types)
			{
				if (!typeof(OverlayModes.Mode).IsAssignableFrom(type))
				{
					continue;
				}
				FieldInfo field = type.GetField("ID");
				if (field != null)
				{
					object value = field.GetValue(null);
					if (value != null)
					{
						HashedString hashedString = (HashedString)value;
						list.Add(type.Name);
						modeLookup[type.Name] = hashedString;
						revModeLookup[hashedString] = type.Name;
					}
				}
			}
		}
		FieldInfo[] fields = typeof(SimDebugView.OverlayModes).GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			if (fieldInfo.FieldType == typeof(HashedString))
			{
				object value2 = fieldInfo.GetValue(null);
				if (value2 != null)
				{
					HashedString hashedString2 = (HashedString)value2;
					list.Add(fieldInfo.Name);
					modeLookup[fieldInfo.Name] = hashedString2;
					revModeLookup[hashedString2] = fieldInfo.Name;
				}
			}
		}
		list.Sort();
		list.Insert(0, "None");
		modeLookup["None"] = "None";
		revModeLookup["None"] = "None";
		list.RemoveAll((string s) => s == null);
		overlayModes = list.ToArray();
		gameGridModes = Enum.GetNames(typeof(SimDebugView.GameGridMode));
	}

	protected override void RenderTo(DevPanel panel)
	{
		if (Game.Instance == null)
		{
			return;
		}
		HashedString hashedString = SimDebugView.Instance.GetMode();
		HashedString hashedString2 = hashedString;
		if (overlayModes != null)
		{
			selectedOverlayMode = Array.IndexOf(overlayModes, revModeLookup[hashedString]);
			selectedOverlayMode = ((selectedOverlayMode != -1) ? selectedOverlayMode : 0);
			ImGui.Combo("Debug Mode", ref selectedOverlayMode, overlayModes, overlayModes.Length);
			hashedString = modeLookup[overlayModes[selectedOverlayMode]];
			if (hashedString == "None")
			{
				hashedString = OverlayModes.None.ID;
			}
		}
		if (hashedString != hashedString2)
		{
			SimDebugView.Instance.SetMode(hashedString);
		}
		if (hashedString == OverlayModes.Temperature.ID)
		{
			ImGui.InputFloat("Min Expected Temp:", ref SimDebugView.Instance.minTempExpected);
			ImGui.InputFloat("Max Expected Temp:", ref SimDebugView.Instance.maxTempExpected);
		}
		else if (hashedString == SimDebugView.OverlayModes.Mass)
		{
			ImGui.InputFloat("Min Expected Mass:", ref SimDebugView.Instance.minMassExpected);
			ImGui.InputFloat("Max Expected Mass:", ref SimDebugView.Instance.maxMassExpected);
		}
		else if (hashedString == SimDebugView.OverlayModes.Pressure)
		{
			ImGui.InputFloat("Min Expected Pressure:", ref SimDebugView.Instance.minPressureExpected);
			ImGui.InputFloat("Max Expected Pressure:", ref SimDebugView.Instance.maxPressureExpected);
		}
		else if (hashedString == SimDebugView.OverlayModes.GameGrid)
		{
			int current_item = (int)SimDebugView.Instance.GetGameGridMode();
			ImGui.Combo("Grid Mode", ref current_item, gameGridModes, gameGridModes.Length);
			SimDebugView.Instance.SetGameGridMode((SimDebugView.GameGridMode)current_item);
		}
		Grid.PosToXY(worldPos, out var x, out var y);
		int v = y * Grid.WidthInCells + x;
		ImGui.Checkbox("Draw Bounding Box", ref shouldDrawBoundingBox);
		if (ImGui.CollapsingHeader("Overlay Box") && shouldDrawBoundingBox)
		{
			if (ImGui.Button("Pick cell"))
			{
				panel.PushDevTool(new DevToolEntity_EyeDrop(delegate(DevToolEntityTarget target)
				{
					boundBoxSimCellTarget = (DevToolEntityTarget.ForSimCell)target;
				}, (DevToolEntityTarget uncastTarget) => (!(uncastTarget is DevToolEntityTarget.ForSimCell)) ? ((Option<string>)"Target is not a sim cell") : ((Option<string>)Option.None)));
			}
			DrawBoundingBoxOverlay();
		}
		showMouseData = ImGui.CollapsingHeader("Mouse Data");
		if (showMouseData)
		{
			ImGui.Indent();
			Vector3 vector = worldPos;
			ImGui.Text("WorldPos: " + vector.ToString());
			ImGui.Unindent();
		}
		if (v < 0 || Grid.CellCount <= v)
		{
			return;
		}
		if (showMouseData)
		{
			ImGui.Indent();
			ImGui.Text("CellPos: " + x + ", " + y);
			int v2 = (y + 1) * (Grid.WidthInCells + 2) + (x + 1);
			if (ImGui.InputInt("Sim Cell:", ref v2))
			{
				x = Mathf.Max(0, v2 % (Grid.WidthInCells + 2) - 1);
				y = Mathf.Max(0, v2 / (Grid.WidthInCells + 2) - 1);
				worldPos = Grid.CellToPosCCC(Grid.XYToCell(x, y), Grid.SceneLayer.Front);
			}
			if (ImGui.InputInt("Game Cell:", ref v))
			{
				x = v % Grid.WidthInCells;
				y = v / Grid.WidthInCells;
				worldPos = Grid.CellToPosCCC(Grid.XYToCell(x, y), Grid.SceneLayer.Front);
			}
			int num = Grid.WidthInCells / 32;
			int num2 = x / 32;
			int num3 = y / 32;
			int num4 = num3 * num + num2;
			ImGui.Text($"Chunk Idx ({num2}, {num3}): {num4}");
			ImGui.Text("RenderedByWorld: " + Grid.RenderedByWorld[v]);
			ImGui.Text("Solid: " + Grid.Solid[v]);
			ImGui.Text("Damage: " + Grid.Damage[v]);
			ImGui.Text("Foundation: " + Grid.Foundation[v]);
			ImGui.Text("Revealed: " + Grid.Revealed[v]);
			ImGui.Text("Visible: " + Grid.Visible[v]);
			ImGui.Text("DupePassable: " + Grid.DupePassable[v]);
			ImGui.Text("DupeImpassable: " + Grid.DupeImpassable[v]);
			ImGui.Text("CritterImpassable: " + Grid.CritterImpassable[v]);
			ImGui.Text("FakeFloor: " + Grid.FakeFloor[v]);
			ImGui.Text("HasDoor: " + Grid.HasDoor[v]);
			ImGui.Text("HasLadder: " + Grid.HasLadder[v]);
			ImGui.Text("HasPole: " + Grid.HasPole[v]);
			ImGui.Text("GravitasFacility: " + Grid.GravitasFacility[v]);
			ImGui.Text("HasNavTeleporter: " + Grid.HasNavTeleporter[v]);
			ImGui.Text("IsTileUnderConstruction: " + Grid.IsTileUnderConstruction[v]);
			ImGui.Text("LiquidVisPlacers: " + Game.Instance.liquidConduitSystem.GetConnections(v, is_physical_building: false));
			ImGui.Text("LiquidPhysPlacers: " + Game.Instance.liquidConduitSystem.GetConnections(v, is_physical_building: true));
			ImGui.Text("GasVisPlacers: " + Game.Instance.gasConduitSystem.GetConnections(v, is_physical_building: false));
			ImGui.Text("GasPhysPlacers: " + Game.Instance.gasConduitSystem.GetConnections(v, is_physical_building: true));
			ImGui.Text("ElecVisPlacers: " + Game.Instance.electricalConduitSystem.GetConnections(v, is_physical_building: false));
			ImGui.Text("ElecPhysPlacers: " + Game.Instance.electricalConduitSystem.GetConnections(v, is_physical_building: true));
			ImGui.Text("World Idx: " + Grid.WorldIdx[v]);
			ImGui.Text("ZoneType: " + World.Instance.zoneRenderData.GetSubWorldZoneType(v));
			ImGui.Text("Light Intensity: " + Grid.LightIntensity[v]);
			ImGui.Text("Sunlight: " + Grid.ExposedToSunlight[v]);
			ImGui.Text("Radiation: " + Grid.Radiation[v]);
			showAccessRestrictions = ImGui.CollapsingHeader("Access Restrictions");
			if (showAccessRestrictions)
			{
				ImGui.Indent();
				if (!Grid.DEBUG_GetRestrictions(v, out var restriction))
				{
					ImGui.Text("No access control.");
				}
				else
				{
					ImGui.Text("Orientation: " + restriction.orientation);
					ImGui.Text("Default Restriction: " + restriction.DirectionMasksForMinionInstanceID[-1]);
					ImGui.Indent();
					foreach (MinionIdentity item in Components.LiveMinionIdentities.Items)
					{
						int instanceID = item.GetComponent<MinionIdentity>().assignableProxy.Get().GetComponent<KPrefabID>().InstanceID;
						if (restriction.DirectionMasksForMinionInstanceID.TryGetValue(instanceID, out var value))
						{
							ImGui.Text(item.name + " Restriction: " + value);
						}
						else
						{
							ImGui.Text(item.name + ": Has No restriction");
						}
					}
					ImGui.Unindent();
				}
				ImGui.Unindent();
			}
			showGridContents = ImGui.CollapsingHeader("Grid Objects");
			if (showGridContents)
			{
				ImGui.Indent();
				for (int i = 0; i < 45; i++)
				{
					GameObject gameObject = Grid.Objects[v, i];
					ImGui.Text(Enum.GetName(typeof(ObjectLayer), i) + ": " + ((gameObject != null) ? gameObject.name : "None"));
				}
				ImGui.Unindent();
			}
			showScenePartitionerContents = ImGui.CollapsingHeader("Scene Partitioner");
			if (showScenePartitionerContents)
			{
				ImGui.Indent();
				if (GameScenePartitioner.Instance != null)
				{
					showLayerToggles = ImGui.CollapsingHeader("Layers");
					if (showLayerToggles)
					{
						bool flag = false;
						foreach (ScenePartitionerLayer layer in GameScenePartitioner.Instance.GetLayers())
						{
							bool flag2 = toggledLayers.Contains(layer);
							bool v3 = flag2;
							ImGui.Checkbox(HashCache.Get().Get(layer.name), ref v3);
							if (v3 != flag2)
							{
								flag = true;
								if (v3)
								{
									toggledLayers.Add(layer);
								}
								else
								{
									toggledLayers.Remove(layer);
								}
							}
						}
						if (flag)
						{
							GameScenePartitioner.Instance.SetToggledLayers(toggledLayers);
							if (toggledLayers.Count > 0)
							{
								SimDebugView.Instance.SetMode(SimDebugView.OverlayModes.ScenePartitioner);
							}
						}
					}
					ListPool<ScenePartitionerEntry, ScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, ScenePartitioner>.Allocate();
					foreach (ScenePartitionerLayer layer2 in GameScenePartitioner.Instance.GetLayers())
					{
						pooledList.Clear();
						GameScenePartitioner.Instance.GatherEntries(x, y, 1, 1, layer2, pooledList);
						foreach (ScenePartitionerEntry item2 in pooledList)
						{
							GameObject gameObject2 = item2.obj as GameObject;
							MonoBehaviour monoBehaviour = item2.obj as MonoBehaviour;
							if (gameObject2 != null)
							{
								ImGui.Text(gameObject2.name);
							}
							else if (monoBehaviour != null)
							{
								ImGui.Text(monoBehaviour.name);
							}
						}
					}
					pooledList.Recycle();
				}
				ImGui.Unindent();
			}
			showCavityInfo = ImGui.CollapsingHeader("Cavity Info");
			if (showCavityInfo)
			{
				ImGui.Indent();
				CavityInfo cavityInfo = null;
				if (Game.Instance != null && Game.Instance.roomProber != null)
				{
					cavityInfo = Game.Instance.roomProber.GetCavityForCell(v);
				}
				if (cavityInfo != null)
				{
					ImGui.Text("Cell Count: " + cavityInfo.numCells);
					Room room = cavityInfo.room;
					if (room != null)
					{
						ImGui.Text("Is Room: True");
						showBuildings = ImGui.CollapsingHeader("Buildings (" + room.buildings.Count + ")");
						if (showBuildings)
						{
							foreach (KPrefabID building in room.buildings)
							{
								ImGui.Text(building.ToString());
							}
						}
						showCreatures = ImGui.CollapsingHeader("Creatures (" + room.cavity.creatures.Count + ")");
						if (showCreatures)
						{
							foreach (KPrefabID creature in room.cavity.creatures)
							{
								ImGui.Text(creature.ToString());
							}
						}
					}
					else
					{
						ImGui.Text("Is Room: False");
					}
				}
				else
				{
					ImGui.Text("No Cavity Detected");
				}
				ImGui.Unindent();
			}
			showPropertyInfo = ImGui.CollapsingHeader("Property Info");
			if (showPropertyInfo)
			{
				ImGui.Indent();
				bool flag3 = true;
				byte b = Grid.Properties[v];
				foreach (object value2 in Enum.GetValues(typeof(Sim.Cell.Properties)))
				{
					if ((b & (int)value2) != 0)
					{
						ImGui.Text(value2.ToString());
						flag3 = false;
					}
				}
				if (flag3)
				{
					ImGui.Text("No properties");
				}
				ImGui.Unindent();
			}
			ImGui.Unindent();
		}
		if (Grid.ObjectLayers != null)
		{
			Element element = Grid.Element[v];
			showElementData = ImGui.CollapsingHeader("Element");
			ImGui.SameLine();
			ImGui.Text("[" + element.name + "]");
			ImGui.Indent();
			ImGui.Text("Mass:" + Grid.Mass[v]);
			if (showElementData)
			{
				DrawElem(element);
			}
			ImGui.Text("Average Flow Rate (kg/s):" + Grid.AccumulatedFlow[v] / 3f);
			ImGui.Unindent();
		}
		showPhysicsData = ImGui.CollapsingHeader("Physics Data");
		if (showPhysicsData)
		{
			ImGui.Indent();
			ImGui.Text("Solid: " + Grid.Solid[v]);
			ImGui.Text("Pressure: " + Grid.Pressure[v]);
			ImGui.Text("Temperature (kelvin -272.15): " + Grid.Temperature[v]);
			ImGui.Text("Radiation: " + Grid.Radiation[v]);
			ImGui.Text("Mass: " + Grid.Mass[v]);
			ImGui.Text("Insulation: " + (float)(int)Grid.Insulation[v] / 255f);
			ImGui.Text("Strength Multiplier: " + Grid.StrengthInfo[v]);
			ImGui.Text("Properties: 0x: " + Grid.Properties[v].ToString("X"));
			ImGui.Text("Disease: " + ((Grid.DiseaseIdx[v] == byte.MaxValue) ? "None" : Db.Get().Diseases[Grid.DiseaseIdx[v]].Name));
			ImGui.Text("Disease Count: " + Grid.DiseaseCount[v]);
			ImGui.Unindent();
		}
		showGasConduitData = ImGui.CollapsingHeader("Gas Conduit Data");
		if (showGasConduitData)
		{
			DrawConduitFlow(Game.Instance.gasConduitFlow, v);
		}
		showLiquidConduitData = ImGui.CollapsingHeader("Liquid Conduit Data");
		if (showLiquidConduitData)
		{
			DrawConduitFlow(Game.Instance.liquidConduitFlow, v);
		}
	}

	private void DrawElem(Element element)
	{
		ImGui.Indent();
		ImGui.Text("State: " + element.state);
		ImGui.Text("Thermal Conductivity: " + element.thermalConductivity);
		ImGui.Text("Specific Heat Capacity: " + element.specificHeatCapacity);
		if (element.lowTempTransition != null)
		{
			ImGui.Text("Low Temperature: " + element.lowTemp);
			ImGui.Text("Low Temperature Transition: " + element.lowTempTransitionTarget);
		}
		if (element.highTempTransition != null)
		{
			ImGui.Text("High Temperature: " + element.highTemp);
			ImGui.Text("HighTemp Temperature Transition: " + element.highTempTransitionTarget);
			if (element.highTempTransitionOreID != 0)
			{
				ImGui.Text("HighTemp Temperature Transition: " + element.highTempTransitionOreID);
			}
		}
		ImGui.Text("Light Absorption Factor: " + element.lightAbsorptionFactor);
		ImGui.Text("Radiation Absorption Factor: " + element.radiationAbsorptionFactor);
		ImGui.Text("Radiation Per 1000 Mass: " + element.radiationPer1000Mass);
		ImGui.Text("Sublimate ID: " + element.sublimateId);
		ImGui.Text("Sublimate FX: " + element.sublimateFX);
		ImGui.Text("Sublimate Rate: " + element.sublimateRate);
		ImGui.Text("Sublimate Efficiency: " + element.sublimateEfficiency);
		ImGui.Text("Sublimate Probability: " + element.sublimateProbability);
		ImGui.Text("Off Gas Percentage: " + element.offGasPercentage);
		if (element.IsGas)
		{
			ImGui.Text("Default Pressure: " + element.defaultValues.pressure);
		}
		else
		{
			ImGui.Text("Default Mass: " + element.defaultValues.mass);
		}
		ImGui.Text("Default Temperature: " + element.defaultValues.temperature);
		if (element.IsGas)
		{
			ImGui.Text("Flow: " + element.flow);
		}
		if (element.IsLiquid)
		{
			ImGui.Text("Max Comp: " + element.maxCompression);
			ImGui.Text("Max Mass: " + element.maxMass);
		}
		if (element.IsSolid)
		{
			ImGui.Text("Hardness: " + element.hardness);
			ImGui.Text("Unstable: " + element.IsUnstable);
		}
		ImGui.Unindent();
	}

	private void DrawConduitFlow(ConduitFlow flow_mgr, int cell)
	{
		ImGui.Indent();
		ConduitFlow.ConduitContents contents = flow_mgr.GetContents(cell);
		ImGui.Text("Element: " + contents.element);
		ImGui.Text($"Mass: {contents.mass}");
		ImGui.Text($"Movable Mass: {contents.movable_mass}");
		ImGui.Text("Temperature: " + contents.temperature);
		ImGui.Text("Disease: " + ((contents.diseaseIdx == byte.MaxValue) ? "None" : Db.Get().Diseases[contents.diseaseIdx].Name));
		ImGui.Text("Disease Count: " + contents.diseaseCount);
		ImGui.Text($"Update Order: {flow_mgr.ComputeUpdateOrder(cell)}");
		flow_mgr.SetContents(cell, contents);
		ConduitFlow.FlowDirections permittedFlow = flow_mgr.GetPermittedFlow(cell);
		if (permittedFlow == ConduitFlow.FlowDirections.None)
		{
			ImGui.Text("PermittedFlow: None");
		}
		else
		{
			string text = "";
			if ((permittedFlow & ConduitFlow.FlowDirections.Up) != 0)
			{
				text += " Up ";
			}
			if ((permittedFlow & ConduitFlow.FlowDirections.Down) != 0)
			{
				text += " Down ";
			}
			if ((permittedFlow & ConduitFlow.FlowDirections.Left) != 0)
			{
				text += " Left ";
			}
			if ((permittedFlow & ConduitFlow.FlowDirections.Right) != 0)
			{
				text += " Right ";
			}
			ImGui.Text("PermittedFlow: " + text);
		}
		ImGui.Unindent();
	}

	private void DrawBoundingBoxOverlay()
	{
		ImGui.InputInt("Width:", ref xBound, 2);
		ImGui.InputInt("Height:", ref yBound, 2);
		Vector2I vector2I = (boundBoxSimCellTarget.HasValue ? Grid.CellToXY(boundBoxSimCellTarget.Unwrap().cellIndex) : Grid.PosToXY(worldPos));
		Vector2I vector2I2 = new Vector2I(Math.Max(0, vector2I.x - xBound / 2), Math.Max(0, vector2I.y - yBound / 2));
		Vector2I vector2I3 = new Vector2I(Math.Min(vector2I.x + xBound / 2, Grid.WidthInCells), Math.Min(vector2I.y + yBound / 2, Grid.HeightInCells));
		Option<(Vector2, Vector2)> screenRect = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(vector2I2.X, vector2I2.Y)).GetScreenRect();
		Option<(Vector2, Vector2)> screenRect2 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(vector2I3.X, vector2I3.Y)).GetScreenRect();
		if (!screenRect.IsSome() || !screenRect2.IsSome())
		{
			return;
		}
		for (int i = vector2I2.Y; i <= vector2I3.Y; i++)
		{
			for (int j = vector2I2.X; j <= vector2I3.X; j++)
			{
				Option<(Vector2, Vector2)> screenRect3 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(j, i)).GetScreenRect();
				Option<(Vector2, Vector2)> screenRect4 = new DevToolEntityTarget.ForSimCell(Grid.XYToCell(j, i)).GetScreenRect();
				(Vector2, Vector2) screenRect5 = (screenRect3.Unwrap().Item1, screenRect4.Unwrap().Item2);
				string text = Grid.XYToCell(j, i).ToString();
				DevToolEntity.DrawScreenRect(screenRect5, text, new Color(1f, 1f, 1f, 0.7f), new Color(1f, 1f, 1f, 0.2f), new Option<DevToolUtil.TextAlignment>(DevToolUtil.TextAlignment.Center));
			}
		}
	}

	public void SetCell(int cell)
	{
		worldPos = Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
	}
}
