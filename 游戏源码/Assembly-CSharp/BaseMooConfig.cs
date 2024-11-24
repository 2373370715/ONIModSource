using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public static class BaseMooConfig
{
	public static GameObject BaseMoo(string id, string name, string desc, string traitId, string anim_file, bool is_baby, string symbol_override_prefix)
	{
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, 50f, decor: DECOR.BONUS.TIER0, anim: Assets.GetAnim(anim_file), initialAnim: "idle_loop", sceneLayer: Grid.SceneLayer.Creatures, width: 2, height: 2);
		EntityTemplates.ExtendEntityToBasicCreature(gameObject, FactionManager.FactionID.Prey, traitId, "FlyerNavGrid2x2", NavType.Hover, 32, 2f, "Meat", 10, drownVulnerable: true, entombVulnerable: true, 223.15f, 323.15f, 73.149994f, 473.15f);
		if (!string.IsNullOrEmpty(symbol_override_prefix))
		{
			gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim(anim_file), symbol_override_prefix);
		}
		Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
		int sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["Moo"];
		pickupable.sortOrder = sortOrder;
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		component.AddTag(GameTags.Creatures.Flyer);
		component.prefabInitFn += delegate(GameObject inst)
		{
			inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost);
		};
		gameObject.AddOrGetDef<BeckoningMonitor.Def>().caloriesPerCycle = MooTuning.WELLFED_CALORIES_PER_CYCLE;
		gameObject.AddOrGet<LoopingSounds>();
		gameObject.AddOrGet<Trappable>();
		gameObject.AddOrGetDef<LureableMonitor.Def>().lures = new Tag[2]
		{
			SimHashes.BleachStone.CreateTag(),
			GameTags.Creatures.FlyersLure
		};
		gameObject.AddOrGetDef<ThreatMonitor.Def>();
		gameObject.AddOrGetDef<SubmergedMonitor.Def>();
		EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, must_stand_on_top_for_pickup: true, allow_mark_for_capture: true);
		gameObject.AddOrGetDef<RanchableMonitor.Def>();
		gameObject.AddOrGetDef<FixedCapturableMonitor.Def>();
		MilkProductionMonitor.Def def = gameObject.AddOrGetDef<MilkProductionMonitor.Def>();
		def.CaloriesPerCycle = MooTuning.WELLFED_CALORIES_PER_CYCLE;
		def.Capacity = MooTuning.MILK_CAPACITY;
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new DeathStates.Def()).Add(new AnimInterruptStates.Def()).Add(new TrappedStates.Def())
			.Add(new BaggedStates.Def())
			.Add(new StunnedStates.Def())
			.Add(new DebugGoToStates.Def())
			.Add(new DrowningStates.Def())
			.PushInterruptGroup()
			.Add(new BeckonFromSpaceStates.Def
			{
				prefab = GassyMooCometConfig.ID
			})
			.Add(new CreatureSleepStates.Def())
			.Add(new FixedCaptureStates.Def())
			.Add(new RanchedStates.Def
			{
				WaitCellOffset = 2
			})
			.Add(new EatStates.Def())
			.Add(new DrinkMilkStates.Def
			{
				shouldBeBehindMilkTank = false,
				drinkCellOffsetGetFn = DrinkMilkStates.Def.DrinkCellOffsetGet_GassyMoo
			})
			.Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, loop: false, "poop", STRINGS.CREATURES.STATUSITEMS.EXPELLING_GAS.NAME, STRINGS.CREATURES.STATUSITEMS.EXPELLING_GAS.TOOLTIP))
			.Add(new MoveToLureStates.Def())
			.Add(new CritterCondoStates.Def
			{
				working_anim = "cc_working_moo"
			}, !is_baby)
			.PopInterruptGroup()
			.Add(new IdleStates.Def
			{
				customIdleAnim = CustomIdleAnim
			});
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Creatures.Species.MooSpecies, symbol_override_prefix);
		gameObject.AddOrGetDef<CritterCondoInteractMontior.Def>().condoPrefabTag = "AirBorneCritterCondo";
		return gameObject;
	}

	public static GameObject SetupDiet(GameObject prefab, Tag consumed_tag, Tag producedTag, float caloriesPerKg, float producedConversionRate, string diseaseId, float diseasePerKgProduced, float minPoopSizeInKg)
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		hashSet.Add(consumed_tag);
		Diet diet = new Diet(new Diet.Info(hashSet, producedTag, caloriesPerKg, producedConversionRate, diseaseId, diseasePerKgProduced));
		CreatureCalorieMonitor.Def def = prefab.AddOrGetDef<CreatureCalorieMonitor.Def>();
		def.diet = diet;
		def.minConsumedCaloriesBeforePooping = minPoopSizeInKg * caloriesPerKg;
		prefab.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		return prefab;
	}

	private static HashedString CustomIdleAnim(IdleStates.Instance smi, ref HashedString pre_anim)
	{
		CreatureCalorieMonitor.Instance sMI = smi.GetSMI<CreatureCalorieMonitor.Instance>();
		return (sMI != null && sMI.stomach.IsReadyToPoop()) ? "idle_loop_full" : "idle_loop";
	}

	public static void OnSpawn(GameObject inst)
	{
		Navigator component = inst.GetComponent<Navigator>();
		component.transitionDriver.overrideLayers.Add(new FullPuftTransitionLayer(component));
	}
}
