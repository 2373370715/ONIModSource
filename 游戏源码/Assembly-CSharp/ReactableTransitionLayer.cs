public class ReactableTransitionLayer : TransitionDriver.InterruptOverrideLayer
{
	private ReactionMonitor.Instance reactionMonitor;

	public ReactableTransitionLayer(Navigator navigator)
		: base(navigator)
	{
	}

	protected override bool IsOverrideComplete()
	{
		if (reactionMonitor.IsReacting())
		{
			return false;
		}
		return base.IsOverrideComplete();
	}

	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		if (reactionMonitor == null)
		{
			reactionMonitor = navigator.GetSMI<ReactionMonitor.Instance>();
		}
		reactionMonitor.PollForReactables(transition);
		if (reactionMonitor.IsReacting())
		{
			base.BeginTransition(navigator, transition);
			transition.start = originalTransition.start;
			transition.end = originalTransition.end;
		}
	}
}
