using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000013 RID: 19
public readonly struct MinionVoice
{
	// Token: 0x06000044 RID: 68 RVA: 0x0013EC64 File Offset: 0x0013CE64
	public MinionVoice(int voiceIndex)
	{
		this.voiceIndex = voiceIndex;
		this.voiceId = (voiceIndex + 1).ToString("D2");
		this.isValid = true;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000A5EE9 File Offset: 0x000A40E9
	public static MinionVoice ByPersonality(Personality personality)
	{
		return MinionVoice.ByPersonality(personality.Id);
	}

	// Token: 0x06000046 RID: 70 RVA: 0x0013EC98 File Offset: 0x0013CE98
	public static MinionVoice ByPersonality(string personalityId)
	{
		if (personalityId == "JORGE")
		{
			return new MinionVoice(-2);
		}
		if (personalityId == "MEEP")
		{
			return new MinionVoice(2);
		}
		MinionVoice minionVoice;
		if (!MinionVoice.personalityVoiceMap.TryGetValue(personalityId, out minionVoice))
		{
			minionVoice = MinionVoice.Random();
			MinionVoice.personalityVoiceMap.Add(personalityId, minionVoice);
		}
		return minionVoice;
	}

	// Token: 0x06000047 RID: 71 RVA: 0x000A5EF6 File Offset: 0x000A40F6
	public static MinionVoice Random()
	{
		return new MinionVoice(UnityEngine.Random.Range(0, 4));
	}

	// Token: 0x06000048 RID: 72 RVA: 0x0013ECF0 File Offset: 0x0013CEF0
	public static Option<MinionVoice> ByObject(UnityEngine.Object unityObject)
	{
		GameObject gameObject = unityObject as GameObject;
		GameObject gameObject2;
		if (gameObject != null)
		{
			gameObject2 = gameObject;
		}
		else
		{
			Component component = unityObject as Component;
			if (component != null)
			{
				gameObject2 = component.gameObject;
			}
			else
			{
				gameObject2 = null;
			}
		}
		if (gameObject2.IsNullOrDestroyed())
		{
			return Option.None;
		}
		MinionVoiceProviderMB componentInParent = gameObject2.GetComponentInParent<MinionVoiceProviderMB>();
		if (componentInParent.IsNullOrDestroyed())
		{
			return Option.None;
		}
		return componentInParent.voice;
	}

	// Token: 0x06000049 RID: 73 RVA: 0x0013ED54 File Offset: 0x0013CF54
	public string GetSoundAssetName(string localName)
	{
		global::Debug.Assert(this.isValid);
		string d = localName;
		if (localName.Contains(":"))
		{
			d = localName.Split(':', StringSplitOptions.None)[0];
		}
		return StringFormatter.Combine("DupVoc_", this.voiceId, "_", d);
	}

	// Token: 0x0600004A RID: 74 RVA: 0x000A5F04 File Offset: 0x000A4104
	public string GetSoundPath(string localName)
	{
		return GlobalAssets.GetSound(this.GetSoundAssetName(localName), true);
	}

	// Token: 0x0600004B RID: 75 RVA: 0x0013EDA0 File Offset: 0x0013CFA0
	public void PlaySoundUI(string localName)
	{
		global::Debug.Assert(this.isValid);
		string soundPath = this.GetSoundPath(localName);
		try
		{
			if (SoundListenerController.Instance == null)
			{
				KFMOD.PlayUISound(soundPath);
			}
			else
			{
				KFMOD.PlayOneShot(soundPath, SoundListenerController.Instance.transform.GetPosition(), 1f);
			}
		}
		catch
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"AUDIOERROR: Missing [" + soundPath + "]"
			});
		}
	}

	// Token: 0x0400003C RID: 60
	public readonly int voiceIndex;

	// Token: 0x0400003D RID: 61
	public readonly string voiceId;

	// Token: 0x0400003E RID: 62
	public readonly bool isValid;

	// Token: 0x0400003F RID: 63
	private static Dictionary<string, MinionVoice> personalityVoiceMap = new Dictionary<string, MinionVoice>();
}
