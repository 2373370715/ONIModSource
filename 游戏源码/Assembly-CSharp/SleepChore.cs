using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000732 RID: 1842
public class SleepChore : Chore<SleepChore.StatesInstance>
{
	// Token: 0x060020D2 RID: 8402 RVA: 0x001BD48C File Offset: 0x001BB68C
	public static void DisplayCustomStatusItemsWhenAsleep(SleepChore.StatesInstance smi)
	{
		if (smi.optional_StatusItemsDisplayedWhileAsleep == null)
		{
			return;
		}
		KSelectable component = smi.gameObject.GetComponent<KSelectable>();
		for (int i = 0; i < smi.optional_StatusItemsDisplayedWhileAsleep.Length; i++)
		{
			StatusItem status_item = smi.optional_StatusItemsDisplayedWhileAsleep[i];
			component.AddStatusItem(status_item, null);
		}
	}

	// Token: 0x060020D3 RID: 8403 RVA: 0x001BD4D4 File Offset: 0x001BB6D4
	public static void RemoveCustomStatusItemsWhenAsleep(SleepChore.StatesInstance smi)
	{
		if (smi.optional_StatusItemsDisplayedWhileAsleep == null)
		{
			return;
		}
		KSelectable component = smi.gameObject.GetComponent<KSelectable>();
		for (int i = 0; i < smi.optional_StatusItemsDisplayedWhileAsleep.Length; i++)
		{
			StatusItem status_item = smi.optional_StatusItemsDisplayedWhileAsleep[i];
			component.RemoveStatusItem(status_item, false);
		}
	}

	// Token: 0x060020D4 RID: 8404 RVA: 0x000B5821 File Offset: 0x000B3A21
	public SleepChore(ChoreType choreType, IStateMachineTarget target, GameObject bed, bool bedIsLocator, bool isInterruptable) : this(choreType, target, bed, bedIsLocator, isInterruptable, null)
	{
	}

	// Token: 0x060020D5 RID: 8405 RVA: 0x001BD51C File Offset: 0x001BB71C
	public SleepChore(ChoreType choreType, IStateMachineTarget target, GameObject bed, bool bedIsLocator, bool isInterruptable, StatusItem[] optional_StatusItemsDisplayedWhileAsleep) : base(choreType, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		base.smi = new SleepChore.StatesInstance(this, target.gameObject, bed, bedIsLocator, isInterruptable);
		base.smi.optional_StatusItemsDisplayedWhileAsleep = optional_StatusItemsDisplayedWhileAsleep;
		if (isInterruptable)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		}
		this.AddPrecondition(SleepChore.IsOkayTimeToSleep, null);
		Operational component = bed.GetComponent<Operational>();
		if (component != null)
		{
			this.AddPrecondition(ChorePreconditions.instance.IsOperational, component);
		}
	}

	// Token: 0x060020D6 RID: 8406 RVA: 0x001BD5AC File Offset: 0x001BB7AC
	public static Sleepable GetSafeFloorLocator(GameObject sleeper)
	{
		int num = sleeper.GetComponent<Sensors>().GetSensor<SafeCellSensor>().GetSleepCellQuery();
		if (num == Grid.InvalidCell)
		{
			num = Grid.PosToCell(sleeper.transform.GetPosition());
		}
		return ChoreHelpers.CreateSleepLocator(Grid.CellToPosCBC(num, Grid.SceneLayer.Move)).GetComponent<Sleepable>();
	}

	// Token: 0x060020D7 RID: 8407 RVA: 0x000B5831 File Offset: 0x000B3A31
	public static bool IsDarkAtCell(int cell)
	{
		return Grid.LightIntensity[cell] < DUPLICANTSTATS.STANDARD.Light.LOW_LIGHT;
	}

	// Token: 0x04001577 RID: 5495
	public static readonly Chore.Precondition IsOkayTimeToSleep = new Chore.Precondition
	{
		id = "IsOkayTimeToSleep",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_OKAY_TIME_TO_SLEEP,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Narcolepsy component = context.consumerState.consumer.GetComponent<Narcolepsy>();
			bool flag = component != null && component.IsNarcolepsing();
			StaminaMonitor.Instance smi = context.consumerState.consumer.GetSMI<StaminaMonitor.Instance>();
			bool flag2 = smi != null && smi.NeedsToSleep();
			bool flag3 = ChorePreconditions.instance.IsScheduledTime.fn(ref context, Db.Get().ScheduleBlockTypes.Sleep);
			return flag || flag3 || flag2;
		}
	};

	// Token: 0x02000733 RID: 1843
	public class StatesInstance : GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.GameInstance
	{
		// Token: 0x060020D9 RID: 8409 RVA: 0x001BD648 File Offset: 0x001BB848
		public StatesInstance(SleepChore master, GameObject sleeper, GameObject bed, bool bedIsLocator, bool isInterruptable) : base(master)
		{
			base.sm.sleeper.Set(sleeper, base.smi, false);
			base.sm.isInterruptable.Set(isInterruptable, base.smi, false);
			Traits component = sleeper.GetComponent<Traits>();
			if (component != null)
			{
				base.sm.needsNightLight.Set(component.HasTrait("NightLight"), base.smi, false);
			}
			if (bedIsLocator)
			{
				this.AddLocator(bed);
				return;
			}
			base.sm.bed.Set(bed, base.smi, false);
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x001BD6FC File Offset: 0x001BB8FC
		public void CheckLightLevel()
		{
			GameObject go = base.sm.sleeper.Get(base.smi);
			int cell = Grid.PosToCell(go);
			if (Grid.IsValidCell(cell))
			{
				bool flag = SleepChore.IsDarkAtCell(cell);
				if (base.sm.needsNightLight.Get(base.smi))
				{
					if (flag)
					{
						go.Trigger(-1307593733, null);
						return;
					}
				}
				else if (!flag && !this.IsLoudSleeper() && !this.IsGlowStick())
				{
					go.Trigger(-1063113160, null);
				}
			}
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x001BD780 File Offset: 0x001BB980
		public void CheckTemperature()
		{
			GameObject go = base.sm.sleeper.Get(base.smi);
			if (go.GetSMI<ExternalTemperatureMonitor.Instance>().IsTooCold())
			{
				go.Trigger(157165762, null);
			}
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x000B584F File Offset: 0x000B3A4F
		public bool IsLoudSleeper()
		{
			return base.sm.sleeper.Get(base.smi).GetComponent<Snorer>() != null;
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x000B5877 File Offset: 0x000B3A77
		public bool IsGlowStick()
		{
			return base.sm.sleeper.Get(base.smi).GetComponent<GlowStick>() != null;
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x000A5E40 File Offset: 0x000A4040
		public void EvaluateSleepQuality()
		{
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x001BD7C0 File Offset: 0x001BB9C0
		public void AddLocator(GameObject sleepable)
		{
			this.locator = sleepable;
			int i = Grid.PosToCell(this.locator);
			Grid.Reserved[i] = true;
			base.sm.bed.Set(this.locator, this, false);
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x001BD808 File Offset: 0x001BBA08
		public void DestroyLocator()
		{
			if (this.locator != null)
			{
				Grid.Reserved[Grid.PosToCell(this.locator)] = false;
				ChoreHelpers.DestroyLocator(this.locator);
				base.sm.bed.Set(null, this);
				this.locator = null;
			}
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x001BD860 File Offset: 0x001BBA60
		public void SetAnim()
		{
			Sleepable sleepable = base.sm.bed.Get<Sleepable>(base.smi);
			if (sleepable.GetComponent<Building>() == null)
			{
				NavType currentNavType = base.sm.sleeper.Get<Navigator>(base.smi).CurrentNavType;
				string s;
				if (currentNavType != NavType.Ladder)
				{
					if (currentNavType != NavType.Pole)
					{
						s = "anim_sleep_floor_kanim";
					}
					else
					{
						s = "anim_sleep_pole_kanim";
					}
				}
				else
				{
					s = "anim_sleep_ladder_kanim";
				}
				sleepable.overrideAnims = new KAnimFile[]
				{
					Assets.GetAnim(s)
				};
			}
		}

		// Token: 0x04001578 RID: 5496
		public bool hadPeacefulSleep;

		// Token: 0x04001579 RID: 5497
		public bool hadNormalSleep;

		// Token: 0x0400157A RID: 5498
		public bool hadBadSleep;

		// Token: 0x0400157B RID: 5499
		public bool hadTerribleSleep;

		// Token: 0x0400157C RID: 5500
		public int lastEvaluatedDay = -1;

		// Token: 0x0400157D RID: 5501
		public float wakeUpBuffer = 2f;

		// Token: 0x0400157E RID: 5502
		public string stateChangeNoiseSource;

		// Token: 0x0400157F RID: 5503
		public StatusItem[] optional_StatusItemsDisplayedWhileAsleep;

		// Token: 0x04001580 RID: 5504
		private GameObject locator;
	}

	// Token: 0x02000734 RID: 1844
	public class States : GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore>
	{
		// Token: 0x060020E2 RID: 8418 RVA: 0x001BD8E8 File Offset: 0x001BBAE8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.sleeper);
			this.root.Exit("DestroyLocator", delegate(SleepChore.StatesInstance smi)
			{
				smi.DestroyLocator();
			});
			this.approach.InitializeStates(this.sleeper, this.bed, this.sleep, null, null, null);
			this.sleep.Enter("SetAnims", delegate(SleepChore.StatesInstance smi)
			{
				smi.SetAnim();
			}).DefaultState(this.sleep.normal).ToggleTag(GameTags.Asleep).DoSleep(this.sleeper, this.bed, this.success, null).Toggle("Custom Status Items", new StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State.Callback(SleepChore.DisplayCustomStatusItemsWhenAsleep), new StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State.Callback(SleepChore.RemoveCustomStatusItemsWhenAsleep)).TriggerOnExit(GameHashes.SleepFinished, null).EventHandler(GameHashes.SleepDisturbedByLight, delegate(SleepChore.StatesInstance smi)
			{
				this.isDisturbedByLight.Set(true, smi, false);
			}).EventHandler(GameHashes.SleepDisturbedByNoise, delegate(SleepChore.StatesInstance smi)
			{
				this.isDisturbedByNoise.Set(true, smi, false);
			}).EventHandler(GameHashes.SleepDisturbedByFearOfDark, delegate(SleepChore.StatesInstance smi)
			{
				this.isScaredOfDark.Set(true, smi, false);
			}).EventHandler(GameHashes.SleepDisturbedByMovement, delegate(SleepChore.StatesInstance smi)
			{
				this.isDisturbedByMovement.Set(true, smi, false);
			}).EventHandler(GameHashes.SleepDisturbedByCold, delegate(SleepChore.StatesInstance smi)
			{
				this.isDisturbedByCold.Set(true, smi, false);
			});
			this.sleep.uninterruptable.DoNothing();
			this.sleep.normal.ParamTransition<bool>(this.isInterruptable, this.sleep.uninterruptable, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsFalse).ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.Sleeping, null).QueueAnim("working_loop", true, null).ParamTransition<bool>(this.isDisturbedByNoise, this.sleep.interrupt_noise, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsTrue).ParamTransition<bool>(this.isDisturbedByLight, this.sleep.interrupt_light, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsTrue).ParamTransition<bool>(this.isScaredOfDark, this.sleep.interrupt_scared, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsTrue).ParamTransition<bool>(this.isDisturbedByMovement, this.sleep.interrupt_movement, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsTrue).ParamTransition<bool>(this.isDisturbedByCold, this.sleep.interrupt_cold, GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.IsTrue).Update(delegate(SleepChore.StatesInstance smi, float dt)
			{
				smi.CheckLightLevel();
				smi.CheckTemperature();
			}, UpdateRate.SIM_200ms, false);
			this.sleep.interrupt_scared.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.SleepingInterruptedByFearOfDark, null).QueueAnim("interrupt_afraid", false, null).OnAnimQueueComplete(this.sleep.interrupt_scared_transition);
			this.sleep.interrupt_scared_transition.Enter(delegate(SleepChore.StatesInstance smi)
			{
				if (!smi.master.GetComponent<Effects>().HasEffect(Db.Get().effects.Get("TerribleSleep")))
				{
					smi.master.GetComponent<Effects>().Add(Db.Get().effects.Get("BadSleepAfraidOfDark"), true);
				}
				GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State state = smi.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Sleep) ? this.sleep.normal : this.success;
				this.isScaredOfDark.Set(false, smi, false);
				smi.GoTo(state);
			});
			this.sleep.interrupt_movement.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.SleepingInterruptedByMovement, null).PlayAnim("interrupt_light").OnAnimQueueComplete(this.sleep.interrupt_movement_transition).Enter(delegate(SleepChore.StatesInstance smi)
			{
				GameObject gameObject = smi.sm.bed.Get(smi);
				if (gameObject != null)
				{
					gameObject.Trigger(-717201811, null);
				}
			});
			this.sleep.interrupt_movement_transition.Enter(delegate(SleepChore.StatesInstance smi)
			{
				if (!smi.master.GetComponent<Effects>().HasEffect(Db.Get().effects.Get("TerribleSleep")))
				{
					smi.master.GetComponent<Effects>().Add(Db.Get().effects.Get("BadSleepMovement"), true);
				}
				GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State state = smi.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Sleep) ? this.sleep.normal : this.success;
				this.isDisturbedByMovement.Set(false, smi, false);
				smi.GoTo(state);
			});
			this.sleep.interrupt_cold.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.SleepingInterruptedByCold, null).PlayAnim("interrupt_cold").ToggleThought(Db.Get().Thoughts.Cold, null).OnAnimQueueComplete(this.sleep.interrupt_cold_transition).Enter(delegate(SleepChore.StatesInstance smi)
			{
				GameObject gameObject = smi.sm.bed.Get(smi);
				if (gameObject != null)
				{
					gameObject.Trigger(157165762, null);
				}
			});
			this.sleep.interrupt_cold_transition.Enter(delegate(SleepChore.StatesInstance smi)
			{
				if (!smi.master.GetComponent<Effects>().HasEffect(Db.Get().effects.Get("TerribleSleep")))
				{
					smi.master.GetComponent<Effects>().Add(Db.Get().effects.Get("BadSleepCold"), true);
				}
				GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State state = smi.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Sleep) ? this.sleep.normal : this.success;
				this.isDisturbedByCold.Set(false, smi, false);
				smi.GoTo(state);
			});
			this.sleep.interrupt_noise.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.SleepingInterruptedByNoise, null).QueueAnim("interrupt_light", false, null).OnAnimQueueComplete(this.sleep.interrupt_noise_transition);
			this.sleep.interrupt_noise_transition.Enter(delegate(SleepChore.StatesInstance smi)
			{
				Effects component = smi.master.GetComponent<Effects>();
				component.Add(Db.Get().effects.Get("TerribleSleep"), true);
				if (component.HasEffect(Db.Get().effects.Get("BadSleep")))
				{
					component.Remove(Db.Get().effects.Get("BadSleep"));
				}
				this.isDisturbedByNoise.Set(false, smi, false);
				GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State state = smi.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Sleep) ? this.sleep.normal : this.success;
				smi.GoTo(state);
			});
			this.sleep.interrupt_light.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().DuplicantStatusItems.SleepingInterruptedByLight, null).QueueAnim("interrupt", false, null).OnAnimQueueComplete(this.sleep.interrupt_light_transition);
			this.sleep.interrupt_light_transition.Enter(delegate(SleepChore.StatesInstance smi)
			{
				if (!smi.master.GetComponent<Effects>().HasEffect(Db.Get().effects.Get("TerribleSleep")))
				{
					smi.master.GetComponent<Effects>().Add(Db.Get().effects.Get("BadSleep"), true);
				}
				GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State state = smi.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Sleep) ? this.sleep.normal : this.success;
				this.isDisturbedByLight.Set(false, smi, false);
				smi.GoTo(state);
			});
			this.success.Enter(delegate(SleepChore.StatesInstance smi)
			{
				smi.EvaluateSleepQuality();
			}).ReturnSuccess();
		}

		// Token: 0x04001581 RID: 5505
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.TargetParameter sleeper;

		// Token: 0x04001582 RID: 5506
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.TargetParameter bed;

		// Token: 0x04001583 RID: 5507
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isInterruptable;

		// Token: 0x04001584 RID: 5508
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isDisturbedByNoise;

		// Token: 0x04001585 RID: 5509
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isDisturbedByLight;

		// Token: 0x04001586 RID: 5510
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isDisturbedByMovement;

		// Token: 0x04001587 RID: 5511
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isDisturbedByCold;

		// Token: 0x04001588 RID: 5512
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter isScaredOfDark;

		// Token: 0x04001589 RID: 5513
		public StateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.BoolParameter needsNightLight;

		// Token: 0x0400158A RID: 5514
		public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x0400158B RID: 5515
		public SleepChore.States.SleepStates sleep;

		// Token: 0x0400158C RID: 5516
		public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State success;

		// Token: 0x02000735 RID: 1845
		public class SleepStates : GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State
		{
			// Token: 0x0400158D RID: 5517
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State condition_transition;

			// Token: 0x0400158E RID: 5518
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State condition_transition_pre;

			// Token: 0x0400158F RID: 5519
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State uninterruptable;

			// Token: 0x04001590 RID: 5520
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State normal;

			// Token: 0x04001591 RID: 5521
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_noise;

			// Token: 0x04001592 RID: 5522
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_noise_transition;

			// Token: 0x04001593 RID: 5523
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_light;

			// Token: 0x04001594 RID: 5524
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_light_transition;

			// Token: 0x04001595 RID: 5525
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_scared;

			// Token: 0x04001596 RID: 5526
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_scared_transition;

			// Token: 0x04001597 RID: 5527
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_movement;

			// Token: 0x04001598 RID: 5528
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_movement_transition;

			// Token: 0x04001599 RID: 5529
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_cold;

			// Token: 0x0400159A RID: 5530
			public GameStateMachine<SleepChore.States, SleepChore.StatesInstance, SleepChore, object>.State interrupt_cold_transition;
		}
	}
}
