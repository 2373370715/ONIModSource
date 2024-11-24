using System;

// Token: 0x02001273 RID: 4723
internal struct EffectorEntryDecibel
{
	// Token: 0x060060CE RID: 24782 RVA: 0x000DF28D File Offset: 0x000DD48D
	public EffectorEntryDecibel(string name, float value)
	{
		this.name = name;
		this.value = value;
		this.count = 1;
	}

	// Token: 0x040044A9 RID: 17577
	public string name;

	// Token: 0x040044AA RID: 17578
	public int count;

	// Token: 0x040044AB RID: 17579
	public float value;
}
