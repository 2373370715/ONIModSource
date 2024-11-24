using STRINGS;
using UnityEngine;

public class DLCToggle : KMonoBehaviour
{
	private bool expansion1Active;

	protected override void OnPrefabInit()
	{
		expansion1Active = DlcManager.IsExpansion1Active();
	}

	public void ToggleExpansion1Cicked()
	{
		Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GetComponentInParent<Canvas>().gameObject, force_active: true).AddDefaultCancel().SetHeader(expansion1Active ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1 : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1)
			.AddSprite(expansion1Active ? GlobalResources.Instance().baseGameLogoSmall : GlobalResources.Instance().expansion1LogoSmall)
			.AddPlainText(expansion1Active ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1_DESC : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1_DESC)
			.AddOption(UI.CONFIRMDIALOG.OK, delegate
			{
				DlcManager.ToggleDLC("EXPANSION1_ID");
			}, rightSide: true);
	}
}
