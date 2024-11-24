using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000910 RID: 2320
public class CreatureChewSoundEvent : SoundEvent
{
	// Token: 0x0600293A RID: 10554 RVA: 0x000BACD9 File Offset: 0x000B8ED9
	public CreatureChewSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, false, false, min_interval, true)
	{
	}

	// Token: 0x0600293B RID: 10555 RVA: 0x001D5390 File Offset: 0x001D3590
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		string sound = GlobalAssets.GetSound(StringFormatter.Combine(base.name, "_", CreatureChewSoundEvent.GetChewSound(behaviour)), false);
		GameObject gameObject = behaviour.controller.gameObject;
		base.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
		if (base.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, sound, base.looping, this.isDynamic))
		{
			Vector3 vector = behaviour.position;
			vector.z = 0f;
			if (base.objectIsSelectedAndVisible)
			{
				vector = SoundEvent.AudioHighlightListenerPosition(vector);
			}
			EventInstance instance = SoundEvent.BeginOneShot(sound, vector, SoundEvent.GetVolume(base.objectIsSelectedAndVisible), false);
			if (behaviour.controller.gameObject.GetDef<BabyMonitor.Def>() != null)
			{
				instance.setParameterByName("isBaby", 1f, false);
			}
			SoundEvent.EndOneShot(instance);
		}
	}

	// Token: 0x0600293C RID: 10556 RVA: 0x001D5458 File Offset: 0x001D3658
	private static string GetChewSound(AnimEventManager.EventPlayerData behaviour)
	{
		string result = CreatureChewSoundEvent.DEFAULT_CHEW_SOUND;
		EatStates.Instance smi = behaviour.controller.GetSMI<EatStates.Instance>();
		if (smi != null)
		{
			Element latestMealElement = smi.GetLatestMealElement();
			if (latestMealElement != null)
			{
				string creatureChewSound = latestMealElement.substance.GetCreatureChewSound();
				if (!string.IsNullOrEmpty(creatureChewSound))
				{
					result = creatureChewSound;
				}
			}
		}
		return result;
	}

	// Token: 0x04001B86 RID: 7046
	private static string DEFAULT_CHEW_SOUND = "Rock";

	// Token: 0x04001B87 RID: 7047
	private const string FMOD_PARAM_IS_BABY_ID = "isBaby";
}
