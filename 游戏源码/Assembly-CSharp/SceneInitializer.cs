using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02001831 RID: 6193
public class SceneInitializer : MonoBehaviour
{
	// Token: 0x1700082E RID: 2094
	// (get) Token: 0x06007FEF RID: 32751 RVA: 0x000F437D File Offset: 0x000F257D
	// (set) Token: 0x06007FF0 RID: 32752 RVA: 0x000F4384 File Offset: 0x000F2584
	public static SceneInitializer Instance { get; private set; }

	// Token: 0x06007FF1 RID: 32753 RVA: 0x003327AC File Offset: 0x003309AC
	private void Awake()
	{
		Localization.SwapToLocalizedFont();
		string environmentVariable = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
		string text = Application.dataPath + Path.DirectorySeparatorChar.ToString() + "Plugins";
		if (!environmentVariable.Contains(text))
		{
			Environment.SetEnvironmentVariable("PATH", environmentVariable + Path.PathSeparator.ToString() + text, EnvironmentVariableTarget.Process);
		}
		SceneInitializer.Instance = this;
		this.PreLoadPrefabs();
	}

	// Token: 0x06007FF2 RID: 32754 RVA: 0x000F438C File Offset: 0x000F258C
	private void OnDestroy()
	{
		SceneInitializer.Instance = null;
	}

	// Token: 0x06007FF3 RID: 32755 RVA: 0x0033281C File Offset: 0x00330A1C
	private void PreLoadPrefabs()
	{
		foreach (GameObject gameObject in this.preloadPrefabs)
		{
			if (gameObject != null)
			{
				Util.KInstantiate(gameObject, gameObject.transform.GetPosition(), Quaternion.identity, base.gameObject, null, true, 0);
			}
		}
	}

	// Token: 0x06007FF4 RID: 32756 RVA: 0x000F4394 File Offset: 0x000F2594
	public void NewSaveGamePrefab()
	{
		if (this.prefab_NewSaveGame != null && SaveGame.Instance == null)
		{
			Util.KInstantiate(this.prefab_NewSaveGame, base.gameObject, null);
		}
	}

	// Token: 0x06007FF5 RID: 32757 RVA: 0x00332894 File Offset: 0x00330A94
	public void PostLoadPrefabs()
	{
		foreach (GameObject gameObject in this.prefabs)
		{
			if (gameObject != null)
			{
				Util.KInstantiate(gameObject, base.gameObject, null);
			}
		}
	}

	// Token: 0x040060F2 RID: 24818
	public const int MAXDEPTH = -30000;

	// Token: 0x040060F3 RID: 24819
	public const int SCREENDEPTH = -1000;

	// Token: 0x040060F5 RID: 24821
	public GameObject prefab_NewSaveGame;

	// Token: 0x040060F6 RID: 24822
	public List<GameObject> preloadPrefabs = new List<GameObject>();

	// Token: 0x040060F7 RID: 24823
	public List<GameObject> prefabs = new List<GameObject>();
}
