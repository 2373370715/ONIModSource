using System;
using Database;

// Token: 0x02000974 RID: 2420
public abstract class BlueprintProvider
{
	// Token: 0x06002BDB RID: 11227 RVA: 0x001E01B4 File Offset: 0x001DE3B4
	protected void AddBuilding(string prefabConfigId, PermitRarity rarity, string permitId, string animFile)
	{
		this.blueprintCollection.buildingFacades.Add(new BuildingFacadeInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), rarity, prefabConfigId, animFile, this.dlcIds, null));
	}

	// Token: 0x06002BDC RID: 11228 RVA: 0x001E0220 File Offset: 0x001DE420
	protected void AddClothing(BlueprintProvider.ClothingType clothingType, PermitRarity rarity, string permitId, string animFile)
	{
		this.blueprintCollection.clothingItems.Add(new ClothingItemInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), (PermitCategory)clothingType, rarity, animFile)
		{
			dlcIds = this.dlcIds
		});
	}

	// Token: 0x06002BDD RID: 11229 RVA: 0x001E0294 File Offset: 0x001DE494
	protected BlueprintProvider.ArtableInfoAuthoringHelper AddArtable(BlueprintProvider.ArtableType artableType, PermitRarity rarity, string permitId, string animFile)
	{
		string text;
		switch (artableType)
		{
		case BlueprintProvider.ArtableType.Painting:
			text = "Canvas";
			break;
		case BlueprintProvider.ArtableType.PaintingTall:
			text = "CanvasTall";
			break;
		case BlueprintProvider.ArtableType.PaintingWide:
			text = "CanvasWide";
			break;
		case BlueprintProvider.ArtableType.Sculpture:
			text = "Sculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureSmall:
			text = "SmallSculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureIce:
			text = "IceSculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureMetal:
			text = "MetalSculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureMarble:
			text = "MarbleSculpture";
			break;
		case BlueprintProvider.ArtableType.SculptureWood:
			text = "WoodSculpture";
			break;
		default:
			text = null;
			break;
		}
		bool flag = true;
		if (text == null)
		{
			DebugUtil.DevAssert(false, "Failed to get buildingConfigId from " + artableType.ToString(), null);
			flag = false;
		}
		BlueprintProvider.ArtableInfoAuthoringHelper result;
		if (flag)
		{
			KAnimFile kanimFile;
			ArtableInfo artableInfo = new ArtableInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), rarity, animFile, (!Assets.TryGetAnim(animFile, out kanimFile)) ? null : kanimFile.GetData().GetAnim(0).name, 0, false, "error", text, "")
			{
				dlcIds = this.dlcIds
			};
			result = new BlueprintProvider.ArtableInfoAuthoringHelper(artableType, artableInfo);
			result.Quality(BlueprintProvider.ArtableQuality.LookingGreat);
			this.blueprintCollection.artables.Add(artableInfo);
		}
		else
		{
			result = default(BlueprintProvider.ArtableInfoAuthoringHelper);
		}
		return result;
	}

	// Token: 0x06002BDE RID: 11230 RVA: 0x001E03F8 File Offset: 0x001DE5F8
	protected void AddJoyResponse(BlueprintProvider.JoyResponseType joyResponseType, PermitRarity rarity, string permitId, string animFile)
	{
		if (joyResponseType == BlueprintProvider.JoyResponseType.BallonSet)
		{
			this.blueprintCollection.balloonArtistFacades.Add(new BalloonArtistFacadeInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), rarity, animFile, BalloonArtistFacadeType.ThreeSet)
			{
				dlcIds = this.dlcIds
			});
			return;
		}
		throw new NotImplementedException("Missing case for " + joyResponseType.ToString());
	}

	// Token: 0x06002BDF RID: 11231 RVA: 0x001E048C File Offset: 0x001DE68C
	protected void AddOutfit(BlueprintProvider.OutfitType outfitType, string outfitId, string[] permitIdList)
	{
		this.blueprintCollection.outfits.Add(new ClothingOutfitResource(outfitId, permitIdList, Strings.Get("STRINGS.BLUEPRINTS." + outfitId.ToUpper() + ".NAME"), (ClothingOutfitUtility.OutfitType)outfitType)
		{
			dlcIds = this.dlcIds
		});
	}

	// Token: 0x06002BE0 RID: 11232 RVA: 0x000A6F3E File Offset: 0x000A513E
	public virtual string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06002BE1 RID: 11233
	public abstract void SetupBlueprints();

	// Token: 0x06002BE2 RID: 11234 RVA: 0x000BC75D File Offset: 0x000BA95D
	public void Interal_PreSetupBlueprints()
	{
		this.dlcIds = this.GetDlcIds();
	}

	// Token: 0x04001D75 RID: 7541
	public BlueprintCollection blueprintCollection;

	// Token: 0x04001D76 RID: 7542
	private string[] dlcIds;

	// Token: 0x02000975 RID: 2421
	public enum ArtableType
	{
		// Token: 0x04001D78 RID: 7544
		Painting,
		// Token: 0x04001D79 RID: 7545
		PaintingTall,
		// Token: 0x04001D7A RID: 7546
		PaintingWide,
		// Token: 0x04001D7B RID: 7547
		Sculpture,
		// Token: 0x04001D7C RID: 7548
		SculptureSmall,
		// Token: 0x04001D7D RID: 7549
		SculptureIce,
		// Token: 0x04001D7E RID: 7550
		SculptureMetal,
		// Token: 0x04001D7F RID: 7551
		SculptureMarble,
		// Token: 0x04001D80 RID: 7552
		SculptureWood
	}

	// Token: 0x02000976 RID: 2422
	public enum ArtableQuality
	{
		// Token: 0x04001D82 RID: 7554
		LookingGreat,
		// Token: 0x04001D83 RID: 7555
		LookingOkay,
		// Token: 0x04001D84 RID: 7556
		LookingUgly
	}

	// Token: 0x02000977 RID: 2423
	public enum ClothingType
	{
		// Token: 0x04001D86 RID: 7558
		DupeTops = 1,
		// Token: 0x04001D87 RID: 7559
		DupeBottoms,
		// Token: 0x04001D88 RID: 7560
		DupeGloves,
		// Token: 0x04001D89 RID: 7561
		DupeShoes,
		// Token: 0x04001D8A RID: 7562
		DupeHats,
		// Token: 0x04001D8B RID: 7563
		DupeAccessories,
		// Token: 0x04001D8C RID: 7564
		AtmoSuitHelmet,
		// Token: 0x04001D8D RID: 7565
		AtmoSuitBody,
		// Token: 0x04001D8E RID: 7566
		AtmoSuitGloves,
		// Token: 0x04001D8F RID: 7567
		AtmoSuitBelt,
		// Token: 0x04001D90 RID: 7568
		AtmoSuitShoes
	}

	// Token: 0x02000978 RID: 2424
	public enum OutfitType
	{
		// Token: 0x04001D92 RID: 7570
		Clothing,
		// Token: 0x04001D93 RID: 7571
		AtmoSuit = 2
	}

	// Token: 0x02000979 RID: 2425
	public enum JoyResponseType
	{
		// Token: 0x04001D95 RID: 7573
		BallonSet
	}

	// Token: 0x0200097A RID: 2426
	protected readonly ref struct ArtableInfoAuthoringHelper
	{
		// Token: 0x06002BE4 RID: 11236 RVA: 0x000BC76B File Offset: 0x000BA96B
		public ArtableInfoAuthoringHelper(BlueprintProvider.ArtableType artableType, ArtableInfo artableInfo)
		{
			this.artableType = artableType;
			this.artableInfo = artableInfo;
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x001E04DC File Offset: 0x001DE6DC
		public void Quality(BlueprintProvider.ArtableQuality artableQuality)
		{
			if (this.artableInfo == null)
			{
				return;
			}
			int num;
			int num2;
			int num3;
			if (this.artableType == BlueprintProvider.ArtableType.SculptureWood)
			{
				num = 4;
				num2 = 8;
				num3 = 12;
			}
			else
			{
				num = 5;
				num2 = 10;
				num3 = 15;
			}
			int decor_value;
			bool cheer_on_complete;
			string status_id;
			switch (artableQuality)
			{
			case BlueprintProvider.ArtableQuality.LookingGreat:
				decor_value = num3;
				cheer_on_complete = true;
				status_id = "LookingGreat";
				break;
			case BlueprintProvider.ArtableQuality.LookingOkay:
				decor_value = num2;
				cheer_on_complete = false;
				status_id = "LookingOkay";
				break;
			case BlueprintProvider.ArtableQuality.LookingUgly:
				decor_value = num;
				cheer_on_complete = false;
				status_id = "LookingUgly";
				break;
			default:
				throw new ArgumentException();
			}
			this.artableInfo.decor_value = decor_value;
			this.artableInfo.cheer_on_complete = cheer_on_complete;
			this.artableInfo.status_id = status_id;
		}

		// Token: 0x04001D96 RID: 7574
		private readonly BlueprintProvider.ArtableType artableType;

		// Token: 0x04001D97 RID: 7575
		private readonly ArtableInfo artableInfo;
	}
}
