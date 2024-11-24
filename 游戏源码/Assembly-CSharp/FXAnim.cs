using System;
using UnityEngine;

// Token: 0x02000BEF RID: 3055
public class FXAnim : GameStateMachine<FXAnim, FXAnim.Instance>
{
	// Token: 0x06003A6C RID: 14956 RVA: 0x00227504 File Offset: 0x00225704
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		base.Target(this.fx);
		this.loop.Enter(delegate(FXAnim.Instance smi)
		{
			smi.Enter();
		}).EventTransition(GameHashes.AnimQueueComplete, this.restart, null).Exit("Post", delegate(FXAnim.Instance smi)
		{
			smi.Exit();
		});
		this.restart.GoTo(this.loop);
	}

	// Token: 0x040027D1 RID: 10193
	public StateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x040027D2 RID: 10194
	public GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.State loop;

	// Token: 0x040027D3 RID: 10195
	public GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.State restart;

	// Token: 0x02000BF0 RID: 3056
	public new class Instance : GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06003A6E RID: 14958 RVA: 0x0022759C File Offset: 0x0022579C
		public Instance(IStateMachineTarget master, string kanim_file, string anim, KAnim.PlayMode mode, Vector3 offset, Color32 tint_colour) : base(master)
		{
			this.animController = FXHelpers.CreateEffect(kanim_file, base.smi.master.transform.GetPosition() + offset, base.smi.master.transform, false, Grid.SceneLayer.Front, false);
			this.animController.gameObject.Subscribe(-1061186183, new Action<object>(this.OnAnimQueueComplete));
			this.animController.TintColour = tint_colour;
			base.sm.fx.Set(this.animController.gameObject, base.smi, false);
			this.anim = anim;
			this.mode = mode;
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x000C5B63 File Offset: 0x000C3D63
		public void Enter()
		{
			this.animController.Play(this.anim, this.mode, 1f, 0f);
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x000C5B8B File Offset: 0x000C3D8B
		public void Exit()
		{
			this.DestroyFX();
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x000C5B8B File Offset: 0x000C3D8B
		private void OnAnimQueueComplete(object data)
		{
			this.DestroyFX();
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x000C5B93 File Offset: 0x000C3D93
		private void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

		// Token: 0x040027D4 RID: 10196
		private string anim;

		// Token: 0x040027D5 RID: 10197
		private KAnim.PlayMode mode;

		// Token: 0x040027D6 RID: 10198
		private KBatchedAnimController animController;
	}
}
