using System;
using STRINGS;
using UnityEngine;

// Token: 0x020013DC RID: 5084
public class HEPFuelTank : KMonoBehaviour, IFuelTank, IUserControlledCapacity
{
	// Token: 0x170006A2 RID: 1698
	// (get) Token: 0x06006836 RID: 26678 RVA: 0x000E43AC File Offset: 0x000E25AC
	public IStorage Storage
	{
		get
		{
			return this.hepStorage;
		}
	}

	// Token: 0x170006A3 RID: 1699
	// (get) Token: 0x06006837 RID: 26679 RVA: 0x000E43B4 File Offset: 0x000E25B4
	public bool ConsumeFuelOnLand
	{
		get
		{
			return this.consumeFuelOnLand;
		}
	}

	// Token: 0x06006838 RID: 26680 RVA: 0x000E43BC File Offset: 0x000E25BC
	public void DEBUG_FillTank()
	{
		this.hepStorage.Store(this.hepStorage.RemainingCapacity());
	}

	// Token: 0x170006A4 RID: 1700
	// (get) Token: 0x06006839 RID: 26681 RVA: 0x000E43D5 File Offset: 0x000E25D5
	// (set) Token: 0x0600683A RID: 26682 RVA: 0x000E43E2 File Offset: 0x000E25E2
	public float UserMaxCapacity
	{
		get
		{
			return this.hepStorage.capacity;
		}
		set
		{
			this.hepStorage.capacity = value;
			base.Trigger(-795826715, this);
		}
	}

	// Token: 0x170006A5 RID: 1701
	// (get) Token: 0x0600683B RID: 26683 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170006A6 RID: 1702
	// (get) Token: 0x0600683C RID: 26684 RVA: 0x000E43FC File Offset: 0x000E25FC
	public float MaxCapacity
	{
		get
		{
			return this.physicalFuelCapacity;
		}
	}

	// Token: 0x170006A7 RID: 1703
	// (get) Token: 0x0600683D RID: 26685 RVA: 0x000E4404 File Offset: 0x000E2604
	public float AmountStored
	{
		get
		{
			return this.hepStorage.Particles;
		}
	}

	// Token: 0x170006A8 RID: 1704
	// (get) Token: 0x0600683E RID: 26686 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170006A9 RID: 1705
	// (get) Token: 0x0600683F RID: 26687 RVA: 0x000CF4F1 File Offset: 0x000CD6F1
	public LocString CapacityUnits
	{
		get
		{
			return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
		}
	}

	// Token: 0x06006840 RID: 26688 RVA: 0x002D68D8 File Offset: 0x002D4AD8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionProperlyFueled(this));
		this.m_meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.m_meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
		this.OnStorageChange(null);
		base.Subscribe<HEPFuelTank>(-795826715, HEPFuelTank.OnStorageChangedDelegate);
		base.Subscribe<HEPFuelTank>(-1837862626, HEPFuelTank.OnStorageChangedDelegate);
	}

	// Token: 0x06006841 RID: 26689 RVA: 0x000E4411 File Offset: 0x000E2611
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HEPFuelTank>(-905833192, HEPFuelTank.OnCopySettingsDelegate);
	}

	// Token: 0x06006842 RID: 26690 RVA: 0x000E442A File Offset: 0x000E262A
	private void OnStorageChange(object data)
	{
		this.m_meter.SetPositionPercent(this.hepStorage.Particles / Mathf.Max(1f, this.hepStorage.capacity));
	}

	// Token: 0x06006843 RID: 26691 RVA: 0x002D6984 File Offset: 0x002D4B84
	private void OnCopySettings(object data)
	{
		HEPFuelTank component = ((GameObject)data).GetComponent<HEPFuelTank>();
		if (component != null)
		{
			this.UserMaxCapacity = component.UserMaxCapacity;
		}
	}

	// Token: 0x04004EA1 RID: 20129
	[MyCmpReq]
	public HighEnergyParticleStorage hepStorage;

	// Token: 0x04004EA2 RID: 20130
	public float physicalFuelCapacity;

	// Token: 0x04004EA3 RID: 20131
	private MeterController m_meter;

	// Token: 0x04004EA4 RID: 20132
	public bool consumeFuelOnLand;

	// Token: 0x04004EA5 RID: 20133
	private static readonly EventSystem.IntraObjectHandler<HEPFuelTank> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<HEPFuelTank>(delegate(HEPFuelTank component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04004EA6 RID: 20134
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04004EA7 RID: 20135
	private static readonly EventSystem.IntraObjectHandler<HEPFuelTank> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<HEPFuelTank>(delegate(HEPFuelTank component, object data)
	{
		component.OnCopySettings(data);
	});
}
