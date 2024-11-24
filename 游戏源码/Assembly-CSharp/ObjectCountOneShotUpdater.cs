using System;
using System.Collections.Generic;

// Token: 0x02000939 RID: 2361
internal class ObjectCountOneShotUpdater : OneShotSoundParameterUpdater
{
	// Token: 0x06002AB5 RID: 10933 RVA: 0x000BBBC8 File Offset: 0x000B9DC8
	public ObjectCountOneShotUpdater() : base("objectCount")
	{
	}

	// Token: 0x06002AB6 RID: 10934 RVA: 0x000BBBE5 File Offset: 0x000B9DE5
	public override void Update(float dt)
	{
		this.soundCounts.Clear();
	}

	// Token: 0x06002AB7 RID: 10935 RVA: 0x001DBA14 File Offset: 0x001D9C14
	public override void Play(OneShotSoundParameterUpdater.Sound sound)
	{
		UpdateObjectCountParameter.Settings settings = UpdateObjectCountParameter.GetSettings(sound.path, sound.description);
		int num = 0;
		this.soundCounts.TryGetValue(sound.path, out num);
		num = (this.soundCounts[sound.path] = num + 1);
		UpdateObjectCountParameter.ApplySettings(sound.ev, num, settings);
	}

	// Token: 0x04001C67 RID: 7271
	private Dictionary<HashedString, int> soundCounts = new Dictionary<HashedString, int>();
}
