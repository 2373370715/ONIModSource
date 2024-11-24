using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

public class ModifierSet : ScriptableObject
{
	public class ModifierInfo : Resource
	{
		public string Type;

		public string Attribute;

		public float Value;

		public Units Units;

		public bool Multiplier;

		public float Duration;

		public bool ShowInUI;

		public string StompGroup;

		public int StompPriority;

		public bool IsBad;

		public string CustomIcon;

		public bool TriggerFloatingText;

		public string EmoteAnim;

		public float EmoteCooldown;
	}

	[Serializable]
	public class ModifierInfos : ResourceLoader<ModifierInfo>
	{
	}

	[Serializable]
	public class TraitSet : ResourceSet<Trait>
	{
	}

	[Serializable]
	public class TraitGroupSet : ResourceSet<TraitGroup>
	{
	}

	public TextAsset modifiersFile;

	public ModifierInfos modifierInfos;

	public TraitSet traits;

	public ResourceSet<Effect> effects;

	public TraitGroupSet traitGroups;

	public FertilityModifiers FertilityModifiers;

	public Database.Attributes Attributes;

	public BuildingAttributes BuildingAttributes;

	public CritterAttributes CritterAttributes;

	public PlantAttributes PlantAttributes;

	public Database.Amounts Amounts;

	public Database.AttributeConverters AttributeConverters;

	public ResourceSet Root;

	public List<Resource> ResourceTable;

	public virtual void Initialize()
	{
		ResourceTable = new List<Resource>();
		Root = new ResourceSet<Resource>("Root", null);
		modifierInfos = new ModifierInfos();
		modifierInfos.Load(modifiersFile);
		Attributes = new Database.Attributes(Root);
		BuildingAttributes = new BuildingAttributes(Root);
		CritterAttributes = new CritterAttributes(Root);
		PlantAttributes = new PlantAttributes(Root);
		effects = new ResourceSet<Effect>("Effects", Root);
		traits = new TraitSet();
		traitGroups = new TraitGroupSet();
		FertilityModifiers = new FertilityModifiers();
		Amounts = new Database.Amounts();
		Amounts.Load();
		AttributeConverters = new Database.AttributeConverters();
		LoadEffects();
		LoadFertilityModifiers();
	}

	public static float ConvertValue(float value, Units units)
	{
		if (Units.PerDay == units)
		{
			return value * 0.0016666667f;
		}
		return value;
	}

	private void LoadEffects()
	{
		foreach (ModifierInfo modifierInfo in modifierInfos)
		{
			if (effects.Exists(modifierInfo.Id) || (!(modifierInfo.Type == "Effect") && !(modifierInfo.Type == "Base") && !(modifierInfo.Type == "Need")))
			{
				continue;
			}
			string description = Strings.Get($"STRINGS.DUPLICANTS.MODIFIERS.{modifierInfo.Id.ToUpper()}.NAME");
			string description2 = Strings.Get($"STRINGS.DUPLICANTS.MODIFIERS.{modifierInfo.Id.ToUpper()}.TOOLTIP");
			Effect effect = new Effect(modifierInfo.Id, description, description2, modifierInfo.Duration * 600f, modifierInfo.ShowInUI && modifierInfo.Type != "Need", modifierInfo.TriggerFloatingText, modifierInfo.IsBad, modifierInfo.EmoteAnim, modifierInfo.EmoteCooldown, modifierInfo.StompGroup, modifierInfo.CustomIcon);
			effect.stompPriority = modifierInfo.StompPriority;
			foreach (ModifierInfo modifierInfo2 in modifierInfos)
			{
				if (modifierInfo2.Id == modifierInfo.Id)
				{
					effect.Add(new AttributeModifier(modifierInfo2.Attribute, ConvertValue(modifierInfo2.Value, modifierInfo2.Units), description, modifierInfo2.Multiplier));
				}
			}
			effects.Add(effect);
		}
		Reactable.ReactablePrecondition precon = delegate(GameObject go, Navigator.ActiveTransition n)
		{
			int cell = Grid.PosToCell(go);
			return Grid.IsValidCell(cell) && Grid.IsGas(cell);
		};
		effects.Get("WetFeet").AddEmotePrecondition(precon);
		effects.Get("SoakingWet").AddEmotePrecondition(precon);
		Effect effect2 = new Effect("PassedOutSleep", DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.NAME, DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: true, is_bad: true, null, 0f, null, showStatusInWorld: true, "status_item_exhausted");
		effect2.Add(new AttributeModifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, 2f / 3f, DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.NAME));
		effect2.Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -1f / 30f, DUPLICANTS.MODIFIERS.PASSEDOUTSLEEP.NAME));
		effects.Add(effect2);
		Effect resource = new Effect("WarmTouch", DUPLICANTS.MODIFIERS.WARMTOUCH.NAME, DUPLICANTS.MODIFIERS.WARMTOUCH.TOOLTIP, 120f, new string[1] { "WetFeet" }, show_in_ui: true, trigger_floating_text: true, is_bad: false, null, 0f, null, showStatusInWorld: false);
		effects.Add(resource);
		Effect resource2 = new Effect("WarmTouchFood", DUPLICANTS.MODIFIERS.WARMTOUCHFOOD.NAME, DUPLICANTS.MODIFIERS.WARMTOUCHFOOD.TOOLTIP, 600f, new string[1] { "WetFeet" }, show_in_ui: true, trigger_floating_text: true, is_bad: false, null, 0f, null, showStatusInWorld: false);
		effects.Add(resource2);
		Effect resource3 = new Effect("RefreshingTouch", DUPLICANTS.MODIFIERS.REFRESHINGTOUCH.NAME, DUPLICANTS.MODIFIERS.REFRESHINGTOUCH.TOOLTIP, 120f, show_in_ui: true, trigger_floating_text: true, is_bad: false);
		effects.Add(resource3);
		CreateCritteEffects();
	}

	public void CreateCritteEffects()
	{
		Effect effect = new Effect("Ranched", STRINGS.CREATURES.MODIFIERS.RANCHED.NAME, STRINGS.CREATURES.MODIFIERS.RANCHED.TOOLTIP, 600f, show_in_ui: true, trigger_floating_text: true, is_bad: false);
		effect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, STRINGS.CREATURES.MODIFIERS.RANCHED.NAME));
		effect.Add(new AttributeModifier(Db.Get().Amounts.Wildness.deltaAttribute.Id, -11f / 120f, STRINGS.CREATURES.MODIFIERS.RANCHED.NAME));
		effects.Add(effect);
		Effect effect2 = new Effect("HadMilk", STRINGS.CREATURES.MODIFIERS.GOTMILK.NAME, STRINGS.CREATURES.MODIFIERS.GOTMILK.TOOLTIP, 600f, show_in_ui: true, trigger_floating_text: true, is_bad: false);
		effect2.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, STRINGS.CREATURES.MODIFIERS.GOTMILK.NAME));
		effects.Add(effect2);
		Effect effect3 = new Effect("EggSong", STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.NAME, STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.TOOLTIP, 600f, show_in_ui: true, trigger_floating_text: false, is_bad: false);
		effect3.Add(new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, 4f, STRINGS.CREATURES.MODIFIERS.INCUBATOR_SONG.NAME, is_multiplier: true));
		effects.Add(effect3);
		Effect effect4 = new Effect("EggHug", STRINGS.CREATURES.MODIFIERS.EGGHUG.NAME, STRINGS.CREATURES.MODIFIERS.EGGHUG.TOOLTIP, 600f, show_in_ui: true, trigger_floating_text: true, is_bad: false);
		effect4.Add(new AttributeModifier(Db.Get().Amounts.Incubation.deltaAttribute.Id, 1f, STRINGS.CREATURES.MODIFIERS.EGGHUG.NAME, is_multiplier: true));
		effects.Add(effect4);
		Effect resource = new Effect("HuggingFrenzy", STRINGS.CREATURES.MODIFIERS.HUGGINGFRENZY.NAME, STRINGS.CREATURES.MODIFIERS.HUGGINGFRENZY.TOOLTIP, 600f, show_in_ui: true, trigger_floating_text: false, is_bad: false);
		effects.Add(resource);
		Effect effect5 = new Effect("DivergentCropTended", STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.NAME, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.TOOLTIP, 600f, show_in_ui: true, trigger_floating_text: true, is_bad: false);
		effect5.Add(new AttributeModifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, 0.05f, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDED.NAME, is_multiplier: true));
		effects.Add(effect5);
		Effect effect6 = new Effect("DivergentCropTendedWorm", STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.NAME, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.TOOLTIP, 600f, show_in_ui: true, trigger_floating_text: true, is_bad: false);
		effect6.Add(new AttributeModifier(Db.Get().Amounts.Maturity.deltaAttribute.Id, 0.5f, STRINGS.CREATURES.MODIFIERS.DIVERGENTPLANTTENDEDWORM.NAME, is_multiplier: true));
		effects.Add(effect6);
		Effect effect7 = new Effect("MooWellFed", STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.MOOWELLFED.TOOLTIP, 1f, show_in_ui: true, trigger_floating_text: true, is_bad: false);
		effect7.Add(new AttributeModifier(Db.Get().Amounts.Beckoning.deltaAttribute.Id, MooTuning.WELLFED_EFFECT, STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME));
		effect7.Add(new AttributeModifier(Db.Get().Amounts.MilkProduction.deltaAttribute.Id, MooTuning.MILK_PRODUCTION_PERCENTAGE_PER_SECOND, STRINGS.CREATURES.MODIFIERS.MOOWELLFED.NAME));
		effects.Add(effect7);
		Effect effect8 = new Effect("WoodDeerWellFed", STRINGS.CREATURES.MODIFIERS.WOODDEERWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.WOODDEERWELLFED.TOOLTIP, 1f, show_in_ui: true, trigger_floating_text: true, is_bad: false);
		effect8.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, 100f / (WoodDeerConfig.ANTLER_GROWTH_TIME_IN_CYCLES * 600f), STRINGS.CREATURES.MODIFIERS.WOODDEERWELLFED.NAME));
		effects.Add(effect8);
		Effect effect9 = new Effect("IceBellyWellFed", STRINGS.CREATURES.MODIFIERS.ICEBELLYWELLFED.NAME, STRINGS.CREATURES.MODIFIERS.ICEBELLYWELLFED.TOOLTIP, 1f, show_in_ui: true, trigger_floating_text: true, is_bad: false);
		effect9.Add(new AttributeModifier(Db.Get().Amounts.ScaleGrowth.deltaAttribute.Id, 100f / (IceBellyConfig.SCALE_GROWTH_TIME_IN_CYCLES * 600f), STRINGS.CREATURES.MODIFIERS.ICEBELLYWELLFED.NAME));
		effects.Add(effect9);
	}

	public Trait CreateTrait(string id, string name, string description, string group_name, bool should_save, ChoreGroup[] disabled_chore_groups, bool positive_trait, bool is_valid_starter_trait)
	{
		Trait trait = new Trait(id, name, description, 0f, should_save, disabled_chore_groups, positive_trait, is_valid_starter_trait);
		traits.Add(trait);
		if (group_name == "" || group_name == null)
		{
			group_name = "Default";
		}
		TraitGroup traitGroup = traitGroups.TryGet(group_name);
		if (traitGroup == null)
		{
			traitGroup = new TraitGroup(group_name, group_name, group_name != "Default");
			traitGroups.Add(traitGroup);
		}
		traitGroup.Add(trait);
		return trait;
	}

	public FertilityModifier CreateFertilityModifier(string id, Tag targetTag, string name, string description, Func<string, string> tooltipCB, FertilityModifier.FertilityModFn applyFunction)
	{
		FertilityModifier fertilityModifier = new FertilityModifier(id, targetTag, name, description, tooltipCB, applyFunction);
		FertilityModifiers.Add(fertilityModifier);
		return fertilityModifier;
	}

	protected void LoadTraits()
	{
		TRAITS.TRAIT_CREATORS.ForEach(delegate(System.Action action)
		{
			action();
		});
	}

	protected void LoadFertilityModifiers()
	{
		TUNING.CREATURES.EGG_CHANCE_MODIFIERS.MODIFIER_CREATORS.ForEach(delegate(System.Action action)
		{
			action();
		});
	}
}
