using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F44 RID: 8004
public class ClusterGridWorldSideScreen : SideScreenContent
{
	// Token: 0x0600A904 RID: 43268 RVA: 0x0010DCCC File Offset: 0x0010BECC
	protected override void OnSpawn()
	{
		this.viewButton.onClick += this.OnClickView;
	}

	// Token: 0x0600A905 RID: 43269 RVA: 0x0010DCE5 File Offset: 0x0010BEE5
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<AsteroidGridEntity>() != null;
	}

	// Token: 0x0600A906 RID: 43270 RVA: 0x003FF18C File Offset: 0x003FD38C
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetEntity = target.GetComponent<AsteroidGridEntity>();
		this.icon.sprite = Def.GetUISprite(this.targetEntity, "ui", false).first;
		WorldContainer component = this.targetEntity.GetComponent<WorldContainer>();
		bool flag = component != null && component.IsDiscovered;
		this.viewButton.isInteractable = flag;
		if (!flag)
		{
			this.viewButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERWORLDSIDESCREEN.VIEW_WORLD_DISABLE_TOOLTIP);
			return;
		}
		this.viewButton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERWORLDSIDESCREEN.VIEW_WORLD_TOOLTIP);
	}

	// Token: 0x0600A907 RID: 43271 RVA: 0x003FF230 File Offset: 0x003FD430
	private void OnClickView()
	{
		WorldContainer component = this.targetEntity.GetComponent<WorldContainer>();
		if (!component.IsDupeVisited)
		{
			component.LookAtSurface();
		}
		ClusterManager.Instance.SetActiveWorld(component.id);
		ManagementMenu.Instance.CloseAll();
	}

	// Token: 0x040084DA RID: 34010
	public Image icon;

	// Token: 0x040084DB RID: 34011
	public KButton viewButton;

	// Token: 0x040084DC RID: 34012
	private AsteroidGridEntity targetEntity;
}
