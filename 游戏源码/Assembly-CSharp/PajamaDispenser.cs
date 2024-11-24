using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000373 RID: 883
public class PajamaDispenser : Workable, IDispenser
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000E56 RID: 3670 RVA: 0x001787C4 File Offset: 0x001769C4
	// (remove) Token: 0x06000E57 RID: 3671 RVA: 0x001787FC File Offset: 0x001769FC
	public event System.Action OnStopWorkEvent;

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000E58 RID: 3672 RVA: 0x000AC664 File Offset: 0x000AA864
	// (set) Token: 0x06000E59 RID: 3673 RVA: 0x00178834 File Offset: 0x00176A34
	private WorkChore<PajamaDispenser> Chore
	{
		get
		{
			return this.chore;
		}
		set
		{
			this.chore = value;
			if (this.chore != null)
			{
				base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.DispenseRequested, null);
				return;
			}
			base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DispenseRequested, true);
		}
	}

	// Token: 0x06000E5A RID: 3674 RVA: 0x000AC66C File Offset: 0x000AA86C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (PajamaDispenser.pajamaPrefab != null)
		{
			return;
		}
		PajamaDispenser.pajamaPrefab = Assets.GetPrefab(new Tag("SleepClinicPajamas"));
	}

	// Token: 0x06000E5B RID: 3675 RVA: 0x00178894 File Offset: 0x00176A94
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Vector3 targetPoint = this.GetTargetPoint();
		targetPoint.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront);
		Util.KInstantiate(PajamaDispenser.pajamaPrefab, targetPoint, Quaternion.identity, null, null, true, 0).SetActive(true);
		this.hasDispenseChore = false;
	}

	// Token: 0x06000E5C RID: 3676 RVA: 0x001788D8 File Offset: 0x00176AD8
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.Chore != null && this.Chore.smi.IsRunning())
		{
			this.Chore.Cancel("work interrupted");
		}
		this.Chore = null;
		if (this.hasDispenseChore)
		{
			this.FetchPajamas();
		}
		if (this.OnStopWorkEvent != null)
		{
			this.OnStopWorkEvent();
		}
	}

	// Token: 0x06000E5D RID: 3677 RVA: 0x00178940 File Offset: 0x00176B40
	[ContextMenu("fetch")]
	public void FetchPajamas()
	{
		if (this.Chore != null)
		{
			return;
		}
		this.hasDispenseChore = true;
		this.Chore = new WorkChore<PajamaDispenser>(Db.Get().ChoreTypes.EquipmentFetch, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, false);
		this.Chore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
	}

	// Token: 0x06000E5E RID: 3678 RVA: 0x001789A0 File Offset: 0x00176BA0
	public void CancelFetch()
	{
		if (this.Chore == null)
		{
			return;
		}
		this.Chore.Cancel("User Cancelled");
		this.Chore = null;
		this.hasDispenseChore = false;
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.DispenseRequested, false);
	}

	// Token: 0x06000E5F RID: 3679 RVA: 0x000AC696 File Offset: 0x000AA896
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.hasDispenseChore)
		{
			this.FetchPajamas();
		}
	}

	// Token: 0x06000E60 RID: 3680 RVA: 0x000AC6AC File Offset: 0x000AA8AC
	public List<Tag> DispensedItems()
	{
		return PajamaDispenser.PajamaList;
	}

	// Token: 0x06000E61 RID: 3681 RVA: 0x000AC6B3 File Offset: 0x000AA8B3
	public Tag SelectedItem()
	{
		return PajamaDispenser.PajamaList[0];
	}

	// Token: 0x06000E62 RID: 3682 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void SelectItem(Tag tag)
	{
	}

	// Token: 0x06000E63 RID: 3683 RVA: 0x000AC6C0 File Offset: 0x000AA8C0
	public void OnOrderDispense()
	{
		this.FetchPajamas();
	}

	// Token: 0x06000E64 RID: 3684 RVA: 0x000AC6C8 File Offset: 0x000AA8C8
	public void OnCancelDispense()
	{
		this.CancelFetch();
	}

	// Token: 0x06000E65 RID: 3685 RVA: 0x000AC6D0 File Offset: 0x000AA8D0
	public bool HasOpenChore()
	{
		return this.Chore != null;
	}

	// Token: 0x04000A67 RID: 2663
	[Serialize]
	private bool hasDispenseChore;

	// Token: 0x04000A68 RID: 2664
	private static GameObject pajamaPrefab = null;

	// Token: 0x04000A6A RID: 2666
	private WorkChore<PajamaDispenser> chore;

	// Token: 0x04000A6B RID: 2667
	private static List<Tag> PajamaList = new List<Tag>
	{
		"SleepClinicPajamas"
	};
}
