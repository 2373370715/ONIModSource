using System;
using FMOD.Studio;
using UnityEngine;

public class MainMenuSoundEvent : SoundEvent
{
	public MainMenuSoundEvent(string file_name, string sound_name, int frame) : base(file_name, sound_name, frame, true, false, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
	}

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
