using KSerialization;
using UnityEngine;

public class SpecialCargoBayClusterReceptacle : SingleEntityReceptacle, IBaggedStateAnimationInstructions
{
	public const string TRAPPED_CRITTER_ANIM_NAME = "rocket_biological";

	[MyCmpReq]
	private SymbolOverrideController symbolOverrideComponent;

	[MyCmpGet]
	private KBatchedAnimController buildingAnimCtr;

	private KBatchedAnimController lootKBAC;

	public Storage sideProductStorage;

	private SpecialCargoBayCluster.Instance capsule;

	private GameObject LastCritterDead;

	[Serialize]
	private int originWorldID;

	private static Tag[] tagsForCritter = new Tag[6]
	{
		GameTags.Creatures.TrappedInCargoBay,
		GameTags.Creatures.PausedHunger,
		GameTags.Creatures.PausedReproduction,
		GameTags.Creatures.PreventGrowAnimation,
		GameTags.HideHealthBar,
		GameTags.PreventDeadAnimation
	};

	private static readonly EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle>(delegate(SpecialCargoBayClusterReceptacle component, object data)
	{
		component.OnRocketLanded(data);
	});

	private static readonly EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle> OnCargoBayRelocatedDelegate = new EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle>(delegate(SpecialCargoBayClusterReceptacle component, object data)
	{
		component.OnCargoBayRelocated(data);
	});

	public bool IsRocketOnGround => base.gameObject.HasTag(GameTags.RocketOnGround);

	public bool IsRocketInSpace => base.gameObject.HasTag(GameTags.RocketInSpace);

	private bool isDoorOpen => capsule.sm.IsDoorOpen.Get(capsule);

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		choreType = Db.Get().ChoreTypes.CreatureFetch;
	}

	protected override void OnSpawn()
	{
		capsule = base.gameObject.GetSMI<SpecialCargoBayCluster.Instance>();
		SetupLootSymbolObject();
		base.OnSpawn();
		SetTrappedCritterAnimations(base.Occupant);
		Subscribe(-1697596308, OnCritterStorageChanged);
		Subscribe(-887025858, OnRocketLandedDelegate);
		Subscribe(-1447108533, OnCargoBayRelocatedDelegate);
		Subscribe(-905833192, OnCopySettings);
	}

	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (!(gameObject != null))
		{
			return;
		}
		SpecialCargoBayClusterReceptacle component = gameObject.GetComponent<SpecialCargoBayClusterReceptacle>();
		if (component != null)
		{
			Tag tag = ((component.Occupant != null) ? component.Occupant.PrefabID() : component.requestedEntityTag);
			if (base.Occupant != null && base.Occupant.PrefabID() != tag)
			{
				ClearOccupant();
			}
			if (tag != requestedEntityTag && fetchChore != null)
			{
				CancelActiveRequest();
			}
			if (tag != Tag.Invalid)
			{
				CreateOrder(tag, component.requestedEntityAdditionalFilterTag);
			}
		}
	}

	public override void CreateOrder(Tag entityTag, Tag additionalFilterTag)
	{
		base.CreateOrder(entityTag, additionalFilterTag);
		if (fetchChore != null)
		{
			fetchChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, this);
		}
	}

	public void SetupLootSymbolObject()
	{
		Vector3 storePositionForDrops = capsule.GetStorePositionForDrops();
		storePositionForDrops.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
		GameObject gameObject = new GameObject();
		gameObject.name = "lootSymbol";
		gameObject.transform.SetParent(base.transform, worldPositionStays: true);
		gameObject.SetActive(value: false);
		gameObject.transform.SetPosition(storePositionForDrops);
		KBatchedAnimTracker kBatchedAnimTracker = gameObject.AddOrGet<KBatchedAnimTracker>();
		kBatchedAnimTracker.symbol = "loot";
		kBatchedAnimTracker.forceAlwaysAlive = true;
		kBatchedAnimTracker.matchParentOffset = true;
		lootKBAC = gameObject.AddComponent<KBatchedAnimController>();
		lootKBAC.AnimFiles = new KAnimFile[1] { Assets.GetAnim("mushbar_kanim") };
		lootKBAC.initialAnim = "object";
		buildingAnimCtr.SetSymbolVisiblity("loot", is_visible: false);
	}

	protected override void ClearOccupant()
	{
		LastCritterDead = null;
		if (base.occupyingObject != null)
		{
			UnsubscribeFromOccupant();
		}
		originWorldID = -1;
		base.occupyingObject = null;
		UpdateActive();
		UpdateStatusItem();
		if (!isDoorOpen)
		{
			if (IsRocketOnGround)
			{
				SetLootSymbolImage(Tag.Invalid);
				capsule.OpenDoor();
			}
		}
		else
		{
			capsule.DropInventory();
		}
		Trigger(-731304873, base.occupyingObject);
	}

	private void OnCritterStorageChanged(object obj)
	{
		if (obj != null && storage.MassStored() == 0f && base.Occupant != null && base.Occupant == (GameObject)obj)
		{
			ClearOccupant();
		}
	}

	protected override void SubscribeToOccupant()
	{
		base.SubscribeToOccupant();
		Subscribe(base.Occupant, -1582839653, OnTrappedCritterTagsChanged);
		Subscribe(base.Occupant, 395373363, OnCreatureInStorageDied);
		Subscribe(base.Occupant, 663420073, OnBabyInStorageGrows);
		SetupCritterTracker();
		for (int i = 0; i < tagsForCritter.Length; i++)
		{
			Tag tag = tagsForCritter[i];
			base.Occupant.AddTag(tag);
		}
		base.Occupant.GetComponent<Health>().UpdateHealthBar();
	}

	protected override void UnsubscribeFromOccupant()
	{
		base.UnsubscribeFromOccupant();
		Unsubscribe(base.Occupant, -1582839653, OnTrappedCritterTagsChanged);
		Unsubscribe(base.Occupant, 395373363, OnCreatureInStorageDied);
		Unsubscribe(base.Occupant, 663420073, OnBabyInStorageGrows);
		RemoveCritterTracker();
		if (base.Occupant != null)
		{
			for (int i = 0; i < tagsForCritter.Length; i++)
			{
				Tag tag = tagsForCritter[i];
				base.occupyingObject.RemoveTag(tag);
			}
			base.occupyingObject.GetComponent<Health>().UpdateHealthBar();
		}
	}

	public void SetLootSymbolImage(Tag productTag)
	{
		bool flag = productTag != Tag.Invalid;
		lootKBAC.gameObject.SetActive(flag);
		if (flag)
		{
			GameObject prefab = Assets.GetPrefab(productTag.ToString());
			lootKBAC.SwapAnims(prefab.GetComponent<KBatchedAnimController>().AnimFiles);
			lootKBAC.Play("object", KAnim.PlayMode.Loop);
		}
	}

	private void SetupCritterTracker()
	{
		if (base.Occupant != null)
		{
			KBatchedAnimTracker kBatchedAnimTracker = base.Occupant.AddOrGet<KBatchedAnimTracker>();
			kBatchedAnimTracker.symbol = "critter";
			kBatchedAnimTracker.forceAlwaysAlive = true;
			kBatchedAnimTracker.matchParentOffset = true;
		}
	}

	private void RemoveCritterTracker()
	{
		if (base.Occupant != null)
		{
			KBatchedAnimTracker component = base.Occupant.GetComponent<KBatchedAnimTracker>();
			if (component != null)
			{
				Object.Destroy(component);
			}
		}
	}

	protected override void ConfigureOccupyingObject(GameObject source)
	{
		originWorldID = source.GetMyWorldId();
		source.GetComponent<Baggable>().SetWrangled();
		SetTrappedCritterAnimations(source);
	}

	private void OnBabyInStorageGrows(object obj)
	{
		int num = originWorldID;
		UnsubscribeFromOccupant();
		GameObject gameObject = (GameObject)obj;
		storage.Store(gameObject);
		base.occupyingObject = gameObject;
		ConfigureOccupyingObject(gameObject);
		originWorldID = num;
		PositionOccupyingObject();
		SubscribeToOccupant();
		UpdateStatusItem();
	}

	private void OnTrappedCritterTagsChanged(object obj)
	{
		if (base.Occupant != null && base.Occupant.HasTag(GameTags.Creatures.Die) && LastCritterDead != base.Occupant)
		{
			capsule.PlayDeathCloud();
			LastCritterDead = base.Occupant;
			RemoveCritterTracker();
			base.Occupant.GetComponent<KBatchedAnimController>().SetVisiblity(is_visible: false);
			Butcherable component = base.Occupant.GetComponent<Butcherable>();
			if (component != null && component.drops != null && component.drops.Length != 0)
			{
				SetLootSymbolImage(component.drops[0]);
			}
			else
			{
				SetLootSymbolImage(Tag.Invalid);
			}
			if (IsRocketInSpace)
			{
				DeathStates.Instance sMI = base.Occupant.GetSMI<DeathStates.Instance>();
				sMI.GoTo(sMI.sm.pst);
			}
		}
	}

	private void OnCreatureInStorageDied(object drops_obj)
	{
		if (drops_obj is GameObject[] array)
		{
			foreach (GameObject go in array)
			{
				sideProductStorage.Store(go);
			}
		}
	}

	private void SetTrappedCritterAnimations(GameObject critter)
	{
		if (critter != null)
		{
			KBatchedAnimController component = critter.GetComponent<KBatchedAnimController>();
			component.FlipX = false;
			component.Play("rocket_biological", KAnim.PlayMode.Loop);
			component.enabled = false;
			component.enabled = true;
		}
	}

	protected override void PositionOccupyingObject()
	{
		if (base.Occupant != null)
		{
			base.Occupant.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
			SetupCritterTracker();
		}
	}

	protected override void UpdateStatusItem()
	{
		KSelectable component = GetComponent<KSelectable>();
		bool flag = base.Occupant != null;
		if (component != null)
		{
			if (flag)
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.SpecialCargoBayClusterCritterStored, this);
			}
			else
			{
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.SpecialCargoBayClusterCritterStored);
			}
		}
		base.UpdateStatusItem();
	}

	private void OnCargoBayRelocated(object data)
	{
		if (base.Occupant != null)
		{
			KBatchedAnimController component = base.Occupant.GetComponent<KBatchedAnimController>();
			component.enabled = false;
			component.enabled = true;
		}
	}

	private void OnRocketLanded(object data)
	{
		if (base.Occupant != null)
		{
			ClusterManager.Instance.MigrateCritter(base.Occupant, base.gameObject.GetMyWorldId(), originWorldID);
			originWorldID = base.Occupant.GetMyWorldId();
		}
		if (base.Occupant == null && !isDoorOpen)
		{
			SetLootSymbolImage(Tag.Invalid);
			if (sideProductStorage.MassStored() > 0f)
			{
				capsule.OpenDoor();
			}
		}
	}

	public string GetBaggedAnimationName()
	{
		return "rocket_biological";
	}
}
