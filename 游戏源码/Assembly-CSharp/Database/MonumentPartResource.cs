using UnityEngine;

namespace Database;

public class MonumentPartResource : PermitResource
{
	public enum Part
	{
		Bottom,
		Middle,
		Top
	}

	public Part part;

	public KAnimFile AnimFile { get; private set; }

	public string SymbolName { get; private set; }

	public string State { get; private set; }

	public MonumentPartResource(string id, string name, string desc, PermitRarity rarity, string animFilename, string state, string symbolName, Part part, string[] dlcIds)
		: base(id, name, desc, PermitCategory.Artwork, rarity, dlcIds)
	{
		AnimFile = Assets.GetAnim(animFilename);
		SymbolName = symbolName;
		State = state;
		this.part = part;
	}

	public Tuple<Sprite, Color> GetUISprite()
	{
		Sprite sprite = Assets.GetSprite("unknown");
		return new Tuple<Sprite, Color>(sprite, (sprite != null) ? Color.white : Color.clear);
	}

	public override PermitPresentationInfo GetPermitPresentationInfo()
	{
		PermitPresentationInfo result = default(PermitPresentationInfo);
		result.sprite = GetUISprite().first;
		result.SetFacadeForText("_monument part");
		return result;
	}
}
