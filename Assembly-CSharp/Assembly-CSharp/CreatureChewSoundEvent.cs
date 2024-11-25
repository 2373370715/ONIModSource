public class CreatureChewSoundEvent : SoundEvent {
    private const           string FMOD_PARAM_IS_BABY_ID = "isBaby";
    private static readonly string DEFAULT_CHEW_SOUND    = "Rock";

    public CreatureChewSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name,
     sound_name,
     frame,
     false,
     false,
     min_interval,
     true) { }

    public override void OnPlay(AnimEventManager.EventPlayerData behaviour) {
        var sound      = GlobalAssets.GetSound(StringFormatter.Combine(name, "_", GetChewSound(behaviour)));
        var gameObject = behaviour.controller.gameObject;
        objectIsSelectedAndVisible = ObjectIsSelectedAndVisible(gameObject);
        if (objectIsSelectedAndVisible || ShouldPlaySound(behaviour.controller, sound, looping, isDynamic)) {
            var vector = behaviour.position;
            vector.z = 0f;
            if (objectIsSelectedAndVisible) vector = AudioHighlightListenerPosition(vector);
            var instance                           = BeginOneShot(sound, vector, GetVolume(objectIsSelectedAndVisible));
            if (behaviour.controller.gameObject.GetDef<BabyMonitor.Def>() != null)
                instance.setParameterByName("isBaby", 1f);

            EndOneShot(instance);
        }
    }

    private static string GetChewSound(AnimEventManager.EventPlayerData behaviour) {
        var result = DEFAULT_CHEW_SOUND;
        var smi = behaviour.controller.GetSMI<EatStates.Instance>();
        if (smi != null) {
            var latestMealElement = smi.GetLatestMealElement();
            if (latestMealElement != null) {
                var creatureChewSound = latestMealElement.substance.GetCreatureChewSound();
                if (!string.IsNullOrEmpty(creatureChewSound)) result = creatureChewSound;
            }
        }

        return result;
    }
}