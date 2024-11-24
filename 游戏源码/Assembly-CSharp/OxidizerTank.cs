using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001913 RID: 6419
[AddComponentMenu("KMonoBehaviour/scripts/OxidizerTank")]
public class OxidizerTank : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x170008CD RID: 2253
	// (get) Token: 0x060085A9 RID: 34217 RVA: 0x000F7A36 File Offset: 0x000F5C36
	public bool IsSuspended
	{
		get
		{
			return this.isSuspended;
		}
	}

	// Token: 0x170008CE RID: 2254
	// (get) Token: 0x060085AA RID: 34218 RVA: 0x000F7A3E File Offset: 0x000F5C3E
	// (set) Token: 0x060085AB RID: 34219 RVA: 0x00349498 File Offset: 0x00347698
	public float UserMaxCapacity
	{
		get
		{
			return this.targetFillMass;
		}
		set
		{
			this.targetFillMass = value;
			this.storage.capacityKg = this.targetFillMass;
			ConduitConsumer component = base.GetComponent<ConduitConsumer>();
			if (component != null)
			{
				component.capacityKG = this.targetFillMass;
			}
			base.Trigger(-945020481, this);
			this.OnStorageCapacityChanged(this.targetFillMass);
			if (this.filteredStorage != null)
			{
				this.filteredStorage.FilterChanged();
			}
		}
	}

	// Token: 0x170008CF RID: 2255
	// (get) Token: 0x060085AC RID: 34220 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170008D0 RID: 2256
	// (get) Token: 0x060085AD RID: 34221 RVA: 0x000F7A46 File Offset: 0x000F5C46
	public float MaxCapacity
	{
		get
		{
			return this.maxFillMass;
		}
	}

	// Token: 0x170008D1 RID: 2257
	// (get) Token: 0x060085AE RID: 34222 RVA: 0x000F7A4E File Offset: 0x000F5C4E
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x170008D2 RID: 2258
	// (get) Token: 0x060085AF RID: 34223 RVA: 0x00349504 File Offset: 0x00347704
	public float TotalOxidizerPower
	{
		get
		{
			float num = 0f;
			foreach (GameObject gameObject in this.storage.items)
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				float num2;
				if (DlcManager.FeatureClusterSpaceEnabled())
				{
					num2 = Clustercraft.dlc1OxidizerEfficiencies[component.ElementID.CreateTag()];
				}
				else
				{
					num2 = RocketStats.oxidizerEfficiencies[component.ElementID.CreateTag()];
				}
				num += component.Mass * num2;
			}
			return num;
		}
	}

	// Token: 0x170008D3 RID: 2259
	// (get) Token: 0x060085B0 RID: 34224 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170008D4 RID: 2260
	// (get) Token: 0x060085B1 RID: 34225 RVA: 0x000C8D02 File Offset: 0x000C6F02
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x060085B2 RID: 34226 RVA: 0x003495A8 File Offset: 0x003477A8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<OxidizerTank>(-905833192, OxidizerTank.OnCopySettingsDelegate);
		if (this.supportsMultipleOxidizers)
		{
			this.filteredStorage = new FilteredStorage(this, null, this, true, Db.Get().ChoreTypes.Fetch);
			this.filteredStorage.FilterChanged();
			KBatchedAnimTracker componentInChildren = base.gameObject.GetComponentInChildren<KBatchedAnimTracker>();
			componentInChildren.forceAlwaysAlive = true;
			componentInChildren.matchParentOffset = true;
		}
	}

	// Token: 0x060085B3 RID: 34227 RVA: 0x00349618 File Offset: 0x00347818
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.discoverResourcesOnSpawn != null)
		{
			foreach (SimHashes hash in this.discoverResourcesOnSpawn)
			{
				Element element = ElementLoader.FindElementByHash(hash);
				DiscoveredResources.Instance.Discover(element.tag, element.GetMaterialCategoryTag());
			}
		}
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			global::Debug.Assert(DlcManager.IsExpansion1Active(), "EXP1 not active but trying to use EXP1 rockety system");
			component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionSufficientOxidizer(this));
		}
		this.UserMaxCapacity = Mathf.Min(this.UserMaxCapacity, this.maxFillMass);
		base.Subscribe<OxidizerTank>(-887025858, OxidizerTank.OnRocketLandedDelegate);
		base.Subscribe<OxidizerTank>(-1697596308, OxidizerTank.OnStorageChangeDelegate);
	}

	// Token: 0x060085B4 RID: 34228 RVA: 0x00349714 File Offset: 0x00347914
	public float GetTotalOxidizerAvailable()
	{
		float num = 0f;
		foreach (Tag tag in this.oxidizerTypes)
		{
			num += this.storage.GetAmountAvailable(tag);
		}
		return num;
	}

	// Token: 0x060085B5 RID: 34229 RVA: 0x00349754 File Offset: 0x00347954
	public Dictionary<Tag, float> GetOxidizersAvailable()
	{
		Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
		foreach (Tag tag in this.oxidizerTypes)
		{
			dictionary[tag] = this.storage.GetAmountAvailable(tag);
		}
		return dictionary;
	}

	// Token: 0x060085B6 RID: 34230 RVA: 0x000F7A5B File Offset: 0x000F5C5B
	private void OnStorageChange(object data)
	{
		this.RefreshMeter();
	}

	// Token: 0x060085B7 RID: 34231 RVA: 0x000F7A5B File Offset: 0x000F5C5B
	private void OnStorageCapacityChanged(float newCapacity)
	{
		this.RefreshMeter();
	}

	// Token: 0x060085B8 RID: 34232 RVA: 0x000F7A63 File Offset: 0x000F5C63
	private void RefreshMeter()
	{
		if (this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x060085B9 RID: 34233 RVA: 0x000F7A78 File Offset: 0x000F5C78
	private void OnRocketLanded(object data)
	{
		if (this.consumeOnLand)
		{
			this.storage.ConsumeAllIgnoringDisease();
		}
		if (this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x060085BA RID: 34234 RVA: 0x00349798 File Offset: 0x00347998
	private void OnCopySettings(object data)
	{
		OxidizerTank component = ((GameObject)data).GetComponent<OxidizerTank>();
		if (component != null)
		{
			this.UserMaxCapacity = component.UserMaxCapacity;
		}
	}

	// Token: 0x060085BB RID: 34235 RVA: 0x003497C8 File Offset: 0x003479C8
	[ContextMenu("Fill Tank")]
	public void DEBUG_FillTank(SimHashes element)
	{
		base.GetComponent<FlatTagFilterable>().selectedTags.Add(element.CreateTag());
		if (ElementLoader.FindElementByHash(element).IsLiquid)
		{
			this.storage.AddLiquid(element, this.targetFillMass, ElementLoader.FindElementByHash(element).defaultValues.temperature, 0, 0, false, true);
			return;
		}
		if (ElementLoader.FindElementByHash(element).IsSolid)
		{
			GameObject go = ElementLoader.FindElementByHash(element).substance.SpawnResource(base.gameObject.transform.GetPosition(), this.targetFillMass, 300f, byte.MaxValue, 0, false, false, false);
			this.storage.Store(go, false, false, true, false);
		}
	}

	// Token: 0x060085BC RID: 34236 RVA: 0x00349874 File Offset: 0x00347A74
	public OxidizerTank()
	{
		Tag[] array2;
		if (!DlcManager.IsExpansion1Active())
		{
			Tag[] array = new Tag[2];
			array[0] = SimHashes.OxyRock.CreateTag();
			array2 = array;
			array[1] = SimHashes.LiquidOxygen.CreateTag();
		}
		else
		{
			Tag[] array3 = new Tag[3];
			array3[0] = SimHashes.OxyRock.CreateTag();
			array3[1] = SimHashes.LiquidOxygen.CreateTag();
			array2 = array3;
			array3[2] = SimHashes.Fertilizer.CreateTag();
		}
		this.oxidizerTypes = array2;
		base..ctor();
	}

	// Token: 0x040064F6 RID: 25846
	public Storage storage;

	// Token: 0x040064F7 RID: 25847
	public bool supportsMultipleOxidizers;

	// Token: 0x040064F8 RID: 25848
	private MeterController meter;

	// Token: 0x040064F9 RID: 25849
	private bool isSuspended;

	// Token: 0x040064FA RID: 25850
	public bool consumeOnLand = true;

	// Token: 0x040064FB RID: 25851
	[Serialize]
	public float maxFillMass;

	// Token: 0x040064FC RID: 25852
	[Serialize]
	public float targetFillMass;

	// Token: 0x040064FD RID: 25853
	public List<SimHashes> discoverResourcesOnSpawn;

	// Token: 0x040064FE RID: 25854
	[SerializeField]
	private Tag[] oxidizerTypes;

	// Token: 0x040064FF RID: 25855
	private FilteredStorage filteredStorage;

	// Token: 0x04006500 RID: 25856
	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04006501 RID: 25857
	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnRocketLanded(data);
	});

	// Token: 0x04006502 RID: 25858
	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnStorageChange(data);
	});
}
