using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using FMOD.Studio;
using Klei;
using Klei.AI;
using Klei.CustomSettings;
using KSerialization;
using ProcGenGame;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[AddComponentMenu("KMonoBehaviour/scripts/Game")]
public class Game : KMonoBehaviour {
    public delegate void CansaveCB();

    public delegate void SavingActiveCB();

    public delegate void SavingPostCB();

    public delegate void SavingPreCB(CansaveCB cb);

    public enum TemperatureOverlayModes {
        AbsoluteTemperature,
        AdaptiveTemperature,
        HeatFlow,
        StateChange,
        RelativeTemperature
    }

    private static readonly string         NextUniqueIDKey = "NextUniqueID";
    public static           string         clusterId       = null;
    public static           bool           quitting;
    private static          Camera         m_CachedCamera;
    public static           int            BlockSelectionLayerMask;
    public static           int            PickupableLayer;
    public static           BrainScheduler BrainScheduler;

    private static readonly EventSystem.IntraObjectHandler<Game> MarkStatusItemRendererDirtyDelegate
        = new EventSystem.IntraObjectHandler<Game>(delegate(Game component, object data) {
                                                       component.MarkStatusItemRendererDirty(data);
                                                   });

    private static readonly EventSystem.IntraObjectHandler<Game> ActiveWorldChangedDelegate
        = new EventSystem.IntraObjectHandler<Game>(delegate(Game component, object data) {
                                                       component.ForceOverlayUpdate(true);
                                                   });

    public  Accumulators   accumulators;
    private SavingActiveCB activateActiveCB;
    private SavingPostCB   activatePostCB;
    private SavingPreCB    activatePreCB;
    private ushort[]       activeFX;

    [NonSerialized]
    public bool advancedPersonalPriorities;

    public AssignmentManager assignmentManager;

    [NonSerialized]
    public bool autoPrioritizeRoles;

    [NonSerialized]
    public bool baseAlreadyCreated;

    private readonly List<Klei.CallbackInfo>    callbackInfo = new List<Klei.CallbackInfo>();
    public           HandleVector<CallbackInfo> callbackManager = new HandleVector<CallbackInfo>(256);
    public           List<int>                  callbackManagerManuallyReleasedHandles = new List<int>();
    private          CameraController           cameraController;
    public           GameObject                 cameraControllerPrefab;
    public           List<uint>                 changelistsPlayedOn;

    [NonSerialized]
    public CircuitManager circuitManager;

    public ConduitDiseaseManager     conduitDiseaseManager;
    public ConduitTemperatureManager conduitTemperatureManager;
    public float                     currentFallbackSunlightIntensity;
    public string                    dateGenerated;
    public bool                      debugWasUsed;

    public ComplexCallbackHandleVector<Sim.DiseaseConsumptionCallback> diseaseConsumptionCallbackManager
        = new ComplexCallbackHandleVector<Sim.DiseaseConsumptionCallback>(64);

    public bool                                                  drawStatusItems = true;
    public EdiblesManager                                        ediblesManager;
    public UtilityNetworkManager<ElectricalUtilityNetwork, Wire> electricalConduitSystem;

    [NonSerialized]
    public EnergySim energySim;

    [SerializeField]
    public TextAsset femaleNamesFile;

    public FetchManager fetchManager;

    [SerializeField]
    private Color flowColour;

    private readonly Dictionary<int, GameObjectPool> fxPools = new Dictionary<int, GameObjectPool>();

    [SerializeField]
    private SpawnPoolData[] fxSpawnData;

    private readonly Dictionary<int, Action<Vector3, float>> fxSpawner = new Dictionary<int, Action<Vector3, float>>();

    [MyCmpGet]
    private GameScenePartitioner gameScenePartitioner;

    private readonly List<SolidInfo>                                 gameSolidInfo = new List<SolidInfo>();
    private          bool                                            gameStarted;
    public           ConduitFlow                                     gasConduitFlow;
    public           UtilityNetworkManager<FlowUtilityNetwork, Vent> gasConduitSystem;

    [SerializeField]
    public ConduitVisInfo gasConduitVisInfo;

    [SerializeField]
    private Material gasFlowMaterial;

    private Vector3                                         gasFlowPos;
    public  ConduitFlowVisualizer                           gasFlowVisualizer;
    private bool                                            hasFirstSimTickRun;
    private bool                                            isLoading;
    private bool                                            IsPaused;
    private HashedString                                    lastDrawnOverlayMode;
    public  ConduitFlow                                     liquidConduitFlow;
    public  UtilityNetworkManager<FlowUtilityNetwork, Vent> liquidConduitSystem;

    [SerializeField]
    public ConduitVisInfo liquidConduitVisInfo;

    [SerializeField]
    private Material liquidFlowMaterial;

    private Vector3               liquidFlowPos;
    public  ConduitFlowVisualizer liquidFlowVisualizer;

    [NonSerialized]
    public Player LocalPlayer;

    [NonSerialized]
    public LogicCircuitManager logicCircuitManager;

    public UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem;

    [SerializeField]
    public TextAsset maleNamesFile;

    public ComplexCallbackHandleVector<Sim.MassConsumedCallback> massConsumedCallbackManager
        = new ComplexCallbackHandleVector<Sim.MassConsumedCallback>(64);

    public ComplexCallbackHandleVector<Sim.MassEmittedCallback> massEmitCallbackManager
        = new ComplexCallbackHandleVector<Sim.MassEmittedCallback>(64);

    public  MingleCellTracker     mingleCellTracker;
    public  Action<GameSaveData>  OnLoad;
    public  Action<GameSaveData>  OnSave;
    public  System.Action         OnSpawnComplete;
    public  PlantElementAbsorbers plantElementAbsorbers;
    private PlayerController      playerController;
    public  GameObject            playerPrefab;
    private EntityCellVisualizer  previewVisualizer;
    private float                 previousGasConduitFlowDiscreteLerpPercent    = -1f;
    private float                 previousLiquidConduitFlowDiscreteLerpPercent = -1f;
    private HashedString          previousOverlayMode                          = OverlayModes.None.ID;
    private float                 previousSolidConduitFlowDiscreteLerpPercent  = -1f;

    public ComplexCallbackHandleVector<Sim.ConsumedRadiationCallback> radiationConsumedCallbackManager
        = new ComplexCallbackHandleVector<Sim.ConsumedRadiationCallback>(256);

    public RoomProber roomProber;
    public SafetyConditions safetyConditions = new SafetyConditions();
    private bool sandboxModeActive;
    public SavedInfo savedInfo;
    public GameObject screenManagerPrefab;
    private GameScreenManager screenMgr;
    public bool showExpandedTemperatures;
    public bool showGasConduitDisease;
    public bool showLiquidConduitDisease;
    private readonly List<SimActiveRegion> simActiveRegions = new List<SimActiveRegion>();
    public ComplexCallbackHandleVector<int> simComponentCallbackManager = new ComplexCallbackHandleVector<int>(256);
    public SimData simData = new SimData();
    private float simDt;
    private int simSubTick;
    private readonly HashSet<int> solidChangedFilter = new HashSet<int>();
    public SolidConduitFlow solidConduitFlow;
    public UtilityNetworkManager<FlowUtilityNetwork, SolidConduit> solidConduitSystem;

    [SerializeField]
    public ConduitVisInfo solidConduitVisInfo;

    private          Vector3                    solidFlowPos;
    public           SolidConduitFlowVisualizer solidFlowVisualizer;
    private readonly List<SolidInfo>            solidInfo = new List<SolidInfo>();
    public           SpacecraftManager          spacecraftManager;
    public           SpaceScannerNetworkManager spaceScannerNetworkManager;
    public           TemperatureOverlayModes    temperatureOverlayMode;
    public           GameObject                 tempIntroScreenPrefab;
    public           List<Tag>                  tileOverlayFilters = new List<Tag>();
    public           Timelapser                 timelapser;
    public           UtilityNetworkTubesManager travelTubeSystem;

    [SerializeField]
    public UIColours uiColours = new UIColours();

    public Unlocks  unlocks;
    public UserMenu userMenu;
    public Element  VisualTunerElement;

    [NonSerialized]
    public World world;

    public        KInputHandler inputHandler { get; set; }
    public static Game          Instance     { get; private set; }

    public static Camera MainCamera {
        get {
            if (m_CachedCamera == null) m_CachedCamera = Camera.main;
            return m_CachedCamera;
        }
    }

    public bool SaveToCloudActive {
        get =>
            CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SaveToCloud).id == "Enabled";
        set {
            var value2 = value ? "Enabled" : "Disabled";
            CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, value2);
        }
    }

    public bool FastWorkersModeActive {
        get =>
            CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.FastWorkersMode).id ==
            "Enabled";
        set {
            var value2 = value ? "Enabled" : "Disabled";
            CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.FastWorkersMode, value2);
        }
    }

    public bool SandboxModeActive {
        get => sandboxModeActive;
        set {
            sandboxModeActive = value;
            Trigger(-1948169901);
            if (PlanScreen.Instance != null) PlanScreen.Instance.Refresh();
            if (BuildMenu.Instance  != null) BuildMenu.Instance.Refresh();
        }
    }

    public bool DebugOnlyBuildingsAllowed =>
        DebugHandler.enabled && (SandboxModeActive || DebugHandler.InstantBuildMode);

    public        StatusItemRenderer    statusItemRenderer    { get; private set; }
    public        PrioritizableRenderer prioritizableRenderer { get; private set; }
    public        float                 LastTimeWorkStarted   { get; private set; } = float.NegativeInfinity;
    public static bool                  IsQuitting()          { return quitting; }

    // 当 prefab 初始化时调用的方法
    protected override void OnPrefabInit() {
        // 记录从游戏启动以来的时间和当前场景名称
        DebugUtil.LogArgs(Time.realtimeSinceStartup, "Level Loaded....", SceneManager.GetActiveScene().name);
        
        // 注册建筑单元格可视化器的添加和移除事件处理程序
        Components.EntityCellVisualizers.OnAdd    += OnAddBuildingCellVisualizer;
        Components.EntityCellVisualizers.OnRemove += OnRemoveBuildingCellVisualizer;
        
        // 创建单例对象
        Singleton<KBatchedAnimUpdater>.CreateInstance();
        Singleton<CellChangeMonitor>.CreateInstance();
        
        // 初始化用户菜单
        userMenu = new UserMenu();
        
        // 清除实例映射
        SimTemperatureTransfer.ClearInstanceMap();
        StructureTemperatureComponents.ClearInstanceMap();
        ElementConsumer.ClearInstanceMap();
        
        // 在场景预加载时添加事件处理程序
        App.OnPreLoadScene    = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(StopBE));
        
        // 设置当前实例
        Instance              = this;
        
        // 初始化渲染器
        statusItemRenderer    = new StatusItemRenderer();
        prioritizableRenderer = new PrioritizableRenderer();
        
        // 加载事件哈希值
        LoadEventHashes();
        
        // 初始化保存信息的变量
        savedInfo.InitializeEmptyVariables();
        
        // 设置不同类型的流体在网格中的位置
        gasFlowPos    = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.GasConduits)          - 0.4f);
        liquidFlowPos = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.LiquidConduits)       - 0.4f);
        solidFlowPos  = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.SolidConduitContents) - 0.4f);
        
        // 预热所有着色器
        Shader.WarmupAllShaders();
        
        // 初始化数据库
        Db.Get();
        
        // 设置退出标志为 false
        quitting                = false;
        
        // 获取图层掩码
        PickupableLayer         = LayerMask.NameToLayer("Pickupable");
        BlockSelectionLayerMask = LayerMask.GetMask("BlockSelection");
        
        // 获取世界实例
        world                   = World.Instance;
        
        // 设置下一个唯一 ID
        KPrefabID.NextUniqueID  = KPlayerPrefs.GetInt(NextUniqueIDKey, 0);
        
        // 初始化电路管理器和能量模拟器
        circuitManager          = new CircuitManager();
        energySim               = new EnergySim();
        
        // 初始化各种管道系统管理器
        gasConduitSystem
            = new UtilityNetworkManager<FlowUtilityNetwork, Vent>(Grid.WidthInCells, Grid.HeightInCells, 13);
        
        liquidConduitSystem
            = new UtilityNetworkManager<FlowUtilityNetwork, Vent>(Grid.WidthInCells, Grid.HeightInCells, 17);
        
        electricalConduitSystem
            = new UtilityNetworkManager<ElectricalUtilityNetwork, Wire>(Grid.WidthInCells, Grid.HeightInCells, 27);
        
        logicCircuitSystem
            = new UtilityNetworkManager<LogicCircuitNetwork, LogicWire>(Grid.WidthInCells, Grid.HeightInCells, 32);
        
        logicCircuitManager = new LogicCircuitManager(logicCircuitSystem);
        travelTubeSystem    = new UtilityNetworkTubesManager(Grid.WidthInCells, Grid.HeightInCells, 35);
        solidConduitSystem
            = new UtilityNetworkManager<FlowUtilityNetwork, SolidConduit>(Grid.WidthInCells, Grid.HeightInCells, 21);
        
        // 初始化管道温度和疾病管理器
        conduitTemperatureManager = new ConduitTemperatureManager();
        conduitDiseaseManager = new ConduitDiseaseManager(conduitTemperatureManager);
        
        // 初始化管道流动模拟器和可视化器
        gasConduitFlow = new ConduitFlow(ConduitType.Gas, Grid.CellCount, gasConduitSystem, 1f, 0.25f);
        liquidConduitFlow = new ConduitFlow(ConduitType.Liquid, Grid.CellCount, liquidConduitSystem, 10f, 0.75f);
        solidConduitFlow = new SolidConduitFlow(Grid.CellCount, solidConduitSystem, 0.75f);
        
        gasFlowVisualizer = new ConduitFlowVisualizer(gasConduitFlow,
                                                      gasConduitVisInfo,
                                                      GlobalResources.Instance().ConduitOverlaySoundGas,
                                                      Lighting.Instance.Settings.GasConduit);
        
        liquidFlowVisualizer = new ConduitFlowVisualizer(liquidConduitFlow,
                                                         liquidConduitVisInfo,
                                                         GlobalResources.Instance().ConduitOverlaySoundLiquid,
                                                         Lighting.Instance.Settings.LiquidConduit);
        
        solidFlowVisualizer = new SolidConduitFlowVisualizer(solidConduitFlow,
                                                             solidConduitVisInfo,
                                                             GlobalResources.Instance().ConduitOverlaySoundSolid,
                                                             Lighting.Instance.Settings.SolidConduit);
        
        // 初始化其他系统和管理器
        accumulators          = new Accumulators();
        plantElementAbsorbers = new PlantElementAbsorbers();
        activeFX              = new ushort[Grid.CellCount];
        
        // 执行不安全的 prefab 初始化
        UnsafePrefabInit();
        
        // 设置全局着色器参数
        Shader.SetGlobalVector("_MetalParameters", new Vector4(0f, 0f, 0f, 0f));
        Shader.SetGlobalVector("_WaterParameters", new Vector4(0f, 0f, 0f, 0f));
        
        // 初始化特效生成器和路径查找器
        InitializeFXSpawners();
        PathFinder.Initialize();
        
        // 初始化游戏导航网格和屏幕管理器
        new GameNavGrids(Pathfinding.Instance);
        screenMgr                  = Util.KInstantiate(screenManagerPrefab).GetComponent<GameScreenManager>();
        
        // 初始化房间探测器和空间扫描网络管理器
        roomProber                 = new RoomProber();
        spaceScannerNetworkManager = new SpaceScannerNetworkManager();
        
        // 添加组件以管理获取和可食用物品
        fetchManager               = gameObject.AddComponent<FetchManager>();
        ediblesManager             = gameObject.AddComponent<EdiblesManager>();
        
        // 设置单元格更改监视器的网格大小
        Singleton<CellChangeMonitor>.Instance.SetGridSize(Grid.WidthInCells, Grid.HeightInCells);
        
        // 获取解锁组件
        unlocks             = GetComponent<Unlocks>();
        
        // 初始化已播放的更改列表
        changelistsPlayedOn = new List<uint>();
        changelistsPlayedOn.Add(626616U);
        
        // 生成日期
        dateGenerated = System.DateTime.UtcNow.ToString("U", CultureInfo.InvariantCulture);
    }

    public  void SetGameStarted()   { gameStarted = true; }
    public  bool GameStarted()      { return gameStarted; }
    private void UnsafePrefabInit() { StepTheSim(0f); }

    protected override void OnLoadLevel() {
        Unsubscribe(1798162660, MarkStatusItemRendererDirtyDelegate);
        Unsubscribe(1983128072, ActiveWorldChangedDelegate);
        base.OnLoadLevel();
    }

    private void MarkStatusItemRendererDirty(object data) { statusItemRenderer.MarkAllDirty(); }

    protected override void OnForcedCleanUp() {
        if (prioritizableRenderer != null) {
            prioritizableRenderer.Cleanup();
            prioritizableRenderer = null;
        }

        if (statusItemRenderer != null) {
            statusItemRenderer.Destroy();
            statusItemRenderer = null;
        }

        if (conduitTemperatureManager != null) conduitTemperatureManager.Shutdown();
        gasFlowVisualizer.FreeResources();
        liquidFlowVisualizer.FreeResources();
        solidFlowVisualizer.FreeResources();
        LightGridManager.Shutdown();
        RadiationGridManager.Shutdown();
        App.OnPreLoadScene = (System.Action)Delegate.Remove(App.OnPreLoadScene, new System.Action(StopBE));
        base.OnForcedCleanUp();
    }

    protected override void OnSpawn() {
        Debug.Log("-- GAME --");
        BrainScheduler                 = GetComponent<BrainScheduler>();
        PropertyTextures.FogOfWarScale = 0f;
        if (CameraController.Instance != null) CameraController.Instance.EnableFreeCamera(false);
        LocalPlayer = SpawnPlayer();
        WaterCubes.Instance.Init();
        SpeedControlScreen.Instance.Pause(false);
        LightGridManager.Initialise();
        RadiationGridManager.Initialise();
        RefreshRadiationLoop();
        UnsafeOnSpawn();
        Time.timeScale = 0f;
        if (tempIntroScreenPrefab != null) Util.KInstantiate(tempIntroScreenPrefab);
        if (SaveLoader.Instance.Cluster != null) {
            foreach (var worldGen in SaveLoader.Instance.Cluster.worlds)
                Reset(worldGen.data.gameSpawnData, worldGen.WorldOffset);

            NewBaseScreen.SetInitialCamera();
        }

        TagManager.FillMissingProperNames();
        CameraController.Instance.OrthographicSize = 20f;
        if (SaveLoader.Instance.loadedFromSave) {
            baseAlreadyCreated = true;
            Trigger(-1992507039);
            Trigger(-838649377);
        }

        var array = Resources.FindObjectsOfTypeAll(typeof(MeshRenderer));
        for (var i = 0; i < array.Length; i++) ((MeshRenderer)array[i]).reflectionProbeUsage = ReflectionProbeUsage.Off;
        Subscribe(1798162660, MarkStatusItemRendererDirtyDelegate);
        Subscribe(1983128072, ActiveWorldChangedDelegate);
        solidConduitFlow.Initialize();
        SimAndRenderScheduler.instance.Add(roomProber);
        SimAndRenderScheduler.instance.Add(spaceScannerNetworkManager);
        SimAndRenderScheduler.instance.Add(KComponentSpawn.instance);
        SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim200ms, AmountInstance>(AmountInstance.BatchUpdate);
        SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim1000ms, SolidTransferArm>(SolidTransferArm.BatchUpdate);
        if (!SaveLoader.Instance.loadedFromSave) {
            var settingConfig = CustomGameSettings.Instance.QualitySettings[CustomGameSettingConfigs.SandboxMode.id];
            var currentQualitySetting
                = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SandboxMode);

            SaveGame.Instance.sandboxEnabled = !settingConfig.IsDefaultLevel(currentQualitySetting.id);
        }

        mingleCellTracker = gameObject.AddComponent<MingleCellTracker>();
        if (Global.Instance != null) {
            Global.Instance.GetComponent<PerformanceMonitor>().Reset();
            Global.Instance.modManager.NotifyDialog(UI.FRONTEND.MOD_DIALOGS.SAVE_GAME_MODS_DIFFER.TITLE,
                                                    UI.FRONTEND.MOD_DIALOGS.SAVE_GAME_MODS_DIFFER.MESSAGE,
                                                    Global.Instance.globalCanvas);
        }
    }

    protected override void OnCleanUp() {
        base.OnCleanUp();
        SimAndRenderScheduler.instance.Remove(KComponentSpawn.instance);
        SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim200ms, AmountInstance>(null);
        SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim1000ms, SolidTransferArm>(null);
        DestroyInstances();
    }

    private new void OnDestroy() {
        base.OnDestroy();
        DestroyInstances();
    }

    private void UnsafeOnSpawn() { world.UpdateCellInfo(gameSolidInfo, callbackInfo, 0, null, 0, null); }

    private void RefreshRadiationLoop() {
        GameScheduler.Instance.Schedule("UpdateRadiation",
                                        1f,
                                        delegate {
                                            RadiationGridManager.Refresh();
                                            RefreshRadiationLoop();
                                        });
    }

    public void SetMusicEnabled(bool enabled) {
        if (enabled) {
            MusicManager.instance.PlaySong("Music_FrontEnd");
            return;
        }

        MusicManager.instance.StopSong("Music_FrontEnd");
    }

    private Player SpawnPlayer() {
        var component = Util.KInstantiate(playerPrefab, gameObject).GetComponent<Player>();
        component.ScreenManager = screenMgr;
        component.ScreenManager.StartScreen(ScreenPrefabs.Instance.HudScreen.gameObject);
        component.ScreenManager.StartScreen(ScreenPrefabs.Instance.HoverTextScreen.gameObject,
                                            null,
                                            GameScreenManager.UIRenderTarget.HoverTextScreen);

        component.ScreenManager.StartScreen(ScreenPrefabs.Instance.ToolTipScreen.gameObject,
                                            null,
                                            GameScreenManager.UIRenderTarget.HoverTextScreen);

        cameraController           = Util.KInstantiate(cameraControllerPrefab).GetComponent<CameraController>();
        component.CameraController = cameraController;
        if (KInputManager.currentController != null)
            KInputHandler.Add(KInputManager.currentController, cameraController, 1);
        else
            KInputHandler.Add(Global.GetInputManager().GetDefaultController(), cameraController, 1);

        Global.GetInputManager().usedMenus.Add(cameraController);
        playerController = component.GetComponent<PlayerController>();
        if (KInputManager.currentController != null)
            KInputHandler.Add(KInputManager.currentController, playerController, 20);
        else
            KInputHandler.Add(Global.GetInputManager().GetDefaultController(), playerController, 20);

        Global.GetInputManager().usedMenus.Add(playerController);
        return component;
    }

    public void SetDupePassableSolid(int cell, bool passable, bool solid) {
        Grid.DupePassable[cell] = passable;
        gameSolidInfo.Add(new SolidInfo(cell, solid));
    }

    private unsafe Sim.GameDataUpdate* StepTheSim(float dt) {
        Sim.GameDataUpdate* result;
        using (new KProfiler.Region("StepTheSim")) {
            var intPtr = IntPtr.Zero;
            using (new KProfiler.Region("WaitingForSim")) {
                if (Grid.Visible == null || Grid.Visible.Length == 0) {
                    Debug.LogError("Invalid Grid.Visible, what have you done?!");
                    return null;
                }

                intPtr = Sim.HandleMessage(SimMessageHashes.PrepareGameData, Grid.Visible.Length, Grid.Visible);
            }

            if (intPtr == IntPtr.Zero)
                result = null;
            else {
                var ptr = (Sim.GameDataUpdate*)(void*)intPtr;
                Grid.elementIdx                            = ptr->elementIdx;
                Grid.temperature                           = ptr->temperature;
                Grid.mass                                  = ptr->mass;
                Grid.radiation                             = ptr->radiation;
                Grid.properties                            = ptr->properties;
                Grid.strengthInfo                          = ptr->strengthInfo;
                Grid.insulation                            = ptr->insulation;
                Grid.diseaseIdx                            = ptr->diseaseIdx;
                Grid.diseaseCount                          = ptr->diseaseCount;
                Grid.AccumulatedFlowValues                 = ptr->accumulatedFlow;
                Grid.exposedToSunlight                     = (byte*)(void*)ptr->propertyTextureExposedToSunlight;
                PropertyTextures.externalFlowTex           = ptr->propertyTextureFlow;
                PropertyTextures.externalLiquidTex         = ptr->propertyTextureLiquid;
                PropertyTextures.externalExposedToSunlight = ptr->propertyTextureExposedToSunlight;
                var elements = ElementLoader.elements;
                simData.emittedMassEntries   = ptr->emittedMassEntries;
                simData.elementChunks        = ptr->elementChunkInfos;
                simData.buildingTemperatures = ptr->buildingTemperatures;
                simData.diseaseEmittedInfos  = ptr->diseaseEmittedInfos;
                simData.diseaseConsumedInfos = ptr->diseaseConsumedInfos;
                for (var i = 0; i < ptr->numSubstanceChangeInfo; i++) {
                    var substanceChangeInfo = ptr->substanceChangeInfo[i];
                    var element             = elements[substanceChangeInfo.newElemIdx];
                    Grid.Element[substanceChangeInfo.cellIdx] = element;
                }

                for (var j = 0; j < ptr->numSolidInfo; j++) {
                    var solidInfo = ptr->solidInfo[j];
                    if (!solidChangedFilter.Contains(solidInfo.cellIdx)) {
                        this.solidInfo.Add(new SolidInfo(solidInfo.cellIdx, solidInfo.isSolid != 0));
                        var solid = solidInfo.isSolid != 0;
                        Grid.SetSolid(solidInfo.cellIdx, solid, CellEventLogger.Instance.SimMessagesSolid);
                    }
                }

                for (var k = 0; k < ptr->numCallbackInfo; k++) {
                    var callbackInfo = ptr->callbackInfo[k];
                    var handle       = new HandleVector<CallbackInfo>.Handle { index = callbackInfo.callbackIdx };
                    if (!IsManuallyReleasedHandle(handle)) this.callbackInfo.Add(new Klei.CallbackInfo(handle));
                }

                var numSpawnFallingLiquidInfo = ptr->numSpawnFallingLiquidInfo;
                for (var l = 0; l < numSpawnFallingLiquidInfo; l++) {
                    var spawnFallingLiquidInfo = ptr->spawnFallingLiquidInfo[l];
                    FallingWater.instance.AddParticle(spawnFallingLiquidInfo.cellIdx,
                                                      spawnFallingLiquidInfo.elemIdx,
                                                      spawnFallingLiquidInfo.mass,
                                                      spawnFallingLiquidInfo.temperature,
                                                      spawnFallingLiquidInfo.diseaseIdx,
                                                      spawnFallingLiquidInfo.diseaseCount);
                }

                var numDigInfo = ptr->numDigInfo;
                var component  = world.GetComponent<WorldDamage>();
                for (var m = 0; m < numDigInfo; m++) {
                    var spawnOreInfo = ptr->digInfo[m];
                    if (spawnOreInfo.temperature <= 0f && spawnOreInfo.mass > 0f)
                        Debug.LogError("Sim is telling us to spawn a zero temperature object. This shouldn't be possible because I have asserts in the dll about this....");

                    component.OnDigComplete(spawnOreInfo.cellIdx,
                                            spawnOreInfo.mass,
                                            spawnOreInfo.temperature,
                                            spawnOreInfo.elemIdx,
                                            spawnOreInfo.diseaseIdx,
                                            spawnOreInfo.diseaseCount);
                }

                var numSpawnOreInfo = ptr->numSpawnOreInfo;
                for (var n = 0; n < numSpawnOreInfo; n++) {
                    var spawnOreInfo2 = ptr->spawnOreInfo[n];
                    var position      = Grid.CellToPosCCC(spawnOreInfo2.cellIdx, Grid.SceneLayer.Ore);
                    var element2      = ElementLoader.elements[spawnOreInfo2.elemIdx];
                    if (spawnOreInfo2.temperature <= 0f && spawnOreInfo2.mass > 0f)
                        Debug.LogError("Sim is telling us to spawn a zero temperature object. This shouldn't be possible because I have asserts in the dll about this....");

                    element2.substance.SpawnResource(position,
                                                     spawnOreInfo2.mass,
                                                     spawnOreInfo2.temperature,
                                                     spawnOreInfo2.diseaseIdx,
                                                     spawnOreInfo2.diseaseCount);
                }

                var numSpawnFXInfo = ptr->numSpawnFXInfo;
                for (var num = 0; num < numSpawnFXInfo; num++) {
                    var spawnFXInfo = ptr->spawnFXInfo[num];
                    SpawnFX((SpawnFXHashes)spawnFXInfo.fxHash, spawnFXInfo.cellIdx, spawnFXInfo.rotation);
                }

                var component2          = world.GetComponent<UnstableGroundManager>();
                var numUnstableCellInfo = ptr->numUnstableCellInfo;
                for (var num2 = 0; num2 < numUnstableCellInfo; num2++) {
                    var unstableCellInfo = ptr->unstableCellInfo[num2];
                    if (unstableCellInfo.fallingInfo == 0)
                        component2.Spawn(unstableCellInfo.cellIdx,
                                         ElementLoader.elements[unstableCellInfo.elemIdx],
                                         unstableCellInfo.mass,
                                         unstableCellInfo.temperature,
                                         unstableCellInfo.diseaseIdx,
                                         unstableCellInfo.diseaseCount);
                }

                var numWorldDamageInfo = ptr->numWorldDamageInfo;
                for (var num3 = 0; num3 < numWorldDamageInfo; num3++) {
                    var damage_info = ptr->worldDamageInfo[num3];
                    WorldDamage.Instance.ApplyDamage(damage_info);
                }

                for (var num4 = 0; num4 < ptr->numRemovedMassEntries; num4++)
                    ElementConsumer.AddMass(ptr->removedMassEntries[num4]);

                var numMassConsumedCallbacks = ptr->numMassConsumedCallbacks;
                var handle2 = default(HandleVector<ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle);
                for (var num5 = 0; num5 < numMassConsumedCallbacks; num5++) {
                    var massConsumedCallback = ptr->massConsumedCallbacks[num5];
                    handle2.index = massConsumedCallback.callbackIdx;
                    var complexCallbackInfo = massConsumedCallbackManager.Release(handle2, "massConsumedCB");
                    if (complexCallbackInfo.cb != null)
                        complexCallbackInfo.cb(massConsumedCallback, complexCallbackInfo.callbackData);
                }

                var numMassEmittedCallbacks = ptr->numMassEmittedCallbacks;
                var handle3 = default(HandleVector<ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle);
                for (var num6 = 0; num6 < numMassEmittedCallbacks; num6++) {
                    var massEmittedCallback = ptr->massEmittedCallbacks[num6];
                    handle3.index = massEmittedCallback.callbackIdx;
                    if (massEmitCallbackManager.IsVersionValid(handle3)) {
                        var item = massEmitCallbackManager.GetItem(handle3);
                        if (item.cb != null) item.cb(massEmittedCallback, item.callbackData);
                    }
                }

                var numDiseaseConsumptionCallbacks = ptr->numDiseaseConsumptionCallbacks;
                var handle4 = default(HandleVector<ComplexCallbackInfo<Sim.DiseaseConsumptionCallback>>.Handle);
                for (var num7 = 0; num7 < numDiseaseConsumptionCallbacks; num7++) {
                    var diseaseConsumptionCallback = ptr->diseaseConsumptionCallbacks[num7];
                    handle4.index = diseaseConsumptionCallback.callbackIdx;
                    if (diseaseConsumptionCallbackManager.IsVersionValid(handle4)) {
                        var item2 = diseaseConsumptionCallbackManager.GetItem(handle4);
                        if (item2.cb != null) item2.cb(diseaseConsumptionCallback, item2.callbackData);
                    }
                }

                var numComponentStateChangedMessages = ptr->numComponentStateChangedMessages;
                var handle5                          = default(HandleVector<ComplexCallbackInfo<int>>.Handle);
                for (var num8 = 0; num8 < numComponentStateChangedMessages; num8++) {
                    var componentStateChangedMessage = ptr->componentStateChangedMessages[num8];
                    handle5.index = componentStateChangedMessage.callbackIdx;
                    if (simComponentCallbackManager.IsVersionValid(handle5)) {
                        var complexCallbackInfo2
                            = simComponentCallbackManager.Release(handle5, "component state changed cb");

                        if (complexCallbackInfo2.cb != null)
                            complexCallbackInfo2.cb(componentStateChangedMessage.simHandle,
                                                    complexCallbackInfo2.callbackData);
                    }
                }

                var numRadiationConsumedCallbacks = ptr->numRadiationConsumedCallbacks;
                var handle6 = default(HandleVector<ComplexCallbackInfo<Sim.ConsumedRadiationCallback>>.Handle);
                for (var num9 = 0; num9 < numRadiationConsumedCallbacks; num9++) {
                    var consumedRadiationCallback = ptr->radiationConsumedCallbacks[num9];
                    handle6.index = consumedRadiationCallback.callbackIdx;
                    var complexCallbackInfo3 = radiationConsumedCallbackManager.Release(handle6, "radiationConsumedCB");
                    if (complexCallbackInfo3.cb != null)
                        complexCallbackInfo3.cb(consumedRadiationCallback, complexCallbackInfo3.callbackData);
                }

                var numElementChunkMeltedInfos = ptr->numElementChunkMeltedInfos;
                for (var num10 = 0; num10 < numElementChunkMeltedInfos; num10++)
                    SimTemperatureTransfer.DoOreMeltTransition(ptr->elementChunkMeltedInfos[num10].handle);

                var numBuildingOverheatInfos = ptr->numBuildingOverheatInfos;
                for (var num11 = 0; num11 < numBuildingOverheatInfos; num11++)
                    StructureTemperatureComponents.DoOverheat(ptr->buildingOverheatInfos[num11].handle);

                var numBuildingNoLongerOverheatedInfos = ptr->numBuildingNoLongerOverheatedInfos;
                for (var num12 = 0; num12 < numBuildingNoLongerOverheatedInfos; num12++)
                    StructureTemperatureComponents.DoNoLongerOverheated(ptr->buildingNoLongerOverheatedInfos[num12]
                                                                            .handle);

                var numBuildingMeltedInfos = ptr->numBuildingMeltedInfos;
                for (var num13 = 0; num13 < numBuildingMeltedInfos; num13++)
                    StructureTemperatureComponents.DoStateTransition(ptr->buildingMeltedInfos[num13].handle);

                var numCellMeltedInfos = ptr->numCellMeltedInfos;
                for (var num14 = 0; num14 < numCellMeltedInfos; num14++) {
                    var gameCell   = ptr->cellMeltedInfos[num14].gameCell;
                    var gameObject = Grid.Objects[gameCell, 9];
                    if (gameObject != null) {
                        gameObject.Trigger(675471409);
                        Util.KDestroyGameObject(gameObject);
                    }
                }

                if (dt > 0f) {
                    conduitTemperatureManager.Sim200ms(0.2f);
                    conduitDiseaseManager.Sim200ms(0.2f);
                    gasConduitFlow.Sim200ms(0.2f);
                    liquidConduitFlow.Sim200ms(0.2f);
                    solidConduitFlow.Sim200ms(0.2f);
                    accumulators.Sim200ms(0.2f);
                    plantElementAbsorbers.Sim200ms(0.2f);
                }

                Sim.DebugProperties debugProperties;
                debugProperties.buildingTemperatureScale           = 100f;
                debugProperties.buildingToBuildingTemperatureScale = 0.001f;
                debugProperties.biomeTemperatureLerpRate           = 0.001f;
                debugProperties.isDebugEditing = DebugPaintElementScreen.Instance != null &&
                                                 DebugPaintElementScreen.Instance.gameObject.activeSelf
                                                     ? 1
                                                     : 0;

                debugProperties.pad0 = debugProperties.pad1 = debugProperties.pad2 = 0;
                SimMessages.SetDebugProperties(debugProperties);
                if (dt > 0f) {
                    if (circuitManager != null) circuitManager.Sim200msFirst(dt);
                    if (energySim      != null) energySim.EnergySim200ms(dt);
                    if (circuitManager != null) circuitManager.Sim200msLast(dt);
                }

                result = ptr;
            }
        }

        return result;
    }

    public void AddSolidChangedFilter(int    cell) { solidChangedFilter.Add(cell); }
    public void RemoveSolidChangedFilter(int cell) { solidChangedFilter.Remove(cell); }
    public void SetIsLoading()                     { isLoading = true; }
    public bool IsLoading()                        { return isLoading; }

    private void ShowDebugCellInfo() {
        var mouseCell = DebugHandler.GetMouseCell();
        var num       = 0;
        var num2      = 0;
        Grid.CellToXY(mouseCell, out num, out num2);
        var text = string.Concat(mouseCell.ToString(), " (", num.ToString(), ", ", num2.ToString(), ")");
        DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
    }

    public void ForceSimStep() {
        DebugUtil.LogArgs("Force-stepping the sim");
        simDt = 0.2f;
    }

    private void Update() {
        if (isLoading) return;

        var deltaTime                                                    = Time.deltaTime;
        if (Debug.developerConsoleVisible) Debug.developerConsoleVisible = false;
        if (DebugHandler.DebugCellInfo) ShowDebugCellInfo();
        gasConduitSystem.Update();
        liquidConduitSystem.Update();
        solidConduitSystem.Update();
        circuitManager.RenderEveryTick(deltaTime);
        logicCircuitManager.RenderEveryTick(deltaTime);
        solidConduitFlow.RenderEveryTick(deltaTime);
        Pathfinding.Instance.RenderEveryTick();
        Singleton<CellChangeMonitor>.Instance.RenderEveryTick();
        SimEveryTick(deltaTime);
    }

    private void SimEveryTick(float dt) {
        dt    =  Mathf.Min(dt, 0.2f);
        simDt += dt;
        if (simDt >= 0.016666668f) {
            do {
                simSubTick++;
                simSubTick %= 12;
                if (simSubTick == 0) {
                    hasFirstSimTickRun = true;
                    UnsafeSim200ms(0.2f);
                }

                if (hasFirstSimTickRun) Singleton<StateMachineUpdater>.Instance.AdvanceOneSimSubTick();
                simDt -= 0.016666668f;
            } while (simDt >= 0.016666668f);

            return;
        }

        UnsafeSim200ms(0f);
    }

    private unsafe void UnsafeSim200ms(float dt) {
        simActiveRegions.Clear();
        foreach (var worldContainer in ClusterManager.Instance.WorldContainers)
            if (worldContainer.IsDiscovered) {
                var simActiveRegion = new SimActiveRegion();
                simActiveRegion.region
                    = new Pair<Vector2I, Vector2I>(worldContainer.WorldOffset,
                                                   worldContainer.WorldOffset + worldContainer.WorldSize);

                simActiveRegion.currentSunlightIntensity        = worldContainer.currentSunlightIntensity;
                simActiveRegion.currentCosmicRadiationIntensity = worldContainer.currentCosmicIntensity;
                simActiveRegions.Add(simActiveRegion);
            }

        Debug.Assert(simActiveRegions.Count > 0, "Cannot send a frame to the sim with zero active regions");
        SimMessages.NewGameFrame(dt, simActiveRegions);
        var ptr = StepTheSim(dt);
        if (ptr == null) {
            Debug.LogError("UNEXPECTED!");
            return;
        }

        if (ptr->numFramesProcessed <= 0) return;

        gameSolidInfo.AddRange(solidInfo);
        world.UpdateCellInfo(gameSolidInfo,
                             callbackInfo,
                             ptr->numSolidSubstanceChangeInfo,
                             ptr->solidSubstanceChangeInfo,
                             ptr->numLiquidChangeInfo,
                             ptr->liquidChangeInfo);

        gameSolidInfo.Clear();
        solidInfo.Clear();
        callbackInfo.Clear();
        callbackManagerManuallyReleasedHandles.Clear();
        Pathfinding.Instance.UpdateNavGrids();
    }

    private void LateUpdateComponents() { UpdateOverlayScreen(); }

    private void OnAddBuildingCellVisualizer(EntityCellVisualizer entity_cell_visualizer) {
        lastDrawnOverlayMode = default(HashedString);
        if (PlayerController.Instance != null) {
            var buildTool = PlayerController.Instance.ActiveTool as BuildTool;
            if (buildTool != null && buildTool.visualizer == entity_cell_visualizer.gameObject)
                previewVisualizer = entity_cell_visualizer;
        }
    }

    private void OnRemoveBuildingCellVisualizer(EntityCellVisualizer entity_cell_visualizer) {
        if (previewVisualizer == entity_cell_visualizer) previewVisualizer = null;
    }

    private void UpdateOverlayScreen() {
        if (OverlayScreen.Instance == null) return;

        var mode = OverlayScreen.Instance.GetMode();
        if (previewVisualizer != null) previewVisualizer.DrawIcons(mode);
        if (mode              == lastDrawnOverlayMode) return;

        foreach (var entityCellVisualizer in Components.EntityCellVisualizers.Items)
            entityCellVisualizer.DrawIcons(mode);

        lastDrawnOverlayMode = mode;
    }

    public void ForceOverlayUpdate(bool clearLastMode = false) {
        previousOverlayMode = OverlayModes.None.ID;
        if (clearLastMode) lastDrawnOverlayMode = OverlayModes.None.ID;
    }

    private void LateUpdate() {
        if (OnSpawnComplete != null) {
            OnSpawnComplete();
            OnSpawnComplete = null;
        }

        if (Time.timeScale == 0f && !IsPaused) {
            IsPaused = true;
            Trigger(-1788536802, IsPaused);
        } else if (Time.timeScale != 0f && IsPaused) {
            IsPaused = false;
            Trigger(-1788536802, IsPaused);
        }

        if (Input.GetMouseButton(0)) {
            VisualTunerElement = null;
            var mouseCell = DebugHandler.GetMouseCell();
            if (Grid.IsValidCell(mouseCell)) {
                var visualTunerElement = Grid.Element[mouseCell];
                VisualTunerElement = visualTunerElement;
            }
        }

        gasConduitSystem.Update();
        liquidConduitSystem.Update();
        solidConduitSystem.Update();
        var mode = SimDebugView.Instance.GetMode();
        if (mode != previousOverlayMode) {
            previousOverlayMode = mode;
            if (mode == OverlayModes.LiquidConduits.ID) {
                liquidFlowVisualizer.ColourizePipeContents(true, true);
                gasFlowVisualizer.ColourizePipeContents(false, true);
                solidFlowVisualizer.ColourizePipeContents(false, true);
            } else if (mode == OverlayModes.GasConduits.ID) {
                liquidFlowVisualizer.ColourizePipeContents(false, true);
                gasFlowVisualizer.ColourizePipeContents(true, true);
                solidFlowVisualizer.ColourizePipeContents(false, true);
            } else if (mode == OverlayModes.SolidConveyor.ID) {
                liquidFlowVisualizer.ColourizePipeContents(false, true);
                gasFlowVisualizer.ColourizePipeContents(false, true);
                solidFlowVisualizer.ColourizePipeContents(true, true);
            } else {
                liquidFlowVisualizer.ColourizePipeContents(false, false);
                gasFlowVisualizer.ColourizePipeContents(false, false);
                solidFlowVisualizer.ColourizePipeContents(false, false);
            }
        }

        gasFlowVisualizer.Render(gasFlowPos.z,
                                 0,
                                 gasConduitFlow.ContinuousLerpPercent,
                                 mode                               == OverlayModes.GasConduits.ID &&
                                 gasConduitFlow.DiscreteLerpPercent != previousGasConduitFlowDiscreteLerpPercent);

        liquidFlowVisualizer.Render(liquidFlowPos.z,
                                    0,
                                    liquidConduitFlow.ContinuousLerpPercent,
                                    mode == OverlayModes.LiquidConduits.ID &&
                                    liquidConduitFlow.DiscreteLerpPercent !=
                                    previousLiquidConduitFlowDiscreteLerpPercent);

        solidFlowVisualizer.Render(solidFlowPos.z,
                                   0,
                                   solidConduitFlow.ContinuousLerpPercent,
                                   mode                                 == OverlayModes.SolidConveyor.ID &&
                                   solidConduitFlow.DiscreteLerpPercent != previousSolidConduitFlowDiscreteLerpPercent);

        previousGasConduitFlowDiscreteLerpPercent
            = mode == OverlayModes.GasConduits.ID ? gasConduitFlow.DiscreteLerpPercent : -1f;

        previousLiquidConduitFlowDiscreteLerpPercent
            = mode == OverlayModes.LiquidConduits.ID ? liquidConduitFlow.DiscreteLerpPercent : -1f;

        previousSolidConduitFlowDiscreteLerpPercent
            = mode == OverlayModes.SolidConveyor.ID ? solidConduitFlow.DiscreteLerpPercent : -1f;

        var vector  = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
        var vector2 = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
        Shader.SetGlobalVector("_WsToCs",
                               new Vector4(vector.x               / Grid.WidthInCells,
                                           vector.y               / Grid.HeightInCells,
                                           (vector2.x - vector.x) / Grid.WidthInCells,
                                           (vector2.y - vector.y) / Grid.HeightInCells));

        var activeWorld = ClusterManager.Instance.activeWorld;
        var worldOffset = activeWorld.WorldOffset;
        var worldSize   = activeWorld.WorldSize;
        var value = new Vector4((vector.x  - worldOffset.x) / worldSize.x,
                                (vector.y  - worldOffset.y) / worldSize.y,
                                (vector2.x - vector.x)      / worldSize.x,
                                (vector2.y - vector.y)      / worldSize.y);

        Shader.SetGlobalVector("_WsToCcs", value);
        if (drawStatusItems) {
            statusItemRenderer.RenderEveryTick();
            prioritizableRenderer.RenderEveryTick();
        }

        LateUpdateComponents();
        Singleton<StateMachineUpdater>.Instance.Render(Time.unscaledDeltaTime);
        Singleton<StateMachineUpdater>.Instance.RenderEveryTick(Time.unscaledDeltaTime);
        if (SelectTool.Instance != null && SelectTool.Instance.selected != null) {
            var component = SelectTool.Instance.selected.GetComponent<Navigator>();
            if (component != null) component.DrawPath();
        }

        KFMOD.RenderEveryTick(Time.deltaTime);
        if (GenericGameSettings.instance.performanceCapture.waitTime != 0f) UpdatePerformanceCapture();
    }

    private void UpdatePerformanceCapture() {
        if (IsPaused && SpeedControlScreen.Instance != null) SpeedControlScreen.Instance.Unpause();
        if (Time.timeSinceLevelLoad < GenericGameSettings.instance.performanceCapture.waitTime) return;

        var num      = 626616U;
        var text     = System.DateTime.Now.ToShortDateString();
        var text2    = System.DateTime.Now.ToShortTimeString();
        var fileName = Path.GetFileName(GenericGameSettings.instance.performanceCapture.saveGame);
        var text3    = "Version,Date,Time,SaveGame";
        var text4    = string.Format("{0},{1},{2},{3}", num, text, text2, fileName);
        var num2     = 0.1f;
        if (GenericGameSettings.instance.performanceCapture.gcStats) {
            Debug.Log("Begin GC profiling...");
            var realtimeSinceStartup = Time.realtimeSinceStartup;
            GC.Collect();
            num2 = Time.realtimeSinceStartup - realtimeSinceStartup;
            Debug.Log("\tGC.Collect() took " + num2 + " seconds");
            var memorySnapshot = new MemorySnapshot();
            var format         = "{0},{1},{2},{3}";
            var path           = "./memory/GCTypeMetrics.csv";
            if (!File.Exists(path))
                using (var streamWriter = new StreamWriter(path)) {
                    streamWriter.WriteLine(format, text3, "Type", "Instances", "References");
                }

            using (var streamWriter2 = new StreamWriter(path, true)) {
                foreach (var typeData in memorySnapshot.types.Values)
                    streamWriter2.WriteLine(format,
                                            text4,
                                            "\"" + typeData.type + "\"",
                                            typeData.instanceCount,
                                            typeData.refCount);
            }

            Debug.Log("...end GC profiling");
        }

        var fps = Global.Instance.GetComponent<PerformanceMonitor>().FPS;
        Directory.CreateDirectory("./memory");
        var format2 = "{0},{1},{2}";
        var path2   = "./memory/GeneralMetrics.csv";
        if (!File.Exists(path2))
            using (var streamWriter3 = new StreamWriter(path2)) {
                streamWriter3.WriteLine(format2, text3, "GCDuration", "FPS");
            }

        using (var streamWriter4 = new StreamWriter(path2, true)) {
            streamWriter4.WriteLine(format2, text4, num2, fps);
        }

        GenericGameSettings.instance.performanceCapture.waitTime = 0f;
        App.Quit();
    }

    public void Reset(GameSpawnData gsd, Vector2I world_offset) {
        using (new KProfiler.Region("World.Reset")) {
            if (gsd != null)
                foreach (var keyValuePair in gsd.preventFoWReveal)
                    if (keyValuePair.Value) {
                        var v = new Vector2I(keyValuePair.Key.X + world_offset.X, keyValuePair.Key.Y + world_offset.Y);
                        Grid.PreventFogOfWarReveal[Grid.PosToCell(v)] = keyValuePair.Value;
                    }
        }
    }

    private void OnApplicationQuit() {
        quitting = true;
        Sim.Shutdown();
        AudioMixer.Destroy();
        if (screenMgr != null && screenMgr.gameObject != null) Destroy(screenMgr.gameObject);
        Console.WriteLine("Game.OnApplicationQuit()");
    }

    private void InitializeFXSpawners() {
        for (var i = 0; i < fxSpawnData.Length; i++) {
            var fx_idx = i;
            fxSpawnData[fx_idx].fxPrefab.SetActive(false);
            var fx_mask = (ushort)(1 << fx_idx);
            Action<SpawnFXHashes, GameObject> destroyer = delegate(SpawnFXHashes fxid, GameObject go) {
                                                              if (!IsQuitting()) {
                                                                  var num   = Grid.PosToCell(go);
                                                                  var array = activeFX;
                                                                  var num2  = num;
                                                                  array[num2] &= ~fx_mask;
                                                                  go.GetComponent<KAnimControllerBase>().enabled
                                                                      = false;

                                                                  fxPools[(int)fxid].ReleaseInstance(go);
                                                              }
                                                          };

            Func<GameObject> instantiator = delegate {
                                                var gameObject
                                                    = GameUtil.KInstantiate(fxSpawnData[fx_idx].fxPrefab,
                                                                            Grid.SceneLayer.Front);

                                                var component = gameObject.GetComponent<KBatchedAnimController>();
                                                component.enabled = false;
                                                gameObject.SetActive(true);
                                                component.onDestroySelf = delegate(GameObject go) {
                                                                              destroyer(fxSpawnData[fx_idx].id, go);
                                                                          };

                                                return gameObject;
                                            };

            var pool = new GameObjectPool(instantiator, fxSpawnData[fx_idx].initialCount);
            fxPools[(int)fxSpawnData[fx_idx].id] = pool;
            fxSpawner[(int)fxSpawnData[fx_idx].id] = delegate(Vector3 pos, float rotation) {
                                                         Action<object> action = delegate {
                                                             var num = Grid.PosToCell(pos);
                                                             if ((activeFX[num] & fx_mask) == 0) {
                                                                 var array = activeFX;
                                                                 var num2  = num;
                                                                 array[num2] |= fx_mask;
                                                                 var instance       = pool.GetInstance();
                                                                 var spawnPoolData  = fxSpawnData[fx_idx];
                                                                 var rotation       = Quaternion.identity;
                                                                 var flipX          = false;
                                                                 var s              = spawnPoolData.initialAnim;
                                                                 var rotationConfig = spawnPoolData.rotationConfig;
                                                                 if (rotationConfig != SpawnRotationConfig.Normal) {
                                                                     if (rotationConfig ==
                                                                         SpawnRotationConfig.StringName) {
                                                                         var num3 = (int)(rotation / 90f);
                                                                         if (num3 < 0)
                                                                             num3 += spawnPoolData.rotationData
                                                                                 .Length;

                                                                         s = spawnPoolData.rotationData[num3]
                                                                             .animName;

                                                                         flipX = spawnPoolData.rotationData[num3]
                                                                             .flip;
                                                                     }
                                                                 } else
                                                                     rotation = Quaternion.Euler(0f, 0f, rotation);

                                                                 pos += spawnPoolData.spawnOffset;
                                                                 var vector = Random.insideUnitCircle;
                                                                 vector.x *= spawnPoolData.spawnRandomOffset.x;
                                                                 vector.y *= spawnPoolData.spawnRandomOffset.y;
                                                                 vector   =  rotation * vector;
                                                                 pos.x    += vector.x;
                                                                 pos.y    += vector.y;
                                                                 instance.transform.SetPosition(pos);
                                                                 instance.transform.rotation = rotation;
                                                                 var component = instance
                                                                     .GetComponent<KBatchedAnimController>();

                                                                 component.FlipX      = flipX;
                                                                 component.TintColour = spawnPoolData.colour;
                                                                 component.Play(s);
                                                                 component.enabled = true;
                                                             }
                                                         };

                                                         if (Instance.IsPaused) {
                                                             action(null);
                                                             return;
                                                         }

                                                         GameScheduler.Instance.Schedule("SpawnFX", 0f, action);
                                                     };
        }
    }

    public void SpawnFX(SpawnFXHashes fx_id, int cell, float rotation) {
        var vector = Grid.CellToPosCBC(cell, Grid.SceneLayer.Front);
        if (CameraController.Instance.IsVisiblePos(vector)) fxSpawner[(int)fx_id](vector, rotation);
    }

    public void SpawnFX(SpawnFXHashes fx_id, Vector3 pos, float rotation) { fxSpawner[(int)fx_id](pos, rotation); }
    public static void SaveSettings(BinaryWriter writer) { Serializer.Serialize(new Settings(Instance), writer); }

    public static void LoadSettings(Deserializer deserializer) {
        var settings = new Settings();
        deserializer.Deserialize(settings);
        KPlayerPrefs.SetInt(NextUniqueIDKey, settings.nextUniqueID);
        KleiMetrics.SetGameID(settings.gameID);
    }

    public void Save(BinaryWriter writer) {
        var gameSaveData = new GameSaveData();
        gameSaveData.gasConduitFlow             = gasConduitFlow;
        gameSaveData.liquidConduitFlow          = liquidConduitFlow;
        gameSaveData.fallingWater               = world.GetComponent<FallingWater>();
        gameSaveData.unstableGround             = world.GetComponent<UnstableGroundManager>();
        gameSaveData.worldDetail                = SaveLoader.Instance.clusterDetailSave;
        gameSaveData.debugWasUsed               = debugWasUsed;
        gameSaveData.customGameSettings         = CustomGameSettings.Instance;
        gameSaveData.storySetings               = StoryManager.Instance;
        gameSaveData.spaceScannerNetworkManager = Instance.spaceScannerNetworkManager;
        gameSaveData.autoPrioritizeRoles        = autoPrioritizeRoles;
        gameSaveData.advancedPersonalPriorities = advancedPersonalPriorities;
        gameSaveData.savedInfo                  = savedInfo;
        Debug.Assert(gameSaveData.worldDetail != null, "World detail null");
        gameSaveData.dateGenerated = dateGenerated;
        if (!changelistsPlayedOn.Contains(626616U)) changelistsPlayedOn.Add(626616U);
        gameSaveData.changelistsPlayedOn = changelistsPlayedOn;
        if (OnSave != null) OnSave(gameSaveData);
        Serializer.Serialize(gameSaveData, writer);
    }

    public void Load(Deserializer deserializer) {
        var gameSaveData = new GameSaveData();
        gameSaveData.gasConduitFlow             = gasConduitFlow;
        gameSaveData.liquidConduitFlow          = liquidConduitFlow;
        gameSaveData.fallingWater               = world.GetComponent<FallingWater>();
        gameSaveData.unstableGround             = world.GetComponent<UnstableGroundManager>();
        gameSaveData.worldDetail                = new WorldDetailSave();
        gameSaveData.customGameSettings         = CustomGameSettings.Instance;
        gameSaveData.storySetings               = StoryManager.Instance;
        gameSaveData.spaceScannerNetworkManager = Instance.spaceScannerNetworkManager;
        deserializer.Deserialize(gameSaveData);
        gasConduitFlow             = gameSaveData.gasConduitFlow;
        liquidConduitFlow          = gameSaveData.liquidConduitFlow;
        debugWasUsed               = gameSaveData.debugWasUsed;
        autoPrioritizeRoles        = gameSaveData.autoPrioritizeRoles;
        advancedPersonalPriorities = gameSaveData.advancedPersonalPriorities;
        dateGenerated              = gameSaveData.dateGenerated;
        changelistsPlayedOn        = gameSaveData.changelistsPlayedOn ?? new List<uint>();
        if (gameSaveData.dateGenerated.IsNullOrWhiteSpace()) dateGenerated = "Before U41 (Feb 2022)";
        DebugUtil.LogArgs("SAVEINFO");
        DebugUtil.LogArgs(" - Generated: "      + dateGenerated);
        DebugUtil.LogArgs(" - Played on: "      + string.Join(", ", changelistsPlayedOn));
        DebugUtil.LogArgs(" - Debug was used: " + Instance.debugWasUsed);
        savedInfo = gameSaveData.savedInfo;
        savedInfo.InitializeEmptyVariables();
        CustomGameSettings.Instance.Print();
        KCrashReporter.debugWasUsed = debugWasUsed;
        SaveLoader.Instance.SetWorldDetail(gameSaveData.worldDetail);
        if (OnLoad != null) OnLoad(gameSaveData);
    }

    public void SetAutoSaveCallbacks(SavingPreCB    activatePreCB,
                                     SavingActiveCB activateActiveCB,
                                     SavingPostCB   activatePostCB) {
        this.activatePreCB    = activatePreCB;
        this.activateActiveCB = activateActiveCB;
        this.activatePostCB   = activatePostCB;
    }

    public void StartDelayedInitialSave() { StartCoroutine(DelayedInitialSave()); }

    private IEnumerator DelayedInitialSave() {
        int num;
        for (var i = 0; i < 1; i = num) {
            yield return null;

            num = i + 1;
        }

        if (GenericGameSettings.instance.devAutoWorldGenActive) {
            foreach (var worldContainer in ClusterManager.Instance.WorldContainers) worldContainer.SetDiscovered(true);
            SaveGame.Instance.worldGenSpawner.SpawnEverything();
            SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().DEBUG_REVEAL_ENTIRE_MAP();
            if (CameraController.Instance != null) CameraController.Instance.EnableFreeCamera(true);
            for (var num2 = 0; num2       != Grid.WidthInCells * Grid.HeightInCells; num2++) Grid.Reveal(num2);
            GenericGameSettings.instance.devAutoWorldGenActive = false;
        }

        SaveLoader.Instance.InitialSave();
    }

    public void StartDelayedSave(string filename, bool isAutoSave = false, bool updateSavePointer = true) {
        if (activatePreCB != null) {
            activatePreCB(delegate { StartCoroutine(DelayedSave(filename, isAutoSave, updateSavePointer)); });
            return;
        }

        StartCoroutine(DelayedSave(filename, isAutoSave, updateSavePointer));
    }

    private IEnumerator DelayedSave(string filename, bool isAutoSave, bool updateSavePointer) {
        while (PlayerController.Instance.IsDragging()) yield return null;

        PlayerController.Instance.CancelDragging();
        PlayerController.Instance.AllowDragging(false);
        int num;
        for (var i = 0; i < 1; i = num) {
            yield return null;

            num = i + 1;
        }

        if (activateActiveCB != null) {
            activateActiveCB();
            for (var i = 0; i < 1; i = num) {
                yield return null;

                num = i + 1;
            }
        }

        SaveLoader.Instance.Save(filename, isAutoSave, updateSavePointer);
        if (activatePostCB != null) activatePostCB();
        for (var i = 0; i < 5; i = num) {
            yield return null;

            num = i + 1;
        }

        PlayerController.Instance.AllowDragging(true);
    }

    public void StartDelayed(int tick_delay, System.Action action) {
        StartCoroutine(DelayedExecutor(tick_delay, action));
    }

    private IEnumerator DelayedExecutor(int tick_delay, System.Action action) {
        int num;
        for (var i = 0; i < tick_delay; i = num) {
            yield return null;

            num = i + 1;
        }

        action();
    }

    private void LoadEventHashes() {
        foreach (var obj in Enum.GetValues(typeof(GameHashes))) {
            var hash = (GameHashes)obj;
            HashCache.Get().Add((int)hash, hash.ToString());
        }

        foreach (var obj2 in Enum.GetValues(typeof(UtilHashes))) {
            var hash2 = (UtilHashes)obj2;
            HashCache.Get().Add((int)hash2, hash2.ToString());
        }

        foreach (var obj3 in Enum.GetValues(typeof(UIHashes))) {
            var hash3 = (UIHashes)obj3;
            HashCache.Get().Add((int)hash3, hash3.ToString());
        }
    }

    public void StopFE() {
        if (SteamUGCService.Instance) SteamUGCService.Instance.enabled = false;
        AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSnapshot);
        if (MusicManager.instance.SongIsPlaying("Music_FrontEnd")) MusicManager.instance.StopSong("Music_FrontEnd");
        MainMenu.Instance.StopMainMenuMusic();
    }

    public void StartBE() {
        Resources.UnloadUnusedAssets();
        AudioMixer.instance.Reset();
        AudioMixer.instance.StartPersistentSnapshots();
        MusicManager.instance.ConfigureSongs();
        if (MusicManager.instance.ShouldPlayDynamicMusicLoadedGame()) MusicManager.instance.PlayDynamicMusic();
    }

    public void StopBE() {
        if (SteamUGCService.Instance) SteamUGCService.Instance.enabled = true;
        var loopingSoundManager                                        = LoopingSoundManager.Get();
        if (loopingSoundManager != null) loopingSoundManager.StopAllSounds();
        MusicManager.instance.KillAllSongs(STOP_MODE.ALLOWFADEOUT);
        AudioMixer.instance.StopPersistentSnapshots();
        foreach (var list in SaveLoader.Instance.saveManager.GetLists().Values) {
            foreach (var saveLoadRoot in list)
                if (saveLoadRoot.gameObject != null)
                    Util.KDestroyGameObject(saveLoadRoot.gameObject);
        }

        GetComponent<EntombedItemVisualizer>().Clear();
        SimTemperatureTransfer.ClearInstanceMap();
        StructureTemperatureComponents.ClearInstanceMap();
        ElementConsumer.ClearInstanceMap();
        KComponentSpawn.instance.comps.Clear();
        KInputHandler.Remove(Global.GetInputManager().GetDefaultController(), cameraController);
        KInputHandler.Remove(Global.GetInputManager().GetDefaultController(), playerController);
        Sim.Shutdown();
        SimAndRenderScheduler.instance.Reset();
        Resources.UnloadUnusedAssets();
    }

    public void SetStatusItemOffset(Transform transform, Vector3 offset) {
        statusItemRenderer.SetOffset(transform, offset);
    }

    public void AddStatusItem(Transform transform, StatusItem status_item) {
        statusItemRenderer.Add(transform, status_item);
    }

    public void RemoveStatusItem(Transform transform, StatusItem status_item) {
        statusItemRenderer.Remove(transform, status_item);
    }

    public  void StartedWork()                                     { LastTimeWorkStarted = Time.time; }
    private void SpawnOxygenBubbles(Vector3 position, float angle) { }

    public void ManualReleaseHandle(HandleVector<CallbackInfo>.Handle handle) {
        if (!handle.IsValid()) return;

        callbackManagerManuallyReleasedHandles.Add(handle.index);
        callbackManager.Release(handle);
    }

    private bool IsManuallyReleasedHandle(HandleVector<CallbackInfo>.Handle handle) {
        return !callbackManager.IsVersionValid(handle) && callbackManagerManuallyReleasedHandles.Contains(handle.index);
    }

    [ContextMenu("Print")]
    private void Print() {
        Console.WriteLine("This is a console writeline test");
        Debug.Log("This is a debug log test");
    }

    private void DestroyInstances() {
        lastGameObject = null;
        lastObj        = null;
        Db.Get().ResetProblematicDbs();
        GridSettings.ClearGrid();
        StateMachineManager.ResetParameters();
        ChoreTable.Instance.ResetParameters();
        BubbleManager.DestroyInstance();
        AmbientSoundManager.Destroy();
        AutoDisinfectableManager.DestroyInstance();
        BuildMenu.DestroyInstance();
        CancelTool.DestroyInstance();
        ClearTool.DestroyInstance();
        ChoreGroupManager.DestroyInstance();
        CO2Manager.DestroyInstance();
        ConsumerManager.DestroyInstance();
        CopySettingsTool.DestroyInstance();
        DateTime.DestroyInstance();
        DebugBaseTemplateButton.DestroyInstance();
        DebugPaintElementScreen.DestroyInstance();
        DetailsScreen.DestroyInstance();
        DietManager.DestroyInstance();
        DebugText.DestroyInstance();
        FactionManager.DestroyInstance();
        EmptyPipeTool.DestroyInstance();
        FetchListStatusItemUpdater.DestroyInstance();
        FishOvercrowingManager.DestroyInstance();
        FallingWater.DestroyInstance();
        GridCompositor.DestroyInstance();
        Infrared.DestroyInstance();
        KPrefabIDTracker.DestroyInstance();
        ManagementMenu.DestroyInstance();
        ClusterMapScreen.DestroyInstance();
        Messenger.DestroyInstance();
        LoopingSoundManager.DestroyInstance();
        MeterScreen.DestroyInstance();
        MinionGroupProber.DestroyInstance();
        NavPathDrawer.DestroyInstance();
        MinionIdentity.DestroyStatics();
        PathFinder.DestroyStatics();
        Pathfinding.DestroyInstance();
        PrebuildTool.DestroyInstance();
        PrioritizeTool.DestroyInstance();
        SelectTool.DestroyInstance();
        PopFXManager.DestroyInstance();
        ProgressBarsConfig.DestroyInstance();
        PropertyTextures.DestroyInstance();
        RationTracker.DestroyInstance();
        ReportManager.DestroyInstance();
        Research.DestroyInstance();
        RootMenu.DestroyInstance();
        SaveLoader.DestroyInstance();
        Scenario.DestroyInstance();
        SimDebugView.DestroyInstance();
        SpriteSheetAnimManager.DestroyInstance();
        ScheduleManager.DestroyInstance();
        Sounds.DestroyInstance();
        ToolMenu.DestroyInstance();
        WorldDamage.DestroyInstance();
        WaterCubes.DestroyInstance();
        WireBuildTool.DestroyInstance();
        VisibilityTester.DestroyInstance();
        Traces.DestroyInstance();
        TopLeftControlScreen.DestroyInstance();
        UtilityBuildTool.DestroyInstance();
        ReportScreen.DestroyInstance();
        ChorePreconditions.DestroyInstance();
        SandboxBrushTool.DestroyInstance();
        SandboxHeatTool.DestroyInstance();
        SandboxStressTool.DestroyInstance();
        SandboxCritterTool.DestroyInstance();
        SandboxClearFloorTool.DestroyInstance();
        GameScreenManager.DestroyInstance();
        GameScheduler.DestroyInstance();
        NavigationReservations.DestroyInstance();
        Tutorial.DestroyInstance();
        CameraController.DestroyInstance();
        CellEventLogger.DestroyInstance();
        GameFlowManager.DestroyInstance();
        Immigration.DestroyInstance();
        BuildTool.DestroyInstance();
        DebugTool.DestroyInstance();
        DeconstructTool.DestroyInstance();
        DisconnectTool.DestroyInstance();
        DigTool.DestroyInstance();
        DisinfectTool.DestroyInstance();
        HarvestTool.DestroyInstance();
        MopTool.DestroyInstance();
        MoveToLocationTool.DestroyInstance();
        PlaceTool.DestroyInstance();
        SpacecraftManager.DestroyInstance();
        GameplayEventManager.DestroyInstance();
        BuildingInventory.DestroyInstance();
        PlantSubSpeciesCatalog.DestroyInstance();
        SandboxDestroyerTool.DestroyInstance();
        SandboxFOWTool.DestroyInstance();
        SandboxFloodTool.DestroyInstance();
        SandboxSprinkleTool.DestroyInstance();
        StampTool.DestroyInstance();
        OnDemandUpdater.DestroyInstance();
        HoverTextScreen.DestroyInstance();
        ImmigrantScreen.DestroyInstance();
        OverlayMenu.DestroyInstance();
        NameDisplayScreen.DestroyInstance();
        PlanScreen.DestroyInstance();
        ResourceCategoryScreen.DestroyInstance();
        ResourceRemainingDisplayScreen.DestroyInstance();
        SandboxToolParameterMenu.DestroyInstance();
        SpeedControlScreen.DestroyInstance();
        Vignette.DestroyInstance();
        PlayerController.DestroyInstance();
        NotificationScreen.DestroyInstance();
        BuildingCellVisualizerResources.DestroyInstance();
        PauseScreen.DestroyInstance();
        SaveLoadRoot.DestroyStatics();
        KTime.DestroyInstance();
        DemoTimer.DestroyInstance();
        UIScheduler.DestroyInstance();
        SaveGame.DestroyInstance();
        GameClock.DestroyInstance();
        TimeOfDay.DestroyInstance();
        DeserializeWarnings.DestroyInstance();
        UISounds.DestroyInstance();
        RenderTextureDestroyer.DestroyInstance();
        HoverTextHelper.DestroyStatics();
        LoadScreen.DestroyInstance();
        LoadingOverlay.DestroyInstance();
        SimAndRenderScheduler.DestroyInstance();
        Singleton<CellChangeMonitor>.DestroyInstance();
        Singleton<StateMachineManager>.Instance.Clear();
        Singleton<StateMachineUpdater>.Instance.Clear();
        UpdateObjectCountParameter.Clear();
        MaterialSelectionPanel.ClearStatics();
        StarmapScreen.DestroyInstance();
        ClusterNameDisplayScreen.DestroyInstance();
        ClusterManager.DestroyInstance();
        ClusterGrid.DestroyInstance();
        PathFinderQueries.Reset();
        var instance = Singleton<KBatchedAnimUpdater>.Instance;
        if (instance != null) instance.InitializeGrid();
        GlobalChoreProvider.DestroyInstance();
        WorldSelector.DestroyInstance();
        ColonyDiagnosticUtility.DestroyInstance();
        DiscoveredResources.DestroyInstance();
        ClusterMapSelectTool.DestroyInstance();
        StoryManager.DestroyInstance();
        AnimEventHandlerManager.DestroyInstance();
        Instance           = null;
        BrainScheduler     = null;
        Grid.OnReveal      = null;
        VisualTunerElement = null;
        Assets.ClearOnAddPrefab();
        lastGameObject = null;
        lastObj        = null;
        (KComponentSpawn.instance.comps as GameComps).Clear();
    }

    [Serializable]
    public struct SavedInfo {
        [OnDeserialized]
        private void OnDeserialized() { InitializeEmptyVariables(); }

        public void InitializeEmptyVariables() {
            if (creaturePoopAmount          == null) creaturePoopAmount          = new Dictionary<Tag, float>();
            if (powerCreatedbyGeneratorType == null) powerCreatedbyGeneratorType = new Dictionary<Tag, float>();
        }

        public bool                   discoveredSurface;
        public bool                   discoveredOilField;
        public bool                   curedDisease;
        public bool                   blockedCometWithBunkerDoor;
        public Dictionary<Tag, float> creaturePoopAmount;
        public Dictionary<Tag, float> powerCreatedbyGeneratorType;
    }

    public struct CallbackInfo {
        public CallbackInfo(System.Action cb, bool manually_release = false) {
            this.cb         = cb;
            manuallyRelease = manually_release;
        }

        public System.Action cb;
        public bool          manuallyRelease;
    }

    public struct ComplexCallbackInfo<DataType> {
        public ComplexCallbackInfo(Action<DataType, object> cb, object callback_data, string debug_info) {
            this.cb      = cb;
            debugInfo    = debug_info;
            callbackData = callback_data;
        }

        public Action<DataType, object> cb;
        public object                   callbackData;
        public string                   debugInfo;
    }

    public class ComplexCallbackHandleVector<DataType> {
        private readonly HandleVector<ComplexCallbackInfo<DataType>> baseMgr;
        private readonly Dictionary<int, string>                     releaseInfo = new Dictionary<int, string>();

        public ComplexCallbackHandleVector(int initial_size) {
            baseMgr = new HandleVector<ComplexCallbackInfo<DataType>>(initial_size);
        }

        public HandleVector<ComplexCallbackInfo<DataType>>.Handle
            Add(Action<DataType, object> cb, object callback_data, string debug_info) {
            return baseMgr.Add(new ComplexCallbackInfo<DataType>(cb, callback_data, debug_info));
        }

        public ComplexCallbackInfo<DataType> GetItem(HandleVector<ComplexCallbackInfo<DataType>>.Handle handle) {
            ComplexCallbackInfo<DataType> item;
            try { item = baseMgr.GetItem(handle); } catch (Exception ex) {
                byte b;
                int  key;
                baseMgr.UnpackHandleUnchecked(handle, out b, out key);
                string str = null;
                if (releaseInfo.TryGetValue(key, out str))
                    KCrashReporter.Assert(false, "Trying to get data for handle that was already released by " + str);
                else
                    KCrashReporter.Assert(false, "Trying to get data for handle that was released ...... magically");

                throw ex;
            }

            return item;
        }

        public ComplexCallbackInfo<DataType> Release(HandleVector<ComplexCallbackInfo<DataType>>.Handle handle,
                                                     string                                             release_info) {
            ComplexCallbackInfo<DataType> result;
            try {
                byte b;
                int  key;
                baseMgr.UnpackHandle(handle, out b, out key);
                releaseInfo[key] = release_info;
                result           = baseMgr.Release(handle);
            } catch (Exception ex) {
                byte b;
                int  key;
                baseMgr.UnpackHandleUnchecked(handle, out b, out key);
                string str = null;
                if (releaseInfo.TryGetValue(key, out str))
                    KCrashReporter.Assert(false,
                                          release_info                                                  +
                                          "is trying to release handle but it was already released by " +
                                          str);
                else
                    KCrashReporter.Assert(false,
                                          release_info +
                                          "is trying to release a handle that was already released by some unknown thing");

                throw ex;
            }

            return result;
        }

        public void Clear() { baseMgr.Clear(); }

        public bool IsVersionValid(HandleVector<ComplexCallbackInfo<DataType>>.Handle handle) {
            return baseMgr.IsVersionValid(handle);
        }
    }

    [Serializable]
    public class ConduitVisInfo {
        public Color32 insulatedTint;
        public string  overlayInsulatedTintName;
        public Vector2 overlayMassScaleRange  = new Vector2f(1f,   1000f);
        public Vector2 overlayMassScaleValues = new Vector2f(0.1f, 1f);
        public string  overlayRadiantTintName;

        [Header("Overlay")]
        public string overlayTintName;

        public GameObject prefab;
        public Color32    radiantTint;

        [Header("Main View")]
        public Color32 tint;
    }

    private class WorldRegion {
        public  bool     isActive;
        private Vector2I max;
        private Vector2I min;
        public  Vector2I regionMin => min;
        public  Vector2I regionMax => max;

        public void UpdateGameActiveRegion(int x0, int y0, int x1, int y1) {
            min.x = Mathf.Max(0,  x0);
            min.y = Mathf.Max(0,  y0);
            max.x = Mathf.Max(x1, regionMax.x);
            max.y = Mathf.Max(y1, regionMax.y);
        }

        public void UpdateGameActiveRegion(Vector2I simActiveRegionMin, Vector2I simActiveRegionMax) {
            min = simActiveRegionMin;
            max = simActiveRegionMax;
        }
    }

    public class SimActiveRegion {
        public float                    currentCosmicRadiationIntensity;
        public float                    currentSunlightIntensity;
        public Pair<Vector2I, Vector2I> region;

        public SimActiveRegion() {
            region                          = default(Pair<Vector2I, Vector2I>);
            currentSunlightIntensity        = FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;
            currentCosmicRadiationIntensity = FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;
        }
    }

    private enum SpawnRotationConfig {
        Normal,
        StringName
    }

    [Serializable]
    private struct SpawnRotationData {
        public string animName;
        public bool   flip;
    }

    [Serializable]
    private struct SpawnPoolData {
        [HashedEnum]
        public SpawnFXHashes id;

        public int                 initialCount;
        public Color32             colour;
        public GameObject          fxPrefab;
        public string              initialAnim;
        public Vector3             spawnOffset;
        public Vector2             spawnRandomOffset;
        public SpawnRotationConfig rotationConfig;
        public SpawnRotationData[] rotationData;
    }

    [Serializable]
    private class Settings {
        public int gameID;
        public int nextUniqueID;

        public Settings(Game game) {
            nextUniqueID = KPrefabID.NextUniqueID;
            gameID       = KleiMetrics.GameID();
        }

        public Settings() { }
    }

    public class GameSaveData {
        public bool                       advancedPersonalPriorities;
        public bool                       autoPrioritizeRoles;
        public List<uint>                 changelistsPlayedOn;
        public CustomGameSettings         customGameSettings;
        public string                     dateGenerated;
        public bool                       debugWasUsed;
        public FallingWater               fallingWater;
        public ConduitFlow                gasConduitFlow;
        public ConduitFlow                liquidConduitFlow;
        public SavedInfo                  savedInfo;
        public SpaceScannerNetworkManager spaceScannerNetworkManager;
        public StoryManager               storySetings;
        public UnstableGroundManager      unstableGround;
        public WorldDetailSave            worldDetail;
    }

    [Serializable]
    public struct LocationColours {
        public Color unreachable;
        public Color invalidLocation;
        public Color validLocation;
        public Color requiresRole;
        public Color unreachable_requiresRole;
    }

    [Serializable]
    public class UIColours {
        [SerializeField]
        private LocationColours buildColours;

        [SerializeField]
        private LocationColours digColours;

        public LocationColours Dig   => digColours;
        public LocationColours Build => buildColours;
    }
}