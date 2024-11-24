using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E0A RID: 3594
public class IceKettleWorkable : Workable
{
	// Token: 0x17000371 RID: 881
	// (get) Token: 0x060046B3 RID: 18099 RVA: 0x000CDD71 File Offset: 0x000CBF71
	// (set) Token: 0x060046B4 RID: 18100 RVA: 0x000CDD79 File Offset: 0x000CBF79
	public MeterController meter { get; private set; }

	// Token: 0x060046B5 RID: 18101 RVA: 0x0024FAC8 File Offset: 0x0024DCC8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_arrow",
			"meter_scale"
		});
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_icemelter_kettle_kanim")
		};
		this.synchronizeAnims = true;
		base.SetOffsets(new CellOffset[]
		{
			this.workCellOffset
		});
		base.SetWorkTime(5f);
		this.resetProgressOnStop = true;
		this.showProgressBar = false;
		this.storage.onDestroyItemsDropped = new Action<List<GameObject>>(this.RestoreStoredItemsInteractions);
		this.handler = base.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
	}

	// Token: 0x060046B6 RID: 18102 RVA: 0x000CDD82 File Offset: 0x000CBF82
	protected override void OnSpawn()
	{
		this.AdjustStoredItemsPositionsAndWorkable();
	}

	// Token: 0x060046B7 RID: 18103 RVA: 0x0024FBA4 File Offset: 0x0024DDA4
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		this.meter.gameObject.SetActive(true);
		PrimaryElement component = pickupableStartWorkInfo.originalPickupable.GetComponent<PrimaryElement>();
		this.meter.SetSymbolTint(new KAnimHashedString("meter_fill"), component.Element.substance.colour);
		this.meter.SetSymbolTint(new KAnimHashedString("water1"), component.Element.substance.colour);
	}

	// Token: 0x060046B8 RID: 18104 RVA: 0x0024FC2C File Offset: 0x0024DE2C
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		float value = (this.workTime - base.WorkTimeRemaining) / this.workTime;
		this.meter.SetPositionPercent(Mathf.Clamp01(value));
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x060046B9 RID: 18105 RVA: 0x0024FC68 File Offset: 0x0024DE68
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = worker.GetComponent<Storage>();
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		if (pickupableStartWorkInfo.amount > 0f)
		{
			this.storage.TransferMass(component, pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID(), pickupableStartWorkInfo.amount, false, false, false);
		}
		GameObject gameObject = component.FindFirst(pickupableStartWorkInfo.originalPickupable.KPrefabID.PrefabID());
		if (gameObject != null)
		{
			pickupableStartWorkInfo.setResultCb(gameObject);
		}
		else
		{
			pickupableStartWorkInfo.setResultCb(null);
		}
		base.OnCompleteWork(worker);
		foreach (GameObject gameObject2 in component.items)
		{
			if (gameObject2.HasTag(GameTags.Liquid))
			{
				Pickupable component2 = gameObject2.GetComponent<Pickupable>();
				this.RestorePickupableInteractions(component2);
			}
		}
	}

	// Token: 0x060046BA RID: 18106 RVA: 0x000CDD8A File Offset: 0x000CBF8A
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		this.meter.gameObject.SetActive(false);
	}

	// Token: 0x060046BB RID: 18107 RVA: 0x000CDD82 File Offset: 0x000CBF82
	private void OnStorageChanged(object obj)
	{
		this.AdjustStoredItemsPositionsAndWorkable();
	}

	// Token: 0x060046BC RID: 18108 RVA: 0x0024FD5C File Offset: 0x0024DF5C
	private void AdjustStoredItemsPositionsAndWorkable()
	{
		int cell = Grid.PosToCell(this);
		Vector3 position = Grid.CellToPosCCC(Grid.OffsetCell(cell, new CellOffset(0, 0)), Grid.SceneLayer.Ore);
		foreach (GameObject gameObject in this.storage.items)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			component.transform.SetPosition(position);
			component.UpdateCachedCell(cell);
			this.OverridePickupableInteractions(component);
		}
	}

	// Token: 0x060046BD RID: 18109 RVA: 0x000CDDA4 File Offset: 0x000CBFA4
	private void OverridePickupableInteractions(Pickupable pickupable)
	{
		pickupable.AddTag(GameTags.LiquidSource);
		pickupable.targetWorkable = this;
		pickupable.SetOffsets(new CellOffset[]
		{
			this.workCellOffset
		});
	}

	// Token: 0x060046BE RID: 18110 RVA: 0x000CDDD1 File Offset: 0x000CBFD1
	private void RestorePickupableInteractions(Pickupable pickupable)
	{
		pickupable.RemoveTag(GameTags.LiquidSource);
		pickupable.targetWorkable = pickupable;
		pickupable.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x060046BF RID: 18111 RVA: 0x0024FDEC File Offset: 0x0024DFEC
	private void RestoreStoredItemsInteractions(List<GameObject> specificItems = null)
	{
		specificItems = ((specificItems == null) ? this.storage.items : specificItems);
		foreach (GameObject gameObject in specificItems)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			this.RestorePickupableInteractions(component);
		}
	}

	// Token: 0x060046C0 RID: 18112 RVA: 0x0024FE54 File Offset: 0x0024E054
	protected override void OnCleanUp()
	{
		if (base.worker != null)
		{
			ChoreDriver component = base.worker.GetComponent<ChoreDriver>();
			base.worker.StopWork();
			component.StopChore();
		}
		this.RestoreStoredItemsInteractions(null);
		base.Unsubscribe(this.handler);
		base.OnCleanUp();
	}

	// Token: 0x040030F7 RID: 12535
	public Storage storage;

	// Token: 0x040030F8 RID: 12536
	private int handler;

	// Token: 0x040030FA RID: 12538
	public CellOffset workCellOffset = new CellOffset(0, 0);
}
