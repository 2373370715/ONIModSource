using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Threading;
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

// Token: 0x02001303 RID: 4867
[AddComponentMenu("KMonoBehaviour/scripts/Game")]
public class Game : KMonoBehaviour
{
	// Token: 0x060063E8 RID: 25576 RVA: 0x000E1343 File Offset: 0x000DF543
	public static bool IsOnMainThread()
	{
		return Game.MainThread == Thread.CurrentThread;
	}

	// Token: 0x060063E9 RID: 25577 RVA: 0x000E1351 File Offset: 0x000DF551
	public static bool IsQuitting()
	{
		return Game.quitting;
	}

	// Token: 0x17000642 RID: 1602
	// (get) Token: 0x060063EA RID: 25578 RVA: 0x000E1358 File Offset: 0x000DF558
	// (set) Token: 0x060063EB RID: 25579 RVA: 0x000E1360 File Offset: 0x000DF560
	public KInputHandler inputHandler { get; set; }

	// Token: 0x17000643 RID: 1603
	// (get) Token: 0x060063EC RID: 25580 RVA: 0x000E1369 File Offset: 0x000DF569
	// (set) Token: 0x060063ED RID: 25581 RVA: 0x000E1370 File Offset: 0x000DF570
	public static Game Instance { get; private set; }

	// Token: 0x17000644 RID: 1604
	// (get) Token: 0x060063EE RID: 25582 RVA: 0x000E1378 File Offset: 0x000DF578
	public static Camera MainCamera
	{
		get
		{
			if (Game.m_CachedCamera == null)
			{
				Game.m_CachedCamera = Camera.main;
			}
			return Game.m_CachedCamera;
		}
	}

	// Token: 0x17000645 RID: 1605
	// (get) Token: 0x060063EF RID: 25583 RVA: 0x000E1396 File Offset: 0x000DF596
	// (set) Token: 0x060063F0 RID: 25584 RVA: 0x002BD430 File Offset: 0x002BB630
	public bool SaveToCloudActive
	{
		get
		{
			return CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SaveToCloud).id == "Enabled";
		}
		set
		{
			string value2 = value ? "Enabled" : "Disabled";
			CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.SaveToCloud, value2);
		}
	}

	// Token: 0x17000646 RID: 1606
	// (get) Token: 0x060063F1 RID: 25585 RVA: 0x000E13B6 File Offset: 0x000DF5B6
	// (set) Token: 0x060063F2 RID: 25586 RVA: 0x002BD460 File Offset: 0x002BB660
	public bool FastWorkersModeActive
	{
		get
		{
			return CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.FastWorkersMode).id == "Enabled";
		}
		set
		{
			string value2 = value ? "Enabled" : "Disabled";
			CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.FastWorkersMode, value2);
		}
	}

	// Token: 0x17000647 RID: 1607
	// (get) Token: 0x060063F3 RID: 25587 RVA: 0x000E13D6 File Offset: 0x000DF5D6
	// (set) Token: 0x060063F4 RID: 25588 RVA: 0x002BD490 File Offset: 0x002BB690
	public bool SandboxModeActive
	{
		get
		{
			return this.sandboxModeActive;
		}
		set
		{
			this.sandboxModeActive = value;
			base.Trigger(-1948169901, null);
			if (PlanScreen.Instance != null)
			{
				PlanScreen.Instance.Refresh();
			}
			if (BuildMenu.Instance != null)
			{
				BuildMenu.Instance.Refresh();
			}
			if (OverlayMenu.Instance != null)
			{
				OverlayMenu.Instance.Refresh();
			}
			if (ManagementMenu.Instance != null)
			{
				ManagementMenu.Instance.Refresh();
			}
		}
	}

	// Token: 0x17000648 RID: 1608
	// (get) Token: 0x060063F5 RID: 25589 RVA: 0x000E13DE File Offset: 0x000DF5DE
	public bool DebugOnlyBuildingsAllowed
	{
		get
		{
			return DebugHandler.enabled && (this.SandboxModeActive || DebugHandler.InstantBuildMode);
		}
	}

	// Token: 0x17000649 RID: 1609
	// (get) Token: 0x060063F6 RID: 25590 RVA: 0x000E13F8 File Offset: 0x000DF5F8
	// (set) Token: 0x060063F7 RID: 25591 RVA: 0x000E1400 File Offset: 0x000DF600
	public StatusItemRenderer statusItemRenderer { get; private set; }

	// Token: 0x1700064A RID: 1610
	// (get) Token: 0x060063F8 RID: 25592 RVA: 0x000E1409 File Offset: 0x000DF609
	// (set) Token: 0x060063F9 RID: 25593 RVA: 0x000E1411 File Offset: 0x000DF611
	public PrioritizableRenderer prioritizableRenderer { get; private set; }

	// Token: 0x060063FA RID: 25594 RVA: 0x002BD50C File Offset: 0x002BB70C
	protected override void OnPrefabInit()
	{
		DebugUtil.LogArgs(new object[]
		{
			Time.realtimeSinceStartup,
			"Level Loaded....",
			SceneManager.GetActiveScene().name
		});
		Components.EntityCellVisualizers.OnAdd += this.OnAddBuildingCellVisualizer;
		Components.EntityCellVisualizers.OnRemove += this.OnRemoveBuildingCellVisualizer;
		Singleton<KBatchedAnimUpdater>.CreateInstance();
		Singleton<CellChangeMonitor>.CreateInstance();
		this.userMenu = new UserMenu();
		SimTemperatureTransfer.ClearInstanceMap();
		StructureTemperatureComponents.ClearInstanceMap();
		ElementConsumer.ClearInstanceMap();
		App.OnPreLoadScene = (System.Action)Delegate.Combine(App.OnPreLoadScene, new System.Action(this.StopBE));
		Game.Instance = this;
		this.statusItemRenderer = new StatusItemRenderer();
		this.prioritizableRenderer = new PrioritizableRenderer();
		this.LoadEventHashes();
		this.savedInfo.InitializeEmptyVariables();
		this.gasFlowPos = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.GasConduits) - 0.4f);
		this.liquidFlowPos = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.LiquidConduits) - 0.4f);
		this.solidFlowPos = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.SolidConduitContents) - 0.4f);
		Shader.WarmupAllShaders();
		Db.Get();
		Game.quitting = false;
		Game.PickupableLayer = LayerMask.NameToLayer("Pickupable");
		Game.BlockSelectionLayerMask = LayerMask.GetMask(new string[]
		{
			"BlockSelection"
		});
		this.world = World.Instance;
		KPrefabID.NextUniqueID = KPlayerPrefs.GetInt(Game.NextUniqueIDKey, 0);
		this.circuitManager = new CircuitManager();
		this.energySim = new EnergySim();
		this.gasConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, Vent>(Grid.WidthInCells, Grid.HeightInCells, 13);
		this.liquidConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, Vent>(Grid.WidthInCells, Grid.HeightInCells, 17);
		this.electricalConduitSystem = new UtilityNetworkManager<ElectricalUtilityNetwork, Wire>(Grid.WidthInCells, Grid.HeightInCells, 27);
		this.logicCircuitSystem = new UtilityNetworkManager<LogicCircuitNetwork, LogicWire>(Grid.WidthInCells, Grid.HeightInCells, 32);
		this.logicCircuitManager = new LogicCircuitManager(this.logicCircuitSystem);
		this.travelTubeSystem = new UtilityNetworkTubesManager(Grid.WidthInCells, Grid.HeightInCells, 35);
		this.solidConduitSystem = new UtilityNetworkManager<FlowUtilityNetwork, SolidConduit>(Grid.WidthInCells, Grid.HeightInCells, 21);
		this.conduitTemperatureManager = new ConduitTemperatureManager();
		this.conduitDiseaseManager = new ConduitDiseaseManager(this.conduitTemperatureManager);
		this.gasConduitFlow = new ConduitFlow(ConduitType.Gas, Grid.CellCount, this.gasConduitSystem, 1f, 0.25f);
		this.liquidConduitFlow = new ConduitFlow(ConduitType.Liquid, Grid.CellCount, this.liquidConduitSystem, 10f, 0.75f);
		this.solidConduitFlow = new SolidConduitFlow(Grid.CellCount, this.solidConduitSystem, 0.75f);
		this.gasFlowVisualizer = new ConduitFlowVisualizer(this.gasConduitFlow, this.gasConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundGas, Lighting.Instance.Settings.GasConduit);
		this.liquidFlowVisualizer = new ConduitFlowVisualizer(this.liquidConduitFlow, this.liquidConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundLiquid, Lighting.Instance.Settings.LiquidConduit);
		this.solidFlowVisualizer = new SolidConduitFlowVisualizer(this.solidConduitFlow, this.solidConduitVisInfo, GlobalResources.Instance().ConduitOverlaySoundSolid, Lighting.Instance.Settings.SolidConduit);
		this.accumulators = new Accumulators();
		this.plantElementAbsorbers = new PlantElementAbsorbers();
		this.activeFX = new ushort[Grid.CellCount];
		this.UnsafePrefabInit();
		Shader.SetGlobalVector("_MetalParameters", new Vector4(0f, 0f, 0f, 0f));
		Shader.SetGlobalVector("_WaterParameters", new Vector4(0f, 0f, 0f, 0f));
		this.InitializeFXSpawners();
		PathFinder.Initialize();
		new GameNavGrids(Pathfinding.Instance);
		this.screenMgr = global::Util.KInstantiate(this.screenManagerPrefab, null, null).GetComponent<GameScreenManager>();
		this.roomProber = new RoomProber();
		this.spaceScannerNetworkManager = new SpaceScannerNetworkManager();
		this.fetchManager = base.gameObject.AddComponent<FetchManager>();
		this.ediblesManager = base.gameObject.AddComponent<EdiblesManager>();
		Singleton<CellChangeMonitor>.Instance.SetGridSize(Grid.WidthInCells, Grid.HeightInCells);
		this.unlocks = base.GetComponent<Unlocks>();
		this.changelistsPlayedOn = new List<uint>();
		this.changelistsPlayedOn.Add(642695U);
		this.dateGenerated = System.DateTime.UtcNow.ToString("U", CultureInfo.InvariantCulture);
	}

	// Token: 0x060063FB RID: 25595 RVA: 0x000E141A File Offset: 0x000DF61A
	public void SetGameStarted()
	{
		this.gameStarted = true;
	}

	// Token: 0x060063FC RID: 25596 RVA: 0x000E1423 File Offset: 0x000DF623
	public bool GameStarted()
	{
		return this.gameStarted;
	}

	// Token: 0x060063FD RID: 25597 RVA: 0x000E142B File Offset: 0x000DF62B
	private void UnsafePrefabInit()
	{
		this.StepTheSim(0f);
	}

	// Token: 0x060063FE RID: 25598 RVA: 0x000E1439 File Offset: 0x000DF639
	protected override void OnLoadLevel()
	{
		base.Unsubscribe<Game>(1798162660, Game.MarkStatusItemRendererDirtyDelegate, false);
		base.Unsubscribe<Game>(1983128072, Game.ActiveWorldChangedDelegate, false);
		base.OnLoadLevel();
	}

	// Token: 0x060063FF RID: 25599 RVA: 0x000E1463 File Offset: 0x000DF663
	private void MarkStatusItemRendererDirty(object data)
	{
		this.statusItemRenderer.MarkAllDirty();
	}

	// Token: 0x06006400 RID: 25600 RVA: 0x002BD988 File Offset: 0x002BBB88
	protected override void OnForcedCleanUp()
	{
		if (this.prioritizableRenderer != null)
		{
			this.prioritizableRenderer.Cleanup();
			this.prioritizableRenderer = null;
		}
		if (this.statusItemRenderer != null)
		{
			this.statusItemRenderer.Destroy();
			this.statusItemRenderer = null;
		}
		if (this.conduitTemperatureManager != null)
		{
			this.conduitTemperatureManager.Shutdown();
		}
		this.gasFlowVisualizer.FreeResources();
		this.liquidFlowVisualizer.FreeResources();
		this.solidFlowVisualizer.FreeResources();
		LightGridManager.Shutdown();
		RadiationGridManager.Shutdown();
		App.OnPreLoadScene = (System.Action)Delegate.Remove(App.OnPreLoadScene, new System.Action(this.StopBE));
		base.OnForcedCleanUp();
	}

	// Token: 0x06006401 RID: 25601 RVA: 0x002BDA30 File Offset: 0x002BBC30
	protected override void OnSpawn()
	{
		global::Debug.Log("-- GAME --");
		Game.BrainScheduler = base.GetComponent<BrainScheduler>();
		PropertyTextures.FogOfWarScale = 0f;
		if (CameraController.Instance != null)
		{
			CameraController.Instance.EnableFreeCamera(false);
		}
		this.LocalPlayer = this.SpawnPlayer();
		WaterCubes.Instance.Init();
		SpeedControlScreen.Instance.Pause(false, false);
		LightGridManager.Initialise();
		RadiationGridManager.Initialise();
		this.RefreshRadiationLoop();
		this.UnsafeOnSpawn();
		Time.timeScale = 0f;
		if (this.tempIntroScreenPrefab != null)
		{
			global::Util.KInstantiate(this.tempIntroScreenPrefab, null, null);
		}
		if (SaveLoader.Instance.Cluster != null)
		{
			foreach (WorldGen worldGen in SaveLoader.Instance.Cluster.worlds)
			{
				this.Reset(worldGen.data.gameSpawnData, worldGen.WorldOffset);
			}
			NewBaseScreen.SetInitialCamera();
		}
		TagManager.FillMissingProperNames();
		CameraController.Instance.OrthographicSize = 20f;
		if (SaveLoader.Instance.loadedFromSave)
		{
			this.baseAlreadyCreated = true;
			base.Trigger(-1992507039, null);
			base.Trigger(-838649377, null);
		}
		UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(MeshRenderer));
		for (int i = 0; i < array.Length; i++)
		{
			((MeshRenderer)array[i]).reflectionProbeUsage = ReflectionProbeUsage.Off;
		}
		base.Subscribe<Game>(1798162660, Game.MarkStatusItemRendererDirtyDelegate);
		base.Subscribe<Game>(1983128072, Game.ActiveWorldChangedDelegate);
		this.solidConduitFlow.Initialize();
		SimAndRenderScheduler.instance.Add(this.roomProber, false);
		SimAndRenderScheduler.instance.Add(this.spaceScannerNetworkManager, false);
		SimAndRenderScheduler.instance.Add(KComponentSpawn.instance, false);
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim200ms, AmountInstance>(new UpdateBucketWithUpdater<ISim200ms>.BatchUpdateDelegate(AmountInstance.BatchUpdate));
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim1000ms, SolidTransferArm>(new UpdateBucketWithUpdater<ISim1000ms>.BatchUpdateDelegate(SolidTransferArm.BatchUpdate));
		if (!SaveLoader.Instance.loadedFromSave)
		{
			SettingConfig settingConfig = CustomGameSettings.Instance.QualitySettings[CustomGameSettingConfigs.SandboxMode.id];
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.SandboxMode);
			SaveGame.Instance.sandboxEnabled = !settingConfig.IsDefaultLevel(currentQualitySetting.id);
		}
		this.mingleCellTracker = base.gameObject.AddComponent<MingleCellTracker>();
		if (Global.Instance != null)
		{
			Global.Instance.GetComponent<PerformanceMonitor>().Reset();
			Global.Instance.modManager.NotifyDialog(UI.FRONTEND.MOD_DIALOGS.SAVE_GAME_MODS_DIFFER.TITLE, UI.FRONTEND.MOD_DIALOGS.SAVE_GAME_MODS_DIFFER.MESSAGE, Global.Instance.globalCanvas);
		}
	}

	// Token: 0x06006402 RID: 25602 RVA: 0x000E1470 File Offset: 0x000DF670
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		SimAndRenderScheduler.instance.Remove(KComponentSpawn.instance);
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim200ms, AmountInstance>(null);
		SimAndRenderScheduler.instance.RegisterBatchUpdate<ISim1000ms, SolidTransferArm>(null);
		this.DestroyInstances();
	}

	// Token: 0x06006403 RID: 25603 RVA: 0x000E14A3 File Offset: 0x000DF6A3
	private new void OnDestroy()
	{
		base.OnDestroy();
		this.DestroyInstances();
	}

	// Token: 0x06006404 RID: 25604 RVA: 0x000E14B1 File Offset: 0x000DF6B1
	private void UnsafeOnSpawn()
	{
		this.world.UpdateCellInfo(this.gameSolidInfo, this.callbackInfo, 0, null, 0, null);
	}

	// Token: 0x06006405 RID: 25605 RVA: 0x000E14D0 File Offset: 0x000DF6D0
	private void RefreshRadiationLoop()
	{
		GameScheduler.Instance.Schedule("UpdateRadiation", 1f, delegate(object obj)
		{
			RadiationGridManager.Refresh();
			this.RefreshRadiationLoop();
		}, null, null);
	}

	// Token: 0x06006406 RID: 25606 RVA: 0x000E14F5 File Offset: 0x000DF6F5
	public void SetMusicEnabled(bool enabled)
	{
		if (enabled)
		{
			MusicManager.instance.PlaySong("Music_FrontEnd", false);
			return;
		}
		MusicManager.instance.StopSong("Music_FrontEnd", true, STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x06006407 RID: 25607 RVA: 0x002BDCE8 File Offset: 0x002BBEE8
	private Player SpawnPlayer()
	{
		Player component = global::Util.KInstantiate(this.playerPrefab, base.gameObject, null).GetComponent<Player>();
		component.ScreenManager = this.screenMgr;
		component.ScreenManager.StartScreen(ScreenPrefabs.Instance.HudScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
		component.ScreenManager.StartScreen(ScreenPrefabs.Instance.HoverTextScreen.gameObject, null, GameScreenManager.UIRenderTarget.HoverTextScreen);
		component.ScreenManager.StartScreen(ScreenPrefabs.Instance.ToolTipScreen.gameObject, null, GameScreenManager.UIRenderTarget.HoverTextScreen);
		this.cameraController = global::Util.KInstantiate(this.cameraControllerPrefab, null, null).GetComponent<CameraController>();
		component.CameraController = this.cameraController;
		if (KInputManager.currentController != null)
		{
			KInputHandler.Add(KInputManager.currentController, this.cameraController, 1);
		}
		else
		{
			KInputHandler.Add(Global.GetInputManager().GetDefaultController(), this.cameraController, 1);
		}
		Global.GetInputManager().usedMenus.Add(this.cameraController);
		this.playerController = component.GetComponent<PlayerController>();
		if (KInputManager.currentController != null)
		{
			KInputHandler.Add(KInputManager.currentController, this.playerController, 20);
		}
		else
		{
			KInputHandler.Add(Global.GetInputManager().GetDefaultController(), this.playerController, 20);
		}
		Global.GetInputManager().usedMenus.Add(this.playerController);
		return component;
	}

	// Token: 0x06006408 RID: 25608 RVA: 0x000E151C File Offset: 0x000DF71C
	public void SetDupePassableSolid(int cell, bool passable, bool solid)
	{
		Grid.DupePassable[cell] = passable;
		this.gameSolidInfo.Add(new SolidInfo(cell, solid));
	}

	// Token: 0x06006409 RID: 25609 RVA: 0x002BDE30 File Offset: 0x002BC030
	private unsafe Sim.GameDataUpdate* StepTheSim(float dt)
	{
		Sim.GameDataUpdate* result;
		using (new KProfiler.Region("StepTheSim", null))
		{
			IntPtr intPtr = IntPtr.Zero;
			using (new KProfiler.Region("WaitingForSim", null))
			{
				if (Grid.Visible == null || Grid.Visible.Length == 0)
				{
					global::Debug.LogError("Invalid Grid.Visible, what have you done?!");
					return null;
				}
				intPtr = Sim.HandleMessage(SimMessageHashes.PrepareGameData, Grid.Visible.Length, Grid.Visible);
			}
			if (intPtr == IntPtr.Zero)
			{
				result = null;
			}
			else
			{
				Sim.GameDataUpdate* ptr = (Sim.GameDataUpdate*)((void*)intPtr);
				Grid.elementIdx = ptr->elementIdx;
				Grid.temperature = ptr->temperature;
				Grid.mass = ptr->mass;
				Grid.radiation = ptr->radiation;
				Grid.properties = ptr->properties;
				Grid.strengthInfo = ptr->strengthInfo;
				Grid.insulation = ptr->insulation;
				Grid.diseaseIdx = ptr->diseaseIdx;
				Grid.diseaseCount = ptr->diseaseCount;
				Grid.AccumulatedFlowValues = ptr->accumulatedFlow;
				Grid.exposedToSunlight = (byte*)((void*)ptr->propertyTextureExposedToSunlight);
				PropertyTextures.externalFlowTex = ptr->propertyTextureFlow;
				PropertyTextures.externalLiquidTex = ptr->propertyTextureLiquid;
				PropertyTextures.externalExposedToSunlight = ptr->propertyTextureExposedToSunlight;
				List<Element> elements = ElementLoader.elements;
				this.simData.emittedMassEntries = ptr->emittedMassEntries;
				this.simData.elementChunks = ptr->elementChunkInfos;
				this.simData.buildingTemperatures = ptr->buildingTemperatures;
				this.simData.diseaseEmittedInfos = ptr->diseaseEmittedInfos;
				this.simData.diseaseConsumedInfos = ptr->diseaseConsumedInfos;
				for (int i = 0; i < ptr->numSubstanceChangeInfo; i++)
				{
					Sim.SubstanceChangeInfo substanceChangeInfo = ptr->substanceChangeInfo[i];
					Element element = elements[(int)substanceChangeInfo.newElemIdx];
					Grid.Element[substanceChangeInfo.cellIdx] = element;
				}
				for (int j = 0; j < ptr->numSolidInfo; j++)
				{
					Sim.SolidInfo solidInfo = ptr->solidInfo[j];
					if (!this.solidChangedFilter.Contains(solidInfo.cellIdx))
					{
						this.solidInfo.Add(new SolidInfo(solidInfo.cellIdx, solidInfo.isSolid != 0));
						bool solid = solidInfo.isSolid != 0;
						Grid.SetSolid(solidInfo.cellIdx, solid, CellEventLogger.Instance.SimMessagesSolid);
					}
				}
				for (int k = 0; k < ptr->numCallbackInfo; k++)
				{
					Sim.CallbackInfo callbackInfo = ptr->callbackInfo[k];
					HandleVector<Game.CallbackInfo>.Handle handle = new HandleVector<Game.CallbackInfo>.Handle
					{
						index = callbackInfo.callbackIdx
					};
					if (!this.IsManuallyReleasedHandle(handle))
					{
						this.callbackInfo.Add(new Klei.CallbackInfo(handle));
					}
				}
				int numSpawnFallingLiquidInfo = ptr->numSpawnFallingLiquidInfo;
				for (int l = 0; l < numSpawnFallingLiquidInfo; l++)
				{
					Sim.SpawnFallingLiquidInfo spawnFallingLiquidInfo = ptr->spawnFallingLiquidInfo[l];
					FallingWater.instance.AddParticle(spawnFallingLiquidInfo.cellIdx, spawnFallingLiquidInfo.elemIdx, spawnFallingLiquidInfo.mass, spawnFallingLiquidInfo.temperature, spawnFallingLiquidInfo.diseaseIdx, spawnFallingLiquidInfo.diseaseCount, false, false, false, false);
				}
				int numDigInfo = ptr->numDigInfo;
				WorldDamage component = this.world.GetComponent<WorldDamage>();
				for (int m = 0; m < numDigInfo; m++)
				{
					Sim.SpawnOreInfo spawnOreInfo = ptr->digInfo[m];
					if (spawnOreInfo.temperature <= 0f && spawnOreInfo.mass > 0f)
					{
						global::Debug.LogError("Sim is telling us to spawn a zero temperature object. This shouldn't be possible because I have asserts in the dll about this....");
					}
					component.OnDigComplete(spawnOreInfo.cellIdx, spawnOreInfo.mass, spawnOreInfo.temperature, spawnOreInfo.elemIdx, spawnOreInfo.diseaseIdx, spawnOreInfo.diseaseCount);
				}
				int numSpawnOreInfo = ptr->numSpawnOreInfo;
				for (int n = 0; n < numSpawnOreInfo; n++)
				{
					Sim.SpawnOreInfo spawnOreInfo2 = ptr->spawnOreInfo[n];
					Vector3 position = Grid.CellToPosCCC(spawnOreInfo2.cellIdx, Grid.SceneLayer.Ore);
					Element element2 = ElementLoader.elements[(int)spawnOreInfo2.elemIdx];
					if (spawnOreInfo2.temperature <= 0f && spawnOreInfo2.mass > 0f)
					{
						global::Debug.LogError("Sim is telling us to spawn a zero temperature object. This shouldn't be possible because I have asserts in the dll about this....");
					}
					element2.substance.SpawnResource(position, spawnOreInfo2.mass, spawnOreInfo2.temperature, spawnOreInfo2.diseaseIdx, spawnOreInfo2.diseaseCount, false, false, false);
				}
				int numSpawnFXInfo = ptr->numSpawnFXInfo;
				for (int num = 0; num < numSpawnFXInfo; num++)
				{
					Sim.SpawnFXInfo spawnFXInfo = ptr->spawnFXInfo[num];
					this.SpawnFX((SpawnFXHashes)spawnFXInfo.fxHash, spawnFXInfo.cellIdx, spawnFXInfo.rotation);
				}
				UnstableGroundManager component2 = this.world.GetComponent<UnstableGroundManager>();
				int numUnstableCellInfo = ptr->numUnstableCellInfo;
				for (int num2 = 0; num2 < numUnstableCellInfo; num2++)
				{
					Sim.UnstableCellInfo unstableCellInfo = ptr->unstableCellInfo[num2];
					if (unstableCellInfo.fallingInfo == 0)
					{
						component2.Spawn(unstableCellInfo.cellIdx, ElementLoader.elements[(int)unstableCellInfo.elemIdx], unstableCellInfo.mass, unstableCellInfo.temperature, unstableCellInfo.diseaseIdx, unstableCellInfo.diseaseCount);
					}
				}
				int numWorldDamageInfo = ptr->numWorldDamageInfo;
				for (int num3 = 0; num3 < numWorldDamageInfo; num3++)
				{
					Sim.WorldDamageInfo damage_info = ptr->worldDamageInfo[num3];
					WorldDamage.Instance.ApplyDamage(damage_info);
				}
				for (int num4 = 0; num4 < ptr->numRemovedMassEntries; num4++)
				{
					ElementConsumer.AddMass(ptr->removedMassEntries[num4]);
				}
				int numMassConsumedCallbacks = ptr->numMassConsumedCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle2 = default(HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle);
				for (int num5 = 0; num5 < numMassConsumedCallbacks; num5++)
				{
					Sim.MassConsumedCallback massConsumedCallback = ptr->massConsumedCallbacks[num5];
					handle2.index = massConsumedCallback.callbackIdx;
					Game.ComplexCallbackInfo<Sim.MassConsumedCallback> complexCallbackInfo = this.massConsumedCallbackManager.Release(handle2, "massConsumedCB");
					if (complexCallbackInfo.cb != null)
					{
						complexCallbackInfo.cb(massConsumedCallback, complexCallbackInfo.callbackData);
					}
				}
				int numMassEmittedCallbacks = ptr->numMassEmittedCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle handle3 = default(HandleVector<Game.ComplexCallbackInfo<Sim.MassEmittedCallback>>.Handle);
				for (int num6 = 0; num6 < numMassEmittedCallbacks; num6++)
				{
					Sim.MassEmittedCallback massEmittedCallback = ptr->massEmittedCallbacks[num6];
					handle3.index = massEmittedCallback.callbackIdx;
					if (this.massEmitCallbackManager.IsVersionValid(handle3))
					{
						Game.ComplexCallbackInfo<Sim.MassEmittedCallback> item = this.massEmitCallbackManager.GetItem(handle3);
						if (item.cb != null)
						{
							item.cb(massEmittedCallback, item.callbackData);
						}
					}
				}
				int numDiseaseConsumptionCallbacks = ptr->numDiseaseConsumptionCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback>>.Handle handle4 = default(HandleVector<Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback>>.Handle);
				for (int num7 = 0; num7 < numDiseaseConsumptionCallbacks; num7++)
				{
					Sim.DiseaseConsumptionCallback diseaseConsumptionCallback = ptr->diseaseConsumptionCallbacks[num7];
					handle4.index = diseaseConsumptionCallback.callbackIdx;
					if (this.diseaseConsumptionCallbackManager.IsVersionValid(handle4))
					{
						Game.ComplexCallbackInfo<Sim.DiseaseConsumptionCallback> item2 = this.diseaseConsumptionCallbackManager.GetItem(handle4);
						if (item2.cb != null)
						{
							item2.cb(diseaseConsumptionCallback, item2.callbackData);
						}
					}
				}
				int numComponentStateChangedMessages = ptr->numComponentStateChangedMessages;
				HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle5 = default(HandleVector<Game.ComplexCallbackInfo<int>>.Handle);
				for (int num8 = 0; num8 < numComponentStateChangedMessages; num8++)
				{
					Sim.ComponentStateChangedMessage componentStateChangedMessage = ptr->componentStateChangedMessages[num8];
					handle5.index = componentStateChangedMessage.callbackIdx;
					if (this.simComponentCallbackManager.IsVersionValid(handle5))
					{
						Game.ComplexCallbackInfo<int> complexCallbackInfo2 = this.simComponentCallbackManager.Release(handle5, "component state changed cb");
						if (complexCallbackInfo2.cb != null)
						{
							complexCallbackInfo2.cb(componentStateChangedMessage.simHandle, complexCallbackInfo2.callbackData);
						}
					}
				}
				int numRadiationConsumedCallbacks = ptr->numRadiationConsumedCallbacks;
				HandleVector<Game.ComplexCallbackInfo<Sim.ConsumedRadiationCallback>>.Handle handle6 = default(HandleVector<Game.ComplexCallbackInfo<Sim.ConsumedRadiationCallback>>.Handle);
				for (int num9 = 0; num9 < numRadiationConsumedCallbacks; num9++)
				{
					Sim.ConsumedRadiationCallback consumedRadiationCallback = ptr->radiationConsumedCallbacks[num9];
					handle6.index = consumedRadiationCallback.callbackIdx;
					Game.ComplexCallbackInfo<Sim.ConsumedRadiationCallback> complexCallbackInfo3 = this.radiationConsumedCallbackManager.Release(handle6, "radiationConsumedCB");
					if (complexCallbackInfo3.cb != null)
					{
						complexCallbackInfo3.cb(consumedRadiationCallback, complexCallbackInfo3.callbackData);
					}
				}
				int numElementChunkMeltedInfos = ptr->numElementChunkMeltedInfos;
				for (int num10 = 0; num10 < numElementChunkMeltedInfos; num10++)
				{
					SimTemperatureTransfer.DoOreMeltTransition(ptr->elementChunkMeltedInfos[num10].handle);
				}
				int numBuildingOverheatInfos = ptr->numBuildingOverheatInfos;
				for (int num11 = 0; num11 < numBuildingOverheatInfos; num11++)
				{
					StructureTemperatureComponents.DoOverheat(ptr->buildingOverheatInfos[num11].handle);
				}
				int numBuildingNoLongerOverheatedInfos = ptr->numBuildingNoLongerOverheatedInfos;
				for (int num12 = 0; num12 < numBuildingNoLongerOverheatedInfos; num12++)
				{
					StructureTemperatureComponents.DoNoLongerOverheated(ptr->buildingNoLongerOverheatedInfos[num12].handle);
				}
				int numBuildingMeltedInfos = ptr->numBuildingMeltedInfos;
				for (int num13 = 0; num13 < numBuildingMeltedInfos; num13++)
				{
					StructureTemperatureComponents.DoStateTransition(ptr->buildingMeltedInfos[num13].handle);
				}
				int numCellMeltedInfos = ptr->numCellMeltedInfos;
				for (int num14 = 0; num14 < numCellMeltedInfos; num14++)
				{
					int gameCell = ptr->cellMeltedInfos[num14].gameCell;
					GameObject gameObject = Grid.Objects[gameCell, 9];
					if (gameObject != null)
					{
						gameObject.Trigger(675471409, null);
						global::Util.KDestroyGameObject(gameObject);
					}
				}
				if (dt > 0f)
				{
					this.conduitTemperatureManager.Sim200ms(0.2f);
					this.conduitDiseaseManager.Sim200ms(0.2f);
					this.gasConduitFlow.Sim200ms(0.2f);
					this.liquidConduitFlow.Sim200ms(0.2f);
					this.solidConduitFlow.Sim200ms(0.2f);
					this.accumulators.Sim200ms(0.2f);
					this.plantElementAbsorbers.Sim200ms(0.2f);
				}
				Sim.DebugProperties debugProperties;
				debugProperties.buildingTemperatureScale = 100f;
				debugProperties.buildingToBuildingTemperatureScale = 0.001f;
				debugProperties.biomeTemperatureLerpRate = 0.001f;
				debugProperties.isDebugEditing = ((DebugPaintElementScreen.Instance != null && DebugPaintElementScreen.Instance.gameObject.activeSelf) ? 1 : 0);
				debugProperties.pad0 = (debugProperties.pad1 = (debugProperties.pad2 = 0));
				SimMessages.SetDebugProperties(debugProperties);
				if (dt > 0f)
				{
					if (this.circuitManager != null)
					{
						this.circuitManager.Sim200msFirst(dt);
					}
					if (this.energySim != null)
					{
						this.energySim.EnergySim200ms(dt);
					}
					if (this.circuitManager != null)
					{
						this.circuitManager.Sim200msLast(dt);
					}
				}
				result = ptr;
			}
		}
		return result;
	}

	// Token: 0x0600640A RID: 25610 RVA: 0x000E153C File Offset: 0x000DF73C
	public void AddSolidChangedFilter(int cell)
	{
		this.solidChangedFilter.Add(cell);
	}

	// Token: 0x0600640B RID: 25611 RVA: 0x000E154B File Offset: 0x000DF74B
	public void RemoveSolidChangedFilter(int cell)
	{
		this.solidChangedFilter.Remove(cell);
	}

	// Token: 0x0600640C RID: 25612 RVA: 0x000E155A File Offset: 0x000DF75A
	public void SetIsLoading()
	{
		this.isLoading = true;
	}

	// Token: 0x0600640D RID: 25613 RVA: 0x000E1563 File Offset: 0x000DF763
	public bool IsLoading()
	{
		return this.isLoading;
	}

	// Token: 0x0600640E RID: 25614 RVA: 0x002BE920 File Offset: 0x002BCB20
	private void ShowDebugCellInfo()
	{
		int mouseCell = DebugHandler.GetMouseCell();
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(mouseCell, out num, out num2);
		string text = string.Concat(new string[]
		{
			mouseCell.ToString(),
			" (",
			num.ToString(),
			", ",
			num2.ToString(),
			")"
		});
		DebugText.Instance.Draw(text, Grid.CellToPosCCC(mouseCell, Grid.SceneLayer.Move), Color.white);
	}

	// Token: 0x0600640F RID: 25615 RVA: 0x000E156B File Offset: 0x000DF76B
	public void ForceSimStep()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Force-stepping the sim"
		});
		this.simDt = 0.2f;
	}

	// Token: 0x06006410 RID: 25616 RVA: 0x002BE99C File Offset: 0x002BCB9C
	private void Update()
	{
		if (this.isLoading)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		if (global::Debug.developerConsoleVisible)
		{
			global::Debug.developerConsoleVisible = false;
		}
		if (DebugHandler.DebugCellInfo)
		{
			this.ShowDebugCellInfo();
		}
		this.gasConduitSystem.Update();
		this.liquidConduitSystem.Update();
		this.solidConduitSystem.Update();
		this.circuitManager.RenderEveryTick(deltaTime);
		this.logicCircuitManager.RenderEveryTick(deltaTime);
		this.solidConduitFlow.RenderEveryTick(deltaTime);
		Pathfinding.Instance.RenderEveryTick();
		Singleton<CellChangeMonitor>.Instance.RenderEveryTick();
		this.SimEveryTick(deltaTime);
	}

	// Token: 0x06006411 RID: 25617 RVA: 0x002BEA34 File Offset: 0x002BCC34
	private void SimEveryTick(float dt)
	{
		dt = Mathf.Min(dt, 0.2f);
		this.simDt += dt;
		if (this.simDt >= 0.016666668f)
		{
			do
			{
				this.simSubTick++;
				this.simSubTick %= 12;
				if (this.simSubTick == 0)
				{
					this.hasFirstSimTickRun = true;
					this.UnsafeSim200ms(0.2f);
				}
				if (this.hasFirstSimTickRun)
				{
					Singleton<StateMachineUpdater>.Instance.AdvanceOneSimSubTick();
				}
				this.simDt -= 0.016666668f;
			}
			while (this.simDt >= 0.016666668f);
			return;
		}
		this.UnsafeSim200ms(0f);
	}

	// Token: 0x06006412 RID: 25618 RVA: 0x002BEAE0 File Offset: 0x002BCCE0
	private unsafe void UnsafeSim200ms(float dt)
	{
		this.simActiveRegions.Clear();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (worldContainer.IsDiscovered)
			{
				Game.SimActiveRegion simActiveRegion = new Game.SimActiveRegion();
				simActiveRegion.region = new Pair<Vector2I, Vector2I>(worldContainer.WorldOffset, worldContainer.WorldOffset + worldContainer.WorldSize);
				simActiveRegion.currentSunlightIntensity = worldContainer.currentSunlightIntensity;
				simActiveRegion.currentCosmicRadiationIntensity = worldContainer.currentCosmicIntensity;
				this.simActiveRegions.Add(simActiveRegion);
			}
		}
		global::Debug.Assert(this.simActiveRegions.Count > 0, "Cannot send a frame to the sim with zero active regions");
		SimMessages.NewGameFrame(dt, this.simActiveRegions);
		Sim.GameDataUpdate* ptr = this.StepTheSim(dt);
		if (ptr == null)
		{
			global::Debug.LogError("UNEXPECTED!");
			return;
		}
		if (ptr->numFramesProcessed <= 0)
		{
			return;
		}
		this.gameSolidInfo.AddRange(this.solidInfo);
		this.world.UpdateCellInfo(this.gameSolidInfo, this.callbackInfo, ptr->numSolidSubstanceChangeInfo, ptr->solidSubstanceChangeInfo, ptr->numLiquidChangeInfo, ptr->liquidChangeInfo);
		this.gameSolidInfo.Clear();
		this.solidInfo.Clear();
		this.callbackInfo.Clear();
		this.callbackManagerManuallyReleasedHandles.Clear();
		Pathfinding.Instance.UpdateNavGrids(false);
	}

	// Token: 0x06006413 RID: 25619 RVA: 0x000E158B File Offset: 0x000DF78B
	private void LateUpdateComponents()
	{
		this.UpdateOverlayScreen();
	}

	// Token: 0x06006414 RID: 25620 RVA: 0x002BEC48 File Offset: 0x002BCE48
	private void OnAddBuildingCellVisualizer(EntityCellVisualizer entity_cell_visualizer)
	{
		this.lastDrawnOverlayMode = default(HashedString);
		if (PlayerController.Instance != null)
		{
			BuildTool buildTool = PlayerController.Instance.ActiveTool as BuildTool;
			if (buildTool != null && buildTool.visualizer == entity_cell_visualizer.gameObject)
			{
				this.previewVisualizer = entity_cell_visualizer;
			}
		}
	}

	// Token: 0x06006415 RID: 25621 RVA: 0x000E1593 File Offset: 0x000DF793
	private void OnRemoveBuildingCellVisualizer(EntityCellVisualizer entity_cell_visualizer)
	{
		if (this.previewVisualizer == entity_cell_visualizer)
		{
			this.previewVisualizer = null;
		}
	}

	// Token: 0x06006416 RID: 25622 RVA: 0x002BECA4 File Offset: 0x002BCEA4
	private void UpdateOverlayScreen()
	{
		if (OverlayScreen.Instance == null)
		{
			return;
		}
		HashedString mode = OverlayScreen.Instance.GetMode();
		if (this.previewVisualizer != null)
		{
			this.previewVisualizer.DrawIcons(mode);
		}
		if (mode == this.lastDrawnOverlayMode)
		{
			return;
		}
		foreach (EntityCellVisualizer entityCellVisualizer in Components.EntityCellVisualizers.Items)
		{
			entityCellVisualizer.DrawIcons(mode);
		}
		this.lastDrawnOverlayMode = mode;
	}

	// Token: 0x06006417 RID: 25623 RVA: 0x000E15AA File Offset: 0x000DF7AA
	public void ForceOverlayUpdate(bool clearLastMode = false)
	{
		this.previousOverlayMode = OverlayModes.None.ID;
		if (clearLastMode)
		{
			this.lastDrawnOverlayMode = OverlayModes.None.ID;
		}
	}

	// Token: 0x06006418 RID: 25624 RVA: 0x002BED44 File Offset: 0x002BCF44
	private void LateUpdate()
	{
		if (this.OnSpawnComplete != null)
		{
			this.OnSpawnComplete();
			this.OnSpawnComplete = null;
		}
		if (Time.timeScale == 0f && !this.IsPaused)
		{
			this.IsPaused = true;
			base.Trigger(-1788536802, this.IsPaused);
		}
		else if (Time.timeScale != 0f && this.IsPaused)
		{
			this.IsPaused = false;
			base.Trigger(-1788536802, this.IsPaused);
		}
		if (Input.GetMouseButton(0))
		{
			this.VisualTunerElement = null;
			int mouseCell = DebugHandler.GetMouseCell();
			if (Grid.IsValidCell(mouseCell))
			{
				Element visualTunerElement = Grid.Element[mouseCell];
				this.VisualTunerElement = visualTunerElement;
			}
		}
		this.gasConduitSystem.Update();
		this.liquidConduitSystem.Update();
		this.solidConduitSystem.Update();
		HashedString mode = SimDebugView.Instance.GetMode();
		if (mode != this.previousOverlayMode)
		{
			this.previousOverlayMode = mode;
			if (mode == OverlayModes.LiquidConduits.ID)
			{
				this.liquidFlowVisualizer.ColourizePipeContents(true, true);
				this.gasFlowVisualizer.ColourizePipeContents(false, true);
				this.solidFlowVisualizer.ColourizePipeContents(false, true);
			}
			else if (mode == OverlayModes.GasConduits.ID)
			{
				this.liquidFlowVisualizer.ColourizePipeContents(false, true);
				this.gasFlowVisualizer.ColourizePipeContents(true, true);
				this.solidFlowVisualizer.ColourizePipeContents(false, true);
			}
			else if (mode == OverlayModes.SolidConveyor.ID)
			{
				this.liquidFlowVisualizer.ColourizePipeContents(false, true);
				this.gasFlowVisualizer.ColourizePipeContents(false, true);
				this.solidFlowVisualizer.ColourizePipeContents(true, true);
			}
			else
			{
				this.liquidFlowVisualizer.ColourizePipeContents(false, false);
				this.gasFlowVisualizer.ColourizePipeContents(false, false);
				this.solidFlowVisualizer.ColourizePipeContents(false, false);
			}
		}
		this.gasFlowVisualizer.Render(this.gasFlowPos.z, 0, this.gasConduitFlow.ContinuousLerpPercent, mode == OverlayModes.GasConduits.ID && this.gasConduitFlow.DiscreteLerpPercent != this.previousGasConduitFlowDiscreteLerpPercent);
		this.liquidFlowVisualizer.Render(this.liquidFlowPos.z, 0, this.liquidConduitFlow.ContinuousLerpPercent, mode == OverlayModes.LiquidConduits.ID && this.liquidConduitFlow.DiscreteLerpPercent != this.previousLiquidConduitFlowDiscreteLerpPercent);
		this.solidFlowVisualizer.Render(this.solidFlowPos.z, 0, this.solidConduitFlow.ContinuousLerpPercent, mode == OverlayModes.SolidConveyor.ID && this.solidConduitFlow.DiscreteLerpPercent != this.previousSolidConduitFlowDiscreteLerpPercent);
		this.previousGasConduitFlowDiscreteLerpPercent = ((mode == OverlayModes.GasConduits.ID) ? this.gasConduitFlow.DiscreteLerpPercent : -1f);
		this.previousLiquidConduitFlowDiscreteLerpPercent = ((mode == OverlayModes.LiquidConduits.ID) ? this.liquidConduitFlow.DiscreteLerpPercent : -1f);
		this.previousSolidConduitFlowDiscreteLerpPercent = ((mode == OverlayModes.SolidConveyor.ID) ? this.solidConduitFlow.DiscreteLerpPercent : -1f);
		Vector3 vector = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		Shader.SetGlobalVector("_WsToCs", new Vector4(vector.x / (float)Grid.WidthInCells, vector.y / (float)Grid.HeightInCells, (vector2.x - vector.x) / (float)Grid.WidthInCells, (vector2.y - vector.y) / (float)Grid.HeightInCells));
		WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
		Vector2I worldOffset = activeWorld.WorldOffset;
		Vector2I worldSize = activeWorld.WorldSize;
		Vector4 value = new Vector4((vector.x - (float)worldOffset.x) / (float)worldSize.x, (vector.y - (float)worldOffset.y) / (float)worldSize.y, (vector2.x - vector.x) / (float)worldSize.x, (vector2.y - vector.y) / (float)worldSize.y);
		Shader.SetGlobalVector("_WsToCcs", value);
		if (this.drawStatusItems)
		{
			this.statusItemRenderer.RenderEveryTick();
			this.prioritizableRenderer.RenderEveryTick();
		}
		this.LateUpdateComponents();
		Singleton<StateMachineUpdater>.Instance.Render(Time.unscaledDeltaTime);
		Singleton<StateMachineUpdater>.Instance.RenderEveryTick(Time.unscaledDeltaTime);
		if (SelectTool.Instance != null && SelectTool.Instance.selected != null)
		{
			Navigator component = SelectTool.Instance.selected.GetComponent<Navigator>();
			if (component != null)
			{
				component.DrawPath();
			}
		}
		KFMOD.RenderEveryTick(Time.deltaTime);
		if (GenericGameSettings.instance.performanceCapture.waitTime != 0f)
		{
			this.UpdatePerformanceCapture();
		}
	}

	// Token: 0x06006419 RID: 25625 RVA: 0x002BF234 File Offset: 0x002BD434
	private void UpdatePerformanceCapture()
	{
		if (this.IsPaused && SpeedControlScreen.Instance != null)
		{
			SpeedControlScreen.Instance.Unpause(true);
		}
		if (Time.timeSinceLevelLoad < GenericGameSettings.instance.performanceCapture.waitTime)
		{
			return;
		}
		uint num = 642695U;
		string text = System.DateTime.Now.ToShortDateString();
		string text2 = System.DateTime.Now.ToShortTimeString();
		string fileName = Path.GetFileName(GenericGameSettings.instance.performanceCapture.saveGame);
		string text3 = "Version,Date,Time,SaveGame";
		string text4 = string.Format("{0},{1},{2},{3}", new object[]
		{
			num,
			text,
			text2,
			fileName
		});
		float num2 = 0.1f;
		if (GenericGameSettings.instance.performanceCapture.gcStats)
		{
			global::Debug.Log("Begin GC profiling...");
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			GC.Collect();
			num2 = Time.realtimeSinceStartup - realtimeSinceStartup;
			global::Debug.Log("\tGC.Collect() took " + num2.ToString() + " seconds");
			MemorySnapshot memorySnapshot = new MemorySnapshot();
			string format = "{0},{1},{2},{3}";
			string path = "./memory/GCTypeMetrics.csv";
			if (!File.Exists(path))
			{
				using (StreamWriter streamWriter = new StreamWriter(path))
				{
					streamWriter.WriteLine(string.Format(format, new object[]
					{
						text3,
						"Type",
						"Instances",
						"References"
					}));
				}
			}
			using (StreamWriter streamWriter2 = new StreamWriter(path, true))
			{
				foreach (MemorySnapshot.TypeData typeData in memorySnapshot.types.Values)
				{
					streamWriter2.WriteLine(string.Format(format, new object[]
					{
						text4,
						"\"" + typeData.type.ToString() + "\"",
						typeData.instanceCount,
						typeData.refCount
					}));
				}
			}
			global::Debug.Log("...end GC profiling");
		}
		float fps = Global.Instance.GetComponent<PerformanceMonitor>().FPS;
		Directory.CreateDirectory("./memory");
		string format2 = "{0},{1},{2}";
		string path2 = "./memory/GeneralMetrics.csv";
		if (!File.Exists(path2))
		{
			using (StreamWriter streamWriter3 = new StreamWriter(path2))
			{
				streamWriter3.WriteLine(string.Format(format2, text3, "GCDuration", "FPS"));
			}
		}
		using (StreamWriter streamWriter4 = new StreamWriter(path2, true))
		{
			streamWriter4.WriteLine(string.Format(format2, text4, num2, fps));
		}
		GenericGameSettings.instance.performanceCapture.waitTime = 0f;
		App.Quit();
	}

	// Token: 0x0600641A RID: 25626 RVA: 0x002BF53C File Offset: 0x002BD73C
	public void Reset(GameSpawnData gsd, Vector2I world_offset)
	{
		using (new KProfiler.Region("World.Reset", null))
		{
			if (gsd != null)
			{
				foreach (KeyValuePair<Vector2I, bool> keyValuePair in gsd.preventFoWReveal)
				{
					if (keyValuePair.Value)
					{
						Vector2I v = new Vector2I(keyValuePair.Key.X + world_offset.X, keyValuePair.Key.Y + world_offset.Y);
						Grid.PreventFogOfWarReveal[Grid.PosToCell(v)] = keyValuePair.Value;
					}
				}
			}
		}
	}

	// Token: 0x0600641B RID: 25627 RVA: 0x002BF614 File Offset: 0x002BD814
	private void OnApplicationQuit()
	{
		Game.quitting = true;
		Sim.Shutdown();
		AudioMixer.Destroy();
		if (this.screenMgr != null && this.screenMgr.gameObject != null)
		{
			UnityEngine.Object.Destroy(this.screenMgr.gameObject);
		}
		Console.WriteLine("Game.OnApplicationQuit()");
	}

	// Token: 0x0600641C RID: 25628 RVA: 0x002BF66C File Offset: 0x002BD86C
	private void InitializeFXSpawners()
	{
		for (int i = 0; i < this.fxSpawnData.Length; i++)
		{
			int fx_idx = i;
			this.fxSpawnData[fx_idx].fxPrefab.SetActive(false);
			ushort fx_mask = (ushort)(1 << fx_idx);
			Action<SpawnFXHashes, GameObject> destroyer = delegate(SpawnFXHashes fxid, GameObject go)
			{
				if (!Game.IsQuitting())
				{
					int num = Grid.PosToCell(go);
					ushort[] array = this.activeFX;
					int num2 = num;
					array[num2] &= ~fx_mask;
					go.GetComponent<KAnimControllerBase>().enabled = false;
					this.fxPools[(int)fxid].ReleaseInstance(go);
				}
			};
			Func<GameObject> instantiator = delegate()
			{
				GameObject gameObject = GameUtil.KInstantiate(this.fxSpawnData[fx_idx].fxPrefab, Grid.SceneLayer.Front, null, 0);
				KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
				component.enabled = false;
				gameObject.SetActive(true);
				component.onDestroySelf = delegate(GameObject go)
				{
					destroyer(this.fxSpawnData[fx_idx].id, go);
				};
				return gameObject;
			};
			GameObjectPool pool = new GameObjectPool(instantiator, this.fxSpawnData[fx_idx].initialCount);
			this.fxPools[(int)this.fxSpawnData[fx_idx].id] = pool;
			this.fxSpawner[(int)this.fxSpawnData[fx_idx].id] = delegate(Vector3 pos, float rotation)
			{
				Action<object> action = delegate(object obj)
				{
					int num = Grid.PosToCell(pos);
					if ((this.activeFX[num] & fx_mask) == 0)
					{
						ushort[] array = this.activeFX;
						int num2 = num;
						array[num2] |= fx_mask;
						GameObject instance = pool.GetInstance();
						Game.SpawnPoolData spawnPoolData = this.fxSpawnData[fx_idx];
						Quaternion rotation = Quaternion.identity;
						bool flipX = false;
						string s = spawnPoolData.initialAnim;
						Game.SpawnRotationConfig rotationConfig = spawnPoolData.rotationConfig;
						if (rotationConfig != Game.SpawnRotationConfig.Normal)
						{
							if (rotationConfig == Game.SpawnRotationConfig.StringName)
							{
								int num3 = (int)(rotation / 90f);
								if (num3 < 0)
								{
									num3 += spawnPoolData.rotationData.Length;
								}
								s = spawnPoolData.rotationData[num3].animName;
								flipX = spawnPoolData.rotationData[num3].flip;
							}
						}
						else
						{
							rotation = Quaternion.Euler(0f, 0f, rotation);
						}
						pos += spawnPoolData.spawnOffset;
						Vector2 vector = UnityEngine.Random.insideUnitCircle;
						vector.x *= spawnPoolData.spawnRandomOffset.x;
						vector.y *= spawnPoolData.spawnRandomOffset.y;
						vector = rotation * vector;
						pos.x += vector.x;
						pos.y += vector.y;
						instance.transform.SetPosition(pos);
						instance.transform.rotation = rotation;
						KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
						component.FlipX = flipX;
						component.TintColour = spawnPoolData.colour;
						component.Play(s, KAnim.PlayMode.Once, 1f, 0f);
						component.enabled = true;
					}
				};
				if (Game.Instance.IsPaused)
				{
					action(null);
					return;
				}
				GameScheduler.Instance.Schedule("SpawnFX", 0f, action, null, null);
			};
		}
	}

	// Token: 0x0600641D RID: 25629 RVA: 0x002BF76C File Offset: 0x002BD96C
	public void SpawnFX(SpawnFXHashes fx_id, int cell, float rotation)
	{
		Vector3 vector = Grid.CellToPosCBC(cell, Grid.SceneLayer.Front);
		if (CameraController.Instance.IsVisiblePos(vector))
		{
			this.fxSpawner[(int)fx_id](vector, rotation);
		}
	}

	// Token: 0x0600641E RID: 25630 RVA: 0x000E15C5 File Offset: 0x000DF7C5
	public void SpawnFX(SpawnFXHashes fx_id, Vector3 pos, float rotation)
	{
		this.fxSpawner[(int)fx_id](pos, rotation);
	}

	// Token: 0x0600641F RID: 25631 RVA: 0x000E15DA File Offset: 0x000DF7DA
	public static void SaveSettings(BinaryWriter writer)
	{
		Serializer.Serialize(new Game.Settings(Game.Instance), writer);
	}

	// Token: 0x06006420 RID: 25632 RVA: 0x002BF7A4 File Offset: 0x002BD9A4
	public static void LoadSettings(Deserializer deserializer)
	{
		Game.Settings settings = new Game.Settings();
		deserializer.Deserialize(settings);
		KPlayerPrefs.SetInt(Game.NextUniqueIDKey, settings.nextUniqueID);
		KleiMetrics.SetGameID(settings.gameID);
	}

	// Token: 0x06006421 RID: 25633 RVA: 0x002BF7DC File Offset: 0x002BD9DC
	public void Save(BinaryWriter writer)
	{
		Game.GameSaveData gameSaveData = new Game.GameSaveData();
		gameSaveData.gasConduitFlow = this.gasConduitFlow;
		gameSaveData.liquidConduitFlow = this.liquidConduitFlow;
		gameSaveData.fallingWater = this.world.GetComponent<FallingWater>();
		gameSaveData.unstableGround = this.world.GetComponent<UnstableGroundManager>();
		gameSaveData.worldDetail = SaveLoader.Instance.clusterDetailSave;
		gameSaveData.debugWasUsed = this.debugWasUsed;
		gameSaveData.customGameSettings = CustomGameSettings.Instance;
		gameSaveData.storySetings = StoryManager.Instance;
		gameSaveData.spaceScannerNetworkManager = Game.Instance.spaceScannerNetworkManager;
		gameSaveData.autoPrioritizeRoles = this.autoPrioritizeRoles;
		gameSaveData.advancedPersonalPriorities = this.advancedPersonalPriorities;
		gameSaveData.savedInfo = this.savedInfo;
		global::Debug.Assert(gameSaveData.worldDetail != null, "World detail null");
		gameSaveData.dateGenerated = this.dateGenerated;
		if (!this.changelistsPlayedOn.Contains(642695U))
		{
			this.changelistsPlayedOn.Add(642695U);
		}
		gameSaveData.changelistsPlayedOn = this.changelistsPlayedOn;
		if (this.OnSave != null)
		{
			this.OnSave(gameSaveData);
		}
		Serializer.Serialize(gameSaveData, writer);
	}

	// Token: 0x06006422 RID: 25634 RVA: 0x002BF8F8 File Offset: 0x002BDAF8
	public void Load(Deserializer deserializer)
	{
		Game.GameSaveData gameSaveData = new Game.GameSaveData();
		gameSaveData.gasConduitFlow = this.gasConduitFlow;
		gameSaveData.liquidConduitFlow = this.liquidConduitFlow;
		gameSaveData.fallingWater = this.world.GetComponent<FallingWater>();
		gameSaveData.unstableGround = this.world.GetComponent<UnstableGroundManager>();
		gameSaveData.worldDetail = new WorldDetailSave();
		gameSaveData.customGameSettings = CustomGameSettings.Instance;
		gameSaveData.storySetings = StoryManager.Instance;
		gameSaveData.spaceScannerNetworkManager = Game.Instance.spaceScannerNetworkManager;
		deserializer.Deserialize(gameSaveData);
		this.gasConduitFlow = gameSaveData.gasConduitFlow;
		this.liquidConduitFlow = gameSaveData.liquidConduitFlow;
		this.debugWasUsed = gameSaveData.debugWasUsed;
		this.autoPrioritizeRoles = gameSaveData.autoPrioritizeRoles;
		this.advancedPersonalPriorities = gameSaveData.advancedPersonalPriorities;
		this.dateGenerated = gameSaveData.dateGenerated;
		this.changelistsPlayedOn = (gameSaveData.changelistsPlayedOn ?? new List<uint>());
		if (gameSaveData.dateGenerated.IsNullOrWhiteSpace())
		{
			this.dateGenerated = "Before U41 (Feb 2022)";
		}
		DebugUtil.LogArgs(new object[]
		{
			"SAVEINFO"
		});
		DebugUtil.LogArgs(new object[]
		{
			" - Generated: " + this.dateGenerated
		});
		DebugUtil.LogArgs(new object[]
		{
			" - Played on: " + string.Join<uint>(", ", this.changelistsPlayedOn)
		});
		DebugUtil.LogArgs(new object[]
		{
			" - Debug was used: " + Game.Instance.debugWasUsed.ToString()
		});
		this.savedInfo = gameSaveData.savedInfo;
		this.savedInfo.InitializeEmptyVariables();
		CustomGameSettings.Instance.Print();
		KCrashReporter.debugWasUsed = this.debugWasUsed;
		SaveLoader.Instance.SetWorldDetail(gameSaveData.worldDetail);
		if (this.OnLoad != null)
		{
			this.OnLoad(gameSaveData);
		}
	}

	// Token: 0x06006423 RID: 25635 RVA: 0x000E15EC File Offset: 0x000DF7EC
	public void SetAutoSaveCallbacks(Game.SavingPreCB activatePreCB, Game.SavingActiveCB activateActiveCB, Game.SavingPostCB activatePostCB)
	{
		this.activatePreCB = activatePreCB;
		this.activateActiveCB = activateActiveCB;
		this.activatePostCB = activatePostCB;
	}

	// Token: 0x06006424 RID: 25636 RVA: 0x000E1603 File Offset: 0x000DF803
	public void StartDelayedInitialSave()
	{
		base.StartCoroutine(this.DelayedInitialSave());
	}

	// Token: 0x06006425 RID: 25637 RVA: 0x000E1612 File Offset: 0x000DF812
	private IEnumerator DelayedInitialSave()
	{
		int num;
		for (int i = 0; i < 1; i = num)
		{
			yield return null;
			num = i + 1;
		}
		if (GenericGameSettings.instance.devAutoWorldGenActive)
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				worldContainer.SetDiscovered(true);
			}
			SaveGame.Instance.worldGenSpawner.SpawnEverything();
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().DEBUG_REVEAL_ENTIRE_MAP();
			if (CameraController.Instance != null)
			{
				CameraController.Instance.EnableFreeCamera(true);
			}
			for (int num2 = 0; num2 != Grid.WidthInCells * Grid.HeightInCells; num2++)
			{
				Grid.Reveal(num2, byte.MaxValue, false);
			}
			GenericGameSettings.instance.devAutoWorldGenActive = false;
		}
		SaveLoader.Instance.InitialSave();
		yield break;
	}

	// Token: 0x06006426 RID: 25638 RVA: 0x002BFAC4 File Offset: 0x002BDCC4
	public void StartDelayedSave(string filename, bool isAutoSave = false, bool updateSavePointer = true)
	{
		if (this.activatePreCB != null)
		{
			this.activatePreCB(delegate
			{
				this.StartCoroutine(this.DelayedSave(filename, isAutoSave, updateSavePointer));
			});
			return;
		}
		base.StartCoroutine(this.DelayedSave(filename, isAutoSave, updateSavePointer));
	}

	// Token: 0x06006427 RID: 25639 RVA: 0x000E161A File Offset: 0x000DF81A
	private IEnumerator DelayedSave(string filename, bool isAutoSave, bool updateSavePointer)
	{
		while (PlayerController.Instance.IsDragging())
		{
			yield return null;
		}
		PlayerController.Instance.CancelDragging();
		PlayerController.Instance.AllowDragging(false);
		int num;
		for (int i = 0; i < 1; i = num)
		{
			yield return null;
			num = i + 1;
		}
		if (this.activateActiveCB != null)
		{
			this.activateActiveCB();
			for (int i = 0; i < 1; i = num)
			{
				yield return null;
				num = i + 1;
			}
		}
		SaveLoader.Instance.Save(filename, isAutoSave, updateSavePointer);
		if (this.activatePostCB != null)
		{
			this.activatePostCB();
		}
		for (int i = 0; i < 5; i = num)
		{
			yield return null;
			num = i + 1;
		}
		PlayerController.Instance.AllowDragging(true);
		yield break;
	}

	// Token: 0x06006428 RID: 25640 RVA: 0x000E163E File Offset: 0x000DF83E
	public void StartDelayed(int tick_delay, System.Action action)
	{
		base.StartCoroutine(this.DelayedExecutor(tick_delay, action));
	}

	// Token: 0x06006429 RID: 25641 RVA: 0x000E164F File Offset: 0x000DF84F
	private IEnumerator DelayedExecutor(int tick_delay, System.Action action)
	{
		int num;
		for (int i = 0; i < tick_delay; i = num)
		{
			yield return null;
			num = i + 1;
		}
		action();
		yield break;
	}

	// Token: 0x0600642A RID: 25642 RVA: 0x002BFB34 File Offset: 0x002BDD34
	private void LoadEventHashes()
	{
		foreach (object obj in Enum.GetValues(typeof(GameHashes)))
		{
			GameHashes hash = (GameHashes)obj;
			HashCache.Get().Add((int)hash, hash.ToString());
		}
		foreach (object obj2 in Enum.GetValues(typeof(UtilHashes)))
		{
			UtilHashes hash2 = (UtilHashes)obj2;
			HashCache.Get().Add((int)hash2, hash2.ToString());
		}
		foreach (object obj3 in Enum.GetValues(typeof(UIHashes)))
		{
			UIHashes hash3 = (UIHashes)obj3;
			HashCache.Get().Add((int)hash3, hash3.ToString());
		}
	}

	// Token: 0x0600642B RID: 25643 RVA: 0x002BFC70 File Offset: 0x002BDE70
	public void StopFE()
	{
		if (SteamUGCService.Instance)
		{
			SteamUGCService.Instance.enabled = false;
		}
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSnapshot, STOP_MODE.ALLOWFADEOUT);
		if (MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
		{
			MusicManager.instance.StopSong("Music_FrontEnd", true, STOP_MODE.ALLOWFADEOUT);
		}
		MainMenu.Instance.StopMainMenuMusic();
	}

	// Token: 0x0600642C RID: 25644 RVA: 0x000E1665 File Offset: 0x000DF865
	public void StartBE()
	{
		Resources.UnloadUnusedAssets();
		AudioMixer.instance.Reset();
		AudioMixer.instance.StartPersistentSnapshots();
		MusicManager.instance.ConfigureSongs();
		if (MusicManager.instance.ShouldPlayDynamicMusicLoadedGame())
		{
			MusicManager.instance.PlayDynamicMusic();
		}
	}

	// Token: 0x0600642D RID: 25645 RVA: 0x002BFCD8 File Offset: 0x002BDED8
	public void StopBE()
	{
		if (SteamUGCService.Instance)
		{
			SteamUGCService.Instance.enabled = true;
		}
		LoopingSoundManager loopingSoundManager = LoopingSoundManager.Get();
		if (loopingSoundManager != null)
		{
			loopingSoundManager.StopAllSounds();
		}
		MusicManager.instance.KillAllSongs(STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.StopPersistentSnapshots();
		foreach (List<SaveLoadRoot> list in SaveLoader.Instance.saveManager.GetLists().Values)
		{
			foreach (SaveLoadRoot saveLoadRoot in list)
			{
				if (saveLoadRoot.gameObject != null)
				{
					global::Util.KDestroyGameObject(saveLoadRoot.gameObject);
				}
			}
		}
		base.GetComponent<EntombedItemVisualizer>().Clear();
		SimTemperatureTransfer.ClearInstanceMap();
		StructureTemperatureComponents.ClearInstanceMap();
		ElementConsumer.ClearInstanceMap();
		KComponentSpawn.instance.comps.Clear();
		KInputHandler.Remove(Global.GetInputManager().GetDefaultController(), this.cameraController);
		KInputHandler.Remove(Global.GetInputManager().GetDefaultController(), this.playerController);
		Sim.Shutdown();
		SimAndRenderScheduler.instance.Reset();
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x0600642E RID: 25646 RVA: 0x000E16A1 File Offset: 0x000DF8A1
	public void SetStatusItemOffset(Transform transform, Vector3 offset)
	{
		this.statusItemRenderer.SetOffset(transform, offset);
	}

	// Token: 0x0600642F RID: 25647 RVA: 0x000E16B0 File Offset: 0x000DF8B0
	public void AddStatusItem(Transform transform, StatusItem status_item)
	{
		this.statusItemRenderer.Add(transform, status_item);
	}

	// Token: 0x06006430 RID: 25648 RVA: 0x000E16BF File Offset: 0x000DF8BF
	public void RemoveStatusItem(Transform transform, StatusItem status_item)
	{
		this.statusItemRenderer.Remove(transform, status_item);
	}

	// Token: 0x1700064B RID: 1611
	// (get) Token: 0x06006431 RID: 25649 RVA: 0x000E16CE File Offset: 0x000DF8CE
	public float LastTimeWorkStarted
	{
		get
		{
			return this.lastTimeWorkStarted;
		}
	}

	// Token: 0x06006432 RID: 25650 RVA: 0x000E16D6 File Offset: 0x000DF8D6
	public void StartedWork()
	{
		this.lastTimeWorkStarted = Time.time;
	}

	// Token: 0x06006433 RID: 25651 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void SpawnOxygenBubbles(Vector3 position, float angle)
	{
	}

	// Token: 0x06006434 RID: 25652 RVA: 0x000E16E3 File Offset: 0x000DF8E3
	public void ManualReleaseHandle(HandleVector<Game.CallbackInfo>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.callbackManagerManuallyReleasedHandles.Add(handle.index);
		this.callbackManager.Release(handle);
	}

	// Token: 0x06006435 RID: 25653 RVA: 0x000E170E File Offset: 0x000DF90E
	private bool IsManuallyReleasedHandle(HandleVector<Game.CallbackInfo>.Handle handle)
	{
		return !this.callbackManager.IsVersionValid(handle) && this.callbackManagerManuallyReleasedHandles.Contains(handle.index);
	}

	// Token: 0x06006436 RID: 25654 RVA: 0x000E1735 File Offset: 0x000DF935
	[ContextMenu("Print")]
	private void Print()
	{
		Console.WriteLine("This is a console writeline test");
		global::Debug.Log("This is a debug log test");
	}

	// Token: 0x06006437 RID: 25655 RVA: 0x002BFE28 File Offset: 0x002BE028
	private void DestroyInstances()
	{
		KMonoBehaviour.lastGameObject = null;
		KMonoBehaviour.lastObj = null;
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
		global::DateTime.DestroyInstance();
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
		WorldResourceAmountTracker<RationTracker>.DestroyInstance();
		WorldResourceAmountTracker<ElectrobankTracker>.DestroyInstance();
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
		KBatchedAnimUpdater instance = Singleton<KBatchedAnimUpdater>.Instance;
		if (instance != null)
		{
			instance.InitializeGrid();
		}
		GlobalChoreProvider.DestroyInstance();
		WorldSelector.DestroyInstance();
		ColonyDiagnosticUtility.DestroyInstance();
		DiscoveredResources.DestroyInstance();
		ClusterMapSelectTool.DestroyInstance();
		StoryManager.DestroyInstance();
		AnimEventHandlerManager.DestroyInstance();
		Game.Instance = null;
		Game.BrainScheduler = null;
		Grid.OnReveal = null;
		this.VisualTunerElement = null;
		Assets.ClearOnAddPrefab();
		KMonoBehaviour.lastGameObject = null;
		KMonoBehaviour.lastObj = null;
		(KComponentSpawn.instance.comps as GameComps).Clear();
	}

	// Token: 0x0400476D RID: 18285
	private static readonly Thread MainThread = Thread.CurrentThread;

	// Token: 0x0400476E RID: 18286
	private static readonly string NextUniqueIDKey = "NextUniqueID";

	// Token: 0x0400476F RID: 18287
	public static string clusterId = null;

	// Token: 0x04004770 RID: 18288
	private PlayerController playerController;

	// Token: 0x04004771 RID: 18289
	private CameraController cameraController;

	// Token: 0x04004772 RID: 18290
	public Action<Game.GameSaveData> OnSave;

	// Token: 0x04004773 RID: 18291
	public Action<Game.GameSaveData> OnLoad;

	// Token: 0x04004774 RID: 18292
	public System.Action OnSpawnComplete;

	// Token: 0x04004775 RID: 18293
	[NonSerialized]
	public bool baseAlreadyCreated;

	// Token: 0x04004776 RID: 18294
	[NonSerialized]
	public bool autoPrioritizeRoles;

	// Token: 0x04004777 RID: 18295
	[NonSerialized]
	public bool advancedPersonalPriorities;

	// Token: 0x04004778 RID: 18296
	public Game.SavedInfo savedInfo;

	// Token: 0x04004779 RID: 18297
	public static bool quitting = false;

	// Token: 0x0400477B RID: 18299
	public AssignmentManager assignmentManager;

	// Token: 0x0400477C RID: 18300
	public GameObject playerPrefab;

	// Token: 0x0400477D RID: 18301
	public GameObject screenManagerPrefab;

	// Token: 0x0400477E RID: 18302
	public GameObject cameraControllerPrefab;

	// Token: 0x04004780 RID: 18304
	private static Camera m_CachedCamera = null;

	// Token: 0x04004781 RID: 18305
	public GameObject tempIntroScreenPrefab;

	// Token: 0x04004782 RID: 18306
	public static int BlockSelectionLayerMask;

	// Token: 0x04004783 RID: 18307
	public static int PickupableLayer;

	// Token: 0x04004784 RID: 18308
	public static BrainScheduler BrainScheduler;

	// Token: 0x04004785 RID: 18309
	public Element VisualTunerElement;

	// Token: 0x04004786 RID: 18310
	public float currentFallbackSunlightIntensity;

	// Token: 0x04004787 RID: 18311
	public RoomProber roomProber;

	// Token: 0x04004788 RID: 18312
	public SpaceScannerNetworkManager spaceScannerNetworkManager;

	// Token: 0x04004789 RID: 18313
	public FetchManager fetchManager;

	// Token: 0x0400478A RID: 18314
	public EdiblesManager ediblesManager;

	// Token: 0x0400478B RID: 18315
	public SpacecraftManager spacecraftManager;

	// Token: 0x0400478C RID: 18316
	public UserMenu userMenu;

	// Token: 0x0400478D RID: 18317
	public Unlocks unlocks;

	// Token: 0x0400478E RID: 18318
	public Timelapser timelapser;

	// Token: 0x0400478F RID: 18319
	private bool sandboxModeActive;

	// Token: 0x04004790 RID: 18320
	public HandleVector<Game.CallbackInfo> callbackManager = new HandleVector<Game.CallbackInfo>(256);

	// Token: 0x04004791 RID: 18321
	public List<int> callbackManagerManuallyReleasedHandles = new List<int>();

	// Token: 0x04004792 RID: 18322
	public Game.ComplexCallbackHandleVector<int> simComponentCallbackManager = new Game.ComplexCallbackHandleVector<int>(256);

	// Token: 0x04004793 RID: 18323
	public Game.ComplexCallbackHandleVector<Sim.MassConsumedCallback> massConsumedCallbackManager = new Game.ComplexCallbackHandleVector<Sim.MassConsumedCallback>(64);

	// Token: 0x04004794 RID: 18324
	public Game.ComplexCallbackHandleVector<Sim.MassEmittedCallback> massEmitCallbackManager = new Game.ComplexCallbackHandleVector<Sim.MassEmittedCallback>(64);

	// Token: 0x04004795 RID: 18325
	public Game.ComplexCallbackHandleVector<Sim.DiseaseConsumptionCallback> diseaseConsumptionCallbackManager = new Game.ComplexCallbackHandleVector<Sim.DiseaseConsumptionCallback>(64);

	// Token: 0x04004796 RID: 18326
	public Game.ComplexCallbackHandleVector<Sim.ConsumedRadiationCallback> radiationConsumedCallbackManager = new Game.ComplexCallbackHandleVector<Sim.ConsumedRadiationCallback>(256);

	// Token: 0x04004797 RID: 18327
	[NonSerialized]
	public Player LocalPlayer;

	// Token: 0x04004798 RID: 18328
	[SerializeField]
	public TextAsset maleNamesFile;

	// Token: 0x04004799 RID: 18329
	[SerializeField]
	public TextAsset femaleNamesFile;

	// Token: 0x0400479A RID: 18330
	[NonSerialized]
	public World world;

	// Token: 0x0400479B RID: 18331
	[NonSerialized]
	public CircuitManager circuitManager;

	// Token: 0x0400479C RID: 18332
	[NonSerialized]
	public EnergySim energySim;

	// Token: 0x0400479D RID: 18333
	[NonSerialized]
	public LogicCircuitManager logicCircuitManager;

	// Token: 0x0400479E RID: 18334
	private GameScreenManager screenMgr;

	// Token: 0x0400479F RID: 18335
	public UtilityNetworkManager<FlowUtilityNetwork, Vent> gasConduitSystem;

	// Token: 0x040047A0 RID: 18336
	public UtilityNetworkManager<FlowUtilityNetwork, Vent> liquidConduitSystem;

	// Token: 0x040047A1 RID: 18337
	public UtilityNetworkManager<ElectricalUtilityNetwork, Wire> electricalConduitSystem;

	// Token: 0x040047A2 RID: 18338
	public UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem;

	// Token: 0x040047A3 RID: 18339
	public UtilityNetworkTubesManager travelTubeSystem;

	// Token: 0x040047A4 RID: 18340
	public UtilityNetworkManager<FlowUtilityNetwork, SolidConduit> solidConduitSystem;

	// Token: 0x040047A5 RID: 18341
	public ConduitFlow gasConduitFlow;

	// Token: 0x040047A6 RID: 18342
	public ConduitFlow liquidConduitFlow;

	// Token: 0x040047A7 RID: 18343
	public SolidConduitFlow solidConduitFlow;

	// Token: 0x040047A8 RID: 18344
	public Accumulators accumulators;

	// Token: 0x040047A9 RID: 18345
	public PlantElementAbsorbers plantElementAbsorbers;

	// Token: 0x040047AA RID: 18346
	public Game.TemperatureOverlayModes temperatureOverlayMode;

	// Token: 0x040047AB RID: 18347
	public bool showExpandedTemperatures;

	// Token: 0x040047AC RID: 18348
	public List<Tag> tileOverlayFilters = new List<Tag>();

	// Token: 0x040047AD RID: 18349
	public bool showGasConduitDisease;

	// Token: 0x040047AE RID: 18350
	public bool showLiquidConduitDisease;

	// Token: 0x040047AF RID: 18351
	public ConduitFlowVisualizer gasFlowVisualizer;

	// Token: 0x040047B0 RID: 18352
	public ConduitFlowVisualizer liquidFlowVisualizer;

	// Token: 0x040047B1 RID: 18353
	public SolidConduitFlowVisualizer solidFlowVisualizer;

	// Token: 0x040047B2 RID: 18354
	public ConduitTemperatureManager conduitTemperatureManager;

	// Token: 0x040047B3 RID: 18355
	public ConduitDiseaseManager conduitDiseaseManager;

	// Token: 0x040047B4 RID: 18356
	public MingleCellTracker mingleCellTracker;

	// Token: 0x040047B5 RID: 18357
	private int simSubTick;

	// Token: 0x040047B6 RID: 18358
	private bool hasFirstSimTickRun;

	// Token: 0x040047B7 RID: 18359
	private float simDt;

	// Token: 0x040047B8 RID: 18360
	public string dateGenerated;

	// Token: 0x040047B9 RID: 18361
	public List<uint> changelistsPlayedOn;

	// Token: 0x040047BA RID: 18362
	[SerializeField]
	public Game.ConduitVisInfo liquidConduitVisInfo;

	// Token: 0x040047BB RID: 18363
	[SerializeField]
	public Game.ConduitVisInfo gasConduitVisInfo;

	// Token: 0x040047BC RID: 18364
	[SerializeField]
	public Game.ConduitVisInfo solidConduitVisInfo;

	// Token: 0x040047BD RID: 18365
	[SerializeField]
	private Material liquidFlowMaterial;

	// Token: 0x040047BE RID: 18366
	[SerializeField]
	private Material gasFlowMaterial;

	// Token: 0x040047BF RID: 18367
	[SerializeField]
	private Color flowColour;

	// Token: 0x040047C0 RID: 18368
	private Vector3 gasFlowPos;

	// Token: 0x040047C1 RID: 18369
	private Vector3 liquidFlowPos;

	// Token: 0x040047C2 RID: 18370
	private Vector3 solidFlowPos;

	// Token: 0x040047C3 RID: 18371
	public bool drawStatusItems = true;

	// Token: 0x040047C4 RID: 18372
	private List<SolidInfo> solidInfo = new List<SolidInfo>();

	// Token: 0x040047C5 RID: 18373
	private List<Klei.CallbackInfo> callbackInfo = new List<Klei.CallbackInfo>();

	// Token: 0x040047C6 RID: 18374
	private List<SolidInfo> gameSolidInfo = new List<SolidInfo>();

	// Token: 0x040047C7 RID: 18375
	private bool IsPaused;

	// Token: 0x040047C8 RID: 18376
	private HashSet<int> solidChangedFilter = new HashSet<int>();

	// Token: 0x040047C9 RID: 18377
	private HashedString lastDrawnOverlayMode;

	// Token: 0x040047CA RID: 18378
	private EntityCellVisualizer previewVisualizer;

	// Token: 0x040047CD RID: 18381
	public SafetyConditions safetyConditions = new SafetyConditions();

	// Token: 0x040047CE RID: 18382
	public SimData simData = new SimData();

	// Token: 0x040047CF RID: 18383
	[MyCmpGet]
	private GameScenePartitioner gameScenePartitioner;

	// Token: 0x040047D0 RID: 18384
	private bool gameStarted;

	// Token: 0x040047D1 RID: 18385
	private static readonly EventSystem.IntraObjectHandler<Game> MarkStatusItemRendererDirtyDelegate = new EventSystem.IntraObjectHandler<Game>(delegate(Game component, object data)
	{
		component.MarkStatusItemRendererDirty(data);
	});

	// Token: 0x040047D2 RID: 18386
	private static readonly EventSystem.IntraObjectHandler<Game> ActiveWorldChangedDelegate = new EventSystem.IntraObjectHandler<Game>(delegate(Game component, object data)
	{
		component.ForceOverlayUpdate(true);
	});

	// Token: 0x040047D3 RID: 18387
	private ushort[] activeFX;

	// Token: 0x040047D4 RID: 18388
	public bool debugWasUsed;

	// Token: 0x040047D5 RID: 18389
	private bool isLoading;

	// Token: 0x040047D6 RID: 18390
	private List<Game.SimActiveRegion> simActiveRegions = new List<Game.SimActiveRegion>();

	// Token: 0x040047D7 RID: 18391
	private HashedString previousOverlayMode = OverlayModes.None.ID;

	// Token: 0x040047D8 RID: 18392
	private float previousGasConduitFlowDiscreteLerpPercent = -1f;

	// Token: 0x040047D9 RID: 18393
	private float previousLiquidConduitFlowDiscreteLerpPercent = -1f;

	// Token: 0x040047DA RID: 18394
	private float previousSolidConduitFlowDiscreteLerpPercent = -1f;

	// Token: 0x040047DB RID: 18395
	[SerializeField]
	private Game.SpawnPoolData[] fxSpawnData;

	// Token: 0x040047DC RID: 18396
	private Dictionary<int, Action<Vector3, float>> fxSpawner = new Dictionary<int, Action<Vector3, float>>();

	// Token: 0x040047DD RID: 18397
	private Dictionary<int, GameObjectPool> fxPools = new Dictionary<int, GameObjectPool>();

	// Token: 0x040047DE RID: 18398
	private Game.SavingPreCB activatePreCB;

	// Token: 0x040047DF RID: 18399
	private Game.SavingActiveCB activateActiveCB;

	// Token: 0x040047E0 RID: 18400
	private Game.SavingPostCB activatePostCB;

	// Token: 0x040047E1 RID: 18401
	[SerializeField]
	public Game.UIColours uiColours = new Game.UIColours();

	// Token: 0x040047E2 RID: 18402
	private float lastTimeWorkStarted = float.NegativeInfinity;

	// Token: 0x02001304 RID: 4868
	[Serializable]
	public struct SavedInfo
	{
		// Token: 0x0600643B RID: 25659 RVA: 0x000E1758 File Offset: 0x000DF958
		[OnDeserialized]
		private void OnDeserialized()
		{
			this.InitializeEmptyVariables();
		}

		// Token: 0x0600643C RID: 25660 RVA: 0x000E1760 File Offset: 0x000DF960
		public void InitializeEmptyVariables()
		{
			if (this.creaturePoopAmount == null)
			{
				this.creaturePoopAmount = new Dictionary<Tag, float>();
			}
			if (this.powerCreatedbyGeneratorType == null)
			{
				this.powerCreatedbyGeneratorType = new Dictionary<Tag, float>();
			}
		}

		// Token: 0x040047E3 RID: 18403
		public bool discoveredSurface;

		// Token: 0x040047E4 RID: 18404
		public bool discoveredOilField;

		// Token: 0x040047E5 RID: 18405
		public bool curedDisease;

		// Token: 0x040047E6 RID: 18406
		public bool blockedCometWithBunkerDoor;

		// Token: 0x040047E7 RID: 18407
		public Dictionary<Tag, float> creaturePoopAmount;

		// Token: 0x040047E8 RID: 18408
		public Dictionary<Tag, float> powerCreatedbyGeneratorType;
	}

	// Token: 0x02001305 RID: 4869
	public struct CallbackInfo
	{
		// Token: 0x0600643D RID: 25661 RVA: 0x000E1788 File Offset: 0x000DF988
		public CallbackInfo(System.Action cb, bool manually_release = false)
		{
			this.cb = cb;
			this.manuallyRelease = manually_release;
		}

		// Token: 0x040047E9 RID: 18409
		public System.Action cb;

		// Token: 0x040047EA RID: 18410
		public bool manuallyRelease;
	}

	// Token: 0x02001306 RID: 4870
	public struct ComplexCallbackInfo<DataType>
	{
		// Token: 0x0600643E RID: 25662 RVA: 0x000E1798 File Offset: 0x000DF998
		public ComplexCallbackInfo(Action<DataType, object> cb, object callback_data, string debug_info)
		{
			this.cb = cb;
			this.debugInfo = debug_info;
			this.callbackData = callback_data;
		}

		// Token: 0x040047EB RID: 18411
		public Action<DataType, object> cb;

		// Token: 0x040047EC RID: 18412
		public object callbackData;

		// Token: 0x040047ED RID: 18413
		public string debugInfo;
	}

	// Token: 0x02001307 RID: 4871
	public class ComplexCallbackHandleVector<DataType>
	{
		// Token: 0x0600643F RID: 25663 RVA: 0x000E17AF File Offset: 0x000DF9AF
		public ComplexCallbackHandleVector(int initial_size)
		{
			this.baseMgr = new HandleVector<Game.ComplexCallbackInfo<DataType>>(initial_size);
		}

		// Token: 0x06006440 RID: 25664 RVA: 0x000E17CE File Offset: 0x000DF9CE
		public HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle Add(Action<DataType, object> cb, object callback_data, string debug_info)
		{
			return this.baseMgr.Add(new Game.ComplexCallbackInfo<DataType>(cb, callback_data, debug_info));
		}

		// Token: 0x06006441 RID: 25665 RVA: 0x002C0300 File Offset: 0x002BE500
		public Game.ComplexCallbackInfo<DataType> GetItem(HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle)
		{
			Game.ComplexCallbackInfo<DataType> item;
			try
			{
				item = this.baseMgr.GetItem(handle);
			}
			catch (Exception ex)
			{
				byte b;
				int key;
				this.baseMgr.UnpackHandleUnchecked(handle, out b, out key);
				string str = null;
				if (this.releaseInfo.TryGetValue(key, out str))
				{
					KCrashReporter.Assert(false, "Trying to get data for handle that was already released by " + str, null);
				}
				else
				{
					KCrashReporter.Assert(false, "Trying to get data for handle that was released ...... magically", null);
				}
				throw ex;
			}
			return item;
		}

		// Token: 0x06006442 RID: 25666 RVA: 0x002C0370 File Offset: 0x002BE570
		public Game.ComplexCallbackInfo<DataType> Release(HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle, string release_info)
		{
			Game.ComplexCallbackInfo<DataType> result;
			try
			{
				byte b;
				int key;
				this.baseMgr.UnpackHandle(handle, out b, out key);
				this.releaseInfo[key] = release_info;
				result = this.baseMgr.Release(handle);
			}
			catch (Exception ex)
			{
				byte b;
				int key;
				this.baseMgr.UnpackHandleUnchecked(handle, out b, out key);
				string str = null;
				if (this.releaseInfo.TryGetValue(key, out str))
				{
					KCrashReporter.Assert(false, release_info + "is trying to release handle but it was already released by " + str, null);
				}
				else
				{
					KCrashReporter.Assert(false, release_info + "is trying to release a handle that was already released by some unknown thing", null);
				}
				throw ex;
			}
			return result;
		}

		// Token: 0x06006443 RID: 25667 RVA: 0x000E17E3 File Offset: 0x000DF9E3
		public void Clear()
		{
			this.baseMgr.Clear();
		}

		// Token: 0x06006444 RID: 25668 RVA: 0x000E17F0 File Offset: 0x000DF9F0
		public bool IsVersionValid(HandleVector<Game.ComplexCallbackInfo<DataType>>.Handle handle)
		{
			return this.baseMgr.IsVersionValid(handle);
		}

		// Token: 0x040047EE RID: 18414
		private HandleVector<Game.ComplexCallbackInfo<DataType>> baseMgr;

		// Token: 0x040047EF RID: 18415
		private Dictionary<int, string> releaseInfo = new Dictionary<int, string>();
	}

	// Token: 0x02001308 RID: 4872
	public enum TemperatureOverlayModes
	{
		// Token: 0x040047F1 RID: 18417
		AbsoluteTemperature,
		// Token: 0x040047F2 RID: 18418
		AdaptiveTemperature,
		// Token: 0x040047F3 RID: 18419
		HeatFlow,
		// Token: 0x040047F4 RID: 18420
		StateChange,
		// Token: 0x040047F5 RID: 18421
		RelativeTemperature
	}

	// Token: 0x02001309 RID: 4873
	[Serializable]
	public class ConduitVisInfo
	{
		// Token: 0x040047F6 RID: 18422
		public GameObject prefab;

		// Token: 0x040047F7 RID: 18423
		[Header("Main View")]
		public Color32 tint;

		// Token: 0x040047F8 RID: 18424
		public Color32 insulatedTint;

		// Token: 0x040047F9 RID: 18425
		public Color32 radiantTint;

		// Token: 0x040047FA RID: 18426
		[Header("Overlay")]
		public string overlayTintName;

		// Token: 0x040047FB RID: 18427
		public string overlayInsulatedTintName;

		// Token: 0x040047FC RID: 18428
		public string overlayRadiantTintName;

		// Token: 0x040047FD RID: 18429
		public Vector2 overlayMassScaleRange = new Vector2f(1f, 1000f);

		// Token: 0x040047FE RID: 18430
		public Vector2 overlayMassScaleValues = new Vector2f(0.1f, 1f);
	}

	// Token: 0x0200130A RID: 4874
	private class WorldRegion
	{
		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06006446 RID: 25670 RVA: 0x000E183A File Offset: 0x000DFA3A
		public Vector2I regionMin
		{
			get
			{
				return this.min;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06006447 RID: 25671 RVA: 0x000E1842 File Offset: 0x000DFA42
		public Vector2I regionMax
		{
			get
			{
				return this.max;
			}
		}

		// Token: 0x06006448 RID: 25672 RVA: 0x002C0404 File Offset: 0x002BE604
		public void UpdateGameActiveRegion(int x0, int y0, int x1, int y1)
		{
			this.min.x = Mathf.Max(0, x0);
			this.min.y = Mathf.Max(0, y0);
			this.max.x = Mathf.Max(x1, this.regionMax.x);
			this.max.y = Mathf.Max(y1, this.regionMax.y);
		}

		// Token: 0x06006449 RID: 25673 RVA: 0x000E184A File Offset: 0x000DFA4A
		public void UpdateGameActiveRegion(Vector2I simActiveRegionMin, Vector2I simActiveRegionMax)
		{
			this.min = simActiveRegionMin;
			this.max = simActiveRegionMax;
		}

		// Token: 0x040047FF RID: 18431
		private Vector2I min;

		// Token: 0x04004800 RID: 18432
		private Vector2I max;

		// Token: 0x04004801 RID: 18433
		public bool isActive;
	}

	// Token: 0x0200130B RID: 4875
	public class SimActiveRegion
	{
		// Token: 0x0600644B RID: 25675 RVA: 0x000E185A File Offset: 0x000DFA5A
		public SimActiveRegion()
		{
			this.region = default(Pair<Vector2I, Vector2I>);
			this.currentSunlightIntensity = (float)FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;
			this.currentCosmicRadiationIntensity = (float)FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;
		}

		// Token: 0x04004802 RID: 18434
		public Pair<Vector2I, Vector2I> region;

		// Token: 0x04004803 RID: 18435
		public float currentSunlightIntensity;

		// Token: 0x04004804 RID: 18436
		public float currentCosmicRadiationIntensity;
	}

	// Token: 0x0200130C RID: 4876
	private enum SpawnRotationConfig
	{
		// Token: 0x04004806 RID: 18438
		Normal,
		// Token: 0x04004807 RID: 18439
		StringName
	}

	// Token: 0x0200130D RID: 4877
	[Serializable]
	private struct SpawnRotationData
	{
		// Token: 0x04004808 RID: 18440
		public string animName;

		// Token: 0x04004809 RID: 18441
		public bool flip;
	}

	// Token: 0x0200130E RID: 4878
	[Serializable]
	private struct SpawnPoolData
	{
		// Token: 0x0400480A RID: 18442
		[HashedEnum]
		public SpawnFXHashes id;

		// Token: 0x0400480B RID: 18443
		public int initialCount;

		// Token: 0x0400480C RID: 18444
		public Color32 colour;

		// Token: 0x0400480D RID: 18445
		public GameObject fxPrefab;

		// Token: 0x0400480E RID: 18446
		public string initialAnim;

		// Token: 0x0400480F RID: 18447
		public Vector3 spawnOffset;

		// Token: 0x04004810 RID: 18448
		public Vector2 spawnRandomOffset;

		// Token: 0x04004811 RID: 18449
		public Game.SpawnRotationConfig rotationConfig;

		// Token: 0x04004812 RID: 18450
		public Game.SpawnRotationData[] rotationData;
	}

	// Token: 0x0200130F RID: 4879
	[Serializable]
	private class Settings
	{
		// Token: 0x0600644C RID: 25676 RVA: 0x000E1886 File Offset: 0x000DFA86
		public Settings(Game game)
		{
			this.nextUniqueID = KPrefabID.NextUniqueID;
			this.gameID = KleiMetrics.GameID();
		}

		// Token: 0x0600644D RID: 25677 RVA: 0x000A5E2C File Offset: 0x000A402C
		public Settings()
		{
		}

		// Token: 0x04004813 RID: 18451
		public int nextUniqueID;

		// Token: 0x04004814 RID: 18452
		public int gameID;
	}

	// Token: 0x02001310 RID: 4880
	public class GameSaveData
	{
		// Token: 0x04004815 RID: 18453
		public ConduitFlow gasConduitFlow;

		// Token: 0x04004816 RID: 18454
		public ConduitFlow liquidConduitFlow;

		// Token: 0x04004817 RID: 18455
		public FallingWater fallingWater;

		// Token: 0x04004818 RID: 18456
		public UnstableGroundManager unstableGround;

		// Token: 0x04004819 RID: 18457
		public WorldDetailSave worldDetail;

		// Token: 0x0400481A RID: 18458
		public CustomGameSettings customGameSettings;

		// Token: 0x0400481B RID: 18459
		public StoryManager storySetings;

		// Token: 0x0400481C RID: 18460
		public SpaceScannerNetworkManager spaceScannerNetworkManager;

		// Token: 0x0400481D RID: 18461
		public bool debugWasUsed;

		// Token: 0x0400481E RID: 18462
		public bool autoPrioritizeRoles;

		// Token: 0x0400481F RID: 18463
		public bool advancedPersonalPriorities;

		// Token: 0x04004820 RID: 18464
		public Game.SavedInfo savedInfo;

		// Token: 0x04004821 RID: 18465
		public string dateGenerated;

		// Token: 0x04004822 RID: 18466
		public List<uint> changelistsPlayedOn;
	}

	// Token: 0x02001311 RID: 4881
	// (Invoke) Token: 0x06006450 RID: 25680
	public delegate void CansaveCB();

	// Token: 0x02001312 RID: 4882
	// (Invoke) Token: 0x06006454 RID: 25684
	public delegate void SavingPreCB(Game.CansaveCB cb);

	// Token: 0x02001313 RID: 4883
	// (Invoke) Token: 0x06006458 RID: 25688
	public delegate void SavingActiveCB();

	// Token: 0x02001314 RID: 4884
	// (Invoke) Token: 0x0600645C RID: 25692
	public delegate void SavingPostCB();

	// Token: 0x02001315 RID: 4885
	[Serializable]
	public struct LocationColours
	{
		// Token: 0x04004823 RID: 18467
		public Color unreachable;

		// Token: 0x04004824 RID: 18468
		public Color invalidLocation;

		// Token: 0x04004825 RID: 18469
		public Color validLocation;

		// Token: 0x04004826 RID: 18470
		public Color requiresRole;

		// Token: 0x04004827 RID: 18471
		public Color unreachable_requiresRole;
	}

	// Token: 0x02001316 RID: 4886
	[Serializable]
	public class UIColours
	{
		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x0600645F RID: 25695 RVA: 0x000E18A4 File Offset: 0x000DFAA4
		public Game.LocationColours Dig
		{
			get
			{
				return this.digColours;
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06006460 RID: 25696 RVA: 0x000E18AC File Offset: 0x000DFAAC
		public Game.LocationColours Build
		{
			get
			{
				return this.buildColours;
			}
		}

		// Token: 0x04004828 RID: 18472
		[SerializeField]
		private Game.LocationColours digColours;

		// Token: 0x04004829 RID: 18473
		[SerializeField]
		private Game.LocationColours buildColours;
	}
}
