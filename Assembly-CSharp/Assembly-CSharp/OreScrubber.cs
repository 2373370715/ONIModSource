﻿using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

public class OreScrubber : StateMachineComponent<OreScrubber.SMInstance>, IGameObjectEffectDescriptor
{
		protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.FindOrAddComponent<Workable>();
	}

		private void RefreshMeters()
	{
		float positionPercent = 0f;
		PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(this.consumedElement);
		if (primaryElement != null)
		{
			positionPercent = Mathf.Clamp01(primaryElement.Mass / base.GetComponent<ConduitConsumer>().capacityKG);
		}
		this.cleanMeter.SetPositionPercent(positionPercent);
	}

		protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.cleanMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_clean_target", "meter_clean", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_clean_target"
		});
		this.RefreshMeters();
		base.Subscribe<OreScrubber>(-1697596308, OreScrubber.OnStorageChangeDelegate);
		DirectionControl component = base.GetComponent<DirectionControl>();
		component.onDirectionChanged = (Action<WorkableReactable.AllowedDirection>)Delegate.Combine(component.onDirectionChanged, new Action<WorkableReactable.AllowedDirection>(this.OnDirectionChanged));
		this.OnDirectionChanged(base.GetComponent<DirectionControl>().allowedDirection);
	}

		private void OnDirectionChanged(WorkableReactable.AllowedDirection allowed_direction)
	{
		if (this.reactable != null)
		{
			this.reactable.allowedDirection = allowed_direction;
		}
	}

		public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string name = ElementLoader.FindElementByHash(this.consumedElement).name;
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		return list;
	}

		public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.outputElement != SimHashes.Vacuum)
		{
			list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTEDPERUSE, ElementLoader.FindElementByHash(this.outputElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTEDPERUSE, ElementLoader.FindElementByHash(this.outputElement).name, GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Effect, false));
		}
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.DISEASECONSUMEDPERUSE, GameUtil.GetFormattedDiseaseAmount(this.diseaseRemovalCount, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DISEASECONSUMEDPERUSE, GameUtil.GetFormattedDiseaseAmount(this.diseaseRemovalCount, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
		return list;
	}

		public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.AddRange(this.RequirementDescriptors());
		list.AddRange(this.EffectDescriptors());
		return list;
	}

		private void OnStorageChange(object data)
	{
		this.RefreshMeters();
	}

		private static PrimaryElement GetFirstInfected(Storage storage)
	{
		foreach (GameObject gameObject in storage.items)
		{
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (component.DiseaseIdx != 255 && !gameObject.HasTag(GameTags.Edible))
				{
					return component;
				}
			}
		}
		return null;
	}

		public float massConsumedPerUse = 1f;

		public SimHashes consumedElement = SimHashes.BleachStone;

		public int diseaseRemovalCount = 10000;

		public SimHashes outputElement = SimHashes.Vacuum;

		private WorkableReactable reactable;

		private MeterController cleanMeter;

		[Serialize]
	public int maxPossiblyRemoved;

		private static readonly EventSystem.IntraObjectHandler<OreScrubber> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<OreScrubber>(delegate(OreScrubber component, object data)
	{
		component.OnStorageChange(data);
	});

		private class ScrubOreReactable : WorkableReactable
	{
				public ScrubOreReactable(Workable workable, ChoreType chore_type, WorkableReactable.AllowedDirection allowed_direction = WorkableReactable.AllowedDirection.Any) : base(workable, "ScrubOre", chore_type, allowed_direction)
		{
		}

				public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (base.InternalCanBegin(new_reactor, transition))
			{
				Storage component = new_reactor.GetComponent<Storage>();
				if (component != null && OreScrubber.GetFirstInfected(component) != null)
				{
					return true;
				}
			}
			return false;
		}
	}

		public class SMInstance : GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.GameInstance
	{
				public SMInstance(OreScrubber master) : base(master)
		{
		}

				public bool HasSufficientMass()
		{
			bool result = false;
			PrimaryElement primaryElement = base.GetComponent<Storage>().FindPrimaryElement(base.master.consumedElement);
			if (primaryElement != null)
			{
				result = (primaryElement.Mass > 0f);
			}
			return result;
		}

				public Dictionary<Tag, float> GetNeededMass()
		{
			return new Dictionary<Tag, float>
			{
				{
					base.master.consumedElement.CreateTag(),
					base.master.massConsumedPerUse
				}
			};
		}

				public void OnCompleteWork(WorkerBase worker)
		{
		}

				public void DumpOutput()
		{
			Storage component = base.master.GetComponent<Storage>();
			if (base.master.outputElement != SimHashes.Vacuum)
			{
				component.Drop(ElementLoader.FindElementByHash(base.master.outputElement).tag);
			}
		}
	}

		public class States : GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber>
	{
				public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.notready;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.notoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.notready, false);
			this.notready.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.ready, (OreScrubber.SMInstance smi) => smi.HasSufficientMass()).ToggleStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailable, (OreScrubber.SMInstance smi) => smi.GetNeededMass()).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.DefaultState(this.ready.free).ToggleReactable((OreScrubber.SMInstance smi) => smi.master.reactable = new OreScrubber.ScrubOreReactable(smi.master.GetComponent<OreScrubber.Work>(), Db.Get().ChoreTypes.ScrubOre, smi.master.GetComponent<DirectionControl>().allowedDirection)).EventTransition(GameHashes.OnStorageChange, this.notready, (OreScrubber.SMInstance smi) => !smi.HasSufficientMass()).TagTransition(GameTags.Operational, this.notoperational, true);
			this.ready.free.PlayAnim("on").WorkableStartTransition((OreScrubber.SMInstance smi) => smi.GetComponent<OreScrubber.Work>(), this.ready.occupied);
			this.ready.occupied.PlayAnim("working_pre").QueueAnim("working_loop", true, null).WorkableStopTransition((OreScrubber.SMInstance smi) => smi.GetComponent<OreScrubber.Work>(), this.ready);
		}

				public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State notready;

				public OreScrubber.States.ReadyStates ready;

				public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State notoperational;

				public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State full;

				public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State empty;

				public class ReadyStates : GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State
		{
						public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State free;

						public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State occupied;
		}
	}

		[AddComponentMenu("KMonoBehaviour/Workable/Work")]
	public class Work : Workable, IGameObjectEffectDescriptor
	{
				protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.resetProgressOnStop = true;
			this.shouldTransferDiseaseWithWorker = false;
		}

				protected override void OnStartWork(WorkerBase worker)
		{
			base.OnStartWork(worker);
			this.diseaseRemoved = 0;
		}

				protected override bool OnWorkTick(WorkerBase worker, float dt)
		{
			base.OnWorkTick(worker, dt);
			OreScrubber component = base.GetComponent<OreScrubber>();
			Storage component2 = base.GetComponent<Storage>();
			PrimaryElement firstInfected = OreScrubber.GetFirstInfected(worker.GetComponent<Storage>());
			int num = 0;
			SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
			if (firstInfected != null)
			{
				num = Math.Min((int)(dt / this.workTime * (float)component.diseaseRemovalCount), firstInfected.DiseaseCount);
				this.diseaseRemoved += num;
				invalid.idx = firstInfected.DiseaseIdx;
				invalid.count = num;
				firstInfected.ModifyDiseaseCount(-num, "OreScrubber.OnWorkTick");
			}
			component.maxPossiblyRemoved += num;
			float amount = component.massConsumedPerUse * dt / this.workTime;
			SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;
			float mass;
			float temperature;
			component2.ConsumeAndGetDisease(ElementLoader.FindElementByHash(component.consumedElement).tag, amount, out mass, out diseaseInfo, out temperature);
			if (component.outputElement != SimHashes.Vacuum)
			{
				diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(invalid, diseaseInfo);
				component2.AddLiquid(component.outputElement, mass, temperature, diseaseInfo.idx, diseaseInfo.count, false, true);
			}
			return this.diseaseRemoved > component.diseaseRemovalCount;
		}

				protected override void OnCompleteWork(WorkerBase worker)
		{
			base.OnCompleteWork(worker);
		}

				private int diseaseRemoved;
	}
}
