using System.Collections.Generic;
using FMOD.Studio;

internal class UpdatePercentCompleteParameter : LoopingSoundParameterUpdater {
    private readonly List<Entry> entries = new List<Entry>();
    public UpdatePercentCompleteParameter() : base("percentComplete") { }

    public override void Add(Sound sound) {
        var item = new Entry {
            worker      = sound.transform.GetComponent<WorkerBase>(),
            ev          = sound.ev,
            parameterId = sound.description.GetParameterId(parameter)
        };

        entries.Add(item);
    }

    public override void Update(float dt) {
        foreach (var entry in entries)
            if (!(entry.worker == null)) {
                var workable = entry.worker.GetWorkable();
                if (!(workable == null)) {
                    var percentComplete = workable.GetPercentComplete();
                    var ev              = entry.ev;
                    ev.setParameterByID(entry.parameterId, percentComplete);
                }
            }
    }

    public override void Remove(Sound sound) {
        for (var i = 0; i < entries.Count; i++)
            if (entries[i].ev.handle == sound.ev.handle) {
                entries.RemoveAt(i);
                return;
            }
    }

    private struct Entry {
        public WorkerBase    worker;
        public EventInstance ev;
        public PARAMETER_ID  parameterId;
    }
}