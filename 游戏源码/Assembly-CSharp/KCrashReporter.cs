using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Klei;
using KMod;
using Newtonsoft.Json;
using Steamworks;
using STRINGS;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class KCrashReporter : MonoBehaviour
{
	public class CRASH_CATEGORY
	{
		public static string DEVNOTIFICATION = "DevNotification";

		public static string VANILLA = "Vanilla";

		public static string SPACEDOUT = "SpacedOut";

		public static string MODDED = "Modded";

		public static string DEBUGUSED = "DebugUsed";

		public static string SANDBOX = "Sandbox";

		public static string STEAMDECK = "SteamDeck";

		public static string SIM = "SimDll";

		public static string FILEIO = "FileIO";

		public static string MODSYSTEM = "ModSystem";

		public static string WORLDGENFAILURE = "WorldgenFailure";
	}

	private class Error
	{
		public string game = "ONI";

		public string userName;

		public string platform;

		public string version = LaunchInitializer.BuildPrefix();

		public string branch = "default";

		public string sku = "";

		public int build = 623711;

		public string callstack = "";

		public string fullstack = "";

		public string summaryline = "";

		public string userMessage = "";

		public List<string> categories = new List<string>();

		public string slackSummary;

		public string logFilename = "Player.log";

		public string saveFilename = "";

		public string screenshotFilename = "";

		public List<string> extraFilenames = new List<string>();

		public string title = "";

		public bool isServer;

		public bool isDedicated;

		public bool isError = true;

		public string emote = "";

		public Error()
		{
			userName = GetUserID();
			platform = Util.GetOperatingSystem();
			InitDefaultCategories();
			InitSku();
			InitSlackSummary();
			if (DistributionPlatform.Inst.Initialized)
			{
				string pchName;
				bool num = !SteamApps.GetCurrentBetaName(out pchName, 100);
				branch = pchName;
				if (pchName == "public_playtest")
				{
					branch = "public_testing";
				}
				if (num || (pchName == "public_testing" && !UnityEngine.Debug.isDebugBuild))
				{
					branch = "default";
				}
			}
		}

		private void InitDefaultCategories()
		{
			if (DlcManager.IsPureVanilla())
			{
				categories.Add(CRASH_CATEGORY.VANILLA);
			}
			if (DlcManager.IsExpansion1Active())
			{
				categories.Add(CRASH_CATEGORY.SPACEDOUT);
			}
			foreach (string activeDLCId in DlcManager.GetActiveDLCIds())
			{
				if (!(activeDLCId == "EXPANSION1_ID"))
				{
					categories.Add(activeDLCId);
				}
			}
			if (debugWasUsed)
			{
				categories.Add(CRASH_CATEGORY.DEBUGUSED);
			}
			if (haveActiveMods)
			{
				categories.Add(CRASH_CATEGORY.MODDED);
			}
			if (SaveGame.Instance != null && SaveGame.Instance.sandboxEnabled)
			{
				categories.Add(CRASH_CATEGORY.SANDBOX);
			}
			if (DistributionPlatform.Inst.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
			{
				categories.Add(CRASH_CATEGORY.STEAMDECK);
			}
		}

		private void InitSku()
		{
			sku = "steam";
			if (!DistributionPlatform.Inst.Initialized)
			{
				return;
			}
			string pchName;
			bool num = !SteamApps.GetCurrentBetaName(out pchName, 100);
			switch (pchName)
			{
			case "public_testing":
			case "preview":
			case "public_playtest":
			case "playtest":
				if (UnityEngine.Debug.isDebugBuild)
				{
					sku = "steam-public-testing";
				}
				else
				{
					sku = "steam-release";
				}
				break;
			}
			if (num || pchName == "release")
			{
				sku = "steam-release";
			}
		}

		private void InitSlackSummary()
		{
			string buildText = BuildWatermark.GetBuildText();
			string text = ((GameClock.Instance != null) ? $" - Cycle {GameClock.Instance.GetCycle() + 1}" : "");
			int num = ((Global.Instance != null && Global.Instance.modManager != null) ? Global.Instance.modManager.mods.Count((Mod x) => x.IsEnabledForActiveDlc()) : 0);
			string text2 = ((num > 0) ? $" - {num} active mods" : "");
			slackSummary = buildText + " " + platform + text + text2;
		}
	}

	public class PendingCrash
	{
		public string jsonString;

		public byte[] archiveData;

		public System.Action successCallback;

		public Action<long> failureCallback;
	}

	public static string MOST_RECENT_SAVEFILE = null;

	public const string CRASH_REPORTER_SERVER = "https://games-feedback.klei.com";

	public const uint MAX_LOGS = 10000000u;

	public static bool ignoreAll = false;

	public static bool debugWasUsed = false;

	public static bool haveActiveMods = false;

	public static uint logCount = 0u;

	public static string error_canvas_name = "ErrorCanvas";

	public static bool disableDeduping = false;

	public static bool hasCrash = false;

	private static readonly Regex failedToLoadModuleRegEx = new Regex("^Failed to load '(.*?)' with error (.*)", RegexOptions.Multiline);

	[SerializeField]
	private LoadScreen loadScreenPrefab;

	[SerializeField]
	private GameObject reportErrorPrefab;

	[SerializeField]
	private ConfirmDialogScreen confirmDialogPrefab;

	private GameObject errorScreen;

	public static bool terminateOnError = true;

	private static string dataRoot;

	private static readonly string[] IgnoreStrings = new string[5] { "Releasing render texture whose render buffer is set as Camera's target buffer with Camera.SetTargetBuffers!", "The profiler has run out of samples for this frame. This frame will be skipped. Increase the sample limit using Profiler.maxNumberOfSamplesPerFrame", "Trying to add Text (LocText) for graphic rebuild while we are already inside a graphic rebuild loop. This is not supported.", "Texture has out of range width / height", "<I> Failed to get cursor position:\r\nSuccess.\r\n" };

	private static HashSet<int> previouslyReportedDevNotifications;

	private static PendingCrash pendingCrash;

	public static bool hasReportedError { get; private set; }

	public static event Action<bool> onCrashReported;

	public static event Action<float> onCrashUploadProgress;

	private void OnEnable()
	{
		dataRoot = Application.dataPath;
		Application.logMessageReceived += HandleLog;
		ignoreAll = true;
		string path = Path.Combine(dataRoot, "hashes.json");
		if (File.Exists(path))
		{
			StringBuilder stringBuilder = new StringBuilder();
			MD5 mD = MD5.Create();
			Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
			if (dictionary.Count > 0)
			{
				bool flag = true;
				foreach (KeyValuePair<string, string> item in dictionary)
				{
					string key = item.Key;
					string value = item.Value;
					stringBuilder.Length = 0;
					using FileStream inputStream = new FileStream(Path.Combine(dataRoot, key), FileMode.Open, FileAccess.Read);
					byte[] array = mD.ComputeHash(inputStream);
					foreach (byte b in array)
					{
						stringBuilder.AppendFormat("{0:x2}", b);
					}
					if (stringBuilder.ToString() != value)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					ignoreAll = false;
				}
			}
			else
			{
				ignoreAll = false;
			}
		}
		else
		{
			ignoreAll = false;
		}
		if (ignoreAll)
		{
			Debug.Log("Ignoring crash due to mismatched hashes.json entries.");
		}
		if (File.Exists("ignorekcrashreporter.txt"))
		{
			ignoreAll = true;
			Debug.Log("Ignoring crash due to ignorekcrashreporter.txt");
		}
		if (Application.isEditor && !GenericGameSettings.instance.enableEditorCrashReporting)
		{
			terminateOnError = false;
		}
	}

	private void OnDisable()
	{
		Application.logMessageReceived -= HandleLog;
	}

	private void HandleLog(string msg, string stack_trace, LogType type)
	{
		if (++logCount == 10000000)
		{
			DebugUtil.DevLogError("Turning off logging to avoid increasing the file to an unreasonable size, please review the logs as they probably contain spam");
			Debug.DisableLogging();
		}
		if (ignoreAll)
		{
			return;
		}
		if (msg != null && msg.StartsWith(DebugUtil.START_CALLSTACK))
		{
			string text = msg;
			msg = text.Substring(text.IndexOf(DebugUtil.END_CALLSTACK, StringComparison.Ordinal) + DebugUtil.END_CALLSTACK.Length);
			stack_trace = text.Substring(DebugUtil.START_CALLSTACK.Length, text.IndexOf(DebugUtil.END_CALLSTACK, StringComparison.Ordinal) - DebugUtil.START_CALLSTACK.Length);
		}
		if (Array.IndexOf(IgnoreStrings, msg) != -1 || (msg != null && msg.StartsWith("<RI.Hid>")) || (msg != null && msg.StartsWith("Failed to load cursor")) || (msg != null && msg.StartsWith("Failed to save a temporary cursor")))
		{
			return;
		}
		if (type == LogType.Exception)
		{
			RestartWarning.ShouldWarn = true;
		}
		if (!(errorScreen == null) || (type != LogType.Exception && type != 0) || (terminateOnError && hasCrash))
		{
			return;
		}
		if (SpeedControlScreen.Instance != null)
		{
			SpeedControlScreen.Instance.Pause(playSound: true, isCrashed: true);
		}
		string text2 = msg;
		string text3 = stack_trace;
		if (string.IsNullOrEmpty(text3))
		{
			text3 = new StackTrace(5, fNeedFileInfo: true).ToString();
		}
		if (App.isLoading)
		{
			if (!SceneInitializerLoader.deferred_error.IsValid)
			{
				SceneInitializerLoader.DeferredError deferred_error = default(SceneInitializerLoader.DeferredError);
				deferred_error.msg = text2;
				deferred_error.stack_trace = text3;
				SceneInitializerLoader.deferred_error = deferred_error;
			}
		}
		else
		{
			ShowDialog(text2, text3);
		}
	}

	public bool ShowDialog(string error, string stack_trace)
	{
		if (errorScreen != null)
		{
			return false;
		}
		GameObject gameObject = GameObject.Find(error_canvas_name);
		if (gameObject == null)
		{
			gameObject = new GameObject();
			gameObject.name = error_canvas_name;
			Canvas canvas = gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;
			canvas.sortingOrder = 32767;
			gameObject.AddComponent<GraphicRaycaster>();
		}
		errorScreen = UnityEngine.Object.Instantiate(reportErrorPrefab, Vector3.zero, Quaternion.identity);
		errorScreen.transform.SetParent(gameObject.transform, worldPositionStays: false);
		ReportErrorDialog errorDialog = errorScreen.GetComponentInChildren<ReportErrorDialog>();
		string stackTrace = error + "\n\n" + stack_trace;
		hasCrash = true;
		if (Global.Instance != null && Global.Instance.modManager != null && Global.Instance.modManager.HasCrashableMods())
		{
			Exception ex = DebugUtil.RetrieveLastExceptionLogged();
			StackTrace stackTrace2 = ((ex != null) ? new StackTrace(ex) : new StackTrace(5, fNeedFileInfo: true));
			Global.Instance.modManager.SearchForModsInStackTrace(stackTrace2);
			Global.Instance.modManager.SearchForModsInStackTrace(stack_trace);
			errorDialog.PopupDisableModsDialog(stackTrace, OnQuitToDesktop, (Global.Instance.modManager.IsInDevMode() || !terminateOnError) ? new System.Action(OnCloseErrorDialog) : null);
		}
		else
		{
			errorDialog.PopupSubmitErrorDialog(stackTrace, delegate
			{
				ReportError(error, stack_trace, confirmDialogPrefab, errorScreen, errorDialog.UserMessage());
			}, OnQuitToDesktop, terminateOnError ? null : new System.Action(OnCloseErrorDialog));
		}
		return true;
	}

	private void OnCloseErrorDialog()
	{
		UnityEngine.Object.Destroy(errorScreen);
		errorScreen = null;
		hasCrash = false;
		if (SpeedControlScreen.Instance != null)
		{
			SpeedControlScreen.Instance.Unpause();
		}
	}

	private void OnQuitToDesktop()
	{
		App.Quit();
	}

	private static string GetUserID()
	{
		if (DistributionPlatform.Initialized)
		{
			return DistributionPlatform.Inst.Name + "ID_" + DistributionPlatform.Inst.LocalUser.Name + "_" + DistributionPlatform.Inst.LocalUser.Id;
		}
		return "LocalUser_" + Environment.UserName;
	}

	private static string GetLogContents()
	{
		string path = Util.LogFilePath();
		if (File.Exists(path))
		{
			using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using StreamReader streamReader = new StreamReader(stream);
				return streamReader.ReadToEnd();
			}
		}
		return "";
	}

	public static void ReportDevNotification(string notification_name, string stack_trace, string details = "", bool includeSaveFile = false, string[] extraCategories = null)
	{
		if (previouslyReportedDevNotifications == null)
		{
			previouslyReportedDevNotifications = new HashSet<int>();
		}
		details = notification_name + " - " + details;
		Debug.Log(details);
		int hashValue = new HashedString(notification_name).HashValue;
		bool flag = hasReportedError;
		if (!previouslyReportedDevNotifications.Contains(hashValue))
		{
			previouslyReportedDevNotifications.Add(hashValue);
			if (extraCategories != null)
			{
				Array.Resize(ref extraCategories, extraCategories.Length + 1);
				extraCategories[extraCategories.Length - 1] = CRASH_CATEGORY.DEVNOTIFICATION;
			}
			else
			{
				extraCategories = new string[1] { CRASH_CATEGORY.DEVNOTIFICATION };
			}
			ReportError("DevNotification: " + notification_name, stack_trace, null, null, details, includeSaveFile, extraCategories);
		}
		hasReportedError = flag;
	}

	public static void ReportError(string msg, string stack_trace, ConfirmDialogScreen confirm_prefab, GameObject confirm_parent, string userMessage = "", bool includeSaveFile = true, string[] extraCategories = null, string[] extraFiles = null)
	{
		if (KPrivacyPrefs.instance.disableDataCollection || ignoreAll)
		{
			return;
		}
		Debug.Log("Reporting error.\n");
		if (msg != null)
		{
			Debug.Log(msg);
		}
		if (stack_trace != null)
		{
			Debug.Log(stack_trace);
		}
		hasReportedError = true;
		if (string.IsNullOrEmpty(msg))
		{
			msg = "No message";
		}
		Match match = failedToLoadModuleRegEx.Match(msg);
		if (match.Success)
		{
			string path = match.Groups[1].ToString();
			string text = match.Groups[2].ToString();
			string fileName = Path.GetFileName(path);
			msg = "Failed to load '" + fileName + "' with error '" + text + "'.";
		}
		if (string.IsNullOrEmpty(stack_trace))
		{
			string buildText = BuildWatermark.GetBuildText();
			stack_trace = $"No stack trace {buildText}\n\n{msg}";
		}
		List<string> list = new List<string>();
		if (debugWasUsed)
		{
			list.Add("(Debug Used)");
		}
		if (haveActiveMods)
		{
			list.Add("(Mods Active)");
		}
		list.Add(msg);
		string[] array = new string[8] { "Debug:LogError", "UnityEngine.Debug", "Output:LogError", "DebugUtil:Assert", "System.Array", "System.Collections", "KCrashReporter.Assert", "No stack trace." };
		string[] array2 = stack_trace.Split('\n');
		foreach (string text2 in array2)
		{
			if (list.Count >= 5)
			{
				break;
			}
			if (string.IsNullOrEmpty(text2))
			{
				continue;
			}
			bool flag = false;
			string[] array3 = array;
			foreach (string value in array3)
			{
				if (text2.StartsWith(value))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(text2);
			}
		}
		userMessage = ((!(userMessage == UI.CRASHSCREEN.BODY.text) && !userMessage.IsNullOrWhiteSpace()) ? ("[" + BuildWatermark.GetBuildText() + "] " + userMessage) : "");
		userMessage = userMessage.Replace(stack_trace, "");
		Error error = new Error();
		if (extraCategories != null)
		{
			error.categories.AddRange(extraCategories);
		}
		error.callstack = stack_trace;
		if (disableDeduping)
		{
			error.callstack = error.callstack + "\n" + Guid.NewGuid().ToString();
		}
		error.fullstack = $"{msg}\n\n{stack_trace}";
		error.summaryline = string.Join("\n", list.ToArray());
		error.userMessage = userMessage;
		List<string> list2 = new List<string>();
		if (includeSaveFile && MOST_RECENT_SAVEFILE != null)
		{
			list2.Add(MOST_RECENT_SAVEFILE);
			error.saveFilename = Path.GetFileName(MOST_RECENT_SAVEFILE);
		}
		if (extraFiles != null)
		{
			array2 = extraFiles;
			foreach (string text3 in array2)
			{
				list2.Add(text3);
				error.extraFilenames.Add(Path.GetFileName(text3));
			}
		}
		string jsonString = JsonConvert.SerializeObject(error);
		byte[] archiveData = CreateArchiveZip(GetLogContents(), list2);
		System.Action successCallback = delegate
		{
			if (confirm_prefab != null && confirm_parent != null)
			{
				((ConfirmDialogScreen)KScreenManager.Instance.StartScreen(confirm_prefab.gameObject, confirm_parent)).PopupConfirmDialog(UI.CRASHSCREEN.REPORTEDERROR_SUCCESS, null, null);
			}
		};
		Action<long> failureCallback = delegate(long errorCode)
		{
			if (confirm_prefab != null && confirm_parent != null)
			{
				string text4 = ((errorCode == 413) ? UI.CRASHSCREEN.REPORTEDERROR_FAILURE_TOO_LARGE : UI.CRASHSCREEN.REPORTEDERROR_FAILURE);
				((ConfirmDialogScreen)KScreenManager.Instance.StartScreen(confirm_prefab.gameObject, confirm_parent)).PopupConfirmDialog(text4, null, null);
			}
		};
		pendingCrash = new PendingCrash
		{
			jsonString = jsonString,
			archiveData = archiveData,
			successCallback = successCallback,
			failureCallback = failureCallback
		};
	}

	private static IEnumerator SubmitCrashAsync(string jsonString, byte[] archiveData, System.Action successCallback, Action<long> failureCallback)
	{
		bool success = false;
		Uri uri = new Uri("https://games-feedback.klei.com/submit");
		List<IMultipartFormSection> list = new List<IMultipartFormSection>
		{
			new MultipartFormDataSection("metadata", jsonString),
			new MultipartFormFileSection("archiveFile", archiveData, "Archive.zip", "application/octet-stream")
		};
		if (KleiAccount.KleiToken != null)
		{
			list.Add(new MultipartFormDataSection("loginToken", KleiAccount.KleiToken));
		}
		using (UnityWebRequest w = UnityWebRequest.Post(uri, list))
		{
			w.SendWebRequest();
			while (!w.isDone)
			{
				yield return null;
				if (KCrashReporter.onCrashUploadProgress != null)
				{
					KCrashReporter.onCrashUploadProgress(w.uploadProgress);
				}
			}
			if (w.result == UnityWebRequest.Result.Success)
			{
				UnityEngine.Debug.Log("Submitted crash!");
				successCallback?.Invoke();
				success = true;
			}
			else
			{
				UnityEngine.Debug.Log("CrashReporter: Could not submit crash " + w.result);
				failureCallback?.Invoke(w.responseCode);
			}
		}
		if (KCrashReporter.onCrashReported != null)
		{
			KCrashReporter.onCrashReported(success);
		}
	}

	public static void ReportBug(string msg, GameObject confirmParent)
	{
		string stack_trace = "Bug Report From: " + GetUserID() + " at " + System.DateTime.Now.ToString();
		ReportError(msg, stack_trace, ScreenPrefabs.Instance.ConfirmDialogScreen, confirmParent);
	}

	public static void Assert(bool condition, string message, string[] extraCategories = null)
	{
		if (!condition && !hasReportedError)
		{
			StackTrace stackTrace = new StackTrace(1, fNeedFileInfo: true);
			ReportError("ASSERT: " + message, stackTrace.ToString(), null, null, null, includeSaveFile: true, extraCategories);
		}
	}

	public static void ReportSimDLLCrash(string msg, string stack_trace, string dmp_filename)
	{
		if (!hasReportedError)
		{
			ReportError(msg, stack_trace, null, null, "", includeSaveFile: true, new string[1] { CRASH_CATEGORY.SIM }, new string[1] { dmp_filename });
		}
	}

	private static byte[] CreateArchiveZip(string log, List<string> files)
	{
		using MemoryStream memoryStream = new MemoryStream();
		using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
		{
			if (files != null)
			{
				foreach (string file in files)
				{
					try
					{
						if (!File.Exists(file))
						{
							UnityEngine.Debug.Log("CrashReporter: file does not exist to include: " + file);
							continue;
						}
						using Stream stream = zipArchive.CreateEntry(Path.GetFileName(file), System.IO.Compression.CompressionLevel.Fastest).Open();
						byte[] array = File.ReadAllBytes(file);
						stream.Write(array, 0, array.Length);
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.Log("CrashReporter: Could not add file '" + file + "' to archive: " + ex);
					}
				}
				using Stream stream2 = zipArchive.CreateEntry("Player.log", System.IO.Compression.CompressionLevel.Fastest).Open();
				byte[] bytes = Encoding.UTF8.GetBytes(log);
				stream2.Write(bytes, 0, bytes.Length);
			}
		}
		return memoryStream.ToArray();
	}

	private void Update()
	{
		if (KCrashReporter.pendingCrash != null)
		{
			PendingCrash pendingCrash = KCrashReporter.pendingCrash;
			KCrashReporter.pendingCrash = null;
			Debug.Log("Submitting crash...");
			StartCoroutine(SubmitCrashAsync(pendingCrash.jsonString, pendingCrash.archiveData, pendingCrash.successCallback, pendingCrash.failureCallback));
		}
	}
}
