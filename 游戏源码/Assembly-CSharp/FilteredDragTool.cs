using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200142F RID: 5167
public class FilteredDragTool : DragTool
{
	// Token: 0x06006AD3 RID: 27347 RVA: 0x000E6203 File Offset: 0x000E4403
	public bool IsActiveLayer(string layer)
	{
		return this.currentFilterTargets[ToolParameterMenu.FILTERLAYERS.ALL] == ToolParameterMenu.ToggleState.On || (this.currentFilterTargets.ContainsKey(layer.ToUpper()) && this.currentFilterTargets[layer.ToUpper()] == ToolParameterMenu.ToggleState.On);
	}

	// Token: 0x06006AD4 RID: 27348 RVA: 0x002E0650 File Offset: 0x002DE850
	public bool IsActiveLayer(ObjectLayer layer)
	{
		if (this.currentFilterTargets.ContainsKey(ToolParameterMenu.FILTERLAYERS.ALL) && this.currentFilterTargets[ToolParameterMenu.FILTERLAYERS.ALL] == ToolParameterMenu.ToggleState.On)
		{
			return true;
		}
		bool result = false;
		foreach (KeyValuePair<string, ToolParameterMenu.ToggleState> keyValuePair in this.currentFilterTargets)
		{
			if (keyValuePair.Value == ToolParameterMenu.ToggleState.On && this.GetObjectLayerFromFilterLayer(keyValuePair.Key) == layer)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x06006AD5 RID: 27349 RVA: 0x002E06E4 File Offset: 0x002DE8E4
	protected virtual void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Add(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On);
		filters.Add(ToolParameterMenu.FILTERLAYERS.WIRES, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.BUILDINGS, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LOGIC, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.BACKWALL, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x06006AD6 RID: 27350 RVA: 0x000E6242 File Offset: 0x000E4442
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.ResetFilter(this.filterTargets);
	}

	// Token: 0x06006AD7 RID: 27351 RVA: 0x000E6256 File Offset: 0x000E4456
	protected override void OnSpawn()
	{
		base.OnSpawn();
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
	}

	// Token: 0x06006AD8 RID: 27352 RVA: 0x000E6284 File Offset: 0x000E4484
	protected override void OnCleanUp()
	{
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		base.OnCleanUp();
	}

	// Token: 0x06006AD9 RID: 27353 RVA: 0x000E62B2 File Offset: 0x000E44B2
	public void ResetFilter()
	{
		this.ResetFilter(this.filterTargets);
	}

	// Token: 0x06006ADA RID: 27354 RVA: 0x000E62C0 File Offset: 0x000E44C0
	protected void ResetFilter(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Clear();
		this.GetDefaultFilters(filters);
		this.currentFilterTargets = filters;
	}

	// Token: 0x06006ADB RID: 27355 RVA: 0x000E62D6 File Offset: 0x000E44D6
	protected override void OnActivateTool()
	{
		this.active = true;
		base.OnActivateTool();
		this.OnOverlayChanged(OverlayScreen.Instance.mode);
	}

	// Token: 0x06006ADC RID: 27356 RVA: 0x000E62F5 File Offset: 0x000E44F5
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		this.active = false;
		ToolMenu.Instance.toolParameterMenu.ClearMenu();
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x06006ADD RID: 27357 RVA: 0x002E0754 File Offset: 0x002DE954
	public virtual string GetFilterLayerFromGameObject(GameObject input)
	{
		BuildingComplete component = input.GetComponent<BuildingComplete>();
		BuildingUnderConstruction component2 = input.GetComponent<BuildingUnderConstruction>();
		if (component)
		{
			return this.GetFilterLayerFromObjectLayer(component.Def.ObjectLayer);
		}
		if (component2)
		{
			return this.GetFilterLayerFromObjectLayer(component2.Def.ObjectLayer);
		}
		if (input.GetComponent<Clearable>() != null || input.GetComponent<Moppable>() != null)
		{
			return "CleanAndClear";
		}
		if (input.GetComponent<Diggable>() != null)
		{
			return "DigPlacer";
		}
		return "Default";
	}

	// Token: 0x06006ADE RID: 27358 RVA: 0x002E07E0 File Offset: 0x002DE9E0
	public string GetFilterLayerFromObjectLayer(ObjectLayer gamer_layer)
	{
		if (gamer_layer > ObjectLayer.FoundationTile)
		{
			switch (gamer_layer)
			{
			case ObjectLayer.GasConduit:
			case ObjectLayer.GasConduitConnection:
				return "GasPipes";
			case ObjectLayer.GasConduitTile:
			case ObjectLayer.ReplacementGasConduit:
			case ObjectLayer.LiquidConduitTile:
			case ObjectLayer.ReplacementLiquidConduit:
				goto IL_AC;
			case ObjectLayer.LiquidConduit:
			case ObjectLayer.LiquidConduitConnection:
				return "LiquidPipes";
			case ObjectLayer.SolidConduit:
				break;
			default:
				switch (gamer_layer)
				{
				case ObjectLayer.SolidConduitConnection:
					break;
				case ObjectLayer.LadderTile:
				case ObjectLayer.ReplacementLadder:
				case ObjectLayer.WireTile:
				case ObjectLayer.ReplacementWire:
					goto IL_AC;
				case ObjectLayer.Wire:
				case ObjectLayer.WireConnectors:
					return "Wires";
				case ObjectLayer.LogicGate:
				case ObjectLayer.LogicWire:
					return "Logic";
				default:
					if (gamer_layer == ObjectLayer.Gantry)
					{
						goto IL_7C;
					}
					goto IL_AC;
				}
				break;
			}
			return "SolidConduits";
		}
		if (gamer_layer != ObjectLayer.Building)
		{
			if (gamer_layer == ObjectLayer.Backwall)
			{
				return "BackWall";
			}
			if (gamer_layer != ObjectLayer.FoundationTile)
			{
				goto IL_AC;
			}
			return "Tiles";
		}
		IL_7C:
		return "Buildings";
		IL_AC:
		return "Default";
	}

	// Token: 0x06006ADF RID: 27359 RVA: 0x002E08A0 File Offset: 0x002DEAA0
	private ObjectLayer GetObjectLayerFromFilterLayer(string filter_layer)
	{
		string text = filter_layer.ToLower();
		uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
		if (num <= 2200975418U)
		{
			if (num <= 388608975U)
			{
				if (num != 25076977U)
				{
					if (num == 388608975U)
					{
						if (text == "solidconduits")
						{
							return ObjectLayer.SolidConduit;
						}
					}
				}
				else if (text == "wires")
				{
					return ObjectLayer.Wire;
				}
			}
			else if (num != 614364310U)
			{
				if (num == 2200975418U)
				{
					if (text == "backwall")
					{
						return ObjectLayer.Backwall;
					}
				}
			}
			else if (text == "liquidpipes")
			{
				return ObjectLayer.LiquidConduit;
			}
		}
		else if (num <= 2875565775U)
		{
			if (num != 2366751346U)
			{
				if (num == 2875565775U)
				{
					if (text == "gaspipes")
					{
						return ObjectLayer.GasConduit;
					}
				}
			}
			else if (text == "buildings")
			{
				return ObjectLayer.Building;
			}
		}
		else if (num != 3464443665U)
		{
			if (num == 4178729166U)
			{
				if (text == "tiles")
				{
					return ObjectLayer.FoundationTile;
				}
			}
		}
		else if (text == "logic")
		{
			return ObjectLayer.LogicWire;
		}
		throw new ArgumentException("Invalid filter layer: " + filter_layer);
	}

	// Token: 0x06006AE0 RID: 27360 RVA: 0x002E09E8 File Offset: 0x002DEBE8
	private void OnOverlayChanged(HashedString overlay)
	{
		if (!this.active)
		{
			return;
		}
		string text = null;
		if (overlay == OverlayModes.Power.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.WIRES;
		}
		else if (overlay == OverlayModes.LiquidConduits.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT;
		}
		else if (overlay == OverlayModes.GasConduits.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.GASCONDUIT;
		}
		else if (overlay == OverlayModes.SolidConveyor.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT;
		}
		else if (overlay == OverlayModes.Logic.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.LOGIC;
		}
		this.currentFilterTargets = this.filterTargets;
		if (text != null)
		{
			using (List<string>.Enumerator enumerator = new List<string>(this.filterTargets.Keys).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text2 = enumerator.Current;
					this.filterTargets[text2] = ToolParameterMenu.ToggleState.Disabled;
					if (text2 == text)
					{
						this.filterTargets[text2] = ToolParameterMenu.ToggleState.On;
					}
				}
				goto IL_102;
			}
		}
		if (this.overlayFilterTargets.Count == 0)
		{
			this.ResetFilter(this.overlayFilterTargets);
		}
		this.currentFilterTargets = this.overlayFilterTargets;
		IL_102:
		ToolMenu.Instance.toolParameterMenu.PopulateMenu(this.currentFilterTargets);
	}

	// Token: 0x0400507E RID: 20606
	private Dictionary<string, ToolParameterMenu.ToggleState> filterTargets = new Dictionary<string, ToolParameterMenu.ToggleState>();

	// Token: 0x0400507F RID: 20607
	private Dictionary<string, ToolParameterMenu.ToggleState> overlayFilterTargets = new Dictionary<string, ToolParameterMenu.ToggleState>();

	// Token: 0x04005080 RID: 20608
	private Dictionary<string, ToolParameterMenu.ToggleState> currentFilterTargets;

	// Token: 0x04005081 RID: 20609
	private bool active;
}
