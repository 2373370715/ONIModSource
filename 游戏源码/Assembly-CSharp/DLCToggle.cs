using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001C91 RID: 7313
public class DLCToggle : KMonoBehaviour
{
	// Token: 0x06009876 RID: 39030 RVA: 0x001032CA File Offset: 0x001014CA
	protected override void OnPrefabInit()
	{
		this.expansion1Active = DlcManager.IsExpansion1Active();
	}

	// Token: 0x06009877 RID: 39031 RVA: 0x003AFC7C File Offset: 0x003ADE7C
	public void ToggleExpansion1Cicked()
	{
		Util.KInstantiateUI<InfoDialogScreen>(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, base.GetComponentInParent<Canvas>().gameObject, true).AddDefaultCancel().SetHeader(this.expansion1Active ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1 : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1).AddSprite(this.expansion1Active ? GlobalResources.Instance().baseGameLogoSmall : GlobalResources.Instance().expansion1LogoSmall).AddPlainText(this.expansion1Active ? UI.FRONTEND.MAINMENU.DLC.DEACTIVATE_EXPANSION1_DESC : UI.FRONTEND.MAINMENU.DLC.ACTIVATE_EXPANSION1_DESC).AddOption(UI.CONFIRMDIALOG.OK, delegate(InfoDialogScreen screen)
		{
			DlcManager.ToggleDLC("EXPANSION1_ID");
		}, true);
	}

	// Token: 0x040076BC RID: 30396
	private bool expansion1Active;
}
