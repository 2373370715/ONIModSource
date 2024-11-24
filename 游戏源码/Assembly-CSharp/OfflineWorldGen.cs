using System;
using System.Collections.Generic;
using System.Threading;
using Klei.CustomSettings;
using ProcGenGame;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x02002074 RID: 8308
[AddComponentMenu("KMonoBehaviour/scripts/OfflineWorldGen")]
public class OfflineWorldGen : KMonoBehaviour
{
	// Token: 0x0600B0C8 RID: 45256 RVA: 0x00112E55 File Offset: 0x00111055
	private void TrackProgress(string text)
	{
		if (this.trackProgress)
		{
			global::Debug.Log(text);
		}
	}

	// Token: 0x0600B0C9 RID: 45257 RVA: 0x00426DA8 File Offset: 0x00424FA8
	public static bool CanLoadSave()
	{
		bool flag = WorldGen.CanLoad(SaveLoader.GetActiveSaveFilePath());
		if (!flag)
		{
			SaveLoader.SetActiveSaveFilePath(null);
			flag = WorldGen.CanLoad(WorldGen.WORLDGEN_SAVE_FILENAME);
		}
		return flag;
	}

	// Token: 0x0600B0CA RID: 45258 RVA: 0x00426DD8 File Offset: 0x00424FD8
	public void Generate()
	{
		this.doWorldGen = !OfflineWorldGen.CanLoadSave();
		this.updateText.gameObject.SetActive(false);
		this.percentText.gameObject.SetActive(false);
		this.doWorldGen |= this.debug;
		if (this.doWorldGen)
		{
			this.seedText.text = string.Format(UI.WORLDGEN.USING_PLAYER_SEED, this.seed);
			this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.TITLE.ToString();
			this.mainText.text = UI.WORLDGEN.CHOOSEWORLDSIZE.ToString();
			for (int i = 0; i < this.validDimensions.Length; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
				gameObject.SetActive(true);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.SetParent(this.buttonRoot);
				component.localScale = Vector3.one;
				TMP_Text componentInChildren = gameObject.GetComponentInChildren<LocText>();
				OfflineWorldGen.ValidDimensions validDimensions = this.validDimensions[i];
				componentInChildren.text = validDimensions.name.ToString();
				int idx = i;
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.DoWorldGen(idx);
					this.ToggleGenerationUI();
				};
			}
			if (this.validDimensions.Length == 1)
			{
				this.DoWorldGen(0);
				this.ToggleGenerationUI();
			}
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
			this.OnResize();
		}
		else
		{
			this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.LOADINGGAME.ToString();
			this.mainText.gameObject.SetActive(false);
			this.currentConvertedCurrentStage = UI.WORLDGEN.COMPLETE.key;
			this.currentPercent = 1f;
			this.updateText.gameObject.SetActive(false);
			this.percentText.gameObject.SetActive(false);
			this.RemoveButtons();
		}
		this.buttonPrefab.SetActive(false);
	}

	// Token: 0x0600B0CB RID: 45259 RVA: 0x00426FD8 File Offset: 0x004251D8
	private void OnResize()
	{
		float canvasScale = base.GetComponentInParent<KCanvasScaler>().GetCanvasScale();
		if (this.asteriodAnim != null)
		{
			this.asteriodAnim.animScale = 0.005f * (1f / canvasScale);
		}
	}

	// Token: 0x0600B0CC RID: 45260 RVA: 0x00427020 File Offset: 0x00425220
	private void ToggleGenerationUI()
	{
		this.percentText.gameObject.SetActive(false);
		this.updateText.gameObject.SetActive(true);
		this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.GENERATINGWORLD.ToString();
		if (this.titleText != null && this.titleText.gameObject != null)
		{
			this.titleText.gameObject.SetActive(false);
		}
		if (this.buttonRoot != null && this.buttonRoot.gameObject != null)
		{
			this.buttonRoot.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600B0CD RID: 45261 RVA: 0x004270C8 File Offset: 0x004252C8
	private bool UpdateProgress(StringKey stringKeyRoot, float completePercent, WorldGenProgressStages.Stages stage)
	{
		if (this.currentStage != stage)
		{
			this.currentStage = stage;
		}
		if (this.currentStringKeyRoot.Hash != stringKeyRoot.Hash)
		{
			this.currentConvertedCurrentStage = stringKeyRoot;
			this.currentStringKeyRoot = stringKeyRoot;
		}
		else
		{
			int num = (int)completePercent * 10;
			LocString locString = this.convertList.Find((LocString s) => s.key.Hash == stringKeyRoot.Hash);
			if (num != 0 && locString != null)
			{
				this.currentConvertedCurrentStage = new StringKey(locString.key.String + num.ToString());
			}
		}
		float num2 = 0f;
		float num3 = 0f;
		float num4 = WorldGenProgressStages.StageWeights[(int)stage].Value * completePercent;
		for (int i = 0; i < WorldGenProgressStages.StageWeights.Length; i++)
		{
			num3 += WorldGenProgressStages.StageWeights[i].Value;
			if (i < (int)this.currentStage)
			{
				num2 += WorldGenProgressStages.StageWeights[i].Value;
			}
		}
		float num5 = (num2 + num4) / num3;
		this.currentPercent = num5;
		return !this.shouldStop;
	}

	// Token: 0x0600B0CE RID: 45262 RVA: 0x004271F0 File Offset: 0x004253F0
	private void Update()
	{
		if (this.loadTriggered)
		{
			return;
		}
		if (this.currentConvertedCurrentStage.String == null)
		{
			return;
		}
		this.errorMutex.WaitOne();
		int count = this.errors.Count;
		this.errorMutex.ReleaseMutex();
		if (count > 0)
		{
			this.DoExitFlow();
			return;
		}
		this.updateText.text = Strings.Get(this.currentConvertedCurrentStage.String);
		if (!this.debug && this.currentConvertedCurrentStage.Hash == UI.WORLDGEN.COMPLETE.key.Hash && this.currentPercent >= 1f && this.cluster.IsGenerationComplete)
		{
			if (KCrashReporter.terminateOnError && KCrashReporter.hasCrash)
			{
				return;
			}
			this.percentText.text = "";
			this.loadTriggered = true;
			App.LoadScene(this.mainGameLevel);
			return;
		}
		else
		{
			if (this.currentPercent < 0f)
			{
				this.DoExitFlow();
				return;
			}
			if (this.currentPercent > 0f && !this.percentText.gameObject.activeSelf)
			{
				this.percentText.gameObject.SetActive(false);
			}
			this.percentText.text = GameUtil.GetFormattedPercent(this.currentPercent * 100f, GameUtil.TimeSlice.None);
			this.meterAnim.SetPositionPercent(this.currentPercent);
			return;
		}
	}

	// Token: 0x0600B0CF RID: 45263 RVA: 0x00427344 File Offset: 0x00425544
	private void DisplayErrors()
	{
		this.errorMutex.WaitOne();
		if (this.errors.Count > 0)
		{
			foreach (OfflineWorldGen.ErrorInfo errorInfo in this.errors)
			{
				Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, FrontEndManager.Instance.gameObject, true).PopupConfirmDialog(errorInfo.errorDesc, new System.Action(this.OnConfirmExit), null, null, null, null, null, null, null);
			}
		}
		this.errorMutex.ReleaseMutex();
	}

	// Token: 0x0600B0D0 RID: 45264 RVA: 0x00112E65 File Offset: 0x00111065
	private void DoExitFlow()
	{
		if (this.startedExitFlow)
		{
			return;
		}
		this.startedExitFlow = true;
		this.percentText.text = UI.WORLDGEN.RESTARTING.ToString();
		this.loadTriggered = true;
		Sim.Shutdown();
		this.DisplayErrors();
	}

	// Token: 0x0600B0D1 RID: 45265 RVA: 0x00112E9E File Offset: 0x0011109E
	private void OnConfirmExit()
	{
		App.LoadScene(this.frontendGameLevel);
	}

	// Token: 0x0600B0D2 RID: 45266 RVA: 0x004273F4 File Offset: 0x004255F4
	private void RemoveButtons()
	{
		for (int i = this.buttonRoot.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.buttonRoot.GetChild(i).gameObject);
		}
	}

	// Token: 0x0600B0D3 RID: 45267 RVA: 0x00112EAB File Offset: 0x001110AB
	private void DoWorldGen(int selectedDimension)
	{
		this.RemoveButtons();
		this.DoWorldGenInitialize();
	}

	// Token: 0x0600B0D4 RID: 45268 RVA: 0x00427430 File Offset: 0x00425630
	private void DoWorldGenInitialize()
	{
		string clusterName = "";
		Func<int, WorldGen, bool> shouldSkipWorldCallback = null;
		this.seed = CustomGameSettings.Instance.GetCurrentWorldgenSeed();
		clusterName = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout).id;
		List<string> list = new List<string>();
		foreach (string id in CustomGameSettings.Instance.GetCurrentStories())
		{
			list.Add(Db.Get().Stories.Get(id).worldgenStoryTraitKey);
		}
		this.cluster = new Cluster(clusterName, this.seed, list, true, false, false);
		this.cluster.ShouldSkipWorldCallback = shouldSkipWorldCallback;
		this.cluster.Generate(new WorldGen.OfflineCallbackFunction(this.UpdateProgress), new Action<OfflineWorldGen.ErrorInfo>(this.OnError), this.seed, this.seed, this.seed, this.seed, true, false, false);
	}

	// Token: 0x0600B0D5 RID: 45269 RVA: 0x00112EB9 File Offset: 0x001110B9
	private void OnError(OfflineWorldGen.ErrorInfo error)
	{
		this.errorMutex.WaitOne();
		this.errors.Add(error);
		this.errorMutex.ReleaseMutex();
	}

	// Token: 0x04008BA8 RID: 35752
	[SerializeField]
	private RectTransform buttonRoot;

	// Token: 0x04008BA9 RID: 35753
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x04008BAA RID: 35754
	[SerializeField]
	private RectTransform chooseLocationPanel;

	// Token: 0x04008BAB RID: 35755
	[SerializeField]
	private GameObject locationButtonPrefab;

	// Token: 0x04008BAC RID: 35756
	private const float baseScale = 0.005f;

	// Token: 0x04008BAD RID: 35757
	private Mutex errorMutex = new Mutex();

	// Token: 0x04008BAE RID: 35758
	private List<OfflineWorldGen.ErrorInfo> errors = new List<OfflineWorldGen.ErrorInfo>();

	// Token: 0x04008BAF RID: 35759
	private OfflineWorldGen.ValidDimensions[] validDimensions = new OfflineWorldGen.ValidDimensions[]
	{
		new OfflineWorldGen.ValidDimensions
		{
			width = 256,
			height = 384,
			name = UI.FRONTEND.WORLDGENSCREEN.SIZES.STANDARD.key
		}
	};

	// Token: 0x04008BB0 RID: 35760
	public string frontendGameLevel = "frontend";

	// Token: 0x04008BB1 RID: 35761
	public string mainGameLevel = "backend";

	// Token: 0x04008BB2 RID: 35762
	private bool shouldStop;

	// Token: 0x04008BB3 RID: 35763
	private StringKey currentConvertedCurrentStage;

	// Token: 0x04008BB4 RID: 35764
	private float currentPercent;

	// Token: 0x04008BB5 RID: 35765
	public bool debug;

	// Token: 0x04008BB6 RID: 35766
	private bool trackProgress = true;

	// Token: 0x04008BB7 RID: 35767
	private bool doWorldGen;

	// Token: 0x04008BB8 RID: 35768
	[SerializeField]
	private LocText titleText;

	// Token: 0x04008BB9 RID: 35769
	[SerializeField]
	private LocText mainText;

	// Token: 0x04008BBA RID: 35770
	[SerializeField]
	private LocText updateText;

	// Token: 0x04008BBB RID: 35771
	[SerializeField]
	private LocText percentText;

	// Token: 0x04008BBC RID: 35772
	[SerializeField]
	private LocText seedText;

	// Token: 0x04008BBD RID: 35773
	[SerializeField]
	private KBatchedAnimController meterAnim;

	// Token: 0x04008BBE RID: 35774
	[SerializeField]
	private KBatchedAnimController asteriodAnim;

	// Token: 0x04008BBF RID: 35775
	private Cluster cluster;

	// Token: 0x04008BC0 RID: 35776
	private StringKey currentStringKeyRoot;

	// Token: 0x04008BC1 RID: 35777
	private List<LocString> convertList = new List<LocString>
	{
		UI.WORLDGEN.SETTLESIM,
		UI.WORLDGEN.BORDERS,
		UI.WORLDGEN.PROCESSING,
		UI.WORLDGEN.COMPLETELAYOUT,
		UI.WORLDGEN.WORLDLAYOUT,
		UI.WORLDGEN.GENERATENOISE,
		UI.WORLDGEN.BUILDNOISESOURCE,
		UI.WORLDGEN.GENERATESOLARSYSTEM
	};

	// Token: 0x04008BC2 RID: 35778
	private WorldGenProgressStages.Stages currentStage;

	// Token: 0x04008BC3 RID: 35779
	private bool loadTriggered;

	// Token: 0x04008BC4 RID: 35780
	private bool startedExitFlow;

	// Token: 0x04008BC5 RID: 35781
	private int seed;

	// Token: 0x02002075 RID: 8309
	public struct ErrorInfo
	{
		// Token: 0x04008BC6 RID: 35782
		public string errorDesc;

		// Token: 0x04008BC7 RID: 35783
		public Exception exception;
	}

	// Token: 0x02002076 RID: 8310
	[Serializable]
	private struct ValidDimensions
	{
		// Token: 0x04008BC8 RID: 35784
		public int width;

		// Token: 0x04008BC9 RID: 35785
		public int height;

		// Token: 0x04008BCA RID: 35786
		public StringKey name;
	}
}
