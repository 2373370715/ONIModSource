using System;

// Token: 0x020010BC RID: 4284
[Serializable]
public class AttackEffect
{
	// Token: 0x060057D0 RID: 22480 RVA: 0x000D9432 File Offset: 0x000D7632
	public AttackEffect(string ID, float probability)
	{
		this.effectID = ID;
		this.effectProbability = probability;
	}

	// Token: 0x04003D59 RID: 15705
	public string effectID;

	// Token: 0x04003D5A RID: 15706
	public float effectProbability;
}
