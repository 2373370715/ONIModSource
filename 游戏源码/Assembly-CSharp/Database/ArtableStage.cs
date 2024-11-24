using STRINGS;

namespace Database;

public class ArtableStage : PermitResource
{
	public string id;

	public string anim;

	public string animFile;

	public string prefabId;

	public string symbolName;

	public int decor;

	public bool cheerOnComplete;

	public ArtableStatusItem statusItem;

	public ArtableStage(string id, string name, string desc, PermitRarity rarity, string animFile, string anim, int decor_value, bool cheer_on_complete, ArtableStatusItem status_item, string prefabId, string symbolName, string[] dlcIds)
		: base(id, name, desc, PermitCategory.Artwork, rarity, dlcIds)
	{
		this.id = id;
		this.animFile = animFile;
		this.anim = anim;
		this.symbolName = symbolName;
		decor = decor_value;
		cheerOnComplete = cheer_on_complete;
		statusItem = status_item;
		this.prefabId = prefabId;
	}

	public override PermitPresentationInfo GetPermitPresentationInfo()
	{
		PermitPresentationInfo result = default(PermitPresentationInfo);
		result.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(animFile));
		result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.ARTABLE_ITEM_FACADE_FOR.Replace("{ConfigProperName}", Assets.GetPrefab(prefabId).GetProperName()).Replace("{ArtableQuality}", statusItem.GetName(null)));
		return result;
	}
}
