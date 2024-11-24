using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;

// Token: 0x0200191C RID: 6428
[SerializationConfig(MemberSerialization.OptIn)]
public class ResearchDestination : ClusterGridEntity
{
	// Token: 0x170008D6 RID: 2262
	// (get) Token: 0x060085EA RID: 34282 RVA: 0x000F7BFB File Offset: 0x000F5DFB
	public override string Name
	{
		get
		{
			return UI.SPACEDESTINATIONS.RESEARCHDESTINATION.NAME;
		}
	}

	// Token: 0x170008D7 RID: 2263
	// (get) Token: 0x060085EB RID: 34283 RVA: 0x000A6603 File Offset: 0x000A4803
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.POI;
		}
	}

	// Token: 0x170008D8 RID: 2264
	// (get) Token: 0x060085EC RID: 34284 RVA: 0x000F7C07 File Offset: 0x000F5E07
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>();
		}
	}

	// Token: 0x170008D9 RID: 2265
	// (get) Token: 0x060085ED RID: 34285 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public override bool IsVisible
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170008DA RID: 2266
	// (get) Token: 0x060085EE RID: 34286 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x060085EF RID: 34287 RVA: 0x000F7C0E File Offset: 0x000F5E0E
	public void Init(AxialI location)
	{
		this.m_location = location;
	}
}
