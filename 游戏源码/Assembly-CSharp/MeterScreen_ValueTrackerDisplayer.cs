using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001E14 RID: 7700
public abstract class MeterScreen_ValueTrackerDisplayer : KMonoBehaviour
{
	// Token: 0x0600A133 RID: 41267 RVA: 0x00108C2B File Offset: 0x00106E2B
	protected override void OnSpawn()
	{
		this.Tooltip.OnToolTip = new Func<string>(this.OnTooltip);
		base.OnSpawn();
	}

	// Token: 0x0600A134 RID: 41268 RVA: 0x00108C4B File Offset: 0x00106E4B
	public void Refresh()
	{
		this.RefreshWorldMinionIdentities();
		this.InternalRefresh();
	}

	// Token: 0x0600A135 RID: 41269
	protected abstract void InternalRefresh();

	// Token: 0x0600A136 RID: 41270
	protected abstract string OnTooltip();

	// Token: 0x0600A137 RID: 41271 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void OnClick(BaseEventData base_ev_data)
	{
	}

	// Token: 0x0600A138 RID: 41272 RVA: 0x003D7324 File Offset: 0x003D5524
	private void RefreshWorldMinionIdentities()
	{
		this.worldLiveMinionIdentities = new List<MinionIdentity>(from x in Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId, false)
		where !x.IsNullOrDestroyed()
		select x);
	}

	// Token: 0x0600A139 RID: 41273 RVA: 0x00108C59 File Offset: 0x00106E59
	protected virtual List<MinionIdentity> GetWorldMinionIdentities()
	{
		if (this.worldLiveMinionIdentities == null)
		{
			this.RefreshWorldMinionIdentities();
		}
		return this.worldLiveMinionIdentities;
	}

	// Token: 0x04007DC7 RID: 32199
	public LocText Label;

	// Token: 0x04007DC8 RID: 32200
	public ToolTip Tooltip;

	// Token: 0x04007DC9 RID: 32201
	public GameObject diagnosticGraph;

	// Token: 0x04007DCA RID: 32202
	public TextStyleSetting ToolTipStyle_Header;

	// Token: 0x04007DCB RID: 32203
	public TextStyleSetting ToolTipStyle_Property;

	// Token: 0x04007DCC RID: 32204
	private List<MinionIdentity> worldLiveMinionIdentities;
}
