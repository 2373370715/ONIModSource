using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x020018E2 RID: 6370
[SerializationConfig(MemberSerialization.OptIn)]
public class HarvestablePOIClusterGridEntity : ClusterGridEntity
{
	// Token: 0x170008A8 RID: 2216
	// (get) Token: 0x060084A3 RID: 33955 RVA: 0x000F71E9 File Offset: 0x000F53E9
	public override string Name
	{
		get
		{
			return this.m_name;
		}
	}

	// Token: 0x170008A9 RID: 2217
	// (get) Token: 0x060084A4 RID: 33956 RVA: 0x000A6603 File Offset: 0x000A4803
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x170008AA RID: 2218
	// (get) Token: 0x060084A5 RID: 33957 RVA: 0x00344C8C File Offset: 0x00342E8C
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("harvestable_space_poi_kanim"),
					initialAnim = (this.m_Anim.IsNullOrWhiteSpace() ? "cloud" : this.m_Anim)
				}
			};
		}
	}

	// Token: 0x170008AB RID: 2219
	// (get) Token: 0x060084A6 RID: 33958 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170008AC RID: 2220
	// (get) Token: 0x060084A7 RID: 33959 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x060084A8 RID: 33960 RVA: 0x000F5F82 File Offset: 0x000F4182
	public void Init(AxialI location)
	{
		base.Location = location;
	}

	// Token: 0x04006433 RID: 25651
	public string m_name;

	// Token: 0x04006434 RID: 25652
	public string m_Anim;
}
