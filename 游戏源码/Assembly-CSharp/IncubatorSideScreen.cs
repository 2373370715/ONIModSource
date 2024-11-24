using System;
using UnityEngine;

// Token: 0x02001F79 RID: 8057
public class IncubatorSideScreen : ReceptacleSideScreen
{
	// Token: 0x0600AA07 RID: 43527 RVA: 0x0010E799 File Offset: 0x0010C999
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<EggIncubator>() != null;
	}

	// Token: 0x0600AA08 RID: 43528 RVA: 0x00403C80 File Offset: 0x00401E80
	protected override void SetResultDescriptions(GameObject go)
	{
		string text = "";
		InfoDescription component = go.GetComponent<InfoDescription>();
		if (component)
		{
			text += component.description;
		}
		this.descriptionLabel.SetText(text);
	}

	// Token: 0x0600AA09 RID: 43529 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	protected override bool RequiresAvailableAmountToDeposit()
	{
		return false;
	}

	// Token: 0x0600AA0A RID: 43530 RVA: 0x0010E7A7 File Offset: 0x0010C9A7
	protected override Sprite GetEntityIcon(Tag prefabTag)
	{
		return Def.GetUISprite(Assets.GetPrefab(prefabTag), "ui", false).first;
	}

	// Token: 0x0600AA0B RID: 43531 RVA: 0x00403CBC File Offset: 0x00401EBC
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		EggIncubator incubator = target.GetComponent<EggIncubator>();
		this.continuousToggle.ChangeState(incubator.autoReplaceEntity ? 0 : 1);
		this.continuousToggle.onClick = delegate()
		{
			incubator.autoReplaceEntity = !incubator.autoReplaceEntity;
			this.continuousToggle.ChangeState(incubator.autoReplaceEntity ? 0 : 1);
		};
	}

	// Token: 0x040085B1 RID: 34225
	public DescriptorPanel RequirementsDescriptorPanel;

	// Token: 0x040085B2 RID: 34226
	public DescriptorPanel HarvestDescriptorPanel;

	// Token: 0x040085B3 RID: 34227
	public DescriptorPanel EffectsDescriptorPanel;

	// Token: 0x040085B4 RID: 34228
	public MultiToggle continuousToggle;
}
