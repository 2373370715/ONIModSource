using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

public class DrinkMilkMonitor : GameStateMachine<DrinkMilkMonitor, DrinkMilkMonitor.Instance, IStateMachineTarget, DrinkMilkMonitor.Def>
{
	public class Def : BaseDef
	{
		public bool consumesMilk = true;

		public DrinkMilkStates.Def.DrinkCellOffsetGetFn drinkCellOffsetGetFn;
	}

	public new class Instance : GameInstance
	{
		public MilkFeeder.Instance targetMilkFeeder;

		public bool doesTargetMilkFeederHaveSpaceForCritter;

		[MyCmpReq]
		public Navigator navigator;

		[MyCmpGet]
		public DrowningMonitor drowningMonitor;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}

		public void NotifyFinishedDrinkingMilkFrom(MilkFeeder.Instance milkFeeder)
		{
			if (milkFeeder != null && base.def.consumesMilk)
			{
				milkFeeder.ConsumeMilkForOneFeeding();
			}
			base.sm.didFinishDrinkingMilk.Trigger(base.smi);
		}

		public int GetDrinkCellOf(MilkFeeder.Instance milkFeeder, bool isTwoByTwoCritterCramped)
		{
			return Grid.OffsetCell(Grid.PosToCell(milkFeeder), base.def.drinkCellOffsetGetFn(milkFeeder, this, isTwoByTwoCritterCramped));
		}
	}

	public State lookingToDrinkMilk;

	public State applyEffect;

	public State satisfied;

	private Signal didFinishDrinkingMilk;

	private FloatParameter remainingSecondsForEffect;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = lookingToDrinkMilk;
		base.serializable = SerializeType.ParamsOnly;
		root.OnSignal(didFinishDrinkingMilk, applyEffect).Enter(delegate(Instance smi)
		{
			remainingSecondsForEffect.Set(Mathf.Clamp(remainingSecondsForEffect.Get(smi), 0f, 600f), smi);
		}).ParamTransition(remainingSecondsForEffect, satisfied, (Instance smi, float val) => val > 0f);
		lookingToDrinkMilk.PreBrainUpdate(FindMilkFeederTarget).ToggleBehaviour(GameTags.Creatures.Behaviour_TryToDrinkMilkFromFeeder, (Instance smi) => !smi.targetMilkFeeder.IsNullOrStopped() && !smi.targetMilkFeeder.IsReserved()).Exit(delegate(Instance smi)
		{
			smi.targetMilkFeeder = null;
		});
		applyEffect.Enter(delegate(Instance smi)
		{
			remainingSecondsForEffect.Set(600f, smi);
		}).EnterTransition(satisfied, (Instance smi) => true);
		satisfied.Enter(delegate(Instance smi)
		{
			if (smi.def.consumesMilk)
			{
				smi.GetComponent<Effects>().Add("HadMilk", should_save: false).timeRemaining = remainingSecondsForEffect.Get(smi);
			}
		}).Exit(delegate(Instance smi)
		{
			if (smi.def.consumesMilk)
			{
				smi.GetComponent<Effects>().Remove("HadMilk");
			}
			remainingSecondsForEffect.Set(-1f, smi);
		}).ScheduleGoTo((Instance smi) => remainingSecondsForEffect.Get(smi), lookingToDrinkMilk)
			.Update(delegate(Instance smi, float deltaSeconds)
			{
				remainingSecondsForEffect.Delta(0f - deltaSeconds, smi);
				if (remainingSecondsForEffect.Get(smi) < 0f)
				{
					smi.GoTo(lookingToDrinkMilk);
				}
			}, UpdateRate.SIM_1000ms);
	}

	private static void FindMilkFeederTarget(Instance smi)
	{
		int num = Grid.PosToCell(smi.gameObject);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		List<MilkFeeder.Instance> items = Components.MilkFeeders.GetItems(Grid.WorldIdx[num]);
		if (items == null || items.Count == 0)
		{
			return;
		}
		using ListPool<MilkFeeder.Instance, DrinkMilkMonitor>.PooledList pooledList = PoolsFor<DrinkMilkMonitor>.AllocateList<MilkFeeder.Instance>();
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(num);
		if (cavityForCell != null && cavityForCell.room != null && cavityForCell.room.roomType == Db.Get().RoomTypes.CreaturePen)
		{
			foreach (MilkFeeder.Instance item in items)
			{
				if (!item.IsNullOrDestroyed())
				{
					int cell2 = Grid.PosToCell(item);
					if (Game.Instance.roomProber.GetCavityForCell(cell2) == cavityForCell && item.IsReadyToStartFeeding())
					{
						pooledList.Add(item);
					}
				}
			}
		}
		bool canDrown = smi.drowningMonitor != null && smi.drowningMonitor.canDrownToDeath && !smi.drowningMonitor.livesUnderWater;
		smi.targetMilkFeeder = null;
		smi.doesTargetMilkFeederHaveSpaceForCritter = false;
		int resultCost = -1;
		foreach (MilkFeeder.Instance item2 in pooledList)
		{
			MilkFeeder.Instance milkFeeder = item2;
			if (ConsiderCell(smi.GetDrinkCellOf(milkFeeder, isTwoByTwoCritterCramped: false)))
			{
				smi.doesTargetMilkFeederHaveSpaceForCritter = false;
			}
			else if (ConsiderCell(smi.GetDrinkCellOf(milkFeeder, isTwoByTwoCritterCramped: true)))
			{
				smi.doesTargetMilkFeederHaveSpaceForCritter = true;
			}
			bool ConsiderCell(int cell)
			{
				if (canDrown && !smi.drowningMonitor.IsCellSafe(cell))
				{
					return false;
				}
				int navigationCost = smi.navigator.GetNavigationCost(cell);
				if (navigationCost == -1)
				{
					return false;
				}
				if (navigationCost < resultCost || resultCost == -1)
				{
					resultCost = navigationCost;
					smi.targetMilkFeeder = milkFeeder;
					return true;
				}
				return false;
			}
		}
	}
}
