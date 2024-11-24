using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02002174 RID: 8564
	public class TechItems : ResourceSet<TechItem>
	{
		// Token: 0x0600B61B RID: 46619 RVA: 0x001155B4 File Offset: 0x001137B4
		public TechItems(ResourceSet parent) : base("TechItems", parent)
		{
		}

		// Token: 0x0600B61C RID: 46620 RVA: 0x00456B5C File Offset: 0x00454D5C
		public void Init()
		{
			this.automationOverlay = this.AddTechItem("AutomationOverlay", RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.AUTOMATION_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_logic"), null, null, false);
			this.suitsOverlay = this.AddTechItem("SuitsOverlay", RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.SUITS_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_suit"), null, null, false);
			this.betaResearchPoint = this.AddTechItem("BetaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.BETA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_beta_icon"), null, null, false);
			this.gammaResearchPoint = this.AddTechItem("GammaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.GAMMA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_gamma_icon"), null, null, false);
			this.orbitalResearchPoint = this.AddTechItem("OrbitalResearchPoint", RESEARCH.OTHER_TECH_ITEMS.ORBITAL_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.ORBITAL_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_orbital_icon"), null, null, false);
			this.conveyorOverlay = this.AddTechItem("ConveyorOverlay", RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.NAME, RESEARCH.OTHER_TECH_ITEMS.CONVEYOR_OVERLAY.DESC, this.GetSpriteFnBuilder("overlay_conveyor"), null, null, false);
			this.jetSuit = this.AddTechItem("JetSuit", RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.JET_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Jet_Suit".ToTag()), null, null, false);
			this.atmoSuit = this.AddTechItem("AtmoSuit", RESEARCH.OTHER_TECH_ITEMS.ATMO_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.ATMO_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Atmo_Suit".ToTag()), null, null, false);
			this.oxygenMask = this.AddTechItem("OxygenMask", RESEARCH.OTHER_TECH_ITEMS.OXYGEN_MASK.NAME, RESEARCH.OTHER_TECH_ITEMS.OXYGEN_MASK.DESC, this.GetPrefabSpriteFnBuilder("Oxygen_Mask".ToTag()), null, null, false);
			this.deltaResearchPoint = this.AddTechItem("DeltaResearchPoint", RESEARCH.OTHER_TECH_ITEMS.DELTA_RESEARCH_POINT.NAME, RESEARCH.OTHER_TECH_ITEMS.DELTA_RESEARCH_POINT.DESC, this.GetSpriteFnBuilder("research_type_delta_icon"), DlcManager.EXPANSION1, null, false);
			this.leadSuit = this.AddTechItem("LeadSuit", RESEARCH.OTHER_TECH_ITEMS.LEAD_SUIT.NAME, RESEARCH.OTHER_TECH_ITEMS.LEAD_SUIT.DESC, this.GetPrefabSpriteFnBuilder("Lead_Suit".ToTag()), DlcManager.EXPANSION1, null, false);
			this.disposableElectrobankOrganic = this.AddTechItem("DisposableElectrobank_BasicSingleHarvestPlant", RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_ORGANIC.NAME, RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_ORGANIC.DESC, this.GetPrefabSpriteFnBuilder("DisposableElectrobank_BasicSingleHarvestPlant".ToTag()), DlcManager.DLC3, null, false);
			this.disposableElectrobankUraniumOre = this.AddTechItem("DisposableElectrobank_UraniumOre", RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_URANIUM_ORE.NAME, RESEARCH.OTHER_TECH_ITEMS.DISPOSABLE_ELECTROBANK_URANIUM_ORE.DESC, this.GetPrefabSpriteFnBuilder("DisposableElectrobank_UraniumOre".ToTag()), new string[]
			{
				"EXPANSION1_ID",
				"DLC3_ID"
			}, null, false);
			this.electrobank = this.AddTechItem("Electrobank", RESEARCH.OTHER_TECH_ITEMS.ELECTROBANK.NAME, RESEARCH.OTHER_TECH_ITEMS.ELECTROBANK.DESC, this.GetPrefabSpriteFnBuilder("Electrobank".ToTag()), DlcManager.DLC3, null, false);
		}

		// Token: 0x0600B61D RID: 46621 RVA: 0x001155C2 File Offset: 0x001137C2
		private Func<string, bool, Sprite> GetSpriteFnBuilder(string spriteName)
		{
			return (string anim, bool centered) => Assets.GetSprite(spriteName);
		}

		// Token: 0x0600B61E RID: 46622 RVA: 0x001155DB File Offset: 0x001137DB
		private Func<string, bool, Sprite> GetPrefabSpriteFnBuilder(Tag prefabTag)
		{
			return (string anim, bool centered) => Def.GetUISprite(prefabTag, "ui", false).first;
		}

		// Token: 0x0600B61F RID: 46623 RVA: 0x00456E7C File Offset: 0x0045507C
		[Obsolete("Used AddTechItem with requiredDlcIds and forbiddenDlcIds instead.")]
		public TechItem AddTechItem(string id, string name, string description, Func<string, bool, Sprite> getUISprite, string[] DLCIds, bool poi_unlock = false)
		{
			string[] requiredDlcIds;
			string[] forbiddenDlcIds;
			DlcManager.ConvertAvailableToRequireAndForbidden(DLCIds, out requiredDlcIds, out forbiddenDlcIds);
			return this.AddTechItem(id, name, description, getUISprite, requiredDlcIds, forbiddenDlcIds, poi_unlock);
		}

		// Token: 0x0600B620 RID: 46624 RVA: 0x00456EA4 File Offset: 0x004550A4
		public TechItem AddTechItem(string id, string name, string description, Func<string, bool, Sprite> getUISprite, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null, bool poi_unlock = false)
		{
			if (!DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds))
			{
				return null;
			}
			if (base.TryGet(id) != null)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Tried adding a tech item called",
					id,
					name,
					"but it was already added!"
				});
				return base.Get(id);
			}
			Tech techFromItemID = this.GetTechFromItemID(id);
			if (techFromItemID == null)
			{
				return null;
			}
			TechItem techItem = new TechItem(id, this, name, description, getUISprite, techFromItemID.Id, requiredDlcIds, forbiddenDlcIds, poi_unlock);
			techFromItemID.unlockedItems.Add(techItem);
			return techItem;
		}

		// Token: 0x0600B621 RID: 46625 RVA: 0x00456F24 File Offset: 0x00455124
		public bool IsTechItemComplete(string id)
		{
			bool result = true;
			foreach (TechItem techItem in this.resources)
			{
				if (techItem.Id == id)
				{
					result = techItem.IsComplete();
					break;
				}
			}
			return result;
		}

		// Token: 0x0600B622 RID: 46626 RVA: 0x001155F4 File Offset: 0x001137F4
		public Tech GetTechFromItemID(string itemId)
		{
			if (Db.Get().Techs == null)
			{
				return null;
			}
			return Db.Get().Techs.TryGetTechForTechItem(itemId);
		}

		// Token: 0x0600B623 RID: 46627 RVA: 0x00456F8C File Offset: 0x0045518C
		public int GetTechTierForItem(string itemId)
		{
			Tech techFromItemID = this.GetTechFromItemID(itemId);
			if (techFromItemID != null)
			{
				return Techs.GetTier(techFromItemID);
			}
			return 0;
		}

		// Token: 0x0400948F RID: 38031
		public const string AUTOMATION_OVERLAY_ID = "AutomationOverlay";

		// Token: 0x04009490 RID: 38032
		public TechItem automationOverlay;

		// Token: 0x04009491 RID: 38033
		public const string SUITS_OVERLAY_ID = "SuitsOverlay";

		// Token: 0x04009492 RID: 38034
		public TechItem suitsOverlay;

		// Token: 0x04009493 RID: 38035
		public const string JET_SUIT_ID = "JetSuit";

		// Token: 0x04009494 RID: 38036
		public TechItem jetSuit;

		// Token: 0x04009495 RID: 38037
		public const string ATMO_SUIT_ID = "AtmoSuit";

		// Token: 0x04009496 RID: 38038
		public TechItem atmoSuit;

		// Token: 0x04009497 RID: 38039
		public const string OXYGEN_MASK_ID = "OxygenMask";

		// Token: 0x04009498 RID: 38040
		public TechItem oxygenMask;

		// Token: 0x04009499 RID: 38041
		public const string LEAD_SUIT_ID = "LeadSuit";

		// Token: 0x0400949A RID: 38042
		public TechItem leadSuit;

		// Token: 0x0400949B RID: 38043
		public TechItem disposableElectrobankOrganic;

		// Token: 0x0400949C RID: 38044
		public TechItem disposableElectrobankUraniumOre;

		// Token: 0x0400949D RID: 38045
		public TechItem electrobank;

		// Token: 0x0400949E RID: 38046
		public const string BETA_RESEARCH_POINT_ID = "BetaResearchPoint";

		// Token: 0x0400949F RID: 38047
		public TechItem betaResearchPoint;

		// Token: 0x040094A0 RID: 38048
		public const string GAMMA_RESEARCH_POINT_ID = "GammaResearchPoint";

		// Token: 0x040094A1 RID: 38049
		public TechItem gammaResearchPoint;

		// Token: 0x040094A2 RID: 38050
		public const string DELTA_RESEARCH_POINT_ID = "DeltaResearchPoint";

		// Token: 0x040094A3 RID: 38051
		public TechItem deltaResearchPoint;

		// Token: 0x040094A4 RID: 38052
		public const string ORBITAL_RESEARCH_POINT_ID = "OrbitalResearchPoint";

		// Token: 0x040094A5 RID: 38053
		public TechItem orbitalResearchPoint;

		// Token: 0x040094A6 RID: 38054
		public const string CONVEYOR_OVERLAY_ID = "ConveyorOverlay";

		// Token: 0x040094A7 RID: 38055
		public TechItem conveyorOverlay;
	}
}
