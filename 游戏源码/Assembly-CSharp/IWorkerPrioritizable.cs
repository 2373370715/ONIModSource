using System;

// Token: 0x0200067C RID: 1660
public interface IWorkerPrioritizable
{
	// Token: 0x06001E0D RID: 7693
	bool GetWorkerPriority(WorkerBase worker, out int priority);
}
