using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02002150 RID: 8528
	public class PermitResources : ResourceSet<PermitResource>
	{
		// Token: 0x0600B5AA RID: 46506 RVA: 0x00452798 File Offset: 0x00450998
		public PermitResources(ResourceSet parent) : base("PermitResources", parent)
		{
			this.Root = new ResourceSet<Resource>("Root", null);
			this.Permits = new Dictionary<string, IEnumerable<PermitResource>>();
			this.BuildingFacades = new BuildingFacades(this.Root);
			this.Permits.Add(this.BuildingFacades.Id, this.BuildingFacades.resources);
			this.EquippableFacades = new EquippableFacades(this.Root);
			this.Permits.Add(this.EquippableFacades.Id, this.EquippableFacades.resources);
			this.ArtableStages = new ArtableStages(this.Root);
			this.Permits.Add(this.ArtableStages.Id, this.ArtableStages.resources);
			this.StickerBombs = new StickerBombs(this.Root);
			this.Permits.Add(this.StickerBombs.Id, this.StickerBombs.resources);
			this.ClothingItems = new ClothingItems(this.Root);
			this.ClothingOutfits = new ClothingOutfits(this.Root, this.ClothingItems);
			this.Permits.Add(this.ClothingItems.Id, this.ClothingItems.resources);
			this.BalloonArtistFacades = new BalloonArtistFacades(this.Root);
			this.Permits.Add(this.BalloonArtistFacades.Id, this.BalloonArtistFacades.resources);
			this.MonumentParts = new MonumentParts(this.Root);
			foreach (IEnumerable<PermitResource> collection in this.Permits.Values)
			{
				this.resources.AddRange(collection);
			}
		}

		// Token: 0x0600B5AB RID: 46507 RVA: 0x0011529B File Offset: 0x0011349B
		public void PostProcess()
		{
			this.BuildingFacades.PostProcess();
		}

		// Token: 0x0400939F RID: 37791
		public ResourceSet Root;

		// Token: 0x040093A0 RID: 37792
		public BuildingFacades BuildingFacades;

		// Token: 0x040093A1 RID: 37793
		public EquippableFacades EquippableFacades;

		// Token: 0x040093A2 RID: 37794
		public ArtableStages ArtableStages;

		// Token: 0x040093A3 RID: 37795
		public StickerBombs StickerBombs;

		// Token: 0x040093A4 RID: 37796
		public ClothingItems ClothingItems;

		// Token: 0x040093A5 RID: 37797
		public ClothingOutfits ClothingOutfits;

		// Token: 0x040093A6 RID: 37798
		public MonumentParts MonumentParts;

		// Token: 0x040093A7 RID: 37799
		public BalloonArtistFacades BalloonArtistFacades;

		// Token: 0x040093A8 RID: 37800
		public Dictionary<string, IEnumerable<PermitResource>> Permits;
	}
}
