using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000231 RID: 561
public class GlomConfig : IEntityConfig
{
	// Token: 0x060007B7 RID: 1975 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x00160A10 File Offset: 0x0015EC10
	public GameObject CreatePrefab()
	{
		string text = STRINGS.CREATURES.SPECIES.GLOM.NAME;
		string id = "Glom";
		string name = text;
		string desc = STRINGS.CREATURES.SPECIES.GLOM.DESC;
		float mass = 25f;
		EffectorValues tier = DECOR.BONUS.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("glom_kanim"), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, tier, default(EffectorValues), SimHashes.Creature, null, 293f);
		Db.Get().CreateTrait("GlomBaseTrait", text, text, null, false, null, true, true).Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, text, false, false, true));
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Creatures.Walker, false);
		component.AddTag(GameTags.OriginalCreature, false);
		component.prefabInitFn += delegate(GameObject inst)
		{
			inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost);
		};
		EntityTemplates.ExtendEntityToBasicCreature(gameObject, FactionManager.FactionID.Pest, "GlomBaseTrait", "WalkerNavGrid1x1", NavType.Floor, 32, 2f, "", 0, true, true, 303.15f, 373.15f, 273.15f, 473.15f);
		gameObject.AddWeapon(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0f);
		Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
		int sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["Glom"];
		pickupable.sortOrder = sortOrder;
		gameObject.AddOrGet<Trappable>();
		gameObject.AddOrGetDef<ThreatMonitor.Def>();
		gameObject.AddOrGetDef<CreatureFallMonitor.Def>();
		ElementDropperMonitor.Def def = gameObject.AddOrGetDef<ElementDropperMonitor.Def>();
		def.dirtyEmitElement = SimHashes.ContaminatedOxygen;
		def.dirtyProbabilityPercent = 25f;
		def.dirtyCellToTargetMass = 1f;
		def.dirtyMassPerDirty = 0.2f;
		def.dirtyMassReleaseOnDeath = 3f;
		def.emitDiseaseIdx = Db.Get().Diseases.GetIndex("SlimeLung");
		def.emitDiseasePerKg = 1000f;
		gameObject.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.GetComponent<LoopingSounds>().updatePosition = true;
		gameObject.AddOrGet<DiseaseSourceVisualizer>().alwaysShowDisease = "SlimeLung";
		SoundEventVolumeCache.instance.AddVolume("glom_kanim", "Morb_movement_short", NOISE_POLLUTION.CREATURES.TIER2);
		SoundEventVolumeCache.instance.AddVolume("glom_kanim", "Morb_jump", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("glom_kanim", "Morb_land", NOISE_POLLUTION.CREATURES.TIER3);
		SoundEventVolumeCache.instance.AddVolume("glom_kanim", "Morb_expel", NOISE_POLLUTION.CREATURES.TIER4);
		EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, true, false, false);
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new DeathStates.Def(), true, -1).Add(new TrappedStates.Def(), true, -1).Add(new BaggedStates.Def(), true, -1).Add(new FallStates.Def(), true, -1).Add(new StunnedStates.Def(), true, -1).Add(new DrowningStates.Def(), true, -1).Add(new DebugGoToStates.Def(), true, -1).Add(new FleeStates.Def(), true, -1).Add(new DropElementStates.Def(), true, -1).Add(new IdleStates.Def(), true, -1);
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Creatures.Species.GlomSpecies, null);
		return gameObject;
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject prefab)
	{
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040005C9 RID: 1481
	public const string ID = "Glom";

	// Token: 0x040005CA RID: 1482
	public const string BASE_TRAIT_ID = "GlomBaseTrait";

	// Token: 0x040005CB RID: 1483
	public const SimHashes dirtyEmitElement = SimHashes.ContaminatedOxygen;

	// Token: 0x040005CC RID: 1484
	public const float dirtyProbabilityPercent = 25f;

	// Token: 0x040005CD RID: 1485
	public const float dirtyCellToTargetMass = 1f;

	// Token: 0x040005CE RID: 1486
	public const float dirtyMassPerDirty = 0.2f;

	// Token: 0x040005CF RID: 1487
	public const float dirtyMassReleaseOnDeath = 3f;

	// Token: 0x040005D0 RID: 1488
	public const string emitDisease = "SlimeLung";

	// Token: 0x040005D1 RID: 1489
	public const int emitDiseasePerKg = 1000;
}
