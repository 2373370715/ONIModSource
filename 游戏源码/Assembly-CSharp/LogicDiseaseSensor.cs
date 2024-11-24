using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E3C RID: 3644
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicDiseaseSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06004808 RID: 18440 RVA: 0x000CEC49 File Offset: 0x000CCE49
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicDiseaseSensor>(-905833192, LogicDiseaseSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06004809 RID: 18441 RVA: 0x00254430 File Offset: 0x00252630
	private void OnCopySettings(object data)
	{
		LogicDiseaseSensor component = ((GameObject)data).GetComponent<LogicDiseaseSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x0600480A RID: 18442 RVA: 0x000CEC62 File Offset: 0x000CCE62
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x0600480B RID: 18443 RVA: 0x0025446C File Offset: 0x0025266C
	public void Sim200ms(float dt)
	{
		if (this.sampleIdx < 8)
		{
			int i = Grid.PosToCell(this);
			if (Grid.Mass[i] > 0f)
			{
				this.samples[this.sampleIdx] = Grid.DiseaseCount[i];
				this.sampleIdx++;
			}
			return;
		}
		this.sampleIdx = 0;
		float currentValue = this.CurrentValue;
		if (this.activateAboveThreshold)
		{
			if ((currentValue > this.threshold && !base.IsSwitchedOn) || (currentValue <= this.threshold && base.IsSwitchedOn))
			{
				this.Toggle();
			}
		}
		else if ((currentValue > this.threshold && base.IsSwitchedOn) || (currentValue <= this.threshold && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
		this.animController.SetSymbolVisiblity(LogicDiseaseSensor.TINT_SYMBOL, currentValue > 0f);
	}

	// Token: 0x0600480C RID: 18444 RVA: 0x000CECA1 File Offset: 0x000CCEA1
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x17000384 RID: 900
	// (get) Token: 0x0600480D RID: 18445 RVA: 0x000CECB0 File Offset: 0x000CCEB0
	// (set) Token: 0x0600480E RID: 18446 RVA: 0x000CECB8 File Offset: 0x000CCEB8
	public float Threshold
	{
		get
		{
			return this.threshold;
		}
		set
		{
			this.threshold = value;
		}
	}

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x0600480F RID: 18447 RVA: 0x000CECC1 File Offset: 0x000CCEC1
	// (set) Token: 0x06004810 RID: 18448 RVA: 0x000CECC9 File Offset: 0x000CCEC9
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateAboveThreshold;
		}
		set
		{
			this.activateAboveThreshold = value;
		}
	}

	// Token: 0x17000386 RID: 902
	// (get) Token: 0x06004811 RID: 18449 RVA: 0x00254548 File Offset: 0x00252748
	public float CurrentValue
	{
		get
		{
			float num = 0f;
			for (int i = 0; i < 8; i++)
			{
				num += (float)this.samples[i];
			}
			return num / 8f;
		}
	}

	// Token: 0x17000387 RID: 903
	// (get) Token: 0x06004812 RID: 18450 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float RangeMin
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x06004813 RID: 18451 RVA: 0x000CA1CC File Offset: 0x000C83CC
	public float RangeMax
	{
		get
		{
			return 100000f;
		}
	}

	// Token: 0x06004814 RID: 18452 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float GetRangeMinInputField()
	{
		return 0f;
	}

	// Token: 0x06004815 RID: 18453 RVA: 0x000CA1CC File Offset: 0x000C83CC
	public float GetRangeMaxInputField()
	{
		return 100000f;
	}

	// Token: 0x17000389 RID: 905
	// (get) Token: 0x06004816 RID: 18454 RVA: 0x000CECD2 File Offset: 0x000CCED2
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE;
		}
	}

	// Token: 0x1700038A RID: 906
	// (get) Token: 0x06004817 RID: 18455 RVA: 0x000CA1E1 File Offset: 0x000C83E1
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x06004818 RID: 18456 RVA: 0x000CA1ED File Offset: 0x000C83ED
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06004819 RID: 18457 RVA: 0x000CA1F9 File Offset: 0x000C83F9
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedInt((float)((int)value), GameUtil.TimeSlice.None);
	}

	// Token: 0x0600481A RID: 18458 RVA: 0x000B1FA8 File Offset: 0x000B01A8
	public float ProcessedSliderValue(float input)
	{
		return input;
	}

	// Token: 0x0600481B RID: 18459 RVA: 0x000B1FA8 File Offset: 0x000B01A8
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x0600481C RID: 18460 RVA: 0x000CA204 File Offset: 0x000C8404
	public LocString ThresholdValueUnits()
	{
		return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_UNITS;
	}

	// Token: 0x1700038C RID: 908
	// (get) Token: 0x0600481D RID: 18461 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x0600481E RID: 18462 RVA: 0x000CECD9 File Offset: 0x000CCED9
	public int IncrementScale
	{
		get
		{
			return 100;
		}
	}

	// Token: 0x1700038E RID: 910
	// (get) Token: 0x0600481F RID: 18463 RVA: 0x000CECDD File Offset: 0x000CCEDD
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x06004820 RID: 18464 RVA: 0x000CA11E File Offset: 0x000C831E
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06004821 RID: 18465 RVA: 0x0025457C File Offset: 0x0025277C
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			if (this.switchedOn)
			{
				this.animController.Play(LogicDiseaseSensor.ON_ANIMS, KAnim.PlayMode.Loop);
				int i = Grid.PosToCell(this);
				byte b = Grid.DiseaseIdx[i];
				Color32 c = Color.white;
				if (b != 255)
				{
					Disease disease = Db.Get().Diseases[(int)b];
					c = GlobalAssets.Instance.colorSet.GetColorByName(disease.overlayColourName);
				}
				this.animController.SetSymbolTint(LogicDiseaseSensor.TINT_SYMBOL, c);
				return;
			}
			this.animController.Play(LogicDiseaseSensor.OFF_ANIMS, KAnim.PlayMode.Once);
		}
	}

	// Token: 0x06004822 RID: 18466 RVA: 0x00253D94 File Offset: 0x00251F94
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x1700038F RID: 911
	// (get) Token: 0x06004823 RID: 18467 RVA: 0x000CA1D3 File Offset: 0x000C83D3
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.DISEASE_TITLE;
		}
	}

	// Token: 0x04003204 RID: 12804
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x04003205 RID: 12805
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x04003206 RID: 12806
	private KBatchedAnimController animController;

	// Token: 0x04003207 RID: 12807
	private bool wasOn;

	// Token: 0x04003208 RID: 12808
	private const float rangeMin = 0f;

	// Token: 0x04003209 RID: 12809
	private const float rangeMax = 100000f;

	// Token: 0x0400320A RID: 12810
	private const int WINDOW_SIZE = 8;

	// Token: 0x0400320B RID: 12811
	private int[] samples = new int[8];

	// Token: 0x0400320C RID: 12812
	private int sampleIdx;

	// Token: 0x0400320D RID: 12813
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400320E RID: 12814
	private static readonly EventSystem.IntraObjectHandler<LogicDiseaseSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicDiseaseSensor>(delegate(LogicDiseaseSensor component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400320F RID: 12815
	private static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on_loop"
	};

	// Token: 0x04003210 RID: 12816
	private static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"on_pst",
		"off"
	};

	// Token: 0x04003211 RID: 12817
	private static readonly HashedString TINT_SYMBOL = "germs";
}
