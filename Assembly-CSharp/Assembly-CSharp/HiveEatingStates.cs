﻿using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class HiveEatingStates : GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>
{
		public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.eating;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State state = this.eating;
		string name = CREATURES.STATUSITEMS.HIVE_DIGESTING.NAME;
		string tooltip = CREATURES.STATUSITEMS.HIVE_DIGESTING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).DefaultState(this.eating.pre).Enter(delegate(HiveEatingStates.Instance smi)
		{
			smi.TurnOn();
		}).Exit(delegate(HiveEatingStates.Instance smi)
		{
			smi.TurnOff();
		});
		this.eating.pre.PlayAnim("eating_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.eating.loop);
		this.eating.loop.PlayAnim("eating_loop", KAnim.PlayMode.Loop).Update(delegate(HiveEatingStates.Instance smi, float dt)
		{
			smi.EatOreFromStorage(smi, dt);
		}, UpdateRate.SIM_4000ms, false).EventTransition(GameHashes.OnStorageChange, this.eating.pst, (HiveEatingStates.Instance smi) => !smi.storage.FindFirst(smi.def.consumedOre));
		this.eating.pst.PlayAnim("eating_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToEat, false);
	}

		public HiveEatingStates.EatingStates eating;

		public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State behaviourcomplete;

		public class Def : StateMachine.BaseDef
	{
				public Def(Tag consumedOre)
		{
			this.consumedOre = consumedOre;
		}

				public Tag consumedOre;
	}

		public class EatingStates : GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State
	{
				public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State pre;

				public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State loop;

				public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State pst;
	}

		public new class Instance : GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.GameInstance
	{
				public Instance(Chore<HiveEatingStates.Instance> chore, HiveEatingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToEat);
		}

				public void TurnOn()
		{
			this.emitter.emitRads = 600f * this.emitter.emitRate;
			this.emitter.Refresh();
		}

				public void TurnOff()
		{
			this.emitter.emitRads = 0f;
			this.emitter.Refresh();
		}

				public void EatOreFromStorage(HiveEatingStates.Instance smi, float dt)
		{
			GameObject gameObject = smi.storage.FindFirst(smi.def.consumedOre);
			if (!gameObject)
			{
				return;
			}
			float num = 0.25f;
			KPrefabID component = gameObject.GetComponent<KPrefabID>();
			if (component == null)
			{
				return;
			}
			PrimaryElement component2 = component.GetComponent<PrimaryElement>();
			if (component2 == null)
			{
				return;
			}
			Diet.Info dietInfo = smi.GetSMI<BeehiveCalorieMonitor.Instance>().stomach.diet.GetDietInfo(component.PrefabTag);
			if (dietInfo == null)
			{
				return;
			}
			AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(smi.gameObject);
			float calories = amountInstance.GetMax() - amountInstance.value;
			float num2 = dietInfo.ConvertCaloriesToConsumptionMass(calories);
			float num3 = num * dt;
			if (num2 < num3)
			{
				num3 = num2;
			}
			num3 = Mathf.Min(num3, component2.Mass);
			component2.Mass -= num3;
			Pickupable component3 = component2.GetComponent<Pickupable>();
			if (component3.storage != null)
			{
				component3.storage.Trigger(-1452790913, smi.gameObject);
				component3.storage.Trigger(-1697596308, smi.gameObject);
			}
			float calories2 = dietInfo.ConvertConsumptionMassToCalories(num3);
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = new CreatureCalorieMonitor.CaloriesConsumedEvent
			{
				tag = component.PrefabTag,
				calories = calories2
			};
			smi.gameObject.Trigger(-2038961714, caloriesConsumedEvent);
		}

				[MyCmpReq]
		public Storage storage;

				[MyCmpReq]
		private RadiationEmitter emitter;
	}
}
