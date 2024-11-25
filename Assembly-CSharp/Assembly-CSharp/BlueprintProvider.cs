using System;
using Database;

public abstract class BlueprintProvider
{
		protected void AddBuilding(string prefabConfigId, PermitRarity rarity, string permitId, string animFile)
	{
		this.blueprintCollection.buildingFacades.Add(new BuildingFacadeInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), rarity, prefabConfigId, animFile, this.dlcIds, null));
	}

		protected void AddClothing(BlueprintProvider.ClothingType clothingType, PermitRarity rarity, string permitId, string animFile)
	{
		this.blueprintCollection.clothingItems.Add(new ClothingItemInfo(permitId, Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".NAME"), Strings.Get("STRINGS.BLUEPRINTS." + permitId.ToUpper() + ".DESC"), (PermitCategory)clothingType, rarity, animFile)
		{
			dlcIds = this.dlcIds
		});
	}

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

		protected void AddOutfit(BlueprintProvider.OutfitType outfitType, string outfitId, string[] permitIdList)
	{
		this.blueprintCollection.outfits.Add(new ClothingOutfitResource(outfitId, permitIdList, Strings.Get("STRINGS.BLUEPRINTS." + outfitId.ToUpper() + ".NAME"), (ClothingOutfitUtility.OutfitType)outfitType)
		{
			dlcIds = this.dlcIds
		});
	}

		public virtual string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

		public abstract void SetupBlueprints();

		public void Interal_PreSetupBlueprints()
	{
		this.dlcIds = this.GetDlcIds();
	}

		public BlueprintCollection blueprintCollection;

		private string[] dlcIds;

		public enum ArtableType
	{
				Painting,
				PaintingTall,
				PaintingWide,
				Sculpture,
				SculptureSmall,
				SculptureIce,
				SculptureMetal,
				SculptureMarble,
				SculptureWood
	}

		public enum ArtableQuality
	{
				LookingGreat,
				LookingOkay,
				LookingUgly
	}

		public enum ClothingType
	{
				DupeTops = 1,
				DupeBottoms,
				DupeGloves,
				DupeShoes,
				DupeHats,
				DupeAccessories,
				AtmoSuitHelmet,
				AtmoSuitBody,
				AtmoSuitGloves,
				AtmoSuitBelt,
				AtmoSuitShoes
	}

		public enum OutfitType
	{
				Clothing,
				AtmoSuit = 2
	}

		public enum JoyResponseType
	{
				BallonSet
	}

		protected readonly ref struct ArtableInfoAuthoringHelper
	{
				public ArtableInfoAuthoringHelper(BlueprintProvider.ArtableType artableType, ArtableInfo artableInfo)
		{
			this.artableType = artableType;
			this.artableInfo = artableInfo;
		}

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

				private readonly BlueprintProvider.ArtableType artableType;

				private readonly ArtableInfo artableInfo;
	}
}
