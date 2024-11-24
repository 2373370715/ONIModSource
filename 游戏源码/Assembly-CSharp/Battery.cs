using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000CA1 RID: 3233
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name}")]
[AddComponentMenu("KMonoBehaviour/scripts/Battery")]
public class Battery : KMonoBehaviour, IEnergyConsumer, ICircuitConnected, IGameObjectEffectDescriptor, IEnergyProducer
{
	// Token: 0x170002DC RID: 732
	// (get) Token: 0x06003E4B RID: 15947 RVA: 0x000C881C File Offset: 0x000C6A1C
	// (set) Token: 0x06003E4C RID: 15948 RVA: 0x000C8824 File Offset: 0x000C6A24
	public float WattsUsed { get; private set; }

	// Token: 0x170002DD RID: 733
	// (get) Token: 0x06003E4D RID: 15949 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float WattsNeededWhenActive
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002DE RID: 734
	// (get) Token: 0x06003E4E RID: 15950 RVA: 0x000C882D File Offset: 0x000C6A2D
	public float PercentFull
	{
		get
		{
			return this.joulesAvailable / this.capacity;
		}
	}

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x06003E4F RID: 15951 RVA: 0x000C883C File Offset: 0x000C6A3C
	public float PreviousPercentFull
	{
		get
		{
			return this.PreviousJoulesAvailable / this.capacity;
		}
	}

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x06003E50 RID: 15952 RVA: 0x000C884B File Offset: 0x000C6A4B
	public float JoulesAvailable
	{
		get
		{
			return this.joulesAvailable;
		}
	}

	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x06003E51 RID: 15953 RVA: 0x000C8853 File Offset: 0x000C6A53
	public float Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x06003E52 RID: 15954 RVA: 0x000C885B File Offset: 0x000C6A5B
	// (set) Token: 0x06003E53 RID: 15955 RVA: 0x000C8863 File Offset: 0x000C6A63
	public float ChargeCapacity { get; private set; }

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x06003E54 RID: 15956 RVA: 0x000C886C File Offset: 0x000C6A6C
	public int PowerSortOrder
	{
		get
		{
			return this.powerSortOrder;
		}
	}

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x06003E55 RID: 15957 RVA: 0x000C8874 File Offset: 0x000C6A74
	public string Name
	{
		get
		{
			return base.GetComponent<KSelectable>().GetName();
		}
	}

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x06003E56 RID: 15958 RVA: 0x000C8881 File Offset: 0x000C6A81
	// (set) Token: 0x06003E57 RID: 15959 RVA: 0x000C8889 File Offset: 0x000C6A89
	public int PowerCell { get; private set; }

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x06003E58 RID: 15960 RVA: 0x000C8892 File Offset: 0x000C6A92
	public ushort CircuitID
	{
		get
		{
			return Game.Instance.circuitManager.GetCircuitID(this);
		}
	}

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x06003E59 RID: 15961 RVA: 0x000C88A4 File Offset: 0x000C6AA4
	public bool IsConnected
	{
		get
		{
			return this.connectionStatus > CircuitManager.ConnectionStatus.NotConnected;
		}
	}

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x06003E5A RID: 15962 RVA: 0x000C88AF File Offset: 0x000C6AAF
	public bool IsPowered
	{
		get
		{
			return this.connectionStatus == CircuitManager.ConnectionStatus.Powered;
		}
	}

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x06003E5B RID: 15963 RVA: 0x000C88BA File Offset: 0x000C6ABA
	// (set) Token: 0x06003E5C RID: 15964 RVA: 0x000C88C2 File Offset: 0x000C6AC2
	public bool IsVirtual { get; protected set; }

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x06003E5D RID: 15965 RVA: 0x000C88CB File Offset: 0x000C6ACB
	// (set) Token: 0x06003E5E RID: 15966 RVA: 0x000C88D3 File Offset: 0x000C6AD3
	public object VirtualCircuitKey { get; protected set; }

	// Token: 0x06003E5F RID: 15967 RVA: 0x002340E8 File Offset: 0x002322E8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Batteries.Add(this);
		Building component = base.GetComponent<Building>();
		this.PowerCell = component.GetPowerInputCell();
		base.Subscribe<Battery>(-1582839653, Battery.OnTagsChangedDelegate);
		this.OnTagsChanged(null);
		this.meter = (base.GetComponent<PowerTransformer>() ? null : new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		}));
		Game.Instance.circuitManager.Connect(this);
		Game.Instance.energySim.AddBattery(this);
	}

	// Token: 0x06003E60 RID: 15968 RVA: 0x002341A8 File Offset: 0x002323A8
	private void OnTagsChanged(object data)
	{
		if (this.HasAllTags(this.connectedTags))
		{
			Game.Instance.circuitManager.Connect(this);
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.BatteryJoulesAvailable, this);
			return;
		}
		Game.Instance.circuitManager.Disconnect(this, false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.BatteryJoulesAvailable, false);
	}

	// Token: 0x06003E61 RID: 15969 RVA: 0x000C88DC File Offset: 0x000C6ADC
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveBattery(this);
		Game.Instance.circuitManager.Disconnect(this, true);
		Components.Batteries.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06003E62 RID: 15970 RVA: 0x0023422C File Offset: 0x0023242C
	public virtual void EnergySim200ms(float dt)
	{
		this.dt = dt;
		this.joulesConsumed = 0f;
		this.WattsUsed = 0f;
		this.ChargeCapacity = this.chargeWattage * dt;
		if (this.meter != null)
		{
			float percentFull = this.PercentFull;
			this.meter.SetPositionPercent(percentFull);
		}
		this.UpdateSounds();
		this.PreviousJoulesAvailable = this.JoulesAvailable;
		this.ConsumeEnergy(this.joulesLostPerSecond * dt, true);
	}

	// Token: 0x06003E63 RID: 15971 RVA: 0x002342A0 File Offset: 0x002324A0
	private void UpdateSounds()
	{
		float previousPercentFull = this.PreviousPercentFull;
		float percentFull = this.PercentFull;
		if (percentFull == 0f && previousPercentFull != 0f)
		{
			base.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryDischarged);
		}
		if (percentFull > 0.999f && previousPercentFull <= 0.999f)
		{
			base.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryFull);
		}
		if (percentFull < 0.25f && previousPercentFull >= 0.25f)
		{
			base.GetComponent<LoopingSounds>().PlayEvent(GameSoundEvents.BatteryWarning);
		}
	}

	// Token: 0x06003E64 RID: 15972 RVA: 0x0023431C File Offset: 0x0023251C
	public void SetConnectionStatus(CircuitManager.ConnectionStatus status)
	{
		this.connectionStatus = status;
		if (status == CircuitManager.ConnectionStatus.NotConnected)
		{
			this.operational.SetActive(false, false);
			return;
		}
		this.operational.SetActive(this.operational.IsOperational && this.JoulesAvailable > 0f, false);
	}

	// Token: 0x06003E65 RID: 15973 RVA: 0x0023436C File Offset: 0x0023256C
	public void AddEnergy(float joules)
	{
		this.joulesAvailable = Mathf.Min(this.capacity, this.JoulesAvailable + joules);
		this.joulesConsumed += joules;
		this.ChargeCapacity -= joules;
		this.WattsUsed = this.joulesConsumed / this.dt;
	}

	// Token: 0x06003E66 RID: 15974 RVA: 0x002343C4 File Offset: 0x002325C4
	public void ConsumeEnergy(float joules, bool report = false)
	{
		if (report)
		{
			float num = Mathf.Min(this.JoulesAvailable, joules);
			ReportManager.Instance.ReportValue(ReportManager.ReportType.EnergyWasted, -num, StringFormatter.Replace(BUILDINGS.PREFABS.BATTERY.CHARGE_LOSS, "{Battery}", this.GetProperName()), null);
		}
		this.joulesAvailable = Mathf.Max(0f, this.JoulesAvailable - joules);
	}

	// Token: 0x06003E67 RID: 15975 RVA: 0x000C8910 File Offset: 0x000C6B10
	public void ConsumeEnergy(float joules)
	{
		this.ConsumeEnergy(joules, false);
	}

	// Token: 0x06003E68 RID: 15976 RVA: 0x00234424 File Offset: 0x00232624
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.powerTransformer == null)
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.REQUIRESPOWERGENERATOR, UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESPOWERGENERATOR, Descriptor.DescriptorType.Requirement, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.BATTERYCAPACITY, GameUtil.GetFormattedJoules(this.capacity, "", GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.BATTERYCAPACITY, GameUtil.GetFormattedJoules(this.capacity, "", GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.BATTERYLEAK, GameUtil.GetFormattedJoules(this.joulesLostPerSecond, "F1", GameUtil.TimeSlice.PerCycle)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.BATTERYLEAK, GameUtil.GetFormattedJoules(this.joulesLostPerSecond, "F1", GameUtil.TimeSlice.PerCycle)), Descriptor.DescriptorType.Effect, false));
		}
		else
		{
			list.Add(new Descriptor(UI.BUILDINGEFFECTS.TRANSFORMER_INPUT_WIRE, UI.BUILDINGEFFECTS.TOOLTIPS.TRANSFORMER_INPUT_WIRE, Descriptor.DescriptorType.Requirement, false));
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.TRANSFORMER_OUTPUT_WIRE, GameUtil.GetFormattedWattage(this.capacity, GameUtil.WattageFormatterUnit.Automatic, true)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.TRANSFORMER_OUTPUT_WIRE, GameUtil.GetFormattedWattage(this.capacity, GameUtil.WattageFormatterUnit.Automatic, true)), Descriptor.DescriptorType.Requirement, false));
		}
		return list;
	}

	// Token: 0x06003E69 RID: 15977 RVA: 0x000C891A File Offset: 0x000C6B1A
	[ContextMenu("Refill Power")]
	public void DEBUG_RefillPower()
	{
		this.joulesAvailable = this.capacity;
	}

	// Token: 0x04002A94 RID: 10900
	[SerializeField]
	public float capacity;

	// Token: 0x04002A95 RID: 10901
	[SerializeField]
	public float chargeWattage = float.PositiveInfinity;

	// Token: 0x04002A96 RID: 10902
	[Serialize]
	private float joulesAvailable;

	// Token: 0x04002A97 RID: 10903
	[MyCmpGet]
	protected Operational operational;

	// Token: 0x04002A98 RID: 10904
	[MyCmpGet]
	public PowerTransformer powerTransformer;

	// Token: 0x04002A99 RID: 10905
	protected MeterController meter;

	// Token: 0x04002A9B RID: 10907
	public float joulesLostPerSecond;

	// Token: 0x04002A9D RID: 10909
	[SerializeField]
	public int powerSortOrder;

	// Token: 0x04002AA1 RID: 10913
	private float PreviousJoulesAvailable;

	// Token: 0x04002AA2 RID: 10914
	private CircuitManager.ConnectionStatus connectionStatus;

	// Token: 0x04002AA3 RID: 10915
	public static readonly Tag[] DEFAULT_CONNECTED_TAGS = new Tag[]
	{
		GameTags.Operational
	};

	// Token: 0x04002AA4 RID: 10916
	[SerializeField]
	public Tag[] connectedTags = Battery.DEFAULT_CONNECTED_TAGS;

	// Token: 0x04002AA5 RID: 10917
	private static readonly EventSystem.IntraObjectHandler<Battery> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Battery>(delegate(Battery component, object data)
	{
		component.OnTagsChanged(data);
	});

	// Token: 0x04002AA6 RID: 10918
	private float dt;

	// Token: 0x04002AA7 RID: 10919
	private float joulesConsumed;
}
