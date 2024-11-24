using System;

// Token: 0x0200092E RID: 2350
public class MouthFlapSoundEvent : SoundEvent
{
	// Token: 0x06002A75 RID: 10869 RVA: 0x000BB9D8 File Offset: 0x000B9BD8
	public MouthFlapSoundEvent(string file_name, string sound_name, int frame, bool is_looping) : base(file_name, sound_name, frame, false, is_looping, (float)SoundEvent.IGNORE_INTERVAL, true)
	{
	}

	// Token: 0x06002A76 RID: 10870 RVA: 0x000BB9ED File Offset: 0x000B9BED
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		behaviour.controller.GetSMI<SpeechMonitor.Instance>().PlaySpeech(base.name, null);
	}
}
