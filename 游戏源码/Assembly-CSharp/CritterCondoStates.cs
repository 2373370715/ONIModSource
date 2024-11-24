using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000153 RID: 339
public class CritterCondoStates : GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>
{
	// Token: 0x060004EE RID: 1262 RVA: 0x00157DB8 File Offset: 0x00155FB8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.goingToCondo;
		this.root.Enter(new StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State.Callback(CritterCondoStates.ReserveCondo)).Exit(new StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State.Callback(CritterCondoStates.UnreserveCondo));
		this.goingToCondo.MoveTo(new Func<CritterCondoStates.Instance, int>(CritterCondoStates.GetCondoInteractCell), this.interact, null, false).ToggleMainStatusItem((CritterCondoStates.Instance smi) => CritterCondoStates.GetTargetCondo(smi).def.moveToStatusItem, null).OnTargetLost(this.targetCondo, null);
		this.interact.DefaultState(this.interact.pre).OnTargetLost(this.targetCondo, null).Enter(delegate(CritterCondoStates.Instance smi)
		{
			this.SetFacing(smi);
			smi.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
		}).Exit(delegate(CritterCondoStates.Instance smi)
		{
			smi.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
		}).ToggleMainStatusItem((CritterCondoStates.Instance smi) => CritterCondoStates.GetTargetCondo(smi).def.interactStatusItem, null);
		this.interact.pre.PlayAnim("cc_working_pre").Enter(delegate(CritterCondoStates.Instance smi)
		{
			CritterCondoStates.PlayCondoBuildingAnim(smi, "cc_working_pre");
		}).OnAnimQueueComplete(this.interact.loop);
		this.interact.loop.PlayAnim("cc_working").Enter(delegate(CritterCondoStates.Instance smi)
		{
			CritterCondoStates.PlayCondoBuildingAnim(smi, smi.def.working_anim);
		}).OnAnimQueueComplete(this.interact.pst);
		this.interact.pst.PlayAnim("cc_working_pst").Enter(delegate(CritterCondoStates.Instance smi)
		{
			CritterCondoStates.PlayCondoBuildingAnim(smi, "cc_working_pst");
		}).OnAnimQueueComplete(this.behaviourComplete);
		this.behaviourComplete.BehaviourComplete(GameTags.Creatures.Behaviour_InteractWithCritterCondo, false).Exit(new StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State.Callback(CritterCondoStates.ApplyEffects));
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x00157FC0 File Offset: 0x001561C0
	private void SetFacing(CritterCondoStates.Instance smi)
	{
		bool isRotated = CritterCondoStates.GetTargetCondo(smi).Get<Rotatable>().IsRotated;
		smi.Get<Facing>().SetFacing(isRotated);
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x00157FEC File Offset: 0x001561EC
	private static CritterCondo.Instance GetTargetCondo(CritterCondoStates.Instance smi)
	{
		GameObject gameObject = smi.sm.targetCondo.Get(smi);
		CritterCondo.Instance instance = (gameObject != null) ? gameObject.GetSMI<CritterCondo.Instance>() : null;
		if (instance.IsNullOrStopped())
		{
			return null;
		}
		return instance;
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x0015802C File Offset: 0x0015622C
	private static void ReserveCondo(CritterCondoStates.Instance smi)
	{
		CritterCondo.Instance instance = smi.GetSMI<CritterCondoInteractMontior.Instance>().targetCondo;
		if (instance == null)
		{
			return;
		}
		smi.sm.targetCondo.Set(instance.gameObject, smi, false);
		instance.SetReserved(true);
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x0015806C File Offset: 0x0015626C
	private static void UnreserveCondo(CritterCondoStates.Instance smi)
	{
		CritterCondo.Instance instance = CritterCondoStates.GetTargetCondo(smi);
		if (instance == null)
		{
			return;
		}
		instance.GetComponent<KBatchedAnimController>().Play("on", KAnim.PlayMode.Loop, 1f, 0f);
		smi.sm.targetCondo.Set(null, smi);
		instance.SetReserved(false);
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x001580C0 File Offset: 0x001562C0
	private static int GetCondoInteractCell(CritterCondoStates.Instance smi)
	{
		CritterCondo.Instance instance = CritterCondoStates.GetTargetCondo(smi);
		if (instance == null)
		{
			return Grid.InvalidCell;
		}
		return instance.GetInteractStartCell();
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x000A7DCD File Offset: 0x000A5FCD
	private static void ApplyEffects(CritterCondoStates.Instance smi)
	{
		smi.Get<Effects>().Add(CritterCondoStates.GetTargetCondo(smi).def.effectId, true);
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x000A7DEC File Offset: 0x000A5FEC
	private static void PlayCondoBuildingAnim(CritterCondoStates.Instance smi, string anim_name)
	{
		if (smi.def.entersBuilding)
		{
			smi.sm.targetCondo.Get<KBatchedAnimController>(smi).Play(anim_name, KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x04000397 RID: 919
	public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State goingToCondo;

	// Token: 0x04000398 RID: 920
	public CritterCondoStates.InteractState interact;

	// Token: 0x04000399 RID: 921
	public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State behaviourComplete;

	// Token: 0x0400039A RID: 922
	public StateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.TargetParameter targetCondo;

	// Token: 0x02000154 RID: 340
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400039B RID: 923
		public bool entersBuilding = true;

		// Token: 0x0400039C RID: 924
		public string working_anim = "cc_working";
	}

	// Token: 0x02000155 RID: 341
	public new class Instance : GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.GameInstance
	{
		// Token: 0x060004F9 RID: 1273 RVA: 0x000A7E5A File Offset: 0x000A605A
		public Instance(Chore<CritterCondoStates.Instance> chore, CritterCondoStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviour_InteractWithCritterCondo);
		}
	}

	// Token: 0x02000156 RID: 342
	public class InteractState : GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State
	{
		// Token: 0x0400039D RID: 925
		public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State pre;

		// Token: 0x0400039E RID: 926
		public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State loop;

		// Token: 0x0400039F RID: 927
		public GameStateMachine<CritterCondoStates, CritterCondoStates.Instance, IStateMachineTarget, CritterCondoStates.Def>.State pst;
	}
}
