using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001DD1 RID: 7633
public class VitalsTableScreen : TableScreen
{
	// Token: 0x06009F65 RID: 40805 RVA: 0x003D0A8C File Offset: 0x003CEC8C
	protected override void OnActivate()
	{
		this.has_default_duplicant_row = false;
		this.title = UI.VITALS;
		base.OnActivate();
		base.AddPortraitColumn("Portrait", new Action<IAssignableIdentity, GameObject>(base.on_load_portrait), null, true);
		base.AddButtonLabelColumn("Names", new Action<IAssignableIdentity, GameObject>(base.on_load_name_label), new Func<IAssignableIdentity, GameObject, string>(base.get_value_name_label), delegate(GameObject widget_go)
		{
			base.GetWidgetRow(widget_go).SelectMinion();
		}, delegate(GameObject widget_go)
		{
			base.GetWidgetRow(widget_go).SelectAndFocusMinion();
		}, new Comparison<IAssignableIdentity>(base.compare_rows_alphabetical), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_name), new Action<IAssignableIdentity, GameObject, ToolTip>(base.on_tooltip_sort_alphabetically), false);
		base.AddLabelColumn("Stress", new Action<IAssignableIdentity, GameObject>(this.on_load_stress), new Func<IAssignableIdentity, GameObject, string>(this.get_value_stress_label), new Comparison<IAssignableIdentity>(this.compare_rows_stress), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_stress), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_stress), 64, true);
		base.AddLabelColumn("QOLExpectations", new Action<IAssignableIdentity, GameObject>(this.on_load_qualityoflife_expectations), new Func<IAssignableIdentity, GameObject, string>(this.get_value_qualityoflife_expectations_label), new Comparison<IAssignableIdentity>(this.compare_rows_qualityoflife_expectations), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_qualityoflife_expectations), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_qualityoflife_expectations), 128, true);
		base.AddLabelColumn("Fullness", new Action<IAssignableIdentity, GameObject>(this.on_load_fullness), new Func<IAssignableIdentity, GameObject, string>(this.get_value_fullness_label), new Comparison<IAssignableIdentity>(this.compare_rows_fullness), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_fullness), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_fullness), 96, true);
		base.AddLabelColumn("EatenToday", new Action<IAssignableIdentity, GameObject>(this.on_load_eaten_today), new Func<IAssignableIdentity, GameObject, string>(this.get_value_eaten_today_label), new Comparison<IAssignableIdentity>(this.compare_rows_eaten_today), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_eaten_today), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_eaten_today), 96, true);
		base.AddLabelColumn("Health", new Action<IAssignableIdentity, GameObject>(this.on_load_health), new Func<IAssignableIdentity, GameObject, string>(this.get_value_health_label), new Comparison<IAssignableIdentity>(this.compare_rows_health), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_health), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_health), 64, true);
		base.AddLabelColumn("Immunity", new Action<IAssignableIdentity, GameObject>(this.on_load_sickness), new Func<IAssignableIdentity, GameObject, string>(this.get_value_sickness_label), new Comparison<IAssignableIdentity>(this.compare_rows_sicknesses), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sicknesses), new Action<IAssignableIdentity, GameObject, ToolTip>(this.on_tooltip_sort_sicknesses), 192, true);
	}

	// Token: 0x06009F66 RID: 40806 RVA: 0x003D0CFC File Offset: 0x003CEEFC
	private void on_load_stress(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN.STRESS.ToString());
	}

	// Token: 0x06009F67 RID: 40807 RVA: 0x003D0D5C File Offset: 0x003CEF5C
	private string get_value_stress_label(IAssignableIdentity identity, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion)
		{
			MinionIdentity minionIdentity = identity as MinionIdentity;
			if (minionIdentity != null)
			{
				return Db.Get().Amounts.Stress.Lookup(minionIdentity).GetValueString();
			}
		}
		else if (widgetRow.rowType == TableRow.RowType.StoredMinon)
		{
			return UI.TABLESCREENS.NA;
		}
		return "";
	}

	// Token: 0x06009F68 RID: 40808 RVA: 0x003D0DC0 File Offset: 0x003CEFC0
	private int compare_rows_stress(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		if (minionIdentity == null && minionIdentity2 == null)
		{
			return 0;
		}
		if (minionIdentity == null)
		{
			return -1;
		}
		if (minionIdentity2 == null)
		{
			return 1;
		}
		float value = Db.Get().Amounts.Stress.Lookup(minionIdentity).value;
		float value2 = Db.Get().Amounts.Stress.Lookup(minionIdentity2).value;
		return value2.CompareTo(value);
	}

	// Token: 0x06009F69 RID: 40809 RVA: 0x003D0E44 File Offset: 0x003CF044
	protected void on_tooltip_stress(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = minion as MinionIdentity;
			if (minionIdentity != null)
			{
				tooltip.AddMultiStringTooltip(Db.Get().Amounts.Stress.Lookup(minionIdentity).GetTooltip(), null);
				return;
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(minion, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x06009F6A RID: 40810 RVA: 0x003D0EB8 File Offset: 0x003CF0B8
	protected void on_tooltip_sort_stress(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_STRESS, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x06009F6B RID: 40811 RVA: 0x003CDC54 File Offset: 0x003CBE54
	private void on_load_qualityoflife_expectations(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN.QUALITYOFLIFE_EXPECTATIONS.ToString());
	}

	// Token: 0x06009F6C RID: 40812 RVA: 0x003D0F00 File Offset: 0x003CF100
	private string get_value_qualityoflife_expectations_label(IAssignableIdentity identity, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion)
		{
			MinionIdentity minionIdentity = identity as MinionIdentity;
			if (minionIdentity != null)
			{
				return Db.Get().Attributes.QualityOfLife.Lookup(minionIdentity).GetFormattedValue();
			}
		}
		else if (widgetRow.rowType == TableRow.RowType.StoredMinon)
		{
			return UI.TABLESCREENS.NA;
		}
		return "";
	}

	// Token: 0x06009F6D RID: 40813 RVA: 0x003CDD10 File Offset: 0x003CBF10
	private int compare_rows_qualityoflife_expectations(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		if (minionIdentity == null && minionIdentity2 == null)
		{
			return 0;
		}
		if (minionIdentity == null)
		{
			return -1;
		}
		if (minionIdentity2 == null)
		{
			return 1;
		}
		float totalValue = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(minionIdentity).GetTotalValue();
		float totalValue2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(minionIdentity2).GetTotalValue();
		return totalValue.CompareTo(totalValue2);
	}

	// Token: 0x06009F6E RID: 40814 RVA: 0x003D0F64 File Offset: 0x003CF164
	protected void on_tooltip_qualityoflife_expectations(IAssignableIdentity identity, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = identity as MinionIdentity;
			if (minionIdentity != null)
			{
				tooltip.AddMultiStringTooltip(Db.Get().Attributes.QualityOfLife.Lookup(minionIdentity).GetAttributeValueTooltip(), null);
				return;
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(identity, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x06009F6F RID: 40815 RVA: 0x003CDE08 File Offset: 0x003CC008
	protected void on_tooltip_sort_qualityoflife_expectations(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_EXPECTATIONS, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x06009F70 RID: 40816 RVA: 0x003D0FD8 File Offset: 0x003CF1D8
	private void on_load_health(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : (componentInChildren.text = UI.VITALSSCREEN_HEALTH.ToString()));
	}

	// Token: 0x06009F71 RID: 40817 RVA: 0x003D1040 File Offset: 0x003CF240
	private string get_value_health_label(IAssignableIdentity minion, GameObject widget_go)
	{
		if (minion != null)
		{
			TableRow widgetRow = base.GetWidgetRow(widget_go);
			if (widgetRow.rowType == TableRow.RowType.Minion && minion as MinionIdentity != null)
			{
				return Db.Get().Amounts.HitPoints.Lookup(minion as MinionIdentity).GetValueString();
			}
			if (widgetRow.rowType == TableRow.RowType.StoredMinon)
			{
				return UI.TABLESCREENS.NA;
			}
		}
		return "";
	}

	// Token: 0x06009F72 RID: 40818 RVA: 0x003D10A8 File Offset: 0x003CF2A8
	private int compare_rows_health(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		if (minionIdentity == null && minionIdentity2 == null)
		{
			return 0;
		}
		if (minionIdentity == null)
		{
			return -1;
		}
		if (minionIdentity2 == null)
		{
			return 1;
		}
		float value = Db.Get().Amounts.HitPoints.Lookup(minionIdentity).value;
		float value2 = Db.Get().Amounts.HitPoints.Lookup(minionIdentity2).value;
		return value2.CompareTo(value);
	}

	// Token: 0x06009F73 RID: 40819 RVA: 0x003D112C File Offset: 0x003CF32C
	protected void on_tooltip_health(IAssignableIdentity identity, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = identity as MinionIdentity;
			if (minionIdentity != null)
			{
				tooltip.AddMultiStringTooltip(Db.Get().Amounts.HitPoints.Lookup(minionIdentity).GetTooltip(), null);
				return;
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(identity, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x06009F74 RID: 40820 RVA: 0x003D11A0 File Offset: 0x003CF3A0
	protected void on_tooltip_sort_health(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_HITPOINTS, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x06009F75 RID: 40821 RVA: 0x003D11E8 File Offset: 0x003CF3E8
	private void on_load_sickness(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN_SICKNESS.ToString());
	}

	// Token: 0x06009F76 RID: 40822 RVA: 0x003D1248 File Offset: 0x003CF448
	private string get_value_sickness_label(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion)
		{
			MinionIdentity minionIdentity = minion as MinionIdentity;
			if (minionIdentity != null)
			{
				List<KeyValuePair<string, float>> list = new List<KeyValuePair<string, float>>();
				foreach (SicknessInstance sicknessInstance in minionIdentity.GetComponent<MinionModifiers>().sicknesses)
				{
					list.Add(new KeyValuePair<string, float>(sicknessInstance.modifier.Name, sicknessInstance.GetInfectedTimeRemaining()));
				}
				if (DlcManager.FeatureRadiationEnabled())
				{
					RadiationMonitor.Instance smi = minionIdentity.GetSMI<RadiationMonitor.Instance>();
					if (smi != null && smi.sm.isSick.Get(smi))
					{
						Effects component = minionIdentity.GetComponent<Effects>();
						string key;
						if (component.HasEffect(RadiationMonitor.minorSicknessEffect))
						{
							key = Db.Get().effects.Get(RadiationMonitor.minorSicknessEffect).Name;
						}
						else if (component.HasEffect(RadiationMonitor.majorSicknessEffect))
						{
							key = Db.Get().effects.Get(RadiationMonitor.majorSicknessEffect).Name;
						}
						else if (component.HasEffect(RadiationMonitor.extremeSicknessEffect))
						{
							key = Db.Get().effects.Get(RadiationMonitor.extremeSicknessEffect).Name;
						}
						else
						{
							key = DUPLICANTS.MODIFIERS.RADIATIONEXPOSUREDEADLY.NAME;
						}
						list.Add(new KeyValuePair<string, float>(key, smi.SicknessSecondsRemaining()));
					}
				}
				if (list.Count > 0)
				{
					string text = "";
					if (list.Count > 1)
					{
						float seconds = 0f;
						foreach (KeyValuePair<string, float> keyValuePair in list)
						{
							seconds = Mathf.Min(new float[]
							{
								keyValuePair.Value
							});
						}
						text += string.Format(UI.VITALSSCREEN.MULTIPLE_SICKNESSES, GameUtil.GetFormattedCycles(seconds, "F1", false));
					}
					else
					{
						foreach (KeyValuePair<string, float> keyValuePair2 in list)
						{
							if (!string.IsNullOrEmpty(text))
							{
								text += "\n";
							}
							text += string.Format(UI.VITALSSCREEN.SICKNESS_REMAINING, keyValuePair2.Key, GameUtil.GetFormattedCycles(keyValuePair2.Value, "F1", false));
						}
					}
					return text;
				}
				return UI.VITALSSCREEN.NO_SICKNESSES;
			}
		}
		else if (widgetRow.rowType == TableRow.RowType.StoredMinon)
		{
			return UI.TABLESCREENS.NA;
		}
		return "";
	}

	// Token: 0x06009F77 RID: 40823 RVA: 0x003D1500 File Offset: 0x003CF700
	private int compare_rows_sicknesses(IAssignableIdentity a, IAssignableIdentity b)
	{
		float value = 0f;
		return 0f.CompareTo(value);
	}

	// Token: 0x06009F78 RID: 40824 RVA: 0x003D1524 File Offset: 0x003CF724
	protected void on_tooltip_sicknesses(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = minion as MinionIdentity;
			if (minionIdentity != null)
			{
				bool flag = false;
				new List<KeyValuePair<string, float>>();
				if (DlcManager.FeatureRadiationEnabled())
				{
					RadiationMonitor.Instance smi = minionIdentity.GetSMI<RadiationMonitor.Instance>();
					if (smi != null && smi.sm.isSick.Get(smi))
					{
						tooltip.AddMultiStringTooltip(smi.GetEffectStatusTooltip(), null);
						flag = true;
					}
				}
				Sicknesses sicknesses = minionIdentity.GetComponent<MinionModifiers>().sicknesses;
				if (sicknesses.IsInfected())
				{
					flag = true;
					foreach (SicknessInstance sicknessInstance in sicknesses)
					{
						tooltip.AddMultiStringTooltip(UI.HORIZONTAL_RULE, null);
						tooltip.AddMultiStringTooltip(sicknessInstance.modifier.Name, null);
						StatusItem statusItem = sicknessInstance.GetStatusItem();
						tooltip.AddMultiStringTooltip(statusItem.GetTooltip(sicknessInstance.ExposureInfo), null);
					}
				}
				if (!flag)
				{
					tooltip.AddMultiStringTooltip(UI.VITALSSCREEN.NO_SICKNESSES, null);
					return;
				}
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(minion, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x06009F79 RID: 40825 RVA: 0x003D1660 File Offset: 0x003CF860
	protected void on_tooltip_sort_sicknesses(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_SICKNESSES, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x06009F7A RID: 40826 RVA: 0x003D16A8 File Offset: 0x003CF8A8
	private void on_load_fullness(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN_CALORIES.ToString());
	}

	// Token: 0x06009F7B RID: 40827 RVA: 0x003D1708 File Offset: 0x003CF908
	private string get_value_fullness_label(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion && minion as MinionIdentity != null)
		{
			AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(minion as MinionIdentity);
			if (amountInstance != null)
			{
				return amountInstance.GetValueString();
			}
			return UI.TABLESCREENS.NA;
		}
		else
		{
			if (widgetRow.rowType == TableRow.RowType.StoredMinon)
			{
				return UI.TABLESCREENS.NA;
			}
			return "";
		}
	}

	// Token: 0x06009F7C RID: 40828 RVA: 0x003D1780 File Offset: 0x003CF980
	private int compare_rows_fullness(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		if (minionIdentity == null && minionIdentity2 == null)
		{
			return 0;
		}
		if (minionIdentity == null)
		{
			return -1;
		}
		if (minionIdentity2 == null)
		{
			return 1;
		}
		AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(minionIdentity);
		AmountInstance amountInstance2 = Db.Get().Amounts.Calories.Lookup(minionIdentity2);
		if (amountInstance == null && amountInstance2 == null)
		{
			return 0;
		}
		if (amountInstance == null)
		{
			return -1;
		}
		if (amountInstance2 == null)
		{
			return 1;
		}
		float value = amountInstance.value;
		float value2 = amountInstance2.value;
		return value2.CompareTo(value);
	}

	// Token: 0x06009F7D RID: 40829 RVA: 0x003D181C File Offset: 0x003CFA1C
	protected void on_tooltip_fullness(IAssignableIdentity identity, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
		{
			MinionIdentity minionIdentity = identity as MinionIdentity;
			if (minionIdentity != null)
			{
				AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(minionIdentity);
				if (amountInstance != null)
				{
					tooltip.AddMultiStringTooltip(amountInstance.GetTooltip(), null);
					return;
				}
			}
			break;
		}
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(identity, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x06009F7E RID: 40830 RVA: 0x003D1894 File Offset: 0x003CFA94
	protected void on_tooltip_sort_fullness(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_FULLNESS, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x06009F7F RID: 40831 RVA: 0x003CEB20 File Offset: 0x003CCD20
	protected void on_tooltip_name(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
		case TableRow.RowType.StoredMinon:
			break;
		case TableRow.RowType.Minion:
			if (minion != null)
			{
				tooltip.AddMultiStringTooltip(string.Format(UI.TABLESCREENS.GOTO_DUPLICANT_BUTTON, minion.GetProperName()), null);
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06009F80 RID: 40832 RVA: 0x003D18DC File Offset: 0x003CFADC
	private void on_load_eaten_today(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		LocText componentInChildren = widget_go.GetComponentInChildren<LocText>(true);
		if (minion != null)
		{
			componentInChildren.text = (base.GetWidgetColumn(widget_go) as LabelTableColumn).get_value_action(minion, widget_go);
			return;
		}
		componentInChildren.text = (widgetRow.isDefault ? "" : UI.VITALSSCREEN_EATENTODAY.ToString());
	}

	// Token: 0x06009F81 RID: 40833 RVA: 0x003D193C File Offset: 0x003CFB3C
	private static float RationsEatenToday(MinionIdentity minion)
	{
		float result = 0f;
		if (minion != null)
		{
			RationMonitor.Instance smi = minion.GetSMI<RationMonitor.Instance>();
			if (smi != null)
			{
				result = smi.GetRationsAteToday();
			}
		}
		return result;
	}

	// Token: 0x06009F82 RID: 40834 RVA: 0x003D196C File Offset: 0x003CFB6C
	private string get_value_eaten_today_label(IAssignableIdentity minion, GameObject widget_go)
	{
		TableRow widgetRow = base.GetWidgetRow(widget_go);
		if (widgetRow.rowType == TableRow.RowType.Minion)
		{
			return GameUtil.GetFormattedCalories(VitalsTableScreen.RationsEatenToday(minion as MinionIdentity), GameUtil.TimeSlice.None, true);
		}
		if (widgetRow.rowType == TableRow.RowType.StoredMinon)
		{
			return UI.TABLESCREENS.NA;
		}
		return "";
	}

	// Token: 0x06009F83 RID: 40835 RVA: 0x003D19B8 File Offset: 0x003CFBB8
	private int compare_rows_eaten_today(IAssignableIdentity a, IAssignableIdentity b)
	{
		MinionIdentity minionIdentity = a as MinionIdentity;
		MinionIdentity minionIdentity2 = b as MinionIdentity;
		if (minionIdentity == null && minionIdentity2 == null)
		{
			return 0;
		}
		if (minionIdentity == null)
		{
			return -1;
		}
		if (minionIdentity2 == null)
		{
			return 1;
		}
		float value = VitalsTableScreen.RationsEatenToday(minionIdentity);
		return VitalsTableScreen.RationsEatenToday(minionIdentity2).CompareTo(value);
	}

	// Token: 0x06009F84 RID: 40836 RVA: 0x003D1A14 File Offset: 0x003CFC14
	protected void on_tooltip_eaten_today(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
		case TableRow.RowType.Default:
			break;
		case TableRow.RowType.Minion:
			if (minion != null)
			{
				float calories = VitalsTableScreen.RationsEatenToday(minion as MinionIdentity);
				tooltip.AddMultiStringTooltip(string.Format(UI.VITALSSCREEN.EATEN_TODAY_TOOLTIP, GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true)), null);
				return;
			}
			break;
		case TableRow.RowType.StoredMinon:
			this.StoredMinionTooltip(minion, tooltip);
			break;
		default:
			return;
		}
	}

	// Token: 0x06009F85 RID: 40837 RVA: 0x003D1A84 File Offset: 0x003CFC84
	protected void on_tooltip_sort_eaten_today(IAssignableIdentity minion, GameObject widget_go, ToolTip tooltip)
	{
		tooltip.ClearMultiStringTooltip();
		switch (base.GetWidgetRow(widget_go).rowType)
		{
		case TableRow.RowType.Header:
			tooltip.AddMultiStringTooltip(UI.TABLESCREENS.COLUMN_SORT_BY_EATEN_TODAY, null);
			break;
		case TableRow.RowType.Default:
		case TableRow.RowType.Minion:
		case TableRow.RowType.StoredMinon:
			break;
		default:
			return;
		}
	}

	// Token: 0x06009F86 RID: 40838 RVA: 0x00107C3B File Offset: 0x00105E3B
	private void StoredMinionTooltip(IAssignableIdentity minion, ToolTip tooltip)
	{
		if (minion != null && minion as StoredMinionIdentity != null)
		{
			tooltip.AddMultiStringTooltip(string.Format(UI.TABLESCREENS.INFORMATION_NOT_AVAILABLE_TOOLTIP, (minion as StoredMinionIdentity).GetStorageReason(), minion.GetProperName()), null);
		}
	}
}
