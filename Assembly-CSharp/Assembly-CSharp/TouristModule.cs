﻿using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class TouristModule : StateMachineComponent<TouristModule.StatesInstance>
{
			public bool IsSuspended
	{
		get
		{
			return this.isSuspended;
		}
	}

		protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

		public void SetSuspended(bool state)
	{
		this.isSuspended = state;
	}

		public void ReleaseAstronaut(object data, bool applyBuff = false)
	{
		if (this.releasingAstronaut)
		{
			return;
		}
		this.releasingAstronaut = true;
		MinionStorage component = base.GetComponent<MinionStorage>();
		List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
		for (int i = storedMinionInfo.Count - 1; i >= 0; i--)
		{
			MinionStorage.Info info = storedMinionInfo[i];
			GameObject gameObject = component.DeserializeMinion(info.id, Grid.CellToPos(Grid.PosToCell(base.smi.master.transform.GetPosition())));
			if (Grid.FakeFloor[Grid.OffsetCell(Grid.PosToCell(base.smi.master.gameObject), 0, -1)])
			{
				gameObject.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
				if (applyBuff)
				{
					gameObject.GetComponent<Effects>().Add(Db.Get().effects.Get("SpaceTourist"), true);
					JoyBehaviourMonitor.Instance smi = gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
					if (smi != null)
					{
						smi.GoToOverjoyed();
					}
				}
			}
		}
		this.releasingAstronaut = false;
	}

		public void OnSuspend(object data)
	{
		Storage component = base.GetComponent<Storage>();
		if (component != null)
		{
			component.capacityKg = component.MassStored();
			component.allowItemRemoval = false;
		}
		if (base.GetComponent<ManualDeliveryKG>() != null)
		{
			UnityEngine.Object.Destroy(base.GetComponent<ManualDeliveryKG>());
		}
		this.SetSuspended(true);
	}

		protected override void OnSpawn()
	{
		base.OnSpawn();
		this.storage = base.GetComponent<Storage>();
		this.assignable = base.GetComponent<Assignable>();
		base.smi.StartSM();
		int cell = Grid.PosToCell(base.gameObject);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("TouristModule.gantryChanged", base.gameObject, cell, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnGantryChanged));
		this.OnGantryChanged(null);
		base.Subscribe<TouristModule>(-1277991738, TouristModule.OnSuspendDelegate);
		base.Subscribe<TouristModule>(684616645, TouristModule.OnAssigneeChangedDelegate);
	}

		private void OnGantryChanged(object data)
	{
		if (base.gameObject != null)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.HasGantry, false);
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.MissingGantry, false);
			int i = Grid.OffsetCell(Grid.PosToCell(base.smi.master.gameObject), 0, -1);
			if (Grid.FakeFloor[i])
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.HasGantry, null);
				return;
			}
			component.AddStatusItem(Db.Get().BuildingStatusItems.MissingGantry, null);
		}
	}

		private Chore CreateWorkChore()
	{
		WorkChore<CommandModuleWorkable> workChore = new WorkChore<CommandModuleWorkable>(Db.Get().ChoreTypes.Astronaut, this, null, true, null, null, null, false, null, false, true, Assets.GetAnim("anim_hat_kanim"), false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, this.assignable);
		return workChore;
	}

		private void OnAssigneeChanged(object data)
	{
		if (data == null && base.gameObject.HasTag(GameTags.RocketOnGround) && base.GetComponent<MinionStorage>().GetStoredMinionInfo().Count > 0)
		{
			this.ReleaseAstronaut(null, false);
			Game.Instance.userMenu.Refresh(base.gameObject);
		}
	}

		protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		this.partitionerEntry.Clear();
		this.ReleaseAstronaut(null, false);
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.smi.StopSM("cleanup");
	}

		public Storage storage;

		[Serialize]
	private bool isSuspended;

		private bool releasingAstronaut;

		private const Sim.Cell.Properties floorCellProperties = (Sim.Cell.Properties)39;

		public Assignable assignable;

		private HandleVector<int>.Handle partitionerEntry;

		private static readonly EventSystem.IntraObjectHandler<TouristModule> OnSuspendDelegate = new EventSystem.IntraObjectHandler<TouristModule>(delegate(TouristModule component, object data)
	{
		component.OnSuspend(data);
	});

		private static readonly EventSystem.IntraObjectHandler<TouristModule> OnAssigneeChangedDelegate = new EventSystem.IntraObjectHandler<TouristModule>(delegate(TouristModule component, object data)
	{
		component.OnAssigneeChanged(data);
	});

		public class StatesInstance : GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.GameInstance
	{
				public StatesInstance(TouristModule smi) : base(smi)
		{
			smi.gameObject.Subscribe(-887025858, delegate(object data)
			{
				smi.SetSuspended(false);
				smi.ReleaseAstronaut(null, true);
				smi.assignable.Unassign();
			});
		}
	}

		public class States : GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule>
	{
				public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("grounded", KAnim.PlayMode.Loop).GoTo(this.awaitingTourist);
			this.awaitingTourist.PlayAnim("grounded", KAnim.PlayMode.Loop).ToggleChore((TouristModule.StatesInstance smi) => smi.master.CreateWorkChore(), this.hasTourist);
			this.hasTourist.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.RocketLanded, this.idle, null).EventTransition(GameHashes.AssigneeChanged, this.idle, null);
		}

				public GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.State idle;

				public GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.State awaitingTourist;

				public GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.State hasTourist;
	}
}
