using System;
using KSerialization;
using UnityEngine;

// Token: 0x020005A4 RID: 1444
public class SpecialCargoBayClusterReceptacle : SingleEntityReceptacle, IBaggedStateAnimationInstructions
{
	// Token: 0x17000084 RID: 132
	// (get) Token: 0x060019A2 RID: 6562 RVA: 0x000B0D01 File Offset: 0x000AEF01
	public bool IsRocketOnGround
	{
		get
		{
			return base.gameObject.HasTag(GameTags.RocketOnGround);
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x060019A3 RID: 6563 RVA: 0x000B0D13 File Offset: 0x000AEF13
	public bool IsRocketInSpace
	{
		get
		{
			return base.gameObject.HasTag(GameTags.RocketInSpace);
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x060019A4 RID: 6564 RVA: 0x000B0D25 File Offset: 0x000AEF25
	private bool isDoorOpen
	{
		get
		{
			return this.capsule.sm.IsDoorOpen.Get(this.capsule);
		}
	}

	// Token: 0x060019A5 RID: 6565 RVA: 0x000B0D42 File Offset: 0x000AEF42
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreType = Db.Get().ChoreTypes.CreatureFetch;
	}

	// Token: 0x060019A6 RID: 6566 RVA: 0x001A3C10 File Offset: 0x001A1E10
	protected override void OnSpawn()
	{
		this.capsule = base.gameObject.GetSMI<SpecialCargoBayCluster.Instance>();
		this.SetupLootSymbolObject();
		base.OnSpawn();
		this.SetTrappedCritterAnimations(base.Occupant);
		base.Subscribe(-1697596308, new Action<object>(this.OnCritterStorageChanged));
		base.Subscribe<SpecialCargoBayClusterReceptacle>(-887025858, SpecialCargoBayClusterReceptacle.OnRocketLandedDelegate);
		base.Subscribe<SpecialCargoBayClusterReceptacle>(-1447108533, SpecialCargoBayClusterReceptacle.OnCargoBayRelocatedDelegate);
		base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
	}

	// Token: 0x060019A7 RID: 6567 RVA: 0x001A3C98 File Offset: 0x001A1E98
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			SpecialCargoBayClusterReceptacle component = gameObject.GetComponent<SpecialCargoBayClusterReceptacle>();
			if (component != null)
			{
				Tag tag = (component.Occupant != null) ? component.Occupant.PrefabID() : component.requestedEntityTag;
				if (base.Occupant != null && base.Occupant.PrefabID() != tag)
				{
					this.ClearOccupant();
				}
				if (tag != this.requestedEntityTag && this.fetchChore != null)
				{
					base.CancelActiveRequest();
				}
				if (tag != Tag.Invalid)
				{
					this.CreateOrder(tag, component.requestedEntityAdditionalFilterTag);
				}
			}
		}
	}

	// Token: 0x060019A8 RID: 6568 RVA: 0x000B0D5F File Offset: 0x000AEF5F
	public override void CreateOrder(Tag entityTag, Tag additionalFilterTag)
	{
		base.CreateOrder(entityTag, additionalFilterTag);
		if (this.fetchChore != null)
		{
			this.fetchChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		}
	}

	// Token: 0x060019A9 RID: 6569 RVA: 0x001A3D48 File Offset: 0x001A1F48
	public void SetupLootSymbolObject()
	{
		Vector3 storePositionForDrops = this.capsule.GetStorePositionForDrops();
		storePositionForDrops.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
		GameObject gameObject = new GameObject();
		gameObject.name = "lootSymbol";
		gameObject.transform.SetParent(base.transform, true);
		gameObject.SetActive(false);
		gameObject.transform.SetPosition(storePositionForDrops);
		KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddOrGet<KBatchedAnimTracker>();
		kbatchedAnimTracker.symbol = "loot";
		kbatchedAnimTracker.forceAlwaysAlive = true;
		kbatchedAnimTracker.matchParentOffset = true;
		this.lootKBAC = gameObject.AddComponent<KBatchedAnimController>();
		this.lootKBAC.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("mushbar_kanim")
		};
		this.lootKBAC.initialAnim = "object";
		this.buildingAnimCtr.SetSymbolVisiblity("loot", false);
	}

	// Token: 0x060019AA RID: 6570 RVA: 0x001A3E20 File Offset: 0x001A2020
	protected override void ClearOccupant()
	{
		this.LastCritterDead = null;
		if (base.occupyingObject != null)
		{
			this.UnsubscribeFromOccupant();
		}
		this.originWorldID = -1;
		base.occupyingObject = null;
		base.UpdateActive();
		this.UpdateStatusItem();
		if (!this.isDoorOpen)
		{
			if (this.IsRocketOnGround)
			{
				this.SetLootSymbolImage(Tag.Invalid);
				this.capsule.OpenDoor();
			}
		}
		else
		{
			this.capsule.DropInventory();
		}
		base.Trigger(-731304873, base.occupyingObject);
	}

	// Token: 0x060019AB RID: 6571 RVA: 0x000B0D87 File Offset: 0x000AEF87
	private void OnCritterStorageChanged(object obj)
	{
		if (obj != null && this.storage.MassStored() == 0f && base.Occupant != null && base.Occupant == (GameObject)obj)
		{
			this.ClearOccupant();
		}
	}

	// Token: 0x060019AC RID: 6572 RVA: 0x001A3EA8 File Offset: 0x001A20A8
	protected override void SubscribeToOccupant()
	{
		base.SubscribeToOccupant();
		base.Subscribe(base.Occupant, -1582839653, new Action<object>(this.OnTrappedCritterTagsChanged));
		base.Subscribe(base.Occupant, 395373363, new Action<object>(this.OnCreatureInStorageDied));
		base.Subscribe(base.Occupant, 663420073, new Action<object>(this.OnBabyInStorageGrows));
		this.SetupCritterTracker();
		for (int i = 0; i < SpecialCargoBayClusterReceptacle.tagsForCritter.Length; i++)
		{
			Tag tag = SpecialCargoBayClusterReceptacle.tagsForCritter[i];
			base.Occupant.AddTag(tag);
		}
		base.Occupant.GetComponent<Health>().UpdateHealthBar();
	}

	// Token: 0x060019AD RID: 6573 RVA: 0x001A3F58 File Offset: 0x001A2158
	protected override void UnsubscribeFromOccupant()
	{
		base.UnsubscribeFromOccupant();
		base.Unsubscribe(base.Occupant, -1582839653, new Action<object>(this.OnTrappedCritterTagsChanged));
		base.Unsubscribe(base.Occupant, 395373363, new Action<object>(this.OnCreatureInStorageDied));
		base.Unsubscribe(base.Occupant, 663420073, new Action<object>(this.OnBabyInStorageGrows));
		this.RemoveCritterTracker();
		if (base.Occupant != null)
		{
			for (int i = 0; i < SpecialCargoBayClusterReceptacle.tagsForCritter.Length; i++)
			{
				Tag tag = SpecialCargoBayClusterReceptacle.tagsForCritter[i];
				base.occupyingObject.RemoveTag(tag);
			}
			base.occupyingObject.GetComponent<Health>().UpdateHealthBar();
		}
	}

	// Token: 0x060019AE RID: 6574 RVA: 0x001A4010 File Offset: 0x001A2210
	public void SetLootSymbolImage(Tag productTag)
	{
		bool flag = productTag != Tag.Invalid;
		this.lootKBAC.gameObject.SetActive(flag);
		if (flag)
		{
			GameObject prefab = Assets.GetPrefab(productTag.ToString());
			this.lootKBAC.SwapAnims(prefab.GetComponent<KBatchedAnimController>().AnimFiles);
			this.lootKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
		}
	}

	// Token: 0x060019AF RID: 6575 RVA: 0x000B0DC5 File Offset: 0x000AEFC5
	private void SetupCritterTracker()
	{
		if (base.Occupant != null)
		{
			KBatchedAnimTracker kbatchedAnimTracker = base.Occupant.AddOrGet<KBatchedAnimTracker>();
			kbatchedAnimTracker.symbol = "critter";
			kbatchedAnimTracker.forceAlwaysAlive = true;
			kbatchedAnimTracker.matchParentOffset = true;
		}
	}

	// Token: 0x060019B0 RID: 6576 RVA: 0x001A408C File Offset: 0x001A228C
	private void RemoveCritterTracker()
	{
		if (base.Occupant != null)
		{
			KBatchedAnimTracker component = base.Occupant.GetComponent<KBatchedAnimTracker>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
		}
	}

	// Token: 0x060019B1 RID: 6577 RVA: 0x000B0DFD File Offset: 0x000AEFFD
	protected override void ConfigureOccupyingObject(GameObject source)
	{
		this.originWorldID = source.GetMyWorldId();
		source.GetComponent<Baggable>().SetWrangled();
		this.SetTrappedCritterAnimations(source);
	}

	// Token: 0x060019B2 RID: 6578 RVA: 0x001A40C4 File Offset: 0x001A22C4
	private void OnBabyInStorageGrows(object obj)
	{
		int num = this.originWorldID;
		this.UnsubscribeFromOccupant();
		GameObject gameObject = (GameObject)obj;
		this.storage.Store(gameObject, false, false, true, false);
		base.occupyingObject = gameObject;
		this.ConfigureOccupyingObject(gameObject);
		this.originWorldID = num;
		this.PositionOccupyingObject();
		this.SubscribeToOccupant();
		this.UpdateStatusItem();
	}

	// Token: 0x060019B3 RID: 6579 RVA: 0x001A4120 File Offset: 0x001A2320
	private void OnTrappedCritterTagsChanged(object obj)
	{
		if (base.Occupant != null && base.Occupant.HasTag(GameTags.Creatures.Die) && this.LastCritterDead != base.Occupant)
		{
			this.capsule.PlayDeathCloud();
			this.LastCritterDead = base.Occupant;
			this.RemoveCritterTracker();
			base.Occupant.GetComponent<KBatchedAnimController>().SetVisiblity(false);
			Butcherable component = base.Occupant.GetComponent<Butcherable>();
			if (component != null && component.drops != null && component.drops.Length != 0)
			{
				this.SetLootSymbolImage(component.drops[0]);
			}
			else
			{
				this.SetLootSymbolImage(Tag.Invalid);
			}
			if (this.IsRocketInSpace)
			{
				DeathStates.Instance smi = base.Occupant.GetSMI<DeathStates.Instance>();
				smi.GoTo(smi.sm.pst);
			}
		}
	}

	// Token: 0x060019B4 RID: 6580 RVA: 0x001A4200 File Offset: 0x001A2400
	private void OnCreatureInStorageDied(object drops_obj)
	{
		GameObject[] array = drops_obj as GameObject[];
		if (array != null)
		{
			foreach (GameObject go in array)
			{
				this.sideProductStorage.Store(go, false, false, true, false);
			}
		}
	}

	// Token: 0x060019B5 RID: 6581 RVA: 0x000B0E1D File Offset: 0x000AF01D
	private void SetTrappedCritterAnimations(GameObject critter)
	{
		if (critter != null)
		{
			KBatchedAnimController component = critter.GetComponent<KBatchedAnimController>();
			component.FlipX = false;
			component.Play("rocket_biological", KAnim.PlayMode.Loop, 1f, 0f);
			component.enabled = false;
			component.enabled = true;
		}
	}

	// Token: 0x060019B6 RID: 6582 RVA: 0x000B0E5D File Offset: 0x000AF05D
	protected override void PositionOccupyingObject()
	{
		if (base.Occupant != null)
		{
			base.Occupant.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
			this.SetupCritterTracker();
		}
	}

	// Token: 0x060019B7 RID: 6583 RVA: 0x001A423C File Offset: 0x001A243C
	protected override void UpdateStatusItem()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		bool flag = base.Occupant != null;
		if (component != null)
		{
			if (flag)
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.SpecialCargoBayClusterCritterStored, this);
			}
			else
			{
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.SpecialCargoBayClusterCritterStored, false);
			}
		}
		base.UpdateStatusItem();
	}

	// Token: 0x060019B8 RID: 6584 RVA: 0x000B0E85 File Offset: 0x000AF085
	private void OnCargoBayRelocated(object data)
	{
		if (base.Occupant != null)
		{
			KBatchedAnimController component = base.Occupant.GetComponent<KBatchedAnimController>();
			component.enabled = false;
			component.enabled = true;
		}
	}

	// Token: 0x060019B9 RID: 6585 RVA: 0x001A42A0 File Offset: 0x001A24A0
	private void OnRocketLanded(object data)
	{
		if (base.Occupant != null)
		{
			ClusterManager.Instance.MigrateCritter(base.Occupant, base.gameObject.GetMyWorldId(), this.originWorldID);
			this.originWorldID = base.Occupant.GetMyWorldId();
		}
		if (base.Occupant == null && !this.isDoorOpen)
		{
			this.SetLootSymbolImage(Tag.Invalid);
			if (this.sideProductStorage.MassStored() > 0f)
			{
				this.capsule.OpenDoor();
			}
		}
	}

	// Token: 0x060019BA RID: 6586 RVA: 0x000B0EAD File Offset: 0x000AF0AD
	public string GetBaggedAnimationName()
	{
		return "rocket_biological";
	}

	// Token: 0x0400105A RID: 4186
	public const string TRAPPED_CRITTER_ANIM_NAME = "rocket_biological";

	// Token: 0x0400105B RID: 4187
	[MyCmpReq]
	private SymbolOverrideController symbolOverrideComponent;

	// Token: 0x0400105C RID: 4188
	[MyCmpGet]
	private KBatchedAnimController buildingAnimCtr;

	// Token: 0x0400105D RID: 4189
	private KBatchedAnimController lootKBAC;

	// Token: 0x0400105E RID: 4190
	public Storage sideProductStorage;

	// Token: 0x0400105F RID: 4191
	private SpecialCargoBayCluster.Instance capsule;

	// Token: 0x04001060 RID: 4192
	private GameObject LastCritterDead;

	// Token: 0x04001061 RID: 4193
	[Serialize]
	private int originWorldID;

	// Token: 0x04001062 RID: 4194
	private static Tag[] tagsForCritter = new Tag[]
	{
		GameTags.Creatures.TrappedInCargoBay,
		GameTags.Creatures.PausedHunger,
		GameTags.Creatures.PausedReproduction,
		GameTags.Creatures.PreventGrowAnimation,
		GameTags.HideHealthBar,
		GameTags.PreventDeadAnimation
	};

	// Token: 0x04001063 RID: 4195
	private static readonly EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle>(delegate(SpecialCargoBayClusterReceptacle component, object data)
	{
		component.OnRocketLanded(data);
	});

	// Token: 0x04001064 RID: 4196
	private static readonly EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle> OnCargoBayRelocatedDelegate = new EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle>(delegate(SpecialCargoBayClusterReceptacle component, object data)
	{
		component.OnCargoBayRelocated(data);
	});
}
