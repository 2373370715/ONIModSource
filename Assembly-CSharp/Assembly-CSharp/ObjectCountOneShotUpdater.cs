using System.Collections.Generic;

internal class ObjectCountOneShotUpdater : OneShotSoundParameterUpdater {
    private readonly Dictionary<HashedString, int> soundCounts = new Dictionary<HashedString, int>();
    public ObjectCountOneShotUpdater() : base("objectCount") { }
    public override void Update(float dt) { soundCounts.Clear(); }

    public override void Play(Sound sound) {
        var settings = UpdateObjectCountParameter.GetSettings(sound.path, sound.description);
        var num      = 0;
        soundCounts.TryGetValue(sound.path, out num);
        num = soundCounts[sound.path] = num + 1;
        UpdateObjectCountParameter.ApplySettings(sound.ev, num, settings);
    }
}