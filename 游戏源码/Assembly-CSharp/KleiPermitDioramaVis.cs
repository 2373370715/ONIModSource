using System.Collections.Generic;
using System.Linq;
using Database;
using UnityEngine;
using UnityEngine.UI;

public class KleiPermitDioramaVis : KMonoBehaviour
{
	[SerializeField]
	private Image dlcImage;

	[SerializeField]
	private KleiPermitDioramaVis_Fallback fallbackVis;

	[SerializeField]
	private KleiPermitDioramaVis_DupeEquipment equipmentVis;

	[SerializeField]
	private KleiPermitDioramaVis_BuildingOnFloor buildingOnFloorVis;

	[SerializeField]
	private KleiPermitDioramaVis_BuildingOnFloorBig buildingOnFloorBigVis;

	[SerializeField]
	private KleiPermitDioramaVis_BuildingPresentationStand buildingOnWallVis;

	[SerializeField]
	private KleiPermitDioramaVis_BuildingPresentationStand buildingOnCeilingVis;

	[SerializeField]
	private KleiPermitDioramaVis_BuildingPresentationStand buildingInCeilingCornerVis;

	[SerializeField]
	private KleiPermitDioramaVis_BuildingRocket buildingRocketVis;

	[SerializeField]
	private KleiPermitDioramaVis_BuildingOnFloor buildingOnFloorBotanicalVis;

	[SerializeField]
	private KleiPermitDioramaVis_BuildingHangingHook buildingHangingHookBotanicalVis;

	[SerializeField]
	private KleiPermitDioramaVis_Wallpaper wallpaperVis;

	[SerializeField]
	private KleiPermitDioramaVis_ArtablePainting artablePaintingVis;

	[SerializeField]
	private KleiPermitDioramaVis_ArtableSculpture artableSculptureVis;

	[SerializeField]
	private KleiPermitDioramaVis_JoyResponseBalloon joyResponseBalloonVis;

	private bool initComplete;

	private IReadOnlyList<IKleiPermitDioramaVisTarget> allVisList;

	public static PermitResource lastRenderedPermit;

	protected override void OnPrefabInit()
	{
		Init();
	}

	private void Init()
	{
		if (initComplete)
		{
			return;
		}
		allVisList = ReflectionUtil.For(this).CollectValuesForFieldsThatInheritOrImplement<IKleiPermitDioramaVisTarget>();
		foreach (IKleiPermitDioramaVisTarget allVis in allVisList)
		{
			allVis.ConfigureSetup();
		}
		initComplete = true;
	}

	public void ConfigureWith(PermitResource permit)
	{
		if (!initComplete)
		{
			Init();
		}
		foreach (IKleiPermitDioramaVisTarget allVis in allVisList)
		{
			allVis.GetGameObject().SetActive(value: false);
		}
		KleiPermitVisUtil.ClearAnimation();
		IKleiPermitDioramaVisTarget permitVisTarget = GetPermitVisTarget(permit);
		permitVisTarget.GetGameObject().SetActive(value: true);
		permitVisTarget.ConfigureWith(permit);
		string dlcIdFrom = permit.GetDlcIdFrom();
		if (DlcManager.IsDlcId(dlcIdFrom))
		{
			dlcImage.gameObject.SetActive(value: true);
			dlcImage.sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(dlcIdFrom));
		}
		else
		{
			dlcImage.gameObject.SetActive(value: false);
		}
	}

	private IKleiPermitDioramaVisTarget GetPermitVisTarget(PermitResource permit)
	{
		lastRenderedPermit = permit;
		if (permit == null)
		{
			return fallbackVis.WithError($"Given invalid permit: {permit}");
		}
		if (permit.Category == PermitCategory.Equipment || permit.Category == PermitCategory.DupeTops || permit.Category == PermitCategory.DupeBottoms || permit.Category == PermitCategory.DupeGloves || permit.Category == PermitCategory.DupeShoes || permit.Category == PermitCategory.DupeHats || permit.Category == PermitCategory.DupeAccessories || permit.Category == PermitCategory.AtmoSuitHelmet || permit.Category == PermitCategory.AtmoSuitBody || permit.Category == PermitCategory.AtmoSuitGloves || permit.Category == PermitCategory.AtmoSuitBelt || permit.Category == PermitCategory.AtmoSuitShoes)
		{
			return equipmentVis;
		}
		if (permit.Category == PermitCategory.Building)
		{
			BuildLocationRule? buildLocationRule = KleiPermitVisUtil.GetBuildLocationRule(permit);
			if (!buildLocationRule.HasValue)
			{
				if (permit.DlcIds.SequenceEqual(DlcManager.AVAILABLE_EXPANSION1_ONLY))
				{
					return buildingOnFloorVis;
				}
				return fallbackVis.WithError("Couldn't get BuildLocationRule on permit with id \"" + permit.Id + "\"");
			}
			BuildingDef buildingDef2 = KleiPermitVisUtil.GetBuildingDef(permit);
			if (!buildingDef2.BuildingComplete.GetComponent<Bed>().IsNullOrDestroyed())
			{
				return buildingOnFloorVis;
			}
			if (buildingDef2.PrefabID == "RockCrusher" || buildingDef2.PrefabID == "GasReservoir" || buildingDef2.PrefabID == "ArcadeMachine" || buildingDef2.PrefabID == "MicrobeMusher" || buildingDef2.PrefabID == "FlushToilet" || buildingDef2.PrefabID == "WashSink" || buildingDef2.PrefabID == "Headquarters" || buildingDef2.PrefabID == "GourmetCookingStation")
			{
				return buildingOnFloorBigVis;
			}
			if (!buildingDef2.BuildingComplete.GetComponent<RocketModule>().IsNullOrDestroyed() || !buildingDef2.BuildingComplete.GetComponent<RocketEngine>().IsNullOrDestroyed())
			{
				return buildingRocketVis;
			}
			if (buildingDef2.PrefabID == "PlanterBox" || buildingDef2.PrefabID == "FlowerVase")
			{
				return buildingOnFloorBotanicalVis;
			}
			if (buildingDef2.PrefabID == "ExteriorWall")
			{
				return wallpaperVis;
			}
			if (buildingDef2.PrefabID == "FlowerVaseHanging" || buildingDef2.PrefabID == "FlowerVaseHangingFancy")
			{
				return buildingHangingHookBotanicalVis;
			}
			switch (buildLocationRule)
			{
			case BuildLocationRule.OnCeiling:
				return buildingOnCeilingVis.WithAlignment(Alignment.Top());
			case BuildLocationRule.InCorner:
				return buildingInCeilingCornerVis.WithAlignment(Alignment.TopLeft());
			case BuildLocationRule.OnWall:
				return buildingOnWallVis.WithAlignment(Alignment.Left());
			case BuildLocationRule.OnFloor:
			case BuildLocationRule.OnFoundationRotatable:
				return buildingOnFloorVis;
			default:
				return fallbackVis.WithError($"No visualization available for building with BuildLocationRule of {buildLocationRule}");
			}
		}
		if (permit.Category == PermitCategory.Artwork)
		{
			BuildingDef buildingDef3 = KleiPermitVisUtil.GetBuildingDef(permit);
			if (buildingDef3.IsNullOrDestroyed())
			{
				return fallbackVis.WithError("Couldn't find building def for Artable " + permit.Id);
			}
			_ = (ArtableStage)permit;
			if (Has<Sculpture>(buildingDef3))
			{
				if (buildingDef3.PrefabID == "WoodSculpture")
				{
					return artablePaintingVis;
				}
				return artableSculptureVis;
			}
			if (Has<Painting>(buildingDef3))
			{
				return artablePaintingVis;
			}
			return fallbackVis.WithError("No visualization available for Artable " + permit.Id);
		}
		if (permit.Category == PermitCategory.JoyResponse)
		{
			if (permit is BalloonArtistFacadeResource)
			{
				return joyResponseBalloonVis;
			}
			return fallbackVis.WithError("No visualization available for JoyResponse " + permit.Id);
		}
		return fallbackVis.WithError("No visualization has been defined for permit with id \"" + permit.Id + "\"");
		static bool Has<T>(BuildingDef buildingDef) where T : Component
		{
			return !buildingDef.BuildingComplete.GetComponent<T>().IsNullOrDestroyed();
		}
	}

	public static Sprite GetDioramaBackground(PermitCategory permitCategory)
	{
		switch (permitCategory)
		{
		case PermitCategory.AtmoSuitHelmet:
		case PermitCategory.AtmoSuitBody:
		case PermitCategory.AtmoSuitGloves:
		case PermitCategory.AtmoSuitBelt:
		case PermitCategory.AtmoSuitShoes:
			return Assets.GetSprite("screen_bg_atmosuit");
		case PermitCategory.DupeTops:
		case PermitCategory.DupeBottoms:
		case PermitCategory.DupeGloves:
		case PermitCategory.DupeShoes:
		case PermitCategory.DupeHats:
		case PermitCategory.DupeAccessories:
			return Assets.GetSprite("screen_bg_clothing");
		case PermitCategory.Building:
			return Assets.GetSprite("screen_bg_buildings");
		case PermitCategory.JoyResponse:
			return Assets.GetSprite("screen_bg_joyresponse");
		case PermitCategory.Artwork:
			return Assets.GetSprite("screen_bg_art");
		default:
			return null;
		}
	}

	public static Sprite GetDioramaBackground(ClothingOutfitUtility.OutfitType outfitType)
	{
		return outfitType switch
		{
			ClothingOutfitUtility.OutfitType.Clothing => Assets.GetSprite("screen_bg_clothing"), 
			ClothingOutfitUtility.OutfitType.AtmoSuit => Assets.GetSprite("screen_bg_atmosuit"), 
			ClothingOutfitUtility.OutfitType.JoyResponse => Assets.GetSprite("screen_bg_joyresponse"), 
			_ => null, 
		};
	}
}
