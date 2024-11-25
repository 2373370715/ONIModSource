using UnityEngine;

public abstract class KCollider2D : KMonoBehaviour, IRenderEveryTick {
    [SerializeField]
    public Vector2 _offset;

    private Extents                  cachedExtents;
    private HandleVector<int>.Handle partitionerEntry;

    public Vector2 offset {
        get => _offset;
        set {
            _offset = value;
            MarkDirty();
        }
    }

    public abstract Bounds bounds                    { get; }
    public          void   RenderEveryTick(float dt) { MarkDirty(); }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        autoRegisterSimRender = false;
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        Singleton<CellChangeMonitor>.Instance.RegisterMovementStateChanged(transform, OnMovementStateChanged);
        MarkDirty(true);
    }

    protected override void OnCleanUp() {
        base.OnCleanUp();
        Singleton<CellChangeMonitor>.Instance.UnregisterMovementStateChanged(transform, OnMovementStateChanged);
        GameScenePartitioner.Instance.Free(ref partitionerEntry);
    }

    public void MarkDirty(bool force = false) {
        var flag = force || partitionerEntry.IsValid();
        if (!flag) return;

        var extents = GetExtents();
        if (!force                                &&
            cachedExtents.x      == extents.x     &&
            cachedExtents.y      == extents.y     &&
            cachedExtents.width  == extents.width &&
            cachedExtents.height == extents.height)
            return;

        cachedExtents = extents;
        GameScenePartitioner.Instance.Free(ref partitionerEntry);
        if (flag)
            partitionerEntry
                = GameScenePartitioner.Instance.Add(name,
                                                    this,
                                                    cachedExtents,
                                                    GameScenePartitioner.Instance.collisionLayer,
                                                    null);
    }

    private void OnMovementStateChanged(bool is_moving) {
        if (is_moving) {
            MarkDirty();
            SimAndRenderScheduler.instance.Add(this);
            return;
        }

        SimAndRenderScheduler.instance.Remove(this);
    }

    private static void OnMovementStateChanged(Transform transform, bool is_moving) {
        transform.GetComponent<KCollider2D>().OnMovementStateChanged(is_moving);
    }

    public abstract bool    Intersects(Vector2 pos);
    public abstract Extents GetExtents();
}