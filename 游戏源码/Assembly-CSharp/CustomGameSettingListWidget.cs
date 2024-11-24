using System;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02001C88 RID: 7304
public class CustomGameSettingListWidget : CustomGameSettingWidget
{
	// Token: 0x06009842 RID: 38978 RVA: 0x00102FF1 File Offset: 0x001011F1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.CycleLeft.onClick += this.DoCycleLeft;
		this.CycleRight.onClick += this.DoCycleRight;
	}

	// Token: 0x06009843 RID: 38979 RVA: 0x00103027 File Offset: 0x00101227
	public void Initialize(ListSettingConfig config, Func<SettingConfig, SettingLevel> getCallback, Func<ListSettingConfig, int, SettingLevel> cycleCallback)
	{
		this.config = config;
		this.Label.text = config.label;
		this.ToolTip.toolTip = config.tooltip;
		this.getCallback = getCallback;
		this.cycleCallback = cycleCallback;
	}

	// Token: 0x06009844 RID: 38980 RVA: 0x003AF638 File Offset: 0x003AD838
	public override void Refresh()
	{
		base.Refresh();
		SettingLevel settingLevel = this.getCallback(this.config);
		this.ValueLabel.text = settingLevel.label;
		this.ValueToolTip.toolTip = settingLevel.tooltip;
		this.CycleLeft.isInteractable = !this.config.IsFirstLevel(settingLevel.id);
		this.CycleRight.isInteractable = !this.config.IsLastLevel(settingLevel.id);
	}

	// Token: 0x06009845 RID: 38981 RVA: 0x00103060 File Offset: 0x00101260
	private void DoCycleLeft()
	{
		this.cycleCallback(this.config, -1);
		base.Notify();
	}

	// Token: 0x06009846 RID: 38982 RVA: 0x0010307B File Offset: 0x0010127B
	private void DoCycleRight()
	{
		this.cycleCallback(this.config, 1);
		base.Notify();
	}

	// Token: 0x0400768B RID: 30347
	[SerializeField]
	private LocText Label;

	// Token: 0x0400768C RID: 30348
	[SerializeField]
	private ToolTip ToolTip;

	// Token: 0x0400768D RID: 30349
	[SerializeField]
	private LocText ValueLabel;

	// Token: 0x0400768E RID: 30350
	[SerializeField]
	private ToolTip ValueToolTip;

	// Token: 0x0400768F RID: 30351
	[SerializeField]
	private KButton CycleLeft;

	// Token: 0x04007690 RID: 30352
	[SerializeField]
	private KButton CycleRight;

	// Token: 0x04007691 RID: 30353
	private ListSettingConfig config;

	// Token: 0x04007692 RID: 30354
	protected Func<ListSettingConfig, int, SettingLevel> cycleCallback;

	// Token: 0x04007693 RID: 30355
	protected Func<SettingConfig, SettingLevel> getCallback;
}
