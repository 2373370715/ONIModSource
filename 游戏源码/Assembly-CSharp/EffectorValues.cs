using System;

// Token: 0x02001271 RID: 4721
[Serializable]
public struct EffectorValues
{
	// Token: 0x060060C6 RID: 24774 RVA: 0x000DF215 File Offset: 0x000DD415
	public EffectorValues(int amt, int rad)
	{
		this.amount = amt;
		this.radius = rad;
	}

	// Token: 0x060060C7 RID: 24775 RVA: 0x000DF225 File Offset: 0x000DD425
	public override bool Equals(object obj)
	{
		return obj is EffectorValues && this.Equals((EffectorValues)obj);
	}

	// Token: 0x060060C8 RID: 24776 RVA: 0x002B0D30 File Offset: 0x002AEF30
	public bool Equals(EffectorValues p)
	{
		return p != null && (this == p || (!(base.GetType() != p.GetType()) && this.amount == p.amount && this.radius == p.radius));
	}

	// Token: 0x060060C9 RID: 24777 RVA: 0x000DF23D File Offset: 0x000DD43D
	public override int GetHashCode()
	{
		return this.amount ^ this.radius;
	}

	// Token: 0x060060CA RID: 24778 RVA: 0x000DF24C File Offset: 0x000DD44C
	public static bool operator ==(EffectorValues lhs, EffectorValues rhs)
	{
		if (lhs == null)
		{
			return rhs == null;
		}
		return lhs.Equals(rhs);
	}

	// Token: 0x060060CB RID: 24779 RVA: 0x000DF26A File Offset: 0x000DD46A
	public static bool operator !=(EffectorValues lhs, EffectorValues rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x040044A4 RID: 17572
	public int amount;

	// Token: 0x040044A5 RID: 17573
	public int radius;
}
