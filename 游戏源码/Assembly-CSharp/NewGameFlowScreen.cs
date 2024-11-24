using System;

// Token: 0x02001E4D RID: 7757
public abstract class NewGameFlowScreen : KModalScreen
{
	// Token: 0x1400002F RID: 47
	// (add) Token: 0x0600A287 RID: 41607 RVA: 0x003DE1AC File Offset: 0x003DC3AC
	// (remove) Token: 0x0600A288 RID: 41608 RVA: 0x003DE1E4 File Offset: 0x003DC3E4
	public event System.Action OnNavigateForward;

	// Token: 0x14000030 RID: 48
	// (add) Token: 0x0600A289 RID: 41609 RVA: 0x003DE21C File Offset: 0x003DC41C
	// (remove) Token: 0x0600A28A RID: 41610 RVA: 0x003DE254 File Offset: 0x003DC454
	public event System.Action OnNavigateBackward;

	// Token: 0x0600A28B RID: 41611 RVA: 0x001096A0 File Offset: 0x001078A0
	protected void NavigateBackward()
	{
		this.OnNavigateBackward();
	}

	// Token: 0x0600A28C RID: 41612 RVA: 0x001096AD File Offset: 0x001078AD
	protected void NavigateForward()
	{
		this.OnNavigateForward();
	}

	// Token: 0x0600A28D RID: 41613 RVA: 0x001096BA File Offset: 0x001078BA
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (e.TryConsume(global::Action.MouseRight))
		{
			this.NavigateBackward();
		}
		base.OnKeyDown(e);
	}
}
