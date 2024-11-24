using System;

// Token: 0x02001A1C RID: 6684
public struct UtilityNetworkGridNode : IEquatable<UtilityNetworkGridNode>
{
	// Token: 0x06008B44 RID: 35652 RVA: 0x000FB0C3 File Offset: 0x000F92C3
	public bool Equals(UtilityNetworkGridNode other)
	{
		return this.connections == other.connections && this.networkIdx == other.networkIdx;
	}

	// Token: 0x06008B45 RID: 35653 RVA: 0x0035E810 File Offset: 0x0035CA10
	public override bool Equals(object obj)
	{
		return ((UtilityNetworkGridNode)obj).Equals(this);
	}

	// Token: 0x06008B46 RID: 35654 RVA: 0x000FB0E3 File Offset: 0x000F92E3
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x06008B47 RID: 35655 RVA: 0x000FB0F5 File Offset: 0x000F92F5
	public static bool operator ==(UtilityNetworkGridNode x, UtilityNetworkGridNode y)
	{
		return x.Equals(y);
	}

	// Token: 0x06008B48 RID: 35656 RVA: 0x000FB0FF File Offset: 0x000F92FF
	public static bool operator !=(UtilityNetworkGridNode x, UtilityNetworkGridNode y)
	{
		return !x.Equals(y);
	}

	// Token: 0x040068C2 RID: 26818
	public UtilityConnections connections;

	// Token: 0x040068C3 RID: 26819
	public int networkIdx;

	// Token: 0x040068C4 RID: 26820
	public const int InvalidNetworkIdx = -1;
}
