using System;
using System.Linq;
using FoodRehydrator;
using KSerialization;
using UnityEngine;

// Token: 0x02001216 RID: 4630
public class DehydratedFoodPackage : Workable, IApproachable
{
	// Token: 0x170005B9 RID: 1465
	// (get) Token: 0x06005EEB RID: 24299 RVA: 0x002A6D30 File Offset: 0x002A4F30
	// (set) Token: 0x06005EEC RID: 24300 RVA: 0x000A5E40 File Offset: 0x000A4040
	public GameObject Rehydrator
	{
		get
		{
			Storage storage = base.gameObject.GetComponent<Pickupable>().storage;
			if (storage != null)
			{
				return storage.gameObject;
			}
			return null;
		}
		private set
		{
		}
	}

	// Token: 0x06005EED RID: 24301 RVA: 0x000DDF97 File Offset: 0x000DC197
	public override BuildingFacade GetBuildingFacade()
	{
		if (!(this.Rehydrator != null))
		{
			return null;
		}
		return this.Rehydrator.GetComponent<BuildingFacade>();
	}

	// Token: 0x06005EEE RID: 24302 RVA: 0x000DDFB4 File Offset: 0x000DC1B4
	public override KAnimControllerBase GetAnimController()
	{
		if (!(this.Rehydrator != null))
		{
			return null;
		}
		return this.Rehydrator.GetComponent<KAnimControllerBase>();
	}

	// Token: 0x06005EEF RID: 24303 RVA: 0x002A6D60 File Offset: 0x002A4F60
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetOffsets(new CellOffset[]
		{
			default(CellOffset),
			new CellOffset(0, -1)
		});
		if (this.storage.items.Count < 1)
		{
			this.storage.ConsumeAllIgnoringDisease(this.FoodTag);
			int cell = Grid.PosToCell(this);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.FoodTag), Grid.CellToPosCBC(cell, Grid.SceneLayer.Creatures), Grid.SceneLayer.Creatures, null, 0);
			gameObject.SetActive(true);
			gameObject.GetComponent<Edible>().Calories = 1000000f;
			this.storage.Store(gameObject, false, false, true, false);
		}
		base.Subscribe(-1697596308, new Action<object>(this.StorageChangeHandler));
		this.DehydrateItem(this.storage.items.ElementAtOrDefault(0));
	}

	// Token: 0x06005EF0 RID: 24304 RVA: 0x002A6E2C File Offset: 0x002A502C
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (this.Rehydrator != null)
		{
			DehydratedManager component = this.Rehydrator.GetComponent<DehydratedManager>();
			if (component != null)
			{
				component.SetFabricatedFoodSymbol(this.FoodTag);
			}
			this.Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable(this);
		}
	}

	// Token: 0x06005EF1 RID: 24305 RVA: 0x002A6E80 File Offset: 0x002A5080
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		if (this.storage.items.Count != 1)
		{
			DebugUtil.DevAssert(false, "OnCompleteWork invalid contents of package", null);
			return;
		}
		GameObject gameObject = this.storage.items[0];
		this.storage.Transfer(worker.GetComponent<Storage>(), false, false);
		DebugUtil.DevAssert(this.Rehydrator != null, "OnCompleteWork but no rehydrator", null);
		DehydratedManager component = this.Rehydrator.GetComponent<DehydratedManager>();
		this.Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable(null);
		component.ConsumeResourcesForRehydration(base.gameObject, gameObject);
		DehydratedFoodPackage.RehydrateStartWorkItem rehydrateStartWorkItem = (DehydratedFoodPackage.RehydrateStartWorkItem)worker.GetStartWorkInfo();
		if (rehydrateStartWorkItem != null && rehydrateStartWorkItem.setResultCb != null && gameObject != null)
		{
			rehydrateStartWorkItem.setResultCb(gameObject);
		}
	}

	// Token: 0x06005EF2 RID: 24306 RVA: 0x000DDFD1 File Offset: 0x000DC1D1
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.Rehydrator != null)
		{
			this.Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable(null);
		}
	}

	// Token: 0x06005EF3 RID: 24307 RVA: 0x000DDFF9 File Offset: 0x000DC1F9
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06005EF4 RID: 24308 RVA: 0x002A6F44 File Offset: 0x002A5144
	private void StorageChangeHandler(object obj)
	{
		GameObject item = (GameObject)obj;
		DebugUtil.DevAssert(!this.storage.items.Contains(item), "Attempting to add item to a dehydrated food package which is not allowed", null);
		this.RehydrateItem(item);
	}

	// Token: 0x06005EF5 RID: 24309 RVA: 0x002A6F80 File Offset: 0x002A5180
	public void DehydrateItem(GameObject item)
	{
		DebugUtil.DevAssert(item != null, "Attempting to dehydrate contents of an empty packet", null);
		if (this.storage.items.Count != 1 || item == null)
		{
			DebugUtil.DevAssert(false, "DehydrateItem called, incorrect content", null);
			return;
		}
		item.AddTag(GameTags.Dehydrated);
	}

	// Token: 0x06005EF6 RID: 24310 RVA: 0x002A6FD4 File Offset: 0x002A51D4
	public void RehydrateItem(GameObject item)
	{
		if (this.storage.items.Count != 0)
		{
			DebugUtil.DevAssert(false, "RehydrateItem called, incorrect storage content", null);
			return;
		}
		item.RemoveTag(GameTags.Dehydrated);
		item.AddTag(GameTags.Rehydrated);
		item.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.RehydratedFood, null);
	}

	// Token: 0x06005EF7 RID: 24311 RVA: 0x002A7038 File Offset: 0x002A5238
	private void Swap<Type>(ref Type a, ref Type b)
	{
		Type type = a;
		a = b;
		b = type;
	}

	// Token: 0x04004384 RID: 17284
	[Serialize]
	public Tag FoodTag;

	// Token: 0x04004385 RID: 17285
	[MyCmpReq]
	private Storage storage;

	// Token: 0x02001217 RID: 4631
	public class RehydrateStartWorkItem : WorkerBase.StartWorkInfo
	{
		// Token: 0x06005EF9 RID: 24313 RVA: 0x000DE001 File Offset: 0x000DC201
		public RehydrateStartWorkItem(DehydratedFoodPackage pkg, Action<GameObject> setResultCB) : base(pkg)
		{
			this.package = pkg;
			this.setResultCb = setResultCB;
		}

		// Token: 0x04004386 RID: 17286
		public DehydratedFoodPackage package;

		// Token: 0x04004387 RID: 17287
		public Action<GameObject> setResultCb;
	}
}
