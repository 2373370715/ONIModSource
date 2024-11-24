using System;

// Token: 0x02001CE0 RID: 7392
public class GameOverScreen : KModalScreen
{
	// Token: 0x06009A59 RID: 39513 RVA: 0x001046C8 File Offset: 0x001028C8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Init();
	}

	// Token: 0x06009A5A RID: 39514 RVA: 0x003B8EB8 File Offset: 0x003B70B8
	private void Init()
	{
		if (this.QuitButton)
		{
			this.QuitButton.onClick += delegate()
			{
				this.Quit();
			};
		}
		if (this.DismissButton)
		{
			this.DismissButton.onClick += delegate()
			{
				this.Dismiss();
			};
		}
	}

	// Token: 0x06009A5B RID: 39515 RVA: 0x00103734 File Offset: 0x00101934
	private void Quit()
	{
		PauseScreen.TriggerQuitGame();
	}

	// Token: 0x06009A5C RID: 39516 RVA: 0x000FE04E File Offset: 0x000FC24E
	private void Dismiss()
	{
		this.Show(false);
	}

	// Token: 0x04007879 RID: 30841
	public KButton DismissButton;

	// Token: 0x0400787A RID: 30842
	public KButton QuitButton;
}
