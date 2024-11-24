using System;

// Token: 0x0200042C RID: 1068
public class ArtifactTier
{
	// Token: 0x06001238 RID: 4664 RVA: 0x000AE250 File Offset: 0x000AC450
	public ArtifactTier(StringKey str_key, EffectorValues values, float payload_drop_chance)
	{
		this.decorValues = values;
		this.name_key = str_key;
		this.payloadDropChance = payload_drop_chance;
	}

	// Token: 0x04000C6F RID: 3183
	public EffectorValues decorValues;

	// Token: 0x04000C70 RID: 3184
	public StringKey name_key;

	// Token: 0x04000C71 RID: 3185
	public float payloadDropChance;
}
