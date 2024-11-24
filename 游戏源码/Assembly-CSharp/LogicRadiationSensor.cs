using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E5E RID: 3678
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicRadiationSensor : Switch, ISaveLoadable, IThresholdSwitch, ISim200ms
{
	// Token: 0x06004971 RID: 18801 RVA: 0x000CFA66 File Offset: 0x000CDC66
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicRadiationSensor>(-905833192, LogicRadiationSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06004972 RID: 18802 RVA: 0x00258948 File Offset: 0x00256B48
	private void OnCopySettings(object data)
	{
		LogicRadiationSensor component = ((GameObject)data).GetComponent<LogicRadiationSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x06004973 RID: 18803 RVA: 0x000CFA7F File Offset: 0x000CDC7F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateVisualState(true);
		this.UpdateLogicCircuit();
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06004974 RID: 18804 RVA: 0x00258984 File Offset: 0x00256B84
	public void Sim200ms(float dt)
	{
		if (this.simUpdateCounter < 8 && !this.dirty)
		{
			int i = Grid.PosToCell(this);
			this.radHistory[this.simUpdateCounter] = Grid.Radiation[i];
			this.simUpdateCounter++;
			return;
		}
		this.simUpdateCounter = 0;
		this.dirty = false;
		this.averageRads = 0f;
		for (int j = 0; j < 8; j++)
		{
			this.averageRads += this.radHistory[j];
		}
		this.averageRads /= 8f;
		if (this.activateOnWarmerThan)
		{
			if ((this.averageRads > this.thresholdRads && !base.IsSwitchedOn) || (this.averageRads <= this.thresholdRads && base.IsSwitchedOn))
			{
				this.Toggle();
				return;
			}
		}
		else if ((this.averageRads >= this.thresholdRads && base.IsSwitchedOn) || (this.averageRads < this.thresholdRads && !base.IsSwitchedOn))
		{
			this.Toggle();
		}
	}

	// Token: 0x06004975 RID: 18805 RVA: 0x000CFAB2 File Offset: 0x000CDCB2
	public float GetAverageRads()
	{
		return this.averageRads;
	}

	// Token: 0x06004976 RID: 18806 RVA: 0x000CFABA File Offset: 0x000CDCBA
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateVisualState(false);
		this.UpdateLogicCircuit();
	}

	// Token: 0x06004977 RID: 18807 RVA: 0x000CA11E File Offset: 0x000C831E
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06004978 RID: 18808 RVA: 0x00258A8C File Offset: 0x00256C8C
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

	// Token: 0x06004979 RID: 18809 RVA: 0x00253D94 File Offset: 0x00251F94
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x0600497A RID: 18810 RVA: 0x000CFAC9 File Offset: 0x000CDCC9
	// (set) Token: 0x0600497B RID: 18811 RVA: 0x000CFAD1 File Offset: 0x000CDCD1
	public float Threshold
	{
		get
		{
			return this.thresholdRads;
		}
		set
		{
			this.thresholdRads = value;
			this.dirty = true;
		}
	}

	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x0600497C RID: 18812 RVA: 0x000CFAE1 File Offset: 0x000CDCE1
	// (set) Token: 0x0600497D RID: 18813 RVA: 0x000CFAE9 File Offset: 0x000CDCE9
	public bool ActivateAboveThreshold
	{
		get
		{
			return this.activateOnWarmerThan;
		}
		set
		{
			this.activateOnWarmerThan = value;
			this.dirty = true;
		}
	}

	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x0600497E RID: 18814 RVA: 0x000CFAF9 File Offset: 0x000CDCF9
	public float CurrentValue
	{
		get
		{
			return this.GetAverageRads();
		}
	}

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x0600497F RID: 18815 RVA: 0x000CFB01 File Offset: 0x000CDD01
	public float RangeMin
	{
		get
		{
			return this.minRads;
		}
	}

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x06004980 RID: 18816 RVA: 0x000CFB09 File Offset: 0x000CDD09
	public float RangeMax
	{
		get
		{
			return this.maxRads;
		}
	}

	// Token: 0x06004981 RID: 18817 RVA: 0x000CFB11 File Offset: 0x000CDD11
	public float GetRangeMinInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMin, false);
	}

	// Token: 0x06004982 RID: 18818 RVA: 0x000CFB1F File Offset: 0x000CDD1F
	public float GetRangeMaxInputField()
	{
		return GameUtil.GetConvertedTemperature(this.RangeMax, false);
	}

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x06004983 RID: 18819 RVA: 0x000CFB2D File Offset: 0x000CDD2D
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.RADIATIONSWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170003DC RID: 988
	// (get) Token: 0x06004984 RID: 18820 RVA: 0x000CFB34 File Offset: 0x000CDD34
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.RADIATION;
		}
	}

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x06004985 RID: 18821 RVA: 0x000CFB3B File Offset: 0x000CDD3B
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.RADIATION_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x06004986 RID: 18822 RVA: 0x000CFB47 File Offset: 0x000CDD47
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.RADIATION_TOOLTIP_BELOW;
		}
	}

	// Token: 0x06004987 RID: 18823 RVA: 0x000C3A2D File Offset: 0x000C1C2D
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedRads(value, GameUtil.TimeSlice.None);
	}

	// Token: 0x06004988 RID: 18824 RVA: 0x000CA28F File Offset: 0x000C848F
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x06004989 RID: 18825 RVA: 0x000B1FA8 File Offset: 0x000B01A8
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x0600498A RID: 18826 RVA: 0x000CEBE2 File Offset: 0x000CCDE2
	public LocString ThresholdValueUnits()
	{
		return "";
	}

	// Token: 0x170003DF RID: 991
	// (get) Token: 0x0600498B RID: 18827 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x0600498C RID: 18828 RVA: 0x000A65EC File Offset: 0x000A47EC
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x0600498D RID: 18829 RVA: 0x00258B14 File Offset: 0x00256D14
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(50f, 200f),
				new NonLinearSlider.Range(25f, 1000f),
				new NonLinearSlider.Range(25f, 5000f)
			};
		}
	}

	// Token: 0x04003322 RID: 13090
	private int simUpdateCounter;

	// Token: 0x04003323 RID: 13091
	[Serialize]
	public float thresholdRads = 280f;

	// Token: 0x04003324 RID: 13092
	[Serialize]
	public bool activateOnWarmerThan;

	// Token: 0x04003325 RID: 13093
	[Serialize]
	private bool dirty = true;

	// Token: 0x04003326 RID: 13094
	public float minRads;

	// Token: 0x04003327 RID: 13095
	public float maxRads = 5000f;

	// Token: 0x04003328 RID: 13096
	private const int NumFrameDelay = 8;

	// Token: 0x04003329 RID: 13097
	private float[] radHistory = new float[8];

	// Token: 0x0400332A RID: 13098
	private float averageRads;

	// Token: 0x0400332B RID: 13099
	private bool wasOn;

	// Token: 0x0400332C RID: 13100
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400332D RID: 13101
	private static readonly EventSystem.IntraObjectHandler<LogicRadiationSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicRadiationSensor>(delegate(LogicRadiationSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
