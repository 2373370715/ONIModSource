using System;
using System.Collections.Generic;
using ImGuiNET;
using STRINGS;
using UnityEngine;

public class DevToolGeyserModifiers : DevTool
{
	private const string DEV_MODIFIER_ID = "DEV MODIFIER";

	private const string NO_SELECTED_STR = "No Geyser Selected";

	private int DevModifierID;

	private const float ITERATION_BAR_HEIGHT = 10f;

	private const float YEAR_BAR_HEIGHT = 10f;

	private const float BAR_SPACING = 2f;

	private const float CURRENT_TIME_PADDING = 2f;

	private const float CURRENT_TIME_LINE_WIDTH = 2f;

	private uint YEAR_ACTIVE_COLOR = Color(220, 15, 65, 175);

	private uint YEAR_DORMANT_COLOR = Color(byte.MaxValue, 0, 65, 60);

	private uint ITERATION_ERUPTION_COLOR = Color(60, 80, byte.MaxValue, 200);

	private uint ITERATION_QUIET_COLOR = Color(60, 80, byte.MaxValue, 80);

	private uint CURRENT_TIME_COLOR = Color(byte.MaxValue, 0, 0, byte.MaxValue);

	private Vector4 MODFIED_VALUE_TEXT_COLOR = new Vector4(0.8f, 0.7f, 0.1f, 1f);

	private Vector4 COMMENT_COLOR = new Vector4(0.1f, 0.5f, 0.1f, 1f);

	private Vector4 SUBTITLE_SLEEP_COLOR = new Vector4(0.15f, 0.35f, 0.7f, 1f);

	private Vector4 SUBTITLE_OVERPRESSURE_COLOR = new Vector4(0.7f, 0f, 0f, 1f);

	private Vector4 SUBTITLE_ERUPTING_COLOR = new Vector4(1f, 0.7f, 0f, 1f);

	private Vector4 ALT_COLOR = new Vector4(0.5f, 0.5f, 0.5f, 1f);

	private List<bool> modificationListUnfold = new List<bool>();

	private GameObject lastSelectedGameObject;

	private Geyser selectedGeyser;

	private Geyser.GeyserModification dev_modification;

	private string[] modifiers_FormatedList_Titles = new string[9] { "Mass per cycle", "Temperature", "Max Pressure", "Iteration duration", "Iteration percentage", "Year duration", "Year percentage", "Using secondary element", "Secondary element" };

	private string[] modifiers_FormatedList = new string[9];

	private string[] modifiers_FormatedList_Tooltip = new string[9];

	private string[] AllSimHashesValues;

	private int modifierSelected = -1;

	private int modifierFormatting_ValuePadding = -1;

	private float GraphHeight => 26f;

	private void DrawGeyserVariable(string variableTitle, float currentValue, float modifier, string modifierFormating = "+0.##; -0.##; +0", string unit = "", string modifierUnit = "", float altValue = 0f, string altUnit = "")
	{
		ImGui.BulletText(variableTitle + ": " + currentValue + unit);
		if (modifier != 0f)
		{
			ImGui.SameLine();
			ImGui.TextColored(MODFIED_VALUE_TEXT_COLOR, "(" + modifier.ToString(modifierFormating) + modifierUnit + ")");
		}
		if (!altUnit.IsNullOrWhiteSpace())
		{
			ImGui.SameLine();
			ImGui.TextColored(ALT_COLOR, "(" + altValue + altUnit + ")");
		}
	}

	public static uint Color(byte r, byte g, byte b, byte a)
	{
		return (uint)((a << 24) | (b << 16) | (g << 8) | r);
	}

	private void DrawYearAndIterationsGraph(Geyser geyser)
	{
		Vector2 windowContentRegionMin = ImGui.GetWindowContentRegionMin();
		Vector2 windowContentRegionMax = ImGui.GetWindowContentRegionMax();
		float num = windowContentRegionMax.x - windowContentRegionMin.x;
		ImGui.Dummy(new Vector2(num, GraphHeight));
		if (ImGui.IsItemVisible())
		{
			Vector2 itemRectMin = ImGui.GetItemRectMin();
			Vector2 itemRectMax = ImGui.GetItemRectMax();
			windowContentRegionMin.x += ImGui.GetWindowPos().x;
			windowContentRegionMin.y += ImGui.GetWindowPos().y;
			windowContentRegionMax.x += ImGui.GetWindowPos().x;
			windowContentRegionMax.y += ImGui.GetWindowPos().y;
			Vector2 vector = windowContentRegionMin;
			Vector2 vector2 = windowContentRegionMax;
			vector.y = itemRectMin.y;
			vector2.y = itemRectMax.y;
			float iterationLength = selectedGeyser.configuration.GetIterationLength();
			float iterationPercent = selectedGeyser.configuration.GetIterationPercent();
			float yearLength = selectedGeyser.configuration.GetYearLength();
			float yearPercent = selectedGeyser.configuration.GetYearPercent();
			Vector2 vector3 = vector;
			Vector2 vector4 = vector2;
			vector4.x = vector.x + num * yearPercent;
			vector4.y = vector3.y + 10f;
			ImGui.GetForegroundDrawList().AddRectFilled(vector3, vector4, YEAR_ACTIVE_COLOR);
			vector3.x = vector4.x;
			vector4.x = vector2.x;
			ImGui.GetForegroundDrawList().AddRectFilled(vector3, vector4, YEAR_DORMANT_COLOR);
			float f = yearLength / iterationLength;
			float num2 = iterationLength / yearLength;
			vector3.y = vector4.y + 2f;
			vector4.y = vector3.y + 10f;
			float num3 = (float)Mathf.FloorToInt(geyser.GetCurrentLifeTime() / yearLength) * yearLength % iterationLength / iterationLength;
			int num4 = Mathf.CeilToInt(f) + 1;
			for (int i = 0; i < num4; i++)
			{
				float x = vector.x - num2 * num3 * num + num2 * (float)i * num;
				vector3.x = x;
				vector4.x = vector3.x + iterationPercent * num2 * num;
				Vector2 p_min = vector3;
				Vector2 p_max = vector4;
				p_min.x = Mathf.Clamp(p_min.x, vector.x, vector2.x);
				p_max.x = Mathf.Clamp(p_max.x, vector.x, vector2.x);
				ImGui.GetForegroundDrawList().AddRectFilled(p_min, p_max, ITERATION_ERUPTION_COLOR);
				vector3.x = vector4.x;
				vector4.x += (1f - iterationPercent) * num2 * num;
				p_min = vector3;
				p_max = vector4;
				p_min.x = Mathf.Clamp(p_min.x, vector.x, vector2.x);
				p_max.x = Mathf.Clamp(p_max.x, vector.x, vector2.x);
				ImGui.GetForegroundDrawList().AddRectFilled(p_min, p_max, ITERATION_QUIET_COLOR);
			}
			float num5 = selectedGeyser.RemainingActiveTime();
			float num6 = selectedGeyser.RemainingDormantTime();
			float num7 = ((num6 > 0f) ? (yearLength - num6) : (yearLength * yearPercent - num5)) / yearLength;
			vector3.x = vector.x + num7 * num - 1f;
			vector4.x = vector.x + num7 * num + 1f;
			vector3.y = vector.y - 2f;
			vector4.y += 2f;
			ImGui.GetForegroundDrawList().AddRectFilled(vector3, vector4, CURRENT_TIME_COLOR);
		}
	}

	protected override void RenderTo(DevPanel panel)
	{
		Update();
		string fmt = ((selectedGeyser == null) ? "No Geyser Selected" : (UI.StripLinkFormatting(selectedGeyser.gameObject.GetProperName()) + " -"));
		uint num = 0u;
		ImGui.AlignTextToFramePadding();
		ImGui.Text(fmt);
		if (!(selectedGeyser != null))
		{
			return;
		}
		StateMachine.BaseState currentState = selectedGeyser.smi.GetCurrentState();
		string fmt2 = "zZ";
		string tooltip = "Current State: " + currentState.name;
		Vector4 col = SUBTITLE_SLEEP_COLOR;
		if (currentState == selectedGeyser.smi.sm.erupt.erupting)
		{
			fmt2 = "Erupting";
			col = SUBTITLE_ERUPTING_COLOR;
		}
		else if (currentState == selectedGeyser.smi.sm.erupt.overpressure)
		{
			fmt2 = "Overpressure";
			col = SUBTITLE_OVERPRESSURE_COLOR;
		}
		ImGui.SameLine();
		ImGui.TextColored(col, fmt2);
		if (ImGui.IsItemHovered())
		{
			ImGui.SetTooltip(tooltip);
		}
		ImGui.Separator();
		ImGui.Spacing();
		Geyser.GeyserModification modifier = selectedGeyser.configuration.GetModifier();
		PrepareSummaryForModification(selectedGeyser.configuration.GetModifier());
		float currentLifeTime = selectedGeyser.GetCurrentLifeTime();
		float yearLength = selectedGeyser.configuration.GetYearLength();
		ImGui.Text("Time Settings: \t");
		ImGui.SameLine();
		bool flag = ImGui.Button("Active");
		ImGui.SameLine();
		bool flag2 = ImGui.Button("Dormant");
		ImGui.SameLine();
		bool flag3 = ImGui.Button("<");
		ImGui.SameLine();
		bool flag4 = ImGui.Button(">");
		ImGui.SameLine();
		ImGui.Text("\tLifetime: " + currentLifeTime.ToString("00.0") + " sec (" + (currentLifeTime / yearLength).ToString("0.00") + " Years)\t");
		bool flag5 = false;
		if (selectedGeyser.timeShift != 0f)
		{
			ImGui.SameLine();
			flag5 = ImGui.Button("Restore");
			if (ImGui.IsItemHovered())
			{
				ImGui.SetTooltip("Restore lifetime to match with current game time");
			}
		}
		ImGui.SliderFloat("rateRoll", ref selectedGeyser.configuration.rateRoll, 0f, 1f);
		ImGui.SliderFloat("iterationLengthRoll", ref selectedGeyser.configuration.iterationLengthRoll, 0f, 1f);
		ImGui.SliderFloat("iterationPercentRoll", ref selectedGeyser.configuration.iterationPercentRoll, 0f, 1f);
		ImGui.SliderFloat("yearLengthRoll", ref selectedGeyser.configuration.yearLengthRoll, 0f, 1f);
		ImGui.SliderFloat("yearPercentRoll", ref selectedGeyser.configuration.yearPercentRoll, 0f, 1f);
		selectedGeyser.configuration.Init(reinit: true);
		if (flag)
		{
			selectedGeyser.ShiftTimeTo(Geyser.TimeShiftStep.ActiveState);
		}
		if (flag2)
		{
			selectedGeyser.ShiftTimeTo(Geyser.TimeShiftStep.DormantState);
		}
		if (flag3)
		{
			selectedGeyser.ShiftTimeTo(Geyser.TimeShiftStep.PreviousIteration);
		}
		if (flag4)
		{
			selectedGeyser.ShiftTimeTo(Geyser.TimeShiftStep.NextIteration);
		}
		if (flag5)
		{
			selectedGeyser.AlterTime(0f);
		}
		DrawYearAndIterationsGraph(selectedGeyser);
		ImGui.Indent();
		bool flag6 = true;
		float num2 = ((!flag6) ? 1 : 100);
		string modifierUnit = (flag6 ? "%%" : "");
		float convertedTemperature = GameUtil.GetConvertedTemperature(selectedGeyser.configuration.GetTemperature());
		string temperatureUnitSuffix = GameUtil.GetTemperatureUnitSuffix();
		Element element = ElementLoader.FindElementByHash(selectedGeyser.configuration.GetElement());
		Element element2 = ElementLoader.FindElementByHash(selectedGeyser.configuration.geyserType.element);
		string text = ((element2.lowTempTransitionTarget == (SimHashes)0) ? "" : (GameUtil.GetConvertedTemperature(element2.lowTemp) + " -> " + element2.lowTempTransitionTarget));
		string text2 = ((element2.highTempTransitionTarget == (SimHashes)0) ? "" : (GameUtil.GetConvertedTemperature(element2.highTemp) + " -> " + element2.highTempTransitionTarget));
		ImGui.BulletText("Element:");
		ImGui.SameLine();
		if (element2 != element)
		{
			string text3 = ((element.lowTempTransitionTarget == (SimHashes)0) ? "" : (GameUtil.GetConvertedTemperature(element.lowTemp) + " " + element.lowTempTransitionTarget));
			string text4 = ((element.highTempTransitionTarget == (SimHashes)0) ? "" : (GameUtil.GetConvertedTemperature(element.highTemp) + " " + element.highTempTransitionTarget));
			ImGui.TextColored(MODFIED_VALUE_TEXT_COLOR, element.ToString());
			ImGui.SameLine();
			ImGui.TextColored(MODFIED_VALUE_TEXT_COLOR, "(Original element: " + element2.id.ToString() + ")");
			ImGui.SameLine();
			ImGui.Text(" [original low: " + text + ", " + text2 + ", current low: " + text3 + ", " + text4 + "]");
		}
		else
		{
			ImGui.Text($"{element2.id} [low: {text}, high: {text2}]");
		}
		float altValue = Mathf.Max(0f, GameUtil.GetConvertedTemperature(element2.highTemp) - convertedTemperature);
		DrawGeyserVariable("Emit Rate", selectedGeyser.configuration.GetEmitRate(), 0f, "+0.##; -0.##; +0", " Kg/s");
		DrawGeyserVariable("Average Output", selectedGeyser.configuration.GetAverageEmission(), 0f, "+0.##; -0.##; +0", " Kg/s");
		DrawGeyserVariable("Mass per cycle", selectedGeyser.configuration.GetMassPerCycle(), modifier.massPerCycleModifier * num2, "+0.##; -0.##; +0", "", modifierUnit);
		DrawGeyserVariable("Temperature", convertedTemperature, modifier.temperatureModifier, "+0.##; -0.##; +0", temperatureUnitSuffix, temperatureUnitSuffix, altValue, temperatureUnitSuffix + " before state change");
		DrawGeyserVariable("Max Pressure", selectedGeyser.configuration.GetMaxPressure(), modifier.maxPressureModifier * num2, "+0.##; -0.##; +0", " Kg", modifierUnit);
		DrawGeyserVariable("Iteration duration", selectedGeyser.configuration.GetIterationLength(), modifier.iterationDurationModifier * num2, "+0.##; -0.##; +0", " sec", modifierUnit);
		DrawGeyserVariable("Iteration percentage", selectedGeyser.configuration.GetIterationPercent(), modifier.iterationPercentageModifier * num2, "+0.##; -0.##; +0", "", modifierUnit, selectedGeyser.configuration.GetIterationLength() * selectedGeyser.configuration.GetIterationPercent(), " sec");
		DrawGeyserVariable("Year duration", selectedGeyser.configuration.GetYearLength(), modifier.yearDurationModifier * num2, "+0.##; -0.##; +0", " sec", modifierUnit, selectedGeyser.configuration.GetYearLength() / 600f, " cycles");
		DrawGeyserVariable("Year percentage", selectedGeyser.configuration.GetYearPercent(), modifier.yearPercentageModifier * num2, "+0.##; -0.##; +0", "", modifierUnit, selectedGeyser.configuration.GetYearPercent() * selectedGeyser.configuration.GetYearLength() / 600f, " cycles");
		ImGui.Unindent();
		ImGui.Spacing();
		ImGui.Separator();
		ImGui.Spacing();
		ImGui.Text("Create Modification");
		ImGui.SameLine();
		bool num3 = ImGui.Button("Clear");
		if (flag6)
		{
			ImGui.TextColored(COMMENT_COLOR, "Units specified in the inputs bellow are percentages E.g. 0.1 represents 10%%\nTemperature is measured in kelvins and percentages affect the kelvin value");
			ImGui.Spacing();
		}
		if (num3)
		{
			dev_modification.Clear();
		}
		ImGui.Indent();
		ImGui.BeginGroup();
		dev_modification.newElement.ToString();
		float num4 = 0.05f;
		float num5 = 0.15f;
		string text5 = "%.2f";
		ImGui.InputFloat(modifiers_FormatedList_Titles[0], ref dev_modification.massPerCycleModifier, flag6 ? num4 : 1f, flag6 ? num5 : 5f, flag6 ? text5 : "%.0f");
		ImGui.InputFloat(modifiers_FormatedList_Titles[1], ref dev_modification.temperatureModifier, flag6 ? num4 : 1f, flag6 ? num5 : 5f, flag6 ? text5 : "%.0f");
		ImGui.InputFloat(modifiers_FormatedList_Titles[2], ref dev_modification.maxPressureModifier, flag6 ? num4 : 0.1f, flag6 ? num5 : 0.5f, flag6 ? text5 : "%.1f");
		ImGui.InputFloat(modifiers_FormatedList_Titles[3], ref dev_modification.iterationDurationModifier, flag6 ? num4 : 1f, flag6 ? num5 : 5f, flag6 ? text5 : "%.0f");
		ImGui.InputFloat(modifiers_FormatedList_Titles[4], ref dev_modification.iterationPercentageModifier, flag6 ? num4 : 0.01f, flag6 ? num5 : 0.1f, flag6 ? text5 : "%.2f");
		ImGui.InputFloat(modifiers_FormatedList_Titles[5], ref dev_modification.yearDurationModifier, flag6 ? num4 : 1f, flag6 ? num5 : 5f, flag6 ? text5 : "%.0f");
		ImGui.InputFloat(modifiers_FormatedList_Titles[6], ref dev_modification.yearPercentageModifier, flag6 ? num4 : 0.01f, flag6 ? num5 : 0.1f, flag6 ? text5 : "%.2f");
		ImGui.Checkbox(modifiers_FormatedList_Titles[7], ref dev_modification.modifyElement);
		string text6 = "None";
		string text7 = ((dev_modification.modifyElement && dev_modification.newElement != 0) ? dev_modification.newElement.ToString() : text6);
		if (ImGui.BeginCombo(modifiers_FormatedList_Titles[8], text7))
		{
			bool flag7 = false;
			for (int i = 0; i < AllSimHashesValues.Length; i++)
			{
				flag7 = dev_modification.newElement.ToString() == text7;
				if (ImGui.Selectable(AllSimHashesValues[i], flag7))
				{
					text7 = AllSimHashesValues[i];
					dev_modification.newElement = (SimHashes)Enum.Parse(typeof(SimHashes), text7);
				}
				if (flag7)
				{
					ImGui.SetItemDefaultFocus();
				}
			}
			ImGui.EndCombo();
		}
		if (ImGui.Button("Add Modification"))
		{
			dev_modification.originID = "DEV MODIFIER#" + DevModifierID++;
			selectedGeyser.AddModification(dev_modification);
		}
		ImGui.SameLine();
		if (ImGui.Button("Remove Last") && selectedGeyser.modifications.Count > 0)
		{
			int num6 = -1;
			for (int num7 = selectedGeyser.modifications.Count - 1; num7 >= 0; num7--)
			{
				if (selectedGeyser.modifications[num7].originID.Contains("DEV MODIFIER"))
				{
					num6 = num7;
					break;
				}
			}
			if (num6 >= 0)
			{
				selectedGeyser.RemoveModification(selectedGeyser.modifications[num6]);
			}
		}
		ImGui.EndGroup();
		ImGui.Unindent();
		ImGui.Spacing();
		ImGui.Separator();
		ImGui.Spacing();
		while (modificationListUnfold.Count < selectedGeyser.modifications.Count)
		{
			modificationListUnfold.Add(item: false);
		}
		ImGui.Text("Modifications: " + selectedGeyser.modifications.Count);
		ImGui.Indent();
		for (int j = 0; j < selectedGeyser.modifications.Count; j++)
		{
			bool flag8 = selectedGeyser.modifications[j].originID.Contains("DEV MODIFIER");
			bool flag9 = false;
			bool flag10 = false;
			if (modificationListUnfold[j] = ImGui.CollapsingHeader(j + ". " + selectedGeyser.modifications[j].originID, ImGuiTreeNodeFlags.SpanAvailWidth))
			{
				PrepareSummaryForModification(selectedGeyser.modifications[j]);
				Vector2 itemRectSize = ImGui.GetItemRectSize();
				itemRectSize.y *= Mathf.Max(modifiers_FormatedList.Length + (flag8 ? 1 : 0) + 1, 1);
				if (ImGui.BeginChild(++num, itemRectSize, border: false, ImGuiWindowFlags.NoBackground))
				{
					ImGui.Indent();
					for (int k = 0; k < modifiers_FormatedList.Length; k++)
					{
						ImGui.Text(modifiers_FormatedList[k]);
						if (ImGui.IsItemHovered())
						{
							modifierSelected = k;
							ImGui.SetTooltip(modifiers_FormatedList_Tooltip[modifierSelected]);
						}
					}
					flag10 = ImGui.Button("Copy");
					if (flag8)
					{
						flag9 = ImGui.Button("Remove");
					}
					ImGui.Unindent();
				}
				ImGui.EndChild();
			}
			if (flag10)
			{
				dev_modification = selectedGeyser.modifications[j];
			}
			if (flag9)
			{
				selectedGeyser.RemoveModification(selectedGeyser.modifications[j]);
				break;
			}
		}
		ImGui.Unindent();
	}

	private void PrepareSummaryForModification(Geyser.GeyserModification modification)
	{
		float num = ((Geyser.massModificationMethod != Geyser.ModificationMethod.Percentages) ? 1 : 100);
		float num2 = ((Geyser.temperatureModificationMethod != Geyser.ModificationMethod.Percentages) ? 1 : 100);
		float num3 = ((Geyser.maxPressureModificationMethod != Geyser.ModificationMethod.Percentages) ? 1 : 100);
		float num4 = ((Geyser.IterationDurationModificationMethod != Geyser.ModificationMethod.Percentages) ? 1 : 100);
		float num5 = ((Geyser.IterationPercentageModificationMethod != Geyser.ModificationMethod.Percentages) ? 1 : 100);
		float num6 = ((Geyser.yearDurationModificationMethod != Geyser.ModificationMethod.Percentages) ? 1 : 100);
		float num7 = ((Geyser.yearPercentageModificationMethod != Geyser.ModificationMethod.Percentages) ? 1 : 100);
		string text = ((num == 100f) ? "%%" : "");
		string text2 = ((num2 == 100f) ? "%%" : "");
		string text3 = ((num3 == 100f) ? "%%" : "");
		string text4 = ((num4 == 100f) ? "%%" : "");
		string text5 = ((num5 == 100f) ? "%%" : "");
		string text6 = ((num6 == 100f) ? "%%" : "");
		string text7 = ((num7 == 100f) ? "%%" : "");
		modifiers_FormatedList[0] = modifiers_FormatedList_Titles[0] + ": " + (modification.massPerCycleModifier * num).ToString("+0.##; -0.##; +0") + text;
		modifiers_FormatedList[1] = modifiers_FormatedList_Titles[1] + ": " + (modification.temperatureModifier * num2).ToString("+0.##; -0.##; +0") + text2;
		modifiers_FormatedList[2] = modifiers_FormatedList_Titles[2] + ": " + (modification.maxPressureModifier * num3).ToString("+0.##; -0.##; +0") + ((num3 == 100f) ? text3 : "Kg");
		modifiers_FormatedList[3] = modifiers_FormatedList_Titles[3] + ": " + (modification.iterationDurationModifier * num4).ToString("+0.##; -0.##; +0") + ((num4 == 100f) ? text4 : "s");
		modifiers_FormatedList[4] = modifiers_FormatedList_Titles[4] + ": " + (modification.iterationPercentageModifier * num5).ToString("+0.##; -0.##; +0") + text5;
		modifiers_FormatedList[5] = modifiers_FormatedList_Titles[5] + ": " + (modification.yearDurationModifier * num6).ToString("+0.##; -0.##; +0") + ((num6 == 100f) ? text6 : "s");
		modifiers_FormatedList[6] = modifiers_FormatedList_Titles[6] + ": " + (modification.yearPercentageModifier * num7).ToString("+0.##; -0.##; +0") + text7;
		modifiers_FormatedList[7] = modifiers_FormatedList_Titles[7] + ": " + modification.modifyElement;
		modifiers_FormatedList[8] = modifiers_FormatedList_Titles[8] + ": " + (modification.IsNewElementInUse() ? modification.newElement.ToString() : "None");
	}

	private void Update()
	{
		Setup();
		GameObject gameObject = SelectTool.Instance?.selected?.gameObject;
		if (lastSelectedGameObject != gameObject && gameObject != null)
		{
			Geyser component = gameObject.GetComponent<Geyser>();
			selectedGeyser = ((component == null) ? selectedGeyser : component);
		}
		lastSelectedGameObject = gameObject;
	}

	private void Setup()
	{
		if (AllSimHashesValues == null)
		{
			AllSimHashesValues = Enum.GetNames(typeof(SimHashes));
		}
		if (modifierFormatting_ValuePadding < 0)
		{
			for (int i = 0; i < modifiers_FormatedList_Titles.Length; i++)
			{
				modifierFormatting_ValuePadding = Mathf.Max(modifierFormatting_ValuePadding, modifiers_FormatedList_Titles[i].Length);
			}
		}
		if (string.IsNullOrEmpty(modifiers_FormatedList_Tooltip[0]))
		{
			modifiers_FormatedList_Tooltip[0] = "Mass per cycle is not mass per iteration, mass per iteration gets calculated out of this";
			modifiers_FormatedList_Tooltip[1] = "Temperature modifier of the emitted element, does not refer to the temperature of the geyser itself";
			modifiers_FormatedList_Tooltip[2] = "Refering to the max pressure allowed in the environment surrounding the geyser before it stops emitting";
			modifiers_FormatedList_Tooltip[3] = "An iteration is a chunk of time that has 2 sections, one section is the erupting time while the other is the non erupting time";
			modifiers_FormatedList_Tooltip[4] = "Represents what percentage out of the iteration duration will be used for 'Erupting' period and the remaining will be the 'Quiet' period";
			modifiers_FormatedList_Tooltip[5] = "A year is a chunk of time that has 2 sections, one section is the Active section while the other is the Dormant section. While active, there could be many Iterations. While Dormant, there is no activity at all.";
			modifiers_FormatedList_Tooltip[6] = "Represents what percentage out of the year duration will be used for 'Active' period and the remaining will be the 'Dormant' period";
			modifiers_FormatedList_Tooltip[7] = "Whether to use or not to use the specified element";
			modifiers_FormatedList_Tooltip[8] = "Extra element to emit";
		}
	}
}
