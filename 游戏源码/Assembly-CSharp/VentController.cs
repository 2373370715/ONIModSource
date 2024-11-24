using System;
using UnityEngine;

// Token: 0x02000080 RID: 128
public class VentController : GameStateMachine<VentController, VentController.Instance>
{
	// Token: 0x06000203 RID: 515 RVA: 0x00145888 File Offset: 0x00143A88
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.EventHandler(GameHashes.VentAnimatingChanged, new GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.GameEvent.Callback(VentController.UpdateMeterColor)).EventTransition(GameHashes.VentClosed, this.closed, (VentController.Instance smi) => smi.GetComponent<Vent>().Closed()).EventTransition(GameHashes.VentOpen, this.off, (VentController.Instance smi) => !smi.GetComponent<Vent>().Closed());
		this.off.PlayAnim("off").EventTransition(GameHashes.VentAnimatingChanged, this.working_pre, new StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(VentController.IsAnimating));
		this.working_pre.PlayAnim("working_pre").OnAnimQueueComplete(this.working_loop);
		this.working_loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).Enter(new StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State.Callback(VentController.PlayOutputMeterAnim)).EventTransition(GameHashes.VentAnimatingChanged, this.working_pst, GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.Not(new StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(VentController.IsAnimating)));
		this.working_pst.PlayAnim("working_pst").OnAnimQueueComplete(this.off);
		this.closed.PlayAnim("closed").EventTransition(GameHashes.VentAnimatingChanged, this.working_pre, new StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.Transition.ConditionCallback(VentController.IsAnimating));
	}

	// Token: 0x06000204 RID: 516 RVA: 0x000A694F File Offset: 0x000A4B4F
	public static void PlayOutputMeterAnim(VentController.Instance smi)
	{
		smi.PlayMeterAnim();
	}

	// Token: 0x06000205 RID: 517 RVA: 0x000A6957 File Offset: 0x000A4B57
	public static bool IsAnimating(VentController.Instance smi)
	{
		return smi.exhaust.IsAnimating();
	}

	// Token: 0x06000206 RID: 518 RVA: 0x001459F0 File Offset: 0x00143BF0
	public static void UpdateMeterColor(VentController.Instance smi, object data)
	{
		if (data != null)
		{
			Color32 meterOutputColor = (Color32)data;
			smi.SetMeterOutputColor(meterOutputColor);
		}
	}

	// Token: 0x0400014E RID: 334
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x0400014F RID: 335
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State working_pre;

	// Token: 0x04000150 RID: 336
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State working_loop;

	// Token: 0x04000151 RID: 337
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State working_pst;

	// Token: 0x04000152 RID: 338
	public GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.State closed;

	// Token: 0x04000153 RID: 339
	public StateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.BoolParameter isAnimating;

	// Token: 0x02000081 RID: 129
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04000154 RID: 340
		public bool usingDynamicColor;

		// Token: 0x04000155 RID: 341
		public string outputSubstanceAnimName;
	}

	// Token: 0x02000082 RID: 130
	public new class Instance : GameStateMachine<VentController, VentController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06000209 RID: 521 RVA: 0x000A696C File Offset: 0x000A4B6C
		public Instance(IStateMachineTarget master, VentController.Def def) : base(master, def)
		{
			if (def.usingDynamicColor)
			{
				this.outputSubstanceMeter = new MeterController(this.anim, "meter_target", def.outputSubstanceAnimName, Meter.Offset.NoChange, Grid.SceneLayer.Building, Array.Empty<string>());
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x000A69A2 File Offset: 0x000A4BA2
		public void PlayMeterAnim()
		{
			if (this.outputSubstanceMeter != null)
			{
				this.outputSubstanceMeter.meterController.Play(this.outputSubstanceMeter.meterController.initialAnim, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x000A69DC File Offset: 0x000A4BDC
		public void SetMeterOutputColor(Color32 color)
		{
			if (this.outputSubstanceMeter != null)
			{
				this.outputSubstanceMeter.meterController.TintColour = color;
			}
		}

		// Token: 0x04000156 RID: 342
		[MyCmpGet]
		private KBatchedAnimController anim;

		// Token: 0x04000157 RID: 343
		[MyCmpGet]
		public Exhaust exhaust;

		// Token: 0x04000158 RID: 344
		private MeterController outputSubstanceMeter;
	}
}
