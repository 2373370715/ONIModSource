using System;
using UnityEngine;

namespace Database
{
	// Token: 0x0200211B RID: 8475
	public class BalloonOverrideSymbolIter
	{
		// Token: 0x0600B40A RID: 46090 RVA: 0x0043D95C File Offset: 0x0043BB5C
		public BalloonOverrideSymbolIter(Option<BalloonArtistFacadeResource> facade)
		{
			global::Debug.Assert(facade.IsNone() || facade.Unwrap().balloonOverrideSymbolIDs.Length != 0);
			this.facade = facade;
			if (facade.IsSome())
			{
				this.index = UnityEngine.Random.Range(0, facade.Unwrap().balloonOverrideSymbolIDs.Length);
			}
			this.Next();
		}

		// Token: 0x0600B40B RID: 46091 RVA: 0x00114BBE File Offset: 0x00112DBE
		public BalloonOverrideSymbol Current()
		{
			return this.current;
		}

		// Token: 0x0600B40C RID: 46092 RVA: 0x0043D9C4 File Offset: 0x0043BBC4
		public BalloonOverrideSymbol Next()
		{
			if (this.facade.IsSome())
			{
				BalloonArtistFacadeResource balloonArtistFacadeResource = this.facade.Unwrap();
				this.current = new BalloonOverrideSymbol(balloonArtistFacadeResource.animFilename, balloonArtistFacadeResource.balloonOverrideSymbolIDs[this.index]);
				this.index = (this.index + 1) % balloonArtistFacadeResource.balloonOverrideSymbolIDs.Length;
				return this.current;
			}
			return default(BalloonOverrideSymbol);
		}

		// Token: 0x04008E81 RID: 36481
		public readonly Option<BalloonArtistFacadeResource> facade;

		// Token: 0x04008E82 RID: 36482
		private BalloonOverrideSymbol current;

		// Token: 0x04008E83 RID: 36483
		private int index;
	}
}
