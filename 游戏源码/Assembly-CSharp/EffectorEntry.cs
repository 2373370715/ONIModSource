using System;
using STRINGS;

// Token: 0x02001272 RID: 4722
internal struct EffectorEntry
{
	// Token: 0x060060CC RID: 24780 RVA: 0x000DF276 File Offset: 0x000DD476
	public EffectorEntry(string name, float value)
	{
		this.name = name;
		this.value = value;
		this.count = 1;
	}

	// Token: 0x060060CD RID: 24781 RVA: 0x002B0DA0 File Offset: 0x002AEFA0
	public override string ToString()
	{
		string arg = "";
		if (this.count > 1)
		{
			arg = string.Format(UI.OVERLAYS.DECOR.COUNT, this.count);
		}
		return string.Format(UI.OVERLAYS.DECOR.ENTRY, GameUtil.GetFormattedDecor(this.value, false), this.name, arg);
	}

	// Token: 0x040044A6 RID: 17574
	public string name;

	// Token: 0x040044A7 RID: 17575
	public int count;

	// Token: 0x040044A8 RID: 17576
	public float value;
}
