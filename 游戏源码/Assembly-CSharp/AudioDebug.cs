using System;
using UnityEngine;

// Token: 0x0200094A RID: 2378
[AddComponentMenu("KMonoBehaviour/scripts/AudioDebug")]
public class AudioDebug : KMonoBehaviour
{
	// Token: 0x06002AF8 RID: 11000 RVA: 0x000BBF04 File Offset: 0x000BA104
	public static AudioDebug Get()
	{
		return AudioDebug.instance;
	}

	// Token: 0x06002AF9 RID: 11001 RVA: 0x000BBF0B File Offset: 0x000BA10B
	protected override void OnPrefabInit()
	{
		AudioDebug.instance = this;
	}

	// Token: 0x06002AFA RID: 11002 RVA: 0x000BBF13 File Offset: 0x000BA113
	public void ToggleMusic()
	{
		if (Game.Instance != null)
		{
			Game.Instance.SetMusicEnabled(this.musicEnabled);
		}
		this.musicEnabled = !this.musicEnabled;
	}

	// Token: 0x04001CB5 RID: 7349
	private static AudioDebug instance;

	// Token: 0x04001CB6 RID: 7350
	public bool musicEnabled;

	// Token: 0x04001CB7 RID: 7351
	public bool debugSoundEvents;

	// Token: 0x04001CB8 RID: 7352
	public bool debugFloorSounds;

	// Token: 0x04001CB9 RID: 7353
	public bool debugGameEventSounds;

	// Token: 0x04001CBA RID: 7354
	public bool debugNotificationSounds;

	// Token: 0x04001CBB RID: 7355
	public bool debugVoiceSounds;
}
