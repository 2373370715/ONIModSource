using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020015A4 RID: 5540
public class MilkProductionMonitor : GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>
{
	// Token: 0x060072FE RID: 29438 RVA: 0x002FF300 File Offset: 0x002FD500
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.producing;
		this.producing.DefaultState(this.producing.paused).EventHandler(GameHashes.CaloriesConsumed, delegate(MilkProductionMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		});
		this.producing.paused.Transition(this.producing.full, new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsFull), UpdateRate.SIM_1000ms).Transition(this.producing.producing, new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsProducing), UpdateRate.SIM_1000ms);
		this.producing.producing.Transition(this.producing.full, new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsFull), UpdateRate.SIM_1000ms).Transition(this.producing.paused, GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Not(new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsProducing)), UpdateRate.SIM_1000ms);
		this.producing.full.ToggleStatusItem(Db.Get().CreatureStatusItems.MilkFull, null).Transition(this.producing.paused, GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Not(new StateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.Transition.ConditionCallback(MilkProductionMonitor.IsFull)), UpdateRate.SIM_1000ms).Enter(delegate(MilkProductionMonitor.Instance smi)
		{
			smi.gameObject.AddTag(GameTags.Creatures.RequiresMilking);
		});
	}

	// Token: 0x060072FF RID: 29439 RVA: 0x000EB62C File Offset: 0x000E982C
	private static bool IsProducing(MilkProductionMonitor.Instance smi)
	{
		return !smi.IsFull && smi.IsUnderProductionEffect;
	}

	// Token: 0x06007300 RID: 29440 RVA: 0x000EB63E File Offset: 0x000E983E
	private static bool IsFull(MilkProductionMonitor.Instance smi)
	{
		return smi.IsFull;
	}

	// Token: 0x06007301 RID: 29441 RVA: 0x000EB646 File Offset: 0x000E9846
	private static bool HasCapacity(MilkProductionMonitor.Instance smi)
	{
		return !smi.IsFull;
	}

	// Token: 0x0400560C RID: 22028
	public MilkProductionMonitor.ProducingStates producing;

	// Token: 0x020015A5 RID: 5541
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06007303 RID: 29443 RVA: 0x000EB659 File Offset: 0x000E9859
		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.MilkProduction.Id);
		}

		// Token: 0x0400560D RID: 22029
		public const SimHashes element = SimHashes.Milk;

		// Token: 0x0400560E RID: 22030
		public string effectId;

		// Token: 0x0400560F RID: 22031
		public float Capacity = 200f;

		// Token: 0x04005610 RID: 22032
		public float CaloriesPerCycle = 1000f;

		// Token: 0x04005611 RID: 22033
		public float HappinessRequired;
	}

	// Token: 0x020015A6 RID: 5542
	public class ProducingStates : GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State
	{
		// Token: 0x04005612 RID: 22034
		public GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State paused;

		// Token: 0x04005613 RID: 22035
		public GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State producing;

		// Token: 0x04005614 RID: 22036
		public GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.State full;
	}

	// Token: 0x020015A7 RID: 5543
	public new class Instance : GameStateMachine<MilkProductionMonitor, MilkProductionMonitor.Instance, IStateMachineTarget, MilkProductionMonitor.Def>.GameInstance
	{
		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06007306 RID: 29446 RVA: 0x000EB6A5 File Offset: 0x000E98A5
		public float MilkAmount
		{
			get
			{
				return this.MilkPercentage / 100f * base.def.Capacity;
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06007307 RID: 29447 RVA: 0x000EB6BF File Offset: 0x000E98BF
		public float MilkPercentage
		{
			get
			{
				return this.milkAmountInstance.value;
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06007308 RID: 29448 RVA: 0x000EB6CC File Offset: 0x000E98CC
		public bool IsFull
		{
			get
			{
				return this.MilkPercentage >= this.milkAmountInstance.GetMax();
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06007309 RID: 29449 RVA: 0x000EB6E4 File Offset: 0x000E98E4
		public bool IsUnderProductionEffect
		{
			get
			{
				return this.milkAmountInstance.GetDelta() > 0f;
			}
		}

		// Token: 0x0600730A RID: 29450 RVA: 0x000EB6F8 File Offset: 0x000E98F8
		public Instance(IStateMachineTarget master, MilkProductionMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x0600730B RID: 29451 RVA: 0x002FF454 File Offset: 0x002FD654
		public override void StartSM()
		{
			this.milkAmountInstance = Db.Get().Amounts.MilkProduction.Lookup(base.gameObject);
			if (base.def.effectId != null)
			{
				this.effectInstance = this.effects.Get(base.smi.def.effectId);
			}
			base.StartSM();
		}

		// Token: 0x0600730C RID: 29452 RVA: 0x002FF4B8 File Offset: 0x002FD6B8
		public void OnCaloriesConsumed(object data)
		{
			if (base.def.effectId == null)
			{
				return;
			}
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			this.effectInstance = this.effects.Get(base.smi.def.effectId);
			if (this.effectInstance == null)
			{
				this.effectInstance = this.effects.Add(base.smi.def.effectId, true);
			}
			this.effectInstance.timeRemaining += caloriesConsumedEvent.calories / base.smi.def.CaloriesPerCycle * 600f;
		}

		// Token: 0x0600730D RID: 29453 RVA: 0x002FF554 File Offset: 0x002FD754
		private void RemoveMilk(float amount)
		{
			if (this.milkAmountInstance != null)
			{
				float value = Mathf.Min(this.milkAmountInstance.GetMin(), this.MilkPercentage - amount);
				this.milkAmountInstance.SetValue(value);
			}
		}

		// Token: 0x0600730E RID: 29454 RVA: 0x002FF590 File Offset: 0x002FD790
		public PrimaryElement ExtractMilk(float desiredAmount)
		{
			float num = Mathf.Min(desiredAmount, this.MilkAmount);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			if (num <= 0f)
			{
				return null;
			}
			this.RemoveMilk(num);
			PrimaryElement component = LiquidSourceManager.Instance.CreateChunk(SimHashes.Milk, num, temperature, 0, 0, base.transform.GetPosition()).GetComponent<PrimaryElement>();
			component.KeepZeroMassObject = false;
			return component;
		}

		// Token: 0x0600730F RID: 29455 RVA: 0x002FF5F4 File Offset: 0x002FD7F4
		public PrimaryElement ExtractMilkIntoElementChunk(float desiredAmount, PrimaryElement elementChunk)
		{
			if (elementChunk == null || elementChunk.ElementID != SimHashes.Milk)
			{
				return null;
			}
			float num = Mathf.Min(desiredAmount, this.MilkAmount);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			this.RemoveMilk(num);
			float mass = elementChunk.Mass;
			float finalTemperature = GameUtil.GetFinalTemperature(elementChunk.Temperature, mass, temperature, num);
			elementChunk.SetMassTemperature(mass + num, finalTemperature);
			return elementChunk;
		}

		// Token: 0x06007310 RID: 29456 RVA: 0x002FF65C File Offset: 0x002FD85C
		public PrimaryElement ExtractMilkIntoStorage(float desiredAmount, Storage storage)
		{
			float num = Mathf.Min(desiredAmount, this.MilkAmount);
			float temperature = base.GetComponent<PrimaryElement>().Temperature;
			this.RemoveMilk(num);
			return storage.AddLiquid(SimHashes.Milk, num, temperature, 0, 0, false, true);
		}

		// Token: 0x04005615 RID: 22037
		public Action<float> OnMilkAmountChanged;

		// Token: 0x04005616 RID: 22038
		public AmountInstance milkAmountInstance;

		// Token: 0x04005617 RID: 22039
		public EffectInstance effectInstance;

		// Token: 0x04005618 RID: 22040
		[MyCmpGet]
		private Effects effects;
	}
}
