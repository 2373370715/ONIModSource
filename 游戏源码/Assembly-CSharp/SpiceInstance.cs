using System;
using Klei.AI;

// Token: 0x020005A8 RID: 1448
[Serializable]
public struct SpiceInstance
{
	// Token: 0x17000087 RID: 135
	// (get) Token: 0x060019CB RID: 6603 RVA: 0x000B0F66 File Offset: 0x000AF166
	public AttributeModifier CalorieModifier
	{
		get
		{
			return SpiceGrinder.SettingOptions[this.Id].Spice.CalorieModifier;
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x060019CC RID: 6604 RVA: 0x000B0F82 File Offset: 0x000AF182
	public AttributeModifier FoodModifier
	{
		get
		{
			return SpiceGrinder.SettingOptions[this.Id].Spice.FoodModifier;
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x060019CD RID: 6605 RVA: 0x000B0F9E File Offset: 0x000AF19E
	public Effect StatBonus
	{
		get
		{
			return SpiceGrinder.SettingOptions[this.Id].StatBonus;
		}
	}

	// Token: 0x04001073 RID: 4211
	public Tag Id;

	// Token: 0x04001074 RID: 4212
	public float TotalKG;
}
