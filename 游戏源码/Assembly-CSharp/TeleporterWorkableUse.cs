using System;

// Token: 0x02000FEF RID: 4079
public class TeleporterWorkableUse : Workable
{
	// Token: 0x060052E7 RID: 21223 RVA: 0x000BC8FA File Offset: 0x000BAAFA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060052E8 RID: 21224 RVA: 0x000D6099 File Offset: 0x000D4299
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(5f);
		this.resetProgressOnStop = true;
	}

	// Token: 0x060052E9 RID: 21225 RVA: 0x002767D8 File Offset: 0x002749D8
	protected override void OnStartWork(WorkerBase worker)
	{
		Teleporter component = base.GetComponent<Teleporter>();
		Teleporter teleporter = component.FindTeleportTarget();
		component.SetTeleportTarget(teleporter);
		TeleportalPad.StatesInstance smi = teleporter.GetSMI<TeleportalPad.StatesInstance>();
		smi.sm.targetTeleporter.Trigger(smi);
	}

	// Token: 0x060052EA RID: 21226 RVA: 0x00276810 File Offset: 0x00274A10
	protected override void OnStopWork(WorkerBase worker)
	{
		TeleportalPad.StatesInstance smi = this.GetSMI<TeleportalPad.StatesInstance>();
		smi.sm.doTeleport.Trigger(smi);
	}
}
