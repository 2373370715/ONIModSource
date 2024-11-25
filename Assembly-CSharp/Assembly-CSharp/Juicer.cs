﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class Juicer : StateMachineComponent<Juicer.StatesInstance>, IGameObjectEffectDescriptor
{
		protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
	}

		protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

		private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
	{
		string arg = tag.ProperName();
		Descriptor item = default(Descriptor);
		string arg2 = (EdiblesManager.GetFoodInfo(tag.Name) != null) ? GameUtil.GetFormattedCaloriesForItem(tag, mass, GameUtil.TimeSlice.None, true) : GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}");
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, arg2), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, arg2), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

		List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		for (int i = 0; i < this.ingredientTags.Length; i++)
		{
			this.AddRequirementDesc(list, this.ingredientTags[i], this.ingredientMassesPerUse[i]);
		}
		this.AddRequirementDesc(list, GameTags.Water, this.waterMassPerUse);
		return list;
	}

		public string specificEffect;

		public string trackingEffect;

		public Tag[] ingredientTags;

		public float[] ingredientMassesPerUse;

		public float waterMassPerUse;

		public class States : GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer>
	{
				public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false);
			this.operational.PlayAnim("off").TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.ready, new StateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Transition.ConditionCallback(this.IsReady), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Transition.ConditionCallback(this.IsReady));
			this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<Juicer.StatesInstance, Chore>(this.CreateChore), this.operational);
			this.ready.idle.Transition(this.operational, GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Not(new StateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Transition.ConditionCallback(this.IsReady)), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Not(new StateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.Transition.ConditionCallback(this.IsReady))).PlayAnim("on").WorkableStartTransition((Juicer.StatesInstance smi) => smi.master.GetComponent<JuicerWorkable>(), this.ready.working);
			this.ready.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).WorkableStopTransition((Juicer.StatesInstance smi) => smi.master.GetComponent<JuicerWorkable>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

				private Chore CreateChore(Juicer.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<JuicerWorkable>();
			WorkChore<JuicerWorkable> workChore = new WorkChore<JuicerWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

				private bool IsReady(Juicer.StatesInstance smi)
		{
			PrimaryElement primaryElement = smi.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
			if (primaryElement == null)
			{
				return false;
			}
			if (primaryElement.Mass < smi.master.waterMassPerUse)
			{
				return false;
			}
			for (int i = 0; i < smi.master.ingredientTags.Length; i++)
			{
				if (smi.GetComponent<Storage>().GetAmountAvailable(smi.master.ingredientTags[i]) < smi.master.ingredientMassesPerUse[i])
				{
					return false;
				}
			}
			return true;
		}

				private GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State unoperational;

				private GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State operational;

				private Juicer.States.ReadyStates ready;

				public class ReadyStates : GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State
		{
						public GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State idle;

						public GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State working;

						public GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.State post;
		}
	}

		public class StatesInstance : GameStateMachine<Juicer.States, Juicer.StatesInstance, Juicer, object>.GameInstance
	{
				public StatesInstance(Juicer smi) : base(smi)
		{
		}
	}
}
