using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02001757 RID: 5975
public class RemoteWorker : StandardWorker
{
	// Token: 0x06007AF4 RID: 31476 RVA: 0x00319F6C File Offset: 0x0031816C
	public override AttributeConverterInstance GetAttributeConverter(string id)
	{
		RemoteWorkerDock homeDepot = this.remoteWorkerSM.HomeDepot;
		WorkerBase workerBase = ((homeDepot != null) ? homeDepot.GetActiveTerminalWorker() : null) ?? null;
		if (workerBase != null)
		{
			return workerBase.GetAttributeConverter(id);
		}
		return null;
	}

	// Token: 0x06007AF5 RID: 31477 RVA: 0x000F0D16 File Offset: 0x000EEF16
	protected override void TryPlayingIdle()
	{
		if (this.remoteWorkerSM.Docked)
		{
			base.GetComponent<KAnimControllerBase>().Play("in_dock_idle", KAnim.PlayMode.Once, 1f, 0f);
			return;
		}
		base.TryPlayingIdle();
	}

	// Token: 0x06007AF6 RID: 31478 RVA: 0x00319FA8 File Offset: 0x003181A8
	protected override void InternalStopWork(Workable target_workable, bool is_aborted)
	{
		base.InternalStopWork(target_workable, is_aborted);
		Vector3 position = base.transform.GetPosition();
		RemoteWorkerSM remoteWorkerSM = this.remoteWorkerSM;
		position.z = Grid.GetLayerZ((remoteWorkerSM != null && remoteWorkerSM.Docked) ? Grid.SceneLayer.BuildingUse : Grid.SceneLayer.Move);
		base.transform.SetPosition(position);
	}

	// Token: 0x04005C2D RID: 23597
	[MyCmpGet]
	private RemoteWorkerSM remoteWorkerSM;
}
