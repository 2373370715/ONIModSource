public class UIAnimationSoundEvent : SoundEvent
{
	public UIAnimationSoundEvent(string file_name, string sound_name, int frame, bool looping)
		: base(file_name, sound_name, frame, do_load: true, looping, SoundEvent.IGNORE_INTERVAL, is_dynamic: false)
	{
	}

	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		PlaySound(behaviour);
	}

	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component == null)
			{
				Debug.Log(behaviour.name + " (UI Object) is missing LoopingSounds component.");
			}
			else if (!component.StartSound(base.sound, pause_on_game_pause: false, enable_culling: false, enable_camera_scaled_position: false))
			{
				DebugUtil.LogWarningArgs($"SoundEvent has invalid sound [{base.sound}] on behaviour [{behaviour.name}]");
			}
			return;
		}
		try
		{
			if (SoundListenerController.Instance == null)
			{
				KFMOD.PlayUISound(base.sound);
			}
			else
			{
				KFMOD.PlayOneShot(base.sound, SoundListenerController.Instance.transform.GetPosition());
			}
		}
		catch
		{
			DebugUtil.LogWarningArgs("AUDIOERROR: Missing [" + base.sound + "]");
		}
	}

	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(base.sound);
			}
		}
	}
}
