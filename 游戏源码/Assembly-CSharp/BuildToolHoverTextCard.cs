using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001AA9 RID: 6825
public class BuildToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x06008EBC RID: 36540 RVA: 0x003721D8 File Offset: 0x003703D8
	public override void UpdateHoverElements(List<KSelectable> hoverObjects)
	{
		HoverTextScreen instance = HoverTextScreen.Instance;
		HoverTextDrawer hoverTextDrawer = instance.BeginDrawing();
		int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		if (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
		{
			hoverTextDrawer.EndDrawing();
			return;
		}
		hoverTextDrawer.BeginShadowBar(false);
		this.ActionName = ((this.currentDef != null && this.currentDef.DragBuild) ? UI.TOOLS.BUILD.TOOLACTION_DRAG : UI.TOOLS.BUILD.TOOLACTION);
		if (this.currentDef != null && this.currentDef.Name != null)
		{
			this.ToolName = string.Format(UI.TOOLS.BUILD.NAME, this.currentDef.Name);
		}
		base.DrawTitle(instance, hoverTextDrawer);
		base.DrawInstructions(instance, hoverTextDrawer);
		int min_height = 26;
		int width = 8;
		if (this.currentDef != null)
		{
			if (PlayerController.Instance.ActiveTool != null)
			{
				Type type = PlayerController.Instance.ActiveTool.GetType();
				if (typeof(BuildTool).IsAssignableFrom(type) || typeof(BaseUtilityBuildTool).IsAssignableFrom(type))
				{
					if (this.currentDef.BuildingComplete.GetComponent<Rotatable>() != null)
					{
						hoverTextDrawer.NewLine(min_height);
						hoverTextDrawer.AddIndent(width);
						string text = UI.TOOLTIPS.HELP_ROTATE_KEY.ToString();
						text = text.Replace("{Key}", GameUtil.GetActionString(global::Action.RotateBuilding));
						hoverTextDrawer.DrawText(text, this.Styles_Instruction.Standard);
					}
					Orientation getBuildingOrientation = BuildTool.Instance.GetBuildingOrientation;
					string text2 = "Unknown reason";
					Vector3 pos = Grid.CellToPosCCC(num, Grid.SceneLayer.Building);
					if (!this.currentDef.IsValidPlaceLocation(BuildTool.Instance.visualizer, pos, getBuildingOrientation, out text2))
					{
						hoverTextDrawer.NewLine(min_height);
						hoverTextDrawer.AddIndent(width);
						hoverTextDrawer.DrawText(text2, this.HoverTextStyleSettings[1]);
					}
					RoomTracker component = this.currentDef.BuildingComplete.GetComponent<RoomTracker>();
					if (component != null && !component.SufficientBuildLocation(num))
					{
						hoverTextDrawer.NewLine(min_height);
						hoverTextDrawer.AddIndent(width);
						hoverTextDrawer.DrawText(UI.TOOLTIPS.HELP_REQUIRES_ROOM, this.HoverTextStyleSettings[1]);
					}
				}
			}
			hoverTextDrawer.NewLine(min_height);
			hoverTextDrawer.AddIndent(width);
			hoverTextDrawer.DrawText(ResourceRemainingDisplayScreen.instance.GetString(), this.Styles_BodyText.Standard);
			hoverTextDrawer.EndShadowBar();
			HashedString mode = SimDebugView.Instance.GetMode();
			if (mode == OverlayModes.Logic.ID && hoverObjects != null)
			{
				SelectToolHoverTextCard component2 = SelectTool.Instance.GetComponent<SelectToolHoverTextCard>();
				using (List<KSelectable>.Enumerator enumerator = hoverObjects.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KSelectable kselectable = enumerator.Current;
						LogicPorts component3 = kselectable.GetComponent<LogicPorts>();
						LogicPorts.Port port;
						bool flag;
						if (component3 != null && component3.TryGetPortAtCell(num, out port, out flag))
						{
							bool flag2 = component3.IsPortConnected(port.id);
							hoverTextDrawer.BeginShadowBar(false);
							int num2;
							if (flag)
							{
								string replacement = port.displayCustomName ? port.description : UI.LOGIC_PORTS.PORT_INPUT_DEFAULT_NAME.text;
								num2 = component3.GetInputValue(port.id);
								hoverTextDrawer.DrawText(UI.TOOLS.GENERIC.LOGIC_INPUT_HOVER_FMT.Replace("{Port}", replacement).Replace("{Name}", kselectable.GetProperName().ToUpper()), component2.Styles_Title.Standard);
							}
							else
							{
								string replacement2 = port.displayCustomName ? port.description : UI.LOGIC_PORTS.PORT_OUTPUT_DEFAULT_NAME.text;
								num2 = component3.GetOutputValue(port.id);
								hoverTextDrawer.DrawText(UI.TOOLS.GENERIC.LOGIC_OUTPUT_HOVER_FMT.Replace("{Port}", replacement2).Replace("{Name}", kselectable.GetProperName().ToUpper()), component2.Styles_Title.Standard);
							}
							hoverTextDrawer.NewLine(26);
							TextStyleSetting style;
							if (flag2)
							{
								style = ((num2 == 1) ? component2.Styles_LogicActive.Selected : component2.Styles_LogicSignalInactive);
							}
							else
							{
								style = component2.Styles_LogicActive.Standard;
							}
							component2.DrawLogicIcon(hoverTextDrawer, (num2 == 1 && flag2) ? component2.iconActiveAutomationPort : component2.iconDash, style);
							component2.DrawLogicText(hoverTextDrawer, port.activeDescription, style);
							hoverTextDrawer.NewLine(26);
							TextStyleSetting style2;
							if (flag2)
							{
								style2 = ((num2 == 0) ? component2.Styles_LogicStandby.Selected : component2.Styles_LogicSignalInactive);
							}
							else
							{
								style2 = component2.Styles_LogicStandby.Standard;
							}
							component2.DrawLogicIcon(hoverTextDrawer, (num2 == 0 && flag2) ? component2.iconActiveAutomationPort : component2.iconDash, style2);
							component2.DrawLogicText(hoverTextDrawer, port.inactiveDescription, style2);
							hoverTextDrawer.EndShadowBar();
						}
						LogicGate component4 = kselectable.GetComponent<LogicGate>();
						LogicGateBase.PortId portId;
						if (component4 != null && component4.TryGetPortAtCell(num, out portId))
						{
							int portValue = component4.GetPortValue(portId);
							bool portConnected = component4.GetPortConnected(portId);
							LogicGate.LogicGateDescriptions.Description portDescription = component4.GetPortDescription(portId);
							hoverTextDrawer.BeginShadowBar(false);
							if (portId == LogicGateBase.PortId.OutputOne)
							{
								hoverTextDrawer.DrawText(UI.TOOLS.GENERIC.LOGIC_MULTI_OUTPUT_HOVER_FMT.Replace("{Port}", portDescription.name).Replace("{Name}", kselectable.GetProperName().ToUpper()), component2.Styles_Title.Standard);
							}
							else
							{
								hoverTextDrawer.DrawText(UI.TOOLS.GENERIC.LOGIC_MULTI_INPUT_HOVER_FMT.Replace("{Port}", portDescription.name).Replace("{Name}", kselectable.GetProperName().ToUpper()), component2.Styles_Title.Standard);
							}
							hoverTextDrawer.NewLine(26);
							TextStyleSetting style3;
							if (portConnected)
							{
								style3 = ((portValue == 1) ? component2.Styles_LogicActive.Selected : component2.Styles_LogicSignalInactive);
							}
							else
							{
								style3 = component2.Styles_LogicActive.Standard;
							}
							component2.DrawLogicIcon(hoverTextDrawer, (portValue == 1 && portConnected) ? component2.iconActiveAutomationPort : component2.iconDash, style3);
							component2.DrawLogicText(hoverTextDrawer, portDescription.active, style3);
							hoverTextDrawer.NewLine(26);
							TextStyleSetting style4;
							if (portConnected)
							{
								style4 = ((portValue == 0) ? component2.Styles_LogicStandby.Selected : component2.Styles_LogicSignalInactive);
							}
							else
							{
								style4 = component2.Styles_LogicStandby.Standard;
							}
							component2.DrawLogicIcon(hoverTextDrawer, (portValue == 0 && portConnected) ? component2.iconActiveAutomationPort : component2.iconDash, style4);
							component2.DrawLogicText(hoverTextDrawer, portDescription.inactive, style4);
							hoverTextDrawer.EndShadowBar();
						}
					}
					goto IL_71D;
				}
			}
			if (mode == OverlayModes.Power.ID)
			{
				CircuitManager circuitManager = Game.Instance.circuitManager;
				ushort circuitID = circuitManager.GetCircuitID(num);
				if (circuitID != 65535)
				{
					hoverTextDrawer.BeginShadowBar(false);
					float num3 = circuitManager.GetWattsNeededWhenActive(circuitID);
					num3 += this.currentDef.EnergyConsumptionWhenActive;
					float maxSafeWattageForCircuit = circuitManager.GetMaxSafeWattageForCircuit(circuitID);
					Color color = (num3 >= maxSafeWattageForCircuit + POWER.FLOAT_FUDGE_FACTOR) ? Color.red : Color.white;
					hoverTextDrawer.AddIndent(width);
					hoverTextDrawer.DrawText(string.Format(UI.DETAILTABS.ENERGYGENERATOR.POTENTIAL_WATTAGE_CONSUMED, GameUtil.GetFormattedWattage(num3, GameUtil.WattageFormatterUnit.Automatic, true)), this.Styles_BodyText.Standard, color, true);
					hoverTextDrawer.EndShadowBar();
				}
			}
		}
		IL_71D:
		hoverTextDrawer.EndDrawing();
	}

	// Token: 0x04006B98 RID: 27544
	public BuildingDef currentDef;
}
