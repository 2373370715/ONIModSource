using System;
using System.Collections.Generic;

// Token: 0x02000934 RID: 2356
public class SoundEventVolumeCache : Singleton<SoundEventVolumeCache>
{
	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06002A84 RID: 10884 RVA: 0x000BBA41 File Offset: 0x000B9C41
	public static SoundEventVolumeCache instance
	{
		get
		{
			return Singleton<SoundEventVolumeCache>.Instance;
		}
	}

	// Token: 0x06002A85 RID: 10885 RVA: 0x001DB14C File Offset: 0x001D934C
	public void AddVolume(string animFile, string eventName, EffectorValues vals)
	{
		HashedString key = new HashedString(animFile + ":" + eventName);
		if (!this.volumeCache.ContainsKey(key))
		{
			this.volumeCache.Add(key, vals);
			return;
		}
		this.volumeCache[key] = vals;
	}

	// Token: 0x06002A86 RID: 10886 RVA: 0x001DB198 File Offset: 0x001D9398
	public EffectorValues GetVolume(string animFile, string eventName)
	{
		HashedString key = new HashedString(animFile + ":" + eventName);
		if (!this.volumeCache.ContainsKey(key))
		{
			return default(EffectorValues);
		}
		return this.volumeCache[key];
	}

	// Token: 0x04001C52 RID: 7250
	public Dictionary<HashedString, EffectorValues> volumeCache = new Dictionary<HashedString, EffectorValues>();
}
