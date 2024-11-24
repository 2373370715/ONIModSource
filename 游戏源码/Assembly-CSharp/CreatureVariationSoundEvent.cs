using System;

// Token: 0x02000911 RID: 2321
public class CreatureVariationSoundEvent : SoundEvent
{
	// Token: 0x0600293E RID: 10558 RVA: 0x000BACF5 File Offset: 0x000B8EF5
	public CreatureVariationSoundEvent(string file_name, string sound_name, int frame, bool do_load, bool is_looping, float min_interval, bool is_dynamic) : base(file_name, sound_name, frame, do_load, is_looping, min_interval, is_dynamic)
	{
	}

	// Token: 0x0600293F RID: 10559 RVA: 0x001D549C File Offset: 0x001D369C
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		string sound = base.sound;
		CreatureBrain component = behaviour.GetComponent<CreatureBrain>();
		if (component != null && !string.IsNullOrEmpty(component.symbolPrefix))
		{
			string sound2 = GlobalAssets.GetSound(StringFormatter.Combine(component.symbolPrefix, base.name), false);
			if (!string.IsNullOrEmpty(sound2))
			{
				sound = sound2;
			}
		}
		base.PlaySound(behaviour, sound);
	}
}
