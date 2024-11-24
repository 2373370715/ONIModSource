using Database;
using UnityEngine;

public static class KleiPermitVisUtil
{
	public const float TILE_SIZE_UI = 176f;

	public static KleiPermitBuildingAnimateIn buildingAnimateIn;

	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, BuildingFacadeResource buildingPermit)
	{
		KAnimFile anim = Assets.GetAnim(buildingPermit.AnimFile);
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[1] { anim });
		buildingKAnim.Play(GetFirstAnimHash(anim), KAnim.PlayMode.Loop);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, BuildingDef buildingDef)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(buildingDef.AnimFiles);
		buildingKAnim.Play(GetFirstAnimHash(buildingDef.AnimFiles[0]), KAnim.PlayMode.Loop);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, ArtableStage artablePermit)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[1] { Assets.GetAnim(artablePermit.animFile) });
		buildingKAnim.Play(artablePermit.anim);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, DbStickerBomb artablePermit)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[1] { artablePermit.animFile });
		HashedString defaultStickerAnimHash = GetDefaultStickerAnimHash(artablePermit.animFile);
		if (defaultStickerAnimHash != null)
		{
			buildingKAnim.Play(defaultStickerAnimHash);
		}
		else
		{
			Debug.Assert(condition: false, "Couldn't find default sticker for sticker " + artablePermit.Id);
			buildingKAnim.Play(GetFirstAnimHash(artablePermit.animFile));
		}
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	public static void ConfigureBuildingPosition(RectTransform transform, PrefabDefinedUIPosition anchorPosition, BuildingDef buildingDef, Alignment alignment)
	{
		anchorPosition.SetOn(transform);
		transform.anchoredPosition += new Vector2(176f * (float)buildingDef.WidthInCells * (0f - (alignment.x - 0.5f)), 176f * (float)buildingDef.HeightInCells * (0f - alignment.y));
	}

	public static void ConfigureBuildingPosition(RectTransform transform, Vector2 anchorPosition, BuildingDef buildingDef, Alignment alignment)
	{
		transform.anchoredPosition = anchorPosition + new Vector2(176f * (float)buildingDef.WidthInCells * (0f - (alignment.x - 0.5f)), 176f * (float)buildingDef.HeightInCells * (0f - alignment.y));
	}

	public static void ClearAnimation()
	{
		if (!buildingAnimateIn.IsNullOrDestroyed())
		{
			Object.Destroy(buildingAnimateIn.gameObject);
		}
	}

	public static void AnimateIn(KBatchedAnimController buildingKAnim, Updater extraUpdater = default(Updater))
	{
		ClearAnimation();
		buildingAnimateIn = KleiPermitBuildingAnimateIn.MakeFor(buildingKAnim, extraUpdater);
	}

	public static HashedString GetFirstAnimHash(KAnimFile animFile)
	{
		return animFile.GetData().GetAnim(0).hash;
	}

	public static HashedString GetDefaultStickerAnimHash(KAnimFile stickerAnimFile)
	{
		KAnimFileData data = stickerAnimFile.GetData();
		for (int i = 0; i < data.animCount; i++)
		{
			KAnim.Anim anim = data.GetAnim(i);
			if (anim.name.StartsWith("idle_sticker"))
			{
				return anim.hash;
			}
		}
		return null;
	}

	public static BuildLocationRule? GetBuildLocationRule(PermitResource permit)
	{
		BuildingDef buildingDef = GetBuildingDef(permit);
		if (buildingDef == null)
		{
			return null;
		}
		return buildingDef.BuildLocationRule;
	}

	public static BuildingDef GetBuildingDef(PermitResource permit)
	{
		if (permit is BuildingFacadeResource buildingFacadeResource)
		{
			GameObject gameObject = Assets.TryGetPrefab(buildingFacadeResource.PrefabID);
			if (gameObject == null)
			{
				return null;
			}
			BuildingComplete component = gameObject.GetComponent<BuildingComplete>();
			if (component == null || !component)
			{
				return null;
			}
			return component.Def;
		}
		if (permit is ArtableStage artableStage)
		{
			BuildingComplete component2 = Assets.GetPrefab(artableStage.prefabId).GetComponent<BuildingComplete>();
			if (component2 == null || !component2)
			{
				return null;
			}
			return component2.Def;
		}
		return null;
	}
}
