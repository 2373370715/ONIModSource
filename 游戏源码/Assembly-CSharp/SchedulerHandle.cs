using System;

// Token: 0x0200081D RID: 2077
public struct SchedulerHandle
{
	// Token: 0x06002536 RID: 9526 RVA: 0x000B84B5 File Offset: 0x000B66B5
	public SchedulerHandle(Scheduler scheduler, SchedulerEntry entry)
	{
		this.entry = entry;
		this.scheduler = scheduler;
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x06002537 RID: 9527 RVA: 0x000B84C5 File Offset: 0x000B66C5
	public float TimeRemaining
	{
		get
		{
			if (!this.IsValid)
			{
				return -1f;
			}
			return this.entry.time - this.scheduler.GetTime();
		}
	}

	// Token: 0x06002538 RID: 9528 RVA: 0x000B84EC File Offset: 0x000B66EC
	public void FreeResources()
	{
		this.entry.FreeResources();
		this.scheduler = null;
	}

	// Token: 0x06002539 RID: 9529 RVA: 0x000B8500 File Offset: 0x000B6700
	public void ClearScheduler()
	{
		if (this.scheduler == null)
		{
			return;
		}
		this.scheduler.Clear(this);
		this.scheduler = null;
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x0600253A RID: 9530 RVA: 0x000B8523 File Offset: 0x000B6723
	public bool IsValid
	{
		get
		{
			return this.scheduler != null;
		}
	}

	// Token: 0x04001921 RID: 6433
	public SchedulerEntry entry;

	// Token: 0x04001922 RID: 6434
	private Scheduler scheduler;
}
