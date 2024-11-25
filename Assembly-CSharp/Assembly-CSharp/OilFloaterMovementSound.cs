internal class OilFloaterMovementSound : KMonoBehaviour {
    private static readonly EventSystem.IntraObjectHandler<OilFloaterMovementSound> OnObjectMovementStateChangedDelegate
        = new EventSystem.IntraObjectHandler<OilFloaterMovementSound>(delegate(OilFloaterMovementSound component,
                                                                               object                  data) {
                                                                          component.OnObjectMovementStateChanged(data);
                                                                      });

    public bool   isMoving;
    public bool   isPlayingSound;
    public string sound;

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        sound = GlobalAssets.GetSound(sound);
        Subscribe(1027377649, OnObjectMovementStateChangedDelegate);
        Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(transform,
                                                                         OnCellChanged,
                                                                         "OilFloaterMovementSound");
    }

    private void OnObjectMovementStateChanged(object data) {
        var gameHashes = (GameHashes)data;
        isMoving = gameHashes == GameHashes.ObjectMovementWakeUp;
        UpdateSound();
    }

    private void OnCellChanged() { UpdateSound(); }

    private void UpdateSound() {
        var flag = isMoving && GetComponent<Navigator>().CurrentNavType != NavType.Swim;
        if (flag == isPlayingSound) return;

        var component = GetComponent<LoopingSounds>();
        if (flag)
            component.StartSound(sound);
        else
            component.StopSound(sound);

        isPlayingSound = flag;
    }

    protected override void OnCleanUp() {
        base.OnCleanUp();
        Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(transform, OnCellChanged);
    }
}