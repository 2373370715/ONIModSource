using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001120 RID: 4384
public class AgeMonitor : GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>
{
	// Token: 0x060059DA RID: 23002 RVA: 0x00293268 File Offset: 0x00291468
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.alive;
		this.alive.ToggleAttributeModifier("Aging", (AgeMonitor.Instance smi) => this.aging, null).Transition(this.time_to_die, new StateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.Transition.ConditionCallback(AgeMonitor.TimeToDie), UpdateRate.SIM_1000ms).Update(new Action<AgeMonitor.Instance, float>(AgeMonitor.UpdateOldStatusItem), UpdateRate.SIM_1000ms, false);
		this.time_to_die.Enter(new StateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State.Callback(AgeMonitor.Die));
		this.aging = new AttributeModifier(Db.Get().Amounts.Age.deltaAttribute.Id, 0.0016666667f, CREATURES.MODIFIERS.AGE.NAME, false, false, true);
	}

	// Token: 0x060059DB RID: 23003 RVA: 0x000DA7FB File Offset: 0x000D89FB
	private static void Die(AgeMonitor.Instance smi)
	{
		smi.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Generic);
	}

	// Token: 0x060059DC RID: 23004 RVA: 0x000DA817 File Offset: 0x000D8A17
	private static bool TimeToDie(AgeMonitor.Instance smi)
	{
		return smi.age.value >= smi.age.GetMax();
	}

	// Token: 0x060059DD RID: 23005 RVA: 0x00293314 File Offset: 0x00291514
	private static void UpdateOldStatusItem(AgeMonitor.Instance smi, float dt)
	{
		bool show = smi.age.value > smi.age.GetMax() * 0.9f;
		smi.oldStatusGuid = smi.kselectable.ToggleStatusItem(Db.Get().CreatureStatusItems.Old, smi.oldStatusGuid, show, smi);
	}

	// Token: 0x04003F6A RID: 16234
	public GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State alive;

	// Token: 0x04003F6B RID: 16235
	public GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.State time_to_die;

	// Token: 0x04003F6C RID: 16236
	private AttributeModifier aging;

	// Token: 0x02001121 RID: 4385
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060059E0 RID: 23008 RVA: 0x000DA844 File Offset: 0x000D8A44
		public override void Configure(GameObject prefab)
		{
			prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Age.Id);
		}

		// Token: 0x04003F6D RID: 16237
		public float maxAgePercentOnSpawn = 0.75f;
	}

	// Token: 0x02001122 RID: 4386
	public new class Instance : GameStateMachine<AgeMonitor, AgeMonitor.Instance, IStateMachineTarget, AgeMonitor.Def>.GameInstance
	{
		// Token: 0x060059E2 RID: 23010 RVA: 0x00293368 File Offset: 0x00291568
		public Instance(IStateMachineTarget master, AgeMonitor.Def def) : base(master, def)
		{
			this.age = Db.Get().Amounts.Age.Lookup(base.gameObject);
			base.Subscribe(1119167081, delegate(object data)
			{
				this.RandomizeAge();
			});
		}

		// Token: 0x060059E3 RID: 23011 RVA: 0x002933B4 File Offset: 0x002915B4
		public void RandomizeAge()
		{
			this.age.value = UnityEngine.Random.value * this.age.GetMax() * base.def.maxAgePercentOnSpawn;
			AmountInstance amountInstance = Db.Get().Amounts.Fertility.Lookup(base.gameObject);
			if (amountInstance != null)
			{
				amountInstance.value = this.age.value / this.age.GetMax() * amountInstance.GetMax() * 1.75f;
				amountInstance.value = Mathf.Min(amountInstance.value, amountInstance.GetMax() * 0.9f);
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x060059E4 RID: 23012 RVA: 0x000DA87D File Offset: 0x000D8A7D
		public float CyclesUntilDeath
		{
			get
			{
				return this.age.GetMax() - this.age.value;
			}
		}

		// Token: 0x04003F6E RID: 16238
		public AmountInstance age;

		// Token: 0x04003F6F RID: 16239
		public Guid oldStatusGuid;

		// Token: 0x04003F70 RID: 16240
		[MyCmpReq]
		public KSelectable kselectable;
	}
}
