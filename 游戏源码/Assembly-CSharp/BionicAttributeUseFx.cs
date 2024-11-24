using System;
using UnityEngine;

// Token: 0x02000BEC RID: 3052
public class BionicAttributeUseFx : GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance>
{
	// Token: 0x06003A64 RID: 14948 RVA: 0x002273B0 File Offset: 0x002255B0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre;
		base.Target(this.fx);
		this.root.OnSignal(this.wasProductive, this.productive, (BionicAttributeUseFx.Instance smi) => smi.GetCurrentState() != smi.sm.pst).OnSignal(this.destroyFX, this.pst);
		this.pre.PlayAnim("bionic_upgrade_active_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
		this.idle.PlayAnim("bionic_upgrade_active_loop", KAnim.PlayMode.Loop);
		this.productive.QueueAnim("bionic_upgrade_active_achievement", false, null).OnAnimQueueComplete(this.idle);
		this.pst.PlayAnim("bionic_upgrade_active_pst").EventHandler(GameHashes.AnimQueueComplete, delegate(BionicAttributeUseFx.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x040027C7 RID: 10183
	public StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.Signal wasProductive;

	// Token: 0x040027C8 RID: 10184
	public StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.Signal destroyFX;

	// Token: 0x040027C9 RID: 10185
	public StateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x040027CA RID: 10186
	public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State pre;

	// Token: 0x040027CB RID: 10187
	public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x040027CC RID: 10188
	public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State productive;

	// Token: 0x040027CD RID: 10189
	public GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.State pst;

	// Token: 0x02000BED RID: 3053
	public new class Instance : GameStateMachine<BionicAttributeUseFx, BionicAttributeUseFx.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06003A66 RID: 14950 RVA: 0x002274A0 File Offset: 0x002256A0
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("bionic_upgrade_active_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.FXFront, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x06003A67 RID: 14951 RVA: 0x000C5B02 File Offset: 0x000C3D02
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
			base.smi.StopSM("destroyed");
		}
	}
}
