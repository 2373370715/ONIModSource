using System;

// Token: 0x02000769 RID: 1897
public struct PrioritySetting : IComparable<PrioritySetting>
{
	// Token: 0x06002205 RID: 8709 RVA: 0x001C115C File Offset: 0x001BF35C
	public override int GetHashCode()
	{
		return ((int)((int)this.priority_class << 28)).GetHashCode() ^ this.priority_value.GetHashCode();
	}

	// Token: 0x06002206 RID: 8710 RVA: 0x000B62CF File Offset: 0x000B44CF
	public static bool operator ==(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.Equals(rhs);
	}

	// Token: 0x06002207 RID: 8711 RVA: 0x000B62E4 File Offset: 0x000B44E4
	public static bool operator !=(PrioritySetting lhs, PrioritySetting rhs)
	{
		return !lhs.Equals(rhs);
	}

	// Token: 0x06002208 RID: 8712 RVA: 0x000B62FC File Offset: 0x000B44FC
	public static bool operator <=(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) <= 0;
	}

	// Token: 0x06002209 RID: 8713 RVA: 0x000B630C File Offset: 0x000B450C
	public static bool operator >=(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) >= 0;
	}

	// Token: 0x0600220A RID: 8714 RVA: 0x000B631C File Offset: 0x000B451C
	public static bool operator <(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) < 0;
	}

	// Token: 0x0600220B RID: 8715 RVA: 0x000B6329 File Offset: 0x000B4529
	public static bool operator >(PrioritySetting lhs, PrioritySetting rhs)
	{
		return lhs.CompareTo(rhs) > 0;
	}

	// Token: 0x0600220C RID: 8716 RVA: 0x000B6336 File Offset: 0x000B4536
	public override bool Equals(object obj)
	{
		return obj is PrioritySetting && ((PrioritySetting)obj).priority_class == this.priority_class && ((PrioritySetting)obj).priority_value == this.priority_value;
	}

	// Token: 0x0600220D RID: 8717 RVA: 0x001C1188 File Offset: 0x001BF388
	public int CompareTo(PrioritySetting other)
	{
		if (this.priority_class > other.priority_class)
		{
			return 1;
		}
		if (this.priority_class < other.priority_class)
		{
			return -1;
		}
		if (this.priority_value > other.priority_value)
		{
			return 1;
		}
		if (this.priority_value < other.priority_value)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x0600220E RID: 8718 RVA: 0x000B636A File Offset: 0x000B456A
	public PrioritySetting(PriorityScreen.PriorityClass priority_class, int priority_value)
	{
		this.priority_class = priority_class;
		this.priority_value = priority_value;
	}

	// Token: 0x04001659 RID: 5721
	public PriorityScreen.PriorityClass priority_class;

	// Token: 0x0400165A RID: 5722
	public int priority_value;
}
