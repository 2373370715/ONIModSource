using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public readonly struct MinionVoice {
    public MinionVoice(int voiceIndex) {
        this.voiceIndex = voiceIndex;
        voiceId         = (voiceIndex + 1).ToString("D2");
        isValid         = true;
    }

    public static MinionVoice ByPersonality(Personality personality) { return ByPersonality(personality.Id); }

    public static MinionVoice ByPersonality(string personalityId) {
        if (personalityId == "JORGE") return new MinionVoice(-2);

        if (personalityId == "MEEP") return new MinionVoice(2);

        MinionVoice minionVoice;
        if (!personalityVoiceMap.TryGetValue(personalityId, out minionVoice)) {
            minionVoice = Random();
            personalityVoiceMap.Add(personalityId, minionVoice);
        }

        return minionVoice;
    }

    public static MinionVoice Random() { return new MinionVoice(UnityEngine.Random.Range(0, 4)); }

    public static Option<MinionVoice> ByObject(Object unityObject) {
        var        gameObject = unityObject as GameObject;
        GameObject gameObject2;
        if (gameObject != null)
            gameObject2 = gameObject;
        else {
            var component = unityObject as Component;
            if (component != null)
                gameObject2 = component.gameObject;
            else
                gameObject2 = null;
        }

        if (gameObject2.IsNullOrDestroyed()) return Option.None;

        var componentInParent = gameObject2.GetComponentInParent<MinionVoiceProviderMB>();
        if (componentInParent.IsNullOrDestroyed()) return Option.None;

        return componentInParent.voice;
    }

    public string GetSoundAssetName(string localName) {
        Debug.Assert(isValid);
        var d                          = localName;
        if (localName.Contains(":")) d = localName.Split(':', StringSplitOptions.None)[0];
        return StringFormatter.Combine("DupVoc_", voiceId, "_", d);
    }

    public string GetSoundPath(string localName) { return GlobalAssets.GetSound(GetSoundAssetName(localName), true); }

    public void PlaySoundUI(string localName) {
        Debug.Assert(isValid);
        var soundPath = GetSoundPath(localName);
        try {
            if (SoundListenerController.Instance == null)
                KFMOD.PlayUISound(soundPath);
            else
                KFMOD.PlayOneShot(soundPath, SoundListenerController.Instance.transform.GetPosition());
        } catch { DebugUtil.LogWarningArgs("AUDIOERROR: Missing [" + soundPath + "]"); }
    }

    public readonly         int                             voiceIndex;
    public readonly         string                          voiceId;
    public readonly         bool                            isValid;
    private static readonly Dictionary<string, MinionVoice> personalityVoiceMap = new Dictionary<string, MinionVoice>();
}