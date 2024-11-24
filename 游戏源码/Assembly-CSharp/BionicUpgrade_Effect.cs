using System;
using Klei.AI;

// Token: 0x02000C45 RID: 3141
public class BionicUpgrade_Effect : GameStateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>
{
	// Token: 0x06003C3B RID: 15419 RVA: 0x000C6EAD File Offset: 0x000C50AD
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
		this.root.Enter(new StateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>.State.Callback(BionicUpgrade_Effect.EnableEffect)).Exit(new StateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>.State.Callback(BionicUpgrade_Effect.DisableEffect));
	}

	// Token: 0x06003C3C RID: 15420 RVA: 0x000C6EE7 File Offset: 0x000C50E7
	public static void EnableEffect(BionicUpgrade_Effect.Instance smi)
	{
		smi.ApplyEffect();
	}

	// Token: 0x06003C3D RID: 15421 RVA: 0x000C6EEF File Offset: 0x000C50EF
	public static void DisableEffect(BionicUpgrade_Effect.Instance smi)
	{
		smi.RemoveEffect();
	}

	// Token: 0x02000C46 RID: 3142
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400292F RID: 10543
		public string EFFECT_NAME;
	}

	// Token: 0x02000C47 RID: 3143
	public new class Instance : GameStateMachine<BionicUpgrade_Effect, BionicUpgrade_Effect.Instance, IStateMachineTarget, BionicUpgrade_Effect.Def>.GameInstance
	{
		// Token: 0x06003C40 RID: 15424 RVA: 0x000C6EFF File Offset: 0x000C50FF
		public Instance(IStateMachineTarget master, BionicUpgrade_Effect.Def def) : base(master, def)
		{
			this.effects = base.GetComponent<Effects>();
		}

		// Token: 0x06003C41 RID: 15425 RVA: 0x0022D714 File Offset: 0x0022B914
		public void ApplyEffect()
		{
			Effect newEffect = Db.Get().effects.Get(base.def.EFFECT_NAME);
			this.effects.Add(newEffect, false);
		}

		// Token: 0x06003C42 RID: 15426 RVA: 0x0022D74C File Offset: 0x0022B94C
		public void RemoveEffect()
		{
			Effect effect = Db.Get().effects.Get(base.def.EFFECT_NAME);
			this.effects.Remove(effect);
		}

		// Token: 0x04002930 RID: 10544
		private Effects effects;
	}
}
