using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02000B7C RID: 2940
internal class UpdatePercentCompleteParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06003820 RID: 14368 RVA: 0x000C4642 File Offset: 0x000C2842
	public UpdatePercentCompleteParameter() : base("percentComplete")
	{
	}

	// Token: 0x06003821 RID: 14369 RVA: 0x0021A68C File Offset: 0x0021888C
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdatePercentCompleteParameter.Entry item = new UpdatePercentCompleteParameter.Entry
		{
			worker = sound.transform.GetComponent<WorkerBase>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06003822 RID: 14370 RVA: 0x0021A6E8 File Offset: 0x002188E8
	public override void Update(float dt)
	{
		foreach (UpdatePercentCompleteParameter.Entry entry in this.entries)
		{
			if (!(entry.worker == null))
			{
				Workable workable = entry.worker.GetWorkable();
				if (!(workable == null))
				{
					float percentComplete = workable.GetPercentComplete();
					EventInstance ev = entry.ev;
					ev.setParameterByID(entry.parameterId, percentComplete, false);
				}
			}
		}
	}

	// Token: 0x06003823 RID: 14371 RVA: 0x0021A778 File Offset: 0x00218978
	public override void Remove(LoopingSoundParameterUpdater.Sound sound)
	{
		for (int i = 0; i < this.entries.Count; i++)
		{
			if (this.entries[i].ev.handle == sound.ev.handle)
			{
				this.entries.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x04002620 RID: 9760
	private List<UpdatePercentCompleteParameter.Entry> entries = new List<UpdatePercentCompleteParameter.Entry>();

	// Token: 0x02000B7D RID: 2941
	private struct Entry
	{
		// Token: 0x04002621 RID: 9761
		public WorkerBase worker;

		// Token: 0x04002622 RID: 9762
		public EventInstance ev;

		// Token: 0x04002623 RID: 9763
		public PARAMETER_ID parameterId;
	}
}
