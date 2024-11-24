using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x020010C9 RID: 4297
internal class UpdateDistanceToImpactParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06005826 RID: 22566 RVA: 0x000D97D8 File Offset: 0x000D79D8
	public UpdateDistanceToImpactParameter() : base("distanceToImpact")
	{
	}

	// Token: 0x06005827 RID: 22567 RVA: 0x0028AF40 File Offset: 0x00289140
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateDistanceToImpactParameter.Entry item = new UpdateDistanceToImpactParameter.Entry
		{
			comet = sound.transform.GetComponent<Comet>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06005828 RID: 22568 RVA: 0x0028AF9C File Offset: 0x0028919C
	public override void Update(float dt)
	{
		foreach (UpdateDistanceToImpactParameter.Entry entry in this.entries)
		{
			if (!(entry.comet == null))
			{
				float soundDistance = entry.comet.GetSoundDistance();
				EventInstance ev = entry.ev;
				ev.setParameterByID(entry.parameterId, soundDistance, false);
			}
		}
	}

	// Token: 0x06005829 RID: 22569 RVA: 0x0028B01C File Offset: 0x0028921C
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

	// Token: 0x04003DB8 RID: 15800
	private List<UpdateDistanceToImpactParameter.Entry> entries = new List<UpdateDistanceToImpactParameter.Entry>();

	// Token: 0x020010CA RID: 4298
	private struct Entry
	{
		// Token: 0x04003DB9 RID: 15801
		public Comet comet;

		// Token: 0x04003DBA RID: 15802
		public EventInstance ev;

		// Token: 0x04003DBB RID: 15803
		public PARAMETER_ID parameterId;
	}
}
