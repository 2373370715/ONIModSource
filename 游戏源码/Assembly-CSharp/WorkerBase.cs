using System;
using Klei.AI;

// Token: 0x02000B7E RID: 2942
public abstract class WorkerBase : KMonoBehaviour
{
	// Token: 0x06003824 RID: 14372
	public abstract bool UsesMultiTool();

	// Token: 0x06003825 RID: 14373
	public abstract bool IsFetchDrone();

	// Token: 0x06003826 RID: 14374
	public abstract KBatchedAnimController GetAnimController();

	// Token: 0x06003827 RID: 14375
	public abstract WorkerBase.State GetState();

	// Token: 0x06003828 RID: 14376
	public abstract WorkerBase.StartWorkInfo GetStartWorkInfo();

	// Token: 0x06003829 RID: 14377
	public abstract Workable GetWorkable();

	// Token: 0x0600382A RID: 14378
	public abstract AttributeConverterInstance GetAttributeConverter(string id);

	// Token: 0x0600382B RID: 14379
	public abstract Guid OfferStatusItem(StatusItem item, object data = null);

	// Token: 0x0600382C RID: 14380
	public abstract void RevokeStatusItem(Guid id);

	// Token: 0x0600382D RID: 14381
	public abstract void StartWork(WorkerBase.StartWorkInfo start_work_info);

	// Token: 0x0600382E RID: 14382
	public abstract void StopWork();

	// Token: 0x0600382F RID: 14383
	public abstract bool InstantlyFinish();

	// Token: 0x06003830 RID: 14384
	public abstract WorkerBase.WorkResult Work(float dt);

	// Token: 0x06003831 RID: 14385
	public abstract CellOffset[] GetFetchCellOffsets();

	// Token: 0x06003832 RID: 14386
	public abstract void SetWorkCompleteData(object data);

	// Token: 0x02000B7F RID: 2943
	public class StartWorkInfo
	{
		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06003834 RID: 14388 RVA: 0x000C465F File Offset: 0x000C285F
		// (set) Token: 0x06003835 RID: 14389 RVA: 0x000C4667 File Offset: 0x000C2867
		public Workable workable { get; set; }

		// Token: 0x06003836 RID: 14390 RVA: 0x000C4670 File Offset: 0x000C2870
		public StartWorkInfo(Workable workable)
		{
			this.workable = workable;
		}
	}

	// Token: 0x02000B80 RID: 2944
	public enum State
	{
		// Token: 0x04002626 RID: 9766
		Idle,
		// Token: 0x04002627 RID: 9767
		Working,
		// Token: 0x04002628 RID: 9768
		PendingCompletion,
		// Token: 0x04002629 RID: 9769
		Completing
	}

	// Token: 0x02000B81 RID: 2945
	public enum WorkResult
	{
		// Token: 0x0400262B RID: 9771
		Success,
		// Token: 0x0400262C RID: 9772
		InProgress,
		// Token: 0x0400262D RID: 9773
		Failed
	}
}
