using System;

namespace TUNING
{
	// Token: 0x02002261 RID: 8801
	public class MEDICINE
	{
		// Token: 0x04009989 RID: 39305
		public const float DEFAULT_MASS = 1f;

		// Token: 0x0400998A RID: 39306
		public const float RECUPERATION_DISEASE_MULTIPLIER = 1.1f;

		// Token: 0x0400998B RID: 39307
		public const float RECUPERATION_DOCTORED_DISEASE_MULTIPLIER = 1.2f;

		// Token: 0x0400998C RID: 39308
		public const float WORK_TIME = 10f;

		// Token: 0x0400998D RID: 39309
		public static readonly MedicineInfo BASICBOOSTER = new MedicineInfo("BasicBooster", "Medicine_BasicBooster", MedicineInfo.MedicineType.Booster, null, null);

		// Token: 0x0400998E RID: 39310
		public static readonly MedicineInfo INTERMEDIATEBOOSTER = new MedicineInfo("IntermediateBooster", "Medicine_IntermediateBooster", MedicineInfo.MedicineType.Booster, null, null);

		// Token: 0x0400998F RID: 39311
		public static readonly MedicineInfo BASICCURE = new MedicineInfo("BasicCure", null, MedicineInfo.MedicineType.CureSpecific, null, new string[]
		{
			"FoodSickness"
		});

		// Token: 0x04009990 RID: 39312
		public static readonly MedicineInfo ANTIHISTAMINE = new MedicineInfo("Antihistamine", "HistamineSuppression", MedicineInfo.MedicineType.CureSpecific, null, new string[]
		{
			"Allergies"
		});

		// Token: 0x04009991 RID: 39313
		public static readonly MedicineInfo INTERMEDIATECURE = new MedicineInfo("IntermediateCure", null, MedicineInfo.MedicineType.CureSpecific, "DoctorStation", new string[]
		{
			"SlimeSickness"
		});

		// Token: 0x04009992 RID: 39314
		public static readonly MedicineInfo ADVANCEDCURE = new MedicineInfo("AdvancedCure", null, MedicineInfo.MedicineType.CureSpecific, "AdvancedDoctorStation", new string[]
		{
			"ZombieSickness"
		});

		// Token: 0x04009993 RID: 39315
		public static readonly MedicineInfo BASICRADPILL = new MedicineInfo("BasicRadPill", "Medicine_BasicRadPill", MedicineInfo.MedicineType.Booster, null, null);

		// Token: 0x04009994 RID: 39316
		public static readonly MedicineInfo INTERMEDIATERADPILL = new MedicineInfo("IntermediateRadPill", "Medicine_IntermediateRadPill", MedicineInfo.MedicineType.Booster, "AdvancedDoctorStation", null);
	}
}
