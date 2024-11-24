using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ionic.Zlib;
using Klei;
using Klei.AI;
using Klei.CustomSettings;
using KMod;
using KSerialization;
using Newtonsoft.Json;
using ProcGen;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x0200180C RID: 6156
[AddComponentMenu("KMonoBehaviour/scripts/SaveLoader")]
public class SaveLoader : KMonoBehaviour
{
	// Token: 0x1700081D RID: 2077
	// (get) Token: 0x06007F01 RID: 32513 RVA: 0x000F3B8A File Offset: 0x000F1D8A
	// (set) Token: 0x06007F02 RID: 32514 RVA: 0x000F3B92 File Offset: 0x000F1D92
	public bool loadedFromSave { get; private set; }

	// Token: 0x06007F03 RID: 32515 RVA: 0x000F3B9B File Offset: 0x000F1D9B
	public static void DestroyInstance()
	{
		SaveLoader.Instance = null;
	}

	// Token: 0x1700081E RID: 2078
	// (get) Token: 0x06007F04 RID: 32516 RVA: 0x000F3BA3 File Offset: 0x000F1DA3
	// (set) Token: 0x06007F05 RID: 32517 RVA: 0x000F3BAA File Offset: 0x000F1DAA
	public static SaveLoader Instance { get; private set; }

	// Token: 0x1700081F RID: 2079
	// (get) Token: 0x06007F06 RID: 32518 RVA: 0x000F3BB2 File Offset: 0x000F1DB2
	// (set) Token: 0x06007F07 RID: 32519 RVA: 0x000F3BBA File Offset: 0x000F1DBA
	public Action<Cluster> OnWorldGenComplete { get; set; }

	// Token: 0x17000820 RID: 2080
	// (get) Token: 0x06007F08 RID: 32520 RVA: 0x000F3BC3 File Offset: 0x000F1DC3
	public Cluster Cluster
	{
		get
		{
			return this.m_cluster;
		}
	}

	// Token: 0x17000821 RID: 2081
	// (get) Token: 0x06007F09 RID: 32521 RVA: 0x000F3BCB File Offset: 0x000F1DCB
	public ClusterLayout ClusterLayout
	{
		get
		{
			if (this.m_clusterLayout == null)
			{
				this.m_clusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
			}
			return this.m_clusterLayout;
		}
	}

	// Token: 0x17000822 RID: 2082
	// (get) Token: 0x06007F0A RID: 32522 RVA: 0x000F3BEB File Offset: 0x000F1DEB
	// (set) Token: 0x06007F0B RID: 32523 RVA: 0x000F3BF3 File Offset: 0x000F1DF3
	public SaveGame.GameInfo GameInfo { get; private set; }

	// Token: 0x06007F0C RID: 32524 RVA: 0x000F3BFC File Offset: 0x000F1DFC
	protected override void OnPrefabInit()
	{
		SaveLoader.Instance = this;
		this.saveManager = base.GetComponent<SaveManager>();
	}

	// Token: 0x06007F0D RID: 32525 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void MoveCorruptFile(string filename)
	{
	}

	// Token: 0x06007F0E RID: 32526 RVA: 0x0032D5EC File Offset: 0x0032B7EC
	protected override void OnSpawn()
	{
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (WorldGen.CanLoad(activeSaveFilePath))
		{
			Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
			SimMessages.CreateSimElementsTable(ElementLoader.elements);
			SimMessages.CreateDiseaseTable(Db.Get().Diseases);
			this.loadedFromSave = true;
			this.loadedFromSave = this.Load(activeSaveFilePath);
			this.saveFileCorrupt = !this.loadedFromSave;
			if (!this.loadedFromSave)
			{
				SaveLoader.SetActiveSaveFilePath(null);
				if (this.mustRestartOnFail)
				{
					this.MoveCorruptFile(activeSaveFilePath);
					Sim.Shutdown();
					App.LoadScene("frontend");
					return;
				}
			}
		}
		if (!this.loadedFromSave)
		{
			Sim.Shutdown();
			if (!string.IsNullOrEmpty(activeSaveFilePath))
			{
				DebugUtil.LogArgs(new object[]
				{
					"Couldn't load [" + activeSaveFilePath + "]"
				});
			}
			if (this.saveFileCorrupt)
			{
				this.MoveCorruptFile(activeSaveFilePath);
			}
			if (!this.LoadFromWorldGen())
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Couldn't start new game with current world gen, moving file"
				});
				KMonoBehaviour.isLoadingScene = true;
				this.MoveCorruptFile(WorldGen.WORLDGEN_SAVE_FILENAME);
				App.LoadScene("frontend");
			}
		}
	}

	// Token: 0x06007F0F RID: 32527 RVA: 0x0032D6FC File Offset: 0x0032B8FC
	private static void CompressContents(BinaryWriter fileWriter, byte[] uncompressed, int length)
	{
		using (ZlibStream zlibStream = new ZlibStream(fileWriter.BaseStream, CompressionMode.Compress, Ionic.Zlib.CompressionLevel.BestSpeed))
		{
			zlibStream.Write(uncompressed, 0, length);
			zlibStream.Flush();
		}
	}

	// Token: 0x06007F10 RID: 32528 RVA: 0x0032D744 File Offset: 0x0032B944
	private byte[] FloatToBytes(float[] floats)
	{
		byte[] array = new byte[floats.Length * 4];
		Buffer.BlockCopy(floats, 0, array, 0, array.Length);
		return array;
	}

	// Token: 0x06007F11 RID: 32529 RVA: 0x000F3C10 File Offset: 0x000F1E10
	private static byte[] DecompressContents(byte[] compressed)
	{
		return ZlibStream.UncompressBuffer(compressed);
	}

	// Token: 0x06007F12 RID: 32530 RVA: 0x0032D76C File Offset: 0x0032B96C
	private float[] BytesToFloat(byte[] bytes)
	{
		float[] array = new float[bytes.Length / 4];
		Buffer.BlockCopy(bytes, 0, array, 0, bytes.Length);
		return array;
	}

	// Token: 0x06007F13 RID: 32531 RVA: 0x0032D794 File Offset: 0x0032B994
	private SaveFileRoot PrepSaveFile()
	{
		SaveFileRoot saveFileRoot = new SaveFileRoot();
		saveFileRoot.WidthInCells = Grid.WidthInCells;
		saveFileRoot.HeightInCells = Grid.HeightInCells;
		saveFileRoot.streamed["GridVisible"] = Grid.Visible;
		saveFileRoot.streamed["GridSpawnable"] = Grid.Spawnable;
		saveFileRoot.streamed["GridDamage"] = this.FloatToBytes(Grid.Damage);
		Global.Instance.modManager.SendMetricsEvent();
		saveFileRoot.active_mods = new List<Label>();
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if (mod.IsEnabledForActiveDlc())
			{
				saveFileRoot.active_mods.Add(mod.label);
			}
		}
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				Camera.main.transform.parent.GetComponent<CameraController>().Save(binaryWriter);
			}
			saveFileRoot.streamed["Camera"] = memoryStream.ToArray();
		}
		return saveFileRoot;
	}

	// Token: 0x06007F14 RID: 32532 RVA: 0x000F3C18 File Offset: 0x000F1E18
	private void Save(BinaryWriter writer)
	{
		writer.WriteKleiString("world");
		Serializer.Serialize(this.PrepSaveFile(), writer);
		Game.SaveSettings(writer);
		Sim.Save(writer, 0, 0);
		this.saveManager.Save(writer);
		Game.Instance.Save(writer);
	}

	// Token: 0x06007F15 RID: 32533 RVA: 0x0032D8F0 File Offset: 0x0032BAF0
	private bool Load(IReader reader)
	{
		global::Debug.Assert(reader.ReadKleiString() == "world");
		Deserializer deserializer = new Deserializer(reader);
		SaveFileRoot saveFileRoot = new SaveFileRoot();
		deserializer.Deserialize(saveFileRoot);
		if ((this.GameInfo.saveMajorVersion == 7 || this.GameInfo.saveMinorVersion < 8) && saveFileRoot.requiredMods != null)
		{
			saveFileRoot.active_mods = new List<Label>();
			foreach (ModInfo modInfo in saveFileRoot.requiredMods)
			{
				saveFileRoot.active_mods.Add(new Label
				{
					id = modInfo.assetID,
					version = (long)modInfo.lastModifiedTime,
					distribution_platform = Label.DistributionPlatform.Steam,
					title = modInfo.description
				});
			}
			saveFileRoot.requiredMods.Clear();
		}
		KMod.Manager modManager = Global.Instance.modManager;
		modManager.Load(Content.LayerableFiles);
		if (!modManager.MatchFootprint(saveFileRoot.active_mods, Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Mod footprint of save file doesn't match current mod configuration"
			});
		}
		string text = string.Format("Mod Footprint ({0}):", saveFileRoot.active_mods.Count);
		foreach (Label label in saveFileRoot.active_mods)
		{
			text = text + "\n  - " + label.title;
		}
		global::Debug.Log(text);
		this.LogActiveMods();
		Global.Instance.modManager.SendMetricsEvent();
		WorldGen.LoadSettings(false);
		CustomGameSettings.Instance.LoadClusters();
		if (this.GameInfo.clusterId == null)
		{
			SaveGame.GameInfo gameInfo = this.GameInfo;
			if (!string.IsNullOrEmpty(saveFileRoot.clusterID))
			{
				gameInfo.clusterId = saveFileRoot.clusterID;
			}
			else
			{
				try
				{
					gameInfo.clusterId = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout).id;
				}
				catch
				{
					gameInfo.clusterId = WorldGenSettings.ClusterDefaultName;
					CustomGameSettings.Instance.SetQualitySetting(CustomGameSettingConfigs.ClusterLayout, gameInfo.clusterId);
				}
			}
			this.GameInfo = gameInfo;
		}
		Game.clusterId = this.GameInfo.clusterId;
		Game.LoadSettings(deserializer);
		GridSettings.Reset(saveFileRoot.WidthInCells, saveFileRoot.HeightInCells);
		if (Application.isPlaying)
		{
			Singleton<KBatchedAnimUpdater>.Instance.InitializeGrid();
		}
		Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
		SimMessages.CreateSimElementsTable(ElementLoader.elements);
		Sim.AllocateCells(saveFileRoot.WidthInCells, saveFileRoot.HeightInCells, false);
		SimMessages.CreateDiseaseTable(Db.Get().Diseases);
		Sim.HandleMessage(SimMessageHashes.ClearUnoccupiedCells, 0, null);
		IReader reader2;
		if (saveFileRoot.streamed.ContainsKey("Sim"))
		{
			reader2 = new FastReader(saveFileRoot.streamed["Sim"]);
		}
		else
		{
			reader2 = reader;
		}
		if (Sim.LoadWorld(reader2) != 0)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"\n--- Error loading save ---\nSimDLL found bad data\n"
			});
			Sim.Shutdown();
			return false;
		}
		Sim.Start();
		SceneInitializer.Instance.PostLoadPrefabs();
		this.mustRestartOnFail = true;
		if (!this.saveManager.Load(reader))
		{
			Sim.Shutdown();
			DebugUtil.LogWarningArgs(new object[]
			{
				"\n--- Error loading save ---\n"
			});
			SaveLoader.SetActiveSaveFilePath(null);
			return false;
		}
		Grid.Visible = saveFileRoot.streamed["GridVisible"];
		if (saveFileRoot.streamed.ContainsKey("GridSpawnable"))
		{
			Grid.Spawnable = saveFileRoot.streamed["GridSpawnable"];
		}
		Grid.Damage = this.BytesToFloat(saveFileRoot.streamed["GridDamage"]);
		Game.Instance.Load(deserializer);
		CameraSaveData.Load(new FastReader(saveFileRoot.streamed["Camera"]));
		ClusterManager.Instance.InitializeWorldGrid();
		SimMessages.DefineWorldOffsets((from container in ClusterManager.Instance.WorldContainers
		select new SimMessages.WorldOffsetData
		{
			worldOffsetX = container.WorldOffset.x,
			worldOffsetY = container.WorldOffset.y,
			worldSizeX = container.WorldSize.x,
			worldSizeY = container.WorldSize.y
		}).ToList<SimMessages.WorldOffsetData>());
		return true;
	}

	// Token: 0x06007F16 RID: 32534 RVA: 0x0032DD14 File Offset: 0x0032BF14
	private void LogActiveMods()
	{
		string text = string.Format("Active Mods ({0}):", Global.Instance.modManager.mods.Count((Mod x) => x.IsEnabledForActiveDlc()));
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if (mod.IsEnabledForActiveDlc())
			{
				text = text + "\n  - " + mod.title;
			}
		}
		global::Debug.Log(text);
	}

	// Token: 0x06007F17 RID: 32535 RVA: 0x000F3C56 File Offset: 0x000F1E56
	public static string GetSavePrefix()
	{
		return System.IO.Path.Combine(global::Util.RootFolder(), string.Format("{0}{1}", "save_files", System.IO.Path.DirectorySeparatorChar));
	}

	// Token: 0x06007F18 RID: 32536 RVA: 0x0032DDCC File Offset: 0x0032BFCC
	public static string GetCloudSavePrefix()
	{
		string text = System.IO.Path.Combine(global::Util.RootFolder(), string.Format("{0}{1}", "cloud_save_files", System.IO.Path.DirectorySeparatorChar));
		string userID = SaveLoader.GetUserID();
		if (string.IsNullOrEmpty(userID))
		{
			return null;
		}
		text = System.IO.Path.Combine(text, userID);
		if (!System.IO.Directory.Exists(text))
		{
			System.IO.Directory.CreateDirectory(text);
		}
		return text;
	}

	// Token: 0x06007F19 RID: 32537 RVA: 0x0032DE28 File Offset: 0x0032C028
	public static string GetSavePrefixAndCreateFolder()
	{
		string savePrefix = SaveLoader.GetSavePrefix();
		if (!System.IO.Directory.Exists(savePrefix))
		{
			System.IO.Directory.CreateDirectory(savePrefix);
		}
		return savePrefix;
	}

	// Token: 0x06007F1A RID: 32538 RVA: 0x0032DE4C File Offset: 0x0032C04C
	public static string GetUserID()
	{
		DistributionPlatform.User localUser = DistributionPlatform.Inst.LocalUser;
		if (localUser == null)
		{
			return null;
		}
		return localUser.Id.ToString();
	}

	// Token: 0x06007F1B RID: 32539 RVA: 0x0032DE74 File Offset: 0x0032C074
	public static string GetNextUsableSavePath(string filename)
	{
		int num = 0;
		string arg = System.IO.Path.ChangeExtension(filename, null);
		while (File.Exists(filename))
		{
			filename = SaveScreen.GetValidSaveFilename(string.Format("{0} ({1})", arg, num));
			num++;
		}
		return filename;
	}

	// Token: 0x06007F1C RID: 32540 RVA: 0x000F3C7B File Offset: 0x000F1E7B
	public static string GetOriginalSaveFileName(string filename)
	{
		if (!filename.Contains("/") && !filename.Contains("\\"))
		{
			return filename;
		}
		filename.Replace('\\', '/');
		return System.IO.Path.GetFileName(filename);
	}

	// Token: 0x06007F1D RID: 32541 RVA: 0x000F3CAA File Offset: 0x000F1EAA
	public static bool IsSaveAuto(string filename)
	{
		filename = filename.Replace('\\', '/');
		return filename.Contains("/auto_save/");
	}

	// Token: 0x06007F1E RID: 32542 RVA: 0x000F3CC3 File Offset: 0x000F1EC3
	public static bool IsSaveLocal(string filename)
	{
		filename = filename.Replace('\\', '/');
		return filename.Contains("/save_files/");
	}

	// Token: 0x06007F1F RID: 32543 RVA: 0x000F3CDC File Offset: 0x000F1EDC
	public static bool IsSaveCloud(string filename)
	{
		filename = filename.Replace('\\', '/');
		return filename.Contains("/cloud_save_files/");
	}

	// Token: 0x06007F20 RID: 32544 RVA: 0x0032DEB4 File Offset: 0x0032C0B4
	public static string GetAutoSavePrefix()
	{
		string text = System.IO.Path.Combine(SaveLoader.GetSavePrefixAndCreateFolder(), string.Format("{0}{1}", "auto_save", System.IO.Path.DirectorySeparatorChar));
		if (!System.IO.Directory.Exists(text))
		{
			System.IO.Directory.CreateDirectory(text);
		}
		return text;
	}

	// Token: 0x06007F21 RID: 32545 RVA: 0x000F3CF5 File Offset: 0x000F1EF5
	public static void SetActiveSaveFilePath(string path)
	{
		KPlayerPrefs.SetString("SaveFilenameKey/", path);
	}

	// Token: 0x06007F22 RID: 32546 RVA: 0x000F3D02 File Offset: 0x000F1F02
	public static string GetActiveSaveFilePath()
	{
		return KPlayerPrefs.GetString("SaveFilenameKey/");
	}

	// Token: 0x06007F23 RID: 32547 RVA: 0x0032DEF8 File Offset: 0x0032C0F8
	public static string GetActiveAutoSavePath()
	{
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (activeSaveFilePath == null)
		{
			return SaveLoader.GetAutoSavePrefix();
		}
		return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(activeSaveFilePath), "auto_save");
	}

	// Token: 0x06007F24 RID: 32548 RVA: 0x000F3D0E File Offset: 0x000F1F0E
	public static string GetAutosaveFilePath()
	{
		return SaveLoader.GetAutoSavePrefix() + "AutoSave Cycle 1.sav";
	}

	// Token: 0x06007F25 RID: 32549 RVA: 0x0032DF24 File Offset: 0x0032C124
	public static string GetActiveSaveColonyFolder()
	{
		string text = SaveLoader.GetActiveSaveFolder();
		if (text == null)
		{
			text = System.IO.Path.Combine(SaveLoader.GetSavePrefix(), SaveLoader.Instance.GameInfo.baseName);
		}
		return text;
	}

	// Token: 0x06007F26 RID: 32550 RVA: 0x0032DF58 File Offset: 0x0032C158
	public static string GetActiveSaveFolder()
	{
		string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
		if (!string.IsNullOrEmpty(activeSaveFilePath))
		{
			return System.IO.Path.GetDirectoryName(activeSaveFilePath);
		}
		return null;
	}

	// Token: 0x06007F27 RID: 32551 RVA: 0x0032DF7C File Offset: 0x0032C17C
	public static List<SaveLoader.SaveFileEntry> GetSaveFiles(string save_dir, bool sort, SearchOption search = SearchOption.AllDirectories)
	{
		List<SaveLoader.SaveFileEntry> list = new List<SaveLoader.SaveFileEntry>();
		if (string.IsNullOrEmpty(save_dir))
		{
			return list;
		}
		try
		{
			if (!System.IO.Directory.Exists(save_dir))
			{
				System.IO.Directory.CreateDirectory(save_dir);
			}
			foreach (string text in System.IO.Directory.GetFiles(save_dir, "*.sav", search))
			{
				try
				{
					System.DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(text);
					SaveLoader.SaveFileEntry item = new SaveLoader.SaveFileEntry
					{
						path = text,
						timeStamp = lastWriteTimeUtc
					};
					list.Add(item);
				}
				catch (Exception ex)
				{
					global::Debug.LogWarning("Problem reading file: " + text + "\n" + ex.ToString());
				}
			}
			if (sort)
			{
				list.Sort((SaveLoader.SaveFileEntry x, SaveLoader.SaveFileEntry y) => y.timeStamp.CompareTo(x.timeStamp));
			}
		}
		catch (Exception ex2)
		{
			string text2 = null;
			if (ex2 is UnauthorizedAccessException)
			{
				text2 = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, save_dir);
			}
			else if (ex2 is IOException)
			{
				text2 = string.Format(UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_INSUFFICIENT_SPACE, save_dir);
			}
			if (text2 == null)
			{
				throw ex2;
			}
			GameObject parent = (FrontEndManager.Instance == null) ? GameScreenManager.Instance.ssOverlayCanvas : FrontEndManager.Instance.gameObject;
			global::Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(text2, null, null, null, null, null, null, null, null);
		}
		return list;
	}

	// Token: 0x06007F28 RID: 32552 RVA: 0x0032E0FC File Offset: 0x0032C2FC
	public static List<SaveLoader.SaveFileEntry> GetAllFiles(bool sort, SaveLoader.SaveType type = SaveLoader.SaveType.both)
	{
		switch (type)
		{
		case SaveLoader.SaveType.local:
			return SaveLoader.GetSaveFiles(SaveLoader.GetSavePrefixAndCreateFolder(), sort, SearchOption.AllDirectories);
		case SaveLoader.SaveType.cloud:
			return SaveLoader.GetSaveFiles(SaveLoader.GetCloudSavePrefix(), sort, SearchOption.AllDirectories);
		case SaveLoader.SaveType.both:
		{
			List<SaveLoader.SaveFileEntry> saveFiles = SaveLoader.GetSaveFiles(SaveLoader.GetSavePrefixAndCreateFolder(), false, SearchOption.AllDirectories);
			List<SaveLoader.SaveFileEntry> saveFiles2 = SaveLoader.GetSaveFiles(SaveLoader.GetCloudSavePrefix(), false, SearchOption.AllDirectories);
			saveFiles.AddRange(saveFiles2);
			if (sort)
			{
				saveFiles.Sort((SaveLoader.SaveFileEntry x, SaveLoader.SaveFileEntry y) => y.timeStamp.CompareTo(x.timeStamp));
			}
			return saveFiles;
		}
		default:
			return new List<SaveLoader.SaveFileEntry>();
		}
	}

	// Token: 0x06007F29 RID: 32553 RVA: 0x000F3D1F File Offset: 0x000F1F1F
	public static List<SaveLoader.SaveFileEntry> GetAllColonyFiles(bool sort, SearchOption search = SearchOption.TopDirectoryOnly)
	{
		return SaveLoader.GetSaveFiles(SaveLoader.GetActiveSaveColonyFolder(), sort, search);
	}

	// Token: 0x06007F2A RID: 32554 RVA: 0x000F3D2D File Offset: 0x000F1F2D
	public static bool GetCloudSavesDefault()
	{
		return !(SaveLoader.GetCloudSavesDefaultPref() == "Disabled");
	}

	// Token: 0x06007F2B RID: 32555 RVA: 0x0032E188 File Offset: 0x0032C388
	public static string GetCloudSavesDefaultPref()
	{
		string text = KPlayerPrefs.GetString("SavesDefaultToCloud", "Enabled");
		if (text != "Enabled" && text != "Disabled")
		{
			text = "Enabled";
		}
		return text;
	}

	// Token: 0x06007F2C RID: 32556 RVA: 0x000F3D43 File Offset: 0x000F1F43
	public static void SetCloudSavesDefault(bool value)
	{
		SaveLoader.SetCloudSavesDefaultPref(value ? "Enabled" : "Disabled");
	}

	// Token: 0x06007F2D RID: 32557 RVA: 0x000F3D59 File Offset: 0x000F1F59
	public static void SetCloudSavesDefaultPref(string pref)
	{
		if (pref != "Enabled" && pref != "Disabled")
		{
			global::Debug.LogWarning("Ignoring cloud saves default pref `" + pref + "` as it's not valid, expected `Enabled` or `Disabled`");
			return;
		}
		KPlayerPrefs.SetString("SavesDefaultToCloud", pref);
	}

	// Token: 0x06007F2E RID: 32558 RVA: 0x000F3D96 File Offset: 0x000F1F96
	public static bool GetCloudSavesAvailable()
	{
		return !string.IsNullOrEmpty(SaveLoader.GetUserID()) && SaveLoader.GetCloudSavePrefix() != null;
	}

	// Token: 0x06007F2F RID: 32559 RVA: 0x0032E1C8 File Offset: 0x0032C3C8
	public static string GetLatestSaveForCurrentDLC()
	{
		List<SaveLoader.SaveFileEntry> allFiles = SaveLoader.GetAllFiles(true, SaveLoader.SaveType.both);
		for (int i = 0; i < allFiles.Count; i++)
		{
			global::Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(allFiles[i].path);
			if (fileInfo != null)
			{
				SaveGame.Header first = fileInfo.first;
				SaveGame.GameInfo second = fileInfo.second;
				HashSet<string> hashSet;
				HashSet<string> hashSet2;
				if (second.saveMajorVersion >= 7 && second.IsCompatableWithCurrentDlcConfiguration(out hashSet, out hashSet2))
				{
					return allFiles[i].path;
				}
			}
		}
		return null;
	}

	// Token: 0x06007F30 RID: 32560 RVA: 0x0032E23C File Offset: 0x0032C43C
	public void InitialSave()
	{
		string text = SaveLoader.GetActiveSaveFilePath();
		if (string.IsNullOrEmpty(text))
		{
			text = SaveLoader.GetAutosaveFilePath();
		}
		else if (!text.Contains(".sav"))
		{
			text += ".sav";
		}
		this.LogActiveMods();
		this.Save(text, false, true);
	}

	// Token: 0x06007F31 RID: 32561 RVA: 0x0032E288 File Offset: 0x0032C488
	public string Save(string filename, bool isAutoSave = false, bool updateSavePointer = true)
	{
		KSerialization.Manager.Clear();
		string directoryName = System.IO.Path.GetDirectoryName(filename);
		try
		{
			if (directoryName != null && !System.IO.Directory.Exists(directoryName))
			{
				System.IO.Directory.CreateDirectory(directoryName);
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning("Problem creating save folder for " + filename + "!\n" + ex.ToString());
		}
		this.ReportSaveMetrics(isAutoSave);
		RetireColonyUtility.SaveColonySummaryData();
		if (isAutoSave && !GenericGameSettings.instance.keepAllAutosaves)
		{
			List<SaveLoader.SaveFileEntry> saveFiles = SaveLoader.GetSaveFiles(SaveLoader.GetActiveAutoSavePath(), true, SearchOption.AllDirectories);
			List<string> list = new List<string>();
			foreach (SaveLoader.SaveFileEntry saveFileEntry in saveFiles)
			{
				global::Tuple<SaveGame.Header, SaveGame.GameInfo> fileInfo = SaveGame.GetFileInfo(saveFileEntry.path);
				if (fileInfo != null && SaveGame.GetSaveUniqueID(fileInfo.second) == SaveLoader.Instance.GameInfo.colonyGuid.ToString())
				{
					list.Add(saveFileEntry.path);
				}
			}
			for (int i = list.Count - 1; i >= 9; i--)
			{
				string text = list[i];
				try
				{
					global::Debug.Log("Deleting old autosave: " + text);
					File.Delete(text);
				}
				catch (Exception ex2)
				{
					global::Debug.LogWarning("Problem deleting autosave: " + text + "\n" + ex2.ToString());
				}
				string text2 = System.IO.Path.ChangeExtension(text, ".png");
				try
				{
					if (File.Exists(text2))
					{
						File.Delete(text2);
					}
				}
				catch (Exception ex3)
				{
					global::Debug.LogWarning("Problem deleting autosave screenshot: " + text2 + "\n" + ex3.ToString());
				}
			}
		}
		using (MemoryStream memoryStream = new MemoryStream((int)((float)this.lastUncompressedSize * 1.1f)))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				this.Save(binaryWriter);
				this.lastUncompressedSize = (int)memoryStream.Length;
				try
				{
					using (BinaryWriter binaryWriter2 = new BinaryWriter(File.Open(filename, FileMode.Create)))
					{
						SaveGame.Header header;
						byte[] saveHeader = SaveGame.Instance.GetSaveHeader(isAutoSave, this.compressSaveData, out header);
						binaryWriter2.Write(header.buildVersion);
						binaryWriter2.Write(header.headerSize);
						binaryWriter2.Write(header.headerVersion);
						binaryWriter2.Write(header.compression);
						binaryWriter2.Write(saveHeader);
						KSerialization.Manager.SerializeDirectory(binaryWriter2);
						if (this.compressSaveData)
						{
							SaveLoader.CompressContents(binaryWriter2, memoryStream.GetBuffer(), (int)memoryStream.Length);
						}
						else
						{
							binaryWriter2.Write(memoryStream.ToArray());
						}
						KCrashReporter.MOST_RECENT_SAVEFILE = filename;
						Stats.Print();
					}
				}
				catch (Exception ex4)
				{
					if (ex4 is UnauthorizedAccessException)
					{
						DebugUtil.LogArgs(new object[]
						{
							"UnauthorizedAccessException for " + filename
						});
						((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format(UI.CRASHSCREEN.SAVEFAILED, "Unauthorized Access Exception"), null, null, null, null, null, null, null, null);
						return SaveLoader.GetActiveSaveFilePath();
					}
					if (ex4 is IOException)
					{
						DebugUtil.LogArgs(new object[]
						{
							"IOException (probably out of disk space) for " + filename
						});
						((ConfirmDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format(UI.CRASHSCREEN.SAVEFAILED, "IOException. You may not have enough free space!"), null, null, null, null, null, null, null, null);
						return SaveLoader.GetActiveSaveFilePath();
					}
					throw ex4;
				}
			}
		}
		if (updateSavePointer)
		{
			SaveLoader.SetActiveSaveFilePath(filename);
		}
		Game.Instance.timelapser.SaveColonyPreview(filename);
		DebugUtil.LogArgs(new object[]
		{
			"Saved to",
			"[" + filename + "]"
		});
		GC.Collect();
		return filename;
	}

	// Token: 0x06007F32 RID: 32562 RVA: 0x0032E730 File Offset: 0x0032C930
	public static SaveGame.GameInfo LoadHeader(string filename, out SaveGame.Header header)
	{
		byte[] array = new byte[512];
		SaveGame.GameInfo header2;
		using (FileStream fileStream = File.OpenRead(filename))
		{
			fileStream.Read(array, 0, 512);
			header2 = SaveGame.GetHeader(new FastReader(array), out header, filename);
		}
		return header2;
	}

	// Token: 0x06007F33 RID: 32563 RVA: 0x0032E788 File Offset: 0x0032C988
	public bool Load(string filename)
	{
		SaveLoader.SetActiveSaveFilePath(filename);
		try
		{
			KSerialization.Manager.Clear();
			byte[] array = File.ReadAllBytes(filename);
			IReader reader = new FastReader(array);
			SaveGame.Header header;
			this.GameInfo = SaveGame.GetHeader(reader, out header, filename);
			ThreadedHttps<KleiMetrics>.Instance.SetExpansionsActive(this.GameInfo.dlcIds);
			DebugUtil.LogArgs(new object[]
			{
				string.Format("Loading save file: {4}\n headerVersion:{0}, buildVersion:{1}, headerSize:{2}, IsCompressed:{3}", new object[]
				{
					header.headerVersion,
					header.buildVersion,
					header.headerSize,
					header.IsCompressed,
					filename
				})
			});
			DebugUtil.LogArgs(new object[]
			{
				string.Format("GameInfo loaded from save header:\n  numberOfCycles:{0},\n  numberOfDuplicants:{1},\n  baseName:{2},\n  isAutoSave:{3},\n  originalSaveName:{4},\n  clusterId:{5},\n  worldTraits:{6},\n  colonyGuid:{7},\n  saveVersion:{8}.{9}", new object[]
				{
					this.GameInfo.numberOfCycles,
					this.GameInfo.numberOfDuplicants,
					this.GameInfo.baseName,
					this.GameInfo.isAutoSave,
					this.GameInfo.originalSaveName,
					this.GameInfo.clusterId,
					(this.GameInfo.worldTraits != null && this.GameInfo.worldTraits.Length != 0) ? string.Join(", ", this.GameInfo.worldTraits) : "<i>none</i>",
					this.GameInfo.colonyGuid,
					this.GameInfo.saveMajorVersion,
					this.GameInfo.saveMinorVersion
				})
			});
			string originalSaveName = this.GameInfo.originalSaveName;
			if (originalSaveName.Contains("/") || originalSaveName.Contains("\\"))
			{
				string originalSaveFileName = SaveLoader.GetOriginalSaveFileName(originalSaveName);
				SaveGame.GameInfo gameInfo = this.GameInfo;
				gameInfo.originalSaveName = originalSaveFileName;
				this.GameInfo = gameInfo;
				global::Debug.Log(string.Concat(new string[]
				{
					"Migration / Save originalSaveName updated from: `",
					originalSaveName,
					"` => `",
					this.GameInfo.originalSaveName,
					"`"
				}));
			}
			if (this.GameInfo.saveMajorVersion == 7 && this.GameInfo.saveMinorVersion < 4)
			{
				Helper.SetTypeInfoMask((SerializationTypeInfo)191);
			}
			KSerialization.Manager.DeserializeDirectory(reader);
			if (header.IsCompressed)
			{
				int num = array.Length - reader.Position;
				byte[] array2 = new byte[num];
				Array.Copy(array, reader.Position, array2, 0, num);
				byte[] array3 = SaveLoader.DecompressContents(array2);
				this.lastUncompressedSize = array3.Length;
				IReader reader2 = new FastReader(array3);
				this.Load(reader2);
			}
			else
			{
				this.lastUncompressedSize = array.Length;
				this.Load(reader);
			}
			KCrashReporter.MOST_RECENT_SAVEFILE = filename;
			if (this.GameInfo.isAutoSave && !string.IsNullOrEmpty(this.GameInfo.originalSaveName))
			{
				string originalSaveFileName2 = SaveLoader.GetOriginalSaveFileName(this.GameInfo.originalSaveName);
				string text;
				if (SaveLoader.IsSaveCloud(filename))
				{
					string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
					if (cloudSavePrefix != null)
					{
						text = System.IO.Path.Combine(cloudSavePrefix, this.GameInfo.baseName, originalSaveFileName2);
					}
					else
					{
						text = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(filename).Replace("auto_save", ""), this.GameInfo.baseName, originalSaveFileName2);
					}
				}
				else
				{
					text = System.IO.Path.Combine(SaveLoader.GetSavePrefix(), this.GameInfo.baseName, originalSaveFileName2);
				}
				if (text != null)
				{
					SaveLoader.SetActiveSaveFilePath(text);
				}
			}
		}
		catch (Exception ex)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"\n--- Error loading save ---\n" + ex.Message + "\n" + ex.StackTrace
			});
			Sim.Shutdown();
			SaveLoader.SetActiveSaveFilePath(null);
			return false;
		}
		Stats.Print();
		DebugUtil.LogArgs(new object[]
		{
			"Loaded",
			"[" + filename + "]"
		});
		DebugUtil.LogArgs(new object[]
		{
			"World Seeds",
			string.Concat(new string[]
			{
				"[",
				this.clusterDetailSave.globalWorldSeed.ToString(),
				"/",
				this.clusterDetailSave.globalWorldLayoutSeed.ToString(),
				"/",
				this.clusterDetailSave.globalTerrainSeed.ToString(),
				"/",
				this.clusterDetailSave.globalNoiseSeed.ToString(),
				"]"
			})
		});
		GC.Collect();
		return true;
	}

	// Token: 0x06007F34 RID: 32564 RVA: 0x0032EC1C File Offset: 0x0032CE1C
	public bool LoadFromWorldGen()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Attempting to start a new game with current world gen"
		});
		WorldGen.LoadSettings(false);
		FastReader reader = new FastReader(File.ReadAllBytes(WorldGen.WORLDGEN_SAVE_FILENAME));
		this.m_cluster = Cluster.Load(reader);
		ListPool<SimSaveFileStructure, SaveLoader>.PooledList pooledList = ListPool<SimSaveFileStructure, SaveLoader>.Allocate();
		this.m_cluster.LoadClusterSim(pooledList, reader);
		SaveGame.GameInfo gameInfo = this.GameInfo;
		gameInfo.clusterId = this.m_cluster.Id;
		gameInfo.colonyGuid = Guid.NewGuid();
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		gameInfo.dlcIds = new List<string>(currentClusterLayout.requiredDlcIds);
		foreach (string item in CustomGameSettings.Instance.GetCurrentDlcMixingIds())
		{
			if (!gameInfo.dlcIds.Contains(item))
			{
				gameInfo.dlcIds.Add(item);
			}
		}
		this.GameInfo = gameInfo;
		ThreadedHttps<KleiMetrics>.Instance.SetExpansionsActive(this.GameInfo.dlcIds);
		if (pooledList.Count != this.m_cluster.worlds.Count)
		{
			global::Debug.LogError("Attempt failed. Failed to load all worlds.");
			pooledList.Recycle();
			return false;
		}
		GridSettings.Reset(this.m_cluster.size.x, this.m_cluster.size.y);
		if (Application.isPlaying)
		{
			Singleton<KBatchedAnimUpdater>.Instance.InitializeGrid();
		}
		this.clusterDetailSave = new WorldDetailSave();
		foreach (SimSaveFileStructure simSaveFileStructure in pooledList)
		{
			this.clusterDetailSave.globalNoiseSeed = simSaveFileStructure.worldDetail.globalNoiseSeed;
			this.clusterDetailSave.globalTerrainSeed = simSaveFileStructure.worldDetail.globalTerrainSeed;
			this.clusterDetailSave.globalWorldLayoutSeed = simSaveFileStructure.worldDetail.globalWorldLayoutSeed;
			this.clusterDetailSave.globalWorldSeed = simSaveFileStructure.worldDetail.globalWorldSeed;
			Vector2 b = Grid.CellToPos2D(Grid.PosToCell(new Vector2I(simSaveFileStructure.x, simSaveFileStructure.y)));
			foreach (WorldDetailSave.OverworldCell overworldCell in simSaveFileStructure.worldDetail.overworldCells)
			{
				for (int num = 0; num != overworldCell.poly.Vertices.Count; num++)
				{
					List<Vector2> vertices = overworldCell.poly.Vertices;
					int index = num;
					vertices[index] += b;
				}
				overworldCell.poly.RefreshBounds();
			}
			this.clusterDetailSave.overworldCells.AddRange(simSaveFileStructure.worldDetail.overworldCells);
		}
		Sim.SIM_Initialize(new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler));
		SimMessages.CreateSimElementsTable(ElementLoader.elements);
		Sim.AllocateCells(this.m_cluster.size.x, this.m_cluster.size.y, false);
		SimMessages.DefineWorldOffsets((from world in this.m_cluster.worlds
		select new SimMessages.WorldOffsetData
		{
			worldOffsetX = world.WorldOffset.x,
			worldOffsetY = world.WorldOffset.y,
			worldSizeX = world.WorldSize.x,
			worldSizeY = world.WorldSize.y
		}).ToList<SimMessages.WorldOffsetData>());
		SimMessages.CreateDiseaseTable(Db.Get().Diseases);
		Sim.HandleMessage(SimMessageHashes.ClearUnoccupiedCells, 0, null);
		try
		{
			foreach (SimSaveFileStructure simSaveFileStructure2 in pooledList)
			{
				FastReader reader2 = new FastReader(simSaveFileStructure2.Sim);
				if (Sim.Load(reader2) != 0)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"\n--- Error loading save ---\nSimDLL found bad data\n"
					});
					Sim.Shutdown();
					pooledList.Recycle();
					return false;
				}
			}
		}
		catch (Exception ex)
		{
			global::Debug.LogWarning("--- Error loading Sim FROM NEW WORLDGEN ---" + ex.Message + "\n" + ex.StackTrace);
			Sim.Shutdown();
			pooledList.Recycle();
			return false;
		}
		global::Debug.Log("Attempt success");
		Sim.Start();
		SceneInitializer.Instance.PostLoadPrefabs();
		SceneInitializer.Instance.NewSaveGamePrefab();
		this.cachedGSD = this.m_cluster.currentWorld.SpawnData;
		this.OnWorldGenComplete.Signal(this.m_cluster);
		OniMetrics.LogEvent(OniMetrics.Event.NewSave, "NewGame", true);
		StoryManager.Instance.InitialSaveSetup();
		ThreadedHttps<KleiMetrics>.Instance.IncrementGameCount();
		OniMetrics.SendEvent(OniMetrics.Event.NewSave, "New Save");
		pooledList.Recycle();
		return true;
	}

	// Token: 0x17000823 RID: 2083
	// (get) Token: 0x06007F35 RID: 32565 RVA: 0x000F3DB0 File Offset: 0x000F1FB0
	// (set) Token: 0x06007F36 RID: 32566 RVA: 0x000F3DB8 File Offset: 0x000F1FB8
	public GameSpawnData cachedGSD { get; private set; }

	// Token: 0x17000824 RID: 2084
	// (get) Token: 0x06007F37 RID: 32567 RVA: 0x000F3DC1 File Offset: 0x000F1FC1
	// (set) Token: 0x06007F38 RID: 32568 RVA: 0x000F3DC9 File Offset: 0x000F1FC9
	public WorldDetailSave clusterDetailSave { get; private set; }

	// Token: 0x06007F39 RID: 32569 RVA: 0x000F3DD2 File Offset: 0x000F1FD2
	public void SetWorldDetail(WorldDetailSave worldDetail)
	{
		this.clusterDetailSave = worldDetail;
	}

	// Token: 0x06007F3A RID: 32570 RVA: 0x0032F120 File Offset: 0x0032D320
	private void ReportSaveMetrics(bool is_auto_save)
	{
		if (ThreadedHttps<KleiMetrics>.Instance == null || !ThreadedHttps<KleiMetrics>.Instance.enabled || this.saveManager == null)
		{
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary[GameClock.NewCycleKey] = GameClock.Instance.GetCycle() + 1;
		dictionary["IsAutoSave"] = is_auto_save;
		dictionary["SavedPrefabs"] = this.GetSavedPrefabMetrics();
		dictionary["ResourcesAccessible"] = this.GetWorldInventoryMetrics();
		dictionary["MinionMetrics"] = this.GetMinionMetrics();
		dictionary["WorldMetrics"] = this.GetWorldMetrics();
		if (is_auto_save)
		{
			dictionary["DailyReport"] = this.GetDailyReportMetrics();
			dictionary["PerformanceMeasurements"] = this.GetPerformanceMeasurements();
			dictionary["AverageFrameTime"] = this.GetFrameTime();
		}
		dictionary["CustomGameSettings"] = CustomGameSettings.Instance.GetSettingsForMetrics();
		dictionary["CustomMixingSettings"] = CustomGameSettings.Instance.GetSettingsForMixingMetrics();
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(dictionary, "ReportSaveMetrics");
	}

	// Token: 0x06007F3B RID: 32571 RVA: 0x0032F23C File Offset: 0x0032D43C
	private List<SaveLoader.MinionMetricsData> GetMinionMetrics()
	{
		List<SaveLoader.MinionMetricsData> list = new List<SaveLoader.MinionMetricsData>();
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			if (!(minionIdentity == null))
			{
				Amounts amounts = minionIdentity.gameObject.GetComponent<Modifiers>().amounts;
				List<SaveLoader.MinionAttrFloatData> list2 = new List<SaveLoader.MinionAttrFloatData>(amounts.Count);
				foreach (AmountInstance amountInstance in amounts)
				{
					float value = amountInstance.value;
					if (!float.IsNaN(value) && !float.IsInfinity(value))
					{
						list2.Add(new SaveLoader.MinionAttrFloatData
						{
							Name = amountInstance.modifier.Id,
							Value = amountInstance.value
						});
					}
				}
				MinionResume component = minionIdentity.gameObject.GetComponent<MinionResume>();
				float totalExperienceGained = component.TotalExperienceGained;
				List<string> list3 = new List<string>();
				foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
				{
					if (keyValuePair.Value)
					{
						list3.Add(keyValuePair.Key);
					}
				}
				list.Add(new SaveLoader.MinionMetricsData
				{
					Name = minionIdentity.name,
					Modifiers = list2,
					TotalExperienceGained = totalExperienceGained,
					Skills = list3
				});
			}
		}
		return list;
	}

	// Token: 0x06007F3C RID: 32572 RVA: 0x0032F40C File Offset: 0x0032D60C
	private List<SaveLoader.SavedPrefabMetricsData> GetSavedPrefabMetrics()
	{
		Dictionary<Tag, List<SaveLoadRoot>> lists = this.saveManager.GetLists();
		List<SaveLoader.SavedPrefabMetricsData> list = new List<SaveLoader.SavedPrefabMetricsData>(lists.Count);
		foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in lists)
		{
			Tag key = keyValuePair.Key;
			List<SaveLoadRoot> value = keyValuePair.Value;
			if (value.Count > 0)
			{
				list.Add(new SaveLoader.SavedPrefabMetricsData
				{
					PrefabName = key.ToString(),
					Count = value.Count
				});
			}
		}
		return list;
	}

	// Token: 0x06007F3D RID: 32573 RVA: 0x0032F4B8 File Offset: 0x0032D6B8
	private List<SaveLoader.WorldInventoryMetricsData> GetWorldInventoryMetrics()
	{
		Dictionary<Tag, float> allWorldsAccessibleAmounts = ClusterManager.Instance.GetAllWorldsAccessibleAmounts();
		List<SaveLoader.WorldInventoryMetricsData> list = new List<SaveLoader.WorldInventoryMetricsData>(allWorldsAccessibleAmounts.Count);
		foreach (KeyValuePair<Tag, float> keyValuePair in allWorldsAccessibleAmounts)
		{
			float value = keyValuePair.Value;
			if (!float.IsInfinity(value) && !float.IsNaN(value))
			{
				list.Add(new SaveLoader.WorldInventoryMetricsData
				{
					Name = keyValuePair.Key.ToString(),
					Amount = value
				});
			}
		}
		return list;
	}

	// Token: 0x06007F3E RID: 32574 RVA: 0x0032F564 File Offset: 0x0032D764
	private List<SaveLoader.DailyReportMetricsData> GetDailyReportMetrics()
	{
		List<SaveLoader.DailyReportMetricsData> list = new List<SaveLoader.DailyReportMetricsData>();
		int cycle = GameClock.Instance.GetCycle();
		ReportManager.DailyReport dailyReport = ReportManager.Instance.FindReport(cycle);
		if (dailyReport != null)
		{
			foreach (ReportManager.ReportEntry reportEntry in dailyReport.reportEntries)
			{
				SaveLoader.DailyReportMetricsData item = default(SaveLoader.DailyReportMetricsData);
				item.Name = reportEntry.reportType.ToString();
				if (!float.IsInfinity(reportEntry.Net) && !float.IsNaN(reportEntry.Net))
				{
					item.Net = new float?(reportEntry.Net);
				}
				if (SaveLoader.force_infinity)
				{
					item.Net = null;
				}
				if (!float.IsInfinity(reportEntry.Positive) && !float.IsNaN(reportEntry.Positive))
				{
					item.Positive = new float?(reportEntry.Positive);
				}
				if (!float.IsInfinity(reportEntry.Negative) && !float.IsNaN(reportEntry.Negative))
				{
					item.Negative = new float?(reportEntry.Negative);
				}
				list.Add(item);
			}
			list.Add(new SaveLoader.DailyReportMetricsData
			{
				Name = "MinionCount",
				Net = new float?((float)Components.LiveMinionIdentities.Count),
				Positive = new float?(0f),
				Negative = new float?(0f)
			});
		}
		return list;
	}

	// Token: 0x06007F3F RID: 32575 RVA: 0x0032F6FC File Offset: 0x0032D8FC
	private List<SaveLoader.PerformanceMeasurement> GetPerformanceMeasurements()
	{
		List<SaveLoader.PerformanceMeasurement> list = new List<SaveLoader.PerformanceMeasurement>();
		if (Global.Instance != null)
		{
			PerformanceMonitor component = Global.Instance.GetComponent<PerformanceMonitor>();
			list.Add(new SaveLoader.PerformanceMeasurement
			{
				name = "FramesAbove30",
				value = component.NumFramesAbove30
			});
			list.Add(new SaveLoader.PerformanceMeasurement
			{
				name = "FramesBelow30",
				value = component.NumFramesBelow30
			});
			component.Reset();
		}
		return list;
	}

	// Token: 0x06007F40 RID: 32576 RVA: 0x0032F784 File Offset: 0x0032D984
	private float GetFrameTime()
	{
		PerformanceMonitor component = Global.Instance.GetComponent<PerformanceMonitor>();
		DebugUtil.LogArgs(new object[]
		{
			"Average frame time:",
			1f / component.FPS
		});
		return 1f / component.FPS;
	}

	// Token: 0x06007F41 RID: 32577 RVA: 0x0032F7D0 File Offset: 0x0032D9D0
	private List<SaveLoader.WorldMetricsData> GetWorldMetrics()
	{
		List<SaveLoader.WorldMetricsData> list = new List<SaveLoader.WorldMetricsData>();
		if (Global.Instance != null)
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (!worldContainer.IsModuleInterior)
				{
					float discoveryTimestamp = worldContainer.IsDiscovered ? worldContainer.DiscoveryTimestamp : -1f;
					float dupeVisitedTimestamp = worldContainer.IsDupeVisited ? worldContainer.DupeVisitedTimestamp : -1f;
					list.Add(new SaveLoader.WorldMetricsData
					{
						Name = worldContainer.worldName,
						DiscoveryTimestamp = discoveryTimestamp,
						DupeVisitedTimestamp = dupeVisitedTimestamp
					});
				}
			}
		}
		return list;
	}

	// Token: 0x06007F42 RID: 32578 RVA: 0x000F3DDB File Offset: 0x000F1FDB
	public bool IsDLCActiveForCurrentSave(string dlcid)
	{
		return DlcManager.IsContentSubscribed(dlcid) && (dlcid == "" || dlcid == "" || this.GameInfo.dlcIds.Contains(dlcid));
	}

	// Token: 0x06007F43 RID: 32579 RVA: 0x0032F894 File Offset: 0x0032DA94
	public bool IsDlcListActiveForCurrentSave(string[] dlcIds)
	{
		if (dlcIds == null || dlcIds.Length == 0)
		{
			return true;
		}
		foreach (string text in dlcIds)
		{
			if (text == "")
			{
				return true;
			}
			if (this.IsDLCActiveForCurrentSave(text))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06007F44 RID: 32580 RVA: 0x0032F8DC File Offset: 0x0032DADC
	public bool IsAllDlcActiveForCurrentSave(string[] dlcIds)
	{
		if (dlcIds == null || dlcIds.Length == 0)
		{
			return true;
		}
		foreach (string text in dlcIds)
		{
			if (!(text == "") && !this.IsDLCActiveForCurrentSave(text))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06007F45 RID: 32581 RVA: 0x0032F920 File Offset: 0x0032DB20
	public bool IsAnyDlcActiveForCurrentSave(string[] dlcIds)
	{
		if (dlcIds == null || dlcIds.Length == 0)
		{
			return false;
		}
		foreach (string text in dlcIds)
		{
			if (!(text == "") && this.IsDLCActiveForCurrentSave(text))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06007F46 RID: 32582 RVA: 0x000F3E14 File Offset: 0x000F2014
	public bool IsCorrectDlcActiveForCurrentSave(string[] required, string[] forbidden)
	{
		return this.IsAllDlcActiveForCurrentSave(required) && !this.IsAnyDlcActiveForCurrentSave(forbidden);
	}

	// Token: 0x06007F47 RID: 32583 RVA: 0x0032F964 File Offset: 0x0032DB64
	public string GetSaveLoadContentLetters()
	{
		if (this.GameInfo.dlcIds.Count <= 0)
		{
			return "V";
		}
		string text = "";
		foreach (string dlcId in this.GameInfo.dlcIds)
		{
			text += DlcManager.GetContentLetter(dlcId);
		}
		return text;
	}

	// Token: 0x06007F48 RID: 32584 RVA: 0x0032F9E4 File Offset: 0x0032DBE4
	public void UpgradeActiveSaveDLCInfo(string dlcId, bool trigger_load = false)
	{
		string activeSaveFolder = SaveLoader.GetActiveSaveFolder();
		string path = SaveGame.Instance.BaseName + UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.BACKUP_SAVE_GAME_APPEND + ".sav";
		string filename = System.IO.Path.Combine(activeSaveFolder, path);
		this.Save(filename, false, false);
		if (!this.GameInfo.dlcIds.Contains(dlcId))
		{
			this.GameInfo.dlcIds.Add(dlcId);
		}
		string current_save = SaveLoader.GetActiveSaveFilePath();
		this.Save(SaveLoader.GetActiveSaveFilePath(), false, false);
		if (trigger_load)
		{
			LoadingOverlay.Load(delegate
			{
				LoadScreen.DoLoad(current_save);
			});
		}
	}

	// Token: 0x0400604E RID: 24654
	[MyCmpGet]
	private GridSettings gridSettings;

	// Token: 0x04006050 RID: 24656
	private bool saveFileCorrupt;

	// Token: 0x04006051 RID: 24657
	private bool compressSaveData = true;

	// Token: 0x04006052 RID: 24658
	private int lastUncompressedSize;

	// Token: 0x04006053 RID: 24659
	public bool saveAsText;

	// Token: 0x04006054 RID: 24660
	public const string MAINMENU_LEVELNAME = "launchscene";

	// Token: 0x04006055 RID: 24661
	public const string FRONTEND_LEVELNAME = "frontend";

	// Token: 0x04006056 RID: 24662
	public const string BACKEND_LEVELNAME = "backend";

	// Token: 0x04006057 RID: 24663
	public const string SAVE_EXTENSION = ".sav";

	// Token: 0x04006058 RID: 24664
	public const string AUTOSAVE_FOLDER = "auto_save";

	// Token: 0x04006059 RID: 24665
	public const string CLOUDSAVE_FOLDER = "cloud_save_files";

	// Token: 0x0400605A RID: 24666
	public const string SAVE_FOLDER = "save_files";

	// Token: 0x0400605B RID: 24667
	public const int MAX_AUTOSAVE_FILES = 10;

	// Token: 0x0400605D RID: 24669
	[NonSerialized]
	public SaveManager saveManager;

	// Token: 0x0400605F RID: 24671
	private Cluster m_cluster;

	// Token: 0x04006060 RID: 24672
	private ClusterLayout m_clusterLayout;

	// Token: 0x04006062 RID: 24674
	private const string CorruptFileSuffix = "_";

	// Token: 0x04006063 RID: 24675
	private const float SAVE_BUFFER_HEAD_ROOM = 0.1f;

	// Token: 0x04006064 RID: 24676
	private bool mustRestartOnFail;

	// Token: 0x04006067 RID: 24679
	public const string METRIC_SAVED_PREFAB_KEY = "SavedPrefabs";

	// Token: 0x04006068 RID: 24680
	public const string METRIC_IS_AUTO_SAVE_KEY = "IsAutoSave";

	// Token: 0x04006069 RID: 24681
	public const string METRIC_WAS_DEBUG_EVER_USED = "WasDebugEverUsed";

	// Token: 0x0400606A RID: 24682
	public const string METRIC_IS_SANDBOX_ENABLED = "IsSandboxEnabled";

	// Token: 0x0400606B RID: 24683
	public const string METRIC_RESOURCES_ACCESSIBLE_KEY = "ResourcesAccessible";

	// Token: 0x0400606C RID: 24684
	public const string METRIC_DAILY_REPORT_KEY = "DailyReport";

	// Token: 0x0400606D RID: 24685
	public const string METRIC_WORLD_METRICS_KEY = "WorldMetrics";

	// Token: 0x0400606E RID: 24686
	public const string METRIC_MINION_METRICS_KEY = "MinionMetrics";

	// Token: 0x0400606F RID: 24687
	public const string METRIC_CUSTOM_GAME_SETTINGS = "CustomGameSettings";

	// Token: 0x04006070 RID: 24688
	public const string METRIC_CUSTOM_MIXING_SETTINGS = "CustomMixingSettings";

	// Token: 0x04006071 RID: 24689
	public const string METRIC_PERFORMANCE_MEASUREMENTS = "PerformanceMeasurements";

	// Token: 0x04006072 RID: 24690
	public const string METRIC_FRAME_TIME = "AverageFrameTime";

	// Token: 0x04006073 RID: 24691
	private static bool force_infinity;

	// Token: 0x0200180D RID: 6157
	public class FlowUtilityNetworkInstance
	{
		// Token: 0x04006074 RID: 24692
		public int id = -1;

		// Token: 0x04006075 RID: 24693
		public SimHashes containedElement = SimHashes.Vacuum;

		// Token: 0x04006076 RID: 24694
		public float containedMass;

		// Token: 0x04006077 RID: 24695
		public float containedTemperature;
	}

	// Token: 0x0200180E RID: 6158
	[SerializationConfig(KSerialization.MemberSerialization.OptOut)]
	public class FlowUtilityNetworkSaver : ISaveLoadable
	{
		// Token: 0x06007F4B RID: 32587 RVA: 0x000F3E54 File Offset: 0x000F2054
		public FlowUtilityNetworkSaver()
		{
			this.gas = new List<SaveLoader.FlowUtilityNetworkInstance>();
			this.liquid = new List<SaveLoader.FlowUtilityNetworkInstance>();
		}

		// Token: 0x04006078 RID: 24696
		public List<SaveLoader.FlowUtilityNetworkInstance> gas;

		// Token: 0x04006079 RID: 24697
		public List<SaveLoader.FlowUtilityNetworkInstance> liquid;
	}

	// Token: 0x0200180F RID: 6159
	public struct SaveFileEntry
	{
		// Token: 0x0400607A RID: 24698
		public string path;

		// Token: 0x0400607B RID: 24699
		public System.DateTime timeStamp;
	}

	// Token: 0x02001810 RID: 6160
	public enum SaveType
	{
		// Token: 0x0400607D RID: 24701
		local,
		// Token: 0x0400607E RID: 24702
		cloud,
		// Token: 0x0400607F RID: 24703
		both
	}

	// Token: 0x02001811 RID: 6161
	private struct MinionAttrFloatData
	{
		// Token: 0x04006080 RID: 24704
		public string Name;

		// Token: 0x04006081 RID: 24705
		public float Value;
	}

	// Token: 0x02001812 RID: 6162
	private struct MinionMetricsData
	{
		// Token: 0x04006082 RID: 24706
		public string Name;

		// Token: 0x04006083 RID: 24707
		public List<SaveLoader.MinionAttrFloatData> Modifiers;

		// Token: 0x04006084 RID: 24708
		public float TotalExperienceGained;

		// Token: 0x04006085 RID: 24709
		public List<string> Skills;
	}

	// Token: 0x02001813 RID: 6163
	private struct SavedPrefabMetricsData
	{
		// Token: 0x04006086 RID: 24710
		public string PrefabName;

		// Token: 0x04006087 RID: 24711
		public int Count;
	}

	// Token: 0x02001814 RID: 6164
	private struct WorldInventoryMetricsData
	{
		// Token: 0x04006088 RID: 24712
		public string Name;

		// Token: 0x04006089 RID: 24713
		public float Amount;
	}

	// Token: 0x02001815 RID: 6165
	private struct DailyReportMetricsData
	{
		// Token: 0x0400608A RID: 24714
		public string Name;

		// Token: 0x0400608B RID: 24715
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public float? Net;

		// Token: 0x0400608C RID: 24716
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public float? Positive;

		// Token: 0x0400608D RID: 24717
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public float? Negative;
	}

	// Token: 0x02001816 RID: 6166
	private struct PerformanceMeasurement
	{
		// Token: 0x0400608E RID: 24718
		public string name;

		// Token: 0x0400608F RID: 24719
		public float value;
	}

	// Token: 0x02001817 RID: 6167
	private struct WorldMetricsData
	{
		// Token: 0x04006090 RID: 24720
		public string Name;

		// Token: 0x04006091 RID: 24721
		public float DiscoveryTimestamp;

		// Token: 0x04006092 RID: 24722
		public float DupeVisitedTimestamp;
	}
}
