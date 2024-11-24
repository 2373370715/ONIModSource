using System;
using Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D61 RID: 7521
public class KleiPermitDioramaVis_Fallback : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009D10 RID: 40208 RVA: 0x000C9F3A File Offset: 0x000C813A
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009D11 RID: 40209 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void ConfigureSetup()
	{
	}

	// Token: 0x06009D12 RID: 40210 RVA: 0x001061DC File Offset: 0x001043DC
	public void ConfigureWith(PermitResource permit)
	{
		this.sprite.sprite = PermitPresentationInfo.GetUnknownSprite();
		this.editorOnlyErrorMessageParent.gameObject.SetActive(false);
	}

	// Token: 0x06009D13 RID: 40211 RVA: 0x001061FF File Offset: 0x001043FF
	public KleiPermitDioramaVis_Fallback WithError(string error)
	{
		this.error = error;
		global::Debug.Log("[KleiInventoryScreen Error] Had to use fallback vis. " + error);
		return this;
	}

	// Token: 0x04007B14 RID: 31508
	[SerializeField]
	private Image sprite;

	// Token: 0x04007B15 RID: 31509
	[SerializeField]
	private RectTransform editorOnlyErrorMessageParent;

	// Token: 0x04007B16 RID: 31510
	[SerializeField]
	private TextMeshProUGUI editorOnlyErrorMessageText;

	// Token: 0x04007B17 RID: 31511
	private Option<string> error;
}
