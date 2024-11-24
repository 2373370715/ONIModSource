using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

public class Personality : Resource
{
	public class StartingAttribute
	{
		public Klei.AI.Attribute attribute;

		public int value;

		public StartingAttribute(Klei.AI.Attribute attribute, int value)
		{
			this.attribute = attribute;
			this.value = value;
		}
	}

	public List<StartingAttribute> attributes = new List<StartingAttribute>();

	public List<Trait> traits = new List<Trait>();

	public int headShape;

	public int mouth;

	public int neck;

	public int eyes;

	public int hair;

	public int body;

	public int belt;

	public int cuff;

	public int foot;

	public int hand;

	public int pelvis;

	public int leg;

	public string nameStringKey;

	public string genderStringKey;

	public string personalityType;

	public string stresstrait;

	public string joyTrait;

	public string stickerType;

	public string congenitaltrait;

	public string unformattedDescription;

	public string graveStone;

	public bool startingMinion;

	public string requiredDlcId;

	public string description => GetDescription();

	[Obsolete("Modders: Use constructor with isStartingMinion parameter")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, string description)
		: this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, description, isStartingMinion: true, "")
	{
	}

	[Obsolete("Modders: Added additional body part customization to duplicant personalities")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, string description, bool isStartingMinion)
		: this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, description, isStartingMinion: true, "")
	{
	}

	[Obsolete("Modders: Added a custom gravestone image to duplicant personalities")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, int belt, int cuff, int foot, int hand, int pelvis, int leg, string description, bool isStartingMinion)
		: this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, description, isStartingMinion, "")
	{
	}

	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, int belt, int cuff, int foot, int hand, int pelvis, int leg, string description, bool isStartingMinion, string graveStone)
		: base(name_string_key, name)
	{
		nameStringKey = name_string_key;
		genderStringKey = Gender;
		personalityType = PersonalityType;
		stresstrait = StressTrait;
		joyTrait = JoyTrait;
		stickerType = StickerType;
		congenitaltrait = CongenitalTrait;
		unformattedDescription = description;
		this.headShape = headShape;
		this.mouth = mouth;
		this.neck = neck;
		this.eyes = eyes;
		this.hair = hair;
		this.body = body;
		this.belt = belt;
		this.cuff = cuff;
		this.foot = foot;
		this.hand = hand;
		this.pelvis = pelvis;
		this.leg = leg;
		startingMinion = isStartingMinion;
		this.graveStone = graveStone;
	}

	public string GetDescription()
	{
		unformattedDescription = unformattedDescription.Replace("{0}", Name);
		return unformattedDescription;
	}

	public void SetAttribute(Klei.AI.Attribute attribute, int value)
	{
		StartingAttribute item = new StartingAttribute(attribute, value);
		attributes.Add(item);
	}

	public void AddTrait(Trait trait)
	{
		traits.Add(trait);
	}

	public void SetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType outfitType, Option<string> outfit)
	{
		CustomClothingOutfits.Instance.Internal_SetDuplicantPersonalityOutfit(outfitType, Id, outfit);
	}

	public string GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType outfitType)
	{
		if (CustomClothingOutfits.Instance.Internal_TryGetDuplicantPersonalityOutfit(outfitType, Id, out var outfitId))
		{
			return outfitId;
		}
		return null;
	}

	public Sprite GetMiniIcon()
	{
		if (string.IsNullOrWhiteSpace(nameStringKey))
		{
			return Assets.GetSprite("unknown");
		}
		string text = ((!(nameStringKey == "MIMA")) ? (nameStringKey[0] + nameStringKey.Substring(1).ToLower()) : "Mi-Ma");
		return Assets.GetSprite("dreamIcon_" + text);
	}
}
