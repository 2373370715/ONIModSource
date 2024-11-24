using System;
using STRINGS;

namespace Database
{
	// Token: 0x02002119 RID: 8473
	public class BalloonArtistFacadeResource : PermitResource
	{
		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x0600B3FC RID: 46076 RVA: 0x00114B48 File Offset: 0x00112D48
		// (set) Token: 0x0600B3FD RID: 46077 RVA: 0x00114B50 File Offset: 0x00112D50
		public string animFilename { get; private set; }

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x0600B3FE RID: 46078 RVA: 0x00114B59 File Offset: 0x00112D59
		// (set) Token: 0x0600B3FF RID: 46079 RVA: 0x00114B61 File Offset: 0x00112D61
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x0600B400 RID: 46080 RVA: 0x00114B6A File Offset: 0x00112D6A
		[Obsolete("Please use constructor with dlcIds parameter")]
		public BalloonArtistFacadeResource(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType) : this(id, name, desc, rarity, animFile, balloonFacadeType, DlcManager.AVAILABLE_ALL_VERSIONS)
		{
		}

		// Token: 0x0600B401 RID: 46081 RVA: 0x0043D78C File Offset: 0x0043B98C
		public BalloonArtistFacadeResource(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType, string[] dlcIds) : base(id, name, desc, PermitCategory.JoyResponse, rarity, dlcIds)
		{
			this.AnimFile = Assets.GetAnim(animFile);
			this.animFilename = animFile;
			this.balloonFacadeType = balloonFacadeType;
			Db.Get().Accessories.AddAccessories(id, this.AnimFile);
			this.balloonOverrideSymbolIDs = this.GetBalloonOverrideSymbolIDs();
			Debug.Assert(this.balloonOverrideSymbolIDs.Length != 0);
		}

		// Token: 0x0600B402 RID: 46082 RVA: 0x0043D7FC File Offset: 0x0043B9FC
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			result.SetFacadeForText(UI.KLEI_INVENTORY_SCREEN.BALLOON_ARTIST_FACADE_FOR);
			return result;
		}

		// Token: 0x0600B403 RID: 46083 RVA: 0x0043D840 File Offset: 0x0043BA40
		public BalloonOverrideSymbol GetNextOverride()
		{
			int num = this.nextSymbolIndex;
			this.nextSymbolIndex = (this.nextSymbolIndex + 1) % this.balloonOverrideSymbolIDs.Length;
			return new BalloonOverrideSymbol(this.animFilename, this.balloonOverrideSymbolIDs[num]);
		}

		// Token: 0x0600B404 RID: 46084 RVA: 0x00114B80 File Offset: 0x00112D80
		public BalloonOverrideSymbolIter GetSymbolIter()
		{
			return new BalloonOverrideSymbolIter(this);
		}

		// Token: 0x0600B405 RID: 46085 RVA: 0x00114B8D File Offset: 0x00112D8D
		public BalloonOverrideSymbol GetOverrideAt(int index)
		{
			return new BalloonOverrideSymbol(this.animFilename, this.balloonOverrideSymbolIDs[index]);
		}

		// Token: 0x0600B406 RID: 46086 RVA: 0x0043D880 File Offset: 0x0043BA80
		private string[] GetBalloonOverrideSymbolIDs()
		{
			KAnim.Build build = this.AnimFile.GetData().build;
			BalloonArtistFacadeType balloonArtistFacadeType = this.balloonFacadeType;
			string[] result;
			if (balloonArtistFacadeType != BalloonArtistFacadeType.Single)
			{
				if (balloonArtistFacadeType != BalloonArtistFacadeType.ThreeSet)
				{
					throw new NotImplementedException();
				}
				result = new string[]
				{
					"body1",
					"body2",
					"body3"
				};
			}
			else
			{
				result = new string[]
				{
					"body"
				};
			}
			return result;
		}

		// Token: 0x04008E7A RID: 36474
		private BalloonArtistFacadeType balloonFacadeType;

		// Token: 0x04008E7B RID: 36475
		public readonly string[] balloonOverrideSymbolIDs;

		// Token: 0x04008E7C RID: 36476
		public int nextSymbolIndex;
	}
}
