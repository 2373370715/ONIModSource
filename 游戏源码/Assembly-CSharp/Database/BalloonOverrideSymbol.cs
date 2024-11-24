using System;

namespace Database
{
	// Token: 0x0200211A RID: 8474
	public readonly struct BalloonOverrideSymbol
	{
		// Token: 0x0600B407 RID: 46087 RVA: 0x0043D8E8 File Offset: 0x0043BAE8
		public BalloonOverrideSymbol(string animFileID, string animFileSymbolID)
		{
			if (string.IsNullOrEmpty(animFileID) || string.IsNullOrEmpty(animFileSymbolID))
			{
				this = default(BalloonOverrideSymbol);
				return;
			}
			this.animFileID = animFileID;
			this.animFileSymbolID = animFileSymbolID;
			this.animFile = Assets.GetAnim(animFileID);
			this.symbol = this.animFile.Value.GetData().build.GetSymbol(animFileSymbolID);
		}

		// Token: 0x0600B408 RID: 46088 RVA: 0x00114BA2 File Offset: 0x00112DA2
		public void ApplyTo(BalloonArtist.Instance artist)
		{
			artist.SetBalloonSymbolOverride(this);
		}

		// Token: 0x0600B409 RID: 46089 RVA: 0x00114BB0 File Offset: 0x00112DB0
		public void ApplyTo(BalloonFX.Instance balloon)
		{
			balloon.SetBalloonSymbolOverride(this);
		}

		// Token: 0x04008E7D RID: 36477
		public readonly Option<KAnim.Build.Symbol> symbol;

		// Token: 0x04008E7E RID: 36478
		public readonly Option<KAnimFile> animFile;

		// Token: 0x04008E7F RID: 36479
		public readonly string animFileID;

		// Token: 0x04008E80 RID: 36480
		public readonly string animFileSymbolID;
	}
}
