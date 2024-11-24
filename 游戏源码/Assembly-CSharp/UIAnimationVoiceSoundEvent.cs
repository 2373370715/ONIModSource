using UnityEngine;

public class UIAnimationVoiceSoundEvent : SoundEvent
{
	private string actualSoundName;

	private string lastPlayedLoopingSoundPath;

	public UIAnimationVoiceSoundEvent(string file_name, string sound_name, int frame, bool looping)
		: base(file_name, sound_name, frame, do_load: false, looping, SoundEvent.IGNORE_INTERVAL, is_dynamic: false)
	{
		actualSoundName = sound_name;
	}

	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		PlaySound(behaviour);
	}

	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		string soundPath = MinionVoice.ByObject(behaviour.controller).UnwrapOr(MinionVoice.Random(), $"Couldn't find MinionVoice on UI {behaviour.controller}, falling back to random voice").GetSoundPath(actualSoundName);
		if (actualSoundName.Contains(":"))
		{
			float num = float.Parse(actualSoundName.Split(':')[1]);
			if ((float)Random.Range(0, 100) > num)
			{
				return;
			}
		}
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component == null)
			{
				Debug.Log(behaviour.name + " (UI Object) is missing LoopingSounds component.");
			}
			else if (!component.StartSound(soundPath, pause_on_game_pause: false, enable_culling: false, enable_camera_scaled_position: false))
			{
				DebugUtil.LogWarningArgs($"SoundEvent has invalid sound [{soundPath}] on behaviour [{behaviour.name}]");
			}
			lastPlayedLoopingSoundPath = soundPath;
			return;
		}
		try
		{
			if (SoundListenerController.Instance == null)
			{
				KFMOD.PlayUISound(soundPath);
			}
			else
			{
				KFMOD.PlayOneShot(soundPath, SoundListenerController.Instance.transform.GetPosition());
			}
		}
		catch
		{
			DebugUtil.LogWarningArgs("AUDIOERROR: Missing [" + soundPath + "]");
		}
	}

	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null && lastPlayedLoopingSoundPath != null)
			{
				component.StopSound(lastPlayedLoopingSoundPath);
			}
		}
		lastPlayedLoopingSoundPath = null;
	}
}
