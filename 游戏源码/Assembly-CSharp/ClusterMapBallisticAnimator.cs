using System;
using UnityEngine;

// Token: 0x0200195B RID: 6491
public class ClusterMapBallisticAnimator : GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer>
{
	// Token: 0x06008773 RID: 34675 RVA: 0x00350C5C File Offset: 0x0034EE5C
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.moving;
		this.root.Target(this.entityTarget).TagTransition(GameTags.BallisticEntityLaunching, this.launching, false).TagTransition(GameTags.BallisticEntityLanding, this.landing, false).TagTransition(GameTags.BallisticEntityMoving, this.moving, false);
		this.moving.Enter(delegate(ClusterMapBallisticAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("inflight_loop", KAnim.PlayMode.Loop);
		});
		this.landing.Enter(delegate(ClusterMapBallisticAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("landing", KAnim.PlayMode.Loop);
		});
		this.launching.Enter(delegate(ClusterMapBallisticAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("launching", KAnim.PlayMode.Loop);
		});
	}

	// Token: 0x04006620 RID: 26144
	public StateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

	// Token: 0x04006621 RID: 26145
	public GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State launching;

	// Token: 0x04006622 RID: 26146
	public GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State moving;

	// Token: 0x04006623 RID: 26147
	public GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State landing;

	// Token: 0x0200195C RID: 6492
	public class StatesInstance : GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
	{
		// Token: 0x06008775 RID: 34677 RVA: 0x000F8912 File Offset: 0x000F6B12
		public StatesInstance(ClusterMapVisualizer master, ClusterGridEntity entity) : base(master)
		{
			this.entity = entity;
			base.sm.entityTarget.Set(entity, this);
		}

		// Token: 0x06008776 RID: 34678 RVA: 0x000F893B File Offset: 0x000F6B3B
		public void PlayVisAnim(string animName, KAnim.PlayMode playMode)
		{
			base.GetComponent<ClusterMapVisualizer>().PlayAnim(animName, playMode);
		}

		// Token: 0x06008777 RID: 34679 RVA: 0x00350D38 File Offset: 0x0034EF38
		public void ToggleVisAnim(bool on)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			if (!on)
			{
				component.GetFirstAnimController().Play("grounded", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06008778 RID: 34680 RVA: 0x00350D70 File Offset: 0x0034EF70
		public void SubscribeOnVisAnimComplete(Action<object> action)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			this.UnsubscribeOnVisAnimComplete();
			this.animCompleteSubscriber = component.GetFirstAnimController().gameObject;
			this.animCompleteHandle = this.animCompleteSubscriber.Subscribe(-1061186183, action);
		}

		// Token: 0x06008779 RID: 34681 RVA: 0x000F894A File Offset: 0x000F6B4A
		public void UnsubscribeOnVisAnimComplete()
		{
			if (this.animCompleteHandle != -1)
			{
				DebugUtil.DevAssert(this.animCompleteSubscriber != null, "ClustermapBallisticAnimator animCompleteSubscriber GameObject is null. Whatever the previous gameObject in this variable was, it may not have unsubscribed from an event properly", null);
				this.animCompleteSubscriber.Unsubscribe(this.animCompleteHandle);
				this.animCompleteHandle = -1;
			}
		}

		// Token: 0x0600877A RID: 34682 RVA: 0x000F8984 File Offset: 0x000F6B84
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			this.UnsubscribeOnVisAnimComplete();
		}

		// Token: 0x04006624 RID: 26148
		public ClusterGridEntity entity;

		// Token: 0x04006625 RID: 26149
		private int animCompleteHandle = -1;

		// Token: 0x04006626 RID: 26150
		private GameObject animCompleteSubscriber;
	}
}
