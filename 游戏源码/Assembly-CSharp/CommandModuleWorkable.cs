using System;
using UnityEngine;

// Token: 0x02000CE4 RID: 3300
[AddComponentMenu("KMonoBehaviour/Workable/CommandModuleWorkable")]
public class CommandModuleWorkable : Workable
{
	// Token: 0x06004000 RID: 16384 RVA: 0x00239364 File Offset: 0x00237564
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsets(CommandModuleWorkable.entryOffsets);
		this.synchronizeAnims = false;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_incubator_kanim")
		};
		base.SetWorkTime(float.PositiveInfinity);
		this.showProgressBar = false;
	}

	// Token: 0x06004001 RID: 16385 RVA: 0x000AB715 File Offset: 0x000A9915
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}

	// Token: 0x06004002 RID: 16386 RVA: 0x002393BC File Offset: 0x002375BC
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (!(worker != null))
		{
			return base.OnWorkTick(worker, dt);
		}
		if (DlcManager.IsExpansion1Active())
		{
			GameObject gameObject = worker.gameObject;
			base.CompleteWork(worker);
			base.GetComponent<ClustercraftExteriorDoor>().FerryMinion(gameObject);
			return true;
		}
		GameObject gameObject2 = worker.gameObject;
		base.CompleteWork(worker);
		base.GetComponent<MinionStorage>().SerializeMinion(gameObject2);
		return true;
	}

	// Token: 0x06004003 RID: 16387 RVA: 0x000C9867 File Offset: 0x000C7A67
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
	}

	// Token: 0x06004004 RID: 16388 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected override void OnCompleteWork(WorkerBase worker)
	{
	}

	// Token: 0x04002BCB RID: 11211
	private static CellOffset[] entryOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1),
		new CellOffset(0, 2),
		new CellOffset(0, 3),
		new CellOffset(0, 4)
	};
}
