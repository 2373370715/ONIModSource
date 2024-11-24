using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

// Token: 0x02001E0B RID: 7691
public class MeterScreen_Electrobanks : MeterScreen_ValueTrackerDisplayer
{
	// Token: 0x0600A10C RID: 41228 RVA: 0x003D689C File Offset: 0x003D4A9C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.LiveMinionIdentities.OnAdd += this.OnNewMinionAdded;
		List<MinionIdentity> worldMinionIdentities = this.GetWorldMinionIdentities();
		bool visibility;
		if (worldMinionIdentities != null)
		{
			visibility = (worldMinionIdentities.Find((MinionIdentity m) => m.model == BionicMinionConfig.MODEL) != null);
		}
		else
		{
			visibility = false;
		}
		this.SetVisibility(visibility);
	}

	// Token: 0x0600A10D RID: 41229 RVA: 0x00108B03 File Offset: 0x00106D03
	protected override void OnCleanUp()
	{
		Components.LiveMinionIdentities.OnAdd -= this.OnNewMinionAdded;
		base.OnCleanUp();
	}

	// Token: 0x0600A10E RID: 41230 RVA: 0x00108B21 File Offset: 0x00106D21
	private void OnNewMinionAdded(MinionIdentity id)
	{
		if (id.model == BionicMinionConfig.MODEL)
		{
			this.SetVisibility(true);
		}
	}

	// Token: 0x0600A10F RID: 41231 RVA: 0x00108B3C File Offset: 0x00106D3C
	public void SetVisibility(bool isVisible)
	{
		base.gameObject.SetActive(isVisible);
	}

	// Token: 0x0600A110 RID: 41232 RVA: 0x003D6904 File Offset: 0x003D4B04
	protected override string OnTooltip()
	{
		this.joulesDictionary.Clear();
		string formattedJoules = GameUtil.GetFormattedJoules(WorldResourceAmountTracker<ElectrobankTracker>.Get().CountAmount(this.joulesDictionary, ClusterManager.Instance.activeWorld.worldInventory, true), "F1", GameUtil.TimeSlice.None);
		this.Label.text = formattedJoules;
		this.Tooltip.ClearMultiStringTooltip();
		this.Tooltip.AddMultiStringTooltip(string.Format(UI.TOOLTIPS.METERSCREEN_ELECTROBANK_JOULES, formattedJoules), this.ToolTipStyle_Header);
		this.Tooltip.AddMultiStringTooltip("", this.ToolTipStyle_Property);
		foreach (KeyValuePair<string, float> keyValuePair in (from x in this.joulesDictionary
		orderby x.Value descending
		select x).ToDictionary((KeyValuePair<string, float> t) => t.Key, (KeyValuePair<string, float> t) => t.Value))
		{
			GameObject prefab = Assets.GetPrefab(keyValuePair.Key);
			this.Tooltip.AddMultiStringTooltip((prefab != null) ? string.Format("{0}: {1}", prefab.GetProperName(), GameUtil.GetFormattedJoules(keyValuePair.Value * 120000f, "F1", GameUtil.TimeSlice.None)) : string.Format(UI.TOOLTIPS.METERSCREEN_INVALID_ELECTROBANK_TYPE, keyValuePair.Key), this.ToolTipStyle_Property);
		}
		return "";
	}

	// Token: 0x0600A111 RID: 41233 RVA: 0x003D6AB0 File Offset: 0x003D4CB0
	protected override void InternalRefresh()
	{
		if (!SaveLoader.Instance.IsDLCActiveForCurrentSave("DLC3_ID"))
		{
			return;
		}
		if (this.Label != null && WorldResourceAmountTracker<ElectrobankTracker>.Get() != null)
		{
			long num = (long)WorldResourceAmountTracker<ElectrobankTracker>.Get().CountAmount(null, ClusterManager.Instance.activeWorld.worldInventory, true);
			if (this.cachedJoules != num)
			{
				this.Label.text = GameUtil.GetFormattedJoules((float)num, "F1", GameUtil.TimeSlice.None);
				this.cachedJoules = num;
			}
		}
		this.diagnosticGraph.GetComponentInChildren<SparkLayer>().SetColor(((float)this.cachedJoules > (float)this.GetWorldMinionIdentities().Count * 120000f) ? Constants.NEUTRAL_COLOR : Constants.NEGATIVE_COLOR);
		WorldTracker worldTracker = TrackerTool.Instance.GetWorldTracker<ElectrobankJoulesTracker>(ClusterManager.Instance.activeWorldId);
		if (worldTracker != null)
		{
			this.diagnosticGraph.GetComponentInChildren<LineLayer>().RefreshLine(worldTracker.ChartableData(600f), "joules");
		}
	}

	// Token: 0x04007DB6 RID: 32182
	private long cachedJoules = -1L;

	// Token: 0x04007DB7 RID: 32183
	private Dictionary<string, float> joulesDictionary = new Dictionary<string, float>();
}
