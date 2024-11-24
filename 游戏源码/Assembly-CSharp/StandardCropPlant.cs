using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001700 RID: 5888
public class StandardCropPlant : StateMachineComponent<StandardCropPlant.StatesInstance>
{
	// Token: 0x0600792C RID: 31020 RVA: 0x000EFB55 File Offset: 0x000EDD55
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0600792D RID: 31021 RVA: 0x000EE9FE File Offset: 0x000ECBFE
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x0600792E RID: 31022 RVA: 0x00313698 File Offset: 0x00311898
	public Notification CreateDeathNotification()
	{
		return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + base.gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x0600792F RID: 31023 RVA: 0x000EFB68 File Offset: 0x000EDD68
	public void RefreshPositionPercent()
	{
		this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
	}

	// Token: 0x06007930 RID: 31024 RVA: 0x003136F8 File Offset: 0x003118F8
	private static string ToolTipResolver(List<Notification> notificationList, object data)
	{
		string text = "";
		for (int i = 0; i < notificationList.Count; i++)
		{
			Notification notification = notificationList[i];
			text += (string)notification.tooltipData;
			if (i < notificationList.Count - 1)
			{
				text += "\n";
			}
		}
		return string.Format(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP, text);
	}

	// Token: 0x04005AC9 RID: 23241
	private const int WILT_LEVELS = 3;

	// Token: 0x04005ACA RID: 23242
	[MyCmpReq]
	private Crop crop;

	// Token: 0x04005ACB RID: 23243
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04005ACC RID: 23244
	[MyCmpReq]
	private ReceptacleMonitor rm;

	// Token: 0x04005ACD RID: 23245
	[MyCmpReq]
	private Growing growing;

	// Token: 0x04005ACE RID: 23246
	[MyCmpReq]
	private KAnimControllerBase animController;

	// Token: 0x04005ACF RID: 23247
	[MyCmpGet]
	private Harvestable harvestable;

	// Token: 0x04005AD0 RID: 23248
	public bool wiltsOnReadyToHarvest;

	// Token: 0x04005AD1 RID: 23249
	public static StandardCropPlant.AnimSet defaultAnimSet = new StandardCropPlant.AnimSet
	{
		grow = "grow",
		grow_pst = "grow_pst",
		idle_full = "idle_full",
		wilt_base = "wilt",
		harvest = "harvest",
		waning = "waning"
	};

	// Token: 0x04005AD2 RID: 23250
	public StandardCropPlant.AnimSet anims = StandardCropPlant.defaultAnimSet;

	// Token: 0x02001701 RID: 5889
	public class AnimSet
	{
		// Token: 0x06007933 RID: 31027 RVA: 0x003137BC File Offset: 0x003119BC
		public string GetWiltLevel(int level)
		{
			if (this.m_wilt == null)
			{
				this.m_wilt = new string[3];
				for (int i = 0; i < 3; i++)
				{
					this.m_wilt[i] = this.wilt_base + (i + 1).ToString();
				}
			}
			return this.m_wilt[level - 1];
		}

		// Token: 0x04005AD3 RID: 23251
		public string grow;

		// Token: 0x04005AD4 RID: 23252
		public string grow_pst;

		// Token: 0x04005AD5 RID: 23253
		public string idle_full;

		// Token: 0x04005AD6 RID: 23254
		public string wilt_base;

		// Token: 0x04005AD7 RID: 23255
		public string harvest;

		// Token: 0x04005AD8 RID: 23256
		public string waning;

		// Token: 0x04005AD9 RID: 23257
		private string[] m_wilt;
	}

	// Token: 0x02001702 RID: 5890
	public class States : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant>
	{
		// Token: 0x06007935 RID: 31029 RVA: 0x00313814 File Offset: 0x00311A14
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.alive;
			this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead, null).Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master.rm.Replanted && !smi.master.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
				{
					Notifier notifier = smi.master.gameObject.AddOrGet<Notifier>();
					Notification notification = smi.master.CreateDeathNotification();
					notifier.Add(notification, "");
				}
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				Harvestable component = smi.master.GetComponent<Harvestable>();
				if (component != null && component.CanBeHarvested && GameScheduler.Instance != null)
				{
					GameScheduler.Instance.Schedule("SpawnFruit", 0.2f, new Action<object>(smi.master.crop.SpawnConfiguredFruit), null, null);
				}
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blighted.InitializeStates(this.masterTarget, this.dead).PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.waning, KAnim.PlayMode.Once).ToggleMainStatusItem(Db.Get().CreatureStatusItems.Crop_Blighted, null).TagTransition(GameTags.Blighted, this.alive, true);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.idle).ToggleComponent<Growing>(false).TagTransition(GameTags.Blighted, this.blighted, false);
			this.alive.idle.EventTransition(GameHashes.Wilt, this.alive.wilting, (StandardCropPlant.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.Grow, this.alive.pre_fruiting, (StandardCropPlant.StatesInstance smi) => smi.master.growing.ReachedNextHarvest()).EventTransition(GameHashes.CropSleep, this.alive.sleeping, new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback(this.IsSleeping)).PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.grow, KAnim.PlayMode.Paused).Enter(new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback(StandardCropPlant.States.RefreshPositionPercent)).Update(new Action<StandardCropPlant.StatesInstance, float>(StandardCropPlant.States.RefreshPositionPercent), UpdateRate.SIM_4000ms, false).EventHandler(GameHashes.ConsumePlant, new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State.Callback(StandardCropPlant.States.RefreshPositionPercent));
			this.alive.pre_fruiting.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.grow_pst, KAnim.PlayMode.Once).TriggerOnEnter(GameHashes.BurstEmitDisease, null).EventTransition(GameHashes.AnimQueueComplete, this.alive.fruiting, null).EventTransition(GameHashes.Wilt, this.alive.wilting, null).ScheduleGoTo(2f, this.alive.fruiting);
			this.alive.fruiting_lost.Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(false);
				}
			}).GoTo(this.alive.idle);
			this.alive.wilting.PlayAnim(new Func<StandardCropPlant.StatesInstance, string>(StandardCropPlant.States.GetWiltAnim), KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.idle, (StandardCropPlant.StatesInstance smi) => !smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.Harvest, this.alive.harvest, null);
			this.alive.sleeping.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.grow, KAnim.PlayMode.Once).EventTransition(GameHashes.CropWakeUp, this.alive.idle, GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Not(new StateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.Transition.ConditionCallback(this.IsSleeping))).EventTransition(GameHashes.Harvest, this.alive.harvest, null).EventTransition(GameHashes.Wilt, this.alive.wilting, null);
			this.alive.fruiting.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.idle_full, KAnim.PlayMode.Loop).Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(true);
				}
			}).EventHandlerTransition(GameHashes.Wilt, this.alive.wilting, (StandardCropPlant.StatesInstance smi, object obj) => smi.master.wiltsOnReadyToHarvest).EventTransition(GameHashes.Harvest, this.alive.harvest, null).EventTransition(GameHashes.Grow, this.alive.fruiting_lost, (StandardCropPlant.StatesInstance smi) => !smi.master.growing.ReachedNextHarvest());
			this.alive.harvest.PlayAnim((StandardCropPlant.StatesInstance smi) => smi.master.anims.harvest, KAnim.PlayMode.Once).Enter(delegate(StandardCropPlant.StatesInstance smi)
			{
				if (smi.master != null)
				{
					smi.master.crop.SpawnConfiguredFruit(null);
				}
				if (smi.master.harvestable != null)
				{
					smi.master.harvestable.SetCanBeHarvested(false);
				}
			}).Exit(delegate(StandardCropPlant.StatesInstance smi)
			{
				smi.Trigger(113170146, null);
			}).OnAnimQueueComplete(this.alive.idle);
		}

		// Token: 0x06007936 RID: 31030 RVA: 0x00313D0C File Offset: 0x00311F0C
		private static string GetWiltAnim(StandardCropPlant.StatesInstance smi)
		{
			float num = smi.master.growing.PercentOfCurrentHarvest();
			int level;
			if (num < 0.75f)
			{
				level = 1;
			}
			else if (num < 1f)
			{
				level = 2;
			}
			else
			{
				level = 3;
			}
			return smi.master.anims.GetWiltLevel(level);
		}

		// Token: 0x06007937 RID: 31031 RVA: 0x000EFB93 File Offset: 0x000EDD93
		private static void RefreshPositionPercent(StandardCropPlant.StatesInstance smi, float dt)
		{
			smi.master.RefreshPositionPercent();
		}

		// Token: 0x06007938 RID: 31032 RVA: 0x000EFB93 File Offset: 0x000EDD93
		private static void RefreshPositionPercent(StandardCropPlant.StatesInstance smi)
		{
			smi.master.RefreshPositionPercent();
		}

		// Token: 0x06007939 RID: 31033 RVA: 0x00313D58 File Offset: 0x00311F58
		public bool IsSleeping(StandardCropPlant.StatesInstance smi)
		{
			CropSleepingMonitor.Instance smi2 = smi.master.GetSMI<CropSleepingMonitor.Instance>();
			return smi2 != null && smi2.IsSleeping();
		}

		// Token: 0x04005ADA RID: 23258
		public StandardCropPlant.States.AliveStates alive;

		// Token: 0x04005ADB RID: 23259
		public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State dead;

		// Token: 0x04005ADC RID: 23260
		public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.PlantAliveSubState blighted;

		// Token: 0x02001703 RID: 5891
		public class AliveStates : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.PlantAliveSubState
		{
			// Token: 0x04005ADD RID: 23261
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State idle;

			// Token: 0x04005ADE RID: 23262
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State pre_fruiting;

			// Token: 0x04005ADF RID: 23263
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State fruiting_lost;

			// Token: 0x04005AE0 RID: 23264
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State barren;

			// Token: 0x04005AE1 RID: 23265
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State fruiting;

			// Token: 0x04005AE2 RID: 23266
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State wilting;

			// Token: 0x04005AE3 RID: 23267
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State destroy;

			// Token: 0x04005AE4 RID: 23268
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State harvest;

			// Token: 0x04005AE5 RID: 23269
			public GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.State sleeping;
		}
	}

	// Token: 0x02001705 RID: 5893
	public class StatesInstance : GameStateMachine<StandardCropPlant.States, StandardCropPlant.StatesInstance, StandardCropPlant, object>.GameInstance
	{
		// Token: 0x0600794E RID: 31054 RVA: 0x000EFCBD File Offset: 0x000EDEBD
		public StatesInstance(StandardCropPlant master) : base(master)
		{
		}
	}
}
