using System.Collections.Generic;
using UnityEngine;

public readonly struct MinionVoice
{
	public readonly int voiceIndex;

	public readonly string voiceId;

	public readonly bool isValid;

	private static Dictionary<string, MinionVoice> personalityVoiceMap = new Dictionary<string, MinionVoice>();

	public MinionVoice(int voiceIndex)
	{
		this.voiceIndex = voiceIndex;
		voiceId = (voiceIndex + 1).ToString("D2");
		isValid = true;
	}

	public static MinionVoice ByPersonality(Personality personality)
	{
		return ByPersonality(personality.Id);
	}

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
		if (!personalityVoiceMap.TryGetValue(personalityId, out var value))
		{
			value = Random();
			personalityVoiceMap.Add(personalityId, value);
		}
		return value;
	}

	public static MinionVoice Random()
	{
		return new MinionVoice(UnityEngine.Random.Range(0, 4));
	}

	public static Option<MinionVoice> ByObject(Object unityObject)
	{
		GameObject gameObject2 = ((!(unityObject is GameObject gameObject)) ? ((!(unityObject is Component component)) ? null : component.gameObject) : gameObject);
		if (!gameObject2.IsNullOrDestroyed())
		{
			MinionVoiceProviderMB componentInParent = gameObject2.GetComponentInParent<MinionVoiceProviderMB>();
			if (componentInParent.IsNullOrDestroyed())
			{
				return Option.None;
			}
			return componentInParent.voice;
		}
		return Option.None;
	}

	public string GetSoundAssetName(string localName)
	{
		Debug.Assert(isValid);
		string d = localName;
		if (localName.Contains(":"))
		{
			d = localName.Split(':')[0];
		}
		return StringFormatter.Combine("DupVoc_", voiceId, "_", d);
	}

	public string GetSoundPath(string localName)
	{
		return GlobalAssets.GetSound(GetSoundAssetName(localName), force_no_warning: true);
	}

	public void PlaySoundUI(string localName)
	{
		Debug.Assert(isValid);
		string soundPath = GetSoundPath(localName);
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
}
