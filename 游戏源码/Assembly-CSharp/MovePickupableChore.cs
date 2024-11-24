using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020006D8 RID: 1752
public class MovePickupableChore : Chore<MovePickupableChore.StatesInstance>
{
	// Token: 0x06001F9B RID: 8091 RVA: 0x001B8F80 File Offset: 0x001B7180
	public MovePickupableChore(IStateMachineTarget target, GameObject pickupable, Action<Chore> onEnd) : base((pickupable.GetComponent<CreatureBrain>() == null) ? Db.Get().ChoreTypes.Fetch : Db.Get().ChoreTypes.Ranch, target, target.GetComponent<ChoreProvider>(), false, null, null, onEnd, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MovePickupableChore.StatesInstance(this);
		Pickupable component = pickupable.GetComponent<Pickupable>();
		this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, target.GetComponent<Storage>());
		this.AddPrecondition(ChorePreconditions.instance.IsNotARobot, "FetchDrone");
		this.AddPrecondition(ChorePreconditions.instance.IsNotTransferArm, this);
		if (pickupable.GetComponent<CreatureBrain>())
		{
			this.AddPrecondition(MovePickupableChore.CanReachCritter, pickupable);
			this.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanWrangleCreatures);
			this.AddPrecondition(ChorePreconditions.instance.CanMoveTo, pickupable.GetComponent<Capturable>());
		}
		else
		{
			this.AddPrecondition(ChorePreconditions.instance.CanPickup, component);
		}
		PrimaryElement primaryElement = component.PrimaryElement;
		base.smi.sm.requestedamount.Set(primaryElement.Mass, base.smi, false);
		base.smi.sm.pickupablesource.Set(pickupable.gameObject, base.smi, false);
		base.smi.sm.deliverypoint.Set(target.gameObject, base.smi, false);
		this.movePlacer = target.gameObject;
		bool flag = MinionGroupProber.Get().IsReachable(Grid.PosToCell(pickupable), OffsetGroups.Standard) && MinionGroupProber.Get().IsReachable(Grid.PosToCell(target.gameObject), OffsetGroups.Standard);
		this.OnReachableChanged(flag);
		pickupable.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
		target.Subscribe(-1432940121, new Action<object>(this.OnReachableChanged));
		Prioritizable component2 = target.GetComponent<Prioritizable>();
		if (!component2.IsPrioritizable())
		{
			component2.AddRef();
		}
		base.SetPrioritizable(target.GetComponent<Prioritizable>());
	}

	// Token: 0x06001F9C RID: 8092 RVA: 0x001B9194 File Offset: 0x001B7394
	private void OnReachableChanged(object data)
	{
		Color color = ((bool)data) ? Color.white : new Color(0.91f, 0.21f, 0.2f);
		this.SetColor(this.movePlacer, color);
	}

	// Token: 0x06001F9D RID: 8093 RVA: 0x000B4C42 File Offset: 0x000B2E42
	private void SetColor(GameObject visualizer, Color color)
	{
		if (visualizer != null)
		{
			visualizer.GetComponentInChildren<MeshRenderer>().material.color = color;
		}
	}

	// Token: 0x06001F9E RID: 8094 RVA: 0x001B91D4 File Offset: 0x001B73D4
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("MovePickupable null context.consumer");
			return;
		}
		if (base.smi == null)
		{
			global::Debug.LogError("MovePickupable null smi");
			return;
		}
		if (base.smi.sm == null)
		{
			global::Debug.LogError("MovePickupable null smi.sm");
			return;
		}
		if (base.smi.sm.pickupablesource == null)
		{
			global::Debug.LogError("MovePickupable null smi.sm.pickupablesource");
			return;
		}
		base.smi.sm.deliverer.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
	}

	// Token: 0x0400147F RID: 5247
	public GameObject movePlacer;

	// Token: 0x04001480 RID: 5248
	public static Chore.Precondition CanReachCritter = new Chore.Precondition
	{
		id = "CanReachCritter",
		description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			GameObject gameObject = (GameObject)data;
			return !(gameObject == null) && gameObject.HasTag(GameTags.Reachable);
		}
	};

	// Token: 0x020006D9 RID: 1753
	public class StatesInstance : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.GameInstance
	{
		// Token: 0x06001FA0 RID: 8096 RVA: 0x000B4C5E File Offset: 0x000B2E5E
		public StatesInstance(MovePickupableChore master) : base(master)
		{
		}
	}

	// Token: 0x020006DA RID: 1754
	public class States : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore>
	{
		// Token: 0x06001FA1 RID: 8097 RVA: 0x001B92C8 File Offset: 0x001B74C8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.deliverypoint);
			this.fetch.Target(this.deliverer).DefaultState(this.fetch.approach).Enter(delegate(MovePickupableChore.StatesInstance smi)
			{
				this.pickupablesource.Get<Pickupable>(smi).ClearReservations();
			}).ToggleReserve(this.deliverer, this.pickupablesource, this.requestedamount, this.actualamount).EnterTransition(this.fetch.approachCritter, (MovePickupableChore.StatesInstance smi) => this.IsCritter(smi)).OnTargetLost(this.pickupablesource, null);
			this.fetch.approachCritter.Enter(delegate(MovePickupableChore.StatesInstance smi)
			{
				GameObject gameObject = this.pickupablesource.Get(smi);
				if (!gameObject.HasTag(GameTags.Creatures.Bagged))
				{
					IdleStates.Instance smi2 = gameObject.GetSMI<IdleStates.Instance>();
					if (!smi2.IsNullOrStopped())
					{
						smi2.GoTo(smi2.sm.root);
					}
					FlopStates.Instance smi3 = gameObject.GetSMI<FlopStates.Instance>();
					if (!smi3.IsNullOrStopped())
					{
						smi3.GoTo(smi3.sm.root);
					}
					gameObject.GetComponent<Navigator>().Stop(false, true);
				}
			}).MoveTo<Capturable>(this.pickupablesource, this.fetch.wrangle, null, null, null);
			this.fetch.wrangle.EnterTransition(this.fetch.approach, (MovePickupableChore.StatesInstance smi) => this.pickupablesource.Get(smi).HasTag(GameTags.Creatures.Bagged)).ToggleWork<Capturable>(this.pickupablesource, this.fetch.approach, null, null);
			this.fetch.approach.MoveTo<IApproachable>(this.pickupablesource, this.fetch.pickup, new Func<MovePickupableChore.StatesInstance, CellOffset[]>(this.GetFetcherOffset), null, null);
			this.fetch.pickup.DoPickup(this.pickupablesource, this.pickup, this.actualamount, this.approachstorage, this.delivering.deliverfail);
			this.approachstorage.DefaultState(this.approachstorage.deliveryStorage);
			this.approachstorage.deliveryStorage.InitializeStates(this.deliverer, this.deliverypoint, new Func<MovePickupableChore.StatesInstance, CellOffset[]>(this.GetFetcherOffset), this.delivering.storing, this.delivering.deliverfail, NavigationTactics.ReduceTravelDistance);
			this.delivering.storing.Target(this.deliverer).DoDelivery(this.deliverer, this.deliverypoint, this.success, this.delivering.deliverfail);
			this.delivering.deliverfail.ReturnFailure();
			this.success.Enter(delegate(MovePickupableChore.StatesInstance smi)
			{
				Storage component = this.deliverypoint.Get(smi).GetComponent<Storage>();
				Storage component2 = this.deliverer.Get(smi).GetComponent<Storage>();
				float num = this.actualamount.Get(smi);
				GameObject gameObject = this.pickup.Get(smi);
				num += gameObject.GetComponent<PrimaryElement>().Mass;
				this.actualamount.Set(num, smi, false);
				component2.Transfer(this.pickup.Get(smi), component, false, false);
				this.DropPickupable(component, gameObject);
				CancellableMove component3 = component.GetComponent<CancellableMove>();
				Movable component4 = gameObject.GetComponent<Movable>();
				component3.RemoveMovable(component4);
				component4.ClearMove();
				if (!this.IsDeliveryComplete(smi))
				{
					GameObject go = this.pickupablesource.Get(smi);
					int num2 = Grid.PosToCell(this.deliverypoint.Get(smi));
					if (this.pickupablesource.Get(smi) == null || Grid.PosToCell(go) == num2)
					{
						GameObject nextTarget = component3.GetNextTarget();
						this.pickupablesource.Set(nextTarget, smi, false);
						PrimaryElement component5 = nextTarget.GetComponent<PrimaryElement>();
						smi.sm.requestedamount.Set(component5.Mass, smi, false);
					}
					smi.GoTo(this.fetch);
				}
			}).ReturnSuccess();
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x000B4C67 File Offset: 0x000B2E67
		private CellOffset[] GetFetcherOffset(MovePickupableChore.StatesInstance smi)
		{
			return this.deliverer.Get(smi).GetComponent<WorkerBase>().GetFetchCellOffsets();
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x001B9500 File Offset: 0x001B7700
		private void DropPickupable(Storage storage, GameObject delivered)
		{
			if (delivered.GetComponent<Capturable>() != null)
			{
				List<GameObject> items = storage.items;
				int count = items.Count;
				Vector3 position = Grid.CellToPosCBC(Grid.PosToCell(storage), Grid.SceneLayer.Creatures);
				for (int i = count - 1; i >= 0; i--)
				{
					GameObject gameObject = items[i];
					storage.Drop(gameObject, true);
					gameObject.transform.SetPosition(position);
					gameObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
				}
				return;
			}
			storage.DropAll(false, false, default(Vector3), true, null);
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x001B9584 File Offset: 0x001B7784
		private bool IsDeliveryComplete(MovePickupableChore.StatesInstance smi)
		{
			GameObject gameObject = smi.sm.deliverypoint.Get(smi);
			return !(gameObject != null) || gameObject.GetComponent<CancellableMove>().IsDeliveryComplete();
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x001B95BC File Offset: 0x001B77BC
		private bool IsCritter(MovePickupableChore.StatesInstance smi)
		{
			GameObject gameObject = this.pickupablesource.Get(smi);
			return gameObject != null && gameObject.GetComponent<Capturable>() != null;
		}

		// Token: 0x04001481 RID: 5249
		public static CellOffset[] critterCellOffsets = new CellOffset[]
		{
			new CellOffset(0, 0)
		};

		// Token: 0x04001482 RID: 5250
		public static HashedString[] critterReleaseWorkAnims = new HashedString[]
		{
			"place",
			"release"
		};

		// Token: 0x04001483 RID: 5251
		public static KAnimFile[] critterReleaseAnim = new KAnimFile[]
		{
			Assets.GetAnim("anim_restrain_creature_kanim")
		};

		// Token: 0x04001484 RID: 5252
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter deliverer;

		// Token: 0x04001485 RID: 5253
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter pickupablesource;

		// Token: 0x04001486 RID: 5254
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter pickup;

		// Token: 0x04001487 RID: 5255
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.TargetParameter deliverypoint;

		// Token: 0x04001488 RID: 5256
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.FloatParameter requestedamount;

		// Token: 0x04001489 RID: 5257
		public StateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.FloatParameter actualamount;

		// Token: 0x0400148A RID: 5258
		public MovePickupableChore.States.FetchState fetch;

		// Token: 0x0400148B RID: 5259
		public MovePickupableChore.States.ApproachStorage approachstorage;

		// Token: 0x0400148C RID: 5260
		public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State success;

		// Token: 0x0400148D RID: 5261
		public MovePickupableChore.States.DeliveryState delivering;

		// Token: 0x020006DB RID: 1755
		public class ApproachStorage : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State
		{
			// Token: 0x0400148E RID: 5262
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.ApproachSubState<Storage> deliveryStorage;

			// Token: 0x0400148F RID: 5263
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.ApproachSubState<Storage> unbagCritter;
		}

		// Token: 0x020006DC RID: 1756
		public class DeliveryState : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State
		{
			// Token: 0x04001490 RID: 5264
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State storing;

			// Token: 0x04001491 RID: 5265
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State deliverfail;
		}

		// Token: 0x020006DD RID: 1757
		public class FetchState : GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State
		{
			// Token: 0x04001492 RID: 5266
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.ApproachSubState<Pickupable> approach;

			// Token: 0x04001493 RID: 5267
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State pickup;

			// Token: 0x04001494 RID: 5268
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State approachCritter;

			// Token: 0x04001495 RID: 5269
			public GameStateMachine<MovePickupableChore.States, MovePickupableChore.StatesInstance, MovePickupableChore, object>.State wrangle;
		}
	}
}
