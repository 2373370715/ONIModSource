using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x0200092D RID: 2349
public class MainMenuSoundEvent : SoundEvent
{
	// Token: 0x06002A73 RID: 10867 RVA: 0x000BB9C4 File Offset: 0x000B9BC4
	public MainMenuSoundEvent(string file_name, string sound_name, int frame) : base(file_name, sound_name, frame, true, false, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
	}

	// Token: 0x06002A74 RID: 10868 RVA: 0x001DA9A4 File Offset: 0x001D8BA4
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		EventInstance instance = KFMOD.BeginOneShot(base.sound, Vector3.zero, 1f);
		if (instance.isValid())
		{
			instance.setParameterByName("frame", (float)base.frame, false);
			KFMOD.EndOneShot(instance);
		}
	}
}
