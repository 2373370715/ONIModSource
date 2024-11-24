using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x020009F3 RID: 2547
internal class UpdateConsumedMassParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06002ED9 RID: 11993 RVA: 0x000BE5D9 File Offset: 0x000BC7D9
	public UpdateConsumedMassParameter() : base("consumedMass")
	{
	}

	// Token: 0x06002EDA RID: 11994 RVA: 0x001F64A0 File Offset: 0x001F46A0
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateConsumedMassParameter.Entry item = new UpdateConsumedMassParameter.Entry
		{
			creatureCalorieMonitor = sound.transform.GetSMI<CreatureCalorieMonitor.Instance>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06002EDB RID: 11995 RVA: 0x001F64FC File Offset: 0x001F46FC
	public override void Update(float dt)
	{
		foreach (UpdateConsumedMassParameter.Entry entry in this.entries)
		{
			if (!entry.creatureCalorieMonitor.IsNullOrStopped())
			{
				float fullness = entry.creatureCalorieMonitor.stomach.GetFullness();
				EventInstance ev = entry.ev;
				ev.setParameterByID(entry.parameterId, fullness, false);
			}
		}
	}

	// Token: 0x06002EDC RID: 11996 RVA: 0x001F6580 File Offset: 0x001F4780
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

	// Token: 0x04001F82 RID: 8066
	private List<UpdateConsumedMassParameter.Entry> entries = new List<UpdateConsumedMassParameter.Entry>();

	// Token: 0x020009F4 RID: 2548
	private struct Entry
	{
		// Token: 0x04001F83 RID: 8067
		public CreatureCalorieMonitor.Instance creatureCalorieMonitor;

		// Token: 0x04001F84 RID: 8068
		public EventInstance ev;

		// Token: 0x04001F85 RID: 8069
		public PARAMETER_ID parameterId;
	}
}
