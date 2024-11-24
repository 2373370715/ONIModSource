using System;
using Klei.CustomSettings;
using UnityEngine;

public class CustomGameSettingListWidget : CustomGameSettingWidget
{
	[SerializeField]
	private LocText Label;

	[SerializeField]
	private ToolTip ToolTip;

	[SerializeField]
	private LocText ValueLabel;

	[SerializeField]
	private ToolTip ValueToolTip;

	[SerializeField]
	private KButton CycleLeft;

	[SerializeField]
	private KButton CycleRight;

	private ListSettingConfig config;

	protected Func<ListSettingConfig, int, SettingLevel> cycleCallback;

	protected Func<SettingConfig, SettingLevel> getCallback;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		CycleLeft.onClick += DoCycleLeft;
		CycleRight.onClick += DoCycleRight;
	}

	public void Initialize(ListSettingConfig config, Func<SettingConfig, SettingLevel> getCallback, Func<ListSettingConfig, int, SettingLevel> cycleCallback)
	{
		this.config = config;
		Label.text = config.label;
		ToolTip.toolTip = config.tooltip;
		this.getCallback = getCallback;
		this.cycleCallback = cycleCallback;
	}

	public override void Refresh()
	{
		base.Refresh();
		SettingLevel settingLevel = getCallback(config);
		ValueLabel.text = settingLevel.label;
		ValueToolTip.toolTip = settingLevel.tooltip;
		CycleLeft.isInteractable = !config.IsFirstLevel(settingLevel.id);
		CycleRight.isInteractable = !config.IsLastLevel(settingLevel.id);
	}

	private void DoCycleLeft()
	{
		cycleCallback(config, -1);
		Notify();
	}

	private void DoCycleRight()
	{
		cycleCallback(config, 1);
		Notify();
	}
}
