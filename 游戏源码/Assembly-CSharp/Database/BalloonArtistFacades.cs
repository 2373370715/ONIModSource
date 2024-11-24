using System;

namespace Database
{
	// Token: 0x02002117 RID: 8471
	public class BalloonArtistFacades : ResourceSet<BalloonArtistFacadeResource>
	{
		// Token: 0x0600B3F9 RID: 46073 RVA: 0x0043D6CC File Offset: 0x0043B8CC
		public BalloonArtistFacades(ResourceSet parent) : base("BalloonArtistFacades", parent)
		{
			foreach (BalloonArtistFacadeInfo balloonArtistFacadeInfo in Blueprints.Get().all.balloonArtistFacades)
			{
				this.Add(balloonArtistFacadeInfo.id, balloonArtistFacadeInfo.name, balloonArtistFacadeInfo.desc, balloonArtistFacadeInfo.rarity, balloonArtistFacadeInfo.animFile, balloonArtistFacadeInfo.balloonFacadeType, balloonArtistFacadeInfo.dlcIds);
			}
		}

		// Token: 0x0600B3FA RID: 46074 RVA: 0x00114B32 File Offset: 0x00112D32
		[Obsolete("Please use Add(...) with dlcIds parameter")]
		public void Add(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType)
		{
			this.Add(id, name, desc, rarity, animFile, balloonFacadeType, DlcManager.AVAILABLE_ALL_VERSIONS);
		}

		// Token: 0x0600B3FB RID: 46075 RVA: 0x0043D760 File Offset: 0x0043B960
		public void Add(string id, string name, string desc, PermitRarity rarity, string animFile, BalloonArtistFacadeType balloonFacadeType, string[] dlcIds)
		{
			BalloonArtistFacadeResource item = new BalloonArtistFacadeResource(id, name, desc, rarity, animFile, balloonFacadeType, dlcIds);
			this.resources.Add(item);
		}
	}
}
