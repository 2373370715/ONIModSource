using System;

// Token: 0x02001C9B RID: 7323
public class DemoOverScreen : KModalScreen
{
	// Token: 0x060098C9 RID: 39113 RVA: 0x001036F2 File Offset: 0x001018F2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Init();
		PlayerController.Instance.ActivateTool(SelectTool.Instance);
		SelectTool.Instance.Select(null, false);
	}

	// Token: 0x060098CA RID: 39114 RVA: 0x0010371B File Offset: 0x0010191B
	private void Init()
	{
		this.QuitButton.onClick += delegate()
		{
			this.Quit();
		};
	}

	// Token: 0x060098CB RID: 39115 RVA: 0x00103734 File Offset: 0x00101934
	private void Quit()
	{
		PauseScreen.TriggerQuitGame();
	}

	// Token: 0x04007708 RID: 30472
	public KButton QuitButton;
}
