﻿using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class ChorePreconditions
{
		public static ChorePreconditions instance
	{
		get
		{
			if (ChorePreconditions._instance == null)
			{
				ChorePreconditions._instance = new ChorePreconditions();
			}
			return ChorePreconditions._instance;
		}
	}

	public static void DestroyInstance()
	{
		ChorePreconditions._instance = null;
	}

	public ChorePreconditions()
	{
		Chore.Precondition precondition = default(Chore.Precondition);
		precondition.id = "IsPreemptable";
		precondition.sortOrder = 1;
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_PREEMPTABLE;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return context.isAttemptingOverride || context.chore.CanPreempt(context) || context.chore.driver == null;
		};
		this.IsPreemptable = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "HasUrge";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_URGE;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.chore.choreType.urge == null)
			{
				return true;
			}
			foreach (Urge urge in context.consumerState.consumer.GetUrges())
			{
				if (context.chore.SatisfiesUrge(urge))
				{
					return true;
				}
			}
			return false;
		};
		this.HasUrge = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsValid";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_VALID;
		precondition.sortOrder = -4;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return !context.chore.isNull && context.chore.IsValid();
		};
		this.IsValid = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsPermitted";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_PERMITTED;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return context.consumerState.consumer.IsPermittedOrEnabled(context.choreTypeForPermission, context.chore);
		};
		this.IsPermitted = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsAssignedToMe";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_ASSIGNED_TO_ME;
		precondition.sortOrder = 10;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Assignable assignable = (Assignable)data;
			IAssignableIdentity component = context.consumerState.gameObject.GetComponent<IAssignableIdentity>();
			return component != null && assignable.IsAssignedTo(component);
		};
		this.IsAssignedtoMe = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsInMyRoom";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_IN_MY_ROOM;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			int cell = (int)data;
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			Room room = null;
			if (cavityForCell != null)
			{
				room = cavityForCell.room;
			}
			if (room != null)
			{
				if (context.consumerState.ownable != null)
				{
					using (List<Ownables>.Enumerator enumerator = room.GetOwners().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.gameObject == context.consumerState.gameObject)
							{
								return true;
							}
						}
						return false;
					}
				}
				Room room2 = null;
				FetchChore fetchChore = context.chore as FetchChore;
				if (fetchChore != null && fetchChore.destination != null)
				{
					CavityInfo cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(fetchChore.destination));
					if (cavityForCell2 != null)
					{
						room2 = cavityForCell2.room;
					}
					return room2 != null && room2 == room;
				}
				if (context.chore is WorkChore<Tinkerable>)
				{
					CavityInfo cavityForCell3 = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell((context.chore as WorkChore<Tinkerable>).gameObject));
					if (cavityForCell3 != null)
					{
						room2 = cavityForCell3.room;
					}
					return room2 != null && room2 == room;
				}
				return false;
			}
			return false;
		};
		this.IsInMyRoom = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsPreferredAssignable";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_PREFERRED_ASSIGNABLE;
		precondition.sortOrder = 10;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Assignable assignable = (Assignable)data;
			return Game.Instance.assignmentManager.GetPreferredAssignables(context.consumerState.assignables, assignable.slot).Contains(assignable);
		};
		this.IsPreferredAssignable = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsPreferredAssignableOrUrgent";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_PREFERRED_ASSIGNABLE_OR_URGENT_BLADDER;
		precondition.sortOrder = 10;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Assignable candidate = (Assignable)data;
			if (Game.Instance.assignmentManager.IsPreferredAssignable(context.consumerState.assignables, candidate))
			{
				return true;
			}
			PeeChoreMonitor.Instance smi = context.consumerState.gameObject.GetSMI<PeeChoreMonitor.Instance>();
			return smi != null && smi.IsInsideState(smi.sm.critical);
		};
		this.IsPreferredAssignableOrUrgentBladder = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsNotTransferArm";
		precondition.description = "";
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return !context.consumerState.hasSolidTransferArm;
		};
		this.IsNotTransferArm = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "HasSkillPerk";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_SKILL_PERK;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			MinionResume resume = context.consumerState.resume;
			if (!resume)
			{
				return false;
			}
			if (data is SkillPerk)
			{
				SkillPerk perk = data as SkillPerk;
				return resume.HasPerk(perk);
			}
			if (data is HashedString)
			{
				HashedString perkId = (HashedString)data;
				return resume.HasPerk(perkId);
			}
			if (data is string)
			{
				HashedString perkId2 = (string)data;
				return resume.HasPerk(perkId2);
			}
			return false;
		};
		this.HasSkillPerk = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsMinion";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MINION;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return context.consumerState.resume != null;
		};
		this.IsMinion = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsMoreSatisfyingEarly";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MORE_SATISFYING;
		precondition.sortOrder = -2;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.isAttemptingOverride)
			{
				return true;
			}
			if (context.skipMoreSatisfyingEarlyPrecondition)
			{
				return true;
			}
			if (context.consumerState.selectable.IsSelected)
			{
				return true;
			}
			Chore currentChore = context.consumerState.choreDriver.GetCurrentChore();
			if (currentChore == null)
			{
				return true;
			}
			if (context.masterPriority.priority_class != currentChore.masterPriority.priority_class)
			{
				return context.masterPriority.priority_class > currentChore.masterPriority.priority_class;
			}
			if (context.consumerState.consumer != null && context.personalPriority != context.consumerState.consumer.GetPersonalPriority(currentChore.choreType))
			{
				return context.personalPriority > context.consumerState.consumer.GetPersonalPriority(currentChore.choreType);
			}
			if (context.masterPriority.priority_value != currentChore.masterPriority.priority_value)
			{
				return context.masterPriority.priority_value > currentChore.masterPriority.priority_value;
			}
			return context.priority > currentChore.choreType.priority;
		};
		this.IsMoreSatisfyingEarly = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsMoreSatisfyingLate";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MORE_SATISFYING;
		precondition.sortOrder = 10000;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.isAttemptingOverride)
			{
				return true;
			}
			if (!context.consumerState.selectable.IsSelected)
			{
				return true;
			}
			Chore currentChore = context.consumerState.choreDriver.GetCurrentChore();
			if (currentChore == null)
			{
				return true;
			}
			if (context.masterPriority.priority_class != currentChore.masterPriority.priority_class)
			{
				return context.masterPriority.priority_class > currentChore.masterPriority.priority_class;
			}
			if (context.consumerState.consumer != null && context.personalPriority != context.consumerState.consumer.GetPersonalPriority(currentChore.choreType))
			{
				return context.personalPriority > context.consumerState.consumer.GetPersonalPriority(currentChore.choreType);
			}
			if (context.masterPriority.priority_value != currentChore.masterPriority.priority_value)
			{
				return context.masterPriority.priority_value > currentChore.masterPriority.priority_value;
			}
			return context.priority > currentChore.choreType.priority;
		};
		this.IsMoreSatisfyingLate = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "CanChat";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_CHAT;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)data;
			return !(context.consumerState.consumer == null) && !(context.consumerState.navigator == null) && !(kmonoBehaviour == null) && context.consumerState.navigator.CanReach(Grid.PosToCell(kmonoBehaviour));
		};
		this.IsChattable = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsNotRedAlert";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_NOT_RED_ALERT;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return context.chore.masterPriority.priority_class == PriorityScreen.PriorityClass.topPriority || !context.chore.gameObject.GetMyWorld().IsRedAlert();
		};
		this.IsNotRedAlert = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsScheduledTime";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_SCHEDULED_TIME;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.chore.gameObject.GetMyWorld().IsRedAlert())
			{
				return true;
			}
			ScheduleBlockType type = (ScheduleBlockType)data;
			ScheduleBlock scheduleBlock = context.consumerState.scheduleBlock;
			return scheduleBlock == null || scheduleBlock.IsAllowed(type);
		};
		this.IsScheduledTime = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "CanMoveTo";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.consumerState.consumer == null)
			{
				return false;
			}
			KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)data;
			if (kmonoBehaviour == null)
			{
				return false;
			}
			IApproachable approachable = (IApproachable)kmonoBehaviour;
			int num;
			if (context.consumerState.consumer.GetNavigationCost(approachable, out num))
			{
				context.cost += num;
				return true;
			}
			return false;
		};
		this.CanMoveTo = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "CanMoveToCell";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.consumerState.consumer == null)
			{
				return false;
			}
			int cell = (int)data;
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			int num;
			if (context.consumerState.consumer.GetNavigationCost(cell, out num))
			{
				context.cost += num;
				return true;
			}
			return false;
		};
		this.CanMoveToCell = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "CanMoveTo";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.consumerState.consumer == null)
			{
				return false;
			}
			Func<int> func = (Func<int>)data;
			if (func == null)
			{
				return false;
			}
			int cell = func();
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			int num;
			if (context.consumerState.consumer.GetNavigationCost(cell, out num))
			{
				context.cost += num;
				return true;
			}
			return false;
		};
		this.CanMoveToDynamicCell = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "CanPickup";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_PICKUP;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Pickupable pickupable = (Pickupable)data;
			return !(pickupable == null) && !(context.consumerState.consumer == null) && !pickupable.KPrefabID.HasTag(GameTags.StoredPrivate) && pickupable.CouldBePickedUpByMinion(context.consumerState.gameObject) && context.consumerState.consumer.CanReach(pickupable);
		};
		this.CanPickup = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsAwake";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_AWAKE;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.consumerState.consumer == null)
			{
				return false;
			}
			StaminaMonitor.Instance smi = context.consumerState.consumer.GetSMI<StaminaMonitor.Instance>();
			return !smi.IsInsideState(smi.sm.sleepy.sleeping);
		};
		this.IsAwake = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsStanding";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_STANDING;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return !(context.consumerState.consumer == null) && !(context.consumerState.navigator == null) && context.consumerState.navigator.CurrentNavType == NavType.Floor;
		};
		this.IsStanding = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsMoving";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MOVING;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return !(context.consumerState.consumer == null) && !(context.consumerState.navigator == null) && context.consumerState.navigator.IsMoving();
		};
		this.IsMoving = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsOffLadder";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_OFF_LADDER;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return !(context.consumerState.consumer == null) && !(context.consumerState.navigator == null) && context.consumerState.navigator.CurrentNavType != NavType.Ladder && context.consumerState.navigator.CurrentNavType != NavType.Pole;
		};
		this.IsOffLadder = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "NotInTube";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.NOT_IN_TUBE;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return !(context.consumerState.consumer == null) && !(context.consumerState.navigator == null) && context.consumerState.navigator.CurrentNavType != NavType.Tube;
		};
		this.NotInTube = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "ConsumerHasTrait";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.HAS_TRAIT;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			string trait_id = (string)data;
			Traits traits = context.consumerState.traits;
			return !(traits == null) && traits.HasTrait(trait_id);
		};
		this.ConsumerHasTrait = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsOperational";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_OPERATIONAL;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return (data as Operational).IsOperational;
		};
		this.IsOperational = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsNotMarkedForDeconstruction";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MARKED_FOR_DECONSTRUCTION;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Deconstructable deconstructable = data as Deconstructable;
			return deconstructable == null || !deconstructable.IsMarkedForDeconstruction();
		};
		this.IsNotMarkedForDeconstruction = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsNotMarkedForDisable";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MARKED_FOR_DISABLE;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			BuildingEnabledButton buildingEnabledButton = data as BuildingEnabledButton;
			return buildingEnabledButton == null || (buildingEnabledButton.IsEnabled && !buildingEnabledButton.WaitingForDisable);
		};
		this.IsNotMarkedForDisable = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsFunctional";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_FUNCTIONAL;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return (data as Operational).IsFunctional;
		};
		this.IsFunctional = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsOverrideTargetNullOrMe";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_OVERRIDE_TARGET_NULL_OR_ME;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return context.isAttemptingOverride || context.chore.overrideTarget == null || context.chore.overrideTarget == context.consumerState.consumer;
		};
		this.IsOverrideTargetNullOrMe = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "NotChoreCreator";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.NOT_CHORE_CREATOR;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			GameObject y = (GameObject)data;
			return !(context.consumerState.consumer == null) && !(context.consumerState.gameObject == y);
		};
		this.NotChoreCreator = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsGettingMoreStressed";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_GETTING_MORE_STRESSED;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return Db.Get().Amounts.Stress.Lookup(context.consumerState.gameObject).GetDelta() > 0f;
		};
		this.IsGettingMoreStressed = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsAllowedByAutomation";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_ALLOWED_BY_AUTOMATION;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((Automatable)data).AllowedByAutomation(context.consumerState.hasSolidTransferArm);
		};
		this.IsAllowedByAutomation = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "HasTag";
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Tag tag = (Tag)data;
			return context.consumerState.prefabid.HasTag(tag);
		};
		this.HasTag = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "CheckBehaviourPrecondition";
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Tag tag = (Tag)data;
			return context.consumerState.consumer.RunBehaviourPrecondition(tag);
		};
		this.CheckBehaviourPrecondition = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "CanDoWorkerPrioritizable";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_DO_RECREATION;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			if (context.consumerState.consumer == null)
			{
				return false;
			}
			IWorkerPrioritizable workerPrioritizable = data as IWorkerPrioritizable;
			if (workerPrioritizable == null)
			{
				return false;
			}
			int num = 0;
			if (workerPrioritizable.GetWorkerPriority(context.consumerState.worker, out num))
			{
				context.consumerPriority += num;
				return true;
			}
			return false;
		};
		this.CanDoWorkerPrioritizable = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsExclusivelyAvailableWithOtherChores";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.EXCLUSIVELY_AVAILABLE;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			foreach (Chore chore in ((List<Chore>)data))
			{
				if (chore != context.chore && chore.driver != null)
				{
					return false;
				}
			}
			return true;
		};
		this.IsExclusivelyAvailableWithOtherChores = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsBladderFull";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.BLADDER_FULL;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			BladderMonitor.Instance smi = context.consumerState.gameObject.GetSMI<BladderMonitor.Instance>();
			return smi != null && smi.NeedsToPee();
		};
		this.IsBladderFull = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsBladderNotFull";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.BLADDER_NOT_FULL;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			BladderMonitor.Instance smi = context.consumerState.gameObject.GetSMI<BladderMonitor.Instance>();
			return smi == null || !smi.NeedsToPee();
		};
		this.IsBladderNotFull = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "NoDeadBodies";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.NO_DEAD_BODIES;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return Components.LiveMinionIdentities.Count == Components.MinionIdentities.Count;
		};
		this.NoDeadBodies = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "NoRobots";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.NOT_A_ROBOT;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return context.consumerState.resume != null;
		};
		this.IsNotARobot = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "NotCurrentlyPeeing";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.CURRENTLY_PEEING;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			bool result = true;
			Chore currentChore = context.consumerState.choreDriver.GetCurrentChore();
			if (currentChore != null)
			{
				string id = currentChore.choreType.Id;
				result = (id != Db.Get().ChoreTypes.BreakPee.Id && id != Db.Get().ChoreTypes.Pee.Id);
			}
			return result;
		};
		this.NotCurrentlyPeeing = precondition;
		precondition = default(Chore.Precondition);
		precondition.id = "IsRocketTravelling";
		precondition.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_ROCKET_TRAVELLING;
		precondition.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Clustercraft component = ClusterManager.Instance.GetWorld(context.chore.gameObject.GetMyWorldId()).GetComponent<Clustercraft>();
			return !(component == null) && component.IsTravellingAndFueled();
		};
		this.IsRocketTravelling = precondition;
		base..ctor();
	}

	private static ChorePreconditions _instance;

	public Chore.Precondition IsPreemptable;

	public Chore.Precondition HasUrge;

	public Chore.Precondition IsValid;

	public Chore.Precondition IsPermitted;

	public Chore.Precondition IsAssignedtoMe;

	public Chore.Precondition IsInMyRoom;

	public Chore.Precondition IsPreferredAssignable;

	public Chore.Precondition IsPreferredAssignableOrUrgentBladder;

	public Chore.Precondition IsNotTransferArm;

	public Chore.Precondition HasSkillPerk;

	public Chore.Precondition IsMinion;

	public Chore.Precondition IsMoreSatisfyingEarly;

	public Chore.Precondition IsMoreSatisfyingLate;

	public Chore.Precondition IsChattable;

	public Chore.Precondition IsNotRedAlert;

	public Chore.Precondition IsScheduledTime;

	public Chore.Precondition CanMoveTo;

	public Chore.Precondition CanMoveToCell;

	public Chore.Precondition CanMoveToDynamicCell;

	public Chore.Precondition CanPickup;

	public Chore.Precondition IsAwake;

	public Chore.Precondition IsStanding;

	public Chore.Precondition IsMoving;

	public Chore.Precondition IsOffLadder;

	public Chore.Precondition NotInTube;

	public Chore.Precondition ConsumerHasTrait;

	public Chore.Precondition IsOperational;

	public Chore.Precondition IsNotMarkedForDeconstruction;

	public Chore.Precondition IsNotMarkedForDisable;

	public Chore.Precondition IsFunctional;

	public Chore.Precondition IsOverrideTargetNullOrMe;

	public Chore.Precondition NotChoreCreator;

	public Chore.Precondition IsGettingMoreStressed;

	public Chore.Precondition IsAllowedByAutomation;

	public Chore.Precondition HasTag;

	public Chore.Precondition CheckBehaviourPrecondition;

	public Chore.Precondition CanDoWorkerPrioritizable;

	public Chore.Precondition IsExclusivelyAvailableWithOtherChores;

	public Chore.Precondition IsBladderFull;

	public Chore.Precondition IsBladderNotFull;

	public Chore.Precondition NoDeadBodies;

	public Chore.Precondition IsNotARobot;

	public Chore.Precondition NotCurrentlyPeeing;

	public Chore.Precondition IsRocketTravelling;
}
