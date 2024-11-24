using System;

// Token: 0x02001CFC RID: 7420
public class Hud : KScreen
{
	// Token: 0x06009AE2 RID: 39650 RVA: 0x00104BFF File Offset: 0x00102DFF
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Help))
		{
			GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ControlsScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
		}
	}
}
