﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class SodaFountain : StateMachineComponent<SodaFountain.StatesInstance>, IGameObjectEffectDescriptor
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
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		this.AddRequirementDesc(list, this.ingredientTag, this.ingredientMassPerUse);
		this.AddRequirementDesc(list, GameTags.Water, this.waterMassPerUse);
		return list;
	}

	public string specificEffect;

	public string trackingEffect;

	public Tag ingredientTag;

	public float ingredientMassPerUse;

	public float waterMassPerUse;

	public class States : GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain>
	{
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false);
			this.operational.PlayAnim("off").TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.ready, new StateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Transition.ConditionCallback(this.IsReady), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.ready, new StateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Transition.ConditionCallback(this.IsReady));
			this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<SodaFountain.StatesInstance, Chore>(this.CreateChore), this.operational);
			this.ready.idle.Transition(this.operational, GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Not(new StateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Transition.ConditionCallback(this.IsReady)), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.operational, GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Not(new StateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.Transition.ConditionCallback(this.IsReady))).WorkableStartTransition((SodaFountain.StatesInstance smi) => smi.master.GetComponent<SodaFountainWorkable>(), this.ready.working);
			this.ready.working.PlayAnim("working_pre").WorkableStopTransition((SodaFountain.StatesInstance smi) => smi.master.GetComponent<SodaFountainWorkable>(), this.ready.post);
			this.ready.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready);
		}

		private Chore CreateChore(SodaFountain.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<SodaFountainWorkable>();
			WorkChore<SodaFountainWorkable> workChore = new WorkChore<SodaFountainWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		private bool IsReady(SodaFountain.StatesInstance smi)
		{
			PrimaryElement primaryElement = smi.GetComponent<Storage>().FindPrimaryElement(SimHashes.Water);
			return !(primaryElement == null) && primaryElement.Mass >= smi.master.waterMassPerUse && smi.GetComponent<Storage>().GetAmountAvailable(smi.master.ingredientTag) >= smi.master.ingredientMassPerUse;
		}

		private GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State unoperational;

		private GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State operational;

		private SodaFountain.States.ReadyStates ready;

		public class ReadyStates : GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State
		{
			public GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State idle;

			public GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State working;

			public GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.State post;
		}
	}

	public class StatesInstance : GameStateMachine<SodaFountain.States, SodaFountain.StatesInstance, SodaFountain, object>.GameInstance
	{
		public StatesInstance(SodaFountain smi) : base(smi)
		{
		}
	}
}
