using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001FE3 RID: 8163
public class TemporalTearSideScreen : SideScreenContent
{
	// Token: 0x17000B15 RID: 2837
	// (get) Token: 0x0600AD04 RID: 44292 RVA: 0x001108C8 File Offset: 0x0010EAC8
	private CraftModuleInterface craftModuleInterface
	{
		get
		{
			return this.targetCraft.GetComponent<CraftModuleInterface>();
		}
	}

	// Token: 0x0600AD05 RID: 44293 RVA: 0x0010E6EC File Offset: 0x0010C8EC
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x0600AD06 RID: 44294 RVA: 0x000FE620 File Offset: 0x000FC820
	public override float GetSortKey()
	{
		return 21f;
	}

	// Token: 0x0600AD07 RID: 44295 RVA: 0x00410278 File Offset: 0x0040E478
	public override bool IsValidForTarget(GameObject target)
	{
		Clustercraft component = target.GetComponent<Clustercraft>();
		TemporalTear temporalTear = ClusterManager.Instance.GetComponent<ClusterPOIManager>().GetTemporalTear();
		return component != null && temporalTear != null && temporalTear.Location == component.Location;
	}

	// Token: 0x0600AD08 RID: 44296 RVA: 0x004102C4 File Offset: 0x0040E4C4
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetCraft = target.GetComponent<Clustercraft>();
		KButton reference = base.GetComponent<HierarchyReferences>().GetReference<KButton>("button");
		reference.ClearOnClick();
		reference.onClick += delegate()
		{
			target.GetComponent<Clustercraft>();
			ClusterManager.Instance.GetComponent<ClusterPOIManager>().GetTemporalTear().ConsumeCraft(this.targetCraft);
		};
		this.RefreshPanel(null);
	}

	// Token: 0x0600AD09 RID: 44297 RVA: 0x00410330 File Offset: 0x0040E530
	private void RefreshPanel(object data = null)
	{
		TemporalTear temporalTear = ClusterManager.Instance.GetComponent<ClusterPOIManager>().GetTemporalTear();
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		bool flag = temporalTear.IsOpen();
		component.GetReference<LocText>("label").SetText(flag ? UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.BUTTON_OPEN : UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.BUTTON_CLOSED);
		component.GetReference<KButton>("button").isInteractable = flag;
	}

	// Token: 0x040087C7 RID: 34759
	private Clustercraft targetCraft;
}
