using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E6E RID: 3694
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicWattageSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06004A21 RID: 18977 RVA: 0x000D0011 File Offset: 0x000CE211
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicWattageSensor>(-905833192, LogicWattageSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06004A22 RID: 18978 RVA: 0x00259C0C File Offset: 0x00257E0C
	private void OnCopySettings(object data)
	{
		LogicWattageSensor component = ((GameObject)data).GetComponent<LogicWattageSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06004A23 RID: 18979 RVA: 0x000D002A File Offset: 0x000CE22A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06004A24 RID: 18980 RVA: 0x00259C48 File Offset: 0x00257E48
	public void Sim200ms(float dt)
	{
		this.currentWattage = Game.Instance.circuitManager.GetWattsUsedByCircuit(Game.Instance.circuitManager.GetCircuitID(Grid.PosToCell(this)));
		this.currentWattage = Mathf.Max(0f, this.currentWattage);
		if (this.activateOnHigherThan)
		{
			if ((this.currentWattage > this.thresholdWattage && !base.IsSwitchedOn) || (this.currentWattage <= this.thresholdWattage && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.currentWattage >= this.thresholdWattage && base.IsSwitchedOn) || (this.currentWattage < this.thresholdWattage && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06004A25 RID: 18981 RVA: 0x000D005D File Offset: 0x000CE25D
	public float GetWattageUsed()
	{
		return this.currentWattage;
	}

	// Token: 0x06004A26 RID: 18982 RVA: 0x000D0065 File Offset: 0x000CE265
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x06004A27 RID: 18983 RVA: 0x000CA11E File Offset: 0x000C831E
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06004A28 RID: 18984 RVA: 0x00259D04 File Offset: 0x00257F04
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

	// Token: 0x06004A29 RID: 18985 RVA: 0x00253D94 File Offset: 0x00251F94
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x06004A2A RID: 18986 RVA: 0x000D0074 File Offset: 0x000CE274
	// (set) Token: 0x06004A2B RID: 18987 RVA: 0x000D007C File Offset: 0x000CE27C
	public float Threshold
	{
		get
		{
			return this.thresholdWattage;
		}
		set
		{
			this.thresholdWattage = value;
			this.dirty = true;
		}
	}

	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x06004A2C RID: 18988 RVA: 0x000D008C File Offset: 0x000CE28C
	// (set) Token: 0x06004A2D RID: 18989 RVA: 0x000D0094 File Offset: 0x000CE294
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnHigherThan;
		}
		set
		{
			this.activateOnHigherThan = value;
			this.dirty = true;
		}
	}

	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x06004A2E RID: 18990 RVA: 0x000D00A4 File Offset: 0x000CE2A4
	public float CurrentValue
	{
		get
		{
			return this.GetWattageUsed();
		}
	}

	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x06004A2F RID: 18991 RVA: 0x000D00AC File Offset: 0x000CE2AC
	public float RangeMin
	{
		get
		{
			return this.minWattage;
		}
	}

	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x06004A30 RID: 18992 RVA: 0x000D00B4 File Offset: 0x000CE2B4
	public float RangeMax
	{
		get
		{
			return this.maxWattage;
		}
	}

	// Token: 0x06004A31 RID: 18993 RVA: 0x000D00AC File Offset: 0x000CE2AC
	public float GetRangeMinInputField()
	{
		return this.minWattage;
	}

	// Token: 0x06004A32 RID: 18994 RVA: 0x000D00B4 File Offset: 0x000CE2B4
	public float GetRangeMaxInputField()
	{
		return this.maxWattage;
	}

	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x06004A33 RID: 18995 RVA: 0x000D00BC File Offset: 0x000CE2BC
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.WATTAGESWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x06004A34 RID: 18996 RVA: 0x000D00C3 File Offset: 0x000CE2C3
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.WATTAGE;
		}
	}

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x06004A35 RID: 18997 RVA: 0x000D00CA File Offset: 0x000CE2CA
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.WATTAGE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x06004A36 RID: 18998 RVA: 0x000D00D6 File Offset: 0x000CE2D6
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.WATTAGE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06004A37 RID: 18999 RVA: 0x000D00E2 File Offset: 0x000CE2E2
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedWattage(value, GameUtil.WattageFormatterUnit.Watts, units);
	}

	// Token: 0x06004A38 RID: 19000 RVA: 0x000CA28F File Offset: 0x000C848F
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06004A39 RID: 19001 RVA: 0x000B1FA8 File Offset: 0x000B01A8
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06004A3A RID: 19002 RVA: 0x000D00EC File Offset: 0x000CE2EC
	public LocString ThresholdValueUnits()
	{
		return UI.UNITSUFFIXES.ELECTRICAL.WATT;
	}

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x06004A3B RID: 19003 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x06004A3C RID: 19004 RVA: 0x000A65EC File Offset: 0x000A47EC
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x06004A3D RID: 19005 RVA: 0x00259D8C File Offset: 0x00257F8C
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(5f, 5f),
				new NonLinearSlider.Range(35f, 1000f),
				new NonLinearSlider.Range(50f, 3000f),
				new NonLinearSlider.Range(10f, this.maxWattage)
			};
		}
	}

	// Token: 0x04003379 RID: 13177
	[Serialize]
	public float thresholdWattage;

	// Token: 0x0400337A RID: 13178
	[Serialize]
	public bool activateOnHigherThan;

	// Token: 0x0400337B RID: 13179
	[Serialize]
	public bool dirty = true;

	// Token: 0x0400337C RID: 13180
	private readonly float minWattage;

	// Token: 0x0400337D RID: 13181
	private readonly float maxWattage = 1.5f * Wire.GetMaxWattageAsFloat(Wire.WattageRating.Max50000);

	// Token: 0x0400337E RID: 13182
	private float currentWattage;

	// Token: 0x0400337F RID: 13183
	private bool wasOn;

	// Token: 0x04003380 RID: 13184
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003381 RID: 13185
	private static readonly EventSystem.IntraObjectHandler<LogicWattageSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicWattageSensor>(delegate(LogicWattageSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
