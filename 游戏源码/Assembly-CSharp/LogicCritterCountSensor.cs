using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E3A RID: 3642
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicCritterCountSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x060047E7 RID: 18407 RVA: 0x000CEB11 File Offset: 0x000CCD11
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.selectable = base.GetComponent<KSelectable>();
		base.Subscribe<LogicCritterCountSensor>(-905833192, LogicCritterCountSensor.OnCopySettingsDelegate);
	}

	// Token: 0x060047E8 RID: 18408 RVA: 0x00254220 File Offset: 0x00252420
	private void OnCopySettings(object data)
	{
		LogicCritterCountSensor component = ((GameObject)data).GetComponent<LogicCritterCountSensor>();
		if (component != null)
		{
			this.countThreshold = component.countThreshold;
			this.activateOnGreaterThan = component.activateOnGreaterThan;
			this.countCritters = component.countCritters;
			this.countEggs = component.countEggs;
		}
	}

	// Token: 0x060047E9 RID: 18409 RVA: 0x000CEB36 File Offset: 0x000CCD36
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x060047EA RID: 18410 RVA: 0x00254274 File Offset: 0x00252474
	public void Sim200ms(float dt)
	{
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			this.currentCount = 0;
			if (this.countCritters)
			{
				this.currentCount += roomOfGameObject.cavity.creatures.Count;
			}
			if (this.countEggs)
			{
				this.currentCount += roomOfGameObject.cavity.eggs.Count;
			}
			bool state = this.activateOnGreaterThan ? (this.currentCount > this.countThreshold) : (this.currentCount < this.countThreshold);
			this.SetState(state);
			if (this.selectable.HasStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom))
			{
				this.selectable.RemoveStatusItem(this.roomStatusGUID, false);
				return;
			}
		}
		else
		{
			if (!this.selectable.HasStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom))
			{
				this.roomStatusGUID = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.NotInAnyRoom, null);
			}
			this.SetState(false);
		}
	}

	// Token: 0x060047EB RID: 18411 RVA: 0x000CEB69 File Offset: 0x000CCD69
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x060047EC RID: 18412 RVA: 0x000CA11E File Offset: 0x000C831E
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060047ED RID: 18413 RVA: 0x00254390 File Offset: 0x00252590
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			if (this.switchedOn)
			{
				component.Queue("on", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
			component.Queue("off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x060047EE RID: 18414 RVA: 0x00253D94 File Offset: 0x00251F94
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x17000378 RID: 888
	// (get) Token: 0x060047EF RID: 18415 RVA: 0x000CEB78 File Offset: 0x000CCD78
	// (set) Token: 0x060047F0 RID: 18416 RVA: 0x000CEB81 File Offset: 0x000CCD81
	public float Threshold
	{
		get
		{
			return (float)this.countThreshold;
		}
		set
		{
			this.countThreshold = (int)value;
		}
	}

	// Token: 0x17000379 RID: 889
	// (get) Token: 0x060047F1 RID: 18417 RVA: 0x000CEB8B File Offset: 0x000CCD8B
	// (set) Token: 0x060047F2 RID: 18418 RVA: 0x000CEB93 File Offset: 0x000CCD93
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnGreaterThan;
		}
		set
		{
			this.activateOnGreaterThan = value;
		}
	}

	// Token: 0x1700037A RID: 890
	// (get) Token: 0x060047F3 RID: 18419 RVA: 0x000CEB9C File Offset: 0x000CCD9C
	public float CurrentValue
	{
		get
		{
			return (float)this.currentCount;
		}
	}

	// Token: 0x1700037B RID: 891
	// (get) Token: 0x060047F4 RID: 18420 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float RangeMin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700037C RID: 892
	// (get) Token: 0x060047F5 RID: 18421 RVA: 0x000CEBA5 File Offset: 0x000CCDA5
	public float RangeMax
	{
		get
		{
			return 64f;
		}
	}

	// Token: 0x060047F6 RID: 18422 RVA: 0x000CEBAC File Offset: 0x000CCDAC
	public float GetRangeMinInputField()
	{
		return this.RangeMin;
	}

	// Token: 0x060047F7 RID: 18423 RVA: 0x000CEBB4 File Offset: 0x000CCDB4
	public float GetRangeMaxInputField()
	{
		return this.RangeMax;
	}

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x060047F8 RID: 18424 RVA: 0x000CEBBC File Offset: 0x000CCDBC
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TITLE;
		}
	}

	// Token: 0x1700037E RID: 894
	// (get) Token: 0x060047F9 RID: 18425 RVA: 0x000CEBC3 File Offset: 0x000CCDC3
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.VALUE_NAME;
		}
	}

	// Token: 0x1700037F RID: 895
	// (get) Token: 0x060047FA RID: 18426 RVA: 0x000CEBCA File Offset: 0x000CCDCA
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TOOLTIP_ABOVE;
		}
	}

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x060047FB RID: 18427 RVA: 0x000CEBD6 File Offset: 0x000CCDD6
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.CRITTER_COUNT_SIDE_SCREEN.TOOLTIP_BELOW;
		}
	}

	// Token: 0x060047FC RID: 18428 RVA: 0x000C20D4 File Offset: 0x000C02D4
	public string Format(float value, bool units)
	{
		return value.ToString();
	}

	// Token: 0x060047FD RID: 18429 RVA: 0x000CA28F File Offset: 0x000C848F
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x060047FE RID: 18430 RVA: 0x000CA28F File Offset: 0x000C848F
	public float ProcessedInputValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x060047FF RID: 18431 RVA: 0x000CEBE2 File Offset: 0x000CCDE2
	public LocString ThresholdValueUnits()
	{
		return "";
	}

	// Token: 0x17000381 RID: 897
	// (get) Token: 0x06004800 RID: 18432 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000382 RID: 898
	// (get) Token: 0x06004801 RID: 18433 RVA: 0x000A65EC File Offset: 0x000A47EC
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000383 RID: 899
	// (get) Token: 0x06004802 RID: 18434 RVA: 0x000CEBEE File Offset: 0x000CCDEE
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x040031F9 RID: 12793
	private bool wasOn;

	// Token: 0x040031FA RID: 12794
	[Serialize]
	public bool countEggs = true;

	// Token: 0x040031FB RID: 12795
	[Serialize]
	public bool countCritters = true;

	// Token: 0x040031FC RID: 12796
	[Serialize]
	public int countThreshold;

	// Token: 0x040031FD RID: 12797
	[Serialize]
	public bool activateOnGreaterThan = true;

	// Token: 0x040031FE RID: 12798
	[Serialize]
	public int currentCount;

	// Token: 0x040031FF RID: 12799
	private KSelectable selectable;

	// Token: 0x04003200 RID: 12800
	private Guid roomStatusGUID;

	// Token: 0x04003201 RID: 12801
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003202 RID: 12802
	private static readonly EventSystem.IntraObjectHandler<LogicCritterCountSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicCritterCountSensor>(delegate(LogicCritterCountSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
