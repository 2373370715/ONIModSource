using UnityEngine;

namespace Database;

public class ClothingOutfitResource : Resource, IBlueprintDlcInfo
{
	public ClothingOutfitUtility.OutfitType outfitType;

	public string[] itemsInOutfit { get; private set; }

	public string[] dlcIds { get; set; } = DlcManager.AVAILABLE_ALL_VERSIONS;


	public ClothingOutfitResource(string id, string[] items_in_outfit, string name, ClothingOutfitUtility.OutfitType outfitType)
		: base(id, name)
	{
		itemsInOutfit = items_in_outfit;
		this.outfitType = outfitType;
	}

	public Tuple<Sprite, Color> GetUISprite()
	{
		Sprite sprite = Assets.GetSprite("unknown");
		return new Tuple<Sprite, Color>(sprite, (sprite != null) ? Color.white : Color.clear);
	}

	public string GetDlcIdFrom()
	{
		if (dlcIds == DlcManager.AVAILABLE_ALL_VERSIONS || dlcIds == DlcManager.AVAILABLE_VANILLA_ONLY)
		{
			return null;
		}
		if (dlcIds.Length == 0)
		{
			return null;
		}
		return dlcIds[0];
	}
}
