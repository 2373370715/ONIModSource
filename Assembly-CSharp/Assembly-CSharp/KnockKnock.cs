public class KnockKnock : Activatable {
    private bool doorAnswered;

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        showProgressBar = false;
    }

    protected override bool OnWorkTick(WorkerBase worker, float dt) {
        if (!doorAnswered) workTimeRemaining += dt;
        return base.OnWorkTick(worker, dt);
    }

    public void AnswerDoor() {
        doorAnswered      = true;
        workTimeRemaining = 1f;
    }
}