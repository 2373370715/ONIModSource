using System;
using UnityEngine;

// Token: 0x02000B54 RID: 2900
public class SplashTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x060036FC RID: 14076 RVA: 0x000C3AF7 File Offset: 0x000C1CF7
	public SplashTransitionLayer(Navigator navigator) : base(navigator)
	{
		this.lastSplashTime = Time.time;
	}

	// Token: 0x060036FD RID: 14077 RVA: 0x002156E8 File Offset: 0x002138E8
	private void RefreshSplashes(Navigator navigator, Navigator.ActiveTransition transition)
	{
		if (navigator == null)
		{
			return;
		}
		if (transition.end == NavType.Tube)
		{
			return;
		}
		Vector3 position = navigator.transform.GetPosition();
		if (this.lastSplashTime + 1f < Time.time && Grid.Element[Grid.PosToCell(position)].IsLiquid)
		{
			this.lastSplashTime = Time.time;
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("splash_step_kanim", position + new Vector3(0f, 0.75f, -0.1f), null, false, Grid.SceneLayer.Front, false);
			kbatchedAnimController.Play("fx1", KAnim.PlayMode.Once, 1f, 0f);
			kbatchedAnimController.destroyOnAnimComplete = true;
		}
	}

	// Token: 0x060036FE RID: 14078 RVA: 0x000C3B0B File Offset: 0x000C1D0B
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		this.RefreshSplashes(navigator, transition);
	}

	// Token: 0x060036FF RID: 14079 RVA: 0x000C3B1D File Offset: 0x000C1D1D
	public override void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.UpdateTransition(navigator, transition);
		this.RefreshSplashes(navigator, transition);
	}

	// Token: 0x06003700 RID: 14080 RVA: 0x000C3B2F File Offset: 0x000C1D2F
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		this.RefreshSplashes(navigator, transition);
	}

	// Token: 0x04002544 RID: 9540
	private float lastSplashTime;

	// Token: 0x04002545 RID: 9541
	private const float SPLASH_INTERVAL = 1f;
}
