using System;
using STRINGS;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public class InhaleStates : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>
{
	// Token: 0x06000660 RID: 1632 RVA: 0x0015BA3C File Offset: 0x00159C3C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingtoeat;
		this.root.Enter("SetTarget", delegate(InhaleStates.Instance smi)
		{
			this.targetCell.Set(smi.monitor.targetCell, smi, false);
		});
		this.goingtoeat.MoveTo((InhaleStates.Instance smi) => this.targetCell.Get(smi), this.inhaling, null, false).ToggleMainStatusItem(new Func<InhaleStates.Instance, StatusItem>(InhaleStates.GetMovingStatusItem), null);
		GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State state = this.inhaling.DefaultState(this.inhaling.inhale);
		string name = CREATURES.STATUSITEMS.INHALING.NAME;
		string tooltip = CREATURES.STATUSITEMS.INHALING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.inhaling.inhale.PlayAnim((InhaleStates.Instance smi) => smi.def.inhaleAnimPre, KAnim.PlayMode.Once).QueueAnim((InhaleStates.Instance smi) => smi.def.inhaleAnimLoop, true, null).Enter("ComputeInhaleAmount", delegate(InhaleStates.Instance smi)
		{
			smi.ComputeInhaleAmounts();
		}).Update("Consume", delegate(InhaleStates.Instance smi, float dt)
		{
			smi.monitor.Consume(dt * smi.consumptionMult);
		}, UpdateRate.SIM_200ms, false).EventTransition(GameHashes.ElementNoLongerAvailable, this.inhaling.pst, null).Enter("StartInhaleSound", delegate(InhaleStates.Instance smi)
		{
			smi.StartInhaleSound();
		}).Exit("StopInhaleSound", delegate(InhaleStates.Instance smi)
		{
			smi.StopInhaleSound();
		}).ScheduleGoTo((InhaleStates.Instance smi) => smi.inhaleTime, this.inhaling.pst);
		this.inhaling.pst.Transition(this.inhaling.full, (InhaleStates.Instance smi) => smi.def.alwaysPlayPstAnim || InhaleStates.IsFull(smi), UpdateRate.SIM_200ms).Transition(this.behaviourcomplete, GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Not(new StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.Transition.ConditionCallback(InhaleStates.IsFull)), UpdateRate.SIM_200ms);
		this.inhaling.full.QueueAnim((InhaleStates.Instance smi) => smi.def.inhaleAnimPst, false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete((InhaleStates.Instance smi) => smi.def.behaviourTag, false);
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x000A8E4D File Offset: 0x000A704D
	private static StatusItem GetMovingStatusItem(InhaleStates.Instance smi)
	{
		if (smi.def.useStorage)
		{
			return smi.def.storageStatusItem;
		}
		return Db.Get().CreatureStatusItems.LookingForFood;
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x0015BD08 File Offset: 0x00159F08
	private static bool IsFull(InhaleStates.Instance smi)
	{
		if (smi.def.useStorage)
		{
			if (smi.storage != null)
			{
				return smi.storage.IsFull();
			}
		}
		else
		{
			CreatureCalorieMonitor.Instance smi2 = smi.GetSMI<CreatureCalorieMonitor.Instance>();
			if (smi2 != null)
			{
				return smi2.stomach.GetFullness() >= 1f;
			}
		}
		return false;
	}

	// Token: 0x04000499 RID: 1177
	public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State goingtoeat;

	// Token: 0x0400049A RID: 1178
	public InhaleStates.InhalingStates inhaling;

	// Token: 0x0400049B RID: 1179
	public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State behaviourcomplete;

	// Token: 0x0400049C RID: 1180
	public StateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.IntParameter targetCell;

	// Token: 0x020001D7 RID: 471
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400049D RID: 1181
		public string inhaleSound;

		// Token: 0x0400049E RID: 1182
		public float inhaleTime = 3f;

		// Token: 0x0400049F RID: 1183
		public Tag behaviourTag = GameTags.Creatures.WantsToEat;

		// Token: 0x040004A0 RID: 1184
		public bool useStorage;

		// Token: 0x040004A1 RID: 1185
		public string inhaleAnimPre = "inhale_pre";

		// Token: 0x040004A2 RID: 1186
		public string inhaleAnimLoop = "inhale_loop";

		// Token: 0x040004A3 RID: 1187
		public string inhaleAnimPst = "inhale_pst";

		// Token: 0x040004A4 RID: 1188
		public bool alwaysPlayPstAnim;

		// Token: 0x040004A5 RID: 1189
		public StatusItem storageStatusItem = Db.Get().CreatureStatusItems.LookingForGas;
	}

	// Token: 0x020001D8 RID: 472
	public new class Instance : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.GameInstance
	{
		// Token: 0x06000667 RID: 1639 RVA: 0x000A8EA8 File Offset: 0x000A70A8
		public Instance(Chore<InhaleStates.Instance> chore, InhaleStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, def.behaviourTag);
			this.inhaleSound = GlobalAssets.GetSound(def.inhaleSound, false);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0015BDC0 File Offset: 0x00159FC0
		public void StartInhaleSound()
		{
			LoopingSounds component = base.GetComponent<LoopingSounds>();
			if (component != null && base.smi.inhaleSound != null)
			{
				component.StartSound(base.smi.inhaleSound);
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0015BDFC File Offset: 0x00159FFC
		public void StopInhaleSound()
		{
			LoopingSounds component = base.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(base.smi.inhaleSound);
			}
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0015BE2C File Offset: 0x0015A02C
		public void ComputeInhaleAmounts()
		{
			float num = base.def.inhaleTime;
			this.inhaleTime = num;
			this.consumptionMult = 1f;
			if (!base.def.useStorage && this.monitor.def.diet != null)
			{
				Diet.Info dietInfo = base.smi.monitor.def.diet.GetDietInfo(base.smi.monitor.GetTargetElement().tag);
				if (dietInfo != null)
				{
					CreatureCalorieMonitor.Instance smi = base.smi.gameObject.GetSMI<CreatureCalorieMonitor.Instance>();
					float num2 = Mathf.Clamp01(smi.GetCalories0to1() / 0.9f);
					float num3 = 1f - num2;
					float consumptionRate = base.smi.monitor.def.consumptionRate;
					float num4 = dietInfo.ConvertConsumptionMassToCalories(consumptionRate);
					float num5 = num * num4 + 0.8f * smi.calories.GetMax() * num3 * num3 * num3;
					float num6 = num5 / num4;
					if (num6 > 5f * num)
					{
						this.inhaleTime = 5f * num;
						this.consumptionMult = num5 / (this.inhaleTime * num4);
						return;
					}
					this.inhaleTime = num6;
				}
			}
		}

		// Token: 0x040004A6 RID: 1190
		public string inhaleSound;

		// Token: 0x040004A7 RID: 1191
		public float inhaleTime;

		// Token: 0x040004A8 RID: 1192
		public float consumptionMult;

		// Token: 0x040004A9 RID: 1193
		[MySmiGet]
		public GasAndLiquidConsumerMonitor.Instance monitor;

		// Token: 0x040004AA RID: 1194
		[MyCmpGet]
		public Storage storage;
	}

	// Token: 0x020001D9 RID: 473
	public class InhalingStates : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State
	{
		// Token: 0x040004AB RID: 1195
		public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State inhale;

		// Token: 0x040004AC RID: 1196
		public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State pst;

		// Token: 0x040004AD RID: 1197
		public GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>.State full;
	}
}
