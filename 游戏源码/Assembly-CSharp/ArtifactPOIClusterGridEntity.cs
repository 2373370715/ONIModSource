using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x020018AC RID: 6316
[SerializationConfig(MemberSerialization.OptIn)]
public class ArtifactPOIClusterGridEntity : ClusterGridEntity
{
	// Token: 0x1700085D RID: 2141
	// (get) Token: 0x060082CE RID: 33486 RVA: 0x000F5F7A File Offset: 0x000F417A
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x1700085E RID: 2142
	// (get) Token: 0x060082CF RID: 33487 RVA: 0x000A6603 File Offset: 0x000A4803
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x1700085F RID: 2143
	// (get) Token: 0x060082D0 RID: 33488 RVA: 0x0033E1A0 File Offset: 0x0033C3A0
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("gravitas_space_poi_kanim"),
					initialAnim = (this.m_Anim.IsNullOrWhiteSpace() ? "station_1" : this.m_Anim)
				}
			};
		}
	}

	// Token: 0x17000860 RID: 2144
	// (get) Token: 0x060082D1 RID: 33489 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000861 RID: 2145
	// (get) Token: 0x060082D2 RID: 33490 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x060082D3 RID: 33491 RVA: 0x000F5F82 File Offset: 0x000F4182
	public void Init(AxialI location)
	{
		base.Location = location;
	}

	// Token: 0x04006342 RID: 25410
	public string m_name;

	// Token: 0x04006343 RID: 25411
	public string m_Anim;
}
