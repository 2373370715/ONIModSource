using System;
using System.Collections.Generic;

// Token: 0x0200081C RID: 2076
public class SchedulerGroup
{
	// Token: 0x17000114 RID: 276
	// (get) Token: 0x06002530 RID: 9520 RVA: 0x000B8440 File Offset: 0x000B6640
	// (set) Token: 0x06002531 RID: 9521 RVA: 0x000B8448 File Offset: 0x000B6648
	public Scheduler scheduler { get; private set; }

	// Token: 0x06002532 RID: 9522 RVA: 0x000B8451 File Offset: 0x000B6651
	public SchedulerGroup(Scheduler scheduler)
	{
		this.scheduler = scheduler;
		this.Reset();
	}

	// Token: 0x06002533 RID: 9523 RVA: 0x000B8471 File Offset: 0x000B6671
	public void FreeResources()
	{
		if (this.scheduler != null)
		{
			this.scheduler.FreeResources();
		}
		this.scheduler = null;
		if (this.handles != null)
		{
			this.handles.Clear();
		}
		this.handles = null;
	}

	// Token: 0x06002534 RID: 9524 RVA: 0x001CB890 File Offset: 0x001C9A90
	public void Reset()
	{
		foreach (SchedulerHandle schedulerHandle in this.handles)
		{
			schedulerHandle.ClearScheduler();
		}
		this.handles.Clear();
	}

	// Token: 0x06002535 RID: 9525 RVA: 0x000B84A7 File Offset: 0x000B66A7
	public void Add(SchedulerHandle handle)
	{
		this.handles.Add(handle);
	}

	// Token: 0x04001920 RID: 6432
	private List<SchedulerHandle> handles = new List<SchedulerHandle>();
}
