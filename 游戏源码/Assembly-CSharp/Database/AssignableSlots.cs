using System;
using STRINGS;
using TUNING;

namespace Database
{
	// Token: 0x02002148 RID: 8520
	public class AssignableSlots : ResourceSet<AssignableSlot>
	{
		// Token: 0x0600B596 RID: 46486 RVA: 0x0045210C File Offset: 0x0045030C
		public AssignableSlots()
		{
			this.Bed = base.Add(new OwnableSlot("Bed", MISC.TAGS.BED));
			this.MessStation = base.Add(new OwnableSlot("MessStation", MISC.TAGS.MESSSTATION));
			this.Clinic = base.Add(new OwnableSlot("Clinic", MISC.TAGS.CLINIC));
			this.MedicalBed = base.Add(new OwnableSlot("MedicalBed", MISC.TAGS.CLINIC));
			this.MedicalBed.showInUI = false;
			this.GeneShuffler = base.Add(new OwnableSlot("GeneShuffler", MISC.TAGS.GENE_SHUFFLER));
			this.GeneShuffler.showInUI = false;
			this.Toilet = base.Add(new OwnableSlot("Toilet", MISC.TAGS.TOILET));
			this.MassageTable = base.Add(new OwnableSlot("MassageTable", MISC.TAGS.MASSAGE_TABLE));
			this.RocketCommandModule = base.Add(new OwnableSlot("RocketCommandModule", MISC.TAGS.COMMAND_MODULE));
			this.HabitatModule = base.Add(new OwnableSlot("HabitatModule", MISC.TAGS.HABITAT_MODULE));
			this.ResetSkillsStation = base.Add(new OwnableSlot("ResetSkillsStation", "ResetSkillsStation"));
			this.WarpPortal = base.Add(new OwnableSlot("WarpPortal", MISC.TAGS.WARP_PORTAL));
			this.WarpPortal.showInUI = false;
			this.BionicUpgrade = base.Add(new OwnableSlot("BionicUpgrade", MISC.TAGS.BIONIC_UPGRADE));
			this.Toy = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.TOYS.SLOT, MISC.TAGS.TOY, false));
			this.Suit = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.SUITS.SLOT, MISC.TAGS.SUIT, true));
			this.Tool = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.TOOLS.TOOLSLOT, MISC.TAGS.MULTITOOL, false));
			this.Outfit = base.Add(new EquipmentSlot(TUNING.EQUIPMENT.CLOTHING.SLOT, UI.StripLinkFormatting(MISC.TAGS.CLOTHES), true));
		}

		// Token: 0x04009369 RID: 37737
		public AssignableSlot Bed;

		// Token: 0x0400936A RID: 37738
		public AssignableSlot MessStation;

		// Token: 0x0400936B RID: 37739
		public AssignableSlot Clinic;

		// Token: 0x0400936C RID: 37740
		public AssignableSlot GeneShuffler;

		// Token: 0x0400936D RID: 37741
		public AssignableSlot MedicalBed;

		// Token: 0x0400936E RID: 37742
		public AssignableSlot Toilet;

		// Token: 0x0400936F RID: 37743
		public AssignableSlot MassageTable;

		// Token: 0x04009370 RID: 37744
		public AssignableSlot RocketCommandModule;

		// Token: 0x04009371 RID: 37745
		public AssignableSlot HabitatModule;

		// Token: 0x04009372 RID: 37746
		public AssignableSlot ResetSkillsStation;

		// Token: 0x04009373 RID: 37747
		public AssignableSlot WarpPortal;

		// Token: 0x04009374 RID: 37748
		public AssignableSlot Toy;

		// Token: 0x04009375 RID: 37749
		public AssignableSlot Suit;

		// Token: 0x04009376 RID: 37750
		public AssignableSlot Tool;

		// Token: 0x04009377 RID: 37751
		public AssignableSlot Outfit;

		// Token: 0x04009378 RID: 37752
		public AssignableSlot BionicUpgrade;
	}
}
