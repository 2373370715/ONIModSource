using System;
using System.Collections.Generic;
using System.IO;
using FMOD.Studio;
using Klei;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001E97 RID: 7831
public class PauseScreen : KModalButtonMenu
{
	// Token: 0x17000A8F RID: 2703
	// (get) Token: 0x0600A430 RID: 42032 RVA: 0x0010A87E File Offset: 0x00108A7E
	public static PauseScreen Instance
	{
		get
		{
			return PauseScreen.instance;
		}
	}

	// Token: 0x0600A431 RID: 42033 RVA: 0x0010A885 File Offset: 0x00108A85
	public static void DestroyInstance()
	{
		PauseScreen.instance = null;
	}

	// Token: 0x0600A432 RID: 42034 RVA: 0x0010A88D File Offset: 0x00108A8D
	protected override void OnPrefabInit()
	{
		this.keepMenuOpen = true;
		base.OnPrefabInit();
		this.ConfigureButtonInfos();
		this.closeButton.onClick += this.OnResume;
		PauseScreen.instance = this;
		this.Show(false);
	}

	// Token: 0x0600A433 RID: 42035 RVA: 0x003E547C File Offset: 0x003E367C
	private void ConfigureButtonInfos()
	{
		if (!GenericGameSettings.instance.demoMode)
		{
			this.buttons = new KButtonMenu.ButtonInfo[]
			{
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.RESUME, global::Action.NumActions, new UnityAction(this.OnResume), null, null),
				new KButtonMenu.ButtonInfo(this.recentlySaved ? UI.FRONTEND.PAUSE_SCREEN.ALREADY_SAVED : UI.FRONTEND.PAUSE_SCREEN.SAVE, global::Action.NumActions, new UnityAction(this.OnSave), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.SAVEAS, global::Action.NumActions, new UnityAction(this.OnSaveAs), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.LOAD, global::Action.NumActions, new UnityAction(this.OnLoad), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.OPTIONS, global::Action.NumActions, new UnityAction(this.OnOptions), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.COLONY_SUMMARY, global::Action.NumActions, new UnityAction(this.OnColonySummary), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.LOCKERMENU, global::Action.NumActions, new UnityAction(this.OnLockerMenu), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.QUIT, global::Action.NumActions, new UnityAction(this.OnQuit), null, null),
				new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.DESKTOPQUIT, global::Action.NumActions, new UnityAction(this.OnDesktopQuit), null, null)
			};
			return;
		}
		this.buttons = new KButtonMenu.ButtonInfo[]
		{
			new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.RESUME, global::Action.NumActions, new UnityAction(this.OnResume), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.OPTIONS, global::Action.NumActions, new UnityAction(this.OnOptions), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.QUIT, global::Action.NumActions, new UnityAction(this.OnQuit), null, null),
			new KButtonMenu.ButtonInfo(UI.FRONTEND.PAUSE_SCREEN.DESKTOPQUIT, global::Action.NumActions, new UnityAction(this.OnDesktopQuit), null, null)
		};
	}

	// Token: 0x0600A434 RID: 42036 RVA: 0x003E56A4 File Offset: 0x003E38A4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.clipboard.GetText = new Func<string>(this.GetClipboardText);
		this.title.SetText(UI.FRONTEND.PAUSE_SCREEN.TITLE);
		try
		{
			string settingsCoordinate = CustomGameSettings.Instance.GetSettingsCoordinate();
			string[] array = CustomGameSettings.ParseSettingCoordinate(settingsCoordinate);
			this.worldSeed.SetText(string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED, settingsCoordinate));
			this.worldSeed.GetComponent<ToolTip>().toolTip = string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED_TOOLTIP, new object[]
			{
				array[1],
				array[2],
				array[3],
				array[4],
				array[5]
			});
		}
		catch (Exception arg)
		{
			global::Debug.LogWarning(string.Format("Failed to load Coordinates on ClusterLayout {0}, please report this error on the forums", arg));
			CustomGameSettings.Instance.Print();
			global::Debug.Log("ClusterCache: " + string.Join(",", SettingsCache.clusterLayouts.clusterCache.Keys));
			this.worldSeed.SetText(string.Format(UI.FRONTEND.PAUSE_SCREEN.WORLD_SEED, "0"));
		}
	}

	// Token: 0x0600A435 RID: 42037 RVA: 0x0010A8C6 File Offset: 0x00108AC6
	public override float GetSortKey()
	{
		return 30f;
	}

	// Token: 0x0600A436 RID: 42038 RVA: 0x003E57CC File Offset: 0x003E39CC
	private string GetClipboardText()
	{
		string result;
		try
		{
			result = CustomGameSettings.Instance.GetSettingsCoordinate();
		}
		catch
		{
			result = "";
		}
		return result;
	}

	// Token: 0x0600A437 RID: 42039 RVA: 0x000FE04E File Offset: 0x000FC24E
	private void OnResume()
	{
		this.Show(false);
	}

	// Token: 0x0600A438 RID: 42040 RVA: 0x003E5800 File Offset: 0x003E3A00
	protected override void OnShow(bool show)
	{
		this.recentlySaved = false;
		this.ConfigureButtonInfos();
		base.OnShow(show);
		if (show)
		{
			this.RefreshButtons();
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().ESCPauseSnapshot);
			MusicManager.instance.OnEscapeMenu(true);
			MusicManager.instance.PlaySong("Music_ESC_Menu", false);
			this.RefreshDLCActivationButtons();
			return;
		}
		ToolTipScreen.Instance.ClearToolTip(this.closeButton.GetComponent<ToolTip>());
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().ESCPauseSnapshot, STOP_MODE.ALLOWFADEOUT);
		MusicManager.instance.OnEscapeMenu(false);
		if (MusicManager.instance.SongIsPlaying("Music_ESC_Menu"))
		{
			MusicManager.instance.StopSong("Music_ESC_Menu", true, STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x0600A439 RID: 42041 RVA: 0x0010A8CD File Offset: 0x00108ACD
	private void OnOptions()
	{
		base.ActivateChildScreen(this.optionsScreen.gameObject);
	}

	// Token: 0x0600A43A RID: 42042 RVA: 0x0010A8E1 File Offset: 0x00108AE1
	private void OnSaveAs()
	{
		base.ActivateChildScreen(this.saveScreenPrefab.gameObject);
	}

	// Token: 0x0600A43B RID: 42043 RVA: 0x003E58BC File Offset: 0x003E3ABC
	private void OnSave()
	{
		string filename = SaveLoader.GetActiveSaveFilePath();
		if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
		{
			base.gameObject.SetActive(false);
			((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format(UI.FRONTEND.SAVESCREEN.OVERWRITEMESSAGE, System.IO.Path.GetFileNameWithoutExtension(filename)), delegate
			{
				this.DoSave(filename);
				this.gameObject.SetActive(true);
			}, new System.Action(this.OnCancelPopup), null, null, null, null, null, null);
			return;
		}
		this.OnSaveAs();
	}

	// Token: 0x0600A43C RID: 42044 RVA: 0x0010A8F5 File Offset: 0x00108AF5
	public void OnSaveComplete()
	{
		this.recentlySaved = true;
		this.ConfigureButtonInfos();
		this.RefreshButtons();
	}

	// Token: 0x0600A43D RID: 42045 RVA: 0x003E5980 File Offset: 0x003E3B80
	private void DoSave(string filename)
	{
		try
		{
			SaveLoader.Instance.Save(filename, false, true);
			this.OnSaveComplete();
		}
		catch (IOException ex)
		{
			IOException e2 = ex;
			IOException e = e2;
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(string.Format(UI.FRONTEND.SAVESCREEN.IO_ERROR, e.ToString()), delegate
			{
				this.Deactivate();
			}, null, UI.FRONTEND.SAVESCREEN.REPORT_BUG, delegate
			{
				KCrashReporter.ReportError(e.Message, e.StackTrace.ToString(), null, null, null, true, new string[]
				{
					KCrashReporter.CRASH_CATEGORY.FILEIO
				}, null);
			}, null, null, null, null);
		}
	}

	// Token: 0x0600A43E RID: 42046 RVA: 0x003E5A38 File Offset: 0x003E3C38
	private void ConfirmDecision(string questionText, string primaryButtonText, System.Action primaryButtonAction, string alternateButtonText = null, System.Action alternateButtonAction = null)
	{
		base.gameObject.SetActive(false);
		((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.transform.parent.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(questionText, primaryButtonAction, new System.Action(this.OnCancelPopup), alternateButtonText, alternateButtonAction, null, primaryButtonText, null, null);
	}

	// Token: 0x0600A43F RID: 42047 RVA: 0x0010A90A File Offset: 0x00108B0A
	private void OnLoad()
	{
		base.ActivateChildScreen(this.loadScreenPrefab.gameObject);
	}

	// Token: 0x0600A440 RID: 42048 RVA: 0x003E5A9C File Offset: 0x003E3C9C
	private void OnColonySummary()
	{
		RetiredColonyData currentColonyRetiredColonyData = RetireColonyUtility.GetCurrentColonyRetiredColonyData();
		MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, currentColonyRetiredColonyData);
	}

	// Token: 0x0600A441 RID: 42049 RVA: 0x00106D4A File Offset: 0x00104F4A
	private void OnLockerMenu()
	{
		LockerMenuScreen.Instance.Show(true);
	}

	// Token: 0x0600A442 RID: 42050 RVA: 0x0010A91E File Offset: 0x00108B1E
	private void OnQuit()
	{
		this.ConfirmDecision(UI.FRONTEND.MAINMENU.QUITCONFIRM, UI.FRONTEND.MAINMENU.SAVEANDQUITTITLE, delegate
		{
			this.OnQuitConfirm(true);
		}, UI.FRONTEND.MAINMENU.QUIT, delegate
		{
			this.OnQuitConfirm(false);
		});
	}

	// Token: 0x0600A443 RID: 42051 RVA: 0x0010A95C File Offset: 0x00108B5C
	private void OnDesktopQuit()
	{
		this.ConfirmDecision(UI.FRONTEND.MAINMENU.DESKTOPQUITCONFIRM, UI.FRONTEND.MAINMENU.SAVEANDQUITDESKTOP, delegate
		{
			this.OnDesktopQuitConfirm(true);
		}, UI.FRONTEND.MAINMENU.QUIT, delegate
		{
			this.OnDesktopQuitConfirm(false);
		});
	}

	// Token: 0x0600A444 RID: 42052 RVA: 0x00100250 File Offset: 0x000FE450
	private void OnCancelPopup()
	{
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600A445 RID: 42053 RVA: 0x0010A99A File Offset: 0x00108B9A
	private void OnLoadConfirm()
	{
		LoadingOverlay.Load(delegate
		{
			LoadScreen.ForceStopGame();
			this.Deactivate();
			App.LoadScene("frontend");
		});
	}

	// Token: 0x0600A446 RID: 42054 RVA: 0x0010A9AD File Offset: 0x00108BAD
	private void OnRetireConfirm()
	{
		RetireColonyUtility.SaveColonySummaryData();
	}

	// Token: 0x0600A447 RID: 42055 RVA: 0x003E5ACC File Offset: 0x003E3CCC
	private void OnQuitConfirm(bool saveFirst)
	{
		if (saveFirst)
		{
			string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
			if (!string.IsNullOrEmpty(activeSaveFilePath) && File.Exists(activeSaveFilePath))
			{
				this.DoSave(activeSaveFilePath);
			}
			else
			{
				this.OnSaveAs();
			}
		}
		LoadingOverlay.Load(delegate
		{
			this.Deactivate();
			PauseScreen.TriggerQuitGame();
		});
	}

	// Token: 0x0600A448 RID: 42056 RVA: 0x003E5B14 File Offset: 0x003E3D14
	private void OnDesktopQuitConfirm(bool saveFirst)
	{
		if (saveFirst)
		{
			string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
			if (!string.IsNullOrEmpty(activeSaveFilePath) && File.Exists(activeSaveFilePath))
			{
				this.DoSave(activeSaveFilePath);
			}
			else
			{
				this.OnSaveAs();
			}
		}
		App.Quit();
	}

	// Token: 0x0600A449 RID: 42057 RVA: 0x0010A9B5 File Offset: 0x00108BB5
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600A44A RID: 42058 RVA: 0x0010A9D8 File Offset: 0x00108BD8
	public static void TriggerQuitGame()
	{
		ThreadedHttps<KleiMetrics>.Instance.EndGame();
		LoadScreen.ForceStopGame();
		App.LoadScene("frontend");
	}

	// Token: 0x0600A44B RID: 42059 RVA: 0x003E5B50 File Offset: 0x003E3D50
	private void RefreshDLCActivationButtons()
	{
		foreach (KeyValuePair<string, DlcManager.DlcInfo> keyValuePair in DlcManager.DLC_PACKS)
		{
			if (!(keyValuePair.Value.id == "DLC3_ID") && !this.dlcActivationButtons.ContainsKey(keyValuePair.Key))
			{
				GameObject gameObject = global::Util.KInstantiateUI(this.dlcActivationButtonPrefab, this.dlcActivationButtonPrefab.transform.parent.gameObject, true);
				Sprite sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(keyValuePair.Key));
				gameObject.GetComponent<Image>().sprite = sprite;
				gameObject.GetComponent<MultiToggle>().states[0].sprite = sprite;
				gameObject.GetComponent<MultiToggle>().states[1].sprite = sprite;
				this.dlcActivationButtons.Add(keyValuePair.Key, gameObject);
			}
		}
		this.RefreshDLCButton("EXPANSION1_ID", this.dlc1ActivationButton, false);
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.dlcActivationButtons)
		{
			this.RefreshDLCButton(keyValuePair2.Key, keyValuePair2.Value.GetComponent<MultiToggle>(), true);
		}
	}

	// Token: 0x0600A44C RID: 42060 RVA: 0x003E5CC4 File Offset: 0x003E3EC4
	private void RefreshDLCButton(string DLCID, MultiToggle button, bool userEditable)
	{
		button.ChangeState(SaveLoader.Instance.IsDLCActiveForCurrentSave(DLCID) ? 1 : 0);
		button.GetComponent<Image>().material = (SaveLoader.Instance.IsDLCActiveForCurrentSave(DLCID) ? GlobalResources.Instance().AnimUIMaterial : GlobalResources.Instance().AnimMaterialUIDesaturated);
		ToolTip component = button.GetComponent<ToolTip>();
		string dlcTitle = DlcManager.GetDlcTitle(DLCID);
		if (!DlcManager.IsContentSubscribed(DLCID))
		{
			component.SetSimpleTooltip(string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_DISABLED_NOT_EDITABLE_TOOLTIP, dlcTitle));
			button.onClick = null;
			return;
		}
		if (userEditable)
		{
			component.SetSimpleTooltip(SaveLoader.Instance.IsDLCActiveForCurrentSave(DLCID) ? string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_ENABLED_TOOLTIP, dlcTitle) : string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_DISABLED_TOOLTIP, dlcTitle));
			button.onClick = delegate()
			{
				this.OnClickAddDLCButton(DLCID);
			};
			return;
		}
		component.SetSimpleTooltip(SaveLoader.Instance.IsDLCActiveForCurrentSave(DLCID) ? string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_ENABLED_TOOLTIP, dlcTitle) : string.Format(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.DLC_DISABLED_NOT_EDITABLE_TOOLTIP, dlcTitle));
		button.onClick = null;
	}

	// Token: 0x0600A44D RID: 42061 RVA: 0x003E5E08 File Offset: 0x003E4008
	private void OnClickAddDLCButton(string dlcID)
	{
		if (!SaveLoader.Instance.IsDLCActiveForCurrentSave(dlcID))
		{
			this.ConfirmDecision(UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.ENABLE_QUESTION, UI.FRONTEND.PAUSE_SCREEN.ADD_DLC_MENU.CONFIRM, delegate
			{
				this.OnConfirmAddDLC(dlcID);
			}, null, null);
		}
	}

	// Token: 0x0600A44E RID: 42062 RVA: 0x0010A9F3 File Offset: 0x00108BF3
	private void OnConfirmAddDLC(string dlcId)
	{
		SaveLoader.Instance.UpgradeActiveSaveDLCInfo(dlcId, true);
	}

	// Token: 0x04008059 RID: 32857
	[SerializeField]
	private OptionsMenuScreen optionsScreen;

	// Token: 0x0400805A RID: 32858
	[SerializeField]
	private SaveScreen saveScreenPrefab;

	// Token: 0x0400805B RID: 32859
	[SerializeField]
	private LoadScreen loadScreenPrefab;

	// Token: 0x0400805C RID: 32860
	[SerializeField]
	private KButton closeButton;

	// Token: 0x0400805D RID: 32861
	[SerializeField]
	private LocText title;

	// Token: 0x0400805E RID: 32862
	[SerializeField]
	private LocText worldSeed;

	// Token: 0x0400805F RID: 32863
	[SerializeField]
	private CopyTextFieldToClipboard clipboard;

	// Token: 0x04008060 RID: 32864
	[SerializeField]
	private MultiToggle dlc1ActivationButton;

	// Token: 0x04008061 RID: 32865
	[SerializeField]
	private GameObject dlcActivationButtonPrefab;

	// Token: 0x04008062 RID: 32866
	private Dictionary<string, GameObject> dlcActivationButtons = new Dictionary<string, GameObject>();

	// Token: 0x04008063 RID: 32867
	private float originalTimeScale;

	// Token: 0x04008064 RID: 32868
	private bool recentlySaved;

	// Token: 0x04008065 RID: 32869
	private static PauseScreen instance;
}
