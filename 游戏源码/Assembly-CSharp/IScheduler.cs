using System;

// Token: 0x02000817 RID: 2071
public interface IScheduler
{
	// Token: 0x0600251A RID: 9498
	SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null);
}
