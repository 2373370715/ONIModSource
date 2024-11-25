using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class KBatchedAnimUpdater : Singleton<KBatchedAnimUpdater> {
    public enum RegistrationState {
        Registered,
        PendingRemoval,
        Unregistered
    }

    private const int VISIBLE_BORDER = 4;
    private const int CHUNKS_TO_CLEAN_PER_TICK = 16;
    public static readonly Vector2I INVALID_CHUNK_ID = Vector2I.minusone;
    private static readonly Vector2 VISIBLE_RANGE_SCALE = new Vector2(1.5f, 1.5f);
    private readonly LinkedList<KBatchedAnimController> alwaysUpdateList = new LinkedList<KBatchedAnimController>();
    private int cleanUpChunkIndex;

    private readonly Dictionary<int, ControllerChunkInfo> controllerChunkInfos
        = new Dictionary<int, ControllerChunkInfo>();

    private Dictionary<int, KBatchedAnimController>[,] controllerGrid;

    private readonly Dictionary<int, MovingControllerInfo> movingControllerInfos
        = new Dictionary<int, MovingControllerInfo>();

    private          bool[,]                            previouslyVisibleChunkGrid;
    private          List<Vector2I>                     previouslyVisibleChunks = new List<Vector2I>();
    private readonly List<RegistrationInfo>             queuedRegistrations = new List<RegistrationInfo>();
    private readonly LinkedList<KBatchedAnimController> updateList = new LinkedList<KBatchedAnimController>();
    private          Vector2I                           vis_chunk_max = Vector2I.zero;
    private          Vector2I                           vis_chunk_min = Vector2I.zero;
    private          bool[,]                            visibleChunkGrid;
    private          List<Vector2I>                     visibleChunks = new List<Vector2I>();

    public void InitializeGrid() {
        Clear();
        var visibleSize = GetVisibleSize();
        var num         = (visibleSize.x + 32 - 1) / 32;
        var num2        = (visibleSize.y + 32 - 1) / 32;
        controllerGrid = new Dictionary<int, KBatchedAnimController>[num, num2];
        for (var i = 0; i < num2; i++) {
            for (var j = 0; j < num; j++) controllerGrid[j, i] = new Dictionary<int, KBatchedAnimController>();
        }

        visibleChunks.Clear();
        previouslyVisibleChunks.Clear();
        previouslyVisibleChunkGrid = new bool[num, num2];
        visibleChunkGrid           = new bool[num, num2];
        controllerChunkInfos.Clear();
        movingControllerInfos.Clear();
    }

    public Vector2I GetVisibleSize() {
        if (CameraController.Instance != null) {
            Vector2I vector2I;
            Vector2I vector2I2;
            CameraController.Instance.GetWorldCamera(out vector2I, out vector2I2);
            return new Vector2I((int)((vector2I2.x + vector2I.x) * VISIBLE_RANGE_SCALE.x),
                                (int)((vector2I2.y + vector2I.y) * VISIBLE_RANGE_SCALE.y));
        }

        return new Vector2I((int)(Grid.WidthInCells  * VISIBLE_RANGE_SCALE.x),
                            (int)(Grid.HeightInCells * VISIBLE_RANGE_SCALE.y));
    }

    public event System.Action OnClear;

    public void Clear() {
        foreach (var kbatchedAnimController in updateList)
            if (kbatchedAnimController != null)
                Object.DestroyImmediate(kbatchedAnimController);

        updateList.Clear();
        foreach (var kbatchedAnimController2 in alwaysUpdateList)
            if (kbatchedAnimController2 != null)
                Object.DestroyImmediate(kbatchedAnimController2);

        alwaysUpdateList.Clear();
        queuedRegistrations.Clear();
        visibleChunks.Clear();
        previouslyVisibleChunks.Clear();
        controllerGrid             = null;
        previouslyVisibleChunkGrid = null;
        visibleChunkGrid           = null;
        var onClear = OnClear;
        if (onClear == null) return;

        onClear();
    }

    public void UpdateRegister(KBatchedAnimController controller) {
        switch (controller.updateRegistrationState) {
            case RegistrationState.Registered:
                break;
            case RegistrationState.PendingRemoval:
                controller.updateRegistrationState = RegistrationState.Registered;
                return;
            case RegistrationState.Unregistered:
                (controller.visibilityType == KAnimControllerBase.VisibilityType.Always ? alwaysUpdateList : updateList)
                    .AddLast(controller);

                controller.updateRegistrationState = RegistrationState.Registered;
                break;
            default:
                return;
        }
    }

    public void UpdateUnregister(KBatchedAnimController controller) {
        switch (controller.updateRegistrationState) {
            case RegistrationState.Registered:
                controller.updateRegistrationState = RegistrationState.PendingRemoval;
                break;
            case RegistrationState.PendingRemoval:
            case RegistrationState.Unregistered:
                break;
            default:
                return;
        }
    }

    public void VisibilityRegister(KBatchedAnimController controller) {
        queuedRegistrations.Add(new RegistrationInfo {
            transformId          = controller.transform.GetInstanceID(),
            controllerInstanceId = controller.GetInstanceID(),
            controller           = controller,
            register             = true
        });
    }

    public void VisibilityUnregister(KBatchedAnimController controller) {
        if (App.IsExiting) return;

        queuedRegistrations.Add(new RegistrationInfo {
            transformId          = controller.transform.GetInstanceID(),
            controllerInstanceId = controller.GetInstanceID(),
            controller           = controller,
            register             = false
        });
    }

    private Dictionary<int, KBatchedAnimController> GetControllerMap(Vector2I chunk_xy) {
        Dictionary<int, KBatchedAnimController> result = null;
        if (controllerGrid != null                       &&
            0              <= chunk_xy.x                 &&
            chunk_xy.x     < controllerGrid.GetLength(0) &&
            0              <= chunk_xy.y                 &&
            chunk_xy.y     < controllerGrid.GetLength(1))
            result = controllerGrid[chunk_xy.x, chunk_xy.y];

        return result;
    }

    public void LateUpdate() {
        ProcessMovingAnims();
        UpdateVisibility();
        ProcessRegistrations();
        CleanUp();
        var num   = Time.unscaledDeltaTime;
        var count = alwaysUpdateList.Count;
        UpdateRegisteredAnims(alwaysUpdateList, num);
        if (DoGridProcessing()) {
            num = Time.deltaTime;
            if (num > 0f) {
                var count2 = updateList.Count;
                UpdateRegisteredAnims(updateList, num);
            }
        }
    }

    private static void UpdateRegisteredAnims(LinkedList<KBatchedAnimController> list, float dt) {
        LinkedListNode<KBatchedAnimController> next;
        for (var linkedListNode = list.First; linkedListNode != null; linkedListNode = next) {
            next = linkedListNode.Next;
            var value = linkedListNode.Value;
            if (value == null)
                list.Remove(linkedListNode);
            else if (value.updateRegistrationState != RegistrationState.Registered) {
                value.updateRegistrationState = RegistrationState.Unregistered;
                list.Remove(linkedListNode);
            } else if (value.forceUseGameTime)
                value.UpdateAnim(Time.deltaTime);
            else
                value.UpdateAnim(dt);
        }
    }

    public bool IsChunkVisible(Vector2I chunk_xy) { return visibleChunkGrid[chunk_xy.x, chunk_xy.y]; }

    public void GetVisibleArea(out Vector2I vis_chunk_min, out Vector2I vis_chunk_max) {
        vis_chunk_min = this.vis_chunk_min;
        vis_chunk_max = this.vis_chunk_max;
    }

    public static Vector2I PosToChunkXY(Vector3 pos) { return KAnimBatchManager.CellXYToChunkXY(Grid.PosToXY(pos)); }

    private void UpdateVisibility() {
        if (!DoGridProcessing()) return;

        Vector2I vector2I;
        Vector2I vector2I2;
        Grid.GetVisibleCellRangeInActiveWorld(out vector2I, out vector2I2);
        vis_chunk_min   = new Vector2I(vector2I.x  / 32, vector2I.y  / 32);
        vis_chunk_max   = new Vector2I(vector2I2.x / 32, vector2I2.y / 32);
        vis_chunk_max.x = Math.Min(vis_chunk_max.x, controllerGrid.GetLength(0) - 1);
        vis_chunk_max.y = Math.Min(vis_chunk_max.y, controllerGrid.GetLength(1) - 1);
        var array = previouslyVisibleChunkGrid;
        previouslyVisibleChunkGrid = visibleChunkGrid;
        visibleChunkGrid           = array;
        Array.Clear(visibleChunkGrid, 0, visibleChunkGrid.Length);
        var list = previouslyVisibleChunks;
        previouslyVisibleChunks = visibleChunks;
        visibleChunks           = list;
        visibleChunks.Clear();
        for (var i = vis_chunk_min.y; i <= vis_chunk_max.y; i++) {
            for (var j = vis_chunk_min.x; j <= vis_chunk_max.x; j++) {
                visibleChunkGrid[j, i] = true;
                visibleChunks.Add(new Vector2I(j, i));
                if (!previouslyVisibleChunkGrid[j, i])
                    foreach (var keyValuePair in controllerGrid[j, i]) {
                        var value = keyValuePair.Value;
                        if (!(value == null)) value.SetVisiblity(true);
                    }
            }
        }

        for (var k = 0; k < previouslyVisibleChunks.Count; k++) {
            var vector2I3 = previouslyVisibleChunks[k];
            if (!visibleChunkGrid[vector2I3.x, vector2I3.y])
                foreach (var keyValuePair2 in controllerGrid[vector2I3.x, vector2I3.y]) {
                    var value2 = keyValuePair2.Value;
                    if (!(value2 == null)) value2.SetVisiblity(false);
                }
        }
    }

    private void ProcessMovingAnims() {
        foreach (var movingControllerInfo in movingControllerInfos.Values)
            if (!(movingControllerInfo.controller == null)) {
                var vector2I = PosToChunkXY(movingControllerInfo.controller.PositionIncludingOffset);
                if (movingControllerInfo.chunkXY != vector2I) {
                    var controllerChunkInfo = default(ControllerChunkInfo);
                    DebugUtil.Assert(controllerChunkInfos.TryGetValue(movingControllerInfo.controllerInstanceId,
                                                                      out controllerChunkInfo));

                    DebugUtil.Assert(movingControllerInfo.controller == controllerChunkInfo.controller);
                    DebugUtil.Assert(controllerChunkInfo.chunkXY     == movingControllerInfo.chunkXY);
                    var controllerMap = GetControllerMap(controllerChunkInfo.chunkXY);
                    if (controllerMap != null) {
                        DebugUtil.Assert(controllerMap.ContainsKey(movingControllerInfo.controllerInstanceId));
                        controllerMap.Remove(movingControllerInfo.controllerInstanceId);
                    }

                    controllerMap = GetControllerMap(vector2I);
                    if (controllerMap != null) {
                        DebugUtil.Assert(!controllerMap.ContainsKey(movingControllerInfo.controllerInstanceId));
                        controllerMap[movingControllerInfo.controllerInstanceId] = controllerChunkInfo.controller;
                    }

                    movingControllerInfo.chunkXY                                    = vector2I;
                    controllerChunkInfo.chunkXY                                     = vector2I;
                    controllerChunkInfos[movingControllerInfo.controllerInstanceId] = controllerChunkInfo;
                    if (controllerMap != null)
                        controllerChunkInfo.controller.SetVisiblity(visibleChunkGrid[vector2I.x, vector2I.y]);
                    else
                        controllerChunkInfo.controller.SetVisiblity(false);
                }
            }
    }

    private void ProcessRegistrations() {
        for (var i = 0; i < queuedRegistrations.Count; i++) {
            var registrationInfo = queuedRegistrations[i];
            if (registrationInfo.register) {
                if (!(registrationInfo.controller == null)) {
                    var instanceID = registrationInfo.controller.GetInstanceID();
                    DebugUtil.Assert(!controllerChunkInfos.ContainsKey(instanceID));
                    var controllerChunkInfo = new ControllerChunkInfo {
                        controller = registrationInfo.controller,
                        chunkXY    = PosToChunkXY(registrationInfo.controller.PositionIncludingOffset)
                    };

                    controllerChunkInfos[instanceID] = controllerChunkInfo;
                    var flag = false;
                    if (Singleton<CellChangeMonitor>.Instance != null) {
                        flag = Singleton<CellChangeMonitor>.Instance.IsMoving(registrationInfo.controller.transform);
                        Singleton<CellChangeMonitor>.Instance.RegisterMovementStateChanged(registrationInfo.controller
                             .transform,
                         OnMovementStateChanged);
                    }

                    var controllerMap = GetControllerMap(controllerChunkInfo.chunkXY);
                    if (controllerMap != null) {
                        DebugUtil.Assert(!controllerMap.ContainsKey(instanceID));
                        controllerMap.Add(instanceID, registrationInfo.controller);
                    }

                    if (flag) {
                        DebugUtil.DevAssertArgs(!movingControllerInfos.ContainsKey(instanceID),
                                                "Readding controller which is already moving",
                                                registrationInfo.controller.name,
                                                controllerChunkInfo.chunkXY,
                                                movingControllerInfos.ContainsKey(instanceID)
                                                    ? movingControllerInfos[instanceID].chunkXY.ToString()
                                                    : null);

                        movingControllerInfos[instanceID] = new MovingControllerInfo {
                            controllerInstanceId = instanceID,
                            controller           = registrationInfo.controller,
                            chunkXY              = controllerChunkInfo.chunkXY
                        };
                    }

                    if (controllerMap != null &&
                        visibleChunkGrid[controllerChunkInfo.chunkXY.x, controllerChunkInfo.chunkXY.y])
                        registrationInfo.controller.SetVisiblity(true);
                }
            } else {
                var controllerChunkInfo2 = default(ControllerChunkInfo);
                if (controllerChunkInfos.TryGetValue(registrationInfo.controllerInstanceId, out controllerChunkInfo2)) {
                    if (registrationInfo.controller != null) {
                        var controllerMap2 = GetControllerMap(controllerChunkInfo2.chunkXY);
                        if (controllerMap2 != null) {
                            DebugUtil.Assert(controllerMap2.ContainsKey(registrationInfo.controllerInstanceId));
                            controllerMap2.Remove(registrationInfo.controllerInstanceId);
                        }

                        registrationInfo.controller.SetVisiblity(false);
                    }

                    movingControllerInfos.Remove(registrationInfo.controllerInstanceId);
                    Singleton<CellChangeMonitor>.Instance.UnregisterMovementStateChanged(registrationInfo.transformId,
                     OnMovementStateChanged);

                    controllerChunkInfos.Remove(registrationInfo.controllerInstanceId);
                }
            }
        }

        queuedRegistrations.Clear();
    }

    public void OnMovementStateChanged(Transform transform, bool is_moving) {
        if (transform == null) return;

        var component = transform.GetComponent<KBatchedAnimController>();
        if (component == null) return;

        var instanceID          = component.GetInstanceID();
        var controllerChunkInfo = default(ControllerChunkInfo);
        DebugUtil.Assert(controllerChunkInfos.TryGetValue(instanceID, out controllerChunkInfo));
        if (is_moving) {
            DebugUtil.DevAssertArgs(!movingControllerInfos.ContainsKey(instanceID),
                                    "Readding controller which is already moving",
                                    component.name,
                                    controllerChunkInfo.chunkXY,
                                    movingControllerInfos.ContainsKey(instanceID)
                                        ? movingControllerInfos[instanceID].chunkXY.ToString()
                                        : null);

            movingControllerInfos[instanceID] = new MovingControllerInfo {
                controllerInstanceId = instanceID, controller = component, chunkXY = controllerChunkInfo.chunkXY
            };

            return;
        }

        movingControllerInfos.Remove(instanceID);
    }

    private void CleanUp() {
        if (!DoGridProcessing()) return;

        var length = controllerGrid.GetLength(0);
        for (var i = 0; i < 16; i++) {
            var num        = (cleanUpChunkIndex + i) % controllerGrid.Length;
            var num2       = num                     % length;
            var num3       = num                     / length;
            var dictionary = controllerGrid[num2, num3];
            var pooledList = ListPool<int, KBatchedAnimUpdater>.Allocate();
            foreach (var keyValuePair in dictionary)
                if (keyValuePair.Value == null)
                    pooledList.Add(keyValuePair.Key);

            foreach (var key in pooledList) dictionary.Remove(key);
            pooledList.Recycle();
        }

        cleanUpChunkIndex = (cleanUpChunkIndex + 16) % controllerGrid.Length;
    }

    private bool DoGridProcessing() { return controllerGrid != null && Camera.main != null; }

    private struct RegistrationInfo {
        public bool                   register;
        public int                    transformId;
        public int                    controllerInstanceId;
        public KBatchedAnimController controller;
    }

    private struct ControllerChunkInfo {
        public KBatchedAnimController controller;
        public Vector2I               chunkXY;
    }

    private class MovingControllerInfo {
        public Vector2I               chunkXY;
        public KBatchedAnimController controller;
        public int                    controllerInstanceId;
    }
}