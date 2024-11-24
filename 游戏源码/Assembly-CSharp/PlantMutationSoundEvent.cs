using System;
using UnityEngine;

// Token: 0x02000931 RID: 2353
public class PlantMutationSoundEvent : SoundEvent
{
	// Token: 0x06002A7C RID: 10876 RVA: 0x000BACD9 File Offset: 0x000B8ED9
	public PlantMutationSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name, sound_name, frame, false, false, min_interval, true)
	{
	}

	// Token: 0x06002A7D RID: 10877 RVA: 0x001DACF8 File Offset: 0x001D8EF8
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		MutantPlant component = behaviour.controller.gameObject.GetComponent<MutantPlant>();
		Vector3 position = behaviour.position;
		if (component != null)
		{
			for (int i = 0; i < component.GetSoundEvents().Count; i++)
			{
				SoundEvent.PlayOneShot(component.GetSoundEvents()[i], position, 1f);
			}
		}
	}
}
