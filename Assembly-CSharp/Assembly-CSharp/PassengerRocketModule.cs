﻿using System;
using System.Collections.Generic;
using FMODUnity;
using KSerialization;
using UnityEngine;

public class PassengerRocketModule : KMonoBehaviour
{
			public PassengerRocketModule.RequestCrewState PassengersRequested
	{
		get
		{
			return this.passengersRequested;
		}
	}

		protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.Subscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
		GameUtil.SubscribeToTags<PassengerRocketModule>(this, PassengerRocketModule.OnRocketOnGroundTagDelegate, false);
		base.Subscribe<PassengerRocketModule>(-1547247383, PassengerRocketModule.OnClustercraftStateChanged);
		base.Subscribe<PassengerRocketModule>(1655598572, PassengerRocketModule.RefreshDelegate);
		base.Subscribe<PassengerRocketModule>(191901966, PassengerRocketModule.RefreshDelegate);
		base.Subscribe<PassengerRocketModule>(-71801987, PassengerRocketModule.RefreshDelegate);
		base.Subscribe<PassengerRocketModule>(-1277991738, PassengerRocketModule.OnLaunchDelegate);
		base.Subscribe<PassengerRocketModule>(-1432940121, PassengerRocketModule.OnReachableChangedDelegate);
		new ReachabilityMonitor.Instance(base.GetComponent<Workable>()).StartSM();
	}

		protected override void OnCleanUp()
	{
		Game.Instance.Unsubscribe(-1123234494, new Action<object>(this.OnAssignmentGroupChanged));
		base.OnCleanUp();
	}

		private void OnAssignmentGroupChanged(object data)
	{
		this.RefreshOrders();
	}

		private void RefreshClusterStateForAudio()
	{
		if (ClusterManager.Instance != null)
		{
			WorldContainer activeWorld = ClusterManager.Instance.activeWorld;
			if (activeWorld != null && activeWorld.IsModuleInterior)
			{
				UnityEngine.Object craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
				Clustercraft component = activeWorld.GetComponent<Clustercraft>();
				if (craftInterface == component.ModuleInterface)
				{
					ClusterManager.Instance.UpdateRocketInteriorAudio();
				}
			}
		}
	}

		private void OnReachableChanged(object data)
	{
		bool flag = (bool)data;
		KSelectable component = base.GetComponent<KSelectable>();
		if (flag)
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.PassengerModuleUnreachable, false);
			return;
		}
		component.AddStatusItem(Db.Get().BuildingStatusItems.PassengerModuleUnreachable, this);
	}

		public void RequestCrewBoard(PassengerRocketModule.RequestCrewState requestBoard)
	{
		this.passengersRequested = requestBoard;
		this.RefreshOrders();
	}

		public bool ShouldCrewGetIn()
	{
		CraftModuleInterface craftInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		return this.passengersRequested == PassengerRocketModule.RequestCrewState.Request || (craftInterface.IsLaunchRequested() && craftInterface.CheckPreppedForLaunch());
	}

		private void RefreshOrders()
	{
		if (!this.HasTag(GameTags.RocketOnGround) || !base.GetComponent<ClustercraftExteriorDoor>().HasTargetWorld())
		{
			return;
		}
		int cell = base.GetComponent<NavTeleporter>().GetCell();
		int num = base.GetComponent<ClustercraftExteriorDoor>().TargetCell();
		bool flag = this.ShouldCrewGetIn();
		if (flag)
		{
			using (List<MinionIdentity>.Enumerator enumerator = Components.LiveMinionIdentities.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MinionIdentity minionIdentity = enumerator.Current;
					bool flag2 = Game.Instance.assignmentManager.assignment_groups[base.GetComponent<AssignmentGroupController>().AssignmentGroupID].HasMember(minionIdentity.assignableProxy.Get());
					bool flag3 = minionIdentity.GetMyWorldId() == (int)Grid.WorldIdx[num];
					if (!flag3 && flag2)
					{
						minionIdentity.GetSMI<RocketPassengerMonitor.Instance>().SetMoveTarget(num);
					}
					else if (flag3 && !flag2)
					{
						minionIdentity.GetSMI<RocketPassengerMonitor.Instance>().SetMoveTarget(cell);
					}
					else
					{
						minionIdentity.GetSMI<RocketPassengerMonitor.Instance>().ClearMoveTarget(num);
					}
				}
				goto IL_148;
			}
		}
		foreach (MinionIdentity cmp in Components.LiveMinionIdentities.Items)
		{
			RocketPassengerMonitor.Instance smi = cmp.GetSMI<RocketPassengerMonitor.Instance>();
			if (smi != null)
			{
				smi.ClearMoveTarget(cell);
				smi.ClearMoveTarget(num);
			}
		}
		IL_148:
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			this.RefreshAccessStatus(Components.LiveMinionIdentities[i], flag);
		}
	}

		private void RefreshAccessStatus(MinionIdentity minion, bool restrict)
	{
		Component interiorDoor = base.GetComponent<ClustercraftExteriorDoor>().GetInteriorDoor();
		AccessControl component = base.GetComponent<AccessControl>();
		AccessControl component2 = interiorDoor.GetComponent<AccessControl>();
		if (!restrict)
		{
			component.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
			component2.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
			return;
		}
		if (Game.Instance.assignmentManager.assignment_groups[base.GetComponent<AssignmentGroupController>().AssignmentGroupID].HasMember(minion.assignableProxy.Get()))
		{
			component.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
			component2.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Neither);
			return;
		}
		component.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Neither);
		component2.SetPermission(minion.assignableProxy.Get(), AccessControl.Permission.Both);
	}

		public bool CheckPilotBoarded()
	{
		ICollection<IAssignableIdentity> members = base.GetComponent<AssignmentGroupController>().GetMembers();
		if (members.Count == 0)
		{
			return false;
		}
		List<IAssignableIdentity> list = new List<IAssignableIdentity>();
		foreach (IAssignableIdentity assignableIdentity in members)
		{
			MinionAssignablesProxy minionAssignablesProxy = (MinionAssignablesProxy)assignableIdentity;
			if (minionAssignablesProxy != null)
			{
				MinionResume component = minionAssignablesProxy.GetTargetGameObject().GetComponent<MinionResume>();
				if (component != null && component.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
				{
					list.Add(assignableIdentity);
				}
			}
		}
		if (list.Count == 0)
		{
			return false;
		}
		using (List<IAssignableIdentity>.Enumerator enumerator2 = list.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				if (((MinionAssignablesProxy)enumerator2.Current).GetTargetGameObject().GetMyWorldId() == (int)Grid.WorldIdx[base.GetComponent<ClustercraftExteriorDoor>().TargetCell()])
				{
					return true;
				}
			}
		}
		return false;
	}

		public global::Tuple<int, int> GetCrewBoardedFraction()
	{
		ICollection<IAssignableIdentity> members = base.GetComponent<AssignmentGroupController>().GetMembers();
		if (members.Count == 0)
		{
			return new global::Tuple<int, int>(0, 0);
		}
		int num = 0;
		using (IEnumerator<IAssignableIdentity> enumerator = members.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (((MinionAssignablesProxy)enumerator.Current).GetTargetGameObject().GetMyWorldId() != (int)Grid.WorldIdx[base.GetComponent<ClustercraftExteriorDoor>().TargetCell()])
				{
					num++;
				}
			}
		}
		return new global::Tuple<int, int>(members.Count - num, members.Count);
	}

		public bool HasCrewAssigned()
	{
		return ((ICollection<IAssignableIdentity>)base.GetComponent<AssignmentGroupController>().GetMembers()).Count > 0;
	}

		public bool CheckPassengersBoarded(bool require_pilot = true)
	{
		ICollection<IAssignableIdentity> members = base.GetComponent<AssignmentGroupController>().GetMembers();
		if (members.Count == 0)
		{
			return false;
		}
		if (require_pilot)
		{
			bool flag = false;
			foreach (IAssignableIdentity assignableIdentity in members)
			{
				MinionAssignablesProxy minionAssignablesProxy = (MinionAssignablesProxy)assignableIdentity;
				if (minionAssignablesProxy != null)
				{
					MinionResume component = minionAssignablesProxy.GetTargetGameObject().GetComponent<MinionResume>();
					if (component != null && component.HasPerk(Db.Get().SkillPerks.CanUseRocketControlStation))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		using (IEnumerator<IAssignableIdentity> enumerator = members.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (((MinionAssignablesProxy)enumerator.Current).GetTargetGameObject().GetMyWorldId() != (int)Grid.WorldIdx[base.GetComponent<ClustercraftExteriorDoor>().TargetCell()])
				{
					return false;
				}
			}
		}
		return true;
	}

		public bool CheckExtraPassengers()
	{
		ClustercraftExteriorDoor component = base.GetComponent<ClustercraftExteriorDoor>();
		if (component.HasTargetWorld())
		{
			byte worldId = Grid.WorldIdx[component.TargetCell()];
			List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems((int)worldId, false);
			string assignmentGroupID = base.GetComponent<AssignmentGroupController>().AssignmentGroupID;
			for (int i = 0; i < worldItems.Count; i++)
			{
				if (!Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].HasMember(worldItems[i].assignableProxy.Get()))
				{
					return true;
				}
			}
		}
		return false;
	}

		public void RemoveRocketPassenger(MinionIdentity minion)
	{
		if (minion != null)
		{
			string assignmentGroupID = base.GetComponent<AssignmentGroupController>().AssignmentGroupID;
			MinionAssignablesProxy member = minion.assignableProxy.Get();
			if (Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].HasMember(member))
			{
				Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].RemoveMember(member);
			}
			this.RefreshOrders();
		}
	}

		public void RemovePassengersOnOtherWorlds()
	{
		ClustercraftExteriorDoor component = base.GetComponent<ClustercraftExteriorDoor>();
		if (component.HasTargetWorld())
		{
			int myWorldId = component.GetMyWorldId();
			string assignmentGroupID = base.GetComponent<AssignmentGroupController>().AssignmentGroupID;
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
			{
				MinionAssignablesProxy member = minionIdentity.assignableProxy.Get();
				if (Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].HasMember(member) && minionIdentity.GetMyParentWorldId() != myWorldId)
				{
					Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].RemoveMember(member);
				}
			}
		}
	}

		public void ClearMinionAssignments(object data)
	{
		string assignmentGroupID = base.GetComponent<AssignmentGroupController>().AssignmentGroupID;
		foreach (IAssignableIdentity minionIdentity in Game.Instance.assignmentManager.assignment_groups[assignmentGroupID].GetMembers())
		{
			Game.Instance.assignmentManager.RemoveFromWorld(minionIdentity, this.GetMyWorldId());
		}
	}

		public EventReference interiorReverbSnapshot;

		[Serialize]
	private PassengerRocketModule.RequestCrewState passengersRequested;

		private static readonly EventSystem.IntraObjectHandler<PassengerRocketModule> OnRocketOnGroundTagDelegate = GameUtil.CreateHasTagHandler<PassengerRocketModule>(GameTags.RocketOnGround, delegate(PassengerRocketModule component, object data)
	{
		component.RequestCrewBoard(PassengerRocketModule.RequestCrewState.Release);
	});

		private static readonly EventSystem.IntraObjectHandler<PassengerRocketModule> OnClustercraftStateChanged = new EventSystem.IntraObjectHandler<PassengerRocketModule>(delegate(PassengerRocketModule cmp, object data)
	{
		cmp.RefreshClusterStateForAudio();
	});

		private static EventSystem.IntraObjectHandler<PassengerRocketModule> RefreshDelegate = new EventSystem.IntraObjectHandler<PassengerRocketModule>(delegate(PassengerRocketModule cmp, object data)
	{
		cmp.RefreshOrders();
		cmp.RefreshClusterStateForAudio();
	});

		private static EventSystem.IntraObjectHandler<PassengerRocketModule> OnLaunchDelegate = new EventSystem.IntraObjectHandler<PassengerRocketModule>(delegate(PassengerRocketModule component, object data)
	{
		component.ClearMinionAssignments(data);
	});

		private static readonly EventSystem.IntraObjectHandler<PassengerRocketModule> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<PassengerRocketModule>(delegate(PassengerRocketModule component, object data)
	{
		component.OnReachableChanged(data);
	});

		public enum RequestCrewState
	{
				Release,
				Request
	}
}
