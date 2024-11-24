using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x0200162D RID: 5677
public class Personality : Resource
{
	// Token: 0x1700076C RID: 1900
	// (get) Token: 0x0600757A RID: 30074 RVA: 0x000ED256 File Offset: 0x000EB456
	public string description
	{
		get
		{
			return this.GetDescription();
		}
	}

	// Token: 0x0600757B RID: 30075 RVA: 0x00306678 File Offset: 0x00304878
	[Obsolete("Modders: Use constructor with isStartingMinion parameter")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, string description) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, description, true, "", GameTags.Minions.Models.Standard)
	{
	}

	// Token: 0x0600757C RID: 30076 RVA: 0x00306678 File Offset: 0x00304878
	[Obsolete("Modders: Added additional body part customization to duplicant personalities")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, string description, bool isStartingMinion) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, description, true, "", GameTags.Minions.Models.Standard)
	{
	}

	// Token: 0x0600757D RID: 30077 RVA: 0x003066B8 File Offset: 0x003048B8
	[Obsolete("Modders: Added a custom gravestone image to duplicant personalities")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, int belt, int cuff, int foot, int hand, int pelvis, int leg, string description, bool isStartingMinion) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, description, isStartingMinion, "", GameTags.Minions.Models.Standard)
	{
	}

	// Token: 0x0600757E RID: 30078 RVA: 0x003066B8 File Offset: 0x003048B8
	[Obsolete("Modders: Added 'model' to duplicant personalities")]
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, int belt, int cuff, int foot, int hand, int pelvis, int leg, string description, bool isStartingMinion, string graveStone) : this(name_string_key, name, Gender, PersonalityType, StressTrait, JoyTrait, StickerType, CongenitalTrait, headShape, mouth, neck, eyes, hair, body, 0, 0, 0, 0, 0, 0, description, isStartingMinion, "", GameTags.Minions.Models.Standard)
	{
	}

	// Token: 0x0600757F RID: 30079 RVA: 0x003066F8 File Offset: 0x003048F8
	public Personality(string name_string_key, string name, string Gender, string PersonalityType, string StressTrait, string JoyTrait, string StickerType, string CongenitalTrait, int headShape, int mouth, int neck, int eyes, int hair, int body, int belt, int cuff, int foot, int hand, int pelvis, int leg, string description, bool isStartingMinion, string graveStone, Tag model) : base(name_string_key, name)
	{
		this.nameStringKey = name_string_key;
		this.genderStringKey = Gender;
		this.personalityType = PersonalityType;
		this.stresstrait = StressTrait;
		this.joyTrait = JoyTrait;
		this.stickerType = StickerType;
		this.congenitaltrait = CongenitalTrait;
		this.unformattedDescription = description;
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
		this.startingMinion = isStartingMinion;
		this.graveStone = graveStone;
		this.model = model;
	}

	// Token: 0x06007580 RID: 30080 RVA: 0x000ED25E File Offset: 0x000EB45E
	public string GetDescription()
	{
		this.unformattedDescription = this.unformattedDescription.Replace("{0}", this.Name);
		return this.unformattedDescription;
	}

	// Token: 0x06007581 RID: 30081 RVA: 0x003067DC File Offset: 0x003049DC
	public void SetAttribute(Klei.AI.Attribute attribute, int value)
	{
		Personality.StartingAttribute item = new Personality.StartingAttribute(attribute, value);
		this.attributes.Add(item);
	}

	// Token: 0x06007582 RID: 30082 RVA: 0x000ED282 File Offset: 0x000EB482
	public void AddTrait(Trait trait)
	{
		this.traits.Add(trait);
	}

	// Token: 0x06007583 RID: 30083 RVA: 0x000ED290 File Offset: 0x000EB490
	public void SetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType outfitType, Option<string> outfit)
	{
		CustomClothingOutfits.Instance.Internal_SetDuplicantPersonalityOutfit(outfitType, this.Id, outfit);
	}

	// Token: 0x06007584 RID: 30084 RVA: 0x00306800 File Offset: 0x00304A00
	public string GetSelectedTemplateOutfitId(ClothingOutfitUtility.OutfitType outfitType)
	{
		string result;
		if (CustomClothingOutfits.Instance.Internal_TryGetDuplicantPersonalityOutfit(outfitType, this.Id, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06007585 RID: 30085 RVA: 0x00306828 File Offset: 0x00304A28
	public Sprite GetMiniIcon()
	{
		if (string.IsNullOrWhiteSpace(this.nameStringKey))
		{
			return Assets.GetSprite("unknown");
		}
		string str;
		if (this.nameStringKey == "MIMA")
		{
			str = "Mi-Ma";
		}
		else
		{
			str = this.nameStringKey[0].ToString() + this.nameStringKey.Substring(1).ToLower();
		}
		return Assets.GetSprite("dreamIcon_" + str);
	}

	// Token: 0x040057F6 RID: 22518
	public List<Personality.StartingAttribute> attributes = new List<Personality.StartingAttribute>();

	// Token: 0x040057F7 RID: 22519
	public List<Trait> traits = new List<Trait>();

	// Token: 0x040057F8 RID: 22520
	public int headShape;

	// Token: 0x040057F9 RID: 22521
	public int mouth;

	// Token: 0x040057FA RID: 22522
	public int neck;

	// Token: 0x040057FB RID: 22523
	public int eyes;

	// Token: 0x040057FC RID: 22524
	public int hair;

	// Token: 0x040057FD RID: 22525
	public int body;

	// Token: 0x040057FE RID: 22526
	public int belt;

	// Token: 0x040057FF RID: 22527
	public int cuff;

	// Token: 0x04005800 RID: 22528
	public int foot;

	// Token: 0x04005801 RID: 22529
	public int hand;

	// Token: 0x04005802 RID: 22530
	public int pelvis;

	// Token: 0x04005803 RID: 22531
	public int leg;

	// Token: 0x04005804 RID: 22532
	public string nameStringKey;

	// Token: 0x04005805 RID: 22533
	public string genderStringKey;

	// Token: 0x04005806 RID: 22534
	public string personalityType;

	// Token: 0x04005807 RID: 22535
	public Tag model;

	// Token: 0x04005808 RID: 22536
	public string stresstrait;

	// Token: 0x04005809 RID: 22537
	public string joyTrait;

	// Token: 0x0400580A RID: 22538
	public string stickerType;

	// Token: 0x0400580B RID: 22539
	public string congenitaltrait;

	// Token: 0x0400580C RID: 22540
	public string unformattedDescription;

	// Token: 0x0400580D RID: 22541
	public string graveStone;

	// Token: 0x0400580E RID: 22542
	public bool startingMinion;

	// Token: 0x0400580F RID: 22543
	public string requiredDlcId;

	// Token: 0x0200162E RID: 5678
	public class StartingAttribute
	{
		// Token: 0x06007586 RID: 30086 RVA: 0x000ED2A4 File Offset: 0x000EB4A4
		public StartingAttribute(Klei.AI.Attribute attribute, int value)
		{
			this.attribute = attribute;
			this.value = value;
		}

		// Token: 0x04005810 RID: 22544
		public Klei.AI.Attribute attribute;

		// Token: 0x04005811 RID: 22545
		public int value;
	}
}
