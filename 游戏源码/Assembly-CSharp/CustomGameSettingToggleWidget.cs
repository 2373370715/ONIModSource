using System;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02001C8A RID: 7306
public class CustomGameSettingToggleWidget : CustomGameSettingWidget
{
	// Token: 0x06009851 RID: 38993 RVA: 0x00103119 File Offset: 0x00101319
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle toggle = this.Toggle;
		toggle.onClick = (System.Action)Delegate.Combine(toggle.onClick, new System.Action(this.ToggleSetting));
	}

	// Token: 0x06009852 RID: 38994 RVA: 0x00103148 File Offset: 0x00101348
	public void Initialize(ToggleSettingConfig config, Func<SettingConfig, SettingLevel> getCurrentSettingCallback, Func<ToggleSettingConfig, SettingLevel> toggleCallback)
	{
		this.config = config;
		this.Label.text = config.label;
		this.ToolTip.toolTip = config.tooltip;
		this.getCurrentSettingCallback = getCurrentSettingCallback;
		this.toggleCallback = toggleCallback;
	}

	// Token: 0x06009853 RID: 38995 RVA: 0x003AF8CC File Offset: 0x003ADACC
	public override void Refresh()
	{
		base.Refresh();
		SettingLevel settingLevel = this.getCurrentSettingCallback(this.config);
		this.Toggle.ChangeState(this.config.IsOnLevel(settingLevel.id) ? 1 : 0);
		this.ToggleToolTip.toolTip = settingLevel.tooltip;
	}

	// Token: 0x06009854 RID: 38996 RVA: 0x00103181 File Offset: 0x00101381
	public void ToggleSetting()
	{
		this.toggleCallback(this.config);
		base.Notify();
	}

	// Token: 0x0400769D RID: 30365
	[SerializeField]
	private LocText Label;

	// Token: 0x0400769E RID: 30366
	[SerializeField]
	private ToolTip ToolTip;

	// Token: 0x0400769F RID: 30367
	[SerializeField]
	private MultiToggle Toggle;

	// Token: 0x040076A0 RID: 30368
	[SerializeField]
	private ToolTip ToggleToolTip;

	// Token: 0x040076A1 RID: 30369
	private ToggleSettingConfig config;

	// Token: 0x040076A2 RID: 30370
	protected Func<SettingConfig, SettingLevel> getCurrentSettingCallback;

	// Token: 0x040076A3 RID: 30371
	protected Func<ToggleSettingConfig, SettingLevel> toggleCallback;
}
