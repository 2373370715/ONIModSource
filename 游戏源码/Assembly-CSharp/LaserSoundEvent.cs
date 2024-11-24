using System;

// Token: 0x0200092C RID: 2348
public class LaserSoundEvent : SoundEvent
{
	// Token: 0x06002A72 RID: 10866 RVA: 0x000BB99E File Offset: 0x000B9B9E
	public LaserSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, true, true, min_interval, false)
	{
		base.noiseValues = SoundEventVolumeCache.instance.GetVolume("LaserSoundEvent", sound_name);
	}
}
