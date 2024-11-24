using System;
using UnityEngine;

// Token: 0x020017C4 RID: 6084
public class FetchDrone : KMonoBehaviour
{
	// Token: 0x06007D60 RID: 32096 RVA: 0x003264F8 File Offset: 0x003246F8
	protected override void OnSpawn()
	{
		ChoreGroup[] array = new ChoreGroup[]
		{
			Db.Get().ChoreGroups.Build,
			Db.Get().ChoreGroups.Basekeeping,
			Db.Get().ChoreGroups.Cook,
			Db.Get().ChoreGroups.Art,
			Db.Get().ChoreGroups.Dig,
			Db.Get().ChoreGroups.Research,
			Db.Get().ChoreGroups.Farming,
			Db.Get().ChoreGroups.Ranching,
			Db.Get().ChoreGroups.MachineOperating,
			Db.Get().ChoreGroups.MedicalAid,
			Db.Get().ChoreGroups.Combat,
			Db.Get().ChoreGroups.LifeSupport,
			Db.Get().ChoreGroups.Recreation,
			Db.Get().ChoreGroups.Toggle,
			Db.Get().ChoreGroups.Rocketry
		};
		for (int i = 0; i < array.Length; i++)
		{
			this.choreConsumer.SetPermittedByUser(array[i], false);
		}
		foreach (Storage storage in base.GetComponents<Storage>())
		{
			if (storage.storageID != GameTags.ChargedPortableBattery)
			{
				this.pickupableStorage = storage;
				break;
			}
		}
		this.SetupPickupable();
		this.pickupableStorage.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
	}

	// Token: 0x06007D61 RID: 32097 RVA: 0x00326698 File Offset: 0x00324898
	public void SetupPickupable()
	{
		this.animController = base.GetComponent<KBatchedAnimController>();
		GameObject gameObject = Util.NewGameObject(base.gameObject, "pickupableSymbol");
		gameObject.SetActive(false);
		bool flag;
		Vector3 position = this.animController.GetSymbolTransform(FetchDrone.HASH_SNAPTO_THING, out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
		gameObject.transform.SetPosition(position);
		this.pickupableKBAC = gameObject.AddComponent<KBatchedAnimController>();
		this.pickupableKBAC.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("algae_kanim")
		};
		this.pickupableKBAC.sceneLayer = Grid.SceneLayer.BuildingUse;
		KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
		kbatchedAnimTracker.symbol = FetchDrone.HASH_SNAPTO_THING;
		kbatchedAnimTracker.offset = Vector3.zero;
	}

	// Token: 0x06007D62 RID: 32098 RVA: 0x000F28E7 File Offset: 0x000F0AE7
	private void OnStorageChanged(object data)
	{
		this.ShowPickupSymbol(!this.pickupableStorage.IsEmpty());
	}

	// Token: 0x06007D63 RID: 32099 RVA: 0x00326768 File Offset: 0x00324968
	private void ShowPickupSymbol(bool show)
	{
		this.pickupableKBAC.gameObject.SetActive(show);
		if (show)
		{
			Pickupable component = this.pickupableStorage.items[0].GetComponent<Pickupable>();
			if (component != null)
			{
				KBatchedAnimController component2 = component.GetComponent<KBatchedAnimController>();
				if (component.GetComponent<MinionIdentity>())
				{
					this.AddAnimTracker(component2.gameObject);
				}
				else
				{
					this.pickupableKBAC.SwapAnims(component2.AnimFiles);
					this.pickupableKBAC.Play(component2.currentAnim, KAnim.PlayMode.Loop, 1f, 0f);
				}
			}
		}
		this.animController.SetSymbolVisiblity(FetchDrone.BOTTOM, !show);
		this.animController.SetSymbolVisiblity(FetchDrone.BOTTOM_CARRY, show);
	}

	// Token: 0x06007D64 RID: 32100 RVA: 0x001AE220 File Offset: 0x001AC420
	private void AddAnimTracker(GameObject go)
	{
		KAnimControllerBase component = go.GetComponent<KAnimControllerBase>();
		if (component == null)
		{
			return;
		}
		if (component.AnimFiles != null && component.AnimFiles.Length != 0 && component.AnimFiles[0] != null && component.GetComponent<Pickupable>().trackOnPickup)
		{
			KBatchedAnimTracker kbatchedAnimTracker = go.AddComponent<KBatchedAnimTracker>();
			kbatchedAnimTracker.useTargetPoint = false;
			kbatchedAnimTracker.fadeOut = false;
			kbatchedAnimTracker.symbol = new HashedString("snapTo_chest");
			kbatchedAnimTracker.forceAlwaysVisible = true;
		}
	}

	// Token: 0x04005EE5 RID: 24293
	private static string HASH_SNAPTO_THING = "snapTo_thing";

	// Token: 0x04005EE6 RID: 24294
	private static string BOTTOM = "bottom";

	// Token: 0x04005EE7 RID: 24295
	private static string BOTTOM_CARRY = "bottom_carry";

	// Token: 0x04005EE8 RID: 24296
	private KBatchedAnimController pickupableKBAC;

	// Token: 0x04005EE9 RID: 24297
	private KBatchedAnimController animController;

	// Token: 0x04005EEA RID: 24298
	private Storage pickupableStorage;

	// Token: 0x04005EEB RID: 24299
	[MyCmpAdd]
	private ChoreConsumer choreConsumer;
}
