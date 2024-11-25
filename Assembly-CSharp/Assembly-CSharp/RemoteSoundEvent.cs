using System;

[Serializable]
public class RemoteSoundEvent : SoundEvent {
    private const string STATE_PARAMETER = "State";

    public RemoteSoundEvent(string file_name, string sound_name, int frame, float min_interval) : base(file_name,
     sound_name,
     frame,
     true,
     false,
     min_interval,
     false) { }

    public override void PlaySound(AnimEventManager.EventPlayerData behaviour) {
        var vector = behaviour.position;
        vector.z = 0f;
        if (ObjectIsSelectedAndVisible(behaviour.controller.gameObject))
            vector = AudioHighlightListenerPosition(vector);

        var workable = behaviour.GetComponent<WorkerBase>().GetWorkable();
        if (workable != null) {
            var component = workable.GetComponent<Toggleable>();
            if (component != null) {
                var toggleHandlerForWorker = component.GetToggleHandlerForWorker(behaviour.GetComponent<WorkerBase>());
                var value = 1f;
                if (toggleHandlerForWorker != null && toggleHandlerForWorker.IsHandlerOn()) value = 0f;
                if (objectIsSelectedAndVisible ||
                    ShouldPlaySound(behaviour.controller, sound, soundHash, looping, isDynamic)) {
                    var instance = BeginOneShot(sound, vector, GetVolume(objectIsSelectedAndVisible));
                    instance.setParameterByName("State", value);
                    EndOneShot(instance);
                }
            }
        }
    }
}