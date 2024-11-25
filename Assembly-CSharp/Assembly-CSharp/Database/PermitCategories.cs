using System;
using System.Collections.Generic;
using STRINGS;

namespace Database {
    public static class PermitCategories {
        private static readonly Dictionary<PermitCategory, CategoryInfo> CategoryInfos
            = new Dictionary<PermitCategory, CategoryInfo> {
                {
                    PermitCategory.Equipment,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.EQUIPMENT,
                                     "icon_inventory_equipment",
                                     Option.None)
                }, {
                    PermitCategory.DupeTops,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_TOPS,
                                     "icon_inventory_tops",
                                     ClothingOutfitUtility.OutfitType.Clothing)
                }, {
                    PermitCategory.DupeBottoms,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_BOTTOMS,
                                     "icon_inventory_bottoms",
                                     ClothingOutfitUtility.OutfitType.Clothing)
                }, {
                    PermitCategory.DupeGloves,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_GLOVES,
                                     "icon_inventory_gloves",
                                     ClothingOutfitUtility.OutfitType.Clothing)
                }, {
                    PermitCategory.DupeShoes,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_SHOES,
                                     "icon_inventory_shoes",
                                     ClothingOutfitUtility.OutfitType.Clothing)
                }, {
                    PermitCategory.DupeHats,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_HATS,
                                     "icon_inventory_hats",
                                     ClothingOutfitUtility.OutfitType.Clothing)
                }, {
                    PermitCategory.DupeAccessories,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPE_ACCESSORIES,
                                     "icon_inventory_accessories",
                                     ClothingOutfitUtility.OutfitType.Clothing)
                }, {
                    PermitCategory.AtmoSuitHelmet,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_HELMET,
                                     "icon_inventory_atmosuit_helmet",
                                     ClothingOutfitUtility.OutfitType.AtmoSuit)
                }, {
                    PermitCategory.AtmoSuitBody,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_BODY,
                                     "icon_inventory_atmosuit_body",
                                     ClothingOutfitUtility.OutfitType.AtmoSuit)
                }, {
                    PermitCategory.AtmoSuitGloves,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_GLOVES,
                                     "icon_inventory_atmosuit_gloves",
                                     ClothingOutfitUtility.OutfitType.AtmoSuit)
                }, {
                    PermitCategory.AtmoSuitBelt,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_BELT,
                                     "icon_inventory_atmosuit_belt",
                                     ClothingOutfitUtility.OutfitType.AtmoSuit)
                }, {
                    PermitCategory.AtmoSuitShoes,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ATMO_SUIT_SHOES,
                                     "icon_inventory_atmosuit_boots",
                                     ClothingOutfitUtility.OutfitType.AtmoSuit)
                }, {
                    PermitCategory.Building,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.BUILDINGS,
                                     "icon_inventory_buildings",
                                     Option.None)
                }, {
                    PermitCategory.Critter,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.CRITTERS,
                                     "icon_inventory_critters",
                                     Option.None)
                }, {
                    PermitCategory.Sweepy,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.SWEEPYS, "icon_inventory_sweepys", Option.None)
                }, {
                    PermitCategory.Duplicant,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.DUPLICANTS,
                                     "icon_inventory_duplicants",
                                     Option.None)
                }, {
                    PermitCategory.Artwork,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.ARTWORKS,
                                     "icon_inventory_artworks",
                                     Option.None)
                }, {
                    PermitCategory.JoyResponse,
                    new CategoryInfo(UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSE,
                                     "icon_inventory_joyresponses",
                                     ClothingOutfitUtility.OutfitType.JoyResponse)
                }
            };

        public static string GetDisplayName(PermitCategory category) { return CategoryInfos[category].displayName; }

        public static string GetUppercaseDisplayName(PermitCategory category) {
            return CategoryInfos[category].displayName.ToUpper();
        }

        public static string GetIconName(PermitCategory category) { return CategoryInfos[category].iconName; }

        public static PermitCategory GetCategoryForId(string id) {
            try { return (PermitCategory)Enum.Parse(typeof(PermitCategory), id); } catch (ArgumentException) {
                Debug.LogError(id + " is not a valid PermitCategory.");
            }

            return PermitCategory.Equipment;
        }

        public static Option<ClothingOutfitUtility.OutfitType> GetOutfitTypeFor(PermitCategory permitCategory) {
            return CategoryInfos[permitCategory].outfitType;
        }

        private class CategoryInfo {
            public readonly string                                   displayName;
            public readonly string                                   iconName;
            public readonly Option<ClothingOutfitUtility.OutfitType> outfitType;

            public CategoryInfo(string                                   displayName,
                                string                                   iconName,
                                Option<ClothingOutfitUtility.OutfitType> outfitType) {
                this.displayName = displayName;
                this.iconName    = iconName;
                this.outfitType  = outfitType;
            }
        }
    }
}