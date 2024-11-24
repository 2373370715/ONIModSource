using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using TUNING;
using UnityEngine;

public class MinionStartingStats : ITelepadDeliverable
{
	public string Name;

	public string NameStringKey;

	public string GenderStringKey;

	public List<Trait> Traits = new List<Trait>();

	public int rarityBalance;

	public Trait stressTrait;

	public Trait joyTrait;

	public Trait congenitaltrait;

	public string stickerType;

	public int voiceIdx;

	public Dictionary<string, int> StartingLevels = new Dictionary<string, int>();

	public Personality personality;

	public List<Accessory> accessories = new List<Accessory>();

	public bool IsValid;

	public Dictionary<SkillGroup, float> skillAptitudes = new Dictionary<SkillGroup, float>();

	public MinionStartingStats(Personality personality, string guaranteedAptitudeID = null, string guaranteedTraitID = null, bool isDebugMinion = false)
	{
		this.personality = personality;
		GenerateStats(guaranteedAptitudeID, guaranteedTraitID, isDebugMinion);
	}

	public MinionStartingStats(bool is_starter_minion, string guaranteedAptitudeID = null, string guaranteedTraitID = null, bool isDebugMinion = false)
	{
		personality = Db.Get().Personalities.GetRandom(onlyEnabledMinions: true, is_starter_minion);
		GenerateStats(guaranteedAptitudeID, guaranteedTraitID, isDebugMinion, is_starter_minion);
	}

	private void GenerateStats(string guaranteedAptitudeID = null, string guaranteedTraitID = null, bool isDebugMinion = false, bool is_starter_minion = false)
	{
		voiceIdx = UnityEngine.Random.Range(0, 4);
		Name = personality.Name;
		NameStringKey = personality.nameStringKey;
		GenderStringKey = personality.genderStringKey;
		Traits.Add(Db.Get().traits.Get(MinionConfig.MINION_BASE_TRAIT_ID));
		List<ChoreGroup> disabled_chore_groups = new List<ChoreGroup>();
		GenerateAptitudes(guaranteedAptitudeID);
		int pointsDelta = GenerateTraits(is_starter_minion, disabled_chore_groups, guaranteedAptitudeID, guaranteedTraitID, isDebugMinion);
		GenerateAttributes(pointsDelta, disabled_chore_groups);
		KCompBuilder.BodyData bodyData = CreateBodyData(personality);
		foreach (AccessorySlot resource in Db.Get().AccessorySlots.resources)
		{
			if (resource.accessories.Count == 0)
			{
				continue;
			}
			Accessory accessory = null;
			if (resource == Db.Get().AccessorySlots.HeadShape)
			{
				accessory = resource.Lookup(bodyData.headShape);
				if (accessory == null)
				{
					personality.headShape = 0;
				}
			}
			else if (resource == Db.Get().AccessorySlots.Mouth)
			{
				accessory = resource.Lookup(bodyData.mouth);
				if (accessory == null)
				{
					personality.mouth = 0;
				}
			}
			else if (resource == Db.Get().AccessorySlots.Eyes)
			{
				accessory = resource.Lookup(bodyData.eyes);
				if (accessory == null)
				{
					personality.eyes = 0;
				}
			}
			else if (resource == Db.Get().AccessorySlots.Hair)
			{
				accessory = resource.Lookup(bodyData.hair);
				if (accessory == null)
				{
					personality.hair = 0;
				}
			}
			else if (resource == Db.Get().AccessorySlots.HatHair)
			{
				accessory = resource.accessories[0];
			}
			else if (resource == Db.Get().AccessorySlots.Body)
			{
				accessory = resource.Lookup(bodyData.body);
				if (accessory == null)
				{
					personality.body = 0;
				}
			}
			else if (resource == Db.Get().AccessorySlots.Arm)
			{
				accessory = resource.Lookup(bodyData.arms);
			}
			else if (resource == Db.Get().AccessorySlots.ArmLower)
			{
				accessory = resource.Lookup(bodyData.armslower);
			}
			else if (resource == Db.Get().AccessorySlots.ArmLowerSkin)
			{
				accessory = resource.Lookup(bodyData.armLowerSkin);
			}
			else if (resource == Db.Get().AccessorySlots.ArmUpperSkin)
			{
				accessory = resource.Lookup(bodyData.armUpperSkin);
			}
			else if (resource == Db.Get().AccessorySlots.LegSkin)
			{
				accessory = resource.Lookup(bodyData.legSkin);
			}
			else if (resource == Db.Get().AccessorySlots.Leg)
			{
				accessory = resource.Lookup(bodyData.legs);
			}
			else if (resource == Db.Get().AccessorySlots.Belt)
			{
				accessory = resource.Lookup(bodyData.belt);
				if (accessory == null)
				{
					accessory = resource.accessories[0];
				}
			}
			else if (resource == Db.Get().AccessorySlots.Neck)
			{
				accessory = resource.Lookup(bodyData.neck);
			}
			else if (resource == Db.Get().AccessorySlots.Pelvis)
			{
				accessory = resource.Lookup(bodyData.pelvis);
			}
			else if (resource == Db.Get().AccessorySlots.Foot)
			{
				accessory = resource.Lookup(bodyData.foot);
				if (accessory == null)
				{
					accessory = resource.accessories[0];
				}
			}
			else if (resource == Db.Get().AccessorySlots.Skirt)
			{
				accessory = resource.Lookup(bodyData.skirt);
			}
			else if (resource == Db.Get().AccessorySlots.Necklace)
			{
				accessory = resource.Lookup(bodyData.necklace);
			}
			else if (resource == Db.Get().AccessorySlots.Cuff)
			{
				accessory = resource.Lookup(bodyData.cuff);
				if (accessory == null)
				{
					accessory = resource.accessories[0];
				}
			}
			else if (resource == Db.Get().AccessorySlots.Hand)
			{
				accessory = resource.Lookup(bodyData.hand);
				if (accessory == null)
				{
					accessory = resource.accessories[0];
				}
			}
			accessories.Add(accessory);
		}
	}

	private int GenerateTraits(bool is_starter_minion, List<ChoreGroup> disabled_chore_groups, string guaranteedAptitudeID = null, string guaranteedTraitID = null, bool isDebugMinion = false)
	{
		int statDelta = 0;
		List<string> selectedTraits = new List<string>();
		KRandom randSeed = new KRandom();
		Trait trait2 = Db.Get().traits.Get(personality.stresstrait);
		stressTrait = trait2;
		Trait trait3 = Db.Get().traits.Get(personality.joyTrait);
		joyTrait = trait3;
		stickerType = personality.stickerType;
		Trait trait4 = Db.Get().traits.TryGet(personality.congenitaltrait);
		if (trait4 == null || trait4.Name == "None")
		{
			congenitaltrait = null;
		}
		else
		{
			congenitaltrait = trait4;
		}
		Func<List<DUPLICANTSTATS.TraitVal>, bool, bool> func = delegate(List<DUPLICANTSTATS.TraitVal> traitPossibilities, bool positiveTrait)
		{
			if (Traits.Count > DUPLICANTSTATS.MAX_TRAITS)
			{
				return false;
			}
			Mathf.Abs(Util.GaussianRandom());
			int num6 = traitPossibilities.Count;
			int num7;
			if (!positiveTrait)
			{
				if (DUPLICANTSTATS.rarityDeckActive.Count < 1)
				{
					DUPLICANTSTATS.rarityDeckActive.AddRange(DUPLICANTSTATS.RARITY_DECK);
				}
				if (DUPLICANTSTATS.rarityDeckActive.Count == DUPLICANTSTATS.RARITY_DECK.Count)
				{
					DUPLICANTSTATS.rarityDeckActive.ShuffleSeeded(randSeed);
				}
				num7 = DUPLICANTSTATS.rarityDeckActive[DUPLICANTSTATS.rarityDeckActive.Count - 1];
				DUPLICANTSTATS.rarityDeckActive.RemoveAt(DUPLICANTSTATS.rarityDeckActive.Count - 1);
			}
			else
			{
				List<int> list = new List<int>();
				if (is_starter_minion)
				{
					list.Add(rarityBalance - 1);
					list.Add(rarityBalance);
					list.Add(rarityBalance);
					list.Add(rarityBalance + 1);
				}
				else
				{
					list.Add(rarityBalance - 2);
					list.Add(rarityBalance - 1);
					list.Add(rarityBalance);
					list.Add(rarityBalance + 1);
					list.Add(rarityBalance + 2);
				}
				list.ShuffleSeeded(randSeed);
				num7 = list[0];
				num7 = Mathf.Max(DUPLICANTSTATS.RARITY_COMMON, num7);
				num7 = Mathf.Min(DUPLICANTSTATS.RARITY_LEGENDARY, num7);
			}
			List<DUPLICANTSTATS.TraitVal> list2 = new List<DUPLICANTSTATS.TraitVal>(traitPossibilities);
			for (int num8 = list2.Count - 1; num8 > -1; num8--)
			{
				if (list2[num8].rarity != num7)
				{
					list2.RemoveAt(num8);
					num6--;
				}
			}
			list2.ShuffleSeeded(randSeed);
			foreach (DUPLICANTSTATS.TraitVal item in list2)
			{
				Debug.Assert(SaveLoader.Instance != null, "IsDLCActiveForCurrentSave should not be called from the front end");
				if (!SaveLoader.Instance.IsDLCActiveForCurrentSave(item.dlcId))
				{
					num6--;
				}
				else if (selectedTraits.Contains(item.id))
				{
					num6--;
				}
				else
				{
					Trait trait6 = Db.Get().traits.TryGet(item.id);
					if (trait6 == null)
					{
						Debug.LogWarning("Trying to add nonexistent trait: " + item.id);
						num6--;
					}
					else if (!isDebugMinion || trait6.disabledChoreGroups == null || trait6.disabledChoreGroups.Length == 0)
					{
						if (is_starter_minion && !trait6.ValidStarterTrait)
						{
							num6--;
						}
						else if (item.doNotGenerateTrait)
						{
							num6--;
						}
						else if (AreTraitAndAptitudesExclusive(item, skillAptitudes))
						{
							num6--;
						}
						else if (is_starter_minion && guaranteedAptitudeID != null && AreTraitAndArchetypeExclusive(item, guaranteedAptitudeID))
						{
							num6--;
						}
						else
						{
							if (!AreTraitsMutuallyExclusive(item, selectedTraits))
							{
								SelectTrait(item, trait6, positiveTrait);
								return true;
							}
							num6--;
						}
					}
				}
			}
			return false;
		};
		int num = 0;
		int num2 = 0;
		if (is_starter_minion)
		{
			num = 1;
			num2 = 1;
		}
		else
		{
			if (DUPLICANTSTATS.podTraitConfigurationsActive.Count < 1)
			{
				DUPLICANTSTATS.podTraitConfigurationsActive.AddRange(DUPLICANTSTATS.POD_TRAIT_CONFIGURATIONS_DECK);
			}
			if (DUPLICANTSTATS.podTraitConfigurationsActive.Count == DUPLICANTSTATS.POD_TRAIT_CONFIGURATIONS_DECK.Count)
			{
				DUPLICANTSTATS.podTraitConfigurationsActive.ShuffleSeeded(randSeed);
			}
			num = DUPLICANTSTATS.podTraitConfigurationsActive[DUPLICANTSTATS.podTraitConfigurationsActive.Count - 1].first;
			num2 = DUPLICANTSTATS.podTraitConfigurationsActive[DUPLICANTSTATS.podTraitConfigurationsActive.Count - 1].second;
			DUPLICANTSTATS.podTraitConfigurationsActive.RemoveAt(DUPLICANTSTATS.podTraitConfigurationsActive.Count - 1);
		}
		bool flag = false;
		int num3 = 0;
		int num4 = 0;
		int num5 = (num2 + num) * 4;
		if (!string.IsNullOrEmpty(guaranteedTraitID))
		{
			DUPLICANTSTATS.TraitVal traitVal2 = DUPLICANTSTATS.GetTraitVal(guaranteedTraitID);
			if (traitVal2.id == guaranteedTraitID)
			{
				Trait trait5 = Db.Get().traits.TryGet(traitVal2.id);
				bool positiveTrait2 = trait5.PositiveTrait;
				selectedTraits.Add(traitVal2.id);
				statDelta += traitVal2.statBonus;
				rarityBalance += (positiveTrait2 ? (-traitVal2.rarity) : traitVal2.rarity);
				Traits.Add(trait5);
				if (trait5.disabledChoreGroups != null)
				{
					for (int i = 0; i < trait5.disabledChoreGroups.Length; i++)
					{
						disabled_chore_groups.Add(trait5.disabledChoreGroups[i]);
					}
				}
				if (positiveTrait2)
				{
					num3++;
				}
				else
				{
					num4++;
				}
			}
		}
		if (!flag)
		{
			if (congenitaltrait != null)
			{
				DUPLICANTSTATS.TraitVal traitVal3;
				if (congenitaltrait.PositiveTrait)
				{
					num3++;
					traitVal3 = DUPLICANTSTATS.GOODTRAITS.Find((DUPLICANTSTATS.TraitVal match) => match.id == congenitaltrait.Id);
				}
				else
				{
					num4++;
					traitVal3 = DUPLICANTSTATS.BADTRAITS.Find((DUPLICANTSTATS.TraitVal match) => match.id == congenitaltrait.Id);
				}
				SelectTrait(traitVal3, congenitaltrait, congenitaltrait.PositiveTrait);
			}
			flag = true;
		}
		while (num5 > 0 && (num4 < num2 || num3 < num))
		{
			if (num4 < num2 && func(DUPLICANTSTATS.BADTRAITS, arg2: false))
			{
				num4++;
			}
			if (num3 < num && func(DUPLICANTSTATS.GOODTRAITS, arg2: true))
			{
				num3++;
			}
			num5--;
		}
		if (num5 > 0)
		{
			IsValid = true;
		}
		return statDelta;
		void SelectTrait(DUPLICANTSTATS.TraitVal traitVal, Trait trait, bool isPositiveTrait)
		{
			selectedTraits.Add(traitVal.id);
			statDelta += traitVal.statBonus;
			rarityBalance += (isPositiveTrait ? (-traitVal.rarity) : traitVal.rarity);
			Traits.Add(trait);
			if (trait.disabledChoreGroups != null)
			{
				for (int j = 0; j < trait.disabledChoreGroups.Length; j++)
				{
					disabled_chore_groups.Add(trait.disabledChoreGroups[j]);
				}
			}
		}
	}

	private void GenerateAptitudes(string guaranteedAptitudeID = null)
	{
		int num = UnityEngine.Random.Range(1, 4);
		List<SkillGroup> list = new List<SkillGroup>(Db.Get().SkillGroups.resources);
		list.Shuffle();
		if (guaranteedAptitudeID != null)
		{
			skillAptitudes.Add(Db.Get().SkillGroups.Get(guaranteedAptitudeID), DUPLICANTSTATS.APTITUDE_BONUS);
			list.Remove(Db.Get().SkillGroups.Get(guaranteedAptitudeID));
			num--;
		}
		for (int i = 0; i < num; i++)
		{
			skillAptitudes.Add(list[i], DUPLICANTSTATS.APTITUDE_BONUS);
		}
	}

	private void GenerateAttributes(int pointsDelta, List<ChoreGroup> disabled_chore_groups)
	{
		List<string> list = new List<string>(DUPLICANTSTATS.ALL_ATTRIBUTES);
		for (int i = 0; i < list.Count; i++)
		{
			if (!StartingLevels.ContainsKey(list[i]))
			{
				StartingLevels[list[i]] = 0;
			}
		}
		foreach (KeyValuePair<SkillGroup, float> skillAptitude in skillAptitudes)
		{
			if (skillAptitude.Key.relevantAttributes.Count <= 0)
			{
				continue;
			}
			for (int j = 0; j < skillAptitude.Key.relevantAttributes.Count; j++)
			{
				if (!StartingLevels.ContainsKey(skillAptitude.Key.relevantAttributes[j].Id))
				{
					Debug.LogError("Need to add " + skillAptitude.Key.relevantAttributes[j].Id + " to TUNING.DUPLICANTSTATS.ALL_ATTRIBUTES");
				}
				StartingLevels[skillAptitude.Key.relevantAttributes[j].Id] += DUPLICANTSTATS.APTITUDE_ATTRIBUTE_BONUSES[skillAptitudes.Count - 1];
			}
		}
		List<SkillGroup> list2 = new List<SkillGroup>(skillAptitudes.Keys);
		if (pointsDelta > 0)
		{
			for (int num = pointsDelta; num > 0; num--)
			{
				list2.Shuffle();
				for (int k = 0; k < list2[0].relevantAttributes.Count; k++)
				{
					StartingLevels[list2[0].relevantAttributes[k].Id]++;
				}
			}
		}
		if (disabled_chore_groups.Count <= 0)
		{
			return;
		}
		int num2 = 0;
		int num3 = 0;
		foreach (KeyValuePair<string, int> startingLevel in StartingLevels)
		{
			if (startingLevel.Value > num2)
			{
				num2 = startingLevel.Value;
			}
			if (startingLevel.Key == disabled_chore_groups[0].attribute.Id)
			{
				num3 = startingLevel.Value;
			}
		}
		if (num2 != num3)
		{
			return;
		}
		foreach (string item in list)
		{
			if (item != disabled_chore_groups[0].attribute.Id)
			{
				int value = 0;
				StartingLevels.TryGetValue(item, out value);
				int num4 = 0;
				if (value > 0)
				{
					num4 = 1;
				}
				StartingLevels[disabled_chore_groups[0].attribute.Id] = value - num4;
				StartingLevels[item] = num2 + num4;
				break;
			}
		}
	}

	public void Apply(GameObject go)
	{
		MinionIdentity component = go.GetComponent<MinionIdentity>();
		component.SetName(Name);
		component.nameStringKey = NameStringKey;
		component.genderStringKey = GenderStringKey;
		component.personalityResourceId = personality.IdHash;
		ApplyTraits(go);
		ApplyRace(go);
		ApplyAptitudes(go);
		ApplyAccessories(go);
		ApplyExperience(go);
		ApplyOutfit(personality, go);
		ApplyJoyResponseOutfit(personality, go);
	}

	public void ApplyExperience(GameObject go)
	{
		foreach (KeyValuePair<string, int> startingLevel in StartingLevels)
		{
			go.GetComponent<AttributeLevels>().SetLevel(startingLevel.Key, startingLevel.Value);
		}
	}

	public void ApplyAccessories(GameObject go)
	{
		Accessorizer component = go.GetComponent<Accessorizer>();
		component.ApplyMinionPersonality(personality);
		component.UpdateHairBasedOnHat();
	}

	public void ApplyOutfit(Personality personality, GameObject go)
	{
		WearableAccessorizer component = go.GetComponent<WearableAccessorizer>();
		Option<ClothingOutfitTarget> option = ClothingOutfitTarget.TryFromTemplateId(personality.GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType.Clothing));
		if (option.IsSome())
		{
			component.ApplyClothingItems(ClothingOutfitUtility.OutfitType.Clothing, option.Unwrap().ReadItemValues());
		}
	}

	public void ApplyJoyResponseOutfit(Personality personality, GameObject go)
	{
		JoyResponseOutfitTarget joyResponseOutfitTarget = JoyResponseOutfitTarget.FromPersonality(personality);
		JoyResponseOutfitTarget.FromMinion(go).WriteFacadeId(joyResponseOutfitTarget.ReadFacadeId());
	}

	public void ApplyRace(GameObject go)
	{
		go.GetComponent<MinionIdentity>().voiceIdx = voiceIdx;
	}

	public static KCompBuilder.BodyData CreateBodyData(Personality p)
	{
		KCompBuilder.BodyData result = default(KCompBuilder.BodyData);
		result.eyes = HashCache.Get().Add($"eyes_{p.eyes:000}");
		result.hair = HashCache.Get().Add($"hair_{p.hair:000}");
		result.headShape = HashCache.Get().Add($"headshape_{p.headShape:000}");
		result.mouth = HashCache.Get().Add($"mouth_{p.mouth:000}");
		result.neck = HashCache.Get().Add("neck");
		result.arms = HashCache.Get().Add($"arm_sleeve_{p.body:000}");
		result.armslower = HashCache.Get().Add($"arm_lower_sleeve_{p.body:000}");
		result.body = HashCache.Get().Add($"torso_{p.body:000}");
		result.hat = HashedString.Invalid;
		result.faceFX = HashedString.Invalid;
		result.armLowerSkin = HashCache.Get().Add($"arm_lower_{p.headShape:000}");
		result.armUpperSkin = HashCache.Get().Add($"arm_upper_{p.headShape:000}");
		result.legSkin = HashCache.Get().Add($"leg_skin_{p.headShape:000}");
		result.neck = HashCache.Get().Add((p.neck != 0) ? $"neck_{p.neck:000}" : "neck");
		result.legs = HashCache.Get().Add((p.leg != 0) ? $"leg_{p.leg:000}" : "leg");
		result.belt = HashCache.Get().Add((p.belt != 0) ? $"belt_{p.belt:000}" : "belt");
		result.pelvis = HashCache.Get().Add((p.pelvis != 0) ? $"pelvis_{p.pelvis:000}" : "pelvis");
		result.foot = HashCache.Get().Add((p.foot != 0) ? $"foot_{p.foot:000}" : "foot");
		result.hand = HashCache.Get().Add((p.hand != 0) ? $"hand_paint_{p.hand:000}" : "hand_paint");
		result.cuff = HashCache.Get().Add((p.cuff != 0) ? $"cuff_{p.cuff:000}" : "cuff");
		return result;
	}

	public void ApplyAptitudes(GameObject go)
	{
		MinionResume component = go.GetComponent<MinionResume>();
		foreach (KeyValuePair<SkillGroup, float> skillAptitude in skillAptitudes)
		{
			component.SetAptitude(skillAptitude.Key.Id, skillAptitude.Value);
		}
	}

	public void ApplyTraits(GameObject go)
	{
		Traits component = go.GetComponent<Traits>();
		component.Clear();
		foreach (Trait trait in Traits)
		{
			component.Add(trait);
		}
		component.Add(stressTrait);
		component.Add(joyTrait);
		go.GetComponent<MinionIdentity>().SetStickerType(stickerType);
		MinionIdentity component2 = go.GetComponent<MinionIdentity>();
		component2.SetName(Name);
		component2.nameStringKey = NameStringKey;
		go.GetComponent<MinionIdentity>().SetGender(GenderStringKey);
	}

	public GameObject Deliver(Vector3 location)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(MinionConfig.ID));
		gameObject.SetActive(value: true);
		gameObject.transform.SetLocalPosition(location);
		Apply(gameObject);
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		new EmoteChore(gameObject.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_interacts_portal_kanim", Telepad.PortalBirthAnim);
		return gameObject;
	}

	private bool AreTraitAndAptitudesExclusive(DUPLICANTSTATS.TraitVal traitVal, Dictionary<SkillGroup, float> aptitudes)
	{
		if (traitVal.mutuallyExclusiveAptitudes == null)
		{
			return false;
		}
		foreach (KeyValuePair<SkillGroup, float> skillAptitude in skillAptitudes)
		{
			foreach (HashedString mutuallyExclusiveAptitude in traitVal.mutuallyExclusiveAptitudes)
			{
				if (mutuallyExclusiveAptitude == skillAptitude.Key.IdHash && skillAptitude.Value > 0f)
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool AreTraitAndArchetypeExclusive(DUPLICANTSTATS.TraitVal traitVal, string guaranteedAptitudeID)
	{
		if (!DUPLICANTSTATS.ARCHETYPE_TRAIT_EXCLUSIONS.ContainsKey(guaranteedAptitudeID))
		{
			Debug.LogError("Need to add attribute " + guaranteedAptitudeID + " to ARCHETYPE_TRAIT_EXCLUSIONS");
		}
		foreach (string item in DUPLICANTSTATS.ARCHETYPE_TRAIT_EXCLUSIONS[guaranteedAptitudeID])
		{
			if (item == traitVal.id)
			{
				return true;
			}
		}
		return false;
	}

	private bool AreTraitsMutuallyExclusive(DUPLICANTSTATS.TraitVal traitVal, List<string> selectedTraits)
	{
		foreach (string selectedTrait in selectedTraits)
		{
			foreach (DUPLICANTSTATS.TraitVal gOODTRAIT in DUPLICANTSTATS.GOODTRAITS)
			{
				if (selectedTrait == gOODTRAIT.id && gOODTRAIT.mutuallyExclusiveTraits != null && gOODTRAIT.mutuallyExclusiveTraits.Contains(traitVal.id))
				{
					return true;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal bADTRAIT in DUPLICANTSTATS.BADTRAITS)
			{
				if (selectedTrait == bADTRAIT.id && bADTRAIT.mutuallyExclusiveTraits != null && bADTRAIT.mutuallyExclusiveTraits.Contains(traitVal.id))
				{
					return true;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal cONGENITALTRAIT in DUPLICANTSTATS.CONGENITALTRAITS)
			{
				if (selectedTrait == cONGENITALTRAIT.id && cONGENITALTRAIT.mutuallyExclusiveTraits != null && cONGENITALTRAIT.mutuallyExclusiveTraits.Contains(traitVal.id))
				{
					return true;
				}
			}
			foreach (DUPLICANTSTATS.TraitVal sPECIALTRAIT in DUPLICANTSTATS.SPECIALTRAITS)
			{
				if (selectedTrait == sPECIALTRAIT.id && sPECIALTRAIT.mutuallyExclusiveTraits != null && sPECIALTRAIT.mutuallyExclusiveTraits.Contains(traitVal.id))
				{
					return true;
				}
			}
			if (traitVal.mutuallyExclusiveTraits != null && traitVal.mutuallyExclusiveTraits.Contains(selectedTrait))
			{
				return true;
			}
		}
		return false;
	}
}
