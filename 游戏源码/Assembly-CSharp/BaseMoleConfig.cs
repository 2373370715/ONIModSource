using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020000EE RID: 238
public static class BaseMoleConfig
{
	// Token: 0x060003C6 RID: 966 RVA: 0x00151E30 File Offset: 0x00150030
	public static GameObject BaseMole(string id, string name, string desc, string traitId, string anim_file, bool is_baby, float warningLowTemperature, float warningHighTemperature, float lethalLowTemperature, float lethalHighTemperature, string symbolOverridePrefix = null, int on_death_drop_count = 10)
	{
		float mass = 25f;
		EffectorValues none = TUNING.BUILDINGS.DECOR.NONE;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim(anim_file), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, none, default(EffectorValues), SimHashes.Creature, null, 293f);
		EntityTemplates.ExtendEntityToBasicCreature(gameObject, FactionManager.FactionID.Pest, traitId, "DiggerNavGrid", NavType.Floor, 32, 2f, "Meat", on_death_drop_count, true, false, warningLowTemperature, warningHighTemperature, lethalLowTemperature, lethalHighTemperature);
		if (symbolOverridePrefix != null)
		{
			gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim(anim_file), symbolOverridePrefix, null, 0);
		}
		Pickupable pickupable = gameObject.AddOrGet<Pickupable>();
		int sortOrder = TUNING.CREATURES.SORTING.CRITTER_ORDER["Mole"];
		pickupable.sortOrder = sortOrder;
		gameObject.AddOrGetDef<CreatureFallMonitor.Def>();
		gameObject.AddOrGet<Trappable>();
		gameObject.AddOrGetDef<DiggerMonitor.Def>().depthToDig = MoleTuning.DEPTH_TO_HIDE;
		EntityTemplates.CreateAndRegisterBaggedCreature(gameObject, true, true, false);
		gameObject.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Walker, false);
		ChoreTable.Builder chore_table = new ChoreTable.Builder().Add(new DeathStates.Def(), true, -1).Add(new AnimInterruptStates.Def(), true, -1).Add(new FallStates.Def(), true, -1).Add(new StunnedStates.Def(), true, -1).Add(new DrowningStates.Def(), true, -1).Add(new DiggerStates.Def(), true, -1).Add(new GrowUpStates.Def(), is_baby, -1).Add(new TrappedStates.Def(), true, -1).Add(new IncubatingStates.Def(), is_baby, -1).Add(new BaggedStates.Def(), true, -1).Add(new DebugGoToStates.Def(), true, -1).Add(new FleeStates.Def(), true, -1).Add(new AttackStates.Def("eat_pre", "eat_pst", null), !is_baby, -1).PushInterruptGroup().Add(new FixedCaptureStates.Def(), true, -1).Add(new RanchedStates.Def(), !is_baby, -1).Add(new LayEggStates.Def(), !is_baby, -1).Add(new CreatureSleepStates.Def(), true, -1).Add(new EatStates.Def(), true, -1).Add(new DrinkMilkStates.Def
		{
			shouldBeBehindMilkTank = is_baby
		}, true, -1).Add(new NestingPoopState.Def(is_baby ? Tag.Invalid : SimHashes.Regolith.CreateTag()), true, -1).Add(new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP), true, -1).Add(new CritterCondoStates.Def(), !is_baby, -1).PopInterruptGroup().Add(new IdleStates.Def
		{
			customIdleAnim = new IdleStates.Def.IdleAnimCallback(BaseMoleConfig.CustomIdleAnim)
		}, true, -1);
		EntityTemplates.AddCreatureBrain(gameObject, chore_table, GameTags.Creatures.Species.MoleSpecies, symbolOverridePrefix);
		return gameObject;
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x001520B8 File Offset: 0x001502B8
	public static List<Diet.Info> SimpleOreDiet(List<Tag> elementTags, float caloriesPerKg, float producedConversionRate)
	{
		List<Diet.Info> list = new List<Diet.Info>();
		foreach (Tag tag in elementTags)
		{
			list.Add(new Diet.Info(new HashSet<Tag>
			{
				tag
			}, tag, caloriesPerKg, producedConversionRate, null, 0f, true, Diet.Info.FoodType.EatSolid, false, null));
		}
		return list;
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x0015212C File Offset: 0x0015032C
	private static HashedString CustomIdleAnim(IdleStates.Instance smi, ref HashedString pre_anim)
	{
		if (smi.gameObject.GetComponent<Navigator>().CurrentNavType == NavType.Solid)
		{
			int num = UnityEngine.Random.Range(0, BaseMoleConfig.SolidIdleAnims.Length);
			return BaseMoleConfig.SolidIdleAnims[num];
		}
		if (smi.gameObject.GetDef<BabyMonitor.Def>() != null && UnityEngine.Random.Range(0, 100) >= 90)
		{
			return "drill_fail";
		}
		return "idle_loop";
	}

	// Token: 0x0400028E RID: 654
	private static readonly string[] SolidIdleAnims = new string[]
	{
		"idle1",
		"idle2",
		"idle3",
		"idle4"
	};
}
