using System.Linq;
using STRINGS;
using UnityEngine;

public class DrinkMilkStates : GameStateMachine<DrinkMilkStates, DrinkMilkStates.Instance, IStateMachineTarget, DrinkMilkStates.Def>
{
	public class Def : BaseDef
	{
		public delegate CellOffset DrinkCellOffsetGetFn(MilkFeeder.Instance milkFeederInstance, DrinkMilkMonitor.Instance critterInstance, bool isCramped);

		public bool shouldBeBehindMilkTank = true;

		public DrinkCellOffsetGetFn drinkCellOffsetGetFn = DrinkCellOffsetGet_CritterOneByOne;

		public static CellOffset DrinkCellOffsetGet_CritterOneByOne(MilkFeeder.Instance milkFeederInstance, DrinkMilkMonitor.Instance critterInstance, bool isCramped)
		{
			return milkFeederInstance.GetComponent<Rotatable>().GetRotatedCellOffset(MilkFeederConfig.DRINK_FROM_OFFSET);
		}

		public static CellOffset DrinkCellOffsetGet_GassyMoo(MilkFeeder.Instance milkFeederInstance, DrinkMilkMonitor.Instance critterInstance, bool isCramped)
		{
			Rotatable component = milkFeederInstance.GetComponent<Rotatable>();
			CellOffset rotatedCellOffset = component.GetRotatedCellOffset(MilkFeederConfig.DRINK_FROM_OFFSET);
			if (component.IsRotated)
			{
				rotatedCellOffset.x--;
			}
			if (isCramped)
			{
				if (component.IsRotated)
				{
					rotatedCellOffset.x += 2;
				}
				else
				{
					rotatedCellOffset.x -= 2;
				}
			}
			return rotatedCellOffset;
		}

		public static CellOffset DrinkCellOffsetGet_BammothAdult(MilkFeeder.Instance milkFeederInstance, DrinkMilkMonitor.Instance critterInstance, bool isCramped)
		{
			Rotatable component = milkFeederInstance.GetComponent<Rotatable>();
			CellOffset rotatedCellOffset = component.GetRotatedCellOffset(MilkFeederConfig.DRINK_FROM_OFFSET);
			if (!isCramped)
			{
				int x = Grid.CellToXY(Grid.OffsetCell(Grid.PosToCell(milkFeederInstance), rotatedCellOffset)).x;
				int x2 = Grid.PosToXY(critterInstance.transform.position).x;
				if (x > x2 && !component.IsRotated)
				{
					rotatedCellOffset.x++;
				}
				else if (x < x2 && component.IsRotated)
				{
					rotatedCellOffset.x--;
				}
				else if (x == x2)
				{
					if (component.IsRotated)
					{
						rotatedCellOffset.x--;
					}
					else
					{
						rotatedCellOffset.x++;
					}
				}
			}
			else if (component.IsRotated)
			{
				rotatedCellOffset.x++;
			}
			else
			{
				rotatedCellOffset.x--;
			}
			return rotatedCellOffset;
		}
	}

	public new class Instance : GameInstance
	{
		public bool critterIsCramped;

		public Instance(Chore<Instance> chore, Def def)
			: base((IStateMachineTarget)chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviour_TryToDrinkMilkFromFeeder);
		}

		public void RequestToStopFeeding()
		{
			base.sm.requestedToStopFeeding.Trigger(base.smi);
		}
	}

	public class EatingState : State
	{
		public State pre;

		public State loop;

		public State pst;
	}

	public State goingToDrink;

	public EatingState drink;

	public State behaviourComplete;

	public TargetParameter targetMilkFeeder;

	public Signal requestedToStopFeeding;

	private static void SetSceneLayer(Instance smi, Grid.SceneLayer layer)
	{
		SegmentedCreature.Instance sMI = smi.GetSMI<SegmentedCreature.Instance>();
		if (sMI != null && sMI.segments != null)
		{
			foreach (SegmentedCreature.CreatureSegment item in sMI.segments.Reverse())
			{
				item.animController.SetSceneLayer(layer);
			}
			return;
		}
		smi.GetComponent<KBatchedAnimController>().SetSceneLayer(layer);
	}

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = goingToDrink;
		root.Enter(SetTarget).Enter(CheckIfCramped).Enter(ReserveMilkFeeder)
			.Exit(UnreserveMilkFeeder)
			.Transition(behaviourComplete, delegate(Instance smi)
			{
				MilkFeeder.Instance instance2 = GetTargetMilkFeeder(smi);
				if (instance2.IsNullOrDestroyed() || !instance2.IsOperational())
				{
					smi.GetComponent<KAnimControllerBase>().Queue("idle_loop", KAnim.PlayMode.Loop);
					return true;
				}
				return false;
			});
		goingToDrink.MoveTo(GetCellToDrinkFrom, drink).ToggleStatusItem(CREATURES.STATUSITEMS.LOOKINGFORMILK.NAME, CREATURES.STATUSITEMS.LOOKINGFORMILK.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		drink.DefaultState(drink.pre).Enter("FaceMilkFeeder", FaceMilkFeeder).ToggleStatusItem(CREATURES.STATUSITEMS.DRINKINGMILK.NAME, CREATURES.STATUSITEMS.DRINKINGMILK.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main)
			.Enter(delegate(Instance smi)
			{
				SetSceneLayer(smi, smi.def.shouldBeBehindMilkTank ? Grid.SceneLayer.BuildingUse : Grid.SceneLayer.Creatures);
			})
			.Exit(delegate(Instance smi)
			{
				SetSceneLayer(smi, Grid.SceneLayer.Creatures);
			});
		drink.pre.QueueAnim(GetAnimDrinkPre).OnAnimQueueComplete(drink.loop);
		drink.loop.QueueAnim(GetAnimDrinkLoop, loop: true).Enter(delegate(Instance smi)
		{
			MilkFeeder.Instance instance = GetTargetMilkFeeder(smi);
			if (instance != null)
			{
				instance.RequestToStartFeeding(smi);
			}
			else
			{
				smi.GoTo(drink.pst);
			}
		}).OnSignal(requestedToStopFeeding, drink.pst);
		drink.pst.QueueAnim(GetAnimDrinkPst).Enter(DrinkMilkComplete).OnAnimQueueComplete(behaviourComplete);
		behaviourComplete.QueueAnim("idle_loop", loop: true).BehaviourComplete(GameTags.Creatures.Behaviour_TryToDrinkMilkFromFeeder);
	}

	private static MilkFeeder.Instance GetTargetMilkFeeder(Instance smi)
	{
		if (smi.sm.targetMilkFeeder.IsNullOrDestroyed())
		{
			return null;
		}
		GameObject gameObject = smi.sm.targetMilkFeeder.Get(smi);
		if (gameObject.IsNullOrDestroyed())
		{
			return null;
		}
		MilkFeeder.Instance sMI = gameObject.GetSMI<MilkFeeder.Instance>();
		if (gameObject.IsNullOrDestroyed() || sMI.IsNullOrStopped())
		{
			return null;
		}
		return sMI;
	}

	private static void SetTarget(Instance smi)
	{
		smi.sm.targetMilkFeeder.Set(smi.GetSMI<DrinkMilkMonitor.Instance>().targetMilkFeeder.gameObject, smi);
	}

	private static void CheckIfCramped(Instance smi)
	{
		smi.critterIsCramped = smi.GetSMI<DrinkMilkMonitor.Instance>().doesTargetMilkFeederHaveSpaceForCritter;
	}

	private static void ReserveMilkFeeder(Instance smi)
	{
		GetTargetMilkFeeder(smi)?.SetReserved(isReserved: true);
	}

	private static void UnreserveMilkFeeder(Instance smi)
	{
		GetTargetMilkFeeder(smi)?.SetReserved(isReserved: false);
	}

	private static void DrinkMilkComplete(Instance smi)
	{
		MilkFeeder.Instance instance = GetTargetMilkFeeder(smi);
		if (instance != null)
		{
			smi.GetSMI<DrinkMilkMonitor.Instance>().NotifyFinishedDrinkingMilkFrom(instance);
		}
	}

	private static int GetCellToDrinkFrom(Instance smi)
	{
		MilkFeeder.Instance instance = GetTargetMilkFeeder(smi);
		if (instance == null)
		{
			return Grid.InvalidCell;
		}
		return smi.GetSMI<DrinkMilkMonitor.Instance>().GetDrinkCellOf(instance, smi.critterIsCramped);
	}

	private static string GetAnimDrinkPre(Instance smi)
	{
		if (smi.critterIsCramped)
		{
			return "drink_cramped_pre";
		}
		return "drink_pre";
	}

	private static string GetAnimDrinkLoop(Instance smi)
	{
		if (smi.critterIsCramped)
		{
			return "drink_cramped_loop";
		}
		return "drink_loop";
	}

	private static string GetAnimDrinkPst(Instance smi)
	{
		if (smi.critterIsCramped)
		{
			return "drink_cramped_pst";
		}
		return "drink_pst";
	}

	private static void FaceMilkFeeder(Instance smi)
	{
		MilkFeeder.Instance instance = GetTargetMilkFeeder(smi);
		if (instance != null)
		{
			bool isRotated = instance.GetComponent<Rotatable>().IsRotated;
			float num = (smi.critterIsCramped ? ((!isRotated) ? 20f : (-20f)) : ((!isRotated) ? (-20f) : 20f));
			IApproachable approachable = smi.sm.targetMilkFeeder.Get<IApproachable>(smi);
			if (approachable != null)
			{
				float target_x = approachable.transform.GetPosition().x + num;
				smi.GetComponent<Facing>().Face(target_x);
			}
		}
	}
}
