﻿using System;
using FMOD.Studio;
using UnityEngine;

[Serializable]
public class RemoteSoundEvent : SoundEvent
{
	public RemoteSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, true, false, min_interval, false)
	{
	}

	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		Vector3 vector = behaviour.GetComponent<Transform>().GetPosition();
		vector.z = 0f;
		if (SoundEvent.ObjectIsSelectedAndVisible(behaviour.controller.gameObject))
		{
			vector = SoundEvent.AudioHighlightListenerPosition(vector);
		}
		Workable workable = behaviour.GetComponent<Worker>().workable;
		if (workable != null)
		{
			Toggleable component = workable.GetComponent<Toggleable>();
			if (component != null)
			{
				IToggleHandler toggleHandlerForWorker = component.GetToggleHandlerForWorker(behaviour.GetComponent<Worker>());
				float value = 1f;
				if (toggleHandlerForWorker != null && toggleHandlerForWorker.IsHandlerOn())
				{
					value = 0f;
				}
				if (base.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, base.sound, base.soundHash, base.looping, this.isDynamic))
				{
					EventInstance instance = SoundEvent.BeginOneShot(base.sound, vector, SoundEvent.GetVolume(base.objectIsSelectedAndVisible), false);
					instance.setParameterByName("State", value, false);
					SoundEvent.EndOneShot(instance);
				}
			}
		}
	}

	private const string STATE_PARAMETER = "State";
}
