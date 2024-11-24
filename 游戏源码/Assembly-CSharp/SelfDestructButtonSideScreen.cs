using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001FC9 RID: 8137
public class SelfDestructButtonSideScreen : SideScreenContent
{
	// Token: 0x0600AC3E RID: 44094 RVA: 0x0010FEE7 File Offset: 0x0010E0E7
	protected override void OnSpawn()
	{
		this.Refresh();
		this.button.onClick += this.TriggerDestruct;
	}

	// Token: 0x0600AC3F RID: 44095 RVA: 0x0010D723 File Offset: 0x0010B923
	public override int GetSideScreenSortOrder()
	{
		return -150;
	}

	// Token: 0x0600AC40 RID: 44096 RVA: 0x0010FF06 File Offset: 0x0010E106
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<CraftModuleInterface>() != null && target.HasTag(GameTags.RocketInSpace);
	}

	// Token: 0x0600AC41 RID: 44097 RVA: 0x0010FF23 File Offset: 0x0010E123
	public override void SetTarget(GameObject target)
	{
		this.craftInterface = target.GetComponent<CraftModuleInterface>();
		this.acknowledgeWarnings = false;
		this.craftInterface.Subscribe<SelfDestructButtonSideScreen>(-1582839653, SelfDestructButtonSideScreen.TagsChangedDelegate);
		this.Refresh();
	}

	// Token: 0x0600AC42 RID: 44098 RVA: 0x0010FF54 File Offset: 0x0010E154
	public override void ClearTarget()
	{
		if (this.craftInterface != null)
		{
			this.craftInterface.Unsubscribe<SelfDestructButtonSideScreen>(-1582839653, SelfDestructButtonSideScreen.TagsChangedDelegate, false);
			this.craftInterface = null;
		}
	}

	// Token: 0x0600AC43 RID: 44099 RVA: 0x0010FF81 File Offset: 0x0010E181
	private void OnTagsChanged(object data)
	{
		if (((TagChangedEventData)data).tag == GameTags.RocketStranded)
		{
			this.Refresh();
		}
	}

	// Token: 0x0600AC44 RID: 44100 RVA: 0x0010FFA0 File Offset: 0x0010E1A0
	private void TriggerDestruct()
	{
		if (this.acknowledgeWarnings)
		{
			this.craftInterface.gameObject.Trigger(-1061799784, null);
			this.acknowledgeWarnings = false;
		}
		else
		{
			this.acknowledgeWarnings = true;
		}
		this.Refresh();
	}

	// Token: 0x0600AC45 RID: 44101 RVA: 0x0040E464 File Offset: 0x0040C664
	private void Refresh()
	{
		if (this.craftInterface == null)
		{
			return;
		}
		this.statusText.text = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.MESSAGE_TEXT;
		if (this.acknowledgeWarnings)
		{
			this.button.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TEXT_CONFIRM;
			this.button.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TOOLTIP_CONFIRM;
			return;
		}
		this.button.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TEXT;
		this.button.GetComponentInChildren<ToolTip>().toolTip = UI.UISIDESCREENS.SELFDESTRUCTSIDESCREEN.BUTTON_TOOLTIP;
	}

	// Token: 0x04008760 RID: 34656
	public KButton button;

	// Token: 0x04008761 RID: 34657
	public LocText statusText;

	// Token: 0x04008762 RID: 34658
	private CraftModuleInterface craftInterface;

	// Token: 0x04008763 RID: 34659
	private bool acknowledgeWarnings;

	// Token: 0x04008764 RID: 34660
	private static readonly EventSystem.IntraObjectHandler<SelfDestructButtonSideScreen> TagsChangedDelegate = new EventSystem.IntraObjectHandler<SelfDestructButtonSideScreen>(delegate(SelfDestructButtonSideScreen cmp, object data)
	{
		cmp.OnTagsChanged(data);
	});
}
