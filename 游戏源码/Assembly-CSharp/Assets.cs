using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using KMod;
using TUNING;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x02000C24 RID: 3108
[AddComponentMenu("KMonoBehaviour/scripts/Assets")]
public class Assets : KMonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x06003B47 RID: 15175 RVA: 0x0022A3BC File Offset: 0x002285BC
	protected override void OnPrefabInit()
	{
		Assets.instance = this;
		if (KPlayerPrefs.HasKey("TemperatureUnit"))
		{
			GameUtil.temperatureUnit = (GameUtil.TemperatureUnit)KPlayerPrefs.GetInt("TemperatureUnit");
		}
		if (KPlayerPrefs.HasKey("MassUnit"))
		{
			GameUtil.massUnit = (GameUtil.MassUnit)KPlayerPrefs.GetInt("MassUnit");
		}
		RecipeManager.DestroyInstance();
		RecipeManager.Get();
		Assets.AnimMaterial = this.AnimMaterialAsset;
		Assets.Prefabs = new List<KPrefabID>(from x in this.PrefabAssets
		where x != null
		select x);
		Assets.PrefabsByTag.Clear();
		Assets.PrefabsByAdditionalTags.Clear();
		Assets.CountableTags.Clear();
		Assets.Sprites = new Dictionary<HashedString, Sprite>();
		foreach (Sprite sprite in this.SpriteAssets)
		{
			if (!(sprite == null))
			{
				HashedString key = new HashedString(sprite.name);
				Assets.Sprites.Add(key, sprite);
			}
		}
		Assets.TintedSprites = (from x in this.TintedSpriteAssets
		where x != null && x.sprite != null
		select x).ToList<TintedSprite>();
		Assets.Materials = (from x in this.MaterialAssets
		where x != null
		select x).ToList<Material>();
		Assets.Textures = (from x in this.TextureAssets
		where x != null
		select x).ToList<Texture2D>();
		Assets.TextureAtlases = (from x in this.TextureAtlasAssets
		where x != null
		select x).ToList<TextureAtlas>();
		Assets.BlockTileDecorInfos = (from x in this.BlockTileDecorInfoAssets
		where x != null
		select x).ToList<BlockTileDecorInfo>();
		this.LoadAnims();
		Assets.UIPrefabs = this.UIPrefabAssets;
		Assets.DebugFont = this.DebugFontAsset;
		AsyncLoadManager<IGlobalAsyncLoader>.Run();
		GameAudioSheets.Get().Initialize();
		this.SubstanceListHookup();
		this.CreatePrefabs();
	}

	// Token: 0x06003B48 RID: 15176 RVA: 0x0022A614 File Offset: 0x00228814
	private void CreatePrefabs()
	{
		Db.Get();
		Assets.BuildingDefs = new List<BuildingDef>();
		foreach (KPrefabID kprefabID in this.PrefabAssets)
		{
			if (!(kprefabID == null))
			{
				kprefabID.InitializeTags(true);
				Assets.AddPrefab(kprefabID);
			}
		}
		LegacyModMain.Load();
		Db.Get().PostProcess();
	}

	// Token: 0x06003B49 RID: 15177 RVA: 0x000C648F File Offset: 0x000C468F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Db.Get();
	}

	// Token: 0x06003B4A RID: 15178 RVA: 0x0022A698 File Offset: 0x00228898
	private static void TryAddCountableTag(KPrefabID prefab)
	{
		foreach (Tag tag in GameTags.DisplayAsUnits)
		{
			if (prefab.HasTag(tag))
			{
				Assets.AddCountableTag(prefab.PrefabTag);
				break;
			}
		}
	}

	// Token: 0x06003B4B RID: 15179 RVA: 0x000C649D File Offset: 0x000C469D
	public static void AddCountableTag(Tag tag)
	{
		Assets.CountableTags.Add(tag);
	}

	// Token: 0x06003B4C RID: 15180 RVA: 0x000C64AB File Offset: 0x000C46AB
	public static bool IsTagCountable(Tag tag)
	{
		return Assets.CountableTags.Contains(tag);
	}

	// Token: 0x06003B4D RID: 15181 RVA: 0x000C64B8 File Offset: 0x000C46B8
	private static void TryAddSolidTransferArmConveyableTag(KPrefabID prefab)
	{
		if (prefab.HasAnyTags(STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE))
		{
			Assets.SolidTransferArmConeyableTags.Add(prefab.PrefabTag);
		}
	}

	// Token: 0x06003B4E RID: 15182 RVA: 0x000C64D8 File Offset: 0x000C46D8
	public static bool IsTagSolidTransferArmConveyable(Tag tag)
	{
		return Assets.SolidTransferArmConeyableTags.Contains(tag);
	}

	// Token: 0x06003B4F RID: 15183 RVA: 0x0022A6F4 File Offset: 0x002288F4
	private void LoadAnims()
	{
		KAnimBatchManager.DestroyInstance();
		KAnimGroupFile.DestroyInstance();
		KGlobalAnimParser.DestroyInstance();
		KAnimBatchManager.CreateInstance();
		KGlobalAnimParser.CreateInstance();
		KAnimGroupFile.LoadGroupResourceFile();
		if (BundledAssetsLoader.instance.Expansion1Assets != null)
		{
			this.AnimAssets.AddRange(BundledAssetsLoader.instance.Expansion1Assets.AnimAssets);
		}
		foreach (BundledAssets bundledAssets in BundledAssetsLoader.instance.DlcAssetsList)
		{
			this.AnimAssets.AddRange(bundledAssets.AnimAssets);
		}
		Assets.Anims = (from x in this.AnimAssets
		where x != null
		select x).ToList<KAnimFile>();
		Assets.Anims.AddRange(Assets.ModLoadedKAnims);
		Assets.AnimTable.Clear();
		foreach (KAnimFile kanimFile in Assets.Anims)
		{
			if (kanimFile != null)
			{
				HashedString key = kanimFile.name;
				Assets.AnimTable[key] = kanimFile;
			}
		}
		KAnimGroupFile.MapNamesToAnimFiles(Assets.AnimTable);
		Global.Instance.modManager.Load(Content.Animation);
		Assets.Anims.AddRange(Assets.ModLoadedKAnims);
		foreach (KAnimFile kanimFile2 in Assets.ModLoadedKAnims)
		{
			if (kanimFile2 != null)
			{
				HashedString key2 = kanimFile2.name;
				Assets.AnimTable[key2] = kanimFile2;
			}
		}
		global::Debug.Assert(Assets.AnimTable.Count > 0, "Anim Assets not yet loaded");
		KAnimGroupFile.LoadAll();
		foreach (KAnimFile kanimFile3 in Assets.Anims)
		{
			kanimFile3.FinalizeLoading();
		}
		KAnimBatchManager.Instance().CompleteInit();
	}

	// Token: 0x06003B50 RID: 15184 RVA: 0x0022A938 File Offset: 0x00228B38
	private void SubstanceListHookup()
	{
		Dictionary<string, SubstanceTable> dictionary = new Dictionary<string, SubstanceTable>
		{
			{
				"",
				this.substanceTable
			}
		};
		if (BundledAssetsLoader.instance.Expansion1Assets != null)
		{
			dictionary["EXPANSION1_ID"] = BundledAssetsLoader.instance.Expansion1Assets.SubstanceTable;
		}
		Hashtable hashtable = new Hashtable();
		ElementsAudio.Instance.LoadData(AsyncLoadManager<IGlobalAsyncLoader>.AsyncLoader<ElementAudioFileLoader>.Get().entries);
		ElementLoader.Load(ref hashtable, dictionary);
		List<Element> list = ElementLoader.elements.FindAll((Element e) => e.HasTag(GameTags.StartingMetalOre));
		GameTags.StartingMetalOres = new Tag[list.Count];
		for (int i = 0; i < list.Count; i++)
		{
			GameTags.StartingMetalOres[i] = list[i].tag;
		}
		List<Element> list2 = ElementLoader.elements.FindAll((Element e) => e.HasTag(GameTags.StartingRefinedMetalOre));
		GameTags.StartingRefinedMetalOres = new Tag[list2.Count];
		for (int j = 0; j < list2.Count; j++)
		{
			GameTags.StartingRefinedMetalOres[j] = list2[j].tag;
		}
	}

	// Token: 0x06003B51 RID: 15185 RVA: 0x000C64E5 File Offset: 0x000C46E5
	public static string GetSimpleSoundEventName(EventReference event_ref)
	{
		return Assets.GetSimpleSoundEventName(KFMOD.GetEventReferencePath(event_ref));
	}

	// Token: 0x06003B52 RID: 15186 RVA: 0x0022AA7C File Offset: 0x00228C7C
	public static string GetSimpleSoundEventName(string path)
	{
		string text = null;
		if (!Assets.simpleSoundEventNames.TryGetValue(path, out text))
		{
			int num = path.LastIndexOf('/');
			text = ((num != -1) ? path.Substring(num + 1) : path);
			Assets.simpleSoundEventNames[path] = text;
		}
		return text;
	}

	// Token: 0x06003B53 RID: 15187 RVA: 0x0022AAC4 File Offset: 0x00228CC4
	private static BuildingDef GetDef(IList<BuildingDef> defs, string prefab_id)
	{
		int count = defs.Count;
		for (int i = 0; i < count; i++)
		{
			if (defs[i].PrefabID == prefab_id)
			{
				return defs[i];
			}
		}
		return null;
	}

	// Token: 0x06003B54 RID: 15188 RVA: 0x000C64F2 File Offset: 0x000C46F2
	public static BuildingDef GetBuildingDef(string prefab_id)
	{
		return Assets.GetDef(Assets.BuildingDefs, prefab_id);
	}

	// Token: 0x06003B55 RID: 15189 RVA: 0x0022AB04 File Offset: 0x00228D04
	public static TintedSprite GetTintedSprite(string name)
	{
		TintedSprite result = null;
		if (Assets.TintedSprites != null)
		{
			for (int i = 0; i < Assets.TintedSprites.Count; i++)
			{
				if (Assets.TintedSprites[i].sprite.name == name)
				{
					result = Assets.TintedSprites[i];
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06003B56 RID: 15190 RVA: 0x0022AB5C File Offset: 0x00228D5C
	public static Sprite GetSprite(HashedString name)
	{
		Sprite result = null;
		if (Assets.Sprites != null)
		{
			Assets.Sprites.TryGetValue(name, out result);
		}
		return result;
	}

	// Token: 0x06003B57 RID: 15191 RVA: 0x000C64FF File Offset: 0x000C46FF
	public static VideoClip GetVideo(string name)
	{
		return Resources.Load<VideoClip>("video_webm/" + name);
	}

	// Token: 0x06003B58 RID: 15192 RVA: 0x0022AB84 File Offset: 0x00228D84
	public static Texture2D GetTexture(string name)
	{
		Texture2D result = null;
		if (Assets.Textures != null)
		{
			for (int i = 0; i < Assets.Textures.Count; i++)
			{
				if (Assets.Textures[i].name == name)
				{
					result = Assets.Textures[i];
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06003B59 RID: 15193 RVA: 0x0022ABD8 File Offset: 0x00228DD8
	public static ComicData GetComic(string id)
	{
		foreach (ComicData comicData in Assets.instance.comics)
		{
			if (comicData.name == id)
			{
				return comicData;
			}
		}
		return null;
	}

	// Token: 0x06003B5A RID: 15194 RVA: 0x0022AC14 File Offset: 0x00228E14
	public static void AddPrefab(KPrefabID prefab)
	{
		if (prefab == null)
		{
			return;
		}
		prefab.InitializeTags(true);
		prefab.UpdateSaveLoadTag();
		if (Assets.PrefabsByTag.ContainsKey(prefab.PrefabTag))
		{
			string str = "Tried loading prefab with duplicate tag, ignoring: ";
			Tag prefabTag = prefab.PrefabTag;
			global::Debug.LogWarning(str + prefabTag.ToString());
			return;
		}
		Assets.PrefabsByTag[prefab.PrefabTag] = prefab;
		foreach (Tag key in prefab.Tags)
		{
			if (!Assets.PrefabsByAdditionalTags.ContainsKey(key))
			{
				Assets.PrefabsByAdditionalTags[key] = new List<KPrefabID>();
			}
			Assets.PrefabsByAdditionalTags[key].Add(prefab);
		}
		Assets.Prefabs.Add(prefab);
		Assets.TryAddCountableTag(prefab);
		Assets.TryAddSolidTransferArmConveyableTag(prefab);
		if (Assets.OnAddPrefab != null)
		{
			Assets.OnAddPrefab(prefab);
		}
	}

	// Token: 0x06003B5B RID: 15195 RVA: 0x0022AD18 File Offset: 0x00228F18
	public static void RegisterOnAddPrefab(Action<KPrefabID> on_add)
	{
		Assets.OnAddPrefab = (Action<KPrefabID>)Delegate.Combine(Assets.OnAddPrefab, on_add);
		foreach (KPrefabID obj in Assets.Prefabs)
		{
			on_add(obj);
		}
	}

	// Token: 0x06003B5C RID: 15196 RVA: 0x000C6511 File Offset: 0x000C4711
	public static void UnregisterOnAddPrefab(Action<KPrefabID> on_add)
	{
		Assets.OnAddPrefab = (Action<KPrefabID>)Delegate.Remove(Assets.OnAddPrefab, on_add);
	}

	// Token: 0x06003B5D RID: 15197 RVA: 0x000C6528 File Offset: 0x000C4728
	public static void ClearOnAddPrefab()
	{
		Assets.OnAddPrefab = null;
	}

	// Token: 0x06003B5E RID: 15198 RVA: 0x0022AD80 File Offset: 0x00228F80
	public static GameObject GetPrefab(Tag tag)
	{
		GameObject gameObject = Assets.TryGetPrefab(tag);
		if (gameObject == null)
		{
			string str = "Missing prefab: ";
			Tag tag2 = tag;
			global::Debug.LogWarning(str + tag2.ToString());
		}
		return gameObject;
	}

	// Token: 0x06003B5F RID: 15199 RVA: 0x0022ADBC File Offset: 0x00228FBC
	public static GameObject TryGetPrefab(Tag tag)
	{
		KPrefabID kprefabID = null;
		Assets.PrefabsByTag.TryGetValue(tag, out kprefabID);
		if (!(kprefabID != null))
		{
			return null;
		}
		return kprefabID.gameObject;
	}

	// Token: 0x06003B60 RID: 15200 RVA: 0x0022ADEC File Offset: 0x00228FEC
	public static List<GameObject> GetPrefabsWithTag(Tag tag)
	{
		List<GameObject> list = new List<GameObject>();
		if (Assets.PrefabsByAdditionalTags.ContainsKey(tag))
		{
			for (int i = 0; i < Assets.PrefabsByAdditionalTags[tag].Count; i++)
			{
				list.Add(Assets.PrefabsByAdditionalTags[tag][i].gameObject);
			}
		}
		return list;
	}

	// Token: 0x06003B61 RID: 15201 RVA: 0x0022AE44 File Offset: 0x00229044
	public static List<GameObject> GetPrefabsWithComponent<Type>()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < Assets.Prefabs.Count; i++)
		{
			if (Assets.Prefabs[i].GetComponent<Type>() != null)
			{
				list.Add(Assets.Prefabs[i].gameObject);
			}
		}
		return list;
	}

	// Token: 0x06003B62 RID: 15202 RVA: 0x000C6530 File Offset: 0x000C4730
	public static GameObject GetPrefabWithComponent<Type>()
	{
		List<GameObject> prefabsWithComponent = Assets.GetPrefabsWithComponent<Type>();
		global::Debug.Assert(prefabsWithComponent.Count > 0, "There are no prefabs of type " + typeof(Type).Name);
		return prefabsWithComponent[0];
	}

	// Token: 0x06003B63 RID: 15203 RVA: 0x0022AE9C File Offset: 0x0022909C
	public static List<Tag> GetPrefabTagsWithComponent<Type>()
	{
		List<Tag> list = new List<Tag>();
		for (int i = 0; i < Assets.Prefabs.Count; i++)
		{
			if (Assets.Prefabs[i].GetComponent<Type>() != null)
			{
				list.Add(Assets.Prefabs[i].PrefabID());
			}
		}
		return list;
	}

	// Token: 0x06003B64 RID: 15204 RVA: 0x0022AEF4 File Offset: 0x002290F4
	public static Assets GetInstanceEditorOnly()
	{
		Assets[] array = (Assets[])Resources.FindObjectsOfTypeAll(typeof(Assets));
		if (array != null)
		{
			int num = array.Length;
		}
		return array[0];
	}

	// Token: 0x06003B65 RID: 15205 RVA: 0x0022AF20 File Offset: 0x00229120
	public static TextureAtlas GetTextureAtlas(string name)
	{
		foreach (TextureAtlas textureAtlas in Assets.TextureAtlases)
		{
			if (textureAtlas.name == name)
			{
				return textureAtlas;
			}
		}
		return null;
	}

	// Token: 0x06003B66 RID: 15206 RVA: 0x0022AF80 File Offset: 0x00229180
	public static Material GetMaterial(string name)
	{
		foreach (Material material in Assets.Materials)
		{
			if (material.name == name)
			{
				return material;
			}
		}
		return null;
	}

	// Token: 0x06003B67 RID: 15207 RVA: 0x0022AFE0 File Offset: 0x002291E0
	public static BlockTileDecorInfo GetBlockTileDecorInfo(string name)
	{
		foreach (BlockTileDecorInfo blockTileDecorInfo in Assets.BlockTileDecorInfos)
		{
			if (blockTileDecorInfo.name == name)
			{
				return blockTileDecorInfo;
			}
		}
		global::Debug.LogError("Could not find BlockTileDecorInfo named [" + name + "]");
		return null;
	}

	// Token: 0x06003B68 RID: 15208 RVA: 0x0022B058 File Offset: 0x00229258
	public static KAnimFile GetAnim(HashedString name)
	{
		if (!name.IsValid)
		{
			global::Debug.LogWarning("Invalid hash name");
			return null;
		}
		KAnimFile kanimFile = null;
		Assets.AnimTable.TryGetValue(name, out kanimFile);
		if (kanimFile == null)
		{
			global::Debug.LogWarning("Missing Anim: [" + name.ToString() + "]. You may have to run Collect Anim on the Assets prefab");
		}
		return kanimFile;
	}

	// Token: 0x06003B69 RID: 15209 RVA: 0x000C6564 File Offset: 0x000C4764
	public static bool TryGetAnim(HashedString name, out KAnimFile anim)
	{
		if (!name.IsValid)
		{
			global::Debug.LogWarning("Invalid hash name");
			anim = null;
			return false;
		}
		Assets.AnimTable.TryGetValue(name, out anim);
		return anim != null;
	}

	// Token: 0x06003B6A RID: 15210 RVA: 0x0022B0B8 File Offset: 0x002292B8
	public void OnAfterDeserialize()
	{
		this.TintedSpriteAssets = (from x in this.TintedSpriteAssets
		where x != null && x.sprite != null
		select x).ToList<TintedSprite>();
		this.TintedSpriteAssets.Sort((TintedSprite a, TintedSprite b) => a.name.CompareTo(b.name));
	}

	// Token: 0x06003B6B RID: 15211 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x06003B6C RID: 15212 RVA: 0x0022B124 File Offset: 0x00229324
	public static void AddBuildingDef(BuildingDef def)
	{
		Assets.BuildingDefs = (from x in Assets.BuildingDefs
		where x.PrefabID != def.PrefabID
		select x).ToList<BuildingDef>();
		Assets.BuildingDefs.Add(def);
	}

	// Token: 0x04002884 RID: 10372
	public static List<KAnimFile> ModLoadedKAnims = new List<KAnimFile>();

	// Token: 0x04002885 RID: 10373
	private static Action<KPrefabID> OnAddPrefab;

	// Token: 0x04002886 RID: 10374
	public static List<BuildingDef> BuildingDefs;

	// Token: 0x04002887 RID: 10375
	public List<KPrefabID> PrefabAssets = new List<KPrefabID>();

	// Token: 0x04002888 RID: 10376
	public static List<KPrefabID> Prefabs = new List<KPrefabID>();

	// Token: 0x04002889 RID: 10377
	private static HashSet<Tag> CountableTags = new HashSet<Tag>();

	// Token: 0x0400288A RID: 10378
	private static HashSet<Tag> SolidTransferArmConeyableTags = new HashSet<Tag>();

	// Token: 0x0400288B RID: 10379
	public List<Sprite> SpriteAssets;

	// Token: 0x0400288C RID: 10380
	public static Dictionary<HashedString, Sprite> Sprites;

	// Token: 0x0400288D RID: 10381
	public List<string> videoClipNames;

	// Token: 0x0400288E RID: 10382
	private const string VIDEO_ASSET_PATH = "video_webm";

	// Token: 0x0400288F RID: 10383
	public List<TintedSprite> TintedSpriteAssets;

	// Token: 0x04002890 RID: 10384
	public static List<TintedSprite> TintedSprites;

	// Token: 0x04002891 RID: 10385
	public List<Texture2D> TextureAssets;

	// Token: 0x04002892 RID: 10386
	public static List<Texture2D> Textures;

	// Token: 0x04002893 RID: 10387
	public static List<TextureAtlas> TextureAtlases;

	// Token: 0x04002894 RID: 10388
	public List<TextureAtlas> TextureAtlasAssets;

	// Token: 0x04002895 RID: 10389
	public static List<Material> Materials;

	// Token: 0x04002896 RID: 10390
	public List<Material> MaterialAssets;

	// Token: 0x04002897 RID: 10391
	public static List<Shader> Shaders;

	// Token: 0x04002898 RID: 10392
	public List<Shader> ShaderAssets;

	// Token: 0x04002899 RID: 10393
	public static List<BlockTileDecorInfo> BlockTileDecorInfos;

	// Token: 0x0400289A RID: 10394
	public List<BlockTileDecorInfo> BlockTileDecorInfoAssets;

	// Token: 0x0400289B RID: 10395
	public Material AnimMaterialAsset;

	// Token: 0x0400289C RID: 10396
	public static Material AnimMaterial;

	// Token: 0x0400289D RID: 10397
	public DiseaseVisualization DiseaseVisualization;

	// Token: 0x0400289E RID: 10398
	public Sprite LegendColourBox;

	// Token: 0x0400289F RID: 10399
	public Texture2D invalidAreaTex;

	// Token: 0x040028A0 RID: 10400
	public Assets.UIPrefabData UIPrefabAssets;

	// Token: 0x040028A1 RID: 10401
	public static Assets.UIPrefabData UIPrefabs;

	// Token: 0x040028A2 RID: 10402
	private static Dictionary<Tag, KPrefabID> PrefabsByTag = new Dictionary<Tag, KPrefabID>();

	// Token: 0x040028A3 RID: 10403
	private static Dictionary<Tag, List<KPrefabID>> PrefabsByAdditionalTags = new Dictionary<Tag, List<KPrefabID>>();

	// Token: 0x040028A4 RID: 10404
	public List<KAnimFile> AnimAssets;

	// Token: 0x040028A5 RID: 10405
	public static List<KAnimFile> Anims;

	// Token: 0x040028A6 RID: 10406
	private static Dictionary<HashedString, KAnimFile> AnimTable = new Dictionary<HashedString, KAnimFile>();

	// Token: 0x040028A7 RID: 10407
	public Font DebugFontAsset;

	// Token: 0x040028A8 RID: 10408
	public static Font DebugFont;

	// Token: 0x040028A9 RID: 10409
	public SubstanceTable substanceTable;

	// Token: 0x040028AA RID: 10410
	[SerializeField]
	public TextAsset elementAudio;

	// Token: 0x040028AB RID: 10411
	[SerializeField]
	public TextAsset personalitiesFile;

	// Token: 0x040028AC RID: 10412
	public LogicModeUI logicModeUIData;

	// Token: 0x040028AD RID: 10413
	public CommonPlacerConfig.CommonPlacerAssets commonPlacerAssets;

	// Token: 0x040028AE RID: 10414
	public DigPlacerConfig.DigPlacerAssets digPlacerAssets;

	// Token: 0x040028AF RID: 10415
	public MopPlacerConfig.MopPlacerAssets mopPlacerAssets;

	// Token: 0x040028B0 RID: 10416
	public MovePickupablePlacerConfig.MovePickupablePlacerAssets movePickupToPlacerAssets;

	// Token: 0x040028B1 RID: 10417
	public ComicData[] comics;

	// Token: 0x040028B2 RID: 10418
	public static Assets instance;

	// Token: 0x040028B3 RID: 10419
	private static Dictionary<string, string> simpleSoundEventNames = new Dictionary<string, string>();

	// Token: 0x02000C25 RID: 3109
	[Serializable]
	public struct UIPrefabData
	{
		// Token: 0x040028B4 RID: 10420
		public ProgressBar ProgressBar;

		// Token: 0x040028B5 RID: 10421
		public HealthBar HealthBar;

		// Token: 0x040028B6 RID: 10422
		public GameObject ResourceVisualizer;

		// Token: 0x040028B7 RID: 10423
		public GameObject KAnimVisualizer;

		// Token: 0x040028B8 RID: 10424
		public Image RegionCellBlocked;

		// Token: 0x040028B9 RID: 10425
		public RectTransform PriorityOverlayIcon;

		// Token: 0x040028BA RID: 10426
		public RectTransform HarvestWhenReadyOverlayIcon;

		// Token: 0x040028BB RID: 10427
		public Assets.TableScreenAssets TableScreenWidgets;
	}

	// Token: 0x02000C26 RID: 3110
	[Serializable]
	public struct TableScreenAssets
	{
		// Token: 0x040028BC RID: 10428
		public Material DefaultUIMaterial;

		// Token: 0x040028BD RID: 10429
		public Material DesaturatedUIMaterial;

		// Token: 0x040028BE RID: 10430
		public GameObject MinionPortrait;

		// Token: 0x040028BF RID: 10431
		public GameObject GenericPortrait;

		// Token: 0x040028C0 RID: 10432
		public GameObject TogglePortrait;

		// Token: 0x040028C1 RID: 10433
		public GameObject ButtonLabel;

		// Token: 0x040028C2 RID: 10434
		public GameObject ButtonLabelWhite;

		// Token: 0x040028C3 RID: 10435
		public GameObject Label;

		// Token: 0x040028C4 RID: 10436
		public GameObject LabelHeader;

		// Token: 0x040028C5 RID: 10437
		public GameObject Checkbox;

		// Token: 0x040028C6 RID: 10438
		public GameObject BlankCell;

		// Token: 0x040028C7 RID: 10439
		public GameObject SuperCheckbox_Horizontal;

		// Token: 0x040028C8 RID: 10440
		public GameObject SuperCheckbox_Vertical;

		// Token: 0x040028C9 RID: 10441
		public GameObject Spacer;

		// Token: 0x040028CA RID: 10442
		public GameObject NumericDropDown;

		// Token: 0x040028CB RID: 10443
		public GameObject DropDownHeader;

		// Token: 0x040028CC RID: 10444
		public GameObject PriorityGroupSelector;

		// Token: 0x040028CD RID: 10445
		public GameObject PriorityGroupSelectorHeader;

		// Token: 0x040028CE RID: 10446
		public GameObject PrioritizeRowWidget;

		// Token: 0x040028CF RID: 10447
		public GameObject PrioritizeRowHeaderWidget;
	}
}
