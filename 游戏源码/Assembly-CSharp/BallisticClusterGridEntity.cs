using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000437 RID: 1079
public class BallisticClusterGridEntity : ClusterGridEntity
{
	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06001261 RID: 4705 RVA: 0x000AE2D9 File Offset: 0x000AC4D9
	public override string Name
	{
		get
		{
			return Strings.Get(this.nameKey);
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06001262 RID: 4706 RVA: 0x000AD3A4 File Offset: 0x000AB5A4
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Payload;
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x06001263 RID: 4707 RVA: 0x00188C1C File Offset: 0x00186E1C
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim(this.clusterAnimName),
					initialAnim = "idle_loop",
					symbolSwapTarget = this.clusterAnimSymbolSwapTarget,
					symbolSwapSymbol = this.clusterAnimSymbolSwapSymbol
				}
			};
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06001264 RID: 4708 RVA: 0x000AE2EB File Offset: 0x000AC4EB
	public override bool IsVisible
	{
		get
		{
			return !base.gameObject.HasTag(GameTags.ClusterEntityGrounded);
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x06001265 RID: 4709 RVA: 0x000A6603 File Offset: 0x000A4803
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Visible;
		}
	}

	// Token: 0x06001266 RID: 4710 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool SpaceOutInSameHex()
	{
		return true;
	}

	// Token: 0x06001267 RID: 4711 RVA: 0x00188C7C File Offset: 0x00186E7C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.m_clusterTraveler.getSpeedCB = new Func<float>(this.GetSpeed);
		this.m_clusterTraveler.getCanTravelCB = new Func<bool, bool>(this.CanTravel);
		this.m_clusterTraveler.onTravelCB = null;
	}

	// Token: 0x06001268 RID: 4712 RVA: 0x000AE300 File Offset: 0x000AC500
	private float GetSpeed()
	{
		return 10f;
	}

	// Token: 0x06001269 RID: 4713 RVA: 0x000AE307 File Offset: 0x000AC507
	private bool CanTravel(bool tryingToLand)
	{
		return this.HasTag(GameTags.EntityInSpace);
	}

	// Token: 0x0600126A RID: 4714 RVA: 0x000AE314 File Offset: 0x000AC514
	public void Configure(AxialI source, AxialI destination)
	{
		this.m_location = source;
		this.m_destionationSelector.SetDestination(destination);
	}

	// Token: 0x0600126B RID: 4715 RVA: 0x000AE329 File Offset: 0x000AC529
	public override bool ShowPath()
	{
		return this.m_selectable.IsSelected;
	}

	// Token: 0x0600126C RID: 4716 RVA: 0x000AE336 File Offset: 0x000AC536
	public override bool ShowProgressBar()
	{
		return this.m_selectable.IsSelected && this.m_clusterTraveler.IsTraveling();
	}

	// Token: 0x0600126D RID: 4717 RVA: 0x000AE352 File Offset: 0x000AC552
	public override float GetProgress()
	{
		return this.m_clusterTraveler.GetMoveProgress();
	}

	// Token: 0x0600126E RID: 4718 RVA: 0x000AE35F File Offset: 0x000AC55F
	public void SwapSymbolFromSameAnim(string targetSymbolName, string swappedSymbolName)
	{
		this.clusterAnimSymbolSwapTarget = targetSymbolName;
		this.clusterAnimSymbolSwapSymbol = swappedSymbolName;
	}

	// Token: 0x04000C90 RID: 3216
	[MyCmpReq]
	private ClusterDestinationSelector m_destionationSelector;

	// Token: 0x04000C91 RID: 3217
	[MyCmpReq]
	private ClusterTraveler m_clusterTraveler;

	// Token: 0x04000C92 RID: 3218
	[SerializeField]
	public string clusterAnimName;

	// Token: 0x04000C93 RID: 3219
	[SerializeField]
	public StringKey nameKey;

	// Token: 0x04000C94 RID: 3220
	private string clusterAnimSymbolSwapTarget;

	// Token: 0x04000C95 RID: 3221
	private string clusterAnimSymbolSwapSymbol;
}
