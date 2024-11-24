using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02000951 RID: 2385
internal abstract class UserVolumeLoopingUpdater : LoopingSoundParameterUpdater
{
	// Token: 0x06002B2C RID: 11052 RVA: 0x000BC125 File Offset: 0x000BA325
	public UserVolumeLoopingUpdater(string parameter, string player_pref) : base(parameter)
	{
		this.playerPref = player_pref;
	}

	// Token: 0x06002B2D RID: 11053 RVA: 0x001DE21C File Offset: 0x001DC41C
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UserVolumeLoopingUpdater.Entry item = new UserVolumeLoopingUpdater.Entry
		{
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06002B2E RID: 11054 RVA: 0x001DE268 File Offset: 0x001DC468
	public override void Update(float dt)
	{
		if (string.IsNullOrEmpty(this.playerPref))
		{
			return;
		}
		float @float = KPlayerPrefs.GetFloat(this.playerPref);
		foreach (UserVolumeLoopingUpdater.Entry entry in this.entries)
		{
			EventInstance ev = entry.ev;
			ev.setParameterByID(entry.parameterId, @float, false);
		}
	}

	// Token: 0x06002B2F RID: 11055 RVA: 0x001DE2E8 File Offset: 0x001DC4E8
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

	// Token: 0x04001CEA RID: 7402
	private List<UserVolumeLoopingUpdater.Entry> entries = new List<UserVolumeLoopingUpdater.Entry>();

	// Token: 0x04001CEB RID: 7403
	private string playerPref;

	// Token: 0x02000952 RID: 2386
	private struct Entry
	{
		// Token: 0x04001CEC RID: 7404
		public EventInstance ev;

		// Token: 0x04001CED RID: 7405
		public PARAMETER_ID parameterId;
	}
}
