using System;
using UnityEngine;

// Token: 0x02000818 RID: 2072
public class Scheduler : IScheduler
{
	// Token: 0x1700010F RID: 271
	// (get) Token: 0x0600251B RID: 9499 RVA: 0x000B8342 File Offset: 0x000B6542
	public int Count
	{
		get
		{
			return this.entries.Count;
		}
	}

	// Token: 0x0600251C RID: 9500 RVA: 0x000B834F File Offset: 0x000B654F
	public Scheduler(SchedulerClock clock)
	{
		this.clock = clock;
	}

	// Token: 0x0600251D RID: 9501 RVA: 0x000B8374 File Offset: 0x000B6574
	public float GetTime()
	{
		return this.clock.GetTime();
	}

	// Token: 0x0600251E RID: 9502 RVA: 0x000B8381 File Offset: 0x000B6581
	private SchedulerHandle Schedule(SchedulerEntry entry)
	{
		this.entries.Enqueue(entry.time, entry);
		return new SchedulerHandle(this, entry);
	}

	// Token: 0x0600251F RID: 9503 RVA: 0x001CB70C File Offset: 0x001C990C
	private SchedulerHandle Schedule(string name, float time, float time_interval, Action<object> callback, object callback_data, GameObject profiler_obj)
	{
		SchedulerEntry entry = new SchedulerEntry(name, time + this.clock.GetTime(), time_interval, callback, callback_data, profiler_obj);
		return this.Schedule(entry);
	}

	// Token: 0x06002520 RID: 9504 RVA: 0x001CB73C File Offset: 0x001C993C
	public void FreeResources()
	{
		this.clock = null;
		if (this.entries != null)
		{
			while (this.entries.Count > 0)
			{
				this.entries.Dequeue().Value.FreeResources();
			}
		}
		this.entries = null;
	}

	// Token: 0x06002521 RID: 9505 RVA: 0x001CB78C File Offset: 0x001C998C
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		if (group != null && group.scheduler != this)
		{
			global::Debug.LogError("Scheduler group mismatch!");
		}
		SchedulerHandle schedulerHandle = this.Schedule(name, time, -1f, callback, callback_data, null);
		if (group != null)
		{
			group.Add(schedulerHandle);
		}
		return schedulerHandle;
	}

	// Token: 0x06002522 RID: 9506 RVA: 0x000B839C File Offset: 0x000B659C
	public void Clear(SchedulerHandle handle)
	{
		handle.entry.Clear();
	}

	// Token: 0x06002523 RID: 9507 RVA: 0x001CB7D0 File Offset: 0x001C99D0
	public void Update()
	{
		if (this.Count == 0)
		{
			return;
		}
		int count = this.Count;
		int num = 0;
		using (new KProfiler.Region("Scheduler.Update", null))
		{
			float time = this.clock.GetTime();
			if (this.previousTime != time)
			{
				this.previousTime = time;
				while (num < count && time >= this.entries.Peek().Key)
				{
					SchedulerEntry value = this.entries.Dequeue().Value;
					if (value.callback != null)
					{
						value.callback(value.callbackData);
					}
					num++;
				}
			}
		}
	}

	// Token: 0x04001917 RID: 6423
	public FloatHOTQueue<SchedulerEntry> entries = new FloatHOTQueue<SchedulerEntry>();

	// Token: 0x04001918 RID: 6424
	private SchedulerClock clock;

	// Token: 0x04001919 RID: 6425
	private float previousTime = float.NegativeInfinity;
}
