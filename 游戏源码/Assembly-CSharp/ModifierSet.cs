using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020007CE RID: 1998
public class ModifierSet : ScriptableObject
{
	// Token: 0x060023BD RID: 9149 RVA: 0x001C64D0 File Offset: 0x001C46D0
	public virtual void Initialize()
	{
		this.ResourceTable = new List<Resource>();
		this.Root = new ResourceSet<Resource>("Root", null);
		this.modifierInfos = new ModifierSet.ModifierInfos();
		this.modifierInfos.Load(this.modifiersFile);
		this.Attributes = new Database.Attributes(this.Root);
		this.BuildingAttributes = new BuildingAttributes(this.Root);
		this.CritterAttributes = new CritterAttributes(this.Root);
		this.PlantAttributes = new PlantAttributes(this.Root);
		this.effects = new ResourceSet<Effect>("Effects", this.Root);
		this.traits = new ModifierSet.TraitSet();
		this.traitGroups = new ModifierSet.TraitGroupSet();
		this.FertilityModifiers = new FertilityModifiers();
		this.Amounts = new Database.Amounts();
		this.Amounts.Load();
		this.AttributeConverters = new Database.AttributeConverters();
		this.LoadEffects();
		this.LoadFertilityModifiers();
	}

	// Token: 0x060023BE RID: 9150 RVA: 0x000B73E0 File Offset: 0x000B55E0
	public static float ConvertValue(float value, Units units)
	{
		if (Units.PerDay == units)
		{
			return value * 0.0016666667f;
		}
		return value;
	}

	// Token: 0x060023BF RID: 9151 RVA: 0x001C65C0 File Offset: 0x001C47C0
	private void LoadEffects()
	{
		foreach (ModifierSet.ModifierInfo modifierInfo in this.modifierInfos)
		{
			if (!this.effects.Exists(modifierInfo.Id) && (modifierInfo.Type == "Effect" || modifierInfo.Type == "Base" || modifierInfo.Type == "Need"))
			{
				string text = Strings.Get(string.Format("STRINGS.DUPLICANTS.MODIFIERS.{0}.NAME", modifierInfo.Id.ToUpper()));
				string description = Strings.Get(string.Format("STRINGS.DUPLICANTS.MODIFIERS.{0}.TOOLTIP", modifierInfo.Id.ToUpper()));
				Effect effect = new Effect(modifierInfo.Id, text, description, modifierInfo.Duration * 600f, modifierInfo.ShowInUI && modifierInfo.Type != "Need", modifierInfo.TriggerFloatingText, modifierInfo.IsBad, modifierInfo.EmoteAnim, modifierInfo.EmoteCooldown, modifierInfo.StompGroup, modifierInfo.CustomIcon);
				effect.stompPriority = modifierInfo.StompPriority;
				foreach (ModifierSet.ModifierInfo modifierInfo2 in this.modifierInfos)
				{
					if (modifierInfo2.Id == modifierInfo.Id)
					{
						effect.Add(new AttributeModifier(modifierInfo2.Attribute, ModifierSet.ConvertValue(modifierInfo2.Value, modifierInfo2.Units), text, modifierInfo2.Multiplier, false, true));
					}
				}
				this.effects.Add(effect);
			}
		}
		Reactable.ReactablePrecondition precon = delegate(GameObject go, Navigator.ActiveTransition n)
		{
			int cell = Grid.PosToCell(go);
			return Grid.IsValidCell(cell) && Grid.IsGas(cell);
		};
		this.effects.Get("WetFeet").AddEmotePrecondition(precon);
		this.effects.Get("SoakingWet").AddEmotePrecondition(precon);
		Effect effect2 = new Effect("PassedOutSleep", DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.NAME, DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.TOOLTIP, 0f, true, true, true, null, 0f, null, true, "status_item_exhausted", -1f);
		effect2.Add(new AttributeModifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, 0.6666667f, DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.NAME, false, false, true));
		effect2.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.033333335f, DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.NAME, false, false, true));
		this.effects.Add(effect2);
		Effect resource = new Effect("WarmTouch", DUPLICANTS.MODIFIERS.WARMTOUCH.NAME, DUPLICANTS.MODIFIERS.WARMTOUCH.TOOLTIP, 120f, new string[]
		{
			"WetFeet"
		}, true, true, false, null, 0f, null, false, "", -1f);
		this.effects.Add(resource);
		Effect resource2 = new Effect("WarmTouchFood", DUPLICANTS.MODIFIERS.WARMTOUCHFOOD.NAME, DUPLICANTS.MODIFIERS.WARMTOUCHFOOD.TOOLTIP, 600f, new string[]
		{
			"WetFeet"
		}, true, true, false, null, 0f, null, false, "", -1f);
		this.effects.Add(resource2);
		Effect resource3 = new Effect("RefreshingTouch", DUPLICANTS.MODIFIERS.REFRESHINGTOUCH.NAME, DUPLICANTS.MODIFIERS.REFRESHINGTOUCH.TOOLTIP, 120f, true, true, false, null, -1f, 0f, null, "");
		this.effects.Add(resource3);
		Effect effect3 = new Effect("GunkSick", DUPLICANTS.MODIFIERS.GUNKSICK.NAME, DUPLICANTS.MODIFIERS.GUNKSICK.TOOLTIP, 0f, true, true, true, null, -1f, 0f, null, "");
		effect3.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.033333335f, DUPLICANTS.MODIFIERS.GUNKSICK.NAME, false, false, true));
		this.effects.Add(effect3);
		Effect effect4 = new Effect("ExpellingGunk", DUPLICANTS.MODIFIERS.EXPELLINGGUNK.NAME, DUPLICANTS.MODIFIERS.EXPELLINGGUNK.TOOLTIP, 0f, true, true, true, null, -1f, 0f, null, "");
		effect4.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.083333336f, DUPLICANTS.MODIFIERS.GUNKSICK.NAME, false, false, true));
		this.effects.Add(effect4);
		Effect effect5 = new Effect("GunkHungover", DUPLICANTS.MODIFIERS.GUNKHUNGOVER.NAME, DUPLICANTS.MODIFIERS.GUNKHUNGOVER.TOOLTIP, 600f, true, false, true, null, -1f, 0f, null, "");
		effect5.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.033333335f, DUPLICANTS.MODIFIERS.GUNKHUNGOVER.NAME, false, false, true));
		this.effects.Add(effect5);
		Effect effect6 = new Effect("NoLubrication", DUPLICANTS.MODIFIERS.NOLUBRICATION.NAME, DUPLICANTS.MODIFIERS.NOLUBRICATION.TOOLTIP, 0f, true, true, true, null, -1f, 0f, null, "");
		effect6.Add(new AttributeModifier(Db.Get().Attributes.Athletics.Id, -5f, DUPLICANTS.MODIFIERS.NOLUBRICATION.NAME, false, false, true));
		this.effects.Add(effect6);
		Effect effect7 = new Effect("BionicOffline", DUPLICANTS.MODIFIERS.BIONICOFFLINE.NAME, DUPLICANTS.MODIFIERS.BIONICOFFLINE.TOOLTIP, 0f, false, true, true, null, -1f, 0f, null, "");
		effect7.Add(new AttributeModifier(Db.Get().Amounts.BionicOil.deltaAttribute.Id, 0f, DUPLICANTS.MODIFIERS.BIONICOFFLINE.NAME, false, false, true));
		this.effects.Add(effect7);
		Effect effect8 = new Effect("BionicBatterySaveMode", DUPLICANTS.MODIFIERS.BIONICBATTERYSAVEMODE.NAME, DUPLICANTS.MODIFIERS.BIONICBATTERYSAVEMODE.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		effect8.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.033333335f, DUPLICANTS.MODIFIERS.BIONICBATTERYSAVEMODE.NAME, false, false, true));
		this.effects.Add(effect8);
		Effect effect9 = new Effect("WaterDamage", DUPLICANTS.MODIFIERS.WATERDAMAGE.NAME, DUPLICANTS.MODIFIERS.WATERDAMAGE.TOOLTIP, 120f, true, true, true, null, -1f, 0f, null, "");
		effect9.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.016666668f, DUPLICANTS.MODIFIERS.WATERDAMAGE.NAME, false, false, true));
		this.effects.Add(effect9);
		Effect effect10 = new Effect("Slipped", DUPLICANTS.MODIFIERS.SLIPPED.NAME, DUPLICANTS.MODIFIERS.SLIPPED.TOOLTIP, 100f, false, true, true, null, -1f, 0f, null, "");
		effect10.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.016666668f, DUPLICANTS.MODIFIERS.SLIPPED.NAME, false, false, true));
		this.effects.Add(effect10);
		foreach (Effect resource4 in BionicOilMonitor.LUBRICANT_TYPE_EFFECT.Values)
		{
			this.effects.Add(resource4);
		}
		this.CreateRoomEffects();
		this.CreateCritteEffects();
		this.CreateBionicBoosterEffects();
	}

	// Token: 0x060023C0 RID: 9152 RVA: 0x001C6E0C File Offset: 0x001C500C
	private void CreateRoomEffects()
	{
		Effect effect = new Effect("RoomBionicUpkeep", DUPLICANTS.MODIFIERS.ROOMBIONICUPKEEP.NAME, DUPLICANTS.MODIFIERS.ROOMBIONICUPKEEP.TOOLTIP, 120f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, 2f, DUPLICANTS.MODIFIERS.ROOMBIONICUPKEEP.NAME, false, false, true));
		this.effects.Add(effect);
	}

	// Token: 0x060023C1 RID: 9153 RVA: 0x001C6E90 File Offset: 0x001C5090
	public void CreateCritteEffects()
	{
		Effect effect = new Effect("Ranched", STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, STRINGS.CREATURES.MODIFIERS.RANCHED.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, false, false, true));
		effect.Add(new AttributeModifier(Db.Get().Amounts.Wildness.deltaAttribute.Id, -0.09166667f, STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, false, false, true));
		this.effects.Add(effect);
		Effect effect2 = new Effect("HadMilk", STRINGS.CREATURES.MODIFIERS.GOTMILK.NAME, STRINGS.CREATURES.MODIFIERS.GOTMILK.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect2.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, STRINGS.CREATURES.MODIFIERS.GOTMILK.NAME, false, false, true));
		this.effects.Add(effect2);
		Effect effect3 = new Effect("EggSong", STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.NAME, STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.TOOLTIP, 600f, true, false, false, null, -1f, 0f, null, "");
		effect3.Add(new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, 4f, STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.NAME, true, false, true));
		this.effects.Add(effect3);
		Effect effect4 = new Effect("EggHug", STRINGS.CREATURES.MODIFIERS.EGGHUG.NAME, STRINGS.CREATURES.MODIFIERS.EGGHUG.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect4.Add(new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, 1f, STRINGS.CREATURES.MODIFIERS.EGGHUG.NAME, true, false, true));
		this.effects.Add(effect4);
		Effect resource = new Effect("HuggingFrenzy", STRINGS.CREATURES.MODIFIERS.HUGGINGFRENZY.NAME, STRINGS.CREATURES.MODIFIERS.HUGGINGFRENZY.TOOLTIP, 600f, true, false, false, null, -1f, 0f, null, "");
		this.effects.Add(resource);
		Effect effect5 = new Effect("DivergentCropTended", STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.NAME, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect5.Add(new AttributeModifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, 0.05f, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.NAME, true, false, true));
		this.effects.Add(effect5);
		Effect effect6 = new Effect("DivergentCropTendedWorm", STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.NAME, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.TOOLTIP, 600f, true, true, false, null, -1f, 0f, null, "");
		effect6.Add(new AttributeModifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, 0.5f, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.NAME, true, false, true));
		this.effects.Add(effect6);
		Effect effect7 = new Effect("MooWellFed", STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.MOOWELLFED.TOOLTIP, 1f, true, true, false, null, -1f, 0f, null, "");
		effect7.Add(new AttributeModifier(Db.Get().Amounts.Beckoning.deltaAttribute.Id, MooTuning.WELLFED_EFFECT, STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME, false, false, true));
		effect7.Add(new AttributeModifier(Db.Get().Amounts.MilkProduction.deltaAttribute.Id, MooTuning.MILK_PRODUCTION_PERCENTAGE_PER_SECOND, STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME, false, false, true));
		this.effects.Add(effect7);
		Effect effect8 = new Effect("WoodDeerWellFed", STRINGS.CREATURES.MODIFIERS.WOODDEERWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.WOODDEERWELLFED.TOOLTIP, 1f, true, true, false, null, -1f, 0f, null, "");
		effect8.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, 100f / (WoodDeerConfig.ANTLER_GROWTH_TIME_IN_CYCLES * 600f), STRINGS.CREATURES.MODIFIERS.WOODDEERWELLFED.NAME, false, false, true));
		this.effects.Add(effect8);
		Effect effect9 = new Effect("IceBellyWellFed", STRINGS.CREATURES.MODIFIERS.ICEBELLYWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.ICEBELLYWELLFED.TOOLTIP, 1f, true, true, false, null, -1f, 0f, null, "");
		effect9.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, 100f / (IceBellyConfig.SCALE_GROWTH_TIME_IN_CYCLES * 600f), STRINGS.CREATURES.MODIFIERS.ICEBELLYWELLFED.NAME, false, false, true));
		this.effects.Add(effect9);
		Effect effect10 = new Effect("GoldBellyWellFed", STRINGS.CREATURES.MODIFIERS.GOLDBELLYWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.GOLDBELLYWELLFED.TOOLTIP, 1f, true, true, false, null, -1f, 0f, null, "");
		effect10.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, 100f / (GoldBellyConfig.SCALE_GROWTH_TIME_IN_CYCLES * 600f), STRINGS.CREATURES.MODIFIERS.GOLDBELLYWELLFED.NAME, false, false, true));
		this.effects.Add(effect10);
	}

	// Token: 0x060023C2 RID: 9154 RVA: 0x001C744C File Offset: 0x001C564C
	public void CreateBionicBoosterEffects()
	{
		this.CreateAndAddBionicBoosterEffect("BionicPilotingBoost", "SpaceNavigation", 10f);
		this.CreateAndAddBionicBoosterEffect("BionicConstructionBoost", "Construction", 10f);
		this.CreateAndAddBionicBoosterEffect("BionicExcavationBoost", "Digging", 10f);
		this.CreateAndAddBionicBoosterEffect("BionicMachineryBoost", "Machinery", 10f);
		this.CreateAndAddBionicBoosterEffect("BionicAthleticsBoost", "Athletics", 10f);
		this.CreateAndAddBionicBoosterEffect("BionicScienceBoost", "Learning", 10f);
		this.CreateAndAddBionicBoosterEffect("BionicCookingBoost", "Cooking", 10f);
		this.CreateAndAddBionicBoosterEffect("BionicMedicineBoost", "Caring", 10f);
		this.CreateAndAddBionicBoosterEffect("BionicStrengthBoost", "Strength", 10f);
		this.CreateAndAddBionicBoosterEffect("BionicCreativityBoost", "Art", 10f);
		this.CreateAndAddBionicBoosterEffect("BionicAgricultureBoost", "Botanist", 10f);
		this.CreateAndAddBionicBoosterEffect("BionicHusbandryBoost", "Ranching", 10f);
	}

	// Token: 0x060023C3 RID: 9155 RVA: 0x001C7564 File Offset: 0x001C5764
	public Effect CreateAndAddBionicBoosterEffect(string name, string attributeID, float value)
	{
		Effect effect = new Effect(name, Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + name.ToUpper() + ".NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + name.ToUpper() + ".TOOLTIP"), 0f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier(attributeID, value, Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + name.ToUpper() + ".NAME"), false, false, true));
		this.effects.Add(effect);
		return effect;
	}

	// Token: 0x060023C4 RID: 9156 RVA: 0x001C760C File Offset: 0x001C580C
	public Trait CreateTrait(string id, string name, string description, string group_name, bool should_save, ChoreGroup[] disabled_chore_groups, bool positive_trait, bool is_valid_starter_trait)
	{
		Trait trait = new Trait(id, name, description, 0f, should_save, disabled_chore_groups, positive_trait, is_valid_starter_trait);
		this.traits.Add(trait);
		if (group_name == "" || group_name == null)
		{
			group_name = "Default";
		}
		TraitGroup traitGroup = this.traitGroups.TryGet(group_name);
		if (traitGroup == null)
		{
			traitGroup = new TraitGroup(group_name, group_name, group_name != "Default");
			this.traitGroups.Add(traitGroup);
		}
		traitGroup.Add(trait);
		return trait;
	}

	// Token: 0x060023C5 RID: 9157 RVA: 0x001C7694 File Offset: 0x001C5894
	public FertilityModifier CreateFertilityModifier(string id, Tag targetTag, string name, string description, Func<string, string> tooltipCB, FertilityModifier.FertilityModFn applyFunction)
	{
		FertilityModifier fertilityModifier = new FertilityModifier(id, targetTag, name, description, tooltipCB, applyFunction);
		this.FertilityModifiers.Add(fertilityModifier);
		return fertilityModifier;
	}

	// Token: 0x060023C6 RID: 9158 RVA: 0x000B73EF File Offset: 0x000B55EF
	protected void LoadTraits()
	{
		TRAITS.TRAIT_CREATORS.ForEach(delegate(System.Action action)
		{
			action();
		});
	}

	// Token: 0x060023C7 RID: 9159 RVA: 0x000B741A File Offset: 0x000B561A
	protected void LoadFertilityModifiers()
	{
		TUNING.CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS.ForEach(delegate(System.Action action)
		{
			action();
		});
	}

	// Token: 0x040017B7 RID: 6071
	public TextAsset modifiersFile;

	// Token: 0x040017B8 RID: 6072
	public ModifierSet.ModifierInfos modifierInfos;

	// Token: 0x040017B9 RID: 6073
	public ModifierSet.TraitSet traits;

	// Token: 0x040017BA RID: 6074
	public ResourceSet<Effect> effects;

	// Token: 0x040017BB RID: 6075
	public ModifierSet.TraitGroupSet traitGroups;

	// Token: 0x040017BC RID: 6076
	public FertilityModifiers FertilityModifiers;

	// Token: 0x040017BD RID: 6077
	public Database.Attributes Attributes;

	// Token: 0x040017BE RID: 6078
	public BuildingAttributes BuildingAttributes;

	// Token: 0x040017BF RID: 6079
	public CritterAttributes CritterAttributes;

	// Token: 0x040017C0 RID: 6080
	public PlantAttributes PlantAttributes;

	// Token: 0x040017C1 RID: 6081
	public Database.Amounts Amounts;

	// Token: 0x040017C2 RID: 6082
	public Database.AttributeConverters AttributeConverters;

	// Token: 0x040017C3 RID: 6083
	public ResourceSet Root;

	// Token: 0x040017C4 RID: 6084
	public List<Resource> ResourceTable;

	// Token: 0x020007CF RID: 1999
	public class ModifierInfo : Resource
	{
		// Token: 0x040017C5 RID: 6085
		public string Type;

		// Token: 0x040017C6 RID: 6086
		public string Attribute;

		// Token: 0x040017C7 RID: 6087
		public float Value;

		// Token: 0x040017C8 RID: 6088
		public Units Units;

		// Token: 0x040017C9 RID: 6089
		public bool Multiplier;

		// Token: 0x040017CA RID: 6090
		public float Duration;

		// Token: 0x040017CB RID: 6091
		public bool ShowInUI;

		// Token: 0x040017CC RID: 6092
		public string StompGroup;

		// Token: 0x040017CD RID: 6093
		public int StompPriority;

		// Token: 0x040017CE RID: 6094
		public bool IsBad;

		// Token: 0x040017CF RID: 6095
		public string CustomIcon;

		// Token: 0x040017D0 RID: 6096
		public bool TriggerFloatingText;

		// Token: 0x040017D1 RID: 6097
		public string EmoteAnim;

		// Token: 0x040017D2 RID: 6098
		public float EmoteCooldown;
	}

	// Token: 0x020007D0 RID: 2000
	[Serializable]
	public class ModifierInfos : ResourceLoader<ModifierSet.ModifierInfo>
	{
	}

	// Token: 0x020007D1 RID: 2001
	[Serializable]
	public class TraitSet : ResourceSet<Trait>
	{
	}

	// Token: 0x020007D2 RID: 2002
	[Serializable]
	public class TraitGroupSet : ResourceSet<TraitGroup>
	{
	}
}
