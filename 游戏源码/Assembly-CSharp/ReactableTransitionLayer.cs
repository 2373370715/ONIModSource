using System;

// Token: 0x02000B59 RID: 2905
public class ReactableTransitionLayer : TransitionDriver.InterruptOverrideLayer
{
	// Token: 0x06003710 RID: 14096 RVA: 0x000C3BAF File Offset: 0x000C1DAF
	public ReactableTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x06003711 RID: 14097 RVA: 0x000C3BB8 File Offset: 0x000C1DB8
	protected override bool IsOverrideComplete()
	{
		return !this.reactionMonitor.IsReacting() && base.IsOverrideComplete();
	}

	// Token: 0x06003712 RID: 14098 RVA: 0x00215CB4 File Offset: 0x00213EB4
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		if (this.reactionMonitor == null)
		{
			this.reactionMonitor = navigator.GetSMI<ReactionMonitor.Instance>();
		}
		this.reactionMonitor.PollForReactables(transition);
		if (this.reactionMonitor.IsReacting())
		{
			base.BeginTransition(navigator, transition);
			transition.start = this.originalTransition.start;
			transition.end = this.originalTransition.end;
		}
	}

	// Token: 0x04002549 RID: 9545
	private ReactionMonitor.Instance reactionMonitor;
}
