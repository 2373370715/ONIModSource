using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x0200200F RID: 8207
public class SpeedLoopingSoundUpdater : LoopingSoundParameterUpdater
{
	// Token: 0x0600AE96 RID: 44694 RVA: 0x00111979 File Offset: 0x0010FB79
	public SpeedLoopingSoundUpdater() : base("Speed")
	{
	}

	// Token: 0x0600AE97 RID: 44695 RVA: 0x0041A064 File Offset: 0x00418264
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		SpeedLoopingSoundUpdater.Entry item = new SpeedLoopingSoundUpdater.Entry
		{
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x0600AE98 RID: 44696 RVA: 0x0041A0B0 File Offset: 0x004182B0
	public override void Update(float dt)
	{
		float speedParameterValue = SpeedLoopingSoundUpdater.GetSpeedParameterValue();
		foreach (SpeedLoopingSoundUpdater.Entry entry in this.entries)
		{
			EventInstance ev = entry.ev;
			ev.setParameterByID(entry.parameterId, speedParameterValue, false);
		}
	}

	// Token: 0x0600AE99 RID: 44697 RVA: 0x0041A11C File Offset: 0x0041831C
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

	// Token: 0x0600AE9A RID: 44698 RVA: 0x00111996 File Offset: 0x0010FB96
	public static float GetSpeedParameterValue()
	{
		return Time.timeScale * 1f;
	}

	// Token: 0x0400894A RID: 35146
	private List<SpeedLoopingSoundUpdater.Entry> entries = new List<SpeedLoopingSoundUpdater.Entry>();

	// Token: 0x02002010 RID: 8208
	private struct Entry
	{
		// Token: 0x0400894B RID: 35147
		public EventInstance ev;

		// Token: 0x0400894C RID: 35148
		public PARAMETER_ID parameterId;
	}
}
