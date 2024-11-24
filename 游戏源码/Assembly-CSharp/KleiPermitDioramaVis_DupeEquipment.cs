using System;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D60 RID: 7520
public class KleiPermitDioramaVis_DupeEquipment : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009D0C RID: 40204 RVA: 0x000C9F3A File Offset: 0x000C813A
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009D0D RID: 40205 RVA: 0x001061CE File Offset: 0x001043CE
	public void ConfigureSetup()
	{
		this.uiMannequin.shouldShowOutfitWithDefaultItems = false;
	}

	// Token: 0x06009D0E RID: 40206 RVA: 0x003C6608 File Offset: 0x003C4808
	public void ConfigureWith(PermitResource permit)
	{
		ClothingItemResource clothingItemResource = permit as ClothingItemResource;
		if (clothingItemResource != null)
		{
			this.uiMannequin.SetOutfit(clothingItemResource.outfitType, new ClothingItemResource[]
			{
				clothingItemResource
			});
			this.uiMannequin.ReactToClothingItemChange(clothingItemResource.Category);
		}
		this.dioramaBGImage.sprite = KleiPermitDioramaVis.GetDioramaBackground(permit.Category);
	}

	// Token: 0x04007B10 RID: 31504
	[SerializeField]
	private UIMannequin uiMannequin;

	// Token: 0x04007B11 RID: 31505
	[Header("Diorama Backgrounds")]
	[SerializeField]
	private Image dioramaBGImage;

	// Token: 0x04007B12 RID: 31506
	[SerializeField]
	private Sprite clothingBG;

	// Token: 0x04007B13 RID: 31507
	[SerializeField]
	private Sprite atmosuitBG;
}
