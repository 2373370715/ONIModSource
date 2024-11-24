using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020001A1 RID: 417
public class HiveEatingStates : GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>
{
	// Token: 0x060005CE RID: 1486 RVA: 0x0015A4F4 File Offset: 0x001586F4
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

	// Token: 0x0400043A RID: 1082
	public HiveEatingStates.EatingStates eating;

	// Token: 0x0400043B RID: 1083
	public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State behaviourcomplete;

	// Token: 0x020001A2 RID: 418
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060005D0 RID: 1488 RVA: 0x000A882A File Offset: 0x000A6A2A
		public Def(Tag consumedOre)
		{
			this.consumedOre = consumedOre;
		}

		// Token: 0x0400043C RID: 1084
		public Tag consumedOre;
	}

	// Token: 0x020001A3 RID: 419
	public class EatingStates : GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State
	{
		// Token: 0x0400043D RID: 1085
		public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State pre;

		// Token: 0x0400043E RID: 1086
		public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State loop;

		// Token: 0x0400043F RID: 1087
		public GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.State pst;
	}

	// Token: 0x020001A4 RID: 420
	public new class Instance : GameStateMachine<HiveEatingStates, HiveEatingStates.Instance, IStateMachineTarget, HiveEatingStates.Def>.GameInstance
	{
		// Token: 0x060005D2 RID: 1490 RVA: 0x000A8841 File Offset: 0x000A6A41
		public Instance(Chore<HiveEatingStates.Instance> chore, HiveEatingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToEat);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x000A8865 File Offset: 0x000A6A65
		public void TurnOn()
		{
			this.emitter.emitRads = 600f * this.emitter.emitRate;
			this.emitter.Refresh();
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x000A888E File Offset: 0x000A6A8E
		public void TurnOff()
		{
			this.emitter.emitRads = 0f;
			this.emitter.Refresh();
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0015A680 File Offset: 0x00158880
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

		// Token: 0x04000440 RID: 1088
		[MyCmpReq]
		public Storage storage;

		// Token: 0x04000441 RID: 1089
		[MyCmpReq]
		private RadiationEmitter emitter;
	}
}
