using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

[AddComponentMenu("KMonoBehaviour/scripts/OverlayScreen")]
public class OverlayScreen : KMonoBehaviour {
    public static HashSet<Tag>  WireIDs          = new HashSet<Tag>();
    public static HashSet<Tag>  GasVentIDs       = new HashSet<Tag>();
    public static HashSet<Tag>  LiquidVentIDs    = new HashSet<Tag>();
    public static HashSet<Tag>  HarvestableIDs   = new HashSet<Tag>();
    public static HashSet<Tag>  DiseaseIDs       = new HashSet<Tag>();
    public static HashSet<Tag>  SuitIDs          = new HashSet<Tag>();
    public static HashSet<Tag>  SolidConveyorIDs = new HashSet<Tag>();
    public static HashSet<Tag>  RadiationIDs     = new HashSet<Tag>();
    public static OverlayScreen Instance;

    [SerializeField]
    private Vector3 batteryUIOffset;

    [SerializeField]
    private Vector3 batteryUISmallTransformerOffset;

    [SerializeField]
    private Vector3 batteryUITransformerOffset;

    [SerializeField]
    private BatteryUI batUIPrefab;

    [SerializeField]
    private Color buildingDisabledColour = Color.gray;

    [SerializeField]
    private Color32 circuitOverloadingColour;

    [SerializeField]
    private Color32 circuitSafeColour;

    [SerializeField]
    private Color32 circuitStrainingColour;

    [Header("Circuits"), SerializeField]
    private Color32 circuitUnpoweredColour;

    [SerializeField]
    private Color consumerColour;

    private ModeInfo currentModeInfo;

    [Header("Disease"), SerializeField]
    private GameObject diseaseOverlayPrefab;

    [SerializeField]
    private Color generatorColour;

    [Header("Crops"), SerializeField]
    private GameObject harvestableNotificationPrefab;

    [Header("Logic"), SerializeField]
    private LogicModeUI logicModeUIPrefab;

    private Dictionary<HashedString, ModeInfo> modeInfos = new Dictionary<HashedString, ModeInfo>();
    public  Action<HashedString>               OnOverlayChanged;

    [SerializeField]
    private Vector3 powerLabelOffset;

    [Header("Power"), SerializeField]
    private Canvas powerLabelParent;

    [SerializeField]
    private LocText powerLabelPrefab;

    [Header("Suit"), SerializeField]
    private GameObject suitOverlayPrefab;

    private EventInstance techViewSound;

    [SerializeField]
    public EventReference techViewSoundPath;

    private bool techViewSoundPlaying;

    [SerializeField]
    private TextStyleSetting TooltipDescription;

    [Header("ToolTip"), SerializeField]
    private TextStyleSetting TooltipHeader;

    public HashedString mode => currentModeInfo.mode.ViewMode();

    protected override void OnPrefabInit() {
        Debug.Assert(Instance == null);
        Instance         = this;
        powerLabelParent = GameObject.Find("WorldSpaceCanvas").GetComponent<Canvas>();
    }

    protected override void OnLoadLevel() {
        harvestableNotificationPrefab = null;
        powerLabelParent              = null;
        Instance                      = null;
        OverlayModes.Mode.Clear();
        modeInfos       = null;
        currentModeInfo = default(ModeInfo);
        base.OnLoadLevel();
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        techViewSound        = KFMOD.CreateInstance(techViewSoundPath);
        techViewSoundPlaying = false;
        Shader.SetGlobalVector("_OverlayParams", Vector4.zero);
        RegisterModes();
        currentModeInfo = modeInfos[OverlayModes.None.ID];
    }

    private void RegisterModes() {
        modeInfos.Clear();
        var mode = new OverlayModes.None();
        RegisterMode(mode);
        RegisterMode(new OverlayModes.Oxygen());
        RegisterMode(new OverlayModes.Power(powerLabelParent,
                                            powerLabelPrefab,
                                            batUIPrefab,
                                            powerLabelOffset,
                                            batteryUIOffset,
                                            batteryUITransformerOffset,
                                            batteryUISmallTransformerOffset));

        RegisterMode(new OverlayModes.Temperature());
        RegisterMode(new OverlayModes.ThermalConductivity());
        RegisterMode(new OverlayModes.Light());
        RegisterMode(new OverlayModes.LiquidConduits());
        RegisterMode(new OverlayModes.GasConduits());
        RegisterMode(new OverlayModes.Decor());
        RegisterMode(new OverlayModes.Disease(powerLabelParent, diseaseOverlayPrefab));
        RegisterMode(new OverlayModes.Crop(powerLabelParent, harvestableNotificationPrefab));
        RegisterMode(new OverlayModes.Harvest());
        RegisterMode(new OverlayModes.Priorities());
        RegisterMode(new OverlayModes.HeatFlow());
        RegisterMode(new OverlayModes.Rooms());
        RegisterMode(new OverlayModes.Suit(powerLabelParent, suitOverlayPrefab));
        RegisterMode(new OverlayModes.Logic(logicModeUIPrefab));
        RegisterMode(new OverlayModes.SolidConveyor());
        RegisterMode(new OverlayModes.TileMode());
        RegisterMode(new OverlayModes.Radiation());
    }

    private void RegisterMode(OverlayModes.Mode mode) { modeInfos[mode.ViewMode()] = new ModeInfo { mode = mode }; }
    private void LateUpdate()                         { currentModeInfo.mode.Update(); }

    public void RunPostProcessEffects(RenderTexture src, RenderTexture dest) {
        currentModeInfo.mode.OnRenderImage(src, dest);
    }

    public void ToggleOverlay(HashedString newMode, bool allowSound = true) {
        var flag = allowSound && !(currentModeInfo.mode.ViewMode() == newMode);
        if (newMode != OverlayModes.None.ID) ManagementMenu.Instance.CloseAll();
        currentModeInfo.mode.Disable();
        if (newMode != currentModeInfo.mode.ViewMode() && newMode == OverlayModes.None.ID)
            ManagementMenu.Instance.CloseAll();

        SimDebugView.Instance.SetMode(newMode);
        if (!modeInfos.TryGetValue(newMode, out currentModeInfo)) currentModeInfo = modeInfos[OverlayModes.None.ID];
        currentModeInfo.mode.Enable();
        if (flag) UpdateOverlaySounds();
        if (OverlayModes.None.ID == currentModeInfo.mode.ViewMode()) {
            AudioMixer.instance.Stop(AudioMixerSnapshots.Get().TechFilterOnMigrated);
            MusicManager.instance.SetDynamicMusicOverlayInactive();
            techViewSound.stop(STOP_MODE.ALLOWFADEOUT);
            techViewSoundPlaying = false;
        } else if (!techViewSoundPlaying) {
            AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterOnMigrated);
            MusicManager.instance.SetDynamicMusicOverlayActive();
            techViewSound.start();
            techViewSoundPlaying = true;
        }

        if (OnOverlayChanged != null) OnOverlayChanged(currentModeInfo.mode.ViewMode());
        ActivateLegend();
    }

    private void ActivateLegend() {
        if (OverlayLegend.Instance == null) return;

        OverlayLegend.Instance.SetLegend(currentModeInfo.mode);
    }

    public void Refresh() { LateUpdate(); }

    public HashedString GetMode() {
        if (currentModeInfo.mode == null) return OverlayModes.None.ID;

        return currentModeInfo.mode.ViewMode();
    }

    private void UpdateOverlaySounds() {
        var text = currentModeInfo.mode.GetSoundName();
        if (text != "") {
            text = GlobalAssets.GetSound(text);
            PlaySound(text);
        }
    }

    private struct ModeInfo {
        public OverlayModes.Mode mode;
    }
}