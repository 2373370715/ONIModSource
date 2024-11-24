using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class BaseStaterpillarConfig
{
	public static GameObject BaseStaterpillar(string id, string name, string desc, string anim_file, string trait_id, bool is_baby, ObjectLayer conduitLayer, string connectorDefId, Tag inhaleTag, string symbolOverridePrefix = null, float warningLowTemperature = 283.15f, float warningHighTemperature = 293.15f, float lethalLowTemperature = 243.15f, float lethalHighTemperature = 343.15f, InhaleStates.Def inhaleDef = null)
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, 200f, decor: DECOR.BONUS.TIER0, anim: Assets.GetAnim(anim_file), initialAnim: "idle_loop", sceneLayer: Grid.SceneLayer.Creatures, width: 1, height: 1);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Walker);
		gameObject.AddTag(GameTags.Amphibious);
		string navGridName = "WalkerBabyNavGrid";
		if (!is_baby)
		{
			navGridName = "DreckoNavGrid";
			gameObject.AddOrGetDef<ConduitSleepMonitor.Def>().conduitLayer = conduitLayer;
		}
		EntityTemplates.ExtendEntityToBasicCreature(gameObject, FactionManager.FactionID.Pest, trait_id, navGridName, NavType.Floor, 32, 1f, "Meat", 2, drownVulnerable: false, entombVulnerable: false, warningLowTemperature, warningHighTemperature, lethalLowTemperature, lethalHighTemperature);
		if (symbolOverridePrefix != null)
		{
			gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim(anim_file), symbolOverridePrefix);
		}
		Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
		int sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["Staterpillar"];
		pickupable.sortOrder = sortOrder;
		gameObject.AddOrGet<Trappable>();
		gameObject.AddOrGetDef<CreatureFallMonitor.Def>();
		gameObject.AddOrGet<LoopingSounds>();
		Staterpillar staterpillar = gameObject.AddOrGet<Staterpillar>();
		staterpillar.conduitLayer = conduitLayer;
		staterpillar.connectorDefId = connectorDefId;
		gameObject.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
		gameObject.AddWeapon(1f, 1f);
		EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, must_stand_on_top_for_pickup: true, allow_mark_for_capture: true);
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new DeathStates.Def()).Add(new AnimInterruptStates.Def()).Add(new GrowUpStates.Def(), is_baby)
			.Add(new TrappedStates.Def())
			.Add(new IncubatingStates.Def(), is_baby)
			.Add(new BaggedStates.Def())
			.Add(new FallStates.Def())
			.Add(new StunnedStates.Def())
			.Add(new DebugGoToStates.Def())
			.Add(new FleeStates.Def())
			.Add(new AttackStates.Def(), !is_baby)
			.Add(new FixedCaptureStates.Def())
			.Add(new RanchedStates.Def
			{
				WaitCellOffset = 2
			}, !is_baby)
			.PushInterruptGroup()
			.Add(new LayEggStates.Def(), !is_baby)
			.Add(new EatStates.Def())
			.Add(new DrinkMilkStates.Def())
			.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, loop: false, "poop", STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP))
			.Add(inhaleDef, inhaleTag != Tag.Invalid)
			.Add(new ConduitSleepStates.Def())
			.Add(new CallAdultStates.Def(), is_baby)
			.Add(new CritterCondoStates.Def(), !is_baby)
			.PopInterruptGroup()
			.Add(new CreatureSleepStates.Def())
			.Add(new IdleStates.Def
			{
				customIdleAnim = CustomIdleAnim
			});
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Creatures.Species.StaterpillarSpecies, symbolOverridePrefix);
		return gameObject;
	}

	public static GameObject SetupDiet(GameObject prefab, List<Diet.Info> diet_infos)
	{
		Diet diet = new Diet(diet_infos.ToArray());
		prefab.AddOrGetDef<CreatureCalorieMonitor.Def>().diet = diet;
		prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		return prefab;
	}

	public static List<Diet.Info> RawMetalDiet(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId, float diseasePerKgProduced)
	{
		List<SimHashes> list = new List<SimHashes> { SimHashes.FoolsGold };
		List<Diet.Info> list2 = new List<Diet.Info>();
		foreach (Element element in ElementLoader.elements)
		{
			if (element.IsSolid && element.materialCategory == GameTags.Metal && element.HasTag(GameTags.Ore) && !element.disabled && !list.Contains(element.id))
			{
				list2.Add(new Diet.Info(new HashSet<Tag>(new Tag[1] { element.tag }), poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced));
			}
		}
		return list2;
	}

	public static List<Diet.Info> RefinedMetalDiet(Tag poopTag, float caloriesPerKg, float producedConversionRate, string diseaseId, float diseasePerKgProduced)
	{
		List<Diet.Info> list = new List<Diet.Info>();
		foreach (Element element in ElementLoader.elements)
		{
			if (element.IsSolid && element.materialCategory == GameTags.RefinedMetal && !element.disabled)
			{
				list.Add(new Diet.Info(new HashSet<Tag>(new Tag[1] { element.tag }), poopTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced));
			}
		}
		return list;
	}

	private static HashedString CustomIdleAnim(IdleStates.Instance smi, ref HashedString pre_anim)
	{
		CellOffset offset = new CellOffset(0, -1);
		bool facing = smi.GetComponent<Facing>().GetFacing();
		switch (smi.GetComponent<Navigator>().CurrentNavType)
		{
		case NavType.Floor:
			offset = (facing ? new CellOffset(1, -1) : new CellOffset(-1, -1));
			break;
		case NavType.Ceiling:
			offset = (facing ? new CellOffset(1, 1) : new CellOffset(-1, 1));
			break;
		}
		HashedString result = "idle_loop";
		int num = Grid.OffsetCell(Grid.PosToCell(smi), offset);
		if (Grid.IsValidCell(num) && !Grid.Solid[num])
		{
			pre_anim = "idle_loop_hang_pre";
			result = "idle_loop_hang";
		}
		return result;
	}
}
