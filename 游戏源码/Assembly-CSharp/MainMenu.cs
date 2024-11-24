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

// Token: 0x02001D84 RID: 7556
public class MainMenu : KScreen
{
	// Token: 0x17000A54 RID: 2644
	// (get) Token: 0x06009DE0 RID: 40416 RVA: 0x00106C01 File Offset: 0x00104E01
	public static MainMenu Instance
	{
		get
		{
			return MainMenu._instance;
		}
	}

	// Token: 0x06009DE1 RID: 40417 RVA: 0x003C8AE4 File Offset: 0x003C6CE4
	private KButton MakeButton(MainMenu.ButtonInfo info)
	{
		KButton kbutton = global::Util.KInstantiateUI<KButton>(this.buttonPrefab.gameObject, this.buttonParent, true);
		kbutton.onClick += info.action;
		KImage component = kbutton.GetComponent<KImage>();
		component.colorStyleSetting = info.style;
		component.ApplyColorStyleSetting();
		LocText componentInChildren = kbutton.GetComponentInChildren<LocText>();
		componentInChildren.text = info.text;
		componentInChildren.fontSize = (float)info.fontSize;
		return kbutton;
	}

	// Token: 0x06009DE2 RID: 40418 RVA: 0x003C8B50 File Offset: 0x003C6D50
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MainMenu._instance = this;
		this.Button_NewGame = this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.NEWGAME, new System.Action(this.NewGame), 22, this.topButtonStyle));
		this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.LOADGAME, new System.Action(this.LoadGame), 22, this.normalButtonStyle));
		this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.RETIREDCOLONIES, delegate()
		{
			MainMenu.ActivateRetiredColoniesScreen(this.transform.gameObject, "");
		}, 14, this.normalButtonStyle));
		this.lockerButton = this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.LOCKERMENU, delegate()
		{
			MainMenu.ActivateLockerMenu();
		}, 14, this.normalButtonStyle));
		if (DistributionPlatform.Initialized)
		{
			this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.TRANSLATIONS, new System.Action(this.Translations), 14, this.normalButtonStyle));
			this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MODS.TITLE, new System.Action(this.Mods), 14, this.normalButtonStyle));
		}
		this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.OPTIONS, new System.Action(this.Options), 14, this.normalButtonStyle));
		this.MakeButton(new MainMenu.ButtonInfo(UI.FRONTEND.MAINMENU.QUITTODESKTOP, new System.Action(this.QuitGame), 14, this.normalButtonStyle));
		this.RefreshResumeButton(false);
		this.Button_ResumeGame.onClick += this.ResumeGame;
		this.SpawnVideoScreen();
		this.StartFEAudio();
		this.CheckPlayerPrefsCorruption();
		if (PatchNotesScreen.ShouldShowScreen())
		{
			global::Util.KInstantiateUI(this.patchNotesScreenPrefab.gameObject, FrontEndManager.Instance.gameObject, true);
		}
		this.CheckDoubleBoundKeys();
		bool flag = DistributionPlatform.Inst.IsDLCPurchased("EXPANSION1_ID");
		this.expansion1Toggle.gameObject.SetActive(flag);
		if (this.expansion1Ad != null)
		{
			this.expansion1Ad.gameObject.SetActive(!flag);
		}
		this.RefreshDLCLogos();
		this.motd.Setup();
		if (DistributionPlatform.Initialized && DistributionPlatform.Inst.IsPreviousVersionBranch)
		{
			UnityEngine.Object.Instantiate<GameObject>(ScreenPrefabs.Instance.OldVersionWarningScreen, this.uiCanvas.transform);
		}
		string targetExpansion1AdURL = "";
		Sprite sprite = Assets.GetSprite("expansionPromo_en");
		if (DistributionPlatform.Initialized && this.expansion1Ad != null)
		{
			string name = DistributionPlatform.Inst.Name;
			if (!(name == "Steam"))
			{
				if (!(name == "Epic"))
				{
					if (name == "Rail")
					{
						targetExpansion1AdURL = "https://www.wegame.com.cn/store/2001539/";
						sprite = Assets.GetSprite("expansionPromo_cn");
					}
				}
				else
				{
					targetExpansion1AdURL = "https://store.epicgames.com/en-US/p/oxygen-not-included--spaced-out";
				}
			}
			else
			{
				targetExpansion1AdURL = "https://store.steampowered.com/app/1452490/Oxygen_Not_Included__Spaced_Out/";
			}
			this.expansion1Ad.GetComponentInChildren<KButton>().onClick += delegate()
			{
				App.OpenWebURL(targetExpansion1AdURL);
			};
			this.expansion1Ad.GetComponent<HierarchyReferences>().GetReference<Image>("Image").sprite = sprite;
		}
		this.activateOnSpawn = true;
	}

	// Token: 0x06009DE3 RID: 40419 RVA: 0x003C8E7C File Offset: 0x003C707C
	private void RefreshDLCLogos()
	{
		this.logoDLC1.GetReference<Image>("icon").material = (DlcManager.IsContentSubscribed("EXPANSION1_ID") ? GlobalResources.Instance().AnimUIMaterial : GlobalResources.Instance().AnimMaterialUIDesaturated);
		this.logoDLC2.GetReference<Image>("icon").material = (DlcManager.IsContentSubscribed("DLC2_ID") ? GlobalResources.Instance().AnimUIMaterial : GlobalResources.Instance().AnimMaterialUIDesaturated);
		this.logoDLC3.GetReference<Image>("icon").material = (DlcManager.IsContentSubscribed("DLC3_ID") ? GlobalResources.Instance().AnimUIMaterial : GlobalResources.Instance().AnimMaterialUIDesaturated);
		if (DistributionPlatform.Initialized || Application.isEditor)
		{
			string DLC1_STORE_URL = "";
			string DLC2_STORE_URL = "";
			string DLC3_STORE_URL = "";
			string name = DistributionPlatform.Inst.Name;
			if (!(name == "Steam"))
			{
				if (!(name == "Epic"))
				{
					if (name == "Rail")
					{
						DLC1_STORE_URL = "https://www.wegame.com.cn/store/2001539/";
						DLC2_STORE_URL = "https://www.wegame.com.cn/store/2002196/";
						DLC3_STORE_URL = "https://www.wegame.com.cn/store/2002196/";
						this.logoDLC1.GetReference<Image>("icon").sprite = Assets.GetSprite("dlc1_logo_crop_cn");
						this.logoDLC2.GetReference<Image>("icon").sprite = Assets.GetSprite("dlc2_logo_crop_cn");
						this.logoDLC3.GetReference<Image>("icon").sprite = Assets.GetSprite("dlc3_logo_crop_cn");
					}
				}
				else
				{
					DLC1_STORE_URL = "https://store.epicgames.com/en-US/p/oxygen-not-included--spaced-out";
					DLC2_STORE_URL = "https://store.epicgames.com/p/oxygen-not-included-oxygen-not-included-the-frosty-planet-pack-915ba1";
					DLC3_STORE_URL = "https://store.epicgames.com/p/oxygen-not-included-oxygen-not-included-the-frosty-planet-pack-915ba1";
				}
			}
			else
			{
				DLC1_STORE_URL = "https://store.steampowered.com/app/1452490/Oxygen_Not_Included__Spaced_Out/";
				DLC2_STORE_URL = "https://store.steampowered.com/app/2952300/Oxygen_Not_Included_The_Frosty_Planet_Pack/";
				DLC3_STORE_URL = "https://store.steampowered.com/app/2952300/Oxygen_Not_Included_The_Frosty_Planet_Pack/";
			}
			MultiToggle reference = this.logoDLC1.GetReference<MultiToggle>("multitoggle");
			reference.onClick = (System.Action)Delegate.Combine(reference.onClick, new System.Action(delegate()
			{
				if (DlcManager.IsContentOwned("EXPANSION1_ID"))
				{
					this.logoDLC1.GetReference<DLCToggle>("dlctoggle").ToggleExpansion1Cicked();
					return;
				}
				App.OpenWebURL(DLC1_STORE_URL);
			}));
			string text = this.GetDLCStatusString("EXPANSION1_ID", true);
			if (!DlcManager.IsContentOwned("EXPANSION1_ID"))
			{
				text = text + "\n\n" + UI.FRONTEND.MAINMENU.WISHLIST_AD_TOOLTIP;
			}
			else
			{
				text = (DlcManager.IsContentSubscribed("EXPANSION1_ID") ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1_TOOLTIP : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1_TOOLTIP);
			}
			this.logoDLC1.GetReference<ToolTip>("tooltip").SetSimpleTooltip(text);
			MultiToggle reference2 = this.logoDLC2.GetReference<MultiToggle>("multitoggle");
			reference2.onClick = (System.Action)Delegate.Combine(reference2.onClick, new System.Action(delegate()
			{
				App.OpenWebURL(DLC2_STORE_URL);
			}));
			this.logoDLC2.GetReference<LocText>("statuslabel").SetText(this.GetDLCStatusString("DLC2_ID", false));
			string text2 = this.GetDLCStatusString("DLC2_ID", true);
			text2 = text2 + "\n\n" + UI.FRONTEND.MAINMENU.WISHLIST_AD_TOOLTIP;
			this.logoDLC2.GetReference<ToolTip>("tooltip").SetSimpleTooltip(text2);
			MultiToggle reference3 = this.logoDLC3.GetReference<MultiToggle>("multitoggle");
			reference3.onClick = (System.Action)Delegate.Combine(reference3.onClick, new System.Action(delegate()
			{
				App.OpenWebURL(DLC3_STORE_URL);
			}));
			this.logoDLC3.GetReference<LocText>("statuslabel").SetText(this.GetDLCStatusString("DLC3_ID", false));
			string text3 = this.GetDLCStatusString("DLC3_ID", true);
			text3 = text3 + "\n\n" + UI.FRONTEND.MAINMENU.WISHLIST_AD_TOOLTIP;
			this.logoDLC3.GetReference<ToolTip>("tooltip").SetSimpleTooltip(text3);
		}
	}

	// Token: 0x06009DE4 RID: 40420 RVA: 0x003C9230 File Offset: 0x003C7430
	public string GetDLCStatusString(string dlcID, bool tooltip = false)
	{
		if (!DlcManager.IsContentOwned(dlcID))
		{
			return tooltip ? UI.FRONTEND.MAINMENU.DLC.CONTENT_NOTOWNED_TOOLTIP : UI.FRONTEND.MAINMENU.WISHLIST_AD;
		}
		if (DlcManager.IsContentSubscribed(dlcID))
		{
			return tooltip ? UI.FRONTEND.MAINMENU.DLC.CONTENT_ACTIVE_TOOLTIP : UI.FRONTEND.MAINMENU.DLC.CONTENT_INSTALLED_LABEL;
		}
		return tooltip ? UI.FRONTEND.MAINMENU.DLC.CONTENT_OWNED_NOTINSTALLED_TOOLTIP : UI.FRONTEND.MAINMENU.DLC.CONTENT_OWNED_NOTINSTALLED_LABEL;
	}

	// Token: 0x06009DE5 RID: 40421 RVA: 0x00106C08 File Offset: 0x00104E08
	private void OnApplicationFocus(bool focus)
	{
		if (focus)
		{
			this.RefreshResumeButton(false);
		}
	}

	// Token: 0x06009DE6 RID: 40422 RVA: 0x003C928C File Offset: 0x003C748C
	public override void OnKeyDown(KButtonEvent e)
	{
		base.OnKeyDown(e);
		if (e.Consumed)
		{
			return;
		}
		if (e.TryConsume(global::Action.DebugToggleUI))
		{
			this.m_screenshotMode = !this.m_screenshotMode;
			this.uiCanvas.alpha = (this.m_screenshotMode ? 0f : 1f);
		}
		KKeyCode key_code;
		switch (this.m_cheatInputCounter)
		{
		case 0:
			key_code = KKeyCode.K;
			break;
		case 1:
			key_code = KKeyCode.L;
			break;
		case 2:
			key_code = KKeyCode.E;
			break;
		case 3:
			key_code = KKeyCode.I;
			break;
		case 4:
			key_code = KKeyCode.P;
			break;
		case 5:
			key_code = KKeyCode.L;
			break;
		case 6:
			key_code = KKeyCode.A;
			break;
		default:
			key_code = KKeyCode.Y;
			break;
		}
		if (e.Controller.GetKeyDown(key_code))
		{
			e.Consumed = true;
			this.m_cheatInputCounter++;
			if (this.m_cheatInputCounter >= 8)
			{
				global::Debug.Log("Cheat Detected - enabling Debug Mode");
				DebugHandler.SetDebugEnabled(true);
				this.buildWatermark.RefreshText();
				this.m_cheatInputCounter = 0;
				return;
			}
		}
		else
		{
			this.m_cheatInputCounter = 0;
		}
	}

	// Token: 0x06009DE7 RID: 40423 RVA: 0x00106C14 File Offset: 0x00104E14
	private void PlayMouseOverSound()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x06009DE8 RID: 40424 RVA: 0x00106C26 File Offset: 0x00104E26
	private void PlayMouseClickSound()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
	}

	// Token: 0x06009DE9 RID: 40425 RVA: 0x003C938C File Offset: 0x003C758C
	protected override void OnSpawn()
	{
		global::Debug.Log("-- MAIN MENU -- ");
		base.OnSpawn();
		this.m_cheatInputCounter = 0;
		Canvas.ForceUpdateCanvases();
		this.ShowLanguageConfirmation();
		this.InitLoadScreen();
		LoadScreen.Instance.ShowMigrationIfNecessary(true);
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
			string format;
			if (ex is IOException)
			{
				format = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_INSUFFICIENT_SPACE, savePrefix);
			}
			else
			{
				format = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, savePrefix);
			}
			string text = string.Format(format, savePrefix);
			global::Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, true).PopupConfirmDialog(text, null, null, null, null, null, null, null, null);
		}
		Global.Instance.modManager.Report(base.gameObject);
		if ((GenericGameSettings.instance.autoResumeGame && !MainMenu.HasAutoresumedOnce && !KCrashReporter.hasCrash) || !string.IsNullOrEmpty(GenericGameSettings.instance.performanceCapture.saveGame) || KPlayerPrefs.HasKey("AutoResumeSaveFile"))
		{
			MainMenu.HasAutoresumedOnce = true;
			this.ResumeGame();
		}
		if (GenericGameSettings.instance.devAutoWorldGen && !KCrashReporter.hasCrash)
		{
			GenericGameSettings.instance.devAutoWorldGen = false;
			GenericGameSettings.instance.devAutoWorldGenActive = true;
			GenericGameSettings.instance.SaveSettings();
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.WorldGenScreen.gameObject, base.gameObject, true);
		}
		this.RefreshInventoryNotification();
	}

	// Token: 0x06009DEA RID: 40426 RVA: 0x001068D6 File Offset: 0x00104AD6
	protected override void OnForcedCleanUp()
	{
		base.OnForcedCleanUp();
	}

	// Token: 0x06009DEB RID: 40427 RVA: 0x003C9554 File Offset: 0x003C7754
	private void RefreshInventoryNotification()
	{
		bool active = PermitItems.HasUnopenedItem();
		this.lockerButton.GetComponent<HierarchyReferences>().GetReference<RectTransform>("AttentionIcon").gameObject.SetActive(active);
	}

	// Token: 0x06009DEC RID: 40428 RVA: 0x00106C38 File Offset: 0x00104E38
	protected override void OnActivate()
	{
		if (!this.ambientLoopEventName.IsNullOrWhiteSpace())
		{
			this.ambientLoop = KFMOD.CreateInstance(GlobalAssets.GetSound(this.ambientLoopEventName, false));
			if (this.ambientLoop.isValid())
			{
				this.ambientLoop.start();
			}
		}
	}

	// Token: 0x06009DED RID: 40429 RVA: 0x00106C77 File Offset: 0x00104E77
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		this.motd.CleanUp();
	}

	// Token: 0x06009DEE RID: 40430 RVA: 0x003C9588 File Offset: 0x003C7788
	public override void ScreenUpdate(bool topLevel)
	{
		this.refreshResumeButton = topLevel;
		if (KleiItemDropScreen.Instance != null && KleiItemDropScreen.Instance.gameObject.activeInHierarchy != this.itemDropOpenFlag)
		{
			this.RefreshInventoryNotification();
			this.itemDropOpenFlag = KleiItemDropScreen.Instance.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x06009DEF RID: 40431 RVA: 0x00106C8A File Offset: 0x00104E8A
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
		this.StopAmbience();
		this.motd.CleanUp();
	}

	// Token: 0x06009DF0 RID: 40432 RVA: 0x003C95DC File Offset: 0x003C77DC
	private void ShowLanguageConfirmation()
	{
		if (SteamManager.Initialized)
		{
			if (SteamUtils.GetSteamUILanguage() != "schinese")
			{
				return;
			}
			if (KPlayerPrefs.GetInt("LanguageConfirmationVersion") >= MainMenu.LANGUAGE_CONFIRMATION_VERSION)
			{
				return;
			}
			KPlayerPrefs.SetInt("LanguageConfirmationVersion", MainMenu.LANGUAGE_CONFIRMATION_VERSION);
			this.Translations();
		}
	}

	// Token: 0x06009DF1 RID: 40433 RVA: 0x003C962C File Offset: 0x003C782C
	private void ResumeGame()
	{
		string text;
		if (KPlayerPrefs.HasKey("AutoResumeSaveFile"))
		{
			text = KPlayerPrefs.GetString("AutoResumeSaveFile");
			KPlayerPrefs.DeleteKey("AutoResumeSaveFile");
		}
		else if (!string.IsNullOrEmpty(GenericGameSettings.instance.performanceCapture.saveGame))
		{
			text = GenericGameSettings.instance.performanceCapture.saveGame;
		}
		else
		{
			text = SaveLoader.GetLatestSaveForCurrentDLC();
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

	// Token: 0x06009DF2 RID: 40434 RVA: 0x00106CA3 File Offset: 0x00104EA3
	private void NewGame()
	{
		WorldGen.WaitForPendingLoadSettings();
		base.GetComponent<NewGameFlow>().BeginFlow();
	}

	// Token: 0x06009DF3 RID: 40435 RVA: 0x00106CB5 File Offset: 0x00104EB5
	private void InitLoadScreen()
	{
		if (LoadScreen.Instance == null)
		{
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.LoadScreen.gameObject, base.gameObject, true).GetComponent<LoadScreen>();
		}
	}

	// Token: 0x06009DF4 RID: 40436 RVA: 0x00106CE5 File Offset: 0x00104EE5
	private void LoadGame()
	{
		this.InitLoadScreen();
		LoadScreen.Instance.Activate();
	}

	// Token: 0x06009DF5 RID: 40437 RVA: 0x003C96C4 File Offset: 0x003C78C4
	public static void ActivateRetiredColoniesScreen(GameObject parent, string colonyID = "")
	{
		if (RetiredColonyInfoScreen.Instance == null)
		{
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.RetiredColonyInfoScreen.gameObject, parent, true);
		}
		RetiredColonyInfoScreen.Instance.Show(true);
		if (!string.IsNullOrEmpty(colonyID))
		{
			if (SaveGame.Instance != null)
			{
				RetireColonyUtility.SaveColonySummaryData();
			}
			RetiredColonyInfoScreen.Instance.LoadColony(RetiredColonyInfoScreen.Instance.GetColonyDataByBaseName(colonyID));
		}
	}

	// Token: 0x06009DF6 RID: 40438 RVA: 0x00106CF7 File Offset: 0x00104EF7
	public static void ActivateRetiredColoniesScreenFromData(GameObject parent, RetiredColonyData data)
	{
		if (RetiredColonyInfoScreen.Instance == null)
		{
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.RetiredColonyInfoScreen.gameObject, parent, true);
		}
		RetiredColonyInfoScreen.Instance.Show(true);
		RetiredColonyInfoScreen.Instance.LoadColony(data);
	}

	// Token: 0x06009DF7 RID: 40439 RVA: 0x00106D33 File Offset: 0x00104F33
	public static void ActivateInventoyScreen()
	{
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.kleiInventoryScreen, null);
	}

	// Token: 0x06009DF8 RID: 40440 RVA: 0x00106D4A File Offset: 0x00104F4A
	public static void ActivateLockerMenu()
	{
		LockerMenuScreen.Instance.Show(true);
	}

	// Token: 0x06009DF9 RID: 40441 RVA: 0x00106D57 File Offset: 0x00104F57
	private void SpawnVideoScreen()
	{
		VideoScreen.Instance = global::Util.KInstantiateUI(ScreenPrefabs.Instance.VideoScreen.gameObject, base.gameObject, false).GetComponent<VideoScreen>();
	}

	// Token: 0x06009DFA RID: 40442 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void Update()
	{
	}

	// Token: 0x06009DFB RID: 40443 RVA: 0x003C9730 File Offset: 0x003C7930
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
				MainMenu.SaveFileEntry saveFileEntry = default(MainMenu.SaveFileEntry);
				SaveGame.Header header = default(SaveGame.Header);
				SaveGame.GameInfo gameInfo = default(SaveGame.GameInfo);
				if (!this.saveFileEntries.TryGetValue(latestSaveForCurrentDLC, out saveFileEntry) || saveFileEntry.timeStamp != lastWriteTime)
				{
					gameInfo = SaveLoader.LoadHeader(latestSaveForCurrentDLC, out header);
					saveFileEntry = new MainMenu.SaveFileEntry
					{
						timeStamp = lastWriteTime,
						header = header,
						headerData = gameInfo
					};
					this.saveFileEntries[latestSaveForCurrentDLC] = saveFileEntry;
				}
				else
				{
					header = saveFileEntry.header;
					gameInfo = saveFileEntry.headerData;
				}
				if (header.buildVersion > 642695U || gameInfo.saveMajorVersion != 7 || gameInfo.saveMinorVersion > 35)
				{
					flag = false;
				}
				HashSet<string> hashSet;
				HashSet<string> hashSet2;
				if (!gameInfo.IsCompatableWithCurrentDlcConfiguration(out hashSet, out hashSet2))
				{
					flag = false;
				}
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(latestSaveForCurrentDLC);
				if (!string.IsNullOrEmpty(gameInfo.baseName))
				{
					this.Button_ResumeGame.GetComponentsInChildren<LocText>()[1].text = string.Format(UI.FRONTEND.MAINMENU.RESUMEBUTTON_BASENAME, gameInfo.baseName, gameInfo.numberOfCycles + 1);
				}
				else
				{
					this.Button_ResumeGame.GetComponentsInChildren<LocText>()[1].text = fileNameWithoutExtension;
				}
			}
			catch (Exception obj)
			{
				global::Debug.LogWarning(obj);
				flag = false;
			}
		}
		if (this.Button_ResumeGame != null && this.Button_ResumeGame.gameObject != null)
		{
			this.Button_ResumeGame.gameObject.SetActive(flag);
			KImage component = this.Button_NewGame.GetComponent<KImage>();
			component.colorStyleSetting = (flag ? this.normalButtonStyle : this.topButtonStyle);
			component.ApplyColorStyleSetting();
			return;
		}
		global::Debug.LogWarning("Why is the resume game button null?");
	}

	// Token: 0x06009DFC RID: 40444 RVA: 0x00106D7E File Offset: 0x00104F7E
	private void Translations()
	{
		global::Util.KInstantiateUI<LanguageOptionsScreen>(ScreenPrefabs.Instance.languageOptionsScreen.gameObject, base.transform.parent.gameObject, false);
	}

	// Token: 0x06009DFD RID: 40445 RVA: 0x00106DA6 File Offset: 0x00104FA6
	private void Mods()
	{
		global::Util.KInstantiateUI<ModsScreen>(ScreenPrefabs.Instance.modsMenu.gameObject, base.transform.parent.gameObject, false);
	}

	// Token: 0x06009DFE RID: 40446 RVA: 0x00106DCE File Offset: 0x00104FCE
	private void Options()
	{
		global::Util.KInstantiateUI<OptionsMenuScreen>(ScreenPrefabs.Instance.OptionsScreen.gameObject, base.gameObject, true);
	}

	// Token: 0x06009DFF RID: 40447 RVA: 0x000E3509 File Offset: 0x000E1709
	private void QuitGame()
	{
		App.Quit();
	}

	// Token: 0x06009E00 RID: 40448 RVA: 0x003C9918 File Offset: 0x003C7B18
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
		if (AudioDebug.Get().musicEnabled && !MusicManager.instance.SongIsPlaying(this.menuMusicEventName))
		{
			MusicManager.instance.PlaySong(this.menuMusicEventName, false);
		}
		this.CheckForAudioDriverIssue();
	}

	// Token: 0x06009E01 RID: 40449 RVA: 0x00106DEC File Offset: 0x00104FEC
	public void StopAmbience()
	{
		if (this.ambientLoop.isValid())
		{
			this.ambientLoop.stop(STOP_MODE.ALLOWFADEOUT);
			this.ambientLoop.release();
			this.ambientLoop.clearHandle();
		}
	}

	// Token: 0x06009E02 RID: 40450 RVA: 0x00106E1F File Offset: 0x0010501F
	public void StopMainMenuMusic()
	{
		if (MusicManager.instance.SongIsPlaying(this.menuMusicEventName))
		{
			MusicManager.instance.StopSong(this.menuMusicEventName, true, STOP_MODE.ALLOWFADEOUT);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSnapshot, STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x06009E03 RID: 40451 RVA: 0x003C99B0 File Offset: 0x003C7BB0
	private void CheckForAudioDriverIssue()
	{
		if (!KFMOD.didFmodInitializeSuccessfully)
		{
			global::Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, true).PopupConfirmDialog(UI.FRONTEND.SUPPORTWARNINGS.AUDIO_DRIVERS, null, null, UI.FRONTEND.SUPPORTWARNINGS.AUDIO_DRIVERS_MORE_INFO, delegate
			{
				App.OpenWebURL("http://support.kleientertainment.com/customer/en/portal/articles/2947881-no-audio-when-playing-oxygen-not-included");
			}, null, null, null, GlobalResources.Instance().sadDupeAudio);
		}
	}

	// Token: 0x06009E04 RID: 40452 RVA: 0x003C9A28 File Offset: 0x003C7C28
	private void CheckPlayerPrefsCorruption()
	{
		if (KPlayerPrefs.HasCorruptedFlag())
		{
			KPlayerPrefs.ResetCorruptedFlag();
			global::Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, true).PopupConfirmDialog(UI.FRONTEND.SUPPORTWARNINGS.PLAYER_PREFS_CORRUPTED, null, null, null, null, null, null, null, GlobalResources.Instance().sadDupe);
		}
	}

	// Token: 0x06009E05 RID: 40453 RVA: 0x003C9A7C File Offset: 0x003C7C7C
	private void CheckDoubleBoundKeys()
	{
		string text = "";
		HashSet<BindingEntry> hashSet = new HashSet<BindingEntry>();
		for (int i = 0; i < GameInputMapping.KeyBindings.Length; i++)
		{
			if (GameInputMapping.KeyBindings[i].mKeyCode != KKeyCode.Mouse1)
			{
				for (int j = 0; j < GameInputMapping.KeyBindings.Length; j++)
				{
					if (i != j)
					{
						BindingEntry bindingEntry = GameInputMapping.KeyBindings[j];
						if (!hashSet.Contains(bindingEntry))
						{
							BindingEntry bindingEntry2 = GameInputMapping.KeyBindings[i];
							if (bindingEntry2.mKeyCode != KKeyCode.None && bindingEntry2.mKeyCode == bindingEntry.mKeyCode && bindingEntry2.mModifier == bindingEntry.mModifier && bindingEntry2.mRebindable && bindingEntry.mRebindable)
							{
								string mGroup = GameInputMapping.KeyBindings[i].mGroup;
								string mGroup2 = GameInputMapping.KeyBindings[j].mGroup;
								if ((mGroup == "Root" || mGroup2 == "Root" || mGroup == mGroup2) && (!(mGroup == "Root") || !bindingEntry.mIgnoreRootConflics) && (!(mGroup2 == "Root") || !bindingEntry2.mIgnoreRootConflics))
								{
									text = string.Concat(new string[]
									{
										text,
										"\n\n",
										bindingEntry2.mAction.ToString(),
										": <b>",
										bindingEntry2.mKeyCode.ToString(),
										"</b>\n",
										bindingEntry.mAction.ToString(),
										": <b>",
										bindingEntry.mKeyCode.ToString(),
										"</b>"
									});
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
					}
				}
				hashSet.Add(GameInputMapping.KeyBindings[i]);
			}
		}
		if (text != "")
		{
			global::Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, base.gameObject, true).PopupConfirmDialog(string.Format(UI.FRONTEND.SUPPORTWARNINGS.DUPLICATE_KEY_BINDINGS, text), null, null, null, null, null, null, null, GlobalResources.Instance().sadDupe);
		}
	}

	// Token: 0x06009E06 RID: 40454 RVA: 0x00106E5B File Offset: 0x0010505B
	private void RestartGame()
	{
		App.instance.Restart();
	}

	// Token: 0x04007BB5 RID: 31669
	private static MainMenu _instance;

	// Token: 0x04007BB6 RID: 31670
	public KButton Button_ResumeGame;

	// Token: 0x04007BB7 RID: 31671
	private KButton Button_NewGame;

	// Token: 0x04007BB8 RID: 31672
	private GameObject GameSettingsScreen;

	// Token: 0x04007BB9 RID: 31673
	private bool m_screenshotMode;

	// Token: 0x04007BBA RID: 31674
	[SerializeField]
	private CanvasGroup uiCanvas;

	// Token: 0x04007BBB RID: 31675
	[SerializeField]
	private KButton buttonPrefab;

	// Token: 0x04007BBC RID: 31676
	[SerializeField]
	private GameObject buttonParent;

	// Token: 0x04007BBD RID: 31677
	[SerializeField]
	private ColorStyleSetting topButtonStyle;

	// Token: 0x04007BBE RID: 31678
	[SerializeField]
	private ColorStyleSetting normalButtonStyle;

	// Token: 0x04007BBF RID: 31679
	[SerializeField]
	private string menuMusicEventName;

	// Token: 0x04007BC0 RID: 31680
	[SerializeField]
	private string ambientLoopEventName;

	// Token: 0x04007BC1 RID: 31681
	private EventInstance ambientLoop;

	// Token: 0x04007BC2 RID: 31682
	[SerializeField]
	private MainMenu_Motd motd;

	// Token: 0x04007BC3 RID: 31683
	[SerializeField]
	private PatchNotesScreen patchNotesScreenPrefab;

	// Token: 0x04007BC4 RID: 31684
	[SerializeField]
	private NextUpdateTimer nextUpdateTimer;

	// Token: 0x04007BC5 RID: 31685
	[SerializeField]
	private DLCToggle expansion1Toggle;

	// Token: 0x04007BC6 RID: 31686
	[SerializeField]
	private GameObject expansion1Ad;

	// Token: 0x04007BC7 RID: 31687
	[SerializeField]
	private BuildWatermark buildWatermark;

	// Token: 0x04007BC8 RID: 31688
	[SerializeField]
	public string IntroShortName;

	// Token: 0x04007BC9 RID: 31689
	[SerializeField]
	private HierarchyReferences logoDLC1;

	// Token: 0x04007BCA RID: 31690
	[SerializeField]
	private HierarchyReferences logoDLC2;

	// Token: 0x04007BCB RID: 31691
	[SerializeField]
	private HierarchyReferences logoDLC3;

	// Token: 0x04007BCC RID: 31692
	private KButton lockerButton;

	// Token: 0x04007BCD RID: 31693
	private bool itemDropOpenFlag;

	// Token: 0x04007BCE RID: 31694
	private static bool HasAutoresumedOnce = false;

	// Token: 0x04007BCF RID: 31695
	private bool refreshResumeButton = true;

	// Token: 0x04007BD0 RID: 31696
	private int m_cheatInputCounter;

	// Token: 0x04007BD1 RID: 31697
	public const string AutoResumeSaveFileKey = "AutoResumeSaveFile";

	// Token: 0x04007BD2 RID: 31698
	public const string PLAY_SHORT_ON_LAUNCH = "PlayShortOnLaunch";

	// Token: 0x04007BD3 RID: 31699
	private static int LANGUAGE_CONFIRMATION_VERSION = 2;

	// Token: 0x04007BD4 RID: 31700
	private Dictionary<string, MainMenu.SaveFileEntry> saveFileEntries = new Dictionary<string, MainMenu.SaveFileEntry>();

	// Token: 0x02001D85 RID: 7557
	private struct ButtonInfo
	{
		// Token: 0x06009E09 RID: 40457 RVA: 0x00106E8F File Offset: 0x0010508F
		public ButtonInfo(LocString text, System.Action action, int font_size, ColorStyleSetting style)
		{
			this.text = text;
			this.action = action;
			this.fontSize = font_size;
			this.style = style;
		}

		// Token: 0x04007BD5 RID: 31701
		public LocString text;

		// Token: 0x04007BD6 RID: 31702
		public System.Action action;

		// Token: 0x04007BD7 RID: 31703
		public int fontSize;

		// Token: 0x04007BD8 RID: 31704
		public ColorStyleSetting style;
	}

	// Token: 0x02001D86 RID: 7558
	private struct SaveFileEntry
	{
		// Token: 0x04007BD9 RID: 31705
		public System.DateTime timeStamp;

		// Token: 0x04007BDA RID: 31706
		public SaveGame.Header header;

		// Token: 0x04007BDB RID: 31707
		public SaveGame.GameInfo headerData;
	}
}
