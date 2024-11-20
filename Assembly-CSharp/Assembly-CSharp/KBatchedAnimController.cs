using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

[DebuggerDisplay("{name} visible={isVisible} suspendUpdates={suspendUpdates} moving={IsMoving}")]
public class KBatchedAnimController : KAnimControllerBase, KAnimConverter.IAnimConverter {
    [NonSerialized]
    protected bool _forceRebuild;

    // 定义动画覆盖的尺寸，默认值为 Vector2.one
    public Vector2 animOverrideSize = Vector2.one;
    
    // 定义动画缩放比例，用于调整动画的大小
    public float animScale = 0.005f;
    
    // KAnimBatch 对象，用于管理动画批次
    private KAnimBatch batch;
    
    // 获取位置数据的函数，用于确定动画的位置
    public Func<Vector4> getPositionDataFunctionInUse;
    
    // 表示动画是否可移动
    public bool isMovable;
    
    // 上一次动画所在的区块坐标，初始化为无效值
    private Vector2I lastChunkXY = KBatchedAnimUpdater.INVALID_CHUNK_ID;
    
    // 上一次动画的位置，初始化为原点
    private Vector3 lastPos = Vector3.zero;
    
    // 导航矩阵，用于变换动画位置和方向
    public Matrix2x3 navMatrix = Matrix2x3.identity;
    
    // 允许的后处理效果，定义了动画可以应用的特效类型
    public KAnimConverter.PostProcessingEffects postProcessingEffectsAllowed;
    
    // 后处理参数，用于调整后处理效果的强度
    public float postProcessingParameters;
    
    // 根 Canvas 对象，用于管理 UI 元素的画布
    private Canvas rootCanvas;
    
    // RectTransform 对象，用于管理 UI 元素的矩形变换
    private RectTransform rt;
    
    // CanvasScaler 对象，用于管理画布的缩放
    private CanvasScaler scaler;
    
    // 定义动画所在的场景层
    public Grid.SceneLayer sceneLayer;
    
    // 屏幕偏移量，用于调整动画在屏幕上的位置
    private Vector3 screenOffset = new Vector3(0f, 0f, 0f);
    
    // 表示是否从动画中设置缩放比例
    public bool setScaleFromAnim = true;
    
    // 表示是否暂停更新动画
    private bool suspendUpdates;
    
    // SymbolOverrideController 对象，用于管理符号覆盖
    private SymbolOverrideController symbolOverrideController;
    
    // 符号覆盖控制器的版本号，用于检测变化
    private int symbolOverrideControllerVersion;

    [NonSerialized]
    public KBatchedAnimUpdater.RegistrationState updateRegistrationState
        = KBatchedAnimUpdater.RegistrationState.Unregistered;

    private bool visibilityListenerRegistered;
    public KBatchedAnimController() { batchInstanceData = new KBatchedAnimInstanceData(this); }

    protected bool forceRebuild {
        get => _forceRebuild;
        set => _forceRebuild = value;
    }

    public bool                     IsMoving               { get; private set; }
    public HashedString             batchGroupID           { get; private set; }
    public HashedString             batchGroupIDOverride   { get; private set; }
    public int                      GetCurrentFrameIndex() { return curAnimFrameIdx; }
    public KBatchedAnimInstanceData GetBatchInstanceData() { return batchInstanceData; }
    public bool                     IsActive()             { return isActiveAndEnabled && _enabled; }
    public bool                     IsVisible()            { return isVisible; }

    public Vector4 GetPositionData() {
        if (getPositionDataFunctionInUse != null) return getPositionDataFunctionInUse();

        var position                = transform.GetPosition();
        var positionIncludingOffset = PositionIncludingOffset;
        return new Vector4(position.x, position.y, positionIncludingOffset.x, positionIncludingOffset.y);
    }

    public Vector2I GetCellXY() {
        var positionIncludingOffset = PositionIncludingOffset;
        if (Grid.CellSizeInMeters == 0f)
            return new Vector2I((int)positionIncludingOffset.x, (int)positionIncludingOffset.y);

        return Grid.PosToXY(positionIncludingOffset);
    }

    public float  GetZ()          { return transform.GetPosition().z; }
    public string GetName()       { return name; }
    public int    GetMaxVisible() { return maxSymbols; }

    public HashedString GetBatchGroupID(bool isEditorWindow = false) {
        Debug.Assert(isEditorWindow           ||
                     animFiles        == null ||
                     animFiles.Length == 0    ||
                     (batchGroupID.IsValid && batchGroupID != KAnimBatchManager.NO_BATCH));

        return batchGroupID;
    }

    public HashedString GetBatchGroupIDOverride() { return batchGroupIDOverride; }
    public int          GetLayer()                { return gameObject.layer; }
    public KAnimBatch   GetBatch()                { return batch; }

    public void SetBatch(KAnimBatch new_batch) {
        batch = new_batch;
        if (materialType == KAnimBatchGroup.MaterialType.UI) {
            var kbatchedAnimCanvasRenderer = GetComponent<KBatchedAnimCanvasRenderer>();
            if (kbatchedAnimCanvasRenderer == null && new_batch != null)
                kbatchedAnimCanvasRenderer = gameObject.AddComponent<KBatchedAnimCanvasRenderer>();

            if (kbatchedAnimCanvasRenderer != null) kbatchedAnimCanvasRenderer.SetBatch(this);
        }
    }

    public int GetCurrentNumFrames() {
        if (curAnim == null) return 0;

        return curAnim.numFrames;
    }

    public int GetFirstFrameIndex() {
        if (curAnim == null) return -1;

        return curAnim.firstFrameIdx;
    }

    public override Matrix2x3 GetTransformMatrix() {
        var v = PositionIncludingOffset;
        v.z = 0f;
        var scale = new Vector2(animScale * animWidth, -animScale * animHeight);
        if (materialType == KAnimBatchGroup.MaterialType.UI) {
            rt = GetComponent<RectTransform>();
            if (rootCanvas == null) rootCanvas               = GetRootCanvas();
            if (scaler == null && rootCanvas != null) scaler = rootCanvas.GetComponent<CanvasScaler>();
            if (rootCanvas == null) {
                screenOffset.x = Screen.width  / 2;
                screenOffset.y = Screen.height / 2;
            } else {
                screenOffset.x = rootCanvas.renderMode == RenderMode.WorldSpace
                                     ? 0f
                                     : rootCanvas.rectTransform().rect.width / 2f;

                screenOffset.y = rootCanvas.renderMode == RenderMode.WorldSpace
                                     ? 0f
                                     : rootCanvas.rectTransform().rect.height / 2f;
            }

            var num                 = 1f;
            if (scaler != null) num = 1f                                 / scaler.scaleFactor;
            v = (rt.localToWorldMatrix.MultiplyPoint(rt.pivot) + offset) * num - screenOffset;
            var num2 = animWidth  * animScale;
            var num3 = animHeight * animScale;
            if (setScaleFromAnim && curAnim != null) {
                num2 *= rt.rect.size.x / curAnim.unScaledSize.x;
                num3 *= rt.rect.size.y / curAnim.unScaledSize.y;
            } else {
                num2 *= rt.rect.size.x / animOverrideSize.x;
                num3 *= rt.rect.size.y / animOverrideSize.y;
            }

            scale = new Vector3(rt.lossyScale.x * num2 * num, -rt.lossyScale.y * num3 * num, rt.lossyScale.z * num);
            pivot = rt.pivot;
        }

        var       n  = Matrix2x3.Scale(scale);
        var       n2 = Matrix2x3.Scale(new Vector2(flipX ? -1f : 1f, flipY ? -1f : 1f));
        Matrix2x3 result;
        if (rotation != 0f) {
            var n3 = Matrix2x3.Translate(-pivot);
            var n4 = Matrix2x3.Rotate(rotation * 0.017453292f);
            var n5 = Matrix2x3.Translate(pivot) * n4 * n3;
            result = Matrix2x3.TRS(v, transform.rotation, transform.localScale) * n5 * n * navMatrix * n2;
        } else
            result = Matrix2x3.TRS(v, transform.rotation, transform.localScale) * n * navMatrix * n2;

        return result;
    }

    public bool ApplySymbolOverrides() {
        batch.atlases.Apply(batch.matProperties);
        if (symbolOverrideController != null) {
            if (symbolOverrideControllerVersion != symbolOverrideController.version ||
                symbolOverrideController.applySymbolOverridesEveryFrame) {
                symbolOverrideControllerVersion = symbolOverrideController.version;
                symbolOverrideController.ApplyOverrides();
            }

            symbolOverrideController.ApplyAtlases();
            return true;
        }

        return false;
    }

    public virtual KAnimConverter.PostProcessingEffects GetPostProcessingEffectsCompatibility() {
        return postProcessingEffectsAllowed;
    }

    public float GetPostProcessingParams() { return postProcessingParameters; }

    public void SetSymbolScale(KAnimHashedString symbol_name, float scale) {
        var symbol = KAnimBatchManager.Instance().GetBatchGroupData(GetBatchGroupID()).GetSymbol(symbol_name);
        if (symbol == null) return;

        symbolInstanceGpuData.SetSymbolScale(symbol.symbolIndexInSourceBuild, scale);
        SuspendUpdates(false);
        SetDirty();
    }

    public void SetSymbolTint(KAnimHashedString symbol_name, Color color) {
        var symbol = KAnimBatchManager.Instance().GetBatchGroupData(GetBatchGroupID()).GetSymbol(symbol_name);
        if (symbol == null) return;

        symbolInstanceGpuData.SetSymbolTint(symbol.symbolIndexInSourceBuild, color);
        SuspendUpdates(false);
        SetDirty();
    }

    public override KAnim.Anim GetAnim(int index) {
        if (!batchGroupID.IsValid || !(batchGroupID != KAnimBatchManager.NO_BATCH))
            Debug.LogError(name + " batch not ready");

        var batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(batchGroupID);
        Debug.Assert(batchGroupData != null);
        return batchGroupData.GetAnim(index);
    }

    private void Initialize() {
        if (batchGroupID.IsValid && batchGroupID != KAnimBatchManager.NO_BATCH) {
            DeRegister();
            Register();
        }
    }

    private void OnMovementStateChanged(bool is_moving) {
        if (is_moving == IsMoving) return;

        IsMoving = is_moving;
        SetDirty();
        ConfigureUpdateListener();
    }

    private static void OnMovementStateChanged(Transform transform, bool is_moving) {
        transform.GetComponent<KBatchedAnimController>().OnMovementStateChanged(is_moving);
    }

    private void SetBatchGroup(KAnimFileData kafd) {
        if (this.batchGroupID.IsValid && kafd != null && this.batchGroupID == kafd.batchTag) return;

        DebugUtil.Assert(!this.batchGroupID.IsValid, "Should only be setting the batch group once.");
        DebugUtil.Assert(kafd != null,               "Null anim data!! For", name);
        curBuild = kafd.build;
        DebugUtil.Assert(curBuild != null, "Null build for anim!! ", name, kafd.name);
        var group        = KAnimGroupFile.GetGroup(curBuild.batchTag);
        var batchGroupID = kafd.build.batchTag;
        if (group.renderType == KAnimBatchGroup.RendererType.DontRender ||
            group.renderType == KAnimBatchGroup.RendererType.AnimOnly) {
            var isValid = group.swapTarget.IsValid;
            var str     = "Invalid swap target fro group [";
            var id      = group.id;
            Debug.Assert(isValid, str + id + "]");
            batchGroupID = group.swapTarget;
        }

        this.batchGroupID = batchGroupID;
        symbolInstanceGpuData
            = new SymbolInstanceGpuData(KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID)
                                                         .maxSymbolsPerBuild);

        symbolOverrideInfoGpuData
            = new SymbolOverrideInfoGpuData(KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID)
                                                             .symbolFrameInstances.Count);

        if (!this.batchGroupID.IsValid || this.batchGroupID == KAnimBatchManager.NO_BATCH)
            Debug.LogError("Batch is not ready: " + name);

        if (materialType == KAnimBatchGroup.MaterialType.Default && this.batchGroupID == KAnimBatchManager.BATCH_HUMAN)
            materialType = KAnimBatchGroup.MaterialType.Human;
    }

    public void LoadAnims() {
        if (!KAnimBatchManager.Instance().isReady)
            Debug.LogError("KAnimBatchManager is not ready when loading anim:" + name);

        if (animFiles.Length == 0) DebugUtil.Assert(false, "KBatchedAnimController has no anim files:" + name);
        if (!animFiles[0].IsBuildLoaded)
            DebugUtil.LogErrorArgs(gameObject,
                                   string
                                       .Format("First anim file needs to be the build file but {0} doesn't have an associated build",
                                               animFiles[0].GetData().name));

        overrideAnims.Clear();
        anims.Clear();
        SetBatchGroup(animFiles[0].GetData());
        for (var i = 0; i < animFiles.Length; i++) AddAnims(animFiles[i]);
        forceRebuild = true;
        if (layering != null) layering.HideSymbols();
        if (usingNewSymbolOverrideSystem) DebugUtil.Assert(GetComponent<SymbolOverrideController>() != null);
    }

    public void SwapAnims(KAnimFile[] anims) {
        if (batchGroupID.IsValid) {
            DeRegister();
            batchGroupID = HashedString.Invalid;
        }

        AnimFiles = anims;
        LoadAnims();
        if (curBuild != null) UpdateHiddenSymbolSet(hiddenSymbolsSet);
        Register();
    }

    public void UpdateAnim(float dt) {
        if (batch != null && transform.hasChanged) {
            transform.hasChanged = false;
            if (batch != null && batch.group.maxGroupSize == 1 && lastPos.z != transform.GetPosition().z)
                batch.OverrideZ(transform.GetPosition().z);

            var positionIncludingOffset = PositionIncludingOffset;
            lastPos = positionIncludingOffset;
            if (visibilityType                              != VisibilityType.Always &&
                KAnimBatchManager.ControllerToChunkXY(this) != lastChunkXY           &&
                lastChunkXY                                 != KBatchedAnimUpdater.INVALID_CHUNK_ID) {
                DeRegister();
                Register();
            }

            SetDirty();
        }

        if (batchGroupID == KAnimBatchManager.NO_BATCH || !IsActive()) return;

        if (!forceRebuild &&
            (mode == KAnim.PlayMode.Paused ||
             stopped                       ||
             curAnim == null               ||
             (mode    == KAnim.PlayMode.Once                                    &&
              curAnim != null                                                   &&
              (this.elapsedTime > curAnim.totalTime || curAnim.totalTime <= 0f) &&
              animQueue.Count == 0)))
            SuspendUpdates(true);

        if (!isVisible && !forceRebuild) {
            if (visibilityType == VisibilityType.OffscreenUpdate && !stopped && mode != KAnim.PlayMode.Paused)
                SetElapsedTime(elapsedTime + dt * playSpeed);

            return;
        }

        curAnimFrameIdx = GetFrameIdx(this.elapsedTime, true);
        if (eventManagerHandle.IsValid() && aem != null) {
            var elapsedTime = aem.GetElapsedTime(eventManagerHandle);
            if ((int)((this.elapsedTime - elapsedTime) * 100f) != 0) UpdateAnimEventSequenceTime();
        }

        UpdateFrame(this.elapsedTime);
        if (!stopped && mode != KAnim.PlayMode.Paused) SetElapsedTime(elapsedTime + dt * playSpeed);
        forceRebuild = false;
    }

    protected override void UpdateFrame(float t) {
        previousFrame = currentFrame;
        if (!stopped || forceRebuild) {
            if (curAnim != null && (mode == KAnim.PlayMode.Loop || elapsedTime <= GetDuration() || forceRebuild)) {
                currentFrame = curAnim.GetFrameIdx(mode, elapsedTime);
                if (currentFrame != previousFrame || forceRebuild) SetDirty();
            } else
                TriggerStop();

            if (!stopped && mode == KAnim.PlayMode.Loop && currentFrame == 0) AnimEnter(curAnim.hash);
        }

        if (synchronizer != null) synchronizer.SyncTime();
    }

    public override void TriggerStop() {
        if (animQueue.Count > 0) {
            StartQueuedAnim();
            return;
        }

        if (curAnim != null && mode == KAnim.PlayMode.Once) {
            currentFrame = curAnim.numFrames - 1;
            Stop();
            gameObject.Trigger(-1061186183);
            if (destroyOnAnimComplete) DestroySelf();
        }
    }

    public override void UpdateHiddenSymbol(KAnimHashedString symbolToUpdate) {
        var batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(batchGroupID);
        for (var i = 0; i < batchGroupData.frameElementSymbols.Count; i++)
            if (!(symbolToUpdate != batchGroupData.frameElementSymbols[i].hash)) {
                var symbol     = batchGroupData.frameElementSymbols[i];
                var is_visible = !hiddenSymbolsSet.Contains(symbol.hash);
                symbolInstanceGpuData.SetVisible(i, is_visible);
            }

        SetDirty();
    }

    public override void UpdateHiddenSymbolSet(HashSet<KAnimHashedString> symbolsToUpdate) {
        var batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(batchGroupID);
        for (var i = 0; i < batchGroupData.frameElementSymbols.Count; i++)
            if (symbolsToUpdate.Contains(batchGroupData.frameElementSymbols[i].hash)) {
                var symbol     = batchGroupData.frameElementSymbols[i];
                var is_visible = !hiddenSymbolsSet.Contains(symbol.hash);
                symbolInstanceGpuData.SetVisible(i, is_visible);
            }

        SetDirty();
    }

    public override void UpdateAllHiddenSymbols() {
        var batchGroupData = KAnimBatchManager.instance.GetBatchGroupData(batchGroupID);
        for (var i = 0; i < batchGroupData.frameElementSymbols.Count; i++) {
            var symbol     = batchGroupData.frameElementSymbols[i];
            var is_visible = !hiddenSymbolsSet.Contains(symbol.hash);
            symbolInstanceGpuData.SetVisible(i, is_visible);
        }

        SetDirty();
    }

    public void SetBatchGroupOverride(HashedString id) {
        batchGroupIDOverride = id;
        DeRegister();
        Register();
    }

    private Canvas GetRootCanvas() {
        if (rt == null) return null;

        var component = rt.parent.GetComponent<RectTransform>();
        while (component != null) {
            var component2 = component.GetComponent<Canvas>();
            if (component2 != null && component2.isRootCanvas) return component2;

            component = component.parent.GetComponent<RectTransform>();
        }

        return null;
    }

    public Matrix2x3 GetTransformMatrix(Vector2 customScale) {
        var v = PositionIncludingOffset;
        v.z = 0f;
        var scale = customScale;
        if (materialType == KAnimBatchGroup.MaterialType.UI) {
            rt = GetComponent<RectTransform>();
            if (rootCanvas == null) rootCanvas               = GetRootCanvas();
            if (scaler == null && rootCanvas != null) scaler = rootCanvas.GetComponent<CanvasScaler>();
            if (rootCanvas == null) {
                screenOffset.x = Screen.width  / 2;
                screenOffset.y = Screen.height / 2;
            } else {
                screenOffset.x = rootCanvas.renderMode == RenderMode.WorldSpace
                                     ? 0f
                                     : rootCanvas.rectTransform().rect.width / 2f;

                screenOffset.y = rootCanvas.renderMode == RenderMode.WorldSpace
                                     ? 0f
                                     : rootCanvas.rectTransform().rect.height / 2f;
            }

            var num                 = 1f;
            if (scaler != null) num = 1f                                 / scaler.scaleFactor;
            v = (rt.localToWorldMatrix.MultiplyPoint(rt.pivot) + offset) * num - screenOffset;
            var num2 = animWidth  * animScale;
            var num3 = animHeight * animScale;
            if (setScaleFromAnim && curAnim != null) {
                num2 *= rt.rect.size.x / curAnim.unScaledSize.x;
                num3 *= rt.rect.size.y / curAnim.unScaledSize.y;
            } else {
                num2 *= rt.rect.size.x / animOverrideSize.x;
                num3 *= rt.rect.size.y / animOverrideSize.y;
            }

            scale = new Vector3(rt.lossyScale.x * num2 * num, -rt.lossyScale.y * num3 * num, rt.lossyScale.z * num);
            pivot = rt.pivot;
        }

        var       n  = Matrix2x3.Scale(scale);
        var       n2 = Matrix2x3.Scale(new Vector2(flipX ? -1f : 1f, flipY ? -1f : 1f));
        Matrix2x3 result;
        if (rotation != 0f) {
            var n3 = Matrix2x3.Translate(-pivot);
            var n4 = Matrix2x3.Rotate(rotation * 0.017453292f);
            var n5 = Matrix2x3.Translate(pivot) * n4 * n3;
            result = Matrix2x3.TRS(v, transform.rotation, transform.localScale) * n5 * n * navMatrix * n2;
        } else
            result = Matrix2x3.TRS(v, transform.rotation, transform.localScale) * n * navMatrix * n2;

        return result;
    }

    public override Matrix4x4 GetSymbolTransform(HashedString symbol, out bool symbolVisible) {
        if (curAnimFrameIdx != -1 && batch != null) {
            var symbolLocalTransform = GetSymbolLocalTransform(symbol, out symbolVisible);
            if (symbolVisible) return GetTransformMatrix() * symbolLocalTransform;
        }

        symbolVisible = false;
        return default(Matrix4x4);
    }

    public override Matrix2x3 GetSymbolLocalTransform(HashedString symbol, out bool symbolVisible) {
        KAnim.Anim.Frame frame;
        if (curAnimFrameIdx != -1 && batch != null && batch.group.data.TryGetFrame(curAnimFrameIdx, out frame))
            for (var i = 0; i < frame.numElements; i++) {
                var num = frame.firstElementIdx + i;
                if (num < batch.group.data.frameElements.Count) {
                    var frameElement = batch.group.data.frameElements[num];
                    if (frameElement.symbol == symbol) {
                        symbolVisible = true;
                        return frameElement.transform;
                    }
                }
            }

        symbolVisible = false;
        return Matrix2x3.identity;
    }

    public override void SetLayer(int layer) {
        if (layer == gameObject.layer) return;

        base.SetLayer(layer);
        DeRegister();
        gameObject.layer = layer;
        Register();
    }

    public override void SetDirty() {
        if (batch != null) batch.SetDirty(this);
    }

    protected override void OnStartQueuedAnim() { SuspendUpdates(false); }

    protected override void OnAwake() {
        LoadAnims();
        if (visibilityType == VisibilityType.Default)
            visibilityType = materialType == KAnimBatchGroup.MaterialType.UI ? VisibilityType.Always : visibilityType;

        if (materialType == KAnimBatchGroup.MaterialType.Default && batchGroupID == KAnimBatchManager.BATCH_HUMAN)
            materialType = KAnimBatchGroup.MaterialType.Human;

        symbolOverrideController = GetComponent<SymbolOverrideController>();
        UpdateAllHiddenSymbols();
        hasEnableRun = false;
    }

    protected override void OnStart() {
        if (batch == null) Initialize();
        if (visibilityType == VisibilityType.Always || visibilityType == VisibilityType.OffscreenUpdate)
            ConfigureUpdateListener();

        var instance = Singleton<CellChangeMonitor>.Instance;
        if (instance != null) {
            instance.RegisterMovementStateChanged(transform, OnMovementStateChanged);
            IsMoving = instance.IsMoving(transform);
        }

        symbolOverrideController = GetComponent<SymbolOverrideController>();
        SetDirty();
    }

    protected override void OnStop() { SetDirty(); }

    private void OnEnable() {
        if (_enabled) Enable();
    }

    protected override void Enable() {
        if (hasEnableRun) return;

        hasEnableRun = true;
        if (batch == null) Initialize();
        SetDirty();
        SuspendUpdates(false);
        ConfigureVisibilityListener(true);
        if (!stopped && curAnim != null && mode != KAnim.PlayMode.Paused && !eventManagerHandle.IsValid())
            StartAnimEventSequence();
    }

    private void OnDisable() { Disable(); }

    protected override void Disable() {
        if (App.IsExiting || KMonoBehaviour.isLoadingScene) return;

        if (!hasEnableRun) return;

        hasEnableRun = false;
        SuspendUpdates(true);
        if (batch != null) DeRegister();
        ConfigureVisibilityListener(false);
        StopAnimEventSequence();
    }

    protected override void OnDestroy() {
        if (App.IsExiting) return;

        var instance = Singleton<CellChangeMonitor>.Instance;
        if (instance != null) instance.UnregisterMovementStateChanged(transform, OnMovementStateChanged);
        var instance2 = Singleton<KBatchedAnimUpdater>.Instance;
        if (instance2 != null) instance2.UpdateUnregister(this);
        isVisible = false;
        DeRegister();
        stopped = true;
        StopAnimEventSequence();
        batchInstanceData = null;
        batch             = null;
        base.OnDestroy();
    }

    public void SetBlendValue(float value) {
        batchInstanceData.SetBlend(value);
        SetDirty();
    }

    public SymbolOverrideController SetupSymbolOverriding() {
        if (!symbolOverrideController.IsNullOrDestroyed()) return symbolOverrideController;

        usingNewSymbolOverrideSystem = true;
        symbolOverrideController     = SymbolOverrideControllerUtil.AddToPrefab(gameObject);
        return symbolOverrideController;
    }

    public void SetSymbolOverrides(int             symbol_start_idx,
                                   int             symbol_num_frames,
                                   int             atlas_idx,
                                   KBatchGroupData source_data,
                                   int             source_start_idx,
                                   int             source_num_frames) {
        symbolOverrideInfoGpuData.SetSymbolOverrideInfo(symbol_start_idx,
                                                        symbol_num_frames,
                                                        atlas_idx,
                                                        source_data,
                                                        source_start_idx,
                                                        source_num_frames);
    }

    public void SetSymbolOverride(int symbol_idx, ref KAnim.Build.SymbolFrameInstance symbol_frame_instance) {
        symbolOverrideInfoGpuData.SetSymbolOverrideInfo(symbol_idx, ref symbol_frame_instance);
    }

    protected override void Register() {
        if (!IsActive()) return;

        if (batch != null) return;

        if (batchGroupID.IsValid && batchGroupID != KAnimBatchManager.NO_BATCH) {
            lastChunkXY = KAnimBatchManager.ControllerToChunkXY(this);
            KAnimBatchManager.Instance().Register(this);
            forceRebuild = true;
            SetDirty();
        }
    }

    protected override void DeRegister() {
        if (batch != null) batch.Deregister(this);
    }

    private void ConfigureUpdateListener() {
        if ((IsActive() && !suspendUpdates && isVisible)     ||
            IsMoving                                         ||
            visibilityType == VisibilityType.OffscreenUpdate ||
            visibilityType == VisibilityType.Always) {
            Singleton<KBatchedAnimUpdater>.Instance.UpdateRegister(this);
            return;
        }

        Singleton<KBatchedAnimUpdater>.Instance.UpdateUnregister(this);
    }

    protected override void SuspendUpdates(bool suspend) {
        suspendUpdates = suspend;
        ConfigureUpdateListener();
    }

    public void SetVisiblity(bool is_visible) {
        if (is_visible != isVisible) {
            isVisible = is_visible;
            if (is_visible) {
                SuspendUpdates(false);
                SetDirty();
                UpdateAnimEventSequenceTime();
                return;
            }

            SuspendUpdates(true);
            SetDirty();
        }
    }

    private void ConfigureVisibilityListener(bool enabled) {
        if (visibilityType == VisibilityType.Always || visibilityType == VisibilityType.OffscreenUpdate) return;

        if (enabled) {
            RegisterVisibilityListener();
            return;
        }

        UnregisterVisibilityListener();
    }

    protected override void RefreshVisibilityListener() {
        if (!visibilityListenerRegistered) return;

        ConfigureVisibilityListener(false);
        ConfigureVisibilityListener(true);
    }

    private void RegisterVisibilityListener() {
        DebugUtil.Assert(!visibilityListenerRegistered);
        Singleton<KBatchedAnimUpdater>.Instance.VisibilityRegister(this);
        visibilityListenerRegistered = true;
    }

    private void UnregisterVisibilityListener() {
        DebugUtil.Assert(visibilityListenerRegistered);
        Singleton<KBatchedAnimUpdater>.Instance.VisibilityUnregister(this);
        visibilityListenerRegistered = false;
    }

    public void SetSceneLayer(Grid.SceneLayer layer) {
        var layerZ = Grid.GetLayerZ(layer);
        sceneLayer = layer;
        var position = transform.GetPosition();
        position.z = layerZ;
        transform.SetPosition(position);
        DeRegister();
        Register();
    }
}