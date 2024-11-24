using System;
using System.IO;
using System.Threading;
using UnityEngine;

// Token: 0x02001488 RID: 5256
public class LaunchInitializer : MonoBehaviour
{
	// Token: 0x06006D03 RID: 27907 RVA: 0x000E789C File Offset: 0x000E5A9C
	public static string BuildPrefix()
	{
		return LaunchInitializer.BUILD_PREFIX;
	}

	// Token: 0x06006D04 RID: 27908 RVA: 0x000E78A3 File Offset: 0x000E5AA3
	public static int UpdateNumber()
	{
		return 53;
	}

	// Token: 0x06006D05 RID: 27909 RVA: 0x002E9EB0 File Offset: 0x002E80B0
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

	// Token: 0x06006D06 RID: 27910 RVA: 0x002EA060 File Offset: 0x002E8260
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

	// Token: 0x040051C5 RID: 20933
	private const string PREFIX = "U";

	// Token: 0x040051C6 RID: 20934
	private const int UPDATE_NUMBER = 53;

	// Token: 0x040051C7 RID: 20935
	private static readonly string BUILD_PREFIX = "U" + 53.ToString();

	// Token: 0x040051C8 RID: 20936
	public GameObject[] SpawnPrefabs;

	// Token: 0x040051C9 RID: 20937
	[SerializeField]
	private int numWaitFrames = 1;
}
