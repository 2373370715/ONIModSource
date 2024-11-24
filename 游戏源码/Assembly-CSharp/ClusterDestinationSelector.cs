using System;
using KSerialization;

// Token: 0x020018C2 RID: 6338
public class ClusterDestinationSelector : KMonoBehaviour
{
	// Token: 0x06008355 RID: 33621 RVA: 0x000F64B4 File Offset: 0x000F46B4
	protected override void OnPrefabInit()
	{
		base.Subscribe<ClusterDestinationSelector>(-1298331547, this.OnClusterLocationChangedDelegate);
	}

	// Token: 0x06008356 RID: 33622 RVA: 0x000F64C8 File Offset: 0x000F46C8
	protected virtual void OnClusterLocationChanged(object data)
	{
		if (((ClusterLocationChangedEvent)data).newLocation == this.m_destination)
		{
			base.Trigger(1796608350, data);
		}
	}

	// Token: 0x06008357 RID: 33623 RVA: 0x000F64EE File Offset: 0x000F46EE
	public int GetDestinationWorld()
	{
		return ClusterUtil.GetAsteroidWorldIdAtLocation(this.m_destination);
	}

	// Token: 0x06008358 RID: 33624 RVA: 0x000F64FB File Offset: 0x000F46FB
	public AxialI GetDestination()
	{
		return this.m_destination;
	}

	// Token: 0x06008359 RID: 33625 RVA: 0x0033F96C File Offset: 0x0033DB6C
	public virtual void SetDestination(AxialI location)
	{
		if (this.requireAsteroidDestination)
		{
			Debug.Assert(ClusterUtil.GetAsteroidWorldIdAtLocation(location) != -1, string.Format("Cannot SetDestination to {0} as there is no world there", location));
		}
		this.m_destination = location;
		base.Trigger(543433792, location);
	}

	// Token: 0x0600835A RID: 33626 RVA: 0x000F6503 File Offset: 0x000F4703
	public bool HasAsteroidDestination()
	{
		return ClusterUtil.GetAsteroidWorldIdAtLocation(this.m_destination) != -1;
	}

	// Token: 0x0600835B RID: 33627 RVA: 0x000F6516 File Offset: 0x000F4716
	public virtual bool IsAtDestination()
	{
		return this.GetMyWorldLocation() == this.m_destination;
	}

	// Token: 0x040063A3 RID: 25507
	[Serialize]
	protected AxialI m_destination;

	// Token: 0x040063A4 RID: 25508
	public bool assignable;

	// Token: 0x040063A5 RID: 25509
	public bool requireAsteroidDestination;

	// Token: 0x040063A6 RID: 25510
	[Serialize]
	public bool canNavigateFogOfWar;

	// Token: 0x040063A7 RID: 25511
	public bool dodgesHiddenAsteroids;

	// Token: 0x040063A8 RID: 25512
	public bool requireLaunchPadOnAsteroidDestination;

	// Token: 0x040063A9 RID: 25513
	public bool shouldPointTowardsPath;

	// Token: 0x040063AA RID: 25514
	private EventSystem.IntraObjectHandler<ClusterDestinationSelector> OnClusterLocationChangedDelegate = new EventSystem.IntraObjectHandler<ClusterDestinationSelector>(delegate(ClusterDestinationSelector cmp, object data)
	{
		cmp.OnClusterLocationChanged(data);
	});
}
