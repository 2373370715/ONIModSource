using System;
using UnityEngine;

// Token: 0x02001CA9 RID: 7337
public abstract class DetailScreenTab : TargetPanel
{
	// Token: 0x0600991E RID: 39198
	public abstract override bool IsValidForTarget(GameObject target);

	// Token: 0x0600991F RID: 39199 RVA: 0x001039E4 File Offset: 0x00101BE4
	protected override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
	}

	// Token: 0x06009920 RID: 39200 RVA: 0x003B37D4 File Offset: 0x003B19D4
	protected CollapsibleDetailContentPanel CreateCollapsableSection(string title = null)
	{
		CollapsibleDetailContentPanel collapsibleDetailContentPanel = Util.KInstantiateUI<CollapsibleDetailContentPanel>(ScreenPrefabs.Instance.CollapsableContentPanel, base.gameObject, false);
		if (!string.IsNullOrEmpty(title))
		{
			collapsibleDetailContentPanel.SetTitle(title);
		}
		return collapsibleDetailContentPanel;
	}

	// Token: 0x06009921 RID: 39201 RVA: 0x001039ED File Offset: 0x00101BED
	private void Update()
	{
		this.Refresh(false);
	}

	// Token: 0x06009922 RID: 39202 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void Refresh(bool force = false)
	{
	}
}
