using System;
using System.Collections.Generic;

public class SoundEventVolumeCache : Singleton<SoundEventVolumeCache>
{
			public static SoundEventVolumeCache instance
	{
		get
		{
			return Singleton<SoundEventVolumeCache>.Instance;
		}
	}

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

		public EffectorValues GetVolume(string animFile, string eventName)
	{
		HashedString key = new HashedString(animFile + ":" + eventName);
		if (!this.volumeCache.ContainsKey(key))
		{
			return default(EffectorValues);
		}
		return this.volumeCache[key];
	}

		public Dictionary<HashedString, EffectorValues> volumeCache = new Dictionary<HashedString, EffectorValues>();
}
