﻿using System;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class LogicTimeOfDaySensor : Switch, ISaveLoadable, ISim200ms
{
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicTimeOfDaySensor>(-905833192, LogicTimeOfDaySensor.OnCopySettingsDelegate);
	}

	private void OnCopySettings(object data)
	{
		LogicTimeOfDaySensor component = ((GameObject)data).GetComponent<LogicTimeOfDaySensor>();
		if (component != null)
		{
			this.startTime = component.startTime;
			this.duration = component.duration;
		}
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	public void Sim200ms(float dt)
	{
		float currentCycleAsPercentage = GameClock.Instance.GetCurrentCycleAsPercentage();
		bool state = false;
		if (currentCycleAsPercentage >= this.startTime && currentCycleAsPercentage < this.startTime + this.duration)
		{
			state = true;
		}
		if (currentCycleAsPercentage < this.startTime + this.duration - 1f)
		{
			state = true;
		}
		this.SetState(state);
	}

	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	[SerializeField]
	[Serialize]
	public float startTime;

	[SerializeField]
	[Serialize]
	public float duration = 1f;

	private bool wasOn;

	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	private static readonly EventSystem.IntraObjectHandler<LogicTimeOfDaySensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicTimeOfDaySensor>(delegate(LogicTimeOfDaySensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
