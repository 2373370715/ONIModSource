using System;
using System.Collections.Generic;
using System.IO;
using FMOD.Studio;
using Klei;
using ProcGenGame;
using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : KScreen
{
	private struct ButtonInfo
	{
		public LocString text;

		public System.Action action;

		public int fontSize;

		public ColorStyleSetting style;

		public ButtonInfo(LocString text, System.Action action, int font_size, ColorStyleSetting style)
		{
			this.text = text;
			this.action = action;
			fontSize = font_size;
			this.style = style;
		}
	}

	private struct SaveFileEntry
	{
		public System.DateTime timeStamp;

		public SaveGame.Header header;

		public SaveGame.GameInfo headerData;
	}

	private static MainMenu _instance;

	public KButton Button_ResumeGame;

	private KButton Button_NewGame;

	private GameObject GameSettingsScreen;

	private bool m_screenshotMode;

	[SerializeField]
	private CanvasGroup uiCanvas;

	[SerializeField]
	private KButton buttonPrefab;

	[SerializeField]
	private GameObject buttonParent;

	[SerializeField]
	private ColorStyleSetting topButtonStyle;

	[SerializeField]
	private ColorStyleSetting normalButtonStyle;

	[SerializeField]
	private string menuMusicEventName;

	[SerializeField]
	private string ambientLoopEventName;

	private EventInstance ambientLoop;

	[SerializeField]
	private MainMenu_Motd motd;

	[SerializeField]
	private PatchNotesScreen patchNotesScreenPrefab;

	[SerializeField]
	private NextUpdateTimer nextUpdateTimer;

	[SerializeField]
	private DLCToggle expansion1Toggle;

	[SerializeField]
	private GameObject expansion1Ad;

	[SerializeField]
	private BuildWatermark buildWatermark;

	[SerializeField]
	public string IntroShortName;

	[SerializeField]
	private HierarchyReferences logoDLC1;

	[SerializeField]
	private HierarchyReferences logoDLC2;

	private KButton lockerButton;

	private bool itemDropOpenFlag;

	private static bool HasAutoresumedOnce = false;

	private bool refreshResumeButton = true;

	private int m_cheatInputCounter;

	public const string AutoResumeSaveFileKey = "AutoResumeSaveFile";

	public const string PLAY_SHORT_ON_LAUNCH = "PlayShortOnLaunch";

	private static int LANGUAGE_CONFIRMATION_VERSION = 2;

	private Dictionary<string, SaveFileEntry> saveFileEntries = new Dictionary<string, SaveFileEntry>();

	public static MainMenu Instance => _instance;

	private KButton MakeButton(ButtonInfo info)
	{
		KButton kButton = Util.KInstantiateUI<KButton>(buttonPrefab.gameObject, buttonParent, force_active: true);
		kButton.onClick += info.action;
		KImage component = kButton.GetComponent<KImage>();
		component.colorStyleSetting = info.style;
		component.ApplyColorStyleSetting();
		LocText componentInChildren = kButton.GetComponentInChildren<LocText>();
		componentInChildren.text = info.text;
		componentInChildren.fontSize = info.fontSize;
		return kButton;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		_instance = this;
		Button_NewGame = MakeButton(new ButtonInfo(UI.FRONTEND.MAINMENU.NEWGAME, NewGame, 22, topButtonStyle));
		MakeButton(new ButtonInfo(UI.FRONTEND.MAINMENU.LOADGAME, LoadGame, 22, normalButtonStyle));
		MakeButton(new ButtonInfo(UI.FRONTEND.MAINMENU.RETIREDCOLONIES, delegate
		{
			ActivateRetiredColoniesScreen(base.transform.gameObject);
		}, 14, normalButtonStyle));
		lockerButton = MakeButton(new ButtonInfo(UI.FRONTEND.MAINMENU.LOCKERMENU, delegate
		{
			ActivateLockerMenu();
		}, 14, normalButtonStyle));
		if (DistributionPlatform.Initialized)
		{
			MakeButton(new ButtonInfo(UI.FRONTEND.MAINMENU.TRANSLATIONS, Translations, 14, normalButtonStyle));
			MakeButton(new ButtonInfo(UI.FRONTEND.MODS.TITLE, Mods, 14, normalButtonStyle));
		}
		MakeButton(new ButtonInfo(UI.FRONTEND.MAINMENU.OPTIONS, Options, 14, normalButtonStyle));
		MakeButton(new ButtonInfo(UI.FRONTEND.MAINMENU.QUITTODESKTOP, QuitGame, 14, normalButtonStyle));
		RefreshResumeButton();
		Button_ResumeGame.onClick += ResumeGame;
		SpawnVideoScreen();
		StartFEAudio();
		CheckPlayerPrefsCorruption();
		if (PatchNotesScreen.ShouldShowScreen())
		{
			Util.KInstantiateUI(patchNotesScreenPrefab.gameObject, FrontEndManager.Instance.gameObject, force_active: true);
		}
		CheckDoubleBoundKeys();
		bool flag = DistributionPlatform.Inst.IsDLCPurchased("EXPANSION1_ID");
		expansion1Toggle.gameObject.SetActive(flag);
		if (expansion1Ad != null)
		{
			expansion1Ad.gameObject.SetActive(!flag);
		}
		RefreshDLCLogos();
		motd.Setup();
		if (DistributionPlatform.Initialized && DistributionPlatform.Inst.IsPreviousVersionBranch)
		{
			UnityEngine.Object.Instantiate(ScreenPrefabs.Instance.OldVersionWarningScreen, uiCanvas.transform);
		}
		string targetExpansion1AdURL = "";
		Sprite sprite = Assets.GetSprite("expansionPromo_en");
		if (DistributionPlatform.Initialized && expansion1Ad != null)
		{
			switch (DistributionPlatform.Inst.Name)
			{
			case "Steam":
				targetExpansion1AdURL = "https://store.steampowered.com/app/1452490/Oxygen_Not_Included__Spaced_Out/";
				break;
			case "Epic":
				targetExpansion1AdURL = "https://store.epicgames.com/en-US/p/oxygen-not-included--spaced-out";
				break;
			case "Rail":
				targetExpansion1AdURL = "https://www.wegame.com.cn/store/2001539/";
				sprite = Assets.GetSprite("expansionPromo_cn");
				break;
			}
			expansion1Ad.GetComponentInChildren<KButton>().onClick += delegate
			{
				App.OpenWebURL(targetExpansion1AdURL);
			};
			expansion1Ad.GetComponent<HierarchyReferences>().GetReference<Image>("Image").sprite = sprite;
		}
		activateOnSpawn = true;
	}

	private void RefreshDLCLogos()
	{
		logoDLC1.GetReference<Image>("icon").material = (DlcManager.IsContentSubscribed("EXPANSION1_ID") ? GlobalResources.Instance().AnimUIMaterial : GlobalResources.Instance().AnimMaterialUIDesaturated);
		logoDLC2.GetReference<Image>("icon").material = (DlcManager.IsContentSubscribed("DLC2_ID") ? GlobalResources.Instance().AnimUIMaterial : GlobalResources.Instance().AnimMaterialUIDesaturated);
		if (!DistributionPlatform.Initialized)
		{
			return;
		}
		string DLC1_STORE_URL = "";
		string DLC2_STORE_URL = "";
		switch (DistributionPlatform.Inst.Name)
		{
		case "Steam":
			DLC1_STORE_URL = "https://store.steampowered.com/app/1452490/Oxygen_Not_Included__Spaced_Out/";
			DLC2_STORE_URL = "https://store.steampowered.com/app/2952300/Oxygen_Not_Included_The_Frosty_Planet_Pack/";
			break;
		case "Epic":
			DLC1_STORE_URL = "https://store.epicgames.com/en-US/p/oxygen-not-included--spaced-out";
			DLC2_STORE_URL = "";
			break;
		case "Rail":
			DLC1_STORE_URL = "https://www.wegame.com.cn/store/2001539/";
			DLC2_STORE_URL = "https://www.wegame.com.cn/store/2002196/";
			logoDLC1.GetReference<Image>("icon").sprite = Assets.GetSprite("dlc1_logo_crop_cn");
			logoDLC2.GetReference<Image>("icon").sprite = Assets.GetSprite("dlc2_logo_crop_cn");
			break;
		}
		MultiToggle reference = logoDLC1.GetReference<MultiToggle>("multitoggle");
		reference.onClick = (System.Action)Delegate.Combine(reference.onClick, (System.Action)delegate
		{
			if (DlcManager.IsContentOwned("EXPANSION1_ID"))
			{
				logoDLC1.GetReference<DLCToggle>("dlctoggle").ToggleExpansion1Cicked();
			}
			else
			{
				App.OpenWebURL(DLC1_STORE_URL);
			}
		});
		string text = "";
		text = (DlcManager.IsContentOwned("EXPANSION1_ID") ? ((string)(DlcManager.IsContentSubscribed("EXPANSION1_ID") ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1_TOOLTIP : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1_TOOLTIP)) : ((string)UI.FRONTEND.MAINMENU.WISHLIST_AD_TOOLTIP));
		logoDLC1.GetReference<ToolTip>("tooltip").SetSimpleTooltip(text);
		MultiToggle reference2 = logoDLC2.GetReference<MultiToggle>("multitoggle");
		reference2.onClick = (System.Action)Delegate.Combine(reference2.onClick, (System.Action)delegate
		{
			App.OpenWebURL(DLC2_STORE_URL);
		});
		logoDLC2.GetReference<LocText>("statuslabel").SetText(GetDLCStatusString("DLC2_ID"));
		string dLCStatusString = GetDLCStatusString("DLC2_ID", tooltip: true);
		dLCStatusString = dLCStatusString + "\n\n" + UI.FRONTEND.MAINMENU.WISHLIST_AD_TOOLTIP;
		logoDLC2.GetReference<ToolTip>("tooltip").SetSimpleTooltip(dLCStatusString);
	}

	public string GetDLCStatusString(string dlcID, bool tooltip = false)
	{
		if (DlcManager.IsContentOwned(dlcID))
		{
			if (DlcManager.IsContentSubscribed(dlcID))
			{
				return tooltip ? UI.FRONTEND.MAINMENU.DLC.CONTENT_ACTIVE_TOOLTIP : UI.FRONTEND.MAINMENU.DLC.CONTENT_INSTALLED_LABEL;
			}
			return tooltip ? UI.FRONTEND.MAINMENU.DLC.CONTENT_OWNED_NOTINSTALLED_TOOLTIP : UI.FRONTEND.MAINMENU.DLC.CONTENT_OWNED_NOTINSTALLED_LABEL;
		}
		return tooltip ? UI.FRONTEND.MAINMENU.DLC.CONTENT_NOTOWNED_TOOLTIP : UI.FRONTEND.MAINMENU.WISHLIST_AD;
	}

	private void OnApplicationFocus(bool focus)
	{
		if (focus)
		{
			RefreshResumeButton();
		}
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		base.OnKeyDown(e);
		if (e.Consumed)
		{
			return;
		}
		if (e.TryConsume(Action.DebugToggleUI))
		{
			m_screenshotMode = !m_screenshotMode;
			uiCanvas.alpha = (m_screenshotMode ? 0f : 1f);
		}
		KKeyCode key_code = m_cheatInputCounter switch
		{
			0 => KKeyCode.K, 
			1 => KKeyCode.L, 
			2 => KKeyCode.E, 
			3 => KKeyCode.I, 
			4 => KKeyCode.P, 
			5 => KKeyCode.L, 
			6 => KKeyCode.A, 
			_ => KKeyCode.Y, 
		};
		if (e.Controller.GetKeyDown(key_code))
		{
			e.Consumed = true;
			m_cheatInputCounter++;
			if (m_cheatInputCounter >= 8)
			{
				Debug.Log("Cheat Detected - enabling Debug Mode");
				DebugHandler.SetDebugEnabled(debugEnabled: true);
				buildWatermark.RefreshText();
				m_cheatInputCounter = 0;
			}
		}
		else
		{
			m_cheatInputCounter = 0;
		}
	}

	private void PlayMouseOverSound()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover"));
	}

	private void PlayMouseClickSound()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open"));
	}

	protected override void OnSpawn()
	{
		Debug.Log("-- MAIN MENU -- ");
		base.OnSpawn();
		m_cheatInputCounter = 0;
		Canvas.ForceUpdateCanvases();
		ShowLanguageConfirmation();
		InitLoadScreen();
		LoadScreen.Instance.ShowMigrationIfNecessary(fromMainMenu: true);
		string savePrefix = SaveLoader.GetSavePrefix();
		try
		{
			string path = Path.Combine(savePrefix, "__SPCCHK");
			using (FileStream fileStream = File.OpenWrite(path))
			{
				byte[] array = new byte[1024];
				for (int i = 0; i < 15360; i++)
				{
					fileStream.Write(array, 0, array.Length);
				}
			}
			File.Delete(path);
		}
		catch (Exception ex)
		{
			string format = ((!(ex is IOException)) ? string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, savePrefix) : string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_INSUFFICIENT_SPACE, savePrefix));
			string text = string.Format(format, savePrefix);
			Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, force_active: true).PopupConfirmDialog(text, null, null);
		}
		Global.Instance.modManager.Report(base.gameObject);
		if ((GenericGameSettings.instance.autoResumeGame && !HasAutoresumedOnce && !KCrashReporter.hasCrash) || !string.IsNullOrEmpty(GenericGameSettings.instance.performanceCapture.saveGame) || KPlayerPrefs.HasKey("AutoResumeSaveFile"))
		{
			HasAutoresumedOnce = true;
			ResumeGame();
		}
		if (GenericGameSettings.instance.devAutoWorldGen && !KCrashReporter.hasCrash)
		{
			GenericGameSettings.instance.devAutoWorldGen = false;
			GenericGameSettings.instance.devAutoWorldGenActive = true;
			GenericGameSettings.instance.SaveSettings();
			Util.KInstantiateUI(ScreenPrefabs.Instance.WorldGenScreen.gameObject, base.gameObject, force_active: true);
		}
		RefreshInventoryNotification();
	}

	protected override void OnForcedCleanUp()
	{
		base.OnForcedCleanUp();
	}

	private void RefreshInventoryNotification()
	{
		bool active = PermitItems.HasUnopenedItem();
		lockerButton.GetComponent<HierarchyReferences>().GetReference<RectTransform>("AttentionIcon").gameObject.SetActive(active);
	}

	protected override void OnActivate()
	{
		if (!ambientLoopEventName.IsNullOrWhiteSpace())
		{
			ambientLoop = KFMOD.CreateInstance(GlobalAssets.GetSound(ambientLoopEventName));
			if (ambientLoop.isValid())
			{
				ambientLoop.start();
			}
		}
	}

	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		motd.CleanUp();
	}

	public override void ScreenUpdate(bool topLevel)
	{
		refreshResumeButton = topLevel;
		if (KleiItemDropScreen.Instance != null && KleiItemDropScreen.Instance.gameObject.activeInHierarchy != itemDropOpenFlag)
		{
			RefreshInventoryNotification();
			itemDropOpenFlag = KleiItemDropScreen.Instance.gameObject.activeInHierarchy;
		}
	}

	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
		StopAmbience();
		motd.CleanUp();
	}

	private void ShowLanguageConfirmation()
	{
		if (SteamManager.Initialized && !(SteamUtils.GetSteamUILanguage() != "schinese") && KPlayerPrefs.GetInt("LanguageConfirmationVersion") < LANGUAGE_CONFIRMATION_VERSION)
		{
			KPlayerPrefs.SetInt("LanguageConfirmationVersion", LANGUAGE_CONFIRMATION_VERSION);
			Translations();
		}
	}

	private void ResumeGame()
	{
		string text;
		if (!KPlayerPrefs.HasKey("AutoResumeSaveFile"))
		{
			text = (string.IsNullOrEmpty(GenericGameSettings.instance.performanceCapture.saveGame) ? SaveLoader.GetLatestSaveForCurrentDLC() : GenericGameSettings.instance.performanceCapture.saveGame);
		}
		else
		{
			text = KPlayerPrefs.GetString("AutoResumeSaveFile");
			KPlayerPrefs.DeleteKey("AutoResumeSaveFile");
		}
		if (!string.IsNullOrEmpty(text))
		{
			KCrashReporter.MOST_RECENT_SAVEFILE = text;
			SaveLoader.SetActiveSaveFilePath(text);
			LoadingOverlay.Load(delegate
			{
				App.LoadScene("backend");
			});
		}
	}

	private void NewGame()
	{
		WorldGen.WaitForPendingLoadSettings();
		GetComponent<NewGameFlow>().BeginFlow();
	}

	private void InitLoadScreen()
	{
		if (LoadScreen.Instance == null)
		{
			Util.KInstantiateUI(ScreenPrefabs.Instance.LoadScreen.gameObject, base.gameObject, force_active: true).GetComponent<LoadScreen>();
		}
	}

	private void LoadGame()
	{
		InitLoadScreen();
		LoadScreen.Instance.Activate();
	}

	public static void ActivateRetiredColoniesScreen(GameObject parent, string colonyID = "")
	{
		if (RetiredColonyInfoScreen.Instance == null)
		{
			Util.KInstantiateUI(ScreenPrefabs.Instance.RetiredColonyInfoScreen.gameObject, parent, force_active: true);
		}
		RetiredColonyInfoScreen.Instance.Show();
		if (!string.IsNullOrEmpty(colonyID))
		{
			if (SaveGame.Instance != null)
			{
				RetireColonyUtility.SaveColonySummaryData();
			}
			RetiredColonyInfoScreen.Instance.LoadColony(RetiredColonyInfoScreen.Instance.GetColonyDataByBaseName(colonyID));
		}
	}

	public static void ActivateRetiredColoniesScreenFromData(GameObject parent, RetiredColonyData data)
	{
		if (RetiredColonyInfoScreen.Instance == null)
		{
			Util.KInstantiateUI(ScreenPrefabs.Instance.RetiredColonyInfoScreen.gameObject, parent, force_active: true);
		}
		RetiredColonyInfoScreen.Instance.Show();
		RetiredColonyInfoScreen.Instance.LoadColony(data);
	}

	public static void ActivateInventoyScreen()
	{
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.kleiInventoryScreen);
	}

	public static void ActivateLockerMenu()
	{
		LockerMenuScreen.Instance.Show();
	}

	private void SpawnVideoScreen()
	{
		VideoScreen.Instance = Util.KInstantiateUI(ScreenPrefabs.Instance.VideoScreen.gameObject, base.gameObject).GetComponent<VideoScreen>();
	}

	private void Update()
	{
	}

	public void RefreshResumeButton(bool simpleCheck = false)
	{
		string latestSaveForCurrentDLC = SaveLoader.GetLatestSaveForCurrentDLC();
		bool flag = !string.IsNullOrEmpty(latestSaveForCurrentDLC) && File.Exists(latestSaveForCurrentDLC);
		if (flag)
		{
			try
			{
				if (GenericGameSettings.instance.demoMode)
				{
					flag = false;
				}
				System.DateTime lastWriteTime = File.GetLastWriteTime(latestSaveForCurrentDLC);
				SaveFileEntry value = default(SaveFileEntry);
				SaveGame.Header header = default(SaveGame.Header);
				SaveGame.GameInfo gameInfo = default(SaveGame.GameInfo);
				if (!saveFileEntries.TryGetValue(latestSaveForCurrentDLC, out value) || value.timeStamp != lastWriteTime)
				{
					gameInfo = SaveLoader.LoadHeader(latestSaveForCurrentDLC, out header);
					SaveFileEntry saveFileEntry = default(SaveFileEntry);
					saveFileEntry.timeStamp = lastWriteTime;
					saveFileEntry.header = header;
					saveFileEntry.headerData = gameInfo;
					value = saveFileEntry;
					saveFileEntries[latestSaveForCurrentDLC] = value;
				}
				else
				{
					header = value.header;
					gameInfo = value.headerData;
				}
				if (header.buildVersion > 623711 || gameInfo.saveMajorVersion != 7 || gameInfo.saveMinorVersion > 34)
				{
					flag = false;
				}
				if (!gameInfo.IsCompatableWithCurrentDlcConfiguration(out var _, out var _))
				{
					flag = false;
				}
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(latestSaveForCurrentDLC);
				if (!string.IsNullOrEmpty(gameInfo.baseName))
				{
					Button_ResumeGame.GetComponentsInChildren<LocText>()[1].text = string.Format(UI.FRONTEND.MAINMENU.RESUMEBUTTON_BASENAME, gameInfo.baseName, gameInfo.numberOfCycles + 1);
				}
				else
				{
					Button_ResumeGame.GetComponentsInChildren<LocText>()[1].text = fileNameWithoutExtension;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning(ex);
				flag = false;
			}
		}
		if (Button_ResumeGame != null && Button_ResumeGame.gameObject != null)
		{
			Button_ResumeGame.gameObject.SetActive(flag);
			KImage component = Button_NewGame.GetComponent<KImage>();
			component.colorStyleSetting = (flag ? normalButtonStyle : topButtonStyle);
			component.ApplyColorStyleSetting();
		}
		else
		{
			Debug.LogWarning("Why is the resume game button null?");
		}
	}

	private void Translations()
	{
		Util.KInstantiateUI<LanguageOptionsScreen>(ScreenPrefabs.Instance.languageOptionsScreen.gameObject, base.transform.parent.gameObject);
	}

	private void Mods()
	{
		Util.KInstantiateUI<ModsScreen>(ScreenPrefabs.Instance.modsMenu.gameObject, base.transform.parent.gameObject);
	}

	private void Options()
	{
		Util.KInstantiateUI<OptionsMenuScreen>(ScreenPrefabs.Instance.OptionsScreen.gameObject, base.gameObject, force_active: true);
	}

	private void QuitGame()
	{
		App.Quit();
	}

	public void StartFEAudio()
	{
		AudioMixer.instance.Reset();
		MusicManager.instance.KillAllSongs(STOP_MODE.ALLOWFADEOUT);
		MusicManager.instance.ConfigureSongs();
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndSnapshot);
		if (!AudioMixer.instance.SnapshotIsActive(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot))
		{
			AudioMixer.instance.StartUserVolumesSnapshot();
		}
		if (AudioDebug.Get().musicEnabled && !MusicManager.instance.SongIsPlaying(menuMusicEventName))
		{
			MusicManager.instance.PlaySong(menuMusicEventName);
		}
		CheckForAudioDriverIssue();
	}

	public void StopAmbience()
	{
		if (ambientLoop.isValid())
		{
			ambientLoop.stop(STOP_MODE.ALLOWFADEOUT);
			ambientLoop.release();
			ambientLoop.clearHandle();
		}
	}

	public void StopMainMenuMusic()
	{
		if (MusicManager.instance.SongIsPlaying(menuMusicEventName))
		{
			MusicManager.instance.StopSong(menuMusicEventName);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSnapshot);
		}
	}

	private void CheckForAudioDriverIssue()
	{
		if (!KFMOD.didFmodInitializeSuccessfully)
		{
			Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, force_active: true).PopupConfirmDialog(UI.FRONTEND.SUPPORTWARNINGS.AUDIO_DRIVERS, null, null, UI.FRONTEND.SUPPORTWARNINGS.AUDIO_DRIVERS_MORE_INFO, delegate
			{
				App.OpenWebURL("http://support.kleientertainment.com/customer/en/portal/articles/2947881-no-audio-when-playing-oxygen-not-included");
			}, null, null, null, GlobalResources.Instance().sadDupeAudio);
		}
	}

	private void CheckPlayerPrefsCorruption()
	{
		if (KPlayerPrefs.HasCorruptedFlag())
		{
			KPlayerPrefs.ResetCorruptedFlag();
			Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, force_active: true).PopupConfirmDialog(UI.FRONTEND.SUPPORTWARNINGS.PLAYER_PREFS_CORRUPTED, null, null, null, null, null, null, null, GlobalResources.Instance().sadDupe);
		}
	}

	private void CheckDoubleBoundKeys()
	{
		string text = "";
		HashSet<BindingEntry> hashSet = new HashSet<BindingEntry>();
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			if (GameInputMapping.KeyBindings[i].mKeyCode == KKeyCode.Mouse1)
			{
				continue;
			}
			for (int j = 0; j < GameInputMapping.KeyBindings.Length; j++)
			{
				if (i == j)
				{
					continue;
				}
				BindingEntry bindingEntry = GameInputMapping.KeyBindings[j];
				if (hashSet.Contains(bindingEntry))
				{
					continue;
				}
				BindingEntry bindingEntry2 = GameInputMapping.KeyBindings[i];
				if (bindingEntry2.mKeyCode != 0 && bindingEntry2.mKeyCode == bindingEntry.mKeyCode && bindingEntry2.mModifier == bindingEntry.mModifier && bindingEntry2.mRebindable && bindingEntry.mRebindable)
				{
					string mGroup = GameInputMapping.KeyBindings[i].mGroup;
					string mGroup2 = GameInputMapping.KeyBindings[j].mGroup;
					if ((mGroup == "Root" || mGroup2 == "Root" || mGroup == mGroup2) && (!(mGroup == "Root") || !bindingEntry.mIgnoreRootConflics) && (!(mGroup2 == "Root") || !bindingEntry2.mIgnoreRootConflics))
					{
						text = text + "\n\n" + bindingEntry2.mAction.ToString() + ": <b>" + bindingEntry2.mKeyCode.ToString() + "</b>\n" + bindingEntry.mAction.ToString() + ": <b>" + bindingEntry.mKeyCode.ToString() + "</b>";
						BindingEntry bindingEntry3 = bindingEntry2;
						bindingEntry3.mKeyCode = KKeyCode.None;
						bindingEntry3.mModifier = Modifier.None;
						GameInputMapping.KeyBindings[i] = bindingEntry3;
						bindingEntry3 = bindingEntry;
						bindingEntry3.mKeyCode = KKeyCode.None;
						bindingEntry3.mModifier = Modifier.None;
						GameInputMapping.KeyBindings[j] = bindingEntry3;
					}
				}
			}
			hashSet.Add(GameInputMapping.KeyBindings[i]);
		}
		if (text != "")
		{
			Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, force_active: true).PopupConfirmDialog(string.Format(UI.FRONTEND.SUPPORTWARNINGS.DUPLICATE_KEY_BINDINGS, text), null, null, null, null, null, null, null, GlobalResources.Instance().sadDupe);
		}
	}

	private void RestartGame()
	{
		App.instance.Restart();
	}
}
