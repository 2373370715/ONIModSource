using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D55 RID: 7509
public class KleiPermitDioramaVis : KMonoBehaviour
{
	// Token: 0x06009CDB RID: 40155 RVA: 0x00106146 File Offset: 0x00104346
	protected override void OnPrefabInit()
	{
		this.Init();
	}

	// Token: 0x06009CDC RID: 40156 RVA: 0x003C5B08 File Offset: 0x003C3D08
	private void Init()
	{
		if (this.initComplete)
		{
			return;
		}
		this.allVisList = ReflectionUtil.For<KleiPermitDioramaVis>(this).CollectValuesForFieldsThatInheritOrImplement<IKleiPermitDioramaVisTarget>(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		foreach (IKleiPermitDioramaVisTarget kleiPermitDioramaVisTarget in this.allVisList)
		{
			kleiPermitDioramaVisTarget.ConfigureSetup();
		}
		this.initComplete = true;
	}

	// Token: 0x06009CDD RID: 40157 RVA: 0x003C5B78 File Offset: 0x003C3D78
	public void ConfigureWith(PermitResource permit)
	{
		if (!this.initComplete)
		{
			this.Init();
		}
		foreach (IKleiPermitDioramaVisTarget kleiPermitDioramaVisTarget in this.allVisList)
		{
			kleiPermitDioramaVisTarget.GetGameObject().SetActive(false);
		}
		KleiPermitVisUtil.ClearAnimation();
		IKleiPermitDioramaVisTarget permitVisTarget = this.GetPermitVisTarget(permit);
		permitVisTarget.GetGameObject().SetActive(true);
		permitVisTarget.ConfigureWith(permit);
		string dlcIdFrom = permit.GetDlcIdFrom();
		if (DlcManager.IsDlcId(dlcIdFrom))
		{
			this.dlcImage.gameObject.SetActive(true);
			this.dlcImage.sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(dlcIdFrom));
			return;
		}
		this.dlcImage.gameObject.SetActive(false);
	}

	// Token: 0x06009CDE RID: 40158 RVA: 0x003C5C44 File Offset: 0x003C3E44
	private IKleiPermitDioramaVisTarget GetPermitVisTarget(PermitResource permit)
	{
		KleiPermitDioramaVis.lastRenderedPermit = permit;
		if (permit == null)
		{
			return this.fallbackVis.WithError(string.Format("Given invalid permit: {0}", permit));
		}
		if (permit.Category == PermitCategory.Equipment || permit.Category == PermitCategory.DupeTops || permit.Category == PermitCategory.DupeBottoms || permit.Category == PermitCategory.DupeGloves || permit.Category == PermitCategory.DupeShoes || permit.Category == PermitCategory.DupeHats || permit.Category == PermitCategory.DupeAccessories || permit.Category == PermitCategory.AtmoSuitHelmet || permit.Category == PermitCategory.AtmoSuitBody || permit.Category == PermitCategory.AtmoSuitGloves || permit.Category == PermitCategory.AtmoSuitBelt || permit.Category == PermitCategory.AtmoSuitShoes)
		{
			return this.equipmentVis;
		}
		if (permit.Category == PermitCategory.Building)
		{
			BuildLocationRule? buildLocationRule = KleiPermitVisUtil.GetBuildLocationRule(permit);
			if (buildLocationRule == null)
			{
				if (permit.DlcIds.SequenceEqual(DlcManager.AVAILABLE_EXPANSION1_ONLY))
				{
					return this.buildingOnFloorVis;
				}
				return this.fallbackVis.WithError("Couldn't get BuildLocationRule on permit with id \"" + permit.Id + "\"");
			}
			else
			{
				BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
				if (!buildingDef.BuildingComplete.GetComponent<Bed>().IsNullOrDestroyed())
				{
					return this.buildingOnFloorVis;
				}
				if (buildingDef.PrefabID == "RockCrusher" || buildingDef.PrefabID == "GasReservoir" || buildingDef.PrefabID == "ArcadeMachine" || buildingDef.PrefabID == "MicrobeMusher" || buildingDef.PrefabID == "FlushToilet" || buildingDef.PrefabID == "WashSink" || buildingDef.PrefabID == "Headquarters" || buildingDef.PrefabID == "GourmetCookingStation")
				{
					return this.buildingOnFloorBigVis;
				}
				if (!buildingDef.BuildingComplete.GetComponent<RocketModule>().IsNullOrDestroyed() || !buildingDef.BuildingComplete.GetComponent<RocketEngine>().IsNullOrDestroyed())
				{
					return this.buildingRocketVis;
				}
				if (buildingDef.PrefabID == "PlanterBox" || buildingDef.PrefabID == "FlowerVase")
				{
					return this.buildingOnFloorBotanicalVis;
				}
				if (buildingDef.PrefabID == "ExteriorWall")
				{
					return this.wallpaperVis;
				}
				if (buildingDef.PrefabID == "FlowerVaseHanging" || buildingDef.PrefabID == "FlowerVaseHangingFancy")
				{
					return this.buildingHangingHookBotanicalVis;
				}
				if (buildLocationRule != null)
				{
					BuildLocationRule valueOrDefault = buildLocationRule.GetValueOrDefault();
					switch (valueOrDefault)
					{
					case BuildLocationRule.OnFloor:
						break;
					case BuildLocationRule.OnFloorOverSpace:
						goto IL_2AE;
					case BuildLocationRule.OnCeiling:
						return this.buildingOnCeilingVis.WithAlignment(Alignment.Top());
					case BuildLocationRule.OnWall:
						return this.buildingOnWallVis.WithAlignment(Alignment.Left());
					case BuildLocationRule.InCorner:
						return this.buildingInCeilingCornerVis.WithAlignment(Alignment.TopLeft());
					default:
						if (valueOrDefault != BuildLocationRule.OnFoundationRotatable)
						{
							goto IL_2AE;
						}
						break;
					}
					return this.buildingOnFloorVis;
				}
				IL_2AE:
				return this.fallbackVis.WithError(string.Format("No visualization available for building with BuildLocationRule of {0}", buildLocationRule));
			}
		}
		else if (permit.Category == PermitCategory.Artwork)
		{
			BuildingDef buildingDef2 = KleiPermitVisUtil.GetBuildingDef(permit);
			if (buildingDef2.IsNullOrDestroyed())
			{
				return this.fallbackVis.WithError("Couldn't find building def for Artable " + permit.Id);
			}
			ArtableStage artableStage = (ArtableStage)permit;
			if (KleiPermitDioramaVis.<GetPermitVisTarget>g__Has|21_0<Sculpture>(buildingDef2))
			{
				if (buildingDef2.PrefabID == "WoodSculpture")
				{
					return this.artablePaintingVis;
				}
				return this.artableSculptureVis;
			}
			else
			{
				if (KleiPermitDioramaVis.<GetPermitVisTarget>g__Has|21_0<Painting>(buildingDef2))
				{
					return this.artablePaintingVis;
				}
				return this.fallbackVis.WithError("No visualization available for Artable " + permit.Id);
			}
		}
		else
		{
			if (permit.Category != PermitCategory.JoyResponse)
			{
				return this.fallbackVis.WithError("No visualization has been defined for permit with id \"" + permit.Id + "\"");
			}
			if (permit is BalloonArtistFacadeResource)
			{
				return this.joyResponseBalloonVis;
			}
			return this.fallbackVis.WithError("No visualization available for JoyResponse " + permit.Id);
		}
	}

	// Token: 0x06009CDF RID: 40159 RVA: 0x003C6004 File Offset: 0x003C4204
	public static Sprite GetDioramaBackground(PermitCategory permitCategory)
	{
		switch (permitCategory)
		{
		case PermitCategory.DupeTops:
		case PermitCategory.DupeBottoms:
		case PermitCategory.DupeGloves:
		case PermitCategory.DupeShoes:
		case PermitCategory.DupeHats:
		case PermitCategory.DupeAccessories:
			return Assets.GetSprite("screen_bg_clothing");
		case PermitCategory.AtmoSuitHelmet:
		case PermitCategory.AtmoSuitBody:
		case PermitCategory.AtmoSuitGloves:
		case PermitCategory.AtmoSuitBelt:
		case PermitCategory.AtmoSuitShoes:
			return Assets.GetSprite("screen_bg_atmosuit");
		case PermitCategory.Building:
			return Assets.GetSprite("screen_bg_buildings");
		case PermitCategory.Artwork:
			return Assets.GetSprite("screen_bg_art");
		case PermitCategory.JoyResponse:
			return Assets.GetSprite("screen_bg_joyresponse");
		}
		return null;
	}

	// Token: 0x06009CE0 RID: 40160 RVA: 0x003C60B0 File Offset: 0x003C42B0
	public static Sprite GetDioramaBackground(ClothingOutfitUtility.OutfitType outfitType)
	{
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			return Assets.GetSprite("screen_bg_clothing");
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			return Assets.GetSprite("screen_bg_joyresponse");
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			return Assets.GetSprite("screen_bg_atmosuit");
		default:
			return null;
		}
	}

	// Token: 0x06009CE2 RID: 40162 RVA: 0x00105B3C File Offset: 0x00103D3C
	[CompilerGenerated]
	internal static bool <GetPermitVisTarget>g__Has|21_0<T>(BuildingDef buildingDef) where T : Component
	{
		return !buildingDef.BuildingComplete.GetComponent<T>().IsNullOrDestroyed();
	}

	// Token: 0x04007AE9 RID: 31465
	[SerializeField]
	private Image dlcImage;

	// Token: 0x04007AEA RID: 31466
	[SerializeField]
	private KleiPermitDioramaVis_Fallback fallbackVis;

	// Token: 0x04007AEB RID: 31467
	[SerializeField]
	private KleiPermitDioramaVis_DupeEquipment equipmentVis;

	// Token: 0x04007AEC RID: 31468
	[SerializeField]
	private KleiPermitDioramaVis_BuildingOnFloor buildingOnFloorVis;

	// Token: 0x04007AED RID: 31469
	[SerializeField]
	private KleiPermitDioramaVis_BuildingOnFloorBig buildingOnFloorBigVis;

	// Token: 0x04007AEE RID: 31470
	[SerializeField]
	private KleiPermitDioramaVis_BuildingPresentationStand buildingOnWallVis;

	// Token: 0x04007AEF RID: 31471
	[SerializeField]
	private KleiPermitDioramaVis_BuildingPresentationStand buildingOnCeilingVis;

	// Token: 0x04007AF0 RID: 31472
	[SerializeField]
	private KleiPermitDioramaVis_BuildingPresentationStand buildingInCeilingCornerVis;

	// Token: 0x04007AF1 RID: 31473
	[SerializeField]
	private KleiPermitDioramaVis_BuildingRocket buildingRocketVis;

	// Token: 0x04007AF2 RID: 31474
	[SerializeField]
	private KleiPermitDioramaVis_BuildingOnFloor buildingOnFloorBotanicalVis;

	// Token: 0x04007AF3 RID: 31475
	[SerializeField]
	private KleiPermitDioramaVis_BuildingHangingHook buildingHangingHookBotanicalVis;

	// Token: 0x04007AF4 RID: 31476
	[SerializeField]
	private KleiPermitDioramaVis_Wallpaper wallpaperVis;

	// Token: 0x04007AF5 RID: 31477
	[SerializeField]
	private KleiPermitDioramaVis_ArtablePainting artablePaintingVis;

	// Token: 0x04007AF6 RID: 31478
	[SerializeField]
	private KleiPermitDioramaVis_ArtableSculpture artableSculptureVis;

	// Token: 0x04007AF7 RID: 31479
	[SerializeField]
	private KleiPermitDioramaVis_JoyResponseBalloon joyResponseBalloonVis;

	// Token: 0x04007AF8 RID: 31480
	private bool initComplete;

	// Token: 0x04007AF9 RID: 31481
	private IReadOnlyList<IKleiPermitDioramaVisTarget> allVisList;

	// Token: 0x04007AFA RID: 31482
	public static PermitResource lastRenderedPermit;
}
