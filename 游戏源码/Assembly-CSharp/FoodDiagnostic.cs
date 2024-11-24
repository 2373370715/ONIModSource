using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200122A RID: 4650
public class FoodDiagnostic : ColonyDiagnostic
{
	// Token: 0x06005F44 RID: 24388 RVA: 0x002A8820 File Offset: 0x002A6A20
	public FoodDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.ALL_NAME)
	{
		this.tracker = TrackerTool.Instance.GetWorldTracker<KCalTracker>(worldID);
		this.icon = "icon_category_food";
		this.trackerSampleCountSeconds = 150f;
		this.presentationSetting = ColonyDiagnostic.PresentationSetting.CurrentValue;
		base.AddCriterion("CheckEnoughFood", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA.CHECKENOUGHFOOD, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEnoughFood)));
		base.AddCriterion("CheckStarvation", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA.CHECKSTARVATION, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckStarvation)));
		this.multiplier = MinionIdentity.GetCalorieBurnMultiplier();
		this.recommendedKCalPerDuplicant = 3000f * this.multiplier;
	}

	// Token: 0x06005F45 RID: 24389 RVA: 0x002A88E0 File Offset: 0x002A6AE0
	private ColonyDiagnostic.DiagnosticResult CheckAnyFood()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA_HAS_FOOD.PASS, null);
		if (Components.LiveMinionIdentities.GetWorldItems(base.worldID, false).Count != 0)
		{
			if (this.tracker.GetDataTimeLength() < 10f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
				result.Message = UI.COLONY_DIAGNOSTICS.NO_DATA;
			}
			else if (this.tracker.GetAverageValue(this.trackerSampleCountSeconds) == 0f)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Bad;
				result.Message = UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.CRITERIA_HAS_FOOD.FAIL;
			}
		}
		return result;
	}

	// Token: 0x06005F46 RID: 24390 RVA: 0x002A8978 File Offset: 0x002A6B78
	private ColonyDiagnostic.DiagnosticResult CheckEnoughFood()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		List<MinionIdentity> list = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false).FindAll((MinionIdentity MID) => Db.Get().Amounts.Calories.Lookup(MID) != null);
		if (this.tracker.GetDataTimeLength() < 10f)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.NO_DATA;
		}
		else if ((float)list.Count * (1000f * this.recommendedKCalPerDuplicant) > this.tracker.GetAverageValue(this.trackerSampleCountSeconds))
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
			float currentValue = this.tracker.GetCurrentValue();
			float f = (float)list.Count * DUPLICANTSTATS.STANDARD.BaseStats.CALORIES_BURNED_PER_CYCLE * this.multiplier;
			string formattedCalories = GameUtil.GetFormattedCalories(currentValue, GameUtil.TimeSlice.None, true);
			string formattedCalories2 = GameUtil.GetFormattedCalories(Mathf.Abs(f), GameUtil.TimeSlice.None, true);
			string text = MISC.NOTIFICATIONS.FOODLOW.TOOLTIP;
			text = text.Replace("{0}", formattedCalories);
			text = text.Replace("{1}", formattedCalories2);
			result.Message = text;
		}
		return result;
	}

	// Token: 0x06005F47 RID: 24391 RVA: 0x002A8AA4 File Offset: 0x002A6CA4
	private ColonyDiagnostic.DiagnosticResult CheckStarvation()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(base.worldID, false))
		{
			if (!minionIdentity.IsNull())
			{
				CalorieMonitor.Instance smi = minionIdentity.GetSMI<CalorieMonitor.Instance>();
				if (!smi.IsNullOrStopped() && smi.IsInsideState(smi.sm.hungry.starving))
				{
					result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Bad;
					result.Message = UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.HUNGRY;
					result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(smi.gameObject.transform.position, smi.gameObject);
				}
			}
		}
		return result;
	}

	// Token: 0x06005F48 RID: 24392 RVA: 0x000DE2FC File Offset: 0x000DC4FC
	public override string GetCurrentValueString()
	{
		return GameUtil.GetFormattedCalories(this.tracker.GetCurrentValue(), GameUtil.TimeSlice.None, true);
	}

	// Token: 0x06005F49 RID: 24393 RVA: 0x002A8B7C File Offset: 0x002A6D7C
	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult;
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out diagnosticResult))
		{
			return diagnosticResult;
		}
		diagnosticResult = base.Evaluate();
		if (diagnosticResult.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal)
		{
			diagnosticResult.Message = UI.COLONY_DIAGNOSTICS.FOODDIAGNOSTIC.NORMAL;
		}
		return diagnosticResult;
	}

	// Token: 0x040043B7 RID: 17335
	private const int CYCLES_OF_FOOD = 3;

	// Token: 0x040043B8 RID: 17336
	private const float BASE_KCAL_PER_CYCLE = 1000f;

	// Token: 0x040043B9 RID: 17337
	private float multiplier = 1f;

	// Token: 0x040043BA RID: 17338
	private float recommendedKCalPerDuplicant;
}
