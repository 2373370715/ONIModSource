using System.Collections.Generic;

namespace Database;

public class PermitResources : ResourceSet<PermitResource>
{
	public ResourceSet Root;

	public BuildingFacades BuildingFacades;

	public EquippableFacades EquippableFacades;

	public ArtableStages ArtableStages;

	public StickerBombs StickerBombs;

	public ClothingItems ClothingItems;

	public ClothingOutfits ClothingOutfits;

	public MonumentParts MonumentParts;

	public BalloonArtistFacades BalloonArtistFacades;

	public Dictionary<string, IEnumerable<PermitResource>> Permits;

	public PermitResources(ResourceSet parent)
		: base("PermitResources", parent)
	{
		Root = new ResourceSet<Resource>("Root", null);
		Permits = new Dictionary<string, IEnumerable<PermitResource>>();
		BuildingFacades = new BuildingFacades(Root);
		Permits.Add(BuildingFacades.Id, BuildingFacades.resources);
		EquippableFacades = new EquippableFacades(Root);
		Permits.Add(EquippableFacades.Id, EquippableFacades.resources);
		ArtableStages = new ArtableStages(Root);
		Permits.Add(ArtableStages.Id, ArtableStages.resources);
		StickerBombs = new StickerBombs(Root);
		Permits.Add(StickerBombs.Id, StickerBombs.resources);
		ClothingItems = new ClothingItems(Root);
		ClothingOutfits = new ClothingOutfits(Root, ClothingItems);
		Permits.Add(ClothingItems.Id, ClothingItems.resources);
		BalloonArtistFacades = new BalloonArtistFacades(Root);
		Permits.Add(BalloonArtistFacades.Id, BalloonArtistFacades.resources);
		MonumentParts = new MonumentParts(Root);
		foreach (IEnumerable<PermitResource> value in Permits.Values)
		{
			resources.AddRange(value);
		}
	}

	public void PostProcess()
	{
		BuildingFacades.PostProcess();
	}
}
