using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;

// Token: 0x02001946 RID: 6470
[SerializationConfig(MemberSerialization.OptIn)]
public class TelescopeTarget : ClusterGridEntity
{
	// Token: 0x170008E5 RID: 2277
	// (get) Token: 0x060086EA RID: 34538 RVA: 0x000F6570 File Offset: 0x000F4770
	public override string Name
	{
		get
		{
			return UI.SPACEDESTINATIONS.TELESCOPE_TARGET.NAME;
		}
	}

	// Token: 0x170008E6 RID: 2278
	// (get) Token: 0x060086EB RID: 34539 RVA: 0x000AD365 File Offset: 0x000AB565
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Telescope;
		}
	}

	// Token: 0x170008E7 RID: 2279
	// (get) Token: 0x060086EC RID: 34540 RVA: 0x0034F340 File Offset: 0x0034D540
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("telescope_target_kanim"),
					initialAnim = "idle"
				}
			};
		}
	}

	// Token: 0x170008E8 RID: 2280
	// (get) Token: 0x060086ED RID: 34541 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170008E9 RID: 2281
	// (get) Token: 0x060086EE RID: 34542 RVA: 0x000A6603 File Offset: 0x000A4803
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x060086EF RID: 34543 RVA: 0x000F5F82 File Offset: 0x000F4182
	public void Init(AxialI location)
	{
		base.Location = location;
	}

	// Token: 0x060086F0 RID: 34544 RVA: 0x000F8582 File Offset: 0x000F6782
	public void SetTargetMeteorShower(ClusterMapMeteorShower.Instance meteorShower)
	{
		this.targetMeteorShower = meteorShower;
	}

	// Token: 0x060086F1 RID: 34545 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool ShowName()
	{
		return true;
	}

	// Token: 0x060086F2 RID: 34546 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool ShowProgressBar()
	{
		return true;
	}

	// Token: 0x060086F3 RID: 34547 RVA: 0x000F858B File Offset: 0x000F678B
	public override float GetProgress()
	{
		if (this.targetMeteorShower != null)
		{
			return this.targetMeteorShower.IdentifyingProgress;
		}
		return SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().GetRevealCompleteFraction(base.Location);
	}

	// Token: 0x040065DA RID: 26074
	private ClusterMapMeteorShower.Instance targetMeteorShower;
}
