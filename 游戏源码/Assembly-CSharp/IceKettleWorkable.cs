using System.Collections.Generic;
using UnityEngine;

public class IceKettleWorkable : Workable
{
	public Storage storage;

	private int handler;

	public CellOffset workCellOffset = new CellOffset(0, 0);

	public MeterController meter { get; private set; }

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		meter = new MeterController(GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, "meter_target", "meter_arrow", "meter_scale");
		overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_icemelter_kettle_kanim") };
		synchronizeAnims = true;
		SetOffsets(new CellOffset[1] { workCellOffset });
		SetWorkTime(5f);
		resetProgressOnStop = true;
		showProgressBar = false;
		storage.onDestroyItemsDropped = RestoreStoredItemsInteractions;
		handler = Subscribe(-1697596308, OnStroageChanged);
	}

	protected override void OnSpawn()
	{
		AdjustStoredItemsPositionsAndWorkable();
	}

	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		Pickupable.PickupableStartWorkInfo obj = (Pickupable.PickupableStartWorkInfo)worker.startWorkInfo;
		meter.gameObject.SetActive(value: true);
		PrimaryElement component = obj.originalPickupable.GetComponent<PrimaryElement>();
		meter.SetSymbolTint(new KAnimHashedString("meter_fill"), component.Element.substance.colour);
		meter.SetSymbolTint(new KAnimHashedString("water1"), component.Element.substance.colour);
	}

	protected override bool OnWorkTick(Worker worker, float dt)
	{
		float value = (workTime - base.WorkTimeRemaining) / workTime;
		meter.SetPositionPercent(Mathf.Clamp01(value));
		return base.OnWorkTick(worker, dt);
	}

	protected override void OnCompleteWork(Worker worker)
	{
		Storage component = worker.GetComponent<Storage>();
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.startWorkInfo;
		if (pickupableStartWorkInfo.amount > 0f)
		{
			storage.TransferMass(component, pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID(), pickupableStartWorkInfo.amount);
		}
		base.OnCompleteWork(worker);
		foreach (GameObject item in component.items)
		{
			if (item.HasTag(GameTags.Liquid))
			{
				Pickupable component2 = item.GetComponent<Pickupable>();
				RestorePickupableInteractions(component2);
			}
		}
	}

	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		meter.gameObject.SetActive(value: false);
	}

	private void OnStroageChanged(object obj)
	{
		AdjustStoredItemsPositionsAndWorkable();
	}

	private void AdjustStoredItemsPositionsAndWorkable()
	{
		int cell = Grid.PosToCell(this);
		Vector3 position = Grid.CellToPosCCC(Grid.OffsetCell(cell, new CellOffset(0, 0)), Grid.SceneLayer.Ore);
		foreach (GameObject item in storage.items)
		{
			Pickupable component = item.GetComponent<Pickupable>();
			component.transform.SetPosition(position);
			component.UpdateCachedCell(cell);
			OverridePickupableInteractions(component);
		}
	}

	private void OverridePickupableInteractions(Pickupable pickupable)
	{
		pickupable.AddTag(GameTags.LiquidSource);
		pickupable.targetWorkable = this;
		pickupable.SetOffsets(new CellOffset[1] { workCellOffset });
	}

	private void RestorePickupableInteractions(Pickupable pickupable)
	{
		pickupable.RemoveTag(GameTags.LiquidSource);
		pickupable.targetWorkable = pickupable;
		pickupable.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	private void RestoreStoredItemsInteractions(List<GameObject> specificItems = null)
	{
		specificItems = ((specificItems == null) ? storage.items : specificItems);
		foreach (GameObject specificItem in specificItems)
		{
			Pickupable component = specificItem.GetComponent<Pickupable>();
			RestorePickupableInteractions(component);
		}
	}

	protected override void OnCleanUp()
	{
		if (base.worker != null)
		{
			ChoreDriver component = base.worker.GetComponent<ChoreDriver>();
			base.worker.StopWork();
			component.StopChore();
		}
		RestoreStoredItemsInteractions();
		Unsubscribe(handler);
		base.OnCleanUp();
	}
}
