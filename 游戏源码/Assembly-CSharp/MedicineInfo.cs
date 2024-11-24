using System;
using System.Collections.Generic;

// Token: 0x020014D0 RID: 5328
[Serializable]
public class MedicineInfo
{
	// Token: 0x06006F0A RID: 28426 RVA: 0x002F0DE4 File Offset: 0x002EEFE4
	public MedicineInfo(string id, string effect, MedicineInfo.MedicineType medicineType, string doctorStationId, string[] curedDiseases = null)
	{
		Debug.Assert(!string.IsNullOrEmpty(effect) || (curedDiseases != null && curedDiseases.Length != 0), "Medicine should have an effect or cure diseases");
		this.id = id;
		this.effect = effect;
		this.medicineType = medicineType;
		this.doctorStationId = doctorStationId;
		if (curedDiseases != null)
		{
			this.curedSicknesses = new List<string>(curedDiseases);
			return;
		}
		this.curedSicknesses = new List<string>();
	}

	// Token: 0x06006F0B RID: 28427 RVA: 0x000E8BED File Offset: 0x000E6DED
	public Tag GetSupplyTag()
	{
		return MedicineInfo.GetSupplyTagForStation(this.doctorStationId);
	}

	// Token: 0x06006F0C RID: 28428 RVA: 0x002F0E54 File Offset: 0x002EF054
	public static Tag GetSupplyTagForStation(string stationID)
	{
		Tag tag = TagManager.Create(stationID + GameTags.MedicalSupplies.Name);
		Assets.AddCountableTag(tag);
		return tag;
	}

	// Token: 0x040052EC RID: 21228
	public string id;

	// Token: 0x040052ED RID: 21229
	public string effect;

	// Token: 0x040052EE RID: 21230
	public MedicineInfo.MedicineType medicineType;

	// Token: 0x040052EF RID: 21231
	public List<string> curedSicknesses;

	// Token: 0x040052F0 RID: 21232
	public string doctorStationId;

	// Token: 0x020014D1 RID: 5329
	public enum MedicineType
	{
		// Token: 0x040052F2 RID: 21234
		Booster,
		// Token: 0x040052F3 RID: 21235
		CureAny,
		// Token: 0x040052F4 RID: 21236
		CureSpecific
	}
}
