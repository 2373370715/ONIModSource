using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/AstronautTrainingCenter")]
public class AstronautTrainingCenter : Workable {
    private Chore              chore;
    public  float              daysToMasterRole;
    public  Chore.Precondition IsNotMarkedForDeconstruction;

    public AstronautTrainingCenter() {
        var isNotMarkedForDeconstruction = default(Chore.Precondition);
        isNotMarkedForDeconstruction.id          = "IsNotMarkedForDeconstruction";
        isNotMarkedForDeconstruction.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MARKED_FOR_DECONSTRUCTION;
        isNotMarkedForDeconstruction.fn = delegate(ref Chore.Precondition.Context context, object data) {
                                              var deconstructable = data as Deconstructable;
                                              return deconstructable == null ||
                                                     !deconstructable.IsMarkedForDeconstruction();
                                          };

        IsNotMarkedForDeconstruction = isNotMarkedForDeconstruction;
        base..ctor();
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        chore = CreateChore();
    }

    private Chore CreateChore() {
        return new WorkChore<AstronautTrainingCenter>(Db.Get().ChoreTypes.Train,
                                                      this,
                                                      null,
                                                      true,
                                                      null,
                                                      null,
                                                      null,
                                                      false);
    }

    protected override void OnStartWork(Worker worker) {
        base.OnStartWork(worker);
        GetComponent<Operational>().SetActive(true);
    }

    protected override bool OnWorkTick(Worker worker, float dt) {
        // worker == null;
        return true;
    }

    protected override void OnCompleteWork(Worker worker) {
        base.OnCompleteWork(worker);
        if (chore != null && !chore.isComplete) chore.Cancel("completed but not complete??");
        chore = CreateChore();
    }

    protected override void OnStopWork(Worker worker) {
        base.OnStopWork(worker);
        GetComponent<Operational>().SetActive(false);
    }

    public override float GetPercentComplete() {
        worker == null;
        return 0f;
    }
}