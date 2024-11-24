using System;
using System.Linq;
using FoodRehydrator;
using KSerialization;
using UnityEngine;

public class DehydratedFoodPackage : Workable, IApproachable
{
	public class RehydrateStartWorkItem : Worker.StartWorkInfo
	{
		public DehydratedFoodPackage package;

		public Action<GameObject> setResultCb;

		public RehydrateStartWorkItem(DehydratedFoodPackage pkg, Action<GameObject> setResultCB)
			: base(pkg)
		{
			package = pkg;
			setResultCb = setResultCB;
		}
	}

	[Serialize]
	public Tag FoodTag;

	[MyCmpReq]
	private Storage storage;

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

	public override BuildingFacade GetBuildingFacade()
	{
		if (!(Rehydrator != null))
		{
			return null;
		}
		return Rehydrator.GetComponent<BuildingFacade>();
	}

	public override KAnimControllerBase GetAnimController()
	{
		if (!(Rehydrator != null))
		{
			return null;
		}
		return Rehydrator.GetComponent<KAnimControllerBase>();
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		SetOffsets(new CellOffset[2]
		{
			default(CellOffset),
			new CellOffset(0, -1)
		});
		if (storage.items.Count < 1)
		{
			storage.ConsumeAllIgnoringDisease(FoodTag);
			int cell = Grid.PosToCell(this);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(FoodTag), Grid.CellToPosCBC(cell, Grid.SceneLayer.Creatures), Grid.SceneLayer.Creatures);
			gameObject.SetActive(value: true);
			gameObject.GetComponent<Edible>().Calories = 1000000f;
			storage.Store(gameObject);
		}
		Subscribe(-1697596308, StorageChangeHandler);
		DehydrateItem(storage.items.ElementAtOrDefault(0));
	}

	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		if (Rehydrator != null)
		{
			DehydratedManager component = Rehydrator.GetComponent<DehydratedManager>();
			if (component != null)
			{
				component.SetFabricatedFoodSymbol(FoodTag);
			}
			Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable(this);
		}
	}

	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		if (storage.items.Count != 1)
		{
			DebugUtil.DevAssert(test: false, "OnCompleteWork invalid contents of package");
			return;
		}
		GameObject gameObject = storage.items[0];
		storage.Transfer(worker.GetComponent<Storage>());
		DebugUtil.DevAssert(Rehydrator != null, "OnCompleteWork but no rehydrator");
		DehydratedManager component = Rehydrator.GetComponent<DehydratedManager>();
		Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable(null);
		component.ConsumeResourcesForRehydration(base.gameObject, gameObject);
		RehydrateStartWorkItem rehydrateStartWorkItem = (RehydrateStartWorkItem)worker.startWorkInfo;
		if (rehydrateStartWorkItem != null && rehydrateStartWorkItem.setResultCb != null && gameObject != null)
		{
			rehydrateStartWorkItem.setResultCb(gameObject);
		}
	}

	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		if (Rehydrator != null)
		{
			Rehydrator.GetComponent<AccessabilityManager>().SetActiveWorkable(null);
		}
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	private void StorageChangeHandler(object obj)
	{
		GameObject item = (GameObject)obj;
		DebugUtil.DevAssert(!storage.items.Contains(item), "Attempting to add item to a dehydrated food package which is not allowed");
		RehydrateItem(item);
	}

	public void DehydrateItem(GameObject item)
	{
		DebugUtil.DevAssert(item != null, "Attempting to dehydrate contents of an empty packet");
		if (storage.items.Count != 1 || item == null)
		{
			DebugUtil.DevAssert(test: false, "DehydrateItem called, incorrect content");
		}
		else
		{
			item.AddTag(GameTags.Dehydrated);
		}
	}

	public void RehydrateItem(GameObject item)
	{
		if (storage.items.Count != 0)
		{
			DebugUtil.DevAssert(test: false, "RehydrateItem called, incorrect storage content");
			return;
		}
		item.RemoveTag(GameTags.Dehydrated);
		item.AddTag(GameTags.Rehydrated);
		item.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.RehydratedFood);
	}

	private void Swap<Type>(ref Type a, ref Type b)
	{
		Type val = a;
		a = b;
		b = val;
	}
}
