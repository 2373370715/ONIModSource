using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200127F RID: 4735
[SkipSaveFileSerialization]
[SerializationConfig(MemberSerialization.OptIn)]
public class ElementConsumer : SimComponent, ISaveLoadable, IGameObjectEffectDescriptor
{
	// Token: 0x1400001E RID: 30
	// (add) Token: 0x06006113 RID: 24851 RVA: 0x002B1868 File Offset: 0x002AFA68
	// (remove) Token: 0x06006114 RID: 24852 RVA: 0x002B18A0 File Offset: 0x002AFAA0
	public event Action<Sim.ConsumedMassInfo> OnElementConsumed;

	// Token: 0x170005F4 RID: 1524
	// (get) Token: 0x06006115 RID: 24853 RVA: 0x000DF58F File Offset: 0x000DD78F
	public float AverageConsumeRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.accumulator);
		}
	}

	// Token: 0x06006116 RID: 24854 RVA: 0x000DF5A6 File Offset: 0x000DD7A6
	public static void ClearInstanceMap()
	{
		ElementConsumer.handleInstanceMap.Clear();
	}

	// Token: 0x06006117 RID: 24855 RVA: 0x002B18D8 File Offset: 0x002AFAD8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.accumulator = Game.Instance.accumulators.Add("Element", this);
		if (this.elementToConsume == SimHashes.Void)
		{
			throw new ArgumentException("No consumable elements specified");
		}
		if (!this.ignoreActiveChanged)
		{
			base.Subscribe<ElementConsumer>(824508782, ElementConsumer.OnActiveChangedDelegate);
		}
		if (this.capacityKG != float.PositiveInfinity)
		{
			this.hasAvailableCapacity = !this.IsStorageFull();
			base.Subscribe<ElementConsumer>(-1697596308, ElementConsumer.OnStorageChangeDelegate);
		}
	}

	// Token: 0x06006118 RID: 24856 RVA: 0x000DF5B2 File Offset: 0x000DD7B2
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.accumulator);
		base.OnCleanUp();
	}

	// Token: 0x06006119 RID: 24857 RVA: 0x000DF5D0 File Offset: 0x000DD7D0
	protected virtual bool IsActive()
	{
		return this.operational == null || this.operational.IsActive;
	}

	// Token: 0x0600611A RID: 24858 RVA: 0x002B1964 File Offset: 0x002AFB64
	public void EnableConsumption(bool enabled)
	{
		bool flag = this.consumptionEnabled;
		this.consumptionEnabled = enabled;
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		if (enabled != flag)
		{
			this.UpdateSimData();
		}
	}

	// Token: 0x0600611B RID: 24859 RVA: 0x002B1998 File Offset: 0x002AFB98
	private bool IsStorageFull()
	{
		PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.elementToConsume);
		return primaryElement != null && primaryElement.Mass >= this.capacityKG;
	}

	// Token: 0x0600611C RID: 24860 RVA: 0x000DF5ED File Offset: 0x000DD7ED
	public void RefreshConsumptionRate()
	{
		if (!Sim.IsValidHandle(this.simHandle))
		{
			return;
		}
		this.UpdateSimData();
	}

	// Token: 0x0600611D RID: 24861 RVA: 0x002B19D4 File Offset: 0x002AFBD4
	private void UpdateSimData()
	{
		global::Debug.Assert(Sim.IsValidHandle(this.simHandle));
		int sampleCell = this.GetSampleCell();
		float num = (this.consumptionEnabled && this.hasAvailableCapacity) ? this.consumptionRate : 0f;
		SimMessages.SetElementConsumerData(this.simHandle, sampleCell, num);
		this.UpdateStatusItem();
	}

	// Token: 0x0600611E RID: 24862 RVA: 0x002B1A2C File Offset: 0x002AFC2C
	public static void AddMass(Sim.ConsumedMassInfo consumed_info)
	{
		if (!Sim.IsValidHandle(consumed_info.simHandle))
		{
			return;
		}
		ElementConsumer elementConsumer;
		if (ElementConsumer.handleInstanceMap.TryGetValue(consumed_info.simHandle, out elementConsumer))
		{
			elementConsumer.AddMassInternal(consumed_info);
		}
	}

	// Token: 0x0600611F RID: 24863 RVA: 0x000DF603 File Offset: 0x000DD803
	private int GetSampleCell()
	{
		return Grid.PosToCell(base.transform.GetPosition() + this.sampleCellOffset);
	}

	// Token: 0x06006120 RID: 24864 RVA: 0x002B1A64 File Offset: 0x002AFC64
	private void AddMassInternal(Sim.ConsumedMassInfo consumed_info)
	{
		if (consumed_info.mass > 0f)
		{
			if (this.storeOnConsume)
			{
				Element element = ElementLoader.elements[(int)consumed_info.removedElemIdx];
				if (this.elementToConsume == SimHashes.Vacuum || this.elementToConsume == element.id)
				{
					if (element.IsLiquid)
					{
						this.storage.AddLiquid(element.id, consumed_info.mass, consumed_info.temperature, consumed_info.diseaseIdx, consumed_info.diseaseCount, true, true);
					}
					else if (element.IsGas)
					{
						this.storage.AddGasChunk(element.id, consumed_info.mass, consumed_info.temperature, consumed_info.diseaseIdx, consumed_info.diseaseCount, true, true);
					}
				}
			}
			else
			{
				this.consumedTemperature = GameUtil.GetFinalTemperature(consumed_info.temperature, consumed_info.mass, this.consumedTemperature, this.consumedMass);
				this.consumedMass += consumed_info.mass;
				if (this.OnElementConsumed != null)
				{
					this.OnElementConsumed(consumed_info);
				}
			}
		}
		Game.Instance.accumulators.Accumulate(this.accumulator, consumed_info.mass);
	}

	// Token: 0x170005F5 RID: 1525
	// (get) Token: 0x06006121 RID: 24865 RVA: 0x002B1B90 File Offset: 0x002AFD90
	public bool IsElementAvailable
	{
		get
		{
			int sampleCell = this.GetSampleCell();
			SimHashes id = Grid.Element[sampleCell].id;
			return this.elementToConsume == id && Grid.Mass[sampleCell] >= this.minimumMass;
		}
	}

	// Token: 0x06006122 RID: 24866 RVA: 0x002B1BD4 File Offset: 0x002AFDD4
	private void UpdateStatusItem()
	{
		if (this.showInStatusPanel)
		{
			if (this.statusHandle == Guid.Empty && this.IsActive() && this.consumptionEnabled)
			{
				this.statusHandle = this.selectable.AddStatusItem(Db.Get().BuildingStatusItems.ElementConsumer, this);
				return;
			}
			if (this.statusHandle != Guid.Empty && !this.consumptionEnabled)
			{
				base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
				this.statusHandle = Guid.Empty;
				return;
			}
		}
		else if (this.statusHandle != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
			this.statusHandle = Guid.Empty;
		}
	}

	// Token: 0x06006123 RID: 24867 RVA: 0x002B1C98 File Offset: 0x002AFE98
	private void OnStorageChange(object data)
	{
		bool flag = !this.IsStorageFull();
		if (flag != this.hasAvailableCapacity)
		{
			this.hasAvailableCapacity = flag;
			this.RefreshConsumptionRate();
		}
	}

	// Token: 0x06006124 RID: 24868 RVA: 0x000DF620 File Offset: 0x000DD820
	protected override void OnCmpEnable()
	{
		if (!base.isSpawned)
		{
			return;
		}
		if (!this.IsActive())
		{
			return;
		}
		this.UpdateStatusItem();
	}

	// Token: 0x06006125 RID: 24869 RVA: 0x000DF63A File Offset: 0x000DD83A
	protected override void OnCmpDisable()
	{
		this.UpdateStatusItem();
	}

	// Token: 0x06006126 RID: 24870 RVA: 0x002B1CC8 File Offset: 0x002AFEC8
	public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.isRequired && this.showDescriptor)
		{
			Element element = ElementLoader.FindElementByHash(this.elementToConsume);
			string arg = element.tag.ProperName();
			if (element.IsVacuum)
			{
				if (this.configuration == ElementConsumer.Configuration.AllGas)
				{
					arg = ELEMENTS.STATE.GAS;
				}
				else if (this.configuration == ElementConsumer.Configuration.AllLiquid)
				{
					arg = ELEMENTS.STATE.LIQUID;
				}
				else
				{
					arg = UI.BUILDINGEFFECTS.CONSUMESANYELEMENT;
				}
			}
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.REQUIRESELEMENT, arg), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REQUIRESELEMENT, arg), Descriptor.DescriptorType.Requirement);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06006127 RID: 24871 RVA: 0x002B1D80 File Offset: 0x002AFF80
	public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.showDescriptor)
		{
			Element element = ElementLoader.FindElementByHash(this.elementToConsume);
			string arg = element.tag.ProperName();
			if (element.IsVacuum)
			{
				if (this.configuration == ElementConsumer.Configuration.AllGas)
				{
					arg = ELEMENTS.STATE.GAS;
				}
				else if (this.configuration == ElementConsumer.Configuration.AllLiquid)
				{
					arg = ELEMENTS.STATE.LIQUID;
				}
				else
				{
					arg = UI.BUILDINGEFFECTS.CONSUMESANYELEMENT;
				}
			}
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.consumptionRate / 100f * 100f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMED, arg, GameUtil.GetFormattedMass(this.consumptionRate / 100f * 100f, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06006128 RID: 24872 RVA: 0x002B1E6C File Offset: 0x002B006C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		foreach (Descriptor item in this.RequirementDescriptors())
		{
			list.Add(item);
		}
		foreach (Descriptor item2 in this.EffectDescriptors())
		{
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x06006129 RID: 24873 RVA: 0x002B1F08 File Offset: 0x002B0108
	private void OnActiveChanged(object data)
	{
		bool isActive = this.operational.IsActive;
		this.EnableConsumption(isActive);
	}

	// Token: 0x0600612A RID: 24874 RVA: 0x000DF642 File Offset: 0x000DD842
	protected override void OnSimUnregister()
	{
		global::Debug.Assert(Sim.IsValidHandle(this.simHandle));
		ElementConsumer.handleInstanceMap.Remove(this.simHandle);
		ElementConsumer.StaticUnregister(this.simHandle);
	}

	// Token: 0x0600612B RID: 24875 RVA: 0x000DF670 File Offset: 0x000DD870
	protected override void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
		SimMessages.AddElementConsumer(this.GetSampleCell(), this.configuration, this.elementToConsume, this.consumptionRadius, cb_handle.index);
	}

	// Token: 0x0600612C RID: 24876 RVA: 0x000DF696 File Offset: 0x000DD896
	protected override Action<int> GetStaticUnregister()
	{
		return new Action<int>(ElementConsumer.StaticUnregister);
	}

	// Token: 0x0600612D RID: 24877 RVA: 0x000DF6A4 File Offset: 0x000DD8A4
	private static void StaticUnregister(int sim_handle)
	{
		global::Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveElementConsumer(-1, sim_handle);
	}

	// Token: 0x0600612E RID: 24878 RVA: 0x000DF6B8 File Offset: 0x000DD8B8
	protected override void OnSimRegistered()
	{
		if (this.consumptionEnabled)
		{
			this.UpdateSimData();
		}
		ElementConsumer.handleInstanceMap[this.simHandle] = this;
	}

	// Token: 0x04004519 RID: 17689
	[HashedEnum]
	[SerializeField]
	public SimHashes elementToConsume = SimHashes.Vacuum;

	// Token: 0x0400451A RID: 17690
	[SerializeField]
	public float consumptionRate;

	// Token: 0x0400451B RID: 17691
	[SerializeField]
	public byte consumptionRadius = 1;

	// Token: 0x0400451C RID: 17692
	[SerializeField]
	public float minimumMass;

	// Token: 0x0400451D RID: 17693
	[SerializeField]
	public bool showInStatusPanel = true;

	// Token: 0x0400451E RID: 17694
	[SerializeField]
	public Vector3 sampleCellOffset;

	// Token: 0x0400451F RID: 17695
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x04004520 RID: 17696
	[SerializeField]
	public ElementConsumer.Configuration configuration;

	// Token: 0x04004521 RID: 17697
	[Serialize]
	[NonSerialized]
	public float consumedMass;

	// Token: 0x04004522 RID: 17698
	[Serialize]
	[NonSerialized]
	public float consumedTemperature;

	// Token: 0x04004523 RID: 17699
	[SerializeField]
	public bool storeOnConsume;

	// Token: 0x04004524 RID: 17700
	[MyCmpGet]
	public Storage storage;

	// Token: 0x04004525 RID: 17701
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04004526 RID: 17702
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04004528 RID: 17704
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04004529 RID: 17705
	public bool ignoreActiveChanged;

	// Token: 0x0400452A RID: 17706
	private Guid statusHandle;

	// Token: 0x0400452B RID: 17707
	public bool showDescriptor = true;

	// Token: 0x0400452C RID: 17708
	public bool isRequired = true;

	// Token: 0x0400452D RID: 17709
	private bool consumptionEnabled;

	// Token: 0x0400452E RID: 17710
	private bool hasAvailableCapacity = true;

	// Token: 0x0400452F RID: 17711
	private static Dictionary<int, ElementConsumer> handleInstanceMap = new Dictionary<int, ElementConsumer>();

	// Token: 0x04004530 RID: 17712
	private static readonly EventSystem.IntraObjectHandler<ElementConsumer> OnActiveChangedDelegate = new EventSystem.IntraObjectHandler<ElementConsumer>(delegate(ElementConsumer component, object data)
	{
		component.OnActiveChanged(data);
	});

	// Token: 0x04004531 RID: 17713
	private static readonly EventSystem.IntraObjectHandler<ElementConsumer> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<ElementConsumer>(delegate(ElementConsumer component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001280 RID: 4736
	public enum Configuration
	{
		// Token: 0x04004533 RID: 17715
		Element,
		// Token: 0x04004534 RID: 17716
		AllLiquid,
		// Token: 0x04004535 RID: 17717
		AllGas
	}
}
