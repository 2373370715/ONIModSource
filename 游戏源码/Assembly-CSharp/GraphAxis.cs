using System;

// Token: 0x02001CEA RID: 7402
[Serializable]
public struct GraphAxis
{
	// Token: 0x17000A36 RID: 2614
	// (get) Token: 0x06009A9D RID: 39581 RVA: 0x001048C1 File Offset: 0x00102AC1
	public float range
	{
		get
		{
			return this.max_value - this.min_value;
		}
	}

	// Token: 0x040078C6 RID: 30918
	public string name;

	// Token: 0x040078C7 RID: 30919
	public float min_value;

	// Token: 0x040078C8 RID: 30920
	public float max_value;

	// Token: 0x040078C9 RID: 30921
	public float guide_frequency;
}
