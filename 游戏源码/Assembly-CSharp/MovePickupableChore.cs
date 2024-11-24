using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class MovePickupableChore : Chore<MovePickupableChore.StatesInstance>
{
	public class StatesInstance : GameStateMachine<States, StatesInstance, MovePickupableChore, object>.GameInstance
	{
		public StatesInstance(MovePickupableChore master)
			: base(master)
		{
		}
	}

	public class States : GameStateMachine<States, StatesInstance, MovePickupableChore>
	{
		public class ApproachStorage : State
		{
			public ApproachSubState<Storage> deliveryStorage;

			public ApproachSubState<Storage> unbagCritter;
		}

		public class DeliveryState : State
		{
			public State storing;

			public State deliverfail;
		}

		public class FetchState : State
		{
			public ApproachSubState<Pickupable> approach;

			public State pickup;

			public State approachCritter;

			public State wrangle;
		}

		public static CellOffset[] critterCellOffsets = new CellOffset[1]
		{
			new CellOffset(0, 0)
		};

		public static HashedString[] critterReleaseWorkAnims = new HashedString[2] { "place", "release" };

		public static KAnimFile[] critterReleaseAnim = new KAnimFile[1] { Assets.GetAnim("anim_restrain_creature_kanim") };

		public TargetParameter deliverer;

		public TargetParameter pickupablesource;

		public TargetParameter pickup;

		public TargetParameter deliverypoint;

		public FloatParameter requestedamount;

		public FloatParameter actualamount;

		public FetchState fetch;

		public ApproachStorage approachstorage;

		public State success;

		public DeliveryState delivering;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = fetch;
			Target(deliverypoint);
			fetch.Target(deliverer).DefaultState(fetch.approach).Enter(delegate(StatesInstance smi)
			{
				pickupablesource.Get<Pickupable>(smi).ClearReservations();
			})
				.ToggleReserve(deliverer, pickupablesource, requestedamount, actualamount)
				.EnterTransition(fetch.approachCritter, (StatesInstance smi) => IsCritter(smi))
				.OnTargetLost(pickupablesource, null);
			fetch.approachCritter.Enter(delegate(StatesInstance smi)
			{
				GameObject gameObject2 = pickupablesource.Get(smi);
				if (!gameObject2.HasTag(GameTags.Creatures.Bagged))
				{
					IdleStates.Instance sMI = gameObject2.GetSMI<IdleStates.Instance>();
					if (!sMI.IsNullOrStopped())
					{
						sMI.GoTo(sMI.sm.root);
					}
					FlopStates.Instance sMI2 = gameObject2.GetSMI<FlopStates.Instance>();
					if (!sMI2.IsNullOrStopped())
					{
						sMI2.GoTo(sMI2.sm.root);
					}
					gameObject2.GetComponent<Navigator>().Stop();
				}
			}).MoveTo<Capturable>(pickupablesource, fetch.wrangle);
			fetch.wrangle.EnterTransition(fetch.approach, (StatesInstance smi) => pickupablesource.Get(smi).HasTag(GameTags.Creatures.Bagged)).ToggleWork<Capturable>(pickupablesource, fetch.approach, null, null);
			fetch.approach.MoveTo<IApproachable>(pickupablesource, fetch.pickup);
			fetch.pickup.DoPickup(pickupablesource, pickup, actualamount, approachstorage, delivering.deliverfail);
			approachstorage.DefaultState(approachstorage.deliveryStorage);
			approachstorage.deliveryStorage.InitializeStates(deliverer, deliverypoint, delivering.storing, delivering.deliverfail, null, NavigationTactics.ReduceTravelDistance);
			delivering.storing.Target(deliverer).DoDelivery(deliverer, deliverypoint, success, delivering.deliverfail);
			delivering.deliverfail.ReturnFailure();
			success.Enter(delegate(StatesInstance smi)
			{
				Storage component = deliverypoint.Get(smi).GetComponent<Storage>();
				Storage component2 = deliverer.Get(smi).GetComponent<Storage>();
				float num = actualamount.Get(smi);
				GameObject gameObject = pickup.Get(smi);
				num += gameObject.GetComponent<PrimaryElement>().Mass;
				actualamount.Set(num, smi);
				component2.Transfer(pickup.Get(smi), component);
				DropPickupable(component, gameObject);
				CancellableMove component3 = component.GetComponent<CancellableMove>();
				Movable component4 = gameObject.GetComponent<Movable>();
				component3.RemoveMovable(component4);
				component4.ClearMove();
				if (!IsDeliveryComplete(smi))
				{
					GameObject go = pickupablesource.Get(smi);
					int num2 = Grid.PosToCell(deliverypoint.Get(smi));
					if (pickupablesource.Get(smi) == null || Grid.PosToCell(go) == num2)
					{
						GameObject nextTarget = component3.GetNextTarget();
						pickupablesource.Set(nextTarget, smi);
						PrimaryElement component5 = nextTarget.GetComponent<PrimaryElement>();
						smi.sm.requestedamount.Set(component5.Mass, smi);
					}
					smi.GoTo(fetch);
				}
			}).ReturnSuccess();
		}

		private void DropPickupable(Storage storage, GameObject delivered)
		{
			if (delivered.GetComponent<Capturable>() != null)
			{
				List<GameObject> items = storage.items;
				int count = items.Count;
				Vector3 position = Grid.CellToPosCBC(Grid.PosToCell(storage), Grid.SceneLayer.Creatures);
				for (int num = count - 1; num >= 0; num--)
				{
					GameObject gameObject = items[num];
					storage.Drop(gameObject);
					gameObject.transform.SetPosition(position);
					gameObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
				}
			}
			else
			{
				storage.DropAll();
			}
		}

		private bool IsDeliveryComplete(StatesInstance smi)
		{
			GameObject gameObject = smi.sm.deliverypoint.Get(smi);
			if (gameObject != null)
			{
				return gameObject.GetComponent<CancellableMove>().IsDeliveryComplete();
			}
			return true;
		}

		private bool IsCritter(StatesInstance smi)
		{
			GameObject gameObject = pickupablesource.Get(smi);
			if (gameObject != null)
			{
				return gameObject.GetComponent<Capturable>() != null;
			}
			return false;
		}
	}

	public GameObject movePlacer;

	public static Precondition CanReachCritter = new Precondition
	{
		id = "CanReachCritter",
		description = DUPLICANTS.CHORES.PRECONDITIONS.CAN_MOVE_TO,
		fn = delegate(ref Precondition.Context context, object data)
		{
			GameObject gameObject = (GameObject)data;
			return !(gameObject == null) && gameObject.HasTag(GameTags.Reachable);
		}
	};

	public MovePickupableChore(IStateMachineTarget target, GameObject pickupable, Action<Chore> onEnd)
		: base((pickupable.GetComponent<CreatureBrain>() == null) ? Db.Get().ChoreTypes.Fetch : Db.Get().ChoreTypes.Ranch, target, target.GetComponent<ChoreProvider>(), run_until_complete: false, (Action<Chore>)null, (Action<Chore>)null, onEnd, PriorityScreen.PriorityClass.basic, 5, is_preemptable: false, allow_in_context_menu: true, 0, add_to_daily_report: false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new StatesInstance(this);
		Pickupable component = pickupable.GetComponent<Pickupable>();
		AddPrecondition(ChorePreconditions.instance.CanMoveTo, target.GetComponent<Storage>());
		AddPrecondition(ChorePreconditions.instance.IsNotARobot, this);
		AddPrecondition(ChorePreconditions.instance.IsNotTransferArm, this);
		if ((bool)pickupable.GetComponent<CreatureBrain>())
		{
			AddPrecondition(CanReachCritter, pickupable);
			AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanWrangleCreatures);
			AddPrecondition(ChorePreconditions.instance.CanMoveTo, pickupable.GetComponent<Capturable>());
		}
		else
		{
			AddPrecondition(ChorePreconditions.instance.CanPickup, component);
		}
		PrimaryElement primaryElement = component.PrimaryElement;
		base.smi.sm.requestedamount.Set(primaryElement.Mass, base.smi);
		base.smi.sm.pickupablesource.Set(pickupable.gameObject, base.smi);
		base.smi.sm.deliverypoint.Set(target.gameObject, base.smi);
		movePlacer = target.gameObject;
		OnReachableChanged(MinionGroupProber.Get().IsReachable(Grid.PosToCell(pickupable), OffsetGroups.Standard) && MinionGroupProber.Get().IsReachable(Grid.PosToCell(target.gameObject), OffsetGroups.Standard));
		pickupable.Subscribe(-1432940121, OnReachableChanged);
		target.Subscribe(-1432940121, OnReachableChanged);
		Prioritizable component2 = target.GetComponent<Prioritizable>();
		if (!component2.IsPrioritizable())
		{
			component2.AddRef();
		}
		SetPrioritizable(target.GetComponent<Prioritizable>());
	}

	private void OnReachableChanged(object data)
	{
		Color color = (((bool)data) ? Color.white : new Color(0.91f, 0.21f, 0.2f));
		SetColor(movePlacer, color);
	}

	private void SetColor(GameObject visualizer, Color color)
	{
		if (visualizer != null)
		{
			visualizer.GetComponentInChildren<MeshRenderer>().material.color = color;
		}
	}

	public override void Begin(Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			Debug.LogError("MovePickupable null context.consumer");
			return;
		}
		if (base.smi == null)
		{
			Debug.LogError("MovePickupable null smi");
			return;
		}
		if (base.smi.sm == null)
		{
			Debug.LogError("MovePickupable null smi.sm");
			return;
		}
		if (base.smi.sm.pickupablesource == null)
		{
			Debug.LogError("MovePickupable null smi.sm.pickupablesource");
			return;
		}
		base.smi.sm.deliverer.Set(context.consumerState.gameObject, base.smi);
		base.Begin(context);
	}
}
