using System.Collections.Generic;
using Database;
using UnityEngine;

public class BlueprintCollection
{
	public List<ArtableInfo> artables = new List<ArtableInfo>();

	public List<BuildingFacadeInfo> buildingFacades = new List<BuildingFacadeInfo>();

	public List<ClothingItemInfo> clothingItems = new List<ClothingItemInfo>();

	public List<BalloonArtistFacadeInfo> balloonArtistFacades = new List<BalloonArtistFacadeInfo>();

	public List<StickerBombFacadeInfo> stickerBombFacades = new List<StickerBombFacadeInfo>();

	public List<EquippableFacadeInfo> equippableFacades = new List<EquippableFacadeInfo>();

	public List<MonumentPartInfo> monumentParts = new List<MonumentPartInfo>();

	public List<ClothingOutfitResource> outfits = new List<ClothingOutfitResource>();

	public void AddBlueprintsFrom<T>(T provider) where T : BlueprintProvider
	{
		provider.blueprintCollection = this;
		provider.Interal_PreSetupBlueprints();
		provider.SetupBlueprints();
	}

	public void AddBlueprintsFrom(BlueprintCollection collection)
	{
		artables.AddRange(collection.artables);
		buildingFacades.AddRange(collection.buildingFacades);
		clothingItems.AddRange(collection.clothingItems);
		balloonArtistFacades.AddRange(collection.balloonArtistFacades);
		stickerBombFacades.AddRange(collection.stickerBombFacades);
		equippableFacades.AddRange(collection.equippableFacades);
		monumentParts.AddRange(collection.monumentParts);
		outfits.AddRange(collection.outfits);
	}

	public void PostProcess()
	{
		if (Application.isPlaying)
		{
			artables.RemoveAll(ShouldExcludeBlueprint);
			buildingFacades.RemoveAll(ShouldExcludeBlueprint);
			clothingItems.RemoveAll(ShouldExcludeBlueprint);
			balloonArtistFacades.RemoveAll(ShouldExcludeBlueprint);
			stickerBombFacades.RemoveAll(ShouldExcludeBlueprint);
			equippableFacades.RemoveAll(ShouldExcludeBlueprint);
			monumentParts.RemoveAll(ShouldExcludeBlueprint);
			outfits.RemoveAll(ShouldExcludeBlueprint);
		}
		static bool ShouldExcludeBlueprint(IBlueprintDlcInfo blueprintDlcInfo)
		{
			if (!DlcManager.HasAnyContentSubscribed(blueprintDlcInfo.dlcIds))
			{
				return true;
			}
			if (blueprintDlcInfo is IBlueprintInfo blueprintInfo && !Assets.TryGetAnim(blueprintInfo.animFile, out var _))
			{
				DebugUtil.DevAssert(test: false, "Couldnt find anim \"" + blueprintInfo.animFile + "\" for blueprint \"" + blueprintInfo.id + "\"");
			}
			return false;
		}
	}
}
