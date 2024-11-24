using System;

// Token: 0x02000B43 RID: 2883
public class ToiletTracker : WorldTracker
{
	// Token: 0x060036BD RID: 14013 RVA: 0x000C3935 File Offset: 0x000C1B35
	public ToiletTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x060036BE RID: 14014 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
	public override void UpdateData()
	{
		throw new NotImplementedException();
	}

	// Token: 0x060036BF RID: 14015 RVA: 0x000C20D4 File Offset: 0x000C02D4
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
