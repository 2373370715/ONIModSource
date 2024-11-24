using System;
using UnityEngine;

// Token: 0x02000E22 RID: 3618
[AddComponentMenu("KMonoBehaviour/Workable/LiquidCooledFanWorkable")]
public class LiquidCooledFanWorkable : Workable
{
	// Token: 0x0600472D RID: 18221 RVA: 0x000CD9C0 File Offset: 0x000CBBC0
	private LiquidCooledFanWorkable()
	{
		this.showProgressBar = false;
	}

	// Token: 0x0600472E RID: 18222 RVA: 0x000CE2B0 File Offset: 0x000CC4B0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = null;
	}

	// Token: 0x0600472F RID: 18223 RVA: 0x000CE2BF File Offset: 0x000CC4BF
	protected override void OnSpawn()
	{
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		base.OnSpawn();
	}

	// Token: 0x06004730 RID: 18224 RVA: 0x000CE2FD File Offset: 0x000CC4FD
	protected override void OnStartWork(WorkerBase worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x06004731 RID: 18225 RVA: 0x000CE30C File Offset: 0x000CC50C
	protected override void OnStopWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x06004732 RID: 18226 RVA: 0x000CE30C File Offset: 0x000CC50C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x04003163 RID: 12643
	[MyCmpGet]
	private Operational operational;
}
