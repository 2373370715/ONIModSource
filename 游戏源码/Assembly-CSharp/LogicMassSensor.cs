using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E56 RID: 3670
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicMassSensor : Switch, ISaveLoadable, IThresholdSwitch
{
	// Token: 0x06004919 RID: 18713 RVA: 0x000CF6FF File Offset: 0x000CD8FF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicMassSensor>(-905833192, LogicMassSensor.OnCopySettingsDelegate);
	}

	// Token: 0x0600491A RID: 18714 RVA: 0x00257F2C File Offset: 0x0025612C
	private void OnCopySettings(object data)
	{
		LogicMassSensor component = ((GameObject)data).GetComponent<LogicMassSensor>();
		if (component != null)
		{
			this.Threshold = component.Threshold;
			this.ActivateAboveThreshold = component.ActivateAboveThreshold;
		}
	}

	// Token: 0x0600491B RID: 18715 RVA: 0x00257F68 File Offset: 0x00256168
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateVisualState(true);
		int cell = Grid.CellAbove(this.NaturalBuildingCell());
		this.solidChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.SolidChanged", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.pickupablesChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.PickupablesChanged", base.gameObject, cell, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.OnPickupablesChanged));
		this.floorSwitchActivatorChangedEntry = GameScenePartitioner.Instance.Add("LogicMassSensor.SwitchActivatorChanged", base.gameObject, cell, GameScenePartitioner.Instance.floorSwitchActivatorChangedLayer, new Action<object>(this.OnActivatorsChanged));
		base.OnToggle += this.SwitchToggled;
	}

	// Token: 0x0600491C RID: 18716 RVA: 0x000CF718 File Offset: 0x000CD918
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.solidChangedEntry);
		GameScenePartitioner.Instance.Free(ref this.pickupablesChangedEntry);
		GameScenePartitioner.Instance.Free(ref this.floorSwitchActivatorChangedEntry);
		base.OnCleanUp();
	}

	// Token: 0x0600491D RID: 18717 RVA: 0x00258038 File Offset: 0x00256238
	private void Update()
	{
		this.toggleCooldown = Mathf.Max(0f, this.toggleCooldown - Time.deltaTime);
		if (this.toggleCooldown == 0f)
		{
			float currentValue = this.CurrentValue;
			if ((this.activateAboveThreshold ? (currentValue > this.threshold) : (currentValue < this.threshold)) != base.IsSwitchedOn)
			{
				this.Toggle();
				this.toggleCooldown = 0.15f;
			}
			this.UpdateVisualState(false);
		}
	}

	// Token: 0x0600491E RID: 18718 RVA: 0x002580B4 File Offset: 0x002562B4
	private void OnSolidChanged(object data)
	{
		int i = Grid.CellAbove(this.NaturalBuildingCell());
		if (Grid.Solid[i])
		{
			this.massSolid = Grid.Mass[i];
			return;
		}
		this.massSolid = 0f;
	}

	// Token: 0x0600491F RID: 18719 RVA: 0x002580F8 File Offset: 0x002562F8
	private void OnPickupablesChanged(object data)
	{
		float num = 0f;
		int cell = Grid.CellAbove(this.NaturalBuildingCell());
		ListPool<ScenePartitionerEntry, LogicMassSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicMassSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			Pickupable pickupable = pooledList[i].obj as Pickupable;
			if (!(pickupable == null) && !pickupable.wasAbsorbed)
			{
				KPrefabID kprefabID = pickupable.KPrefabID;
				if (!kprefabID.HasTag(GameTags.Creature) || (kprefabID.HasTag(GameTags.Creatures.Walker) || kprefabID.HasTag(GameTags.Creatures.Hoverer) || kprefabID.HasTag(GameTags.Creatures.Flopping)))
				{
					num += pickupable.PrimaryElement.Mass;
				}
			}
		}
		pooledList.Recycle();
		this.massPickupables = num;
	}

	// Token: 0x06004920 RID: 18720 RVA: 0x002581E4 File Offset: 0x002563E4
	private void OnActivatorsChanged(object data)
	{
		float num = 0f;
		int cell = Grid.CellAbove(this.NaturalBuildingCell());
		ListPool<ScenePartitionerEntry, LogicMassSensor>.PooledList pooledList = ListPool<ScenePartitionerEntry, LogicMassSensor>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(Grid.CellToXY(cell).x, Grid.CellToXY(cell).y, 1, 1, GameScenePartitioner.Instance.floorSwitchActivatorLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			FloorSwitchActivator floorSwitchActivator = pooledList[i].obj as FloorSwitchActivator;
			if (!(floorSwitchActivator == null))
			{
				num += floorSwitchActivator.PrimaryElement.Mass;
			}
		}
		pooledList.Recycle();
		this.massActivators = num;
	}

	// Token: 0x170003BE RID: 958
	// (get) Token: 0x06004921 RID: 18721 RVA: 0x000CF750 File Offset: 0x000CD950
	public LocString Title
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.TITLE;
		}
	}

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x06004922 RID: 18722 RVA: 0x000CF757 File Offset: 0x000CD957
	// (set) Token: 0x06004923 RID: 18723 RVA: 0x000CF75F File Offset: 0x000CD95F
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

	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x06004924 RID: 18724 RVA: 0x000CF768 File Offset: 0x000CD968
	// (set) Token: 0x06004925 RID: 18725 RVA: 0x000CF770 File Offset: 0x000CD970
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

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x06004926 RID: 18726 RVA: 0x000CF779 File Offset: 0x000CD979
	public float CurrentValue
	{
		get
		{
			return this.massSolid + this.massPickupables + this.massActivators;
		}
	}

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x06004927 RID: 18727 RVA: 0x000CF78F File Offset: 0x000CD98F
	public float RangeMin
	{
		get
		{
			return this.rangeMin;
		}
	}

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x06004928 RID: 18728 RVA: 0x000CF797 File Offset: 0x000CD997
	public float RangeMax
	{
		get
		{
			return this.rangeMax;
		}
	}

	// Token: 0x06004929 RID: 18729 RVA: 0x000CF78F File Offset: 0x000CD98F
	public float GetRangeMinInputField()
	{
		return this.rangeMin;
	}

	// Token: 0x0600492A RID: 18730 RVA: 0x000CF797 File Offset: 0x000CD997
	public float GetRangeMaxInputField()
	{
		return this.rangeMax;
	}

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x0600492B RID: 18731 RVA: 0x000CF79F File Offset: 0x000CD99F
	public LocString ThresholdValueName
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE;
		}
	}

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x0600492C RID: 18732 RVA: 0x000CF7A6 File Offset: 0x000CD9A6
	public string AboveToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_ABOVE;
		}
	}

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x0600492D RID: 18733 RVA: 0x000CF7B2 File Offset: 0x000CD9B2
	public string BelowToolTip
	{
		get
		{
			return UI.UISIDESCREENS.THRESHOLD_SWITCH_SIDESCREEN.PRESSURE_TOOLTIP_BELOW;
		}
	}

	// Token: 0x0600492E RID: 18734 RVA: 0x00258280 File Offset: 0x00256480
	public string Format(float value, bool units)
	{
		GameUtil.MetricMassFormat massFormat = GameUtil.MetricMassFormat.Kilogram;
		return GameUtil.GetFormattedMass(value, GameUtil.TimeSlice.None, massFormat, units, "{0:0.#}");
	}

	// Token: 0x0600492F RID: 18735 RVA: 0x000CF7BE File Offset: 0x000CD9BE
	public float ProcessedSliderValue(float input)
	{
		input = Mathf.Round(input);
		return input;
	}

	// Token: 0x06004930 RID: 18736 RVA: 0x000B1FA8 File Offset: 0x000B01A8
	public float ProcessedInputValue(float input)
	{
		return input;
	}

	// Token: 0x06004931 RID: 18737 RVA: 0x000C8D02 File Offset: 0x000C6F02
	public LocString ThresholdValueUnits()
	{
		return GameUtil.GetCurrentMassUnit(false);
	}

	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x06004932 RID: 18738 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public ThresholdScreenLayoutType LayoutType
	{
		get
		{
			return ThresholdScreenLayoutType.SliderBar;
		}
	}

	// Token: 0x170003C8 RID: 968
	// (get) Token: 0x06004933 RID: 18739 RVA: 0x000A65EC File Offset: 0x000A47EC
	public int IncrementScale
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x170003C9 RID: 969
	// (get) Token: 0x06004934 RID: 18740 RVA: 0x000CF7C9 File Offset: 0x000CD9C9
	public NonLinearSlider.Range[] GetRanges
	{
		get
		{
			return NonLinearSlider.GetDefaultRange(this.RangeMax);
		}
	}

	// Token: 0x06004935 RID: 18741 RVA: 0x000CF7D6 File Offset: 0x000CD9D6
	private void SwitchToggled(bool toggled_on)
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, toggled_on ? 1 : 0);
	}

	// Token: 0x06004936 RID: 18742 RVA: 0x002582A0 File Offset: 0x002564A0
	private void UpdateVisualState(bool force = false)
	{
		bool flag = this.CurrentValue > this.threshold;
		if (flag != this.was_pressed || this.was_on != base.IsSwitchedOn || force)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			if (flag)
			{
				if (force)
				{
					component.Play(base.IsSwitchedOn ? "on_down" : "off_down", KAnim.PlayMode.Once, 1f, 0f);
				}
				else
				{
					component.Play(base.IsSwitchedOn ? "on_down_pre" : "off_down_pre", KAnim.PlayMode.Once, 1f, 0f);
					component.Queue(base.IsSwitchedOn ? "on_down" : "off_down", KAnim.PlayMode.Once, 1f, 0f);
				}
			}
			else if (force)
			{
				component.Play(base.IsSwitchedOn ? "on_up" : "off_up", KAnim.PlayMode.Once, 1f, 0f);
			}
			else
			{
				component.Play(base.IsSwitchedOn ? "on_up_pre" : "off_up_pre", KAnim.PlayMode.Once, 1f, 0f);
				component.Queue(base.IsSwitchedOn ? "on_up" : "off_up", KAnim.PlayMode.Once, 1f, 0f);
			}
			this.was_pressed = flag;
			this.was_on = base.IsSwitchedOn;
		}
	}

	// Token: 0x06004937 RID: 18743 RVA: 0x00253D94 File Offset: 0x00251F94
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x040032F5 RID: 13045
	[SerializeField]
	[Serialize]
	private float threshold;

	// Token: 0x040032F6 RID: 13046
	[SerializeField]
	[Serialize]
	private bool activateAboveThreshold = true;

	// Token: 0x040032F7 RID: 13047
	[MyCmpGet]
	private LogicPorts logicPorts;

	// Token: 0x040032F8 RID: 13048
	private bool was_pressed;

	// Token: 0x040032F9 RID: 13049
	private bool was_on;

	// Token: 0x040032FA RID: 13050
	public float rangeMin;

	// Token: 0x040032FB RID: 13051
	public float rangeMax = 1f;

	// Token: 0x040032FC RID: 13052
	[Serialize]
	private float massSolid;

	// Token: 0x040032FD RID: 13053
	[Serialize]
	private float massPickupables;

	// Token: 0x040032FE RID: 13054
	[Serialize]
	private float massActivators;

	// Token: 0x040032FF RID: 13055
	private const float MIN_TOGGLE_TIME = 0.15f;

	// Token: 0x04003300 RID: 13056
	private float toggleCooldown = 0.15f;

	// Token: 0x04003301 RID: 13057
	private HandleVector<int>.Handle solidChangedEntry;

	// Token: 0x04003302 RID: 13058
	private HandleVector<int>.Handle pickupablesChangedEntry;

	// Token: 0x04003303 RID: 13059
	private HandleVector<int>.Handle floorSwitchActivatorChangedEntry;

	// Token: 0x04003304 RID: 13060
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003305 RID: 13061
	private static readonly EventSystem.IntraObjectHandler<LogicMassSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicMassSensor>(delegate(LogicMassSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
