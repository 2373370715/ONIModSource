using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Database;
using UnityEngine;

// Token: 0x0200096A RID: 2410
public class BlueprintCollection
{
	// Token: 0x06002B6F RID: 11119 RVA: 0x000BC3EF File Offset: 0x000BA5EF
	public void AddBlueprintsFrom<T>(T provider) where T : BlueprintProvider
	{
		provider.blueprintCollection = this;
		provider.Interal_PreSetupBlueprints();
		provider.SetupBlueprints();
	}

	// Token: 0x06002B70 RID: 11120 RVA: 0x001DFD68 File Offset: 0x001DDF68
	public void AddBlueprintsFrom(BlueprintCollection collection)
	{
		this.artables.AddRange(collection.artables);
		this.buildingFacades.AddRange(collection.buildingFacades);
		this.clothingItems.AddRange(collection.clothingItems);
		this.balloonArtistFacades.AddRange(collection.balloonArtistFacades);
		this.stickerBombFacades.AddRange(collection.stickerBombFacades);
		this.equippableFacades.AddRange(collection.equippableFacades);
		this.monumentParts.AddRange(collection.monumentParts);
		this.outfits.AddRange(collection.outfits);
	}

	// Token: 0x06002B71 RID: 11121 RVA: 0x001DFE00 File Offset: 0x001DE000
	public void PostProcess()
	{
		if (Application.isPlaying)
		{
			this.artables.RemoveAll(new Predicate<ArtableInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.buildingFacades.RemoveAll(new Predicate<BuildingFacadeInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.clothingItems.RemoveAll(new Predicate<ClothingItemInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.balloonArtistFacades.RemoveAll(new Predicate<BalloonArtistFacadeInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.stickerBombFacades.RemoveAll(new Predicate<StickerBombFacadeInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.equippableFacades.RemoveAll(new Predicate<EquippableFacadeInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.monumentParts.RemoveAll(new Predicate<MonumentPartInfo>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
			this.outfits.RemoveAll(new Predicate<ClothingOutfitResource>(BlueprintCollection.<PostProcess>g__ShouldExcludeBlueprint|10_0));
		}
	}

	// Token: 0x06002B73 RID: 11123 RVA: 0x001DFF44 File Offset: 0x001DE144
	[CompilerGenerated]
	internal static bool <PostProcess>g__ShouldExcludeBlueprint|10_0(IBlueprintDlcInfo blueprintDlcInfo)
	{
		if (!DlcManager.IsAnyContentSubscribed(blueprintDlcInfo.dlcIds))
		{
			return true;
		}
		IBlueprintInfo blueprintInfo = blueprintDlcInfo as IBlueprintInfo;
		KAnimFile kanimFile;
		if (blueprintInfo != null && !Assets.TryGetAnim(blueprintInfo.animFile, out kanimFile))
		{
			DebugUtil.DevAssert(false, string.Concat(new string[]
			{
				"Couldnt find anim \"",
				blueprintInfo.animFile,
				"\" for blueprint \"",
				blueprintInfo.id,
				"\""
			}), null);
		}
		return false;
	}

	// Token: 0x04001D32 RID: 7474
	public List<ArtableInfo> artables = new List<ArtableInfo>();

	// Token: 0x04001D33 RID: 7475
	public List<BuildingFacadeInfo> buildingFacades = new List<BuildingFacadeInfo>();

	// Token: 0x04001D34 RID: 7476
	public List<ClothingItemInfo> clothingItems = new List<ClothingItemInfo>();

	// Token: 0x04001D35 RID: 7477
	public List<BalloonArtistFacadeInfo> balloonArtistFacades = new List<BalloonArtistFacadeInfo>();

	// Token: 0x04001D36 RID: 7478
	public List<StickerBombFacadeInfo> stickerBombFacades = new List<StickerBombFacadeInfo>();

	// Token: 0x04001D37 RID: 7479
	public List<EquippableFacadeInfo> equippableFacades = new List<EquippableFacadeInfo>();

	// Token: 0x04001D38 RID: 7480
	public List<MonumentPartInfo> monumentParts = new List<MonumentPartInfo>();

	// Token: 0x04001D39 RID: 7481
	public List<ClothingOutfitResource> outfits = new List<ClothingOutfitResource>();
}
