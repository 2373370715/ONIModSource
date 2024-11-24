namespace Database;

public readonly struct BalloonOverrideSymbol
{
	public readonly Option<KAnim.Build.Symbol> symbol;

	public readonly Option<KAnimFile> animFile;

	public readonly string animFileID;

	public readonly string animFileSymbolID;

	public BalloonOverrideSymbol(string animFileID, string animFileSymbolID)
	{
		if (string.IsNullOrEmpty(animFileID) || string.IsNullOrEmpty(animFileSymbolID))
		{
			this = default(BalloonOverrideSymbol);
			return;
		}
		this.animFileID = animFileID;
		this.animFileSymbolID = animFileSymbolID;
		animFile = Assets.GetAnim(animFileID);
		symbol = animFile.Value.GetData().build.GetSymbol(animFileSymbolID);
	}

	public void ApplyTo(BalloonArtist.Instance artist)
	{
		artist.SetBalloonSymbolOverride(this);
	}

	public void ApplyTo(BalloonFX.Instance balloon)
	{
		balloon.SetBalloonSymbolOverride(this);
	}
}
