using UnityEngine;

namespace Database;

public class BalloonOverrideSymbolIter
{
	public readonly Option<BalloonArtistFacadeResource> facade;

	private BalloonOverrideSymbol current;

	private int index;

	public BalloonOverrideSymbolIter(Option<BalloonArtistFacadeResource> facade)
	{
		Debug.Assert(facade.IsNone() || facade.Unwrap().balloonOverrideSymbolIDs.Length != 0);
		this.facade = facade;
		if (facade.IsSome())
		{
			index = Random.Range(0, facade.Unwrap().balloonOverrideSymbolIDs.Length);
		}
		Next();
	}

	public BalloonOverrideSymbol Current()
	{
		return current;
	}

	public BalloonOverrideSymbol Next()
	{
		if (facade.IsSome())
		{
			BalloonArtistFacadeResource balloonArtistFacadeResource = facade.Unwrap();
			current = new BalloonOverrideSymbol(balloonArtistFacadeResource.animFilename, balloonArtistFacadeResource.balloonOverrideSymbolIDs[index]);
			index = (index + 1) % balloonArtistFacadeResource.balloonOverrideSymbolIDs.Length;
			return current;
		}
		return default(BalloonOverrideSymbol);
	}
}
