using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200095E RID: 2398
public abstract class AudioSheets : ScriptableObject
{
	// Token: 0x06002B3C RID: 11068 RVA: 0x001DE388 File Offset: 0x001DC588
	public virtual void Initialize()
	{
		foreach (AudioSheet audioSheet in this.sheets)
		{
			foreach (AudioSheet.SoundInfo soundInfo in audioSheet.soundInfos)
			{
				if (DlcManager.IsContentSubscribed(soundInfo.RequiredDlcId))
				{
					string text = soundInfo.Type;
					if (text == null || text == "")
					{
						text = audioSheet.defaultType;
					}
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name0, soundInfo.Frame0, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name1, soundInfo.Frame1, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name2, soundInfo.Frame2, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name3, soundInfo.Frame3, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name4, soundInfo.Frame4, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name5, soundInfo.Frame5, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name6, soundInfo.Frame6, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name7, soundInfo.Frame7, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name8, soundInfo.Frame8, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name9, soundInfo.Frame9, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name10, soundInfo.Frame10, soundInfo.RequiredDlcId);
					this.CreateSound(soundInfo.File, soundInfo.Anim, text, soundInfo.MinInterval, soundInfo.Name11, soundInfo.Frame11, soundInfo.RequiredDlcId);
				}
			}
		}
	}

	// Token: 0x06002B3D RID: 11069 RVA: 0x001DE694 File Offset: 0x001DC894
	private void CreateSound(string file_name, string anim_name, string type, float min_interval, string sound_name, int frame, string dlcId)
	{
		if (string.IsNullOrEmpty(sound_name))
		{
			return;
		}
		HashedString key = file_name + "." + anim_name;
		AnimEvent animEvent = this.CreateSoundOfType(type, file_name, sound_name, frame, min_interval, dlcId);
		if (animEvent == null)
		{
			global::Debug.LogError("Unknown sound type: " + type);
			return;
		}
		List<AnimEvent> list = null;
		if (!this.events.TryGetValue(key, out list))
		{
			list = new List<AnimEvent>();
			this.events[key] = list;
		}
		list.Add(animEvent);
	}

	// Token: 0x06002B3E RID: 11070
	protected abstract AnimEvent CreateSoundOfType(string type, string file_name, string sound_name, int frame, float min_interval, string dlcId);

	// Token: 0x06002B3F RID: 11071 RVA: 0x001DE710 File Offset: 0x001DC910
	public List<AnimEvent> GetEvents(HashedString anim_id)
	{
		List<AnimEvent> result = null;
		this.events.TryGetValue(anim_id, out result);
		return result;
	}

	// Token: 0x04001D0F RID: 7439
	public List<AudioSheet> sheets = new List<AudioSheet>();

	// Token: 0x04001D10 RID: 7440
	public Dictionary<HashedString, List<AnimEvent>> events = new Dictionary<HashedString, List<AnimEvent>>();
}
