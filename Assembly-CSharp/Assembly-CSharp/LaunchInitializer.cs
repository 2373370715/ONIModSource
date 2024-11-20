using System;
using System.IO;
using System.Threading;
using UnityEngine;

public class LaunchInitializer : MonoBehaviour
{
	public static string BuildPrefix()
	{
		return LaunchInitializer.BUILD_PREFIX;
	}

	public static int UpdateNumber()
	{
		return 52;
	}

	private void Update()
	{
		if (this.numWaitFrames > Time.renderedFrameCount)
		{
			return;
		}
		if (!DistributionPlatform.Initialized)
		{
			if (!SystemInfo.SupportsTextureFormat(TextureFormat.RGBAFloat))
			{
				global::Debug.LogError("Machine does not support RGBAFloat32");
			}
			GraphicsOptionsScreen.SetSettingsFromPrefs();
			Util.ApplyInvariantCultureToThread(Thread.CurrentThread);
			global::Debug.Log("Date: " + System.DateTime.Now.ToString());
			global::Debug.Log("Build: " + BuildWatermark.GetBuildText() + " (release)");
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			KPlayerPrefs.instance.Load();
			DistributionPlatform.Initialize();
		}
		if (!DistributionPlatform.Inst.IsDLCStatusReady())
		{
			return;
		}
		global::Debug.Log("DistributionPlatform initialized.");
		DebugUtil.LogArgs(new object[]
		{
			DebugUtil.LINE
		});
		global::Debug.Log("Build: " + BuildWatermark.GetBuildText() + " (release)");
		DebugUtil.LogArgs(new object[]
		{
			DebugUtil.LINE
		});
		DebugUtil.LogArgs(new object[]
		{
			"DLC Information"
		});
		foreach (string text in DlcManager.GetOwnedDLCIds())
		{
			global::Debug.Log(string.Format("- {0} loaded: {1}", text, DlcManager.IsContentSubscribed(text)));
		}
		DebugUtil.LogArgs(new object[]
		{
			DebugUtil.LINE
		});
		KFMOD.Initialize();
		for (int i = 0; i < this.SpawnPrefabs.Length; i++)
		{
			GameObject gameObject = this.SpawnPrefabs[i];
			if (gameObject != null)
			{
				Util.KInstantiate(gameObject, base.gameObject, null);
			}
		}
		LaunchInitializer.DeleteLingeringFiles();
		base.enabled = false;
	}

	private static void DeleteLingeringFiles()
	{
		string[] array = new string[]
		{
			"fmod.log",
			"load_stats_0.json",
			"OxygenNotIncluded_Data/output_log.txt"
		};
		string directoryName = Path.GetDirectoryName(Application.dataPath);
		foreach (string path in array)
		{
			string path2 = Path.Combine(directoryName, path);
			try
			{
				if (File.Exists(path2))
				{
					File.Delete(path2);
				}
			}
			catch (Exception obj)
			{
				global::Debug.LogWarning(obj);
			}
		}
	}

	private const string PREFIX = "U";

	private const int UPDATE_NUMBER = 52;

	private static readonly string BUILD_PREFIX = "U" + 52.ToString();

	public GameObject[] SpawnPrefabs;

	[SerializeField]
	private int numWaitFrames = 1;
}
