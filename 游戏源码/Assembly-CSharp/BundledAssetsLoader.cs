using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02001051 RID: 4177
public class BundledAssetsLoader : KMonoBehaviour
{
	// Token: 0x170004E9 RID: 1257
	// (get) Token: 0x06005537 RID: 21815 RVA: 0x000D798E File Offset: 0x000D5B8E
	// (set) Token: 0x06005538 RID: 21816 RVA: 0x000D7996 File Offset: 0x000D5B96
	public BundledAssets Expansion1Assets { get; private set; }

	// Token: 0x170004EA RID: 1258
	// (get) Token: 0x06005539 RID: 21817 RVA: 0x000D799F File Offset: 0x000D5B9F
	// (set) Token: 0x0600553A RID: 21818 RVA: 0x000D79A7 File Offset: 0x000D5BA7
	public List<BundledAssets> DlcAssetsList { get; private set; }

	// Token: 0x0600553B RID: 21819 RVA: 0x0027D7C8 File Offset: 0x0027B9C8
	protected override void OnPrefabInit()
	{
		BundledAssetsLoader.instance = this;
		if (DlcManager.IsExpansion1Active())
		{
			global::Debug.Log("Loading Expansion1 assets from bundle");
			AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, DlcManager.GetContentBundleName("EXPANSION1_ID")));
			global::Debug.Assert(assetBundle != null, "Expansion1 is Active but its asset bundle failed to load");
			GameObject gameObject = assetBundle.LoadAsset<GameObject>("Expansion1Assets");
			global::Debug.Assert(gameObject != null, "Could not load the Expansion1Assets prefab");
			this.Expansion1Assets = Util.KInstantiate(gameObject, base.gameObject, null).GetComponent<BundledAssets>();
		}
		this.DlcAssetsList = new List<BundledAssets>(DlcManager.DLC_PACKS.Count);
		foreach (KeyValuePair<string, DlcManager.DlcInfo> keyValuePair in DlcManager.DLC_PACKS)
		{
			if (DlcManager.IsContentSubscribed(keyValuePair.Key))
			{
				global::Debug.Log("Loading DLC " + keyValuePair.Key + " assets from bundle");
				AssetBundle assetBundle2 = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, DlcManager.GetContentBundleName(keyValuePair.Key)));
				global::Debug.Assert(assetBundle2 != null, "DLC " + keyValuePair.Key + " is Active but its asset bundle failed to load");
				GameObject gameObject2 = assetBundle2.LoadAsset<GameObject>(keyValuePair.Value.directory + "Assets");
				global::Debug.Assert(gameObject2 != null, "Could not load the " + keyValuePair.Key + " prefab");
				this.DlcAssetsList.Add(Util.KInstantiate(gameObject2, base.gameObject, null).GetComponent<BundledAssets>());
			}
		}
	}

	// Token: 0x04003BC2 RID: 15298
	public static BundledAssetsLoader instance;
}
