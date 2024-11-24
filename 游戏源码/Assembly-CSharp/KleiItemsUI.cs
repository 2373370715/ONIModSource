using Database;
using STRINGS;
using UnityEngine;

public static class KleiItemsUI
{
	public static readonly Color TEXT_COLOR__PERMIT_NOT_OWNED = GetColor("#DD992F");

	public static string WrapAsToolTipTitle(string text)
	{
		return "<b><style=\"KLink\">" + text + "</style></b>";
	}

	public static string WrapWithColor(string text, Color color)
	{
		return "<color=#" + color.ToHexString() + ">" + text + "</color>";
	}

	public static Sprite GetNoneClothingItemIcon(PermitCategory category, Option<Personality> personality)
	{
		return GetNoneIconForCategory(category, personality);
	}

	public static Sprite GetNoneBalloonArtistIcon()
	{
		return GetNoneIconForCategory(PermitCategory.JoyResponse, null);
	}

	private static Sprite GetNoneIconForCategory(PermitCategory category, Option<Personality> personality)
	{
		return Assets.GetSprite(GetIconName(category, personality));
		static string GetIconName(PermitCategory category, Option<Personality> personality)
		{
			return category switch
			{
				PermitCategory.DupeTops => "default_no_top", 
				PermitCategory.DupeGloves => "default_no_gloves", 
				PermitCategory.DupeBottoms => "default_no_bottom", 
				PermitCategory.DupeShoes => "default_no_footwear", 
				PermitCategory.DupeHats => "icon_inventory_hats", 
				PermitCategory.DupeAccessories => "icon_inventory_accessories", 
				PermitCategory.AtmoSuitHelmet => "icon_inventory_atmosuit_helmet", 
				PermitCategory.AtmoSuitBody => "icon_inventory_atmosuit_body", 
				PermitCategory.AtmoSuitGloves => "icon_inventory_atmosuit_gloves", 
				PermitCategory.AtmoSuitBelt => "icon_inventory_atmosuit_belt", 
				PermitCategory.AtmoSuitShoes => "icon_inventory_atmosuit_boots", 
				PermitCategory.Building => "icon_inventory_buildings", 
				PermitCategory.Critter => "icon_inventory_critters", 
				PermitCategory.Sweepy => "icon_inventory_sweepys", 
				PermitCategory.Duplicant => "icon_inventory_duplicants", 
				PermitCategory.Artwork => "icon_inventory_artworks", 
				PermitCategory.JoyResponse => "icon_inventory_joyresponses", 
				_ => "NoTraits", 
			};
		}
	}

	public static string GetNoneOutfitName(ClothingOutfitUtility.OutfitType outfitType)
	{
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			return UI.OUTFIT_NAME.NONE;
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			return UI.OUTFIT_NAME.NONE_ATMO_SUIT;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			return UI.OUTFIT_NAME.NONE_JOY_RESPONSE;
		default:
			DebugUtil.DevAssert(test: false, $"Couldn't find \"no item\" string for outfit {outfitType}");
			return "-";
		}
	}

	public static (string name, string desc) GetNoneClothingItemStrings(PermitCategory category)
	{
		switch (category)
		{
		case PermitCategory.DupeHats:
			return (name: EQUIPMENT.PREFABS.CLOTHING_HATS.NAME, desc: EQUIPMENT.PREFABS.CLOTHING_HATS.DESC);
		case PermitCategory.DupeTops:
			return (name: EQUIPMENT.PREFABS.CLOTHING_TOPS.NAME, desc: EQUIPMENT.PREFABS.CLOTHING_TOPS.DESC);
		case PermitCategory.DupeGloves:
			return (name: EQUIPMENT.PREFABS.CLOTHING_GLOVES.NAME, desc: EQUIPMENT.PREFABS.CLOTHING_GLOVES.DESC);
		case PermitCategory.DupeBottoms:
			return (name: EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.NAME, desc: EQUIPMENT.PREFABS.CLOTHING_BOTTOMS.DESC);
		case PermitCategory.DupeShoes:
			return (name: EQUIPMENT.PREFABS.CLOTHING_SHOES.NAME, desc: EQUIPMENT.PREFABS.CLOTHING_SHOES.DESC);
		case PermitCategory.DupeAccessories:
			return (name: EQUIPMENT.PREFABS.CLOTHING_ACCESORIES.NAME, desc: EQUIPMENT.PREFABS.CLOTHING_ACCESORIES.DESC);
		case PermitCategory.JoyResponse:
			return (name: UI.OUTFIT_DESCRIPTION.NO_JOY_RESPONSE_NAME, desc: UI.OUTFIT_DESCRIPTION.NO_JOY_RESPONSE_DESC);
		case PermitCategory.AtmoSuitHelmet:
			return (name: EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.NAME, desc: EQUIPMENT.PREFABS.ATMO_SUIT_HELMET.DESC);
		case PermitCategory.AtmoSuitBody:
			return (name: EQUIPMENT.PREFABS.ATMO_SUIT_BODY.NAME, desc: EQUIPMENT.PREFABS.ATMO_SUIT_BODY.DESC);
		case PermitCategory.AtmoSuitGloves:
			return (name: EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.NAME, desc: EQUIPMENT.PREFABS.ATMO_SUIT_GLOVES.DESC);
		case PermitCategory.AtmoSuitBelt:
			return (name: EQUIPMENT.PREFABS.ATMO_SUIT_BELT.NAME, desc: EQUIPMENT.PREFABS.ATMO_SUIT_BELT.DESC);
		case PermitCategory.AtmoSuitShoes:
			return (name: EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.NAME, desc: EQUIPMENT.PREFABS.ATMO_SUIT_SHOES.DESC);
		default:
			DebugUtil.DevAssert(test: false, $"Couldn't find \"no item\" string for category {category}");
			return (name: "-", desc: "-");
		}
	}

	public static void ConfigureTooltipOn(GameObject gameObject, Option<LocString> tooltipText = default(Option<LocString>))
	{
		ConfigureTooltipOn(gameObject, tooltipText.HasValue ? Option.Some((string)tooltipText.Value) : ((Option<string>)Option.None));
	}

	public static void ConfigureTooltipOn(GameObject gameObject, Option<string> tooltipText = default(Option<string>))
	{
		ToolTip toolTip = gameObject.GetComponent<ToolTip>();
		if (toolTip.IsNullOrDestroyed())
		{
			toolTip = gameObject.AddComponent<ToolTip>();
			toolTip.tooltipPivot = new Vector2(0.5f, 1f);
			if ((bool)gameObject.GetComponent<KButton>())
			{
				toolTip.tooltipPositionOffset = new Vector2(0f, 22f);
			}
			else
			{
				toolTip.tooltipPositionOffset = new Vector2(0f, 0f);
			}
			toolTip.parentPositionAnchor = new Vector2(0.5f, 0f);
			toolTip.toolTipPosition = ToolTip.TooltipPosition.Custom;
		}
		if (!tooltipText.HasValue)
		{
			toolTip.ClearMultiStringTooltip();
		}
		else
		{
			toolTip.SetSimpleTooltip(tooltipText.Value);
		}
	}

	public static string GetTooltipStringFor(PermitResource permit)
	{
		string text = WrapAsToolTipTitle(permit.Name);
		if (!string.IsNullOrWhiteSpace(permit.Description))
		{
			text = text + "\n" + permit.Description;
		}
		string dlcIdFrom = permit.GetDlcIdFrom();
		if (DlcManager.IsDlcId(dlcIdFrom))
		{
			text = ((permit.Rarity != PermitRarity.UniversalLocked) ? (text + "\n\n" + UI.KLEI_INVENTORY_SCREEN.COLLECTION.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom))) : (text + "\n\n" + UI.KLEI_INVENTORY_SCREEN.COLLECTION_COMING_SOON.Replace("{Collection}", DlcManager.GetDlcTitle(dlcIdFrom))));
		}
		else
		{
			string text2 = UI.KLEI_INVENTORY_SCREEN.ITEM_RARITY_DETAILS.Replace("{RarityName}", permit.Rarity.GetLocStringName());
			if (!string.IsNullOrWhiteSpace(text2))
			{
				text = text + "\n\n" + text2;
			}
		}
		if (permit.IsOwnableOnServer() && PermitItems.GetOwnedCount(permit) <= 0)
		{
			text = text + "\n\n" + WrapWithColor(UI.KLEI_INVENTORY_SCREEN.ITEM_PLAYER_OWN_NONE, TEXT_COLOR__PERMIT_NOT_OWNED);
		}
		return text;
	}

	public static string GetNoneTooltipStringFor(PermitCategory category)
	{
		var (text, text2) = GetNoneClothingItemStrings(category);
		return WrapAsToolTipTitle(text) + "\n" + text2;
	}

	public static Color GetColor(string input)
	{
		if (input[0] == '#')
		{
			return Util.ColorFromHex(input.Substring(1));
		}
		return Util.ColorFromHex(input);
	}
}
