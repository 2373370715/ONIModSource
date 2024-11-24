using System.Collections.Generic;
using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/OxidizerTank")]
public class OxidizerTank : KMonoBehaviour, IUserControlledCapacity
{
	public Storage storage;

	public bool supportsMultipleOxidizers;

	private MeterController meter;

	private bool isSuspended;

	public bool consumeOnLand = true;

	[Serialize]
	public float maxFillMass;

	[Serialize]
	public float targetFillMass;

	public List<SimHashes> discoverResourcesOnSpawn;

	[SerializeField]
	private Tag[] oxidizerTypes = ((!DlcManager.IsExpansion1Active()) ? new Tag[2]
	{
		SimHashes.OxyRock.CreateTag(),
		SimHashes.LiquidOxygen.CreateTag()
	} : new Tag[3]
	{
		SimHashes.OxyRock.CreateTag(),
		SimHashes.LiquidOxygen.CreateTag(),
		SimHashes.Fertilizer.CreateTag()
	});

	private FilteredStorage filteredStorage;

	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnCopySettings(data);
	});

	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnRocketLanded(data);
	});

	private static readonly EventSystem.IntraObjectHandler<OxidizerTank> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<OxidizerTank>(delegate(OxidizerTank component, object data)
	{
		component.OnStorageChange(data);
	});

	public bool IsSuspended => isSuspended;

	public float UserMaxCapacity
	{
		get
		{
			return targetFillMass;
		}
		set
		{
			targetFillMass = value;
			storage.capacityKg = targetFillMass;
			ConduitConsumer component = GetComponent<ConduitConsumer>();
			if (component != null)
			{
				component.capacityKG = targetFillMass;
			}
			Trigger(-945020481, this);
			OnStorageCapacityChanged(targetFillMass);
			if (filteredStorage != null)
			{
				filteredStorage.FilterChanged();
			}
		}
	}

	public float MinCapacity => 0f;

	public float MaxCapacity => maxFillMass;

	public float AmountStored => storage.MassStored();

	public float TotalOxidizerPower
	{
		get
		{
			float num = 0f;
			foreach (GameObject item in storage.items)
			{
				PrimaryElement component = item.GetComponent<PrimaryElement>();
				float num2 = 0f;
				num2 = ((!DlcManager.FeatureClusterSpaceEnabled()) ? RocketStats.oxidizerEfficiencies[component.ElementID.CreateTag()] : Clustercraft.dlc1OxidizerEfficiencies[component.ElementID.CreateTag()]);
				num += component.Mass * num2;
			}
			return num;
		}
	}

	public bool WholeValues => false;

	public LocString CapacityUnits => GameUtil.GetCurrentMassUnit();

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Subscribe(-905833192, OnCopySettingsDelegate);
		if (supportsMultipleOxidizers)
		{
			filteredStorage = new FilteredStorage(this, null, this, use_logic_meter: true, Db.Get().ChoreTypes.Fetch);
			filteredStorage.FilterChanged();
			KBatchedAnimTracker componentInChildren = base.gameObject.GetComponentInChildren<KBatchedAnimTracker>();
			componentInChildren.forceAlwaysAlive = true;
			componentInChildren.matchParentOffset = true;
		}
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (discoverResourcesOnSpawn != null)
		{
			foreach (SimHashes item in discoverResourcesOnSpawn)
			{
				Element element = ElementLoader.FindElementByHash(item);
				DiscoveredResources.Instance.Discover(element.tag, element.GetMaterialCategoryTag());
			}
		}
		GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop);
		RocketModuleCluster component = GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			Debug.Assert(DlcManager.IsExpansion1Active(), "EXP1 not active but trying to use EXP1 rockety system");
			component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionSufficientOxidizer(this));
		}
		UserMaxCapacity = Mathf.Min(UserMaxCapacity, maxFillMass);
		Subscribe(-887025858, OnRocketLandedDelegate);
		Subscribe(-1697596308, OnStorageChangeDelegate);
	}

	public float GetTotalOxidizerAvailable()
	{
		float num = 0f;
		Tag[] array = oxidizerTypes;
		foreach (Tag tag in array)
		{
			num += storage.GetAmountAvailable(tag);
		}
		return num;
	}

	public Dictionary<Tag, float> GetOxidizersAvailable()
	{
		Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
		Tag[] array = oxidizerTypes;
		foreach (Tag key in array)
		{
			dictionary[key] = storage.GetAmountAvailable(key);
		}
		return dictionary;
	}

	private void OnStorageChange(object data)
	{
		RefreshMeter();
	}

	private void OnStorageCapacityChanged(float newCapacity)
	{
		RefreshMeter();
	}

	private void RefreshMeter()
	{
		if (filteredStorage != null)
		{
			filteredStorage.FilterChanged();
		}
	}

	private void OnRocketLanded(object data)
	{
		if (consumeOnLand)
		{
			storage.ConsumeAllIgnoringDisease();
		}
		if (filteredStorage != null)
		{
			filteredStorage.FilterChanged();
		}
	}

	private void OnCopySettings(object data)
	{
		OxidizerTank component = ((GameObject)data).GetComponent<OxidizerTank>();
		if (component != null)
		{
			UserMaxCapacity = component.UserMaxCapacity;
		}
	}

	[ContextMenu("Fill Tank")]
	public void DEBUG_FillTank(SimHashes element)
	{
		GetComponent<FlatTagFilterable>().selectedTags.Add(element.CreateTag());
		if (ElementLoader.FindElementByHash(element).IsLiquid)
		{
			storage.AddLiquid(element, targetFillMass, ElementLoader.FindElementByHash(element).defaultValues.temperature, 0, 0);
		}
		else if (ElementLoader.FindElementByHash(element).IsSolid)
		{
			GameObject go = ElementLoader.FindElementByHash(element).substance.SpawnResource(base.gameObject.transform.GetPosition(), targetFillMass, 300f, byte.MaxValue, 0);
			storage.Store(go);
		}
	}
}
