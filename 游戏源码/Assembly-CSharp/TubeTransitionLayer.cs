using System;
using UnityEngine;

// Token: 0x02000B57 RID: 2903
public class TubeTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x0600370A RID: 14090 RVA: 0x000C3B70 File Offset: 0x000C1D70
	public TubeTransitionLayer(Navigator navigator) : base(navigator)
	{
		this.tube_traveller = navigator.GetSMI<TubeTraveller.Instance>();
		if (this.tube_traveller != null && navigator.CurrentNavType == NavType.Tube && !this.tube_traveller.inTube)
		{
			this.tube_traveller.OnTubeTransition(true);
		}
	}

	// Token: 0x0600370B RID: 14091 RVA: 0x00215A88 File Offset: 0x00213C88
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		this.tube_traveller.OnPathAdvanced(null);
		if (transition.start != NavType.Tube && transition.end == NavType.Tube)
		{
			int cell = Grid.PosToCell(navigator);
			this.entrance = this.GetEntrance(cell);
			return;
		}
		this.entrance = null;
	}

	// Token: 0x0600370C RID: 14092 RVA: 0x00215AD8 File Offset: 0x00213CD8
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		if (transition.start != NavType.Tube && transition.end == NavType.Tube && this.entrance)
		{
			this.entrance.ConsumeCharge(navigator.gameObject);
			this.entrance = null;
		}
		this.tube_traveller.OnTubeTransition(transition.end == NavType.Tube);
	}

	// Token: 0x0600370D RID: 14093 RVA: 0x00215B38 File Offset: 0x00213D38
	private TravelTubeEntrance GetEntrance(int cell)
	{
		if (!Grid.HasUsableTubeEntrance(cell, this.tube_traveller.prefabInstanceID))
		{
			return null;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			TravelTubeEntrance component = gameObject.GetComponent<TravelTubeEntrance>();
			if (component != null && component.isSpawned)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x04002547 RID: 9543
	private TubeTraveller.Instance tube_traveller;

	// Token: 0x04002548 RID: 9544
	private TravelTubeEntrance entrance;
}
