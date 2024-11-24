using System;
using Klei.CustomSettings;
using UnityEngine;

public class CustomGameSettingToggleWidget : CustomGameSettingWidget
{
	[SerializeField]
	private LocText Label;

	[SerializeField]
	private ToolTip ToolTip;

	[SerializeField]
	private MultiToggle Toggle;

	[SerializeField]
	private ToolTip ToggleToolTip;

	private ToggleSettingConfig config;

	protected Func<SettingConfig, SettingLevel> getCurrentSettingCallback;

	protected Func<ToggleSettingConfig, SettingLevel> toggleCallback;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle toggle = Toggle;
		toggle.onClick = (System.Action)Delegate.Combine(toggle.onClick, new System.Action(ToggleSetting));
	}

	public void Initialize(ToggleSettingConfig config, Func<SettingConfig, SettingLevel> getCurrentSettingCallback, Func<ToggleSettingConfig, SettingLevel> toggleCallback)
	{
		this.config = config;
		Label.text = config.label;
		ToolTip.toolTip = config.tooltip;
		this.getCurrentSettingCallback = getCurrentSettingCallback;
		this.toggleCallback = toggleCallback;
	}

	public override void Refresh()
	{
		base.Refresh();
		SettingLevel settingLevel = getCurrentSettingCallback(config);
		Toggle.ChangeState(config.IsOnLevel(settingLevel.id) ? 1 : 0);
		ToggleToolTip.toolTip = settingLevel.tooltip;
	}

	public void ToggleSetting()
	{
		toggleCallback(config);
		Notify();
	}
}
