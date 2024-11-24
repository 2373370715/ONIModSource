using System;
using Klei.AI;
using STRINGS;

namespace Database
{
	// Token: 0x02002125 RID: 8485
	public class ChoreGroups : ResourceSet<ChoreGroup>
	{
		// Token: 0x0600B4AD RID: 46253 RVA: 0x0044510C File Offset: 0x0044330C
		private ChoreGroup Add(string id, string name, Klei.AI.Attribute attribute, string sprite, int default_personal_priority, bool user_prioritizable = true)
		{
			ChoreGroup choreGroup = new ChoreGroup(id, name, attribute, sprite, default_personal_priority, user_prioritizable);
			base.Add(choreGroup);
			return choreGroup;
		}

		// Token: 0x0600B4AE RID: 46254 RVA: 0x00445134 File Offset: 0x00443334
		public ChoreGroups(ResourceSet parent) : base("ChoreGroups", parent)
		{
			this.Combat = this.Add("Combat", DUPLICANTS.CHOREGROUPS.COMBAT.NAME, Db.Get().Attributes.Digging, "icon_errand_combat", 5, true);
			this.LifeSupport = this.Add("LifeSupport", DUPLICANTS.CHOREGROUPS.LIFESUPPORT.NAME, Db.Get().Attributes.LifeSupport, "icon_errand_life_support", 5, true);
			this.Toggle = this.Add("Toggle", DUPLICANTS.CHOREGROUPS.TOGGLE.NAME, Db.Get().Attributes.Toggle, "icon_errand_toggle", 5, true);
			this.MedicalAid = this.Add("MedicalAid", DUPLICANTS.CHOREGROUPS.MEDICALAID.NAME, Db.Get().Attributes.Caring, "icon_errand_care", 4, true);
			if (DlcManager.FeatureClusterSpaceEnabled())
			{
				this.Rocketry = this.Add("Rocketry", DUPLICANTS.CHOREGROUPS.ROCKETRY.NAME, Db.Get().Attributes.SpaceNavigation, "icon_errand_rocketry", 4, true);
			}
			this.Basekeeping = this.Add("Basekeeping", DUPLICANTS.CHOREGROUPS.BASEKEEPING.NAME, Db.Get().Attributes.Strength, "icon_errand_tidy", 4, true);
			this.Cook = this.Add("Cook", DUPLICANTS.CHOREGROUPS.COOK.NAME, Db.Get().Attributes.Cooking, "icon_errand_cook", 3, true);
			this.Art = this.Add("Art", DUPLICANTS.CHOREGROUPS.ART.NAME, Db.Get().Attributes.Art, "icon_errand_art", 3, true);
			this.Research = this.Add("Research", DUPLICANTS.CHOREGROUPS.RESEARCH.NAME, Db.Get().Attributes.Learning, "icon_errand_research", 3, true);
			this.MachineOperating = this.Add("MachineOperating", DUPLICANTS.CHOREGROUPS.MACHINEOPERATING.NAME, Db.Get().Attributes.Machinery, "icon_errand_operate", 3, true);
			this.Farming = this.Add("Farming", DUPLICANTS.CHOREGROUPS.FARMING.NAME, Db.Get().Attributes.Botanist, "icon_errand_farm", 3, true);
			this.Ranching = this.Add("Ranching", DUPLICANTS.CHOREGROUPS.RANCHING.NAME, Db.Get().Attributes.Ranching, "icon_errand_ranch", 3, true);
			this.Build = this.Add("Build", DUPLICANTS.CHOREGROUPS.BUILD.NAME, Db.Get().Attributes.Construction, "icon_errand_toggle", 2, true);
			this.Dig = this.Add("Dig", DUPLICANTS.CHOREGROUPS.DIG.NAME, Db.Get().Attributes.Digging, "icon_errand_dig", 2, true);
			this.Hauling = this.Add("Hauling", DUPLICANTS.CHOREGROUPS.HAULING.NAME, Db.Get().Attributes.Strength, "icon_errand_supply", 1, true);
			this.Storage = this.Add("Storage", DUPLICANTS.CHOREGROUPS.STORAGE.NAME, Db.Get().Attributes.Strength, "icon_errand_storage", 1, true);
			this.Recreation = this.Add("Recreation", DUPLICANTS.CHOREGROUPS.RECREATION.NAME, Db.Get().Attributes.Strength, "icon_errand_storage", 1, false);
			Debug.Assert(true);
		}

		// Token: 0x0600B4AF RID: 46255 RVA: 0x0044549C File Offset: 0x0044369C
		public ChoreGroup FindByHash(HashedString id)
		{
			ChoreGroup result = null;
			foreach (ChoreGroup choreGroup in Db.Get().ChoreGroups.resources)
			{
				if (choreGroup.IdHash == id)
				{
					result = choreGroup;
					break;
				}
			}
			return result;
		}

		// Token: 0x04009053 RID: 36947
		public ChoreGroup Build;

		// Token: 0x04009054 RID: 36948
		public ChoreGroup Basekeeping;

		// Token: 0x04009055 RID: 36949
		public ChoreGroup Cook;

		// Token: 0x04009056 RID: 36950
		public ChoreGroup Art;

		// Token: 0x04009057 RID: 36951
		public ChoreGroup Dig;

		// Token: 0x04009058 RID: 36952
		public ChoreGroup Research;

		// Token: 0x04009059 RID: 36953
		public ChoreGroup Farming;

		// Token: 0x0400905A RID: 36954
		public ChoreGroup Ranching;

		// Token: 0x0400905B RID: 36955
		public ChoreGroup Hauling;

		// Token: 0x0400905C RID: 36956
		public ChoreGroup Storage;

		// Token: 0x0400905D RID: 36957
		public ChoreGroup MachineOperating;

		// Token: 0x0400905E RID: 36958
		public ChoreGroup MedicalAid;

		// Token: 0x0400905F RID: 36959
		public ChoreGroup Combat;

		// Token: 0x04009060 RID: 36960
		public ChoreGroup LifeSupport;

		// Token: 0x04009061 RID: 36961
		public ChoreGroup Toggle;

		// Token: 0x04009062 RID: 36962
		public ChoreGroup Recreation;

		// Token: 0x04009063 RID: 36963
		public ChoreGroup Rocketry;
	}
}
