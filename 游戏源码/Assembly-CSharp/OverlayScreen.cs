using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02001B31 RID: 6961
[AddComponentMenu("KMonoBehaviour/scripts/OverlayScreen")]
public class OverlayScreen : KMonoBehaviour
{
	// Token: 0x17000991 RID: 2449
	// (get) Token: 0x06009208 RID: 37384 RVA: 0x000FF595 File Offset: 0x000FD795
	public HashedString mode
	{
		get
		{
			return this.currentModeInfo.mode.ViewMode();
		}
	}

	// Token: 0x06009209 RID: 37385 RVA: 0x000FF5A7 File Offset: 0x000FD7A7
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(OverlayScreen.Instance == null);
		OverlayScreen.Instance = this;
		this.powerLabelParent = GameObject.Find("WorldSpaceCanvas").GetComponent<Canvas>();
	}

	// Token: 0x0600920A RID: 37386 RVA: 0x000FF5D4 File Offset: 0x000FD7D4
	protected override void OnLoadLevel()
	{
		this.harvestableNotificationPrefab = null;
		this.powerLabelParent = null;
		OverlayScreen.Instance = null;
		OverlayModes.Mode.Clear();
		this.modeInfos = null;
		this.currentModeInfo = default(OverlayScreen.ModeInfo);
		base.OnLoadLevel();
	}

	// Token: 0x0600920B RID: 37387 RVA: 0x00385858 File Offset: 0x00383A58
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.techViewSound = KFMOD.CreateInstance(this.techViewSoundPath);
		this.techViewSoundPlaying = false;
		Shader.SetGlobalVector("_OverlayParams", Vector4.zero);
		this.RegisterModes();
		this.currentModeInfo = this.modeInfos[OverlayModes.None.ID];
	}

	// Token: 0x0600920C RID: 37388 RVA: 0x003858B0 File Offset: 0x00383AB0
	private void RegisterModes()
	{
		this.modeInfos.Clear();
		OverlayModes.None mode = new OverlayModes.None();
		this.RegisterMode(mode);
		this.RegisterMode(new OverlayModes.Oxygen());
		this.RegisterMode(new OverlayModes.Power(this.powerLabelParent, this.powerLabelPrefab, this.batUIPrefab, this.powerLabelOffset, this.batteryUIOffset, this.batteryUITransformerOffset, this.batteryUISmallTransformerOffset));
		this.RegisterMode(new OverlayModes.Temperature());
		this.RegisterMode(new OverlayModes.ThermalConductivity());
		this.RegisterMode(new OverlayModes.Light());
		this.RegisterMode(new OverlayModes.LiquidConduits());
		this.RegisterMode(new OverlayModes.GasConduits());
		this.RegisterMode(new OverlayModes.Decor());
		this.RegisterMode(new OverlayModes.Disease(this.powerLabelParent, this.diseaseOverlayPrefab));
		this.RegisterMode(new OverlayModes.Crop(this.powerLabelParent, this.harvestableNotificationPrefab));
		this.RegisterMode(new OverlayModes.Harvest());
		this.RegisterMode(new OverlayModes.Priorities());
		this.RegisterMode(new OverlayModes.HeatFlow());
		this.RegisterMode(new OverlayModes.Rooms());
		this.RegisterMode(new OverlayModes.Suit(this.powerLabelParent, this.suitOverlayPrefab));
		this.RegisterMode(new OverlayModes.Logic(this.logicModeUIPrefab));
		this.RegisterMode(new OverlayModes.SolidConveyor());
		this.RegisterMode(new OverlayModes.TileMode());
		this.RegisterMode(new OverlayModes.Radiation());
	}

	// Token: 0x0600920D RID: 37389 RVA: 0x003859FC File Offset: 0x00383BFC
	private void RegisterMode(OverlayModes.Mode mode)
	{
		this.modeInfos[mode.ViewMode()] = new OverlayScreen.ModeInfo
		{
			mode = mode
		};
	}

	// Token: 0x0600920E RID: 37390 RVA: 0x000FF608 File Offset: 0x000FD808
	private void LateUpdate()
	{
		this.currentModeInfo.mode.Update();
	}

	// Token: 0x0600920F RID: 37391 RVA: 0x000FF61A File Offset: 0x000FD81A
	public void RunPostProcessEffects(RenderTexture src, RenderTexture dest)
	{
		this.currentModeInfo.mode.OnRenderImage(src, dest);
	}

	// Token: 0x06009210 RID: 37392 RVA: 0x00385A2C File Offset: 0x00383C2C
	public void ToggleOverlay(HashedString newMode, bool allowSound = true)
	{
		bool flag = allowSound && !(this.currentModeInfo.mode.ViewMode() == newMode);
		if (newMode != OverlayModes.None.ID)
		{
			ManagementMenu.Instance.CloseAll();
		}
		this.currentModeInfo.mode.Disable();
		if (newMode != this.currentModeInfo.mode.ViewMode() && newMode == OverlayModes.None.ID)
		{
			ManagementMenu.Instance.CloseAll();
		}
		SimDebugView.Instance.SetMode(newMode);
		if (!this.modeInfos.TryGetValue(newMode, out this.currentModeInfo))
		{
			this.currentModeInfo = this.modeInfos[OverlayModes.None.ID];
		}
		this.currentModeInfo.mode.Enable();
		if (flag)
		{
			this.UpdateOverlaySounds();
		}
		if (OverlayModes.None.ID == this.currentModeInfo.mode.ViewMode())
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().TechFilterOnMigrated, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			MusicManager.instance.SetDynamicMusicOverlayInactive();
			this.techViewSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			this.techViewSoundPlaying = false;
		}
		else if (!this.techViewSoundPlaying)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterOnMigrated);
			MusicManager.instance.SetDynamicMusicOverlayActive();
			this.techViewSound.start();
			this.techViewSoundPlaying = true;
		}
		if (this.OnOverlayChanged != null)
		{
			this.OnOverlayChanged(this.currentModeInfo.mode.ViewMode());
		}
		this.ActivateLegend();
	}

	// Token: 0x06009211 RID: 37393 RVA: 0x000FF62E File Offset: 0x000FD82E
	private void ActivateLegend()
	{
		if (OverlayLegend.Instance == null)
		{
			return;
		}
		OverlayLegend.Instance.SetLegend(this.currentModeInfo.mode, false);
	}

	// Token: 0x06009212 RID: 37394 RVA: 0x000FF654 File Offset: 0x000FD854
	public void Refresh()
	{
		this.LateUpdate();
	}

	// Token: 0x06009213 RID: 37395 RVA: 0x000FF65C File Offset: 0x000FD85C
	public HashedString GetMode()
	{
		if (this.currentModeInfo.mode == null)
		{
			return OverlayModes.None.ID;
		}
		return this.currentModeInfo.mode.ViewMode();
	}

	// Token: 0x06009214 RID: 37396 RVA: 0x00385BB4 File Offset: 0x00383DB4
	private void UpdateOverlaySounds()
	{
		string text = this.currentModeInfo.mode.GetSoundName();
		if (text != "")
		{
			text = GlobalAssets.GetSound(text, false);
			KMonoBehaviour.PlaySound(text);
		}
	}

	// Token: 0x04006E5F RID: 28255
	public static HashSet<Tag> WireIDs = new HashSet<Tag>();

	// Token: 0x04006E60 RID: 28256
	public static HashSet<Tag> GasVentIDs = new HashSet<Tag>();

	// Token: 0x04006E61 RID: 28257
	public static HashSet<Tag> LiquidVentIDs = new HashSet<Tag>();

	// Token: 0x04006E62 RID: 28258
	public static HashSet<Tag> HarvestableIDs = new HashSet<Tag>();

	// Token: 0x04006E63 RID: 28259
	public static HashSet<Tag> DiseaseIDs = new HashSet<Tag>();

	// Token: 0x04006E64 RID: 28260
	public static HashSet<Tag> SuitIDs = new HashSet<Tag>();

	// Token: 0x04006E65 RID: 28261
	public static HashSet<Tag> SolidConveyorIDs = new HashSet<Tag>();

	// Token: 0x04006E66 RID: 28262
	public static HashSet<Tag> RadiationIDs = new HashSet<Tag>();

	// Token: 0x04006E67 RID: 28263
	[SerializeField]
	public EventReference techViewSoundPath;

	// Token: 0x04006E68 RID: 28264
	private EventInstance techViewSound;

	// Token: 0x04006E69 RID: 28265
	private bool techViewSoundPlaying;

	// Token: 0x04006E6A RID: 28266
	public static OverlayScreen Instance;

	// Token: 0x04006E6B RID: 28267
	[Header("Power")]
	[SerializeField]
	private Canvas powerLabelParent;

	// Token: 0x04006E6C RID: 28268
	[SerializeField]
	private LocText powerLabelPrefab;

	// Token: 0x04006E6D RID: 28269
	[SerializeField]
	private BatteryUI batUIPrefab;

	// Token: 0x04006E6E RID: 28270
	[SerializeField]
	private Vector3 powerLabelOffset;

	// Token: 0x04006E6F RID: 28271
	[SerializeField]
	private Vector3 batteryUIOffset;

	// Token: 0x04006E70 RID: 28272
	[SerializeField]
	private Vector3 batteryUITransformerOffset;

	// Token: 0x04006E71 RID: 28273
	[SerializeField]
	private Vector3 batteryUISmallTransformerOffset;

	// Token: 0x04006E72 RID: 28274
	[SerializeField]
	private Color consumerColour;

	// Token: 0x04006E73 RID: 28275
	[SerializeField]
	private Color generatorColour;

	// Token: 0x04006E74 RID: 28276
	[SerializeField]
	private Color buildingDisabledColour = Color.gray;

	// Token: 0x04006E75 RID: 28277
	[Header("Circuits")]
	[SerializeField]
	private Color32 circuitUnpoweredColour;

	// Token: 0x04006E76 RID: 28278
	[SerializeField]
	private Color32 circuitSafeColour;

	// Token: 0x04006E77 RID: 28279
	[SerializeField]
	private Color32 circuitStrainingColour;

	// Token: 0x04006E78 RID: 28280
	[SerializeField]
	private Color32 circuitOverloadingColour;

	// Token: 0x04006E79 RID: 28281
	[Header("Crops")]
	[SerializeField]
	private GameObject harvestableNotificationPrefab;

	// Token: 0x04006E7A RID: 28282
	[Header("Disease")]
	[SerializeField]
	private GameObject diseaseOverlayPrefab;

	// Token: 0x04006E7B RID: 28283
	[Header("Suit")]
	[SerializeField]
	private GameObject suitOverlayPrefab;

	// Token: 0x04006E7C RID: 28284
	[Header("ToolTip")]
	[SerializeField]
	private TextStyleSetting TooltipHeader;

	// Token: 0x04006E7D RID: 28285
	[SerializeField]
	private TextStyleSetting TooltipDescription;

	// Token: 0x04006E7E RID: 28286
	[Header("Logic")]
	[SerializeField]
	private LogicModeUI logicModeUIPrefab;

	// Token: 0x04006E7F RID: 28287
	public Action<HashedString> OnOverlayChanged;

	// Token: 0x04006E80 RID: 28288
	private OverlayScreen.ModeInfo currentModeInfo;

	// Token: 0x04006E81 RID: 28289
	private Dictionary<HashedString, OverlayScreen.ModeInfo> modeInfos = new Dictionary<HashedString, OverlayScreen.ModeInfo>();

	// Token: 0x02001B32 RID: 6962
	private struct ModeInfo
	{
		// Token: 0x04006E82 RID: 28290
		public OverlayModes.Mode mode;
	}
}
