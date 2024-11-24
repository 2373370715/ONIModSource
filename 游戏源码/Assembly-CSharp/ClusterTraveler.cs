using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020018CC RID: 6348
public class ClusterTraveler : KMonoBehaviour, ISim200ms
{
	// Token: 0x17000880 RID: 2176
	// (get) Token: 0x060083A1 RID: 33697 RVA: 0x003405F4 File Offset: 0x0033E7F4
	public List<AxialI> CurrentPath
	{
		get
		{
			if (this.m_cachedPath == null || this.m_destinationSelector.GetDestination() != this.m_cachedPathDestination)
			{
				this.m_cachedPathDestination = this.m_destinationSelector.GetDestination();
				this.m_cachedPath = ClusterGrid.Instance.GetPath(this.m_clusterGridEntity.Location, this.m_cachedPathDestination, this.m_destinationSelector);
			}
			return this.m_cachedPath;
		}
	}

	// Token: 0x060083A2 RID: 33698 RVA: 0x000F67DF File Offset: 0x000F49DF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.ClusterTravelers.Add(this);
	}

	// Token: 0x060083A3 RID: 33699 RVA: 0x000F67F2 File Offset: 0x000F49F2
	protected override void OnCleanUp()
	{
		Components.ClusterTravelers.Remove(this);
		Game.Instance.Unsubscribe(-1991583975, new Action<object>(this.OnClusterFogOfWarRevealed));
		base.OnCleanUp();
	}

	// Token: 0x060083A4 RID: 33700 RVA: 0x000F6820 File Offset: 0x000F4A20
	private void ForceRevealLocation(AxialI location)
	{
		if (!ClusterGrid.Instance.IsCellVisible(location))
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(location, 0);
		}
	}

	// Token: 0x060083A5 RID: 33701 RVA: 0x00340660 File Offset: 0x0033E860
	protected override void OnSpawn()
	{
		base.Subscribe<ClusterTraveler>(543433792, ClusterTraveler.ClusterDestinationChangedHandler);
		Game.Instance.Subscribe(-1991583975, new Action<object>(this.OnClusterFogOfWarRevealed));
		this.UpdateAnimationTags();
		this.MarkPathDirty();
		this.RevalidatePath(false);
		if (this.revealsFogOfWarAsItTravels)
		{
			this.ForceRevealLocation(this.m_clusterGridEntity.Location);
		}
	}

	// Token: 0x060083A6 RID: 33702 RVA: 0x000F6840 File Offset: 0x000F4A40
	private void MarkPathDirty()
	{
		this.m_isPathDirty = true;
	}

	// Token: 0x060083A7 RID: 33703 RVA: 0x000F6849 File Offset: 0x000F4A49
	private void OnClusterFogOfWarRevealed(object data)
	{
		this.MarkPathDirty();
	}

	// Token: 0x060083A8 RID: 33704 RVA: 0x000F6851 File Offset: 0x000F4A51
	private void OnClusterDestinationChanged(object data)
	{
		if (this.m_destinationSelector.IsAtDestination())
		{
			this.m_movePotential = 0f;
			if (this.CurrentPath != null)
			{
				this.CurrentPath.Clear();
			}
		}
		this.MarkPathDirty();
	}

	// Token: 0x060083A9 RID: 33705 RVA: 0x000F6884 File Offset: 0x000F4A84
	public int GetDestinationWorldID()
	{
		return this.m_destinationSelector.GetDestinationWorld();
	}

	// Token: 0x060083AA RID: 33706 RVA: 0x000F6891 File Offset: 0x000F4A91
	public float TravelETA()
	{
		if (!this.IsTraveling() || this.getSpeedCB == null)
		{
			return 0f;
		}
		return this.RemainingTravelDistance() / this.getSpeedCB();
	}

	// Token: 0x060083AB RID: 33707 RVA: 0x003406C8 File Offset: 0x0033E8C8
	public float RemainingTravelDistance()
	{
		int num = this.RemainingTravelNodes();
		if (this.GetDestinationWorldID() >= 0)
		{
			num--;
			num = Mathf.Max(num, 0);
		}
		return (float)num * 600f - this.m_movePotential;
	}

	// Token: 0x060083AC RID: 33708 RVA: 0x00340700 File Offset: 0x0033E900
	public int RemainingTravelNodes()
	{
		if (this.CurrentPath == null)
		{
			return 0;
		}
		int count = this.CurrentPath.Count;
		return Mathf.Max(0, count);
	}

	// Token: 0x060083AD RID: 33709 RVA: 0x000F68BB File Offset: 0x000F4ABB
	public float GetMoveProgress()
	{
		return this.m_movePotential / 600f;
	}

	// Token: 0x060083AE RID: 33710 RVA: 0x000F68C9 File Offset: 0x000F4AC9
	public bool IsTraveling()
	{
		return !this.m_destinationSelector.IsAtDestination();
	}

	// Token: 0x060083AF RID: 33711 RVA: 0x0034072C File Offset: 0x0033E92C
	public void Sim200ms(float dt)
	{
		if (!this.IsTraveling())
		{
			return;
		}
		bool flag = this.CurrentPath != null && this.CurrentPath.Count > 0;
		bool flag2 = this.m_destinationSelector.HasAsteroidDestination();
		bool arg = flag2 && flag && this.CurrentPath.Count == 1;
		if (this.getCanTravelCB != null && !this.getCanTravelCB(arg))
		{
			return;
		}
		AxialI location = this.m_clusterGridEntity.Location;
		if (flag)
		{
			if (flag2)
			{
				bool requireLaunchPadOnAsteroidDestination = this.m_destinationSelector.requireLaunchPadOnAsteroidDestination;
			}
			if (!flag2 || this.CurrentPath.Count > 1 || !this.quickTravelToAsteroidIfInOrbit)
			{
				float num = dt * this.getSpeedCB();
				this.m_movePotential += num;
				if (this.m_movePotential >= 600f)
				{
					this.m_movePotential = 600f;
					if (this.AdvancePathOneStep())
					{
						global::Debug.Assert(ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(this.m_clusterGridEntity.Location, EntityLayer.Asteroid) == null || (flag2 && this.CurrentPath.Count == 0), string.Format("Somehow this clustercraft pathed through an asteroid at {0}", this.m_clusterGridEntity.Location));
						this.m_movePotential -= 600f;
						if (this.onTravelCB != null)
						{
							this.onTravelCB();
						}
					}
				}
			}
			else
			{
				this.AdvancePathOneStep();
			}
		}
		this.RevalidatePath(true);
	}

	// Token: 0x060083B0 RID: 33712 RVA: 0x003408A4 File Offset: 0x0033EAA4
	public bool AdvancePathOneStep()
	{
		if (this.validateTravelCB != null && !this.validateTravelCB(this.CurrentPath[0]))
		{
			return false;
		}
		AxialI location = this.CurrentPath[0];
		this.CurrentPath.RemoveAt(0);
		if (this.revealsFogOfWarAsItTravels)
		{
			this.ForceRevealLocation(location);
		}
		this.m_clusterGridEntity.Location = location;
		this.UpdateAnimationTags();
		return true;
	}

	// Token: 0x060083B1 RID: 33713 RVA: 0x00340910 File Offset: 0x0033EB10
	private void UpdateAnimationTags()
	{
		if (this.CurrentPath == null)
		{
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLaunching);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLanding);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityMoving);
			return;
		}
		if (!(ClusterGrid.Instance.GetAsteroidAtCell(this.m_clusterGridEntity.Location) != null))
		{
			this.m_clusterGridEntity.AddTag(GameTags.BallisticEntityMoving);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLanding);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLaunching);
			return;
		}
		if (this.CurrentPath.Count == 0 || this.m_clusterGridEntity.Location == this.CurrentPath[this.CurrentPath.Count - 1])
		{
			this.m_clusterGridEntity.AddTag(GameTags.BallisticEntityLanding);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLaunching);
			this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityMoving);
			return;
		}
		this.m_clusterGridEntity.AddTag(GameTags.BallisticEntityLaunching);
		this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityLanding);
		this.m_clusterGridEntity.RemoveTag(GameTags.BallisticEntityMoving);
	}

	// Token: 0x060083B2 RID: 33714 RVA: 0x00340A40 File Offset: 0x0033EC40
	public void RevalidatePath(bool react_to_change = true)
	{
		string reason;
		List<AxialI> cachedPath;
		if (this.HasCurrentPathChanged(out reason, out cachedPath))
		{
			if (this.stopAndNotifyWhenPathChanges && react_to_change)
			{
				this.m_destinationSelector.SetDestination(this.m_destinationSelector.GetMyWorldLocation());
				string message = MISC.NOTIFICATIONS.BADROCKETPATH.TOOLTIP;
				Notification notification = new Notification(MISC.NOTIFICATIONS.BADROCKETPATH.NAME, NotificationType.BadMinor, (List<Notification> notificationList, object data) => message + notificationList.ReduceMessages(false) + "\n\n" + reason, null, true, 0f, null, null, null, true, false, false);
				base.GetComponent<Notifier>().Add(notification, "");
				return;
			}
			this.m_cachedPath = cachedPath;
		}
	}

	// Token: 0x060083B3 RID: 33715 RVA: 0x00340AD8 File Offset: 0x0033ECD8
	private bool HasCurrentPathChanged(out string reason, out List<AxialI> updatedPath)
	{
		if (!this.m_isPathDirty)
		{
			reason = null;
			updatedPath = null;
			return false;
		}
		this.m_isPathDirty = false;
		updatedPath = ClusterGrid.Instance.GetPath(this.m_clusterGridEntity.Location, this.m_cachedPathDestination, this.m_destinationSelector, out reason, this.m_destinationSelector.dodgesHiddenAsteroids);
		if (updatedPath == null)
		{
			return true;
		}
		if (updatedPath.Count != this.m_cachedPath.Count)
		{
			return true;
		}
		for (int i = 0; i < this.m_cachedPath.Count; i++)
		{
			if (this.m_cachedPath[i] != updatedPath[i])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060083B4 RID: 33716 RVA: 0x000F68D9 File Offset: 0x000F4AD9
	[ContextMenu("Fill Move Potential")]
	public void FillMovePotential()
	{
		this.m_movePotential = 600f;
	}

	// Token: 0x040063CF RID: 25551
	[MyCmpReq]
	private ClusterDestinationSelector m_destinationSelector;

	// Token: 0x040063D0 RID: 25552
	[MyCmpReq]
	private ClusterGridEntity m_clusterGridEntity;

	// Token: 0x040063D1 RID: 25553
	[Serialize]
	private float m_movePotential;

	// Token: 0x040063D2 RID: 25554
	public Func<float> getSpeedCB;

	// Token: 0x040063D3 RID: 25555
	public Func<bool, bool> getCanTravelCB;

	// Token: 0x040063D4 RID: 25556
	public Func<AxialI, bool> validateTravelCB;

	// Token: 0x040063D5 RID: 25557
	public System.Action onTravelCB;

	// Token: 0x040063D6 RID: 25558
	private AxialI m_cachedPathDestination;

	// Token: 0x040063D7 RID: 25559
	private List<AxialI> m_cachedPath;

	// Token: 0x040063D8 RID: 25560
	private bool m_isPathDirty;

	// Token: 0x040063D9 RID: 25561
	public bool revealsFogOfWarAsItTravels = true;

	// Token: 0x040063DA RID: 25562
	public bool quickTravelToAsteroidIfInOrbit = true;

	// Token: 0x040063DB RID: 25563
	public bool stopAndNotifyWhenPathChanges;

	// Token: 0x040063DC RID: 25564
	private static EventSystem.IntraObjectHandler<ClusterTraveler> ClusterDestinationChangedHandler = new EventSystem.IntraObjectHandler<ClusterTraveler>(delegate(ClusterTraveler cmp, object data)
	{
		cmp.OnClusterDestinationChanged(data);
	});
}
