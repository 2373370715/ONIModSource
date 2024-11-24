﻿using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using TUNING;

// Token: 0x02001E0D RID: 7693
public class MeterScreen_Rations : MeterScreen_ValueTrackerDisplayer
{
	// Token: 0x0600A119 RID: 41241 RVA: 0x003D6BA0 File Offset: 0x003D4DA0
	protected override string OnTooltip()
	{
		this.rationsDict.Clear();
		float calories = WorldResourceAmountTracker<RationTracker>.Get().CountAmount(this.rationsDict, ClusterManager.Instance.activeWorld.worldInventory, true);
		this.Label.text = GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true);
		this.Tooltip.ClearMultiStringTooltip();
		this.Tooltip.AddMultiStringTooltip(string.Format(UI.TOOLTIPS.METERSCREEN_MEALHISTORY, GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true), GameUtil.GetFormattedCalories(-MinionIdentity.GetCalorieBurnMultiplier() * DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_CYCLE, GameUtil.TimeSlice.None, true)), this.ToolTipStyle_Header);
		this.Tooltip.AddMultiStringTooltip("", this.ToolTipStyle_Property);
		foreach (KeyValuePair<string, float> keyValuePair in this.rationsDict.OrderByDescending(delegate(KeyValuePair<string, float> x)
		{
			EdiblesManager.FoodInfo foodInfo2 = EdiblesManager.GetFoodInfo(x.Key);
			return x.Value * ((foodInfo2 != null) ? foodInfo2.CaloriesPerUnit : -1f);
		}).ToDictionary((KeyValuePair<string, float> t) => t.Key, (KeyValuePair<string, float> t) => t.Value))
		{
			EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(keyValuePair.Key);
			this.Tooltip.AddMultiStringTooltip((foodInfo != null) ? string.Format("{0}: {1}", foodInfo.Name, GameUtil.GetFormattedCalories(keyValuePair.Value * foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true)) : string.Format(UI.TOOLTIPS.METERSCREEN_INVALID_FOOD_TYPE, keyValuePair.Key), this.ToolTipStyle_Property);
		}
		return "";
	}

	// Token: 0x0600A11A RID: 41242 RVA: 0x003D6D5C File Offset: 0x003D4F5C
	protected override void InternalRefresh()
	{
		if (this.Label != null && WorldResourceAmountTracker<RationTracker>.Get() != null)
		{
			long num = (long)WorldResourceAmountTracker<RationTracker>.Get().CountAmount(null, ClusterManager.Instance.activeWorld.worldInventory, true);
			if (this.cachedCalories != num)
			{
				this.Label.text = GameUtil.GetFormattedCalories((float)num, GameUtil.TimeSlice.None, true);
				this.cachedCalories = num;
			}
		}
		this.diagnosticGraph.GetComponentInChildren<SparkLayer>().SetColor(((float)this.cachedCalories > (float)this.GetWorldMinionIdentities().Count * FOOD.FOOD_CALORIES_PER_CYCLE) ? Constants.NEUTRAL_COLOR : Constants.NEGATIVE_COLOR);
		this.diagnosticGraph.GetComponentInChildren<LineLayer>().RefreshLine(TrackerTool.Instance.GetWorldTracker<KCalTracker>(ClusterManager.Instance.activeWorldId).ChartableData(600f), "kcal");
	}

	// Token: 0x04007DBD RID: 32189
	private long cachedCalories = -1L;

	// Token: 0x04007DBE RID: 32190
	private Dictionary<string, float> rationsDict = new Dictionary<string, float>();
}
