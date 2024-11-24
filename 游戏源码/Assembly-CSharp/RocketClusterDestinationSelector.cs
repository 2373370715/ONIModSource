using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02001924 RID: 6436
public class RocketClusterDestinationSelector : ClusterDestinationSelector
{
	// Token: 0x170008DB RID: 2267
	// (get) Token: 0x06008617 RID: 34327 RVA: 0x000F7E15 File Offset: 0x000F6015
	// (set) Token: 0x06008618 RID: 34328 RVA: 0x000F7E1D File Offset: 0x000F601D
	public bool Repeat
	{
		get
		{
			return this.m_repeat;
		}
		set
		{
			this.m_repeat = value;
		}
	}

	// Token: 0x06008619 RID: 34329 RVA: 0x000F7E26 File Offset: 0x000F6026
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<RocketClusterDestinationSelector>(-1277991738, this.OnLaunchDelegate);
	}

	// Token: 0x0600861A RID: 34330 RVA: 0x000F7E40 File Offset: 0x000F6040
	protected override void OnSpawn()
	{
		if (this.isHarvesting)
		{
			this.WaitForPOIHarvest();
		}
	}

	// Token: 0x0600861B RID: 34331 RVA: 0x0034B178 File Offset: 0x00349378
	public LaunchPad GetDestinationPad(AxialI destination)
	{
		int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(destination);
		if (this.m_launchPad.ContainsKey(asteroidWorldIdAtLocation))
		{
			return this.m_launchPad[asteroidWorldIdAtLocation].Get();
		}
		return null;
	}

	// Token: 0x0600861C RID: 34332 RVA: 0x000F7E50 File Offset: 0x000F6050
	public LaunchPad GetDestinationPad()
	{
		return this.GetDestinationPad(this.m_destination);
	}

	// Token: 0x0600861D RID: 34333 RVA: 0x000F7E5E File Offset: 0x000F605E
	public override void SetDestination(AxialI location)
	{
		base.SetDestination(location);
	}

	// Token: 0x0600861E RID: 34334 RVA: 0x0034B1B0 File Offset: 0x003493B0
	public void SetDestinationPad(LaunchPad pad)
	{
		Debug.Assert(pad == null || ClusterGrid.Instance.IsInRange(pad.GetMyWorldLocation(), this.m_destination, 1), "Tried sending a rocket to a launchpad that wasn't its destination world.");
		if (pad != null)
		{
			this.AddDestinationPad(pad.GetMyWorldLocation(), pad);
			base.SetDestination(pad.GetMyWorldLocation());
		}
		base.GetComponent<CraftModuleInterface>().TriggerEventOnCraftAndRocket(GameHashes.ClusterDestinationChanged, null);
	}

	// Token: 0x0600861F RID: 34335 RVA: 0x0034B220 File Offset: 0x00349420
	private void AddDestinationPad(AxialI location, LaunchPad pad)
	{
		int asteroidWorldIdAtLocation = ClusterUtil.GetAsteroidWorldIdAtLocation(location);
		if (asteroidWorldIdAtLocation < 0)
		{
			return;
		}
		if (!this.m_launchPad.ContainsKey(asteroidWorldIdAtLocation))
		{
			this.m_launchPad.Add(asteroidWorldIdAtLocation, new Ref<LaunchPad>());
		}
		this.m_launchPad[asteroidWorldIdAtLocation].Set(pad);
	}

	// Token: 0x06008620 RID: 34336 RVA: 0x0034B26C File Offset: 0x0034946C
	protected override void OnClusterLocationChanged(object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		if (clusterLocationChangedEvent.newLocation == this.m_destination)
		{
			base.GetComponent<CraftModuleInterface>().TriggerEventOnCraftAndRocket(GameHashes.ClusterDestinationReached, null);
			if (this.m_repeat)
			{
				if (ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(clusterLocationChangedEvent.newLocation, EntityLayer.POI) != null && this.CanRocketHarvest())
				{
					this.WaitForPOIHarvest();
					return;
				}
				this.SetUpReturnTrip();
			}
		}
	}

	// Token: 0x06008621 RID: 34337 RVA: 0x0034B2E0 File Offset: 0x003494E0
	private void SetUpReturnTrip()
	{
		this.AddDestinationPad(this.m_prevDestination, this.m_prevLaunchPad.Get());
		this.m_destination = this.m_prevDestination;
		this.m_prevDestination = base.GetComponent<Clustercraft>().Location;
		this.m_prevLaunchPad.Set(base.GetComponent<CraftModuleInterface>().CurrentPad);
	}

	// Token: 0x06008622 RID: 34338 RVA: 0x0034B338 File Offset: 0x00349538
	private bool CanRocketHarvest()
	{
		bool flag = false;
		List<ResourceHarvestModule.StatesInstance> allResourceHarvestModules = base.GetComponent<Clustercraft>().GetAllResourceHarvestModules();
		if (allResourceHarvestModules.Count > 0)
		{
			using (List<ResourceHarvestModule.StatesInstance>.Enumerator enumerator = allResourceHarvestModules.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CheckIfCanHarvest())
					{
						flag = true;
					}
				}
			}
		}
		if (!flag)
		{
			List<ArtifactHarvestModule.StatesInstance> allArtifactHarvestModules = base.GetComponent<Clustercraft>().GetAllArtifactHarvestModules();
			if (allArtifactHarvestModules.Count > 0)
			{
				using (List<ArtifactHarvestModule.StatesInstance>.Enumerator enumerator2 = allArtifactHarvestModules.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.CheckIfCanHarvest())
						{
							flag = true;
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x06008623 RID: 34339 RVA: 0x0034B3F8 File Offset: 0x003495F8
	private void OnStorageChange(object data)
	{
		if (!this.CanRocketHarvest())
		{
			this.isHarvesting = false;
			foreach (Ref<RocketModuleCluster> @ref in base.GetComponent<Clustercraft>().ModuleInterface.ClusterModules)
			{
				if (@ref.Get().GetComponent<Storage>())
				{
					base.Unsubscribe(@ref.Get().gameObject, -1697596308, new Action<object>(this.OnStorageChange));
				}
			}
			this.SetUpReturnTrip();
		}
	}

	// Token: 0x06008624 RID: 34340 RVA: 0x0034B494 File Offset: 0x00349694
	private void WaitForPOIHarvest()
	{
		this.isHarvesting = true;
		foreach (Ref<RocketModuleCluster> @ref in base.GetComponent<Clustercraft>().ModuleInterface.ClusterModules)
		{
			if (@ref.Get().GetComponent<Storage>())
			{
				base.Subscribe(@ref.Get().gameObject, -1697596308, new Action<object>(this.OnStorageChange));
			}
		}
	}

	// Token: 0x06008625 RID: 34341 RVA: 0x0034B520 File Offset: 0x00349720
	private void OnLaunch(object data)
	{
		CraftModuleInterface component = base.GetComponent<CraftModuleInterface>();
		this.m_prevLaunchPad.Set(component.CurrentPad);
		Clustercraft component2 = base.GetComponent<Clustercraft>();
		this.m_prevDestination = component2.Location;
	}

	// Token: 0x04006538 RID: 25912
	[Serialize]
	private Dictionary<int, Ref<LaunchPad>> m_launchPad = new Dictionary<int, Ref<LaunchPad>>();

	// Token: 0x04006539 RID: 25913
	[Serialize]
	private bool m_repeat;

	// Token: 0x0400653A RID: 25914
	[Serialize]
	private AxialI m_prevDestination;

	// Token: 0x0400653B RID: 25915
	[Serialize]
	private Ref<LaunchPad> m_prevLaunchPad = new Ref<LaunchPad>();

	// Token: 0x0400653C RID: 25916
	[Serialize]
	private bool isHarvesting;

	// Token: 0x0400653D RID: 25917
	private EventSystem.IntraObjectHandler<RocketClusterDestinationSelector> OnLaunchDelegate = new EventSystem.IntraObjectHandler<RocketClusterDestinationSelector>(delegate(RocketClusterDestinationSelector cmp, object data)
	{
		cmp.OnLaunch(data);
	});
}
