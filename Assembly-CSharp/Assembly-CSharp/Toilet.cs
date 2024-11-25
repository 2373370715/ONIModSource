﻿using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

public class Toilet : StateMachineComponent<Toilet.StatesInstance>, ISaveLoadable, IUsable, IGameObjectEffectDescriptor, IBasicBuilding
{
				public int FlushesUsed
	{
		get
		{
			return this._flushesUsed;
		}
		set
		{
			this._flushesUsed = value;
			base.smi.sm.flushes.Set(value, base.smi, false);
		}
	}

		protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Toilets.Add(this);
		Components.BasicBuildings.Add(this);
		base.smi.StartSM();
		base.GetComponent<ToiletWorkableUse>().trackUses = true;
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_arrow",
			"meter_scale"
		});
		this.meter.SetPositionPercent((float)this.FlushesUsed / (float)this.maxFlushes);
		this.FlushesUsed = this._flushesUsed;
		base.Subscribe<Toilet>(493375141, Toilet.OnRefreshUserMenuDelegate);
	}

		protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.BasicBuildings.Remove(this);
		Components.Toilets.Remove(this);
	}

		public bool IsUsable()
	{
		return base.smi.HasTag(GameTags.Usable);
	}

		public void Flush(WorkerBase worker)
	{
		this.FlushMultiple(worker, 1);
	}

		public void FlushMultiple(WorkerBase worker, int flushCount)
	{
		int b = this.maxFlushes - this.FlushesUsed;
		int num = Mathf.Min(flushCount, b);
		this.FlushesUsed += num;
		this.meter.SetPositionPercent((float)this.FlushesUsed / (float)this.maxFlushes);
		float num2 = 0f;
		Tag tag = ElementLoader.FindElementByHash(SimHashes.Dirt).tag;
		float num3;
		SimUtil.DiseaseInfo diseaseInfo;
		this.storage.ConsumeAndGetDisease(tag, base.smi.DirtUsedPerFlush() * (float)num, out num3, out diseaseInfo, out num2);
		byte index = Db.Get().Diseases.GetIndex(this.diseaseId);
		int num4 = this.diseasePerFlush * num;
		float mass = base.smi.MassPerFlush() + num3;
		GameObject gameObject = ElementLoader.FindElementByHash(this.solidWastePerUse.elementID).substance.SpawnResource(base.transform.GetPosition(), mass, this.solidWasteTemperature, index, num4, true, false, false);
		gameObject.GetComponent<PrimaryElement>().AddDisease(diseaseInfo.idx, diseaseInfo.count, "Toilet.Flush");
		num4 += diseaseInfo.count;
		this.storage.Store(gameObject, false, false, true, false);
		int num5 = this.diseaseOnDupePerFlush * num;
		worker.GetComponent<PrimaryElement>().AddDisease(index, num5, "Toilet.Flush");
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(DUPLICANTS.DISEASES.ADDED_POPFX, Db.Get().Diseases[(int)index].Name, num4 + num5), base.transform, Vector3.up, 1.5f, false, false);
		Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_LotsOfGerms, true);
	}

		private void OnRefreshUserMenu(object data)
	{
		if (base.smi.GetCurrentState() == base.smi.sm.full || !base.smi.IsSoiled || base.smi.cleanChore != null)
		{
			return;
		}
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("status_item_toilet_needs_emptying", UI.USERMENUACTIONS.CLEANTOILET.NAME, delegate()
		{
			base.smi.GoTo(base.smi.sm.earlyclean);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CLEANTOILET.TOOLTIP, true), 1f);
	}

		private void SpawnMonster()
	{
		GameUtil.KInstantiate(Assets.GetPrefab(new Tag("Glom")), base.smi.transform.GetPosition(), Grid.SceneLayer.Creatures, null, 0).SetActive(true);
	}

		public List<Descriptor> RequirementDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = base.GetComponent<ManualDeliveryKG>().RequestedItemTag.ProperName();
		float mass = base.smi.DirtUsedPerFlush();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		list.Add(item);
		return list;
	}

		public List<Descriptor> EffectDescriptors()
	{
		List<Descriptor> list = new List<Descriptor>();
		string arg = ElementLoader.FindElementByHash(this.solidWastePerUse.elementID).tag.ProperName();
		float mass = base.smi.MassPerFlush() + base.smi.DirtUsedPerFlush();
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTEMITTED_TOILET, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(this.solidWasteTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_TOILET, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), GameUtil.GetFormattedTemperature(this.solidWasteTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false));
		Disease disease = Db.Get().Diseases.Get(this.diseaseId);
		int units = this.diseasePerFlush + this.diseaseOnDupePerFlush;
		list.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.DISEASEEMITTEDPERUSE, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DISEASEEMITTEDPERUSE, disease.Name, GameUtil.GetFormattedDiseaseAmount(units, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.DiseaseSource, false));
		return list;
	}

		public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.AddRange(this.RequirementDescriptors());
		list.AddRange(this.EffectDescriptors());
		return list;
	}

		[SerializeField]
	public Toilet.SpawnInfo solidWastePerUse;

		[SerializeField]
	public float solidWasteTemperature;

		[SerializeField]
	public Toilet.SpawnInfo gasWasteWhenFull;

		[SerializeField]
	public int maxFlushes = 15;

		[SerializeField]
	public string diseaseId;

		[SerializeField]
	public int diseasePerFlush;

		[SerializeField]
	public int diseaseOnDupePerFlush;

		[SerializeField]
	public float dirtUsedPerFlush = 13f;

		[Serialize]
	public int _flushesUsed;

		private MeterController meter;

		[MyCmpReq]
	private Storage storage;

		[MyCmpReq]
	private ManualDeliveryKG manualdeliverykg;

		private static readonly EventSystem.IntraObjectHandler<Toilet> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Toilet>(delegate(Toilet component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

		[Serializable]
	public struct SpawnInfo
	{
				public SpawnInfo(SimHashes element_id, float mass, float interval)
		{
			this.elementID = element_id;
			this.mass = mass;
			this.interval = interval;
		}

				[HashedEnum]
		public SimHashes elementID;

				public float mass;

				public float interval;
	}

		public class StatesInstance : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.GameInstance
	{
				public StatesInstance(Toilet master) : base(master)
		{
			this.activeUseChores = new List<Chore>();
		}

						public bool IsSoiled
		{
			get
			{
				return base.master.FlushesUsed > 0;
			}
		}

				public int GetFlushesRemaining()
		{
			return base.master.maxFlushes - base.master.FlushesUsed;
		}

				public bool RequiresDirtDelivery()
		{
			return base.master.storage.IsEmpty() || !base.master.storage.Has(GameTags.Dirt) || (base.master.storage.GetAmountAvailable(GameTags.Dirt) < base.master.manualdeliverykg.capacity && !this.IsSoiled);
		}

				public float MassPerFlush()
		{
			return base.master.solidWastePerUse.mass;
		}

				public float DirtUsedPerFlush()
		{
			return base.master.dirtUsedPerFlush;
		}

				public bool IsToxicSandRemoved()
		{
			Tag tag = GameTagExtensions.Create(base.master.solidWastePerUse.elementID);
			return base.master.storage.FindFirst(tag) == null;
		}

				public void CreateCleanChore()
		{
			if (this.cleanChore != null)
			{
				this.cleanChore.Cancel("dupe");
			}
			ToiletWorkableClean component = base.master.GetComponent<ToiletWorkableClean>();
			this.cleanChore = new WorkChore<ToiletWorkableClean>(Db.Get().ChoreTypes.CleanToilet, component, null, true, new Action<Chore>(this.OnCleanComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
		}

				public void CancelCleanChore()
		{
			if (this.cleanChore != null)
			{
				this.cleanChore.Cancel("Cancelled");
				this.cleanChore = null;
			}
		}

				private void DropFromStorage(Tag tag)
		{
			ListPool<GameObject, Toilet>.PooledList pooledList = ListPool<GameObject, Toilet>.Allocate();
			base.master.storage.Find(tag, pooledList);
			foreach (GameObject go in pooledList)
			{
				base.master.storage.Drop(go, true);
			}
			pooledList.Recycle();
		}

				private void OnCleanComplete(Chore chore)
		{
			this.cleanChore = null;
			Tag tag = GameTagExtensions.Create(base.master.solidWastePerUse.elementID);
			Tag tag2 = ElementLoader.FindElementByHash(SimHashes.Dirt).tag;
			this.DropFromStorage(tag);
			this.DropFromStorage(tag2);
			base.master.meter.SetPositionPercent((float)base.master.FlushesUsed / (float)base.master.maxFlushes);
		}

				public void Flush()
		{
			WorkerBase worker = base.master.GetComponent<ToiletWorkableUse>().worker;
			base.master.Flush(worker);
		}

				public void FlushAll()
		{
			WorkerBase worker = base.master.GetComponent<ToiletWorkableUse>().worker;
			base.master.FlushMultiple(worker, base.master.maxFlushes - base.master.FlushesUsed);
		}

				public Chore cleanChore;

				public List<Chore> activeUseChores;

				public float monsterSpawnTime = 1200f;
	}

		public class States : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet>
	{
				public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.needsdirt;
			this.root.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.needsdirt, (Toilet.StatesInstance smi) => smi.RequiresDirtDelivery()).EventTransition(GameHashes.OperationalChanged, this.notoperational, (Toilet.StatesInstance smi) => !smi.Get<Operational>().IsOperational);
			this.needsdirt.Enter(delegate(Toilet.StatesInstance smi)
			{
				if (smi.RequiresDirtDelivery())
				{
					smi.master.manualdeliverykg.RequestDelivery();
				}
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null).EventTransition(GameHashes.OnStorageChange, this.ready, (Toilet.StatesInstance smi) => !smi.RequiresDirtDelivery());
			this.ready.ParamTransition<int>(this.flushes, this.full, (Toilet.StatesInstance smi, int p) => smi.GetFlushesRemaining() <= 0).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Toilet, null).ToggleRecurringChore(new Func<Toilet.StatesInstance, Chore>(this.CreateUrgentUseChore), null).ToggleRecurringChore(new Func<Toilet.StatesInstance, Chore>(this.CreateBreakUseChore), null).ToggleTag(GameTags.Usable).EventHandler(GameHashes.Flush, delegate(Toilet.StatesInstance smi, object data)
			{
				smi.Flush();
			}).EventHandler(GameHashes.FlushAll, delegate(Toilet.StatesInstance smi, object data)
			{
				smi.FlushAll();
			});
			this.earlyclean.PlayAnims((Toilet.StatesInstance smi) => Toilet.States.FULL_ANIMS, KAnim.PlayMode.Once).OnAnimQueueComplete(this.earlyWaitingForClean);
			this.earlyWaitingForClean.Enter(delegate(Toilet.StatesInstance smi)
			{
				smi.CreateCleanChore();
			}).Exit(delegate(Toilet.StatesInstance smi)
			{
				smi.CancelCleanChore();
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.ToiletNeedsEmptying, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null).EventTransition(GameHashes.OnStorageChange, this.empty, (Toilet.StatesInstance smi) => smi.IsToxicSandRemoved());
			this.full.PlayAnims((Toilet.StatesInstance smi) => Toilet.States.FULL_ANIMS, KAnim.PlayMode.Once).OnAnimQueueComplete(this.fullWaitingForClean);
			this.fullWaitingForClean.Enter(delegate(Toilet.StatesInstance smi)
			{
				smi.CreateCleanChore();
			}).Exit(delegate(Toilet.StatesInstance smi)
			{
				smi.CancelCleanChore();
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.ToiletNeedsEmptying, null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null).EventTransition(GameHashes.OnStorageChange, this.empty, (Toilet.StatesInstance smi) => smi.IsToxicSandRemoved()).Enter(delegate(Toilet.StatesInstance smi)
			{
				smi.Schedule(smi.monsterSpawnTime, delegate
				{
					smi.master.SpawnMonster();
				}, null);
			});
			this.empty.PlayAnim("off").Enter("ClearFlushes", delegate(Toilet.StatesInstance smi)
			{
				smi.master.FlushesUsed = 0;
			}).GoTo(this.needsdirt);
			this.notoperational.EventTransition(GameHashes.OperationalChanged, this.needsdirt, (Toilet.StatesInstance smi) => smi.Get<Operational>().IsOperational).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable, null);
		}

				private Chore CreateUrgentUseChore(Toilet.StatesInstance smi)
		{
			Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.Pee);
			chore.AddPrecondition(ChorePreconditions.instance.IsBladderFull, null);
			chore.AddPrecondition(ChorePreconditions.instance.NotCurrentlyPeeing, null);
			return chore;
		}

				private Chore CreateBreakUseChore(Toilet.StatesInstance smi)
		{
			Chore chore = this.CreateUseChore(smi, Db.Get().ChoreTypes.BreakPee);
			chore.AddPrecondition(ChorePreconditions.instance.IsBladderNotFull, null);
			chore.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Hygiene);
			return chore;
		}

				private Chore CreateUseChore(Toilet.StatesInstance smi, ChoreType choreType)
		{
			WorkChore<ToiletWorkableUse> workChore = new WorkChore<ToiletWorkableUse>(choreType, smi.master, null, true, null, null, null, false, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
			smi.activeUseChores.Add(workChore);
			WorkChore<ToiletWorkableUse> workChore2 = workChore;
			workChore2.onExit = (Action<Chore>)Delegate.Combine(workChore2.onExit, new Action<Chore>(delegate(Chore exiting_chore)
			{
				smi.activeUseChores.Remove(exiting_chore);
			}));
			workChore.AddPrecondition(ChorePreconditions.instance.IsPreferredAssignableOrUrgentBladder, smi.master.GetComponent<Assignable>());
			workChore.AddPrecondition(ChorePreconditions.instance.IsExclusivelyAvailableWithOtherChores, smi.activeUseChores);
			return workChore;
		}

				public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State needsdirt;

				public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State empty;

				public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State notoperational;

				public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State ready;

				public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State earlyclean;

				public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State earlyWaitingForClean;

				public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State full;

				public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State fullWaitingForClean;

				private static readonly HashedString[] FULL_ANIMS = new HashedString[]
		{
			"full_pre",
			"full"
		};

				public StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.IntParameter flushes = new StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.IntParameter(0);

				public class ReadyStates : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State
		{
						public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State idle;

						public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State inuse;

						public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State flush;
		}
	}
}
