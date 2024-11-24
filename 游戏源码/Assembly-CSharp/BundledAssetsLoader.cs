using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BundledAssetsLoader : KMonoBehaviour
{
	public static BundledAssetsLoader instance;

	public BundledAssets Expansion1Assets { get; private set; }

	public List<BundledAssets> DlcAssetsList { get; private set; }

	protected override void OnPrefabInit()
	{
		instance = this;
		if (DlcManager.IsExpansion1Active())
		{
			Debug.Log("Loading Expansion1 assets from bundle");
			AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, DlcManager.GetContentBundleName("EXPANSION1_ID")));
			Debug.Assert(assetBundle != null, "Expansion1 is Active but its asset bundle failed to load");
			GameObject gameObject = assetBundle.LoadAsset<GameObject>("Expansion1Assets");
			Debug.Assert(gameObject != null, "Could not load the Expansion1Assets prefab");
			Expansion1Assets = Util.KInstantiate(gameObject, base.gameObject).GetComponent<BundledAssets>();
		}
		DlcAssetsList = new List<BundledAssets>(DlcManager.DLC_PACKS.Count);
		foreach (KeyValuePair<string, DlcManager.DlcInfo> dLC_PACK in DlcManager.DLC_PACKS)
		{
			if (DlcManager.IsContentSubscribed(dLC_PACK.Key))
			{
				Debug.Log("Loading DLC " + dLC_PACK.Key + " assets from bundle");
				AssetBundle assetBundle2 = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, DlcManager.GetContentBundleName(dLC_PACK.Key)));
				Debug.Assert(assetBundle2 != null, "DLC " + dLC_PACK.Key + " is Active but its asset bundle failed to load");
				GameObject gameObject2 = assetBundle2.LoadAsset<GameObject>(dLC_PACK.Value.directory + "Assets");
				Debug.Assert(gameObject2 != null, "Could not load the " + dLC_PACK.Key + " prefab");
				DlcAssetsList.Add(Util.KInstantiate(gameObject2, base.gameObject).GetComponent<BundledAssets>());
			}
		}
	}
}
