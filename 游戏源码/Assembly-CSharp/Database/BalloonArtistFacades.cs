using System;

namespace Database;

public class BalloonArtistFacades : ResourceSet<BalloonArtistFacadeResource>
{
	public BalloonArtistFacades(ResourceSet parent)
		: base("BalloonArtistFacades", parent)
	{
		foreach (BalloonArtistFacadeInfo balloonArtistFacade in Blueprints.Get().all.balloonArtistFacades)
		{
			Add(balloonArtistFacade.id, balloonArtistFacade.name, balloonArtistFacade.desc, balloonArtistFacade.rarity, balloonArtistFacade.animFile, balloonArtistFacade.balloonFacadeType, balloonArtistFacade.dlcIds);
		}
	}

	[Obsolete("Please use Add(...) with dlcIds parameter")]
	public void Add(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType)
	{
		Add(id, name, desc, rarity, animFile, balloonFacadeType, DlcManager.AVAILABLE_ALL_VERSIONS);
	}

	public void Add(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType, string[] dlcIds)
	{
		BalloonArtistFacadeResource item = new BalloonArtistFacadeResource(id, name, desc, rarity, animFile, balloonFacadeType, dlcIds);
		resources.Add(item);
	}
}
