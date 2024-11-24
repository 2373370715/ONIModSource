using System;

// Token: 0x02000B55 RID: 2901
public class FullPuftTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x06003701 RID: 14081 RVA: 0x000C3B41 File Offset: 0x000C1D41
	public FullPuftTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x06003702 RID: 14082 RVA: 0x00215790 File Offset: 0x00213990
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		CreatureCalorieMonitor.Instance smi = navigator.GetSMI<CreatureCalorieMonitor.Instance>();
		if (smi != null && smi.stomach.IsReadyToPoop())
		{
			string s = HashCache.Get().Get(transition.anim.HashValue) + "_full";
			if (navigator.animController.HasAnimation(s))
			{
				transition.anim = s;
			}
		}
	}
}
