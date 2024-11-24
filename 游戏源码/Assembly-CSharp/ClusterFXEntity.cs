using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020018C4 RID: 6340
[SerializationConfig(MemberSerialization.OptIn)]
public class ClusterFXEntity : ClusterGridEntity
{
	// Token: 0x1700086E RID: 2158
	// (get) Token: 0x06008360 RID: 33632 RVA: 0x000F6570 File Offset: 0x000F4770
	public override string Name
	{
		get
		{
			return UI.SPACEDESTINATIONS.TELESCOPE_TARGET.NAME;
		}
	}

	// Token: 0x1700086F RID: 2159
	// (get) Token: 0x06008361 RID: 33633 RVA: 0x000AD3F5 File Offset: 0x000AB5F5
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.FX;
		}
	}

	// Token: 0x17000870 RID: 2160
	// (get) Token: 0x06008362 RID: 33634 RVA: 0x0033F9BC File Offset: 0x0033DBBC
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim(this.kAnimName),
					initialAnim = this.animName,
					playMode = this.animPlayMode,
					animOffset = this.animOffset
				}
			};
		}
	}

	// Token: 0x17000871 RID: 2161
	// (get) Token: 0x06008363 RID: 33635 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000872 RID: 2162
	// (get) Token: 0x06008364 RID: 33636 RVA: 0x000A6603 File Offset: 0x000A4803
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x06008365 RID: 33637 RVA: 0x000F657C File Offset: 0x000F477C
	public void Init(AxialI location, Vector3 animOffset)
	{
		base.Location = location;
		this.animOffset = animOffset;
	}

	// Token: 0x040063AD RID: 25517
	[SerializeField]
	public string kAnimName;

	// Token: 0x040063AE RID: 25518
	[SerializeField]
	public string animName;

	// Token: 0x040063AF RID: 25519
	public KAnim.PlayMode animPlayMode = KAnim.PlayMode.Once;

	// Token: 0x040063B0 RID: 25520
	public Vector3 animOffset;
}
