using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000C99 RID: 3225
[AddComponentMenu("KMonoBehaviour/Workable/AstronautTrainingCenter")]
public class AstronautTrainingCenter : Workable
{
	// Token: 0x06003E16 RID: 15894 RVA: 0x000C854D File Offset: 0x000C674D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.chore = this.CreateChore();
	}

	// Token: 0x06003E17 RID: 15895 RVA: 0x0023364C File Offset: 0x0023184C
	private Chore CreateChore()
	{
		return new WorkChore<AstronautTrainingCenter>(Db.Get().ChoreTypes.Train, this, null, true, null, null, null, false, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x06003E18 RID: 15896 RVA: 0x000C8561 File Offset: 0x000C6761
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<Operational>().SetActive(true, false);
	}

	// Token: 0x06003E19 RID: 15897 RVA: 0x000C8577 File Offset: 0x000C6777
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		worker == null;
		return true;
	}

	// Token: 0x06003E1A RID: 15898 RVA: 0x000C8582 File Offset: 0x000C6782
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		if (this.chore != null && !this.chore.isComplete)
		{
			this.chore.Cancel("completed but not complete??");
		}
		this.chore = this.CreateChore();
	}

	// Token: 0x06003E1B RID: 15899 RVA: 0x000C85BC File Offset: 0x000C67BC
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<Operational>().SetActive(false, false);
	}

	// Token: 0x06003E1C RID: 15900 RVA: 0x000C85D2 File Offset: 0x000C67D2
	public override float GetPercentComplete()
	{
		base.worker == null;
		return 0f;
	}

	// Token: 0x06003E1D RID: 15901 RVA: 0x00233680 File Offset: 0x00231880
	public AstronautTrainingCenter()
	{
		Chore.Precondition isNotMarkedForDeconstruction = default(Chore.Precondition);
		isNotMarkedForDeconstruction.id = "IsNotMarkedForDeconstruction";
		isNotMarkedForDeconstruction.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MARKED_FOR_DECONSTRUCTION;
		isNotMarkedForDeconstruction.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Deconstructable deconstructable = data as Deconstructable;
			return deconstructable == null || !deconstructable.IsMarkedForDeconstruction();
		};
		this.IsNotMarkedForDeconstruction = isNotMarkedForDeconstruction;
		base..ctor();
	}

	// Token: 0x04002A64 RID: 10852
	public float daysToMasterRole;

	// Token: 0x04002A65 RID: 10853
	private Chore chore;

	// Token: 0x04002A66 RID: 10854
	public Chore.Precondition IsNotMarkedForDeconstruction;
}
