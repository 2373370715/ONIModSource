using STRINGS;
using UnityEngine;

public class InhaleStates : GameStateMachine<InhaleStates, InhaleStates.Instance, IStateMachineTarget, InhaleStates.Def>
{
	public class Def : BaseDef
	{
		public string inhaleSound;

		public float inhaleTime = 3f;

		public Tag behaviourTag = GameTags.Creatures.WantsToEat;

		public bool useStorage;

		public string inhaleAnimPre = "inhale_pre";

		public string inhaleAnimLoop = "inhale_loop";

		public string inhaleAnimPst = "inhale_pst";

		public bool alwaysPlayPstAnim;

		public StatusItem storageStatusItem = Db.Get().CreatureStatusItems.LookingForGas;
	}

	public new class Instance : GameInstance
	{
		public string inhaleSound;

		public float inhaleTime;

		public float consumptionMult;

		[MySmiGet]
		public GasAndLiquidConsumerMonitor.Instance monitor;

		[MyCmpGet]
		public Storage storage;

		public Instance(Chore<Instance> chore, Def def)
			: base((IStateMachineTarget)chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, def.behaviourTag);
			inhaleSound = GlobalAssets.GetSound(def.inhaleSound);
		}

		public void StartInhaleSound()
		{
			LoopingSounds component = GetComponent<LoopingSounds>();
			if (component != null && base.smi.inhaleSound != null)
			{
				component.StartSound(base.smi.inhaleSound);
			}
		}

		public void StopInhaleSound()
		{
			LoopingSounds component = GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(base.smi.inhaleSound);
			}
		}

		public void ComputeInhaleAmounts()
		{
			float num = (inhaleTime = base.def.inhaleTime);
			consumptionMult = 1f;
			if (base.def.useStorage || monitor.def.diet == null)
			{
				return;
			}
			Diet.Info dietInfo = base.smi.monitor.def.diet.GetDietInfo(base.smi.monitor.GetTargetElement().tag);
			if (dietInfo != null)
			{
				CreatureCalorieMonitor.Instance sMI = base.smi.gameObject.GetSMI<CreatureCalorieMonitor.Instance>();
				float num2 = Mathf.Clamp01(sMI.GetCalories0to1() / 0.9f);
				float num3 = 1f - num2;
				float consumptionRate = base.smi.monitor.def.consumptionRate;
				float num4 = dietInfo.ConvertConsumptionMassToCalories(consumptionRate);
				float num5 = num * num4 + 0.8f * sMI.calories.GetMax() * num3 * num3 * num3;
				float num6 = num5 / num4;
				if (num6 > 5f * num)
				{
					inhaleTime = 5f * num;
					consumptionMult = num5 / (inhaleTime * num4);
				}
				else
				{
					inhaleTime = num6;
				}
			}
		}
	}

	public class InhalingStates : State
	{
		public State inhale;

		public State pst;

		public State full;
	}

	public State goingtoeat;

	public InhalingStates inhaling;

	public State behaviourcomplete;

	public IntParameter targetCell;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = goingtoeat;
		root.Enter("SetTarget", delegate(Instance smi)
		{
			targetCell.Set(smi.monitor.targetCell, smi);
		});
		goingtoeat.MoveTo((Instance smi) => targetCell.Get(smi), inhaling).ToggleMainStatusItem(GetMovingStatusItem);
		inhaling.DefaultState(inhaling.inhale).ToggleStatusItem(CREATURES.STATUSITEMS.INHALING.NAME, CREATURES.STATUSITEMS.INHALING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		inhaling.inhale.PlayAnim((Instance smi) => smi.def.inhaleAnimPre).QueueAnim((Instance smi) => smi.def.inhaleAnimLoop, loop: true).Enter("ComputeInhaleAmount", delegate(Instance smi)
		{
			smi.ComputeInhaleAmounts();
		})
			.Update("Consume", delegate(Instance smi, float dt)
			{
				smi.monitor.Consume(dt * smi.consumptionMult);
			})
			.EventTransition(GameHashes.ElementNoLongerAvailable, inhaling.pst)
			.Enter("StartInhaleSound", delegate(Instance smi)
			{
				smi.StartInhaleSound();
			})
			.Exit("StopInhaleSound", delegate(Instance smi)
			{
				smi.StopInhaleSound();
			})
			.ScheduleGoTo((Instance smi) => smi.inhaleTime, inhaling.pst);
		inhaling.pst.Transition(inhaling.full, (Instance smi) => smi.def.alwaysPlayPstAnim || IsFull(smi)).Transition(behaviourcomplete, GameStateMachine<InhaleStates, Instance, IStateMachineTarget, Def>.Not(IsFull));
		inhaling.full.QueueAnim((Instance smi) => smi.def.inhaleAnimPst).OnAnimQueueComplete(behaviourcomplete);
		behaviourcomplete.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete((Instance smi) => smi.def.behaviourTag);
	}

	private static StatusItem GetMovingStatusItem(Instance smi)
	{
		if (smi.def.useStorage)
		{
			return smi.def.storageStatusItem;
		}
		return Db.Get().CreatureStatusItems.LookingForFood;
	}

	private static bool IsFull(Instance smi)
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
			CreatureCalorieMonitor.Instance sMI = smi.GetSMI<CreatureCalorieMonitor.Instance>();
			if (sMI != null)
			{
				return sMI.stomach.GetFullness() >= 1f;
			}
		}
		return false;
	}
}
