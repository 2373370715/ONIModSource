using Database;
using UnityEngine;
using UnityEngine.UI;

public class KleiPermitDioramaVis_DupeEquipment : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	[SerializeField]
	private UIMannequin uiMannequin;

	[Header("Diorama Backgrounds")]
	[SerializeField]
	private Image dioramaBGImage;

	[SerializeField]
	private Sprite clothingBG;

	[SerializeField]
	private Sprite atmosuitBG;

	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	public void ConfigureSetup()
	{
		uiMannequin.shouldShowOutfitWithDefaultItems = false;
	}

	public void ConfigureWith(PermitResource permit)
	{
		if (permit is ClothingItemResource clothingItemResource)
		{
			uiMannequin.SetOutfit(clothingItemResource.outfitType, new ClothingItemResource[1] { clothingItemResource });
			uiMannequin.ReactToClothingItemChange(clothingItemResource.Category);
		}
		dioramaBGImage.sprite = KleiPermitDioramaVis.GetDioramaBackground(permit.Category);
	}
}
