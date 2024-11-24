using System;

// Token: 0x02000C58 RID: 3160
public class BionicUpgrade_Skill : GameStateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>
{
	// Token: 0x06003C88 RID: 15496 RVA: 0x000C7253 File Offset: 0x000C5453
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
		this.root.Enter(new StateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>.State.Callback(BionicUpgrade_Skill.EnableEffect)).Exit(new StateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>.State.Callback(BionicUpgrade_Skill.DisableEffect));
	}

	// Token: 0x06003C89 RID: 15497 RVA: 0x000C728D File Offset: 0x000C548D
	public static void EnableEffect(BionicUpgrade_Skill.Instance smi)
	{
		smi.ApplySkill();
	}

	// Token: 0x06003C8A RID: 15498 RVA: 0x000C7295 File Offset: 0x000C5495
	public static void DisableEffect(BionicUpgrade_Skill.Instance smi)
	{
		smi.RemoveSkill();
	}

	// Token: 0x02000C59 RID: 3161
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400294B RID: 10571
		public string SKILL_ID;
	}

	// Token: 0x02000C5A RID: 3162
	public new class Instance : GameStateMachine<BionicUpgrade_Skill, BionicUpgrade_Skill.Instance, IStateMachineTarget, BionicUpgrade_Skill.Def>.GameInstance
	{
		// Token: 0x06003C8D RID: 15501 RVA: 0x000C72A5 File Offset: 0x000C54A5
		public Instance(IStateMachineTarget master, BionicUpgrade_Skill.Def def) : base(master, def)
		{
			this.resume = base.GetComponent<MinionResume>();
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x000C72BB File Offset: 0x000C54BB
		public void ApplySkill()
		{
			this.resume.GrantSkill(base.def.SKILL_ID);
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x000C72D3 File Offset: 0x000C54D3
		public void RemoveSkill()
		{
			this.resume.UngrantSkill(base.def.SKILL_ID);
		}

		// Token: 0x0400294C RID: 10572
		private MinionResume resume;
	}
}
