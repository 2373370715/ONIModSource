using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200026A RID: 618
public class RemoteWorkerConfig : IEntityConfig
{
	// Token: 0x06000902 RID: 2306 RVA: 0x000AA527 File Offset: 0x000A8727
	public string[] GetRequiredDlcIds()
	{
		return new string[]
		{
			"DLC3_ID"
		};
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x00164544 File Offset: 0x00162744
	public GameObject CreatePrefab()
	{
		string name = DUPLICANTS.MODEL.REMOTEWORKER.NAME;
		string description = DUPLICANTS.MODEL.REMOTEWORKER.DESC;
		GameObject gameObject = EntityTemplates.CreateEntity(RemoteWorkerConfig.ID, name, true);
		gameObject.AddOrGet<InfoDescription>().description = description;
		gameObject.AddComponent<Accessorizer>();
		gameObject.AddOrGet<WearableAccessorizer>();
		gameObject.AddComponent<StateMachineController>();
		KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
		kbatchedAnimController.defaultAnim = "in_dock_idle";
		kbatchedAnimController.initialAnim = "in_dock_idle";
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("body_comp_default_kanim"),
			Assets.GetAnim("anim_idles_default_kanim"),
			Assets.GetAnim("anim_loco_new_kanim"),
			Assets.GetAnim("anim_interacts_remote_work_dock_kanim")
		};
		gameObject.AddOrGet<AnimEventHandler>();
		SymbolOverrideController symbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		symbolOverrideController.applySymbolOverridesEveryFrame = true;
		symbolOverrideController.AddSymbolOverride("snapto_cheek", Assets.GetAnim("head_swap_kanim").GetData().build.GetSymbol("cheek_007"), 1);
		BaseMinionConfig.ConfigureSymbols(gameObject, true);
		Accessorizer component = gameObject.GetComponent<Accessorizer>();
		component.ApplyBodyData(RemoteWorkerConfig.CreateBodyData());
		component.ApplyAccessories();
		gameObject.AddTag(GameTags.Experimental);
		gameObject.AddTag(GameTags.Robot);
		KBoxCollider2D kboxCollider2D = gameObject.AddOrGet<KBoxCollider2D>();
		kboxCollider2D.size = new Vector2f(1f, 2f);
		kboxCollider2D.offset = new Vector2f(0f, 1f);
		KBoxCollider2D kboxCollider2D2 = gameObject.AddOrGet<KBoxCollider2D>();
		kboxCollider2D2.offset = new Vector2(0f, 0.75f);
		kboxCollider2D2.size = new Vector2(1f, 1.5f);
		Navigator navigator = gameObject.AddOrGet<Navigator>();
		navigator.NavGridName = "WalkerBabyNavGrid";
		navigator.CurrentNavType = NavType.Floor;
		navigator.defaultSpeed = 1f;
		navigator.updateProber = true;
		navigator.maxProbingRadius = 0;
		navigator.sceneLayer = Grid.SceneLayer.Creatures;
		PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
		primaryElement.ElementID = SimHashes.Steel;
		primaryElement.Mass = 200f;
		gameObject.AddComponent<RemoteWorkerExperienceProxy>();
		gameObject.AddComponent<RemoteWorker>();
		gameObject.AddComponent<RemoteWorkerSM>();
		gameObject.AddComponent<ChoreConsumer>();
		gameObject.AddComponent<Pickupable>();
		gameObject.AddComponent<SaveLoadRoot>();
		gameObject.AddComponent<Storage>();
		return gameObject;
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x000AA537 File Offset: 0x000A8737
	public void OnPrefabInit(GameObject go)
	{
		Navigator navigator = go.AddOrGet<Navigator>();
		navigator.SetAbilities(new CreaturePathFinderAbilities(navigator));
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x00164780 File Offset: 0x00162980
	public static KCompBuilder.BodyData CreateBodyData()
	{
		return new KCompBuilder.BodyData
		{
			eyes = HashCache.Get().Add("eyes_014"),
			hair = HashCache.Get().Add("hair_051"),
			headShape = HashCache.Get().Add("headshape_006"),
			mouth = HashCache.Get().Add("mouth_007"),
			neck = HashCache.Get().Add("neck"),
			arms = HashCache.Get().Add("arm_sleeve_006"),
			armslower = HashCache.Get().Add("arm_lower_sleeve_006"),
			body = HashCache.Get().Add("torso_006"),
			hat = HashedString.Invalid,
			faceFX = HashedString.Invalid,
			armLowerSkin = HashCache.Get().Add("arm_lower_001"),
			armUpperSkin = HashCache.Get().Add("arm_upper_001"),
			legSkin = HashCache.Get().Add("leg_skin_001"),
			neck = HashCache.Get().Add("neck_006"),
			legs = HashCache.Get().Add("leg_006"),
			belt = HashCache.Get().Add("belt_006"),
			pelvis = HashCache.Get().Add("pelvis_006"),
			foot = HashCache.Get().Add("foot_006"),
			hand = HashCache.Get().Add("hand_paint_006"),
			cuff = HashCache.Get().Add("cuff_006")
		};
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x000AA54A File Offset: 0x000A874A
	string[] IEntityConfig.GetDlcIds()
	{
		return this.GetRequiredDlcIds();
	}

	// Token: 0x040006BF RID: 1727
	public static string ID = "RemoteWorker";

	// Token: 0x040006C0 RID: 1728
	public const float MASS_KG = 200f;

	// Token: 0x040006C1 RID: 1729
	public const float DEBRIS_MASS_KG = 42f;

	// Token: 0x040006C2 RID: 1730
	public static string IDLE_IN_DOCK_ANIM = "idle_in_dock";

	// Token: 0x040006C3 RID: 1731
	public static readonly string BUILD_MATERIAL = "Steel";

	// Token: 0x040006C4 RID: 1732
	public static readonly Tag BUILD_MATERIAL_TAG = new Tag(RemoteWorkerConfig.BUILD_MATERIAL);
}
