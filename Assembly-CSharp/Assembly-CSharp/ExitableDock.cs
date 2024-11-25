public class ExitableDock : Workable {
    private static readonly HashedString[] WORK_ANIMS = { "exit_dock" };

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        workAnims            = WORK_ANIMS;
        workAnimPlayMode     = KAnim.PlayMode.Once;
        synchronizeAnims     = true;
        triggerWorkReactions = false;
        workLayer            = Grid.SceneLayer.BuildingUse;
    }

    protected override void OnCompleteWork(WorkerBase worker) {
        base.OnCompleteWork(worker);
        worker.GetComponent<RemoteWorkerSM>().Docked = false;
    }
}