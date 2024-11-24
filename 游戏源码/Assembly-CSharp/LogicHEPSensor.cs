using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E50 RID: 3664
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicHEPSensor : Switch, ISaveLoadable, IThresholdSwitch, ISimEveryTick
{
	// Token: 0x060048C9 RID: 18633 RVA: 0x000CF415 File Offset: 0x000CD615
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicHEPSensor>(-905833192, LogicHEPSensor.OnCopySettingsDelegate);
	}

	// Token: 0x060048CA RID: 18634 RVA: 0x002575E0 File Offset: 0x002557E0
	private void OnCopySettings(object data)
	{
		LogicHEPSensor component = ((GameObject)data).GetComponent<LogicHEPSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x060048CB RID: 18635 RVA: 0x0025761C File Offset: 0x0025581C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Combine(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
	}

	// Token: 0x060048CC RID: 18636 RVA: 0x000CF42E File Offset: 0x000CD62E
	protected override void OnCleanUp()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		logicCircuitManager.onLogicTick = (System.Action)Delegate.Remove(logicCircuitManager.onLogicTick, new System.Action(this.LogicTick));
		base.OnCleanUp();
	}

	// Token: 0x060048CD RID: 18637 RVA: 0x00257688 File Offset: 0x00255888
	public void SimEveryTick(float dt)
	{
		if (this.waitForLogicTick)
		{
			return;
		}
		Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(this));
		ListPool<ScenePartitionerEntry, LogicHEPSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicHEPSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(vector2I.x, vector2I.y, 1, 1, GameScenePartitioner.Instance.collisionLayer, pooledList);
		float num = 0f;
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			HighEnergyParticle component = (scenePartitionerEntry.obj as KCollider2D).gameObject.GetComponent<HighEnergyParticle>();
			if (!(component == null) && component.isCollideable)
			{
				num += component.payload;
			}
		}
		pooledList.Recycle();
		this.foundPayload = num;
		bool flag = (this.activateOnHigherThan && num > this.thresholdPayload) || (!this.activateOnHigherThan && num < this.thresholdPayload);
		if (flag != this.switchedOn)
		{
			this.waitForLogicTick = true;
		}
		this.SetState(flag);
	}

	// Token: 0x060048CE RID: 18638 RVA: 0x000CF461 File Offset: 0x000CD661
	private void LogicTick()
	{
		this.waitForLogicTick = false;
	}

	// Token: 0x060048CF RID: 18639 RVA: 0x000CF46A File Offset: 0x000CD66A
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x060048D0 RID: 18640 RVA: 0x000CA11E File Offset: 0x000C831E
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060048D1 RID: 18641 RVA: 0x00257794 File Offset: 0x00255994
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

	// Token: 0x060048D2 RID: 18642 RVA: 0x00253D94 File Offset: 0x00251F94
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x060048D3 RID: 18643 RVA: 0x000CF479 File Offset: 0x000CD679
	// (set) Token: 0x060048D4 RID: 18644 RVA: 0x000CF481 File Offset: 0x000CD681
	public float Threshold
	{
		get
		{
			return this.thresholdPayload;
		}
		set
		{
			this.thresholdPayload = value;
			this.dirty = true;
		}
	}

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x060048D5 RID: 18645 RVA: 0x000CF491 File Offset: 0x000CD691
	// (set) Token: 0x060048D6 RID: 18646 RVA: 0x000CF499 File Offset: 0x000CD699
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

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x060048D7 RID: 18647 RVA: 0x000CF4A9 File Offset: 0x000CD6A9
	public float CurrentValue
	{
		get
		{
			return this.foundPayload;
		}
	}

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x060048D8 RID: 18648 RVA: 0x000CF4B1 File Offset: 0x000CD6B1
	public float RangeMin
	{
		get
		{
			return this.minPayload;
		}
	}

	// Token: 0x170003AA RID: 938
	// (get) Token: 0x060048D9 RID: 18649 RVA: 0x000CF4B9 File Offset: 0x000CD6B9
	public float RangeMax
	{
		get
		{
			return this.maxPayload;
		}
	}

	// Token: 0x060048DA RID: 18650 RVA: 0x000CF4B1 File Offset: 0x000CD6B1
	public float GetRangeMinInputField()
	{
		return this.minPayload;
	}

	// Token: 0x060048DB RID: 18651 RVA: 0x000CF4B9 File Offset: 0x000CD6B9
	public float GetRangeMaxInputField()
	{
		return this.maxPayload;
	}

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x060048DC RID: 18652 RVA: 0x000CF4C1 File Offset: 0x000CD6C1
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.HEPSWITCHSIDESCREEN.TITLE;
		}
	}

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x060048DD RID: 18653 RVA: 0x000CF4C8 File Offset: 0x000CD6C8
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS;
		}
	}

	// Token: 0x170003AD RID: 941
	// (get) Token: 0x060048DE RID: 18654 RVA: 0x000CF4CF File Offset: 0x000CD6CF
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170003AE RID: 942
	// (get) Token: 0x060048DF RID: 18655 RVA: 0x000CF4DB File Offset: 0x000CD6DB
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.HEPS_TOOLTIP_BELOW;
		}
	}

	// Token: 0x060048E0 RID: 18656 RVA: 0x000CF4E7 File Offset: 0x000CD6E7
	public string Format(float value, bool units)
	{
		return GameUtil.GetFormattedHighEnergyParticles(value, GameUtil.TimeSlice.None, units);
	}

	// Token: 0x060048E1 RID: 18657 RVA: 0x000CA28F File Offset: 0x000C848F
	public float ProcessedSliderValue(float input)
	{
		return Mathf.Round(input);
	}

	// Token: 0x060048E2 RID: 18658 RVA: 0x000B1FA8 File Offset: 0x000B01A8
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x060048E3 RID: 18659 RVA: 0x000CF4F1 File Offset: 0x000CD6F1
	public LocString ThresholdValueUnits()
	{
		return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
	}

	// Token: 0x170003AF RID: 943
	// (get) Token: 0x060048E4 RID: 18660 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x060048E5 RID: 18661 RVA: 0x000A65EC File Offset: 0x000A47EC
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x060048E6 RID: 18662 RVA: 0x0025781C File Offset: 0x00255A1C
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return new NonLinearSlider.Range[]
			{
				new NonLinearSlider.Range(30f, 50f),
				new NonLinearSlider.Range(30f, 200f),
				new NonLinearSlider.Range(40f, 500f)
			};
		}
	}

	// Token: 0x040032CD RID: 13005
	[Serialize]
	public float thresholdPayload;

	// Token: 0x040032CE RID: 13006
	[Serialize]
	public bool activateOnHigherThan;

	// Token: 0x040032CF RID: 13007
	[Serialize]
	public bool dirty = true;

	// Token: 0x040032D0 RID: 13008
	private readonly float minPayload;

	// Token: 0x040032D1 RID: 13009
	private readonly float maxPayload = 500f;

	// Token: 0x040032D2 RID: 13010
	private float foundPayload;

	// Token: 0x040032D3 RID: 13011
	private bool waitForLogicTick;

	// Token: 0x040032D4 RID: 13012
	private bool wasOn;

	// Token: 0x040032D5 RID: 13013
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040032D6 RID: 13014
	private static readonly EventSystem.IntraObjectHandler<LogicHEPSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicHEPSensor>(delegate(LogicHEPSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
