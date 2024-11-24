using System;

// Token: 0x02000A79 RID: 2681
public class KnockKnock : Activatable
{
	// Token: 0x06003184 RID: 12676 RVA: 0x000C02FB File Offset: 0x000BE4FB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = false;
	}

	// Token: 0x06003185 RID: 12677 RVA: 0x000C030A File Offset: 0x000BE50A
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (!this.doorAnswered)
		{
			this.workTimeRemaining += dt;
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06003186 RID: 12678 RVA: 0x000C032A File Offset: 0x000BE52A
	public void AnswerDoor()
	{
		this.doorAnswered = true;
		this.workTimeRemaining = 1f;
	}

	// Token: 0x04002149 RID: 8521
	private bool doorAnswered;
}
