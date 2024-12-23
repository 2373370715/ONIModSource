﻿using System;
using STRINGS;
using UnityEngine;

namespace Database {
    public class TechItems : ResourceSet<TechItem> {
        public const string   AUTOMATION_OVERLAY_ID     = "AutomationOverlay";
        public const string   SUITS_OVERLAY_ID          = "SuitsOverlay";
        public const string   JET_SUIT_ID               = "JetSuit";
        public const string   ATMO_SUIT_ID              = "AtmoSuit";
        public const string   OXYGEN_MASK_ID            = "OxygenMask";
        public const string   LEAD_SUIT_ID              = "LeadSuit";
        public const string   BETA_RESEARCH_POINT_ID    = "BetaResearchPoint";
        public const string   GAMMA_RESEARCH_POINT_ID   = "GammaResearchPoint";
        public const string   DELTA_RESEARCH_POINT_ID   = "DeltaResearchPoint";
        public const string   ORBITAL_RESEARCH_POINT_ID = "OrbitalResearchPoint";
        public const string   CONVEYOR_OVERLAY_ID       = "ConveyorOverlay";
        public       TechItem atmoSuit;
        public       TechItem automationOverlay;
        public       TechItem betaResearchPoint;
        public       TechItem conveyorOverlay;
        public       TechItem deltaResearchPoint;
        public       TechItem disposableElectrobankOrganic;
        public       TechItem disposableElectrobankUraniumOre;
        public       TechItem electrobank;
        public       TechItem gammaResearchPoint;
        public       TechItem jetSuit;
        public       TechItem leadSuit;
        public       TechItem orbitalResearchPoint;
        public       TechItem oxygenMask;
        public       TechItem suitsOverlay;
        public TechItems(ResourceSet parent) : base("TechItems", parent) { }

        public void Init() {
            automationOverlay = AddTechItem("AutomationOverlay",
                                            RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.NAME,
                                            RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.DESC,
                                            GetSpriteFnBuilder("overlay_logic"));

            suitsOverlay = AddTechItem("SuitsOverlay",
                                       RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.NAME,
                                       RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.DESC,
                                       GetSpriteFnBuilder("overlay_suit"));

            betaResearchPoint = AddTechItem("BetaResearchPoint",
                                            RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.NAME,
                                            RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.DESC,
                                            GetSpriteFnBuilder("research_type_beta_icon"));

            gammaResearchPoint = AddTechItem("GammaResearchPoint",
                                             RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.NAME,
                                             RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.DESC,
                                             GetSpriteFnBuilder("research_type_gamma_icon"));

            orbitalResearchPoint = AddTechItem("OrbitalResearchPoint",
                                               RESEARCH.OTHER_TECH_ITEMS.ORBITAL_RESEARCH_POINT.NAME,
                                               RESEARCH.OTHER_TECH_ITEMS.ORBITAL_RESEARCH_POINT.DESC,
                                               GetSpriteFnBuilder("research_type_orbital_icon"));

            conveyorOverlay = AddTechItem("ConveyorOverlay",
                                          RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.NAME,
                                          RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.DESC,
                                          GetSpriteFnBuilder("overlay_conveyor"));

            jetSuit = AddTechItem("JetSuit",
                                  RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.NAME,
                                  RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.DESC,
                                  GetPrefabSpriteFnBuilder("Jet_Suit".ToTag()));

            atmoSuit = AddTechItem("AtmoSuit",
                                   RESEARCH.OTHER_TECH_ITEMS.ATMO_SUIT.NAME,
                                   RESEARCH.OTHER_TECH_ITEMS.ATMO_SUIT.DESC,
                                   GetPrefabSpriteFnBuilder("Atmo_Suit".ToTag()));

            oxygenMask = AddTechItem("OxygenMask",
                                     RESEARCH.OTHER_TECH_ITEMS.OXYGEN_MASK.NAME,
                                     RESEARCH.OTHER_TECH_ITEMS.OXYGEN_MASK.DESC,
                                     GetPrefabSpriteFnBuilder("Oxygen_Mask".ToTag()));

            deltaResearchPoint = AddTechItem("DeltaResearchPoint",
                                             RESEARCH.OTHER_TECH_ITEMS.DELTA_RESEARCH_POINT.NAME,
                                             RESEARCH.OTHER_TECH_ITEMS.DELTA_RESEARCH_POINT.DESC,
                                             GetSpriteFnBuilder("research_type_delta_icon"),
                                             DlcManager.EXPANSION1,
                                             null);

            leadSuit = AddTechItem("LeadSuit",
                                   RESEARCH.OTHER_TECH_ITEMS.LEAD_SUIT.NAME,
                                   RESEARCH.OTHER_TECH_ITEMS.LEAD_SUIT.DESC,
                                   GetPrefabSpriteFnBuilder("Lead_Suit".ToTag()),
                                   DlcManager.EXPANSION1,
                                   null);

            disposableElectrobankOrganic = AddTechItem("DisposableElectrobank_BasicSingleHarvestPlant",
                                                       RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_ORGANIC.NAME,
                                                       RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_ORGANIC.DESC,
                                                       GetPrefabSpriteFnBuilder("DisposableElectrobank_BasicSingleHarvestPlant"
                                                                                    .ToTag()),
                                                       DlcManager.DLC3,
                                                       null);

            disposableElectrobankUraniumOre = AddTechItem("DisposableElectrobank_UraniumOre",
                                                          RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_URANIUM_ORE
                                                                  .NAME,
                                                          RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_URANIUM_ORE
                                                                  .DESC,
                                                          GetPrefabSpriteFnBuilder("DisposableElectrobank_UraniumOre"
                                                              .ToTag()),
                                                          new[] { "EXPANSION1_ID", "DLC3_ID" },
                                                          null);

            electrobank = AddTechItem("Electrobank",
                                      RESEARCH.OTHER_TECH_ITEMS.ELECTROBANK.NAME,
                                      RESEARCH.OTHER_TECH_ITEMS.ELECTROBANK.DESC,
                                      GetPrefabSpriteFnBuilder("Electrobank".ToTag()),
                                      DlcManager.DLC3,
                                      null);
        }

        private Func<string, bool, Sprite> GetSpriteFnBuilder(string spriteName) {
            return (anim, centered) => Assets.GetSprite(spriteName);
        }

        private Func<string, bool, Sprite> GetPrefabSpriteFnBuilder(Tag prefabTag) {
            return (anim, centered) => Def.GetUISprite(prefabTag).first;
        }

        [Obsolete("Used AddTechItem with requiredDlcIds and forbiddenDlcIds instead.")]
        public TechItem AddTechItem(string                     id,
                                    string                     name,
                                    string                     description,
                                    Func<string, bool, Sprite> getUISprite,
                                    string[]                   DLCIds,
                                    bool                       poi_unlock = false) {
            string[] requiredDlcIds;
            string[] forbiddenDlcIds;
            DlcManager.ConvertAvailableToRequireAndForbidden(DLCIds, out requiredDlcIds, out forbiddenDlcIds);
            return AddTechItem(id, name, description, getUISprite, requiredDlcIds, forbiddenDlcIds, poi_unlock);
        }

        public TechItem AddTechItem(string                     id,
                                    string                     name,
                                    string                     description,
                                    Func<string, bool, Sprite> getUISprite,
                                    string[]                   requiredDlcIds  = null,
                                    string[]                   forbiddenDlcIds = null,
                                    bool                       poi_unlock      = false) {
            if (!DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds)) return null;

            if (TryGet(id) != null) {
                DebugUtil.LogWarningArgs("Tried adding a tech item called", id, name, "but it was already added!");
                return Get(id);
            }

            var techFromItemID = GetTechFromItemID(id);
            if (techFromItemID == null) return null;

            var techItem = new TechItem(id,
                                        this,
                                        name,
                                        description,
                                        getUISprite,
                                        techFromItemID.Id,
                                        requiredDlcIds,
                                        forbiddenDlcIds,
                                        poi_unlock);

            techFromItemID.unlockedItems.Add(techItem);
            return techItem;
        }

        public bool IsTechItemComplete(string id) {
            var result = true;
            foreach (var techItem in resources)
                if (techItem.Id == id) {
                    result = techItem.IsComplete();
                    break;
                }

            return result;
        }

        public Tech GetTechFromItemID(string itemId) {
            if (Db.Get().Techs == null) return null;

            return Db.Get().Techs.TryGetTechForTechItem(itemId);
        }

        public int GetTechTierForItem(string itemId) {
            var techFromItemID = GetTechFromItemID(itemId);
            if (techFromItemID != null) return Techs.GetTier(techFromItemID);

            return 0;
        }
    }
}