using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000C5B RID: 3163
public class BionicUpgrade_SkilledWorker : BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>
{
	// Token: 0x06003C90 RID: 15504 RVA: 0x0022E048 File Offset: 0x0022C248
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.Inactive;
		this.root.Enter(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.ApplySkills)).ToggleEffect(new Func<BionicUpgrade_SkilledWorker.Instance, string>(BionicUpgrade_SkilledWorker.GetEffectName)).Exit(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.RemoveSkills));
		this.Inactive.EventTransition(GameHashes.ScheduleBlocksChanged, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.ScheduleChanged, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.BionicOnline, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.StartWork, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null);
		this.Active.EventTransition(GameHashes.ScheduleBlocksChanged, this.Inactive, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsInBatterySaveMode)).EventTransition(GameHashes.ScheduleChanged, this.Inactive, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsInBatterySaveMode)).EventTransition(GameHashes.BionicOffline, this.Inactive, null).EventTransition(GameHashes.StopWork, this.Inactive, null).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null).Enter(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.CreateFX)).Exit(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.ClearFX));
	}

	// Token: 0x06003C91 RID: 15505 RVA: 0x000C72EB File Offset: 0x000C54EB
	public static string GetEffectName(BionicUpgrade_SkilledWorker.Instance smi)
	{
		return ((BionicUpgrade_SkilledWorker.Def)smi.def).EFFECT_NAME;
	}

	// Token: 0x06003C92 RID: 15506 RVA: 0x000C72FD File Offset: 0x000C54FD
	public static void ApplySkills(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.ApplySkills();
	}

	// Token: 0x06003C93 RID: 15507 RVA: 0x000C7305 File Offset: 0x000C5505
	public static void RemoveSkills(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.RemoveSkills();
	}

	// Token: 0x06003C94 RID: 15508 RVA: 0x000C730D File Offset: 0x000C550D
	public static bool IsMinionWorkingOnlineAndNotInBatterySaveMode(BionicUpgrade_SkilledWorker.Instance smi)
	{
		return BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsOnline(smi) && !BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsInBatterySaveMode(smi) && BionicUpgrade_SkilledWorker.IsMinionWorkingWithAttribute(smi);
	}

	// Token: 0x06003C95 RID: 15509 RVA: 0x0022E1B0 File Offset: 0x0022C3B0
	public static bool IsMinionWorkingWithAttribute(BionicUpgrade_SkilledWorker.Instance smi)
	{
		Workable workable = smi.worker.GetWorkable();
		return workable != null && smi.worker.GetState() == WorkerBase.State.Working && workable.GetWorkAttribute() != null && workable.GetWorkAttribute().Id == ((BionicUpgrade_SkilledWorker.Def)smi.def).ATTRIBUTE_ID;
	}

	// Token: 0x06003C96 RID: 15510 RVA: 0x000C7327 File Offset: 0x000C5527
	public static void CreateFX(BionicUpgrade_SkilledWorker.Instance smi)
	{
		BionicUpgrade_SkilledWorker.CreateAndReturnFX(smi);
	}

	// Token: 0x06003C97 RID: 15511 RVA: 0x0022E20C File Offset: 0x0022C40C
	public static BionicAttributeUseFx.Instance CreateAndReturnFX(BionicUpgrade_SkilledWorker.Instance smi)
	{
		if (!smi.isMasterNull)
		{
			smi.fx = new BionicAttributeUseFx.Instance(smi.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.FXFront)));
			smi.fx.StartSM();
			return smi.fx;
		}
		return null;
	}

	// Token: 0x06003C98 RID: 15512 RVA: 0x000C7330 File Offset: 0x000C5530
	public static void ClearFX(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.fx.sm.destroyFX.Trigger(smi.fx);
		smi.fx = null;
	}

	// Token: 0x02000C5C RID: 3164
	public new class Def : BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def
	{
		// Token: 0x06003C9A RID: 15514 RVA: 0x000C735C File Offset: 0x000C555C
		public Def(string upgradeID, string attributeID, string effectID, string[] skills = null) : base(upgradeID)
		{
			this.ATTRIBUTE_ID = attributeID;
			this.EFFECT_NAME = effectID;
			this.SKILLS_IDS = skills;
		}

		// Token: 0x0400294D RID: 10573
		public string EFFECT_NAME;

		// Token: 0x0400294E RID: 10574
		public string[] SKILLS_IDS;

		// Token: 0x0400294F RID: 10575
		public string ATTRIBUTE_ID;
	}

	// Token: 0x02000C5D RID: 3165
	public new class Instance : BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.BaseInstance
	{
		// Token: 0x06003C9B RID: 15515 RVA: 0x000C737B File Offset: 0x000C557B
		public Instance(IStateMachineTarget master, BionicUpgrade_SkilledWorker.Def def) : base(master, def)
		{
			this.worker = base.GetComponent<WorkerBase>();
			this.resume = base.GetComponent<MinionResume>();
		}

		// Token: 0x06003C9C RID: 15516 RVA: 0x000C739D File Offset: 0x000C559D
		public override float GetCurrentWattageCost()
		{
			if (base.IsInsideState(base.sm.Active))
			{
				return base.Data.WattageCost;
			}
			return 0f;
		}

		// Token: 0x06003C9D RID: 15517 RVA: 0x0022E25C File Offset: 0x0022C45C
		public override string GetCurrentWattageCostName()
		{
			float currentWattageCost = this.GetCurrentWattageCost();
			if (base.IsInsideState(base.sm.Active))
			{
				string str = "<b>" + ((currentWattageCost >= 0f) ? "+" : "-") + "</b>";
				return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.STANDARD_ACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), str + GameUtil.GetFormattedWattage(currentWattageCost, GameUtil.WattageFormatterUnit.Automatic, true));
			}
			return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.STANDARD_INACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(this.upgradeComponent.PotentialWattage, GameUtil.WattageFormatterUnit.Automatic, true));
		}

		// Token: 0x06003C9E RID: 15518 RVA: 0x0022E2FC File Offset: 0x0022C4FC
		public void ApplySkills()
		{
			BionicUpgrade_SkilledWorker.Def def = (BionicUpgrade_SkilledWorker.Def)base.def;
			if (def.SKILLS_IDS != null)
			{
				for (int i = 0; i < def.SKILLS_IDS.Length; i++)
				{
					string skillId = def.SKILLS_IDS[i];
					this.resume.GrantSkill(skillId);
				}
			}
		}

		// Token: 0x06003C9F RID: 15519 RVA: 0x0022E348 File Offset: 0x0022C548
		public void RemoveSkills()
		{
			BionicUpgrade_SkilledWorker.Def def = (BionicUpgrade_SkilledWorker.Def)base.def;
			if (def.SKILLS_IDS != null)
			{
				for (int i = 0; i < def.SKILLS_IDS.Length; i++)
				{
					string skillId = def.SKILLS_IDS[i];
					this.resume.UngrantSkill(skillId);
				}
			}
		}

		// Token: 0x04002950 RID: 10576
		public WorkerBase worker;

		// Token: 0x04002951 RID: 10577
		public BionicAttributeUseFx.Instance fx;

		// Token: 0x04002952 RID: 10578
		private MinionResume resume;
	}
}
