using System;

// Token: 0x02001F02 RID: 7938
public class SaveActive : KScreen
{
	// Token: 0x0600A74F RID: 42831 RVA: 0x0010C910 File Offset: 0x0010AB10
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.SetAutoSaveCallbacks(new Game.SavingPreCB(this.ActiveateSaveIndicator), new Game.SavingActiveCB(this.SetActiveSaveIndicator), new Game.SavingPostCB(this.DeactivateSaveIndicator));
	}

	// Token: 0x0600A750 RID: 42832 RVA: 0x0010C946 File Offset: 0x0010AB46
	private void DoCallBack(HashedString name)
	{
		this.controller.onAnimComplete -= this.DoCallBack;
		this.readyForSaveCallback();
		this.readyForSaveCallback = null;
	}

	// Token: 0x0600A751 RID: 42833 RVA: 0x0010C971 File Offset: 0x0010AB71
	private void ActiveateSaveIndicator(Game.CansaveCB cb)
	{
		this.readyForSaveCallback = cb;
		this.controller.onAnimComplete += this.DoCallBack;
		this.controller.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0600A752 RID: 42834 RVA: 0x0010C9B1 File Offset: 0x0010ABB1
	private void SetActiveSaveIndicator()
	{
		this.controller.Play("working_loop", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0600A753 RID: 42835 RVA: 0x0010C9D3 File Offset: 0x0010ABD3
	private void DeactivateSaveIndicator()
	{
		this.controller.Play("working_pst", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0600A754 RID: 42836 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void OnKeyDown(KButtonEvent e)
	{
	}

	// Token: 0x0400838F RID: 33679
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x04008390 RID: 33680
	private Game.CansaveCB readyForSaveCallback;
}
