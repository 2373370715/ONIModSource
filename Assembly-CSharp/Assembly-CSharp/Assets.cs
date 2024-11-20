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

[AddComponentMenu("KMonoBehaviour/scripts/Assets")]
public class Assets : KMonoBehaviour, ISerializationCallbackReceiver {
    private const           string                           VIDEO_ASSET_PATH = "video_webm";
    public static           List<KAnimFile>                  ModLoadedKAnims  = new List<KAnimFile>();
    private static          Action<KPrefabID>                OnAddPrefab;
    public static           List<BuildingDef>                BuildingDefs;
    public static           List<KPrefabID>                  Prefabs                       = new List<KPrefabID>();
    private static readonly HashSet<Tag>                     CountableTags                 = new HashSet<Tag>();
    private static readonly HashSet<Tag>                     SolidTransferArmConeyableTags = new HashSet<Tag>();
    public static           Dictionary<HashedString, Sprite> Sprites;
    public static           List<TintedSprite>               TintedSprites;
    public static           List<Texture2D>                  Textures;
    public static           List<TextureAtlas>               TextureAtlases;
    public static           List<Material>                   Materials;
    public static           List<Shader>                     Shaders;
    public static           List<BlockTileDecorInfo>         BlockTileDecorInfos;
    public static           Material                         AnimMaterial;
    public static           UIPrefabData                     UIPrefabs;
    private static readonly Dictionary<Tag, KPrefabID>       PrefabsByTag = new Dictionary<Tag, KPrefabID>();

    private static readonly Dictionary<Tag, List<KPrefabID>> PrefabsByAdditionalTags
        = new Dictionary<Tag, List<KPrefabID>>();

    public static           List<KAnimFile> Anims;
    private static readonly Dictionary<HashedString, KAnimFile> AnimTable = new Dictionary<HashedString, KAnimFile>();
    public static           Font DebugFont;
    public static           Assets instance;
    private static readonly Dictionary<string, string> simpleSoundEventNames = new Dictionary<string, string>();
    public                  List<KAnimFile> AnimAssets;
    public                  Material AnimMaterialAsset;
    public                  List<BlockTileDecorInfo> BlockTileDecorInfoAssets;
    public                  ComicData[] comics;
    public                  CommonPlacerConfig.CommonPlacerAssets commonPlacerAssets;
    public                  Font DebugFontAsset;
    public                  DigPlacerConfig.DigPlacerAssets digPlacerAssets;
    public                  DiseaseVisualization DiseaseVisualization;

    [SerializeField]
    public TextAsset elementAudio;

    public Texture2D                                             invalidAreaTex;
    public Sprite                                                LegendColourBox;
    public LogicModeUI                                           logicModeUIData;
    public List<Material>                                        MaterialAssets;
    public MopPlacerConfig.MopPlacerAssets                       mopPlacerAssets;
    public MovePickupablePlacerConfig.MovePickupablePlacerAssets movePickupToPlacerAssets;

    [SerializeField]
    public TextAsset personalitiesFile;

    public List<KPrefabID>    PrefabAssets = new List<KPrefabID>();
    public List<Shader>       ShaderAssets;
    public List<Sprite>       SpriteAssets;
    public SubstanceTable     substanceTable;
    public List<Texture2D>    TextureAssets;
    public List<TextureAtlas> TextureAtlasAssets;
    public List<TintedSprite> TintedSpriteAssets;
    public UIPrefabData       UIPrefabAssets;
    public List<string>       videoClipNames;

    public void OnAfterDeserialize() {
        TintedSpriteAssets = (from x in TintedSpriteAssets where x != null && x.sprite != null select x).ToList();
        TintedSpriteAssets.Sort((a, b) => a.name.CompareTo(b.name));
    }

    public void OnBeforeSerialize() { }

    protected override void OnPrefabInit() {
        instance = this;
        if (KPlayerPrefs.HasKey("TemperatureUnit"))
            GameUtil.temperatureUnit = (GameUtil.TemperatureUnit)KPlayerPrefs.GetInt("TemperatureUnit");

        if (KPlayerPrefs.HasKey("MassUnit")) GameUtil.massUnit = (GameUtil.MassUnit)KPlayerPrefs.GetInt("MassUnit");
        RecipeManager.DestroyInstance();
        RecipeManager.Get();
        AnimMaterial = AnimMaterialAsset;
        Prefabs      = new List<KPrefabID>(from x in PrefabAssets where x != null select x);
        PrefabsByTag.Clear();
        PrefabsByAdditionalTags.Clear();
        CountableTags.Clear();
        Sprites = new Dictionary<HashedString, Sprite>();
        foreach (var sprite in SpriteAssets)
            if (!(sprite == null)) {
                var key = new HashedString(sprite.name);
                Sprites.Add(key, sprite);
            }

        TintedSprites       = (from x in TintedSpriteAssets where x != null && x.sprite != null select x).ToList();
        Materials           = (from x in MaterialAssets where x           != null select x).ToList();
        Textures            = (from x in TextureAssets where x            != null select x).ToList();
        TextureAtlases      = (from x in TextureAtlasAssets where x       != null select x).ToList();
        BlockTileDecorInfos = (from x in BlockTileDecorInfoAssets where x != null select x).ToList();
        LoadAnims();
        UIPrefabs = UIPrefabAssets;
        DebugFont = DebugFontAsset;
        AsyncLoadManager<IGlobalAsyncLoader>.Run();
        GameAudioSheets.Get().Initialize();
        SubstanceListHookup();
        CreatePrefabs();
    }

    private void CreatePrefabs() {
        Db.Get();
        BuildingDefs = new List<BuildingDef>();
        foreach (var kprefabID in PrefabAssets)
            if (!(kprefabID == null)) {
                kprefabID.InitializeTags(true);
                AddPrefab(kprefabID);
            }

        LegacyModMain.Load();
        Db.Get().PostProcess();
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        Db.Get();
    }

    private static void TryAddCountableTag(KPrefabID prefab) {
        foreach (var tag in GameTags.DisplayAsUnits)
            if (prefab.HasTag(tag)) {
                AddCountableTag(prefab.PrefabTag);
                break;
            }
    }

    public static void AddCountableTag(Tag tag) { CountableTags.Add(tag); }
    public static bool IsTagCountable(Tag  tag) { return CountableTags.Contains(tag); }

    private static void TryAddSolidTransferArmConveyableTag(KPrefabID prefab) {
        if (prefab.HasAnyTags(STORAGEFILTERS.SOLID_TRANSFER_ARM_CONVEYABLE))
            SolidTransferArmConeyableTags.Add(prefab.PrefabTag);
    }

    public static bool IsTagSolidTransferArmConveyable(Tag tag) { return SolidTransferArmConeyableTags.Contains(tag); }

    private void LoadAnims() {
        KAnimBatchManager.DestroyInstance();
        KAnimGroupFile.DestroyInstance();
        KGlobalAnimParser.DestroyInstance();
        KAnimBatchManager.CreateInstance();
        KGlobalAnimParser.CreateInstance();
        KAnimGroupFile.LoadGroupResourceFile();
        if (BundledAssetsLoader.instance.Expansion1Assets != null)
            AnimAssets.AddRange(BundledAssetsLoader.instance.Expansion1Assets.AnimAssets);

        foreach (var bundledAssets in BundledAssetsLoader.instance.DlcAssetsList)
            AnimAssets.AddRange(bundledAssets.AnimAssets);

        Anims = (from x in AnimAssets where x != null select x).ToList();
        Anims.AddRange(ModLoadedKAnims);
        AnimTable.Clear();
        foreach (var kanimFile in Anims)
            if (kanimFile != null) {
                HashedString key = kanimFile.name;
                AnimTable[key] = kanimFile;
            }

        KAnimGroupFile.MapNamesToAnimFiles(AnimTable);
        Global.Instance.modManager.Load(Content.Animation);
        Anims.AddRange(ModLoadedKAnims);
        foreach (var kanimFile2 in ModLoadedKAnims)
            if (kanimFile2 != null) {
                HashedString key2 = kanimFile2.name;
                AnimTable[key2] = kanimFile2;
            }

        Debug.Assert(AnimTable.Count > 0, "Anim Assets not yet loaded");
        KAnimGroupFile.LoadAll();
        foreach (var kanimFile3 in Anims) kanimFile3.FinalizeLoading();
        KAnimBatchManager.Instance().CompleteInit();
    }

    private void SubstanceListHookup() {
        var dictionary = new Dictionary<string, SubstanceTable> { { "", substanceTable } };
        if (BundledAssetsLoader.instance.Expansion1Assets != null)
            dictionary["EXPANSION1_ID"] = BundledAssetsLoader.instance.Expansion1Assets.SubstanceTable;

        var hashtable = new Hashtable();
        ElementsAudio.Instance.LoadData(AsyncLoadManager<IGlobalAsyncLoader>.AsyncLoader<ElementAudioFileLoader>.Get()
                                                                            .entries);

        ElementLoader.Load(ref hashtable, dictionary);
    }

    public static string GetSimpleSoundEventName(EventReference event_ref) {
        return GetSimpleSoundEventName(KFMOD.GetEventReferencePath(event_ref));
    }

    public static string GetSimpleSoundEventName(string path) {
        string text = null;
        if (!simpleSoundEventNames.TryGetValue(path, out text)) {
            var num = path.LastIndexOf('/');
            text                        = num != -1 ? path.Substring(num + 1) : path;
            simpleSoundEventNames[path] = text;
        }

        return text;
    }

    private static BuildingDef GetDef(IList<BuildingDef> defs, string prefab_id) {
        var count = defs.Count;
        for (var i = 0; i < count; i++)
            if (defs[i].PrefabID == prefab_id)
                return defs[i];

        return null;
    }

    public static BuildingDef GetBuildingDef(string prefab_id) { return GetDef(BuildingDefs, prefab_id); }

    public static TintedSprite GetTintedSprite(string name) {
        TintedSprite result = null;
        if (TintedSprites != null)
            for (var i = 0; i < TintedSprites.Count; i++)
                if (TintedSprites[i].sprite.name == name) {
                    result = TintedSprites[i];
                    break;
                }

        return result;
    }

    public static Sprite GetSprite(HashedString name) {
        Sprite result = null;
        if (Sprites != null) Sprites.TryGetValue(name, out result);
        return result;
    }

    public static VideoClip GetVideo(string name) { return Resources.Load<VideoClip>("video_webm/" + name); }

    public static Texture2D GetTexture(string name) {
        Texture2D result = null;
        if (Textures != null)
            for (var i = 0; i < Textures.Count; i++)
                if (Textures[i].name == name) {
                    result = Textures[i];
                    break;
                }

        return result;
    }

    public static ComicData GetComic(string id) {
        foreach (var comicData in instance.comics)
            if (comicData.name == id)
                return comicData;

        return null;
    }

    public static void AddPrefab(KPrefabID prefab) {
        if (prefab == null) return;

        prefab.InitializeTags(true);
        prefab.UpdateSaveLoadTag();
        if (PrefabsByTag.ContainsKey(prefab.PrefabTag)) {
            var str       = "Tried loading prefab with duplicate tag, ignoring: ";
            var prefabTag = prefab.PrefabTag;
            Debug.LogWarning(str + prefabTag);
            return;
        }

        PrefabsByTag[prefab.PrefabTag] = prefab;
        foreach (var key in prefab.Tags) {
            if (!PrefabsByAdditionalTags.ContainsKey(key)) PrefabsByAdditionalTags[key] = new List<KPrefabID>();
            PrefabsByAdditionalTags[key].Add(prefab);
        }

        Prefabs.Add(prefab);
        TryAddCountableTag(prefab);
        TryAddSolidTransferArmConveyableTag(prefab);
        if (OnAddPrefab != null) OnAddPrefab(prefab);
    }

    public static void RegisterOnAddPrefab(Action<KPrefabID> on_add) {
        OnAddPrefab = (Action<KPrefabID>)Delegate.Combine(OnAddPrefab, on_add);
        foreach (var obj in Prefabs) on_add(obj);
    }

    public static void UnregisterOnAddPrefab(Action<KPrefabID> on_add) {
        OnAddPrefab = (Action<KPrefabID>)Delegate.Remove(OnAddPrefab, on_add);
    }

    public static void ClearOnAddPrefab() { OnAddPrefab = null; }

    public static GameObject GetPrefab(Tag tag) {
        var gameObject = TryGetPrefab(tag);
        if (gameObject == null) {
            var str  = "Missing prefab: ";
            var tag2 = tag;
            Debug.LogWarning(str + tag2);
        }

        return gameObject;
    }

    public static GameObject TryGetPrefab(Tag tag) {
        KPrefabID kprefabID = null;
        PrefabsByTag.TryGetValue(tag, out kprefabID);
        if (!(kprefabID != null)) return null;

        return kprefabID.gameObject;
    }

    public static List<GameObject> GetPrefabsWithTag(Tag tag) {
        var list = new List<GameObject>();
        if (PrefabsByAdditionalTags.ContainsKey(tag))
            for (var i = 0; i < PrefabsByAdditionalTags[tag].Count; i++)
                list.Add(PrefabsByAdditionalTags[tag][i].gameObject);

        return list;
    }

    public static List<GameObject> GetPrefabsWithComponent<Type>() {
        var list = new List<GameObject>();
        for (var i = 0; i < Prefabs.Count; i++)
            if (Prefabs[i].GetComponent<Type>() != null)
                list.Add(Prefabs[i].gameObject);

        return list;
    }

    public static GameObject GetPrefabWithComponent<Type>() {
        var prefabsWithComponent = GetPrefabsWithComponent<Type>();
        Debug.Assert(prefabsWithComponent.Count > 0, "There are no prefabs of type " + typeof(Type).Name);
        return prefabsWithComponent[0];
    }

    public static List<Tag> GetPrefabTagsWithComponent<Type>() {
        var list = new List<Tag>();
        for (var i = 0; i < Prefabs.Count; i++)
            if (Prefabs[i].GetComponent<Type>() != null)
                list.Add(Prefabs[i].PrefabID());

        return list;
    }

    public static Assets GetInstanceEditorOnly() {
        var array = (Assets[])Resources.FindObjectsOfTypeAll(typeof(Assets));
        if (array != null) {
            var num = array.Length;
        }

        return array[0];
    }

    public static TextureAtlas GetTextureAtlas(string name) {
        foreach (var textureAtlas in TextureAtlases)
            if (textureAtlas.name == name)
                return textureAtlas;

        return null;
    }

    public static Material GetMaterial(string name) {
        foreach (var material in Materials)
            if (material.name == name)
                return material;

        return null;
    }

    public static BlockTileDecorInfo GetBlockTileDecorInfo(string name) {
        foreach (var blockTileDecorInfo in BlockTileDecorInfos)
            if (blockTileDecorInfo.name == name)
                return blockTileDecorInfo;

        Debug.LogError("Could not find BlockTileDecorInfo named [" + name + "]");
        return null;
    }

    public static KAnimFile GetAnim(HashedString name) {
        if (!name.IsValid) {
            Debug.LogWarning("Invalid hash name");
            return null;
        }

        KAnimFile kanimFile;
        AnimTable.TryGetValue(name, out kanimFile);
        if (kanimFile == null)
            Debug.LogWarning("Missing Anim: [" + name + "]. You may have to run Collect Anim on the Assets prefab");

        return kanimFile;
    }

    public static bool TryGetAnim(HashedString name, out KAnimFile anim) {
        if (!name.IsValid) {
            Debug.LogWarning("Invalid hash name");
            anim = null;
            return false;
        }

        AnimTable.TryGetValue(name, out anim);
        return anim != null;
    }

    public static void AddBuildingDef(BuildingDef def) {
        BuildingDefs = (from x in BuildingDefs where x.PrefabID != def.PrefabID select x).ToList();
        BuildingDefs.Add(def);
    }

    [Serializable]
    public struct UIPrefabData {
        public ProgressBar       ProgressBar;
        public HealthBar         HealthBar;
        public GameObject        ResourceVisualizer;
        public GameObject        KAnimVisualizer;
        public Image             RegionCellBlocked;
        public RectTransform     PriorityOverlayIcon;
        public RectTransform     HarvestWhenReadyOverlayIcon;
        public TableScreenAssets TableScreenWidgets;
    }

    [Serializable]
    public struct TableScreenAssets {
        public Material   DefaultUIMaterial;
        public Material   DesaturatedUIMaterial;
        public GameObject MinionPortrait;
        public GameObject GenericPortrait;
        public GameObject TogglePortrait;
        public GameObject ButtonLabel;
        public GameObject ButtonLabelWhite;
        public GameObject Label;
        public GameObject LabelHeader;
        public GameObject Checkbox;
        public GameObject BlankCell;
        public GameObject SuperCheckbox_Horizontal;
        public GameObject SuperCheckbox_Vertical;
        public GameObject Spacer;
        public GameObject NumericDropDown;
        public GameObject DropDownHeader;
        public GameObject PriorityGroupSelector;
        public GameObject PriorityGroupSelectorHeader;
        public GameObject PrioritizeRowWidget;
        public GameObject PrioritizeRowHeaderWidget;
    }
}