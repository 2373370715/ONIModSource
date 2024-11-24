using System;

// Token: 0x02000B5A RID: 2906
public class NavTeleportTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x06003713 RID: 14099 RVA: 0x000C3B41 File Offset: 0x000C1D41
	public NavTeleportTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x06003714 RID: 14100 RVA: 0x00215D18 File Offset: 0x00213F18
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		if (transition.start == NavType.Teleport)
		{
			int num = Grid.PosToCell(navigator);
			int num2;
			int num3;
			Grid.CellToXY(num, out num2, out num3);
			int num4 = navigator.NavGrid.teleportTransitions[num];
			int num5;
			int num6;
			Grid.CellToXY(navigator.NavGrid.teleportTransitions[num], out num5, out num6);
			transition.x = num5 - num2;
			transition.y = num6 - num3;
		}
	}
}
