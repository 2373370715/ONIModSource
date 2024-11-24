using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020014BE RID: 5310
[AddComponentMenu("KMonoBehaviour/scripts/LoreBearer")]
public class LoreBearer : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x17000714 RID: 1812
	// (get) Token: 0x06006EA5 RID: 28325 RVA: 0x000E87EB File Offset: 0x000E69EB
	public string content
	{
		get
		{
			return Strings.Get("STRINGS.LORE.BUILDINGS." + base.gameObject.name + ".ENTRY");
		}
	}

	// Token: 0x06006EA6 RID: 28326 RVA: 0x000BFD08 File Offset: 0x000BDF08
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06006EA7 RID: 28327 RVA: 0x000E8811 File Offset: 0x000E6A11
	public LoreBearer Internal_SetContent(LoreBearerAction action)
	{
		this.displayContentAction = action;
		return this;
	}

	// Token: 0x06006EA8 RID: 28328 RVA: 0x000E881B File Offset: 0x000E6A1B
	public LoreBearer Internal_SetContent(LoreBearerAction action, string[] collectionsToUnlockFrom)
	{
		this.displayContentAction = action;
		this.collectionsToUnlockFrom = collectionsToUnlockFrom;
		return this;
	}

	// Token: 0x06006EA9 RID: 28329 RVA: 0x000E882C File Offset: 0x000E6A2C
	public static InfoDialogScreen ShowPopupDialog()
	{
		return (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
	}

	// Token: 0x06006EAA RID: 28330 RVA: 0x002EFC48 File Offset: 0x002EDE48
	private void OnClickRead()
	{
		InfoDialogScreen infoDialogScreen = LoreBearer.ShowPopupDialog().SetHeader(base.gameObject.GetComponent<KSelectable>().GetProperName()).AddDefaultOK(true);
		if (this.BeenClicked)
		{
			infoDialogScreen.AddPlainText(this.BeenSearched);
			return;
		}
		this.BeenClicked = true;
		if (DlcManager.IsExpansion1Active())
		{
			Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), 0, 1, "OrbitalResearchDatabank", Grid.SceneLayer.Front).SetActive(true);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab("OrbitalResearchDatabank".ToTag()).GetProperName(), base.gameObject.transform, 1.5f, false);
		}
		if (this.displayContentAction != null)
		{
			this.displayContentAction(infoDialogScreen);
			return;
		}
		LoreBearerUtil.UnlockNextJournalEntry(infoDialogScreen);
	}

	// Token: 0x17000715 RID: 1813
	// (get) Token: 0x06006EAB RID: 28331 RVA: 0x000E885C File Offset: 0x000E6A5C
	public string SidescreenButtonText
	{
		get
		{
			return this.BeenClicked ? UI.USERMENUACTIONS.READLORE.ALREADYINSPECTED : UI.USERMENUACTIONS.READLORE.NAME;
		}
	}

	// Token: 0x17000716 RID: 1814
	// (get) Token: 0x06006EAC RID: 28332 RVA: 0x000E8877 File Offset: 0x000E6A77
	public string SidescreenButtonTooltip
	{
		get
		{
			return this.BeenClicked ? UI.USERMENUACTIONS.READLORE.TOOLTIP_ALREADYINSPECTED : UI.USERMENUACTIONS.READLORE.TOOLTIP;
		}
	}

	// Token: 0x06006EAD RID: 28333 RVA: 0x000ABC75 File Offset: 0x000A9E75
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x06006EAE RID: 28334 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06006EAF RID: 28335 RVA: 0x000E8892 File Offset: 0x000E6A92
	public void OnSidescreenButtonPressed()
	{
		this.OnClickRead();
	}

	// Token: 0x06006EB0 RID: 28336 RVA: 0x000E889A File Offset: 0x000E6A9A
	public bool SidescreenButtonInteractable()
	{
		return !this.BeenClicked;
	}

	// Token: 0x06006EB1 RID: 28337 RVA: 0x000ABCBD File Offset: 0x000A9EBD
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x06006EB2 RID: 28338 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x040052B9 RID: 21177
	[Serialize]
	private bool BeenClicked;

	// Token: 0x040052BA RID: 21178
	public string BeenSearched = UI.USERMENUACTIONS.READLORE.ALREADY_SEARCHED;

	// Token: 0x040052BB RID: 21179
	private string[] collectionsToUnlockFrom;

	// Token: 0x040052BC RID: 21180
	private LoreBearerAction displayContentAction;
}
