using System;
using Database;
using UnityEngine;

// Token: 0x02001D67 RID: 7527
public static class KleiPermitVisUtil
{
	// Token: 0x06009D39 RID: 40249 RVA: 0x003C69E0 File Offset: 0x003C4BE0
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, BuildingFacadeResource buildingPermit)
	{
		KAnimFile anim = Assets.GetAnim(buildingPermit.AnimFile);
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[]
		{
			anim
		});
		buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(anim), KAnim.PlayMode.Loop, 1f, 0f);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x06009D3A RID: 40250 RVA: 0x003C6A48 File Offset: 0x003C4C48
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, BuildingDef buildingDef)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(buildingDef.AnimFiles);
		buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(buildingDef.AnimFiles[0]), KAnim.PlayMode.Loop, 1f, 0f);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x06009D3B RID: 40251 RVA: 0x003C6AA0 File Offset: 0x003C4CA0
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, ArtableStage artablePermit)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[]
		{
			Assets.GetAnim(artablePermit.animFile)
		});
		buildingKAnim.Play(artablePermit.anim, KAnim.PlayMode.Once, 1f, 0f);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x06009D3C RID: 40252 RVA: 0x003C6B08 File Offset: 0x003C4D08
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, DbStickerBomb artablePermit)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[]
		{
			artablePermit.animFile
		});
		HashedString defaultStickerAnimHash = KleiPermitVisUtil.GetDefaultStickerAnimHash(artablePermit.animFile);
		if (defaultStickerAnimHash != null)
		{
			buildingKAnim.Play(defaultStickerAnimHash, KAnim.PlayMode.Once, 1f, 0f);
		}
		else
		{
			global::Debug.Assert(false, "Couldn't find default sticker for sticker " + artablePermit.Id);
			buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(artablePermit.animFile), KAnim.PlayMode.Once, 1f, 0f);
		}
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x06009D3D RID: 40253 RVA: 0x003C6BAC File Offset: 0x003C4DAC
	public static void ConfigureBuildingPosition(RectTransform transform, PrefabDefinedUIPosition anchorPosition, BuildingDef buildingDef, Alignment alignment)
	{
		anchorPosition.SetOn(transform);
		transform.anchoredPosition += new Vector2(176f * (float)buildingDef.WidthInCells * -(alignment.x - 0.5f), 176f * (float)buildingDef.HeightInCells * -alignment.y);
	}

	// Token: 0x06009D3E RID: 40254 RVA: 0x003C6C08 File Offset: 0x003C4E08
	public static void ConfigureBuildingPosition(RectTransform transform, Vector2 anchorPosition, BuildingDef buildingDef, Alignment alignment)
	{
		transform.anchoredPosition = anchorPosition + new Vector2(176f * (float)buildingDef.WidthInCells * -(alignment.x - 0.5f), 176f * (float)buildingDef.HeightInCells * -alignment.y);
	}

	// Token: 0x06009D3F RID: 40255 RVA: 0x00106460 File Offset: 0x00104660
	public static void ClearAnimation()
	{
		if (!KleiPermitVisUtil.buildingAnimateIn.IsNullOrDestroyed())
		{
			UnityEngine.Object.Destroy(KleiPermitVisUtil.buildingAnimateIn.gameObject);
		}
	}

	// Token: 0x06009D40 RID: 40256 RVA: 0x0010647D File Offset: 0x0010467D
	public static void AnimateIn(KBatchedAnimController buildingKAnim, Updater extraUpdater = default(Updater))
	{
		KleiPermitVisUtil.ClearAnimation();
		KleiPermitVisUtil.buildingAnimateIn = KleiPermitBuildingAnimateIn.MakeFor(buildingKAnim, extraUpdater);
	}

	// Token: 0x06009D41 RID: 40257 RVA: 0x00106490 File Offset: 0x00104690
	public static HashedString GetFirstAnimHash(KAnimFile animFile)
	{
		return animFile.GetData().GetAnim(0).hash;
	}

	// Token: 0x06009D42 RID: 40258 RVA: 0x003C6C58 File Offset: 0x003C4E58
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

	// Token: 0x06009D43 RID: 40259 RVA: 0x003C6CA4 File Offset: 0x003C4EA4
	public static BuildLocationRule? GetBuildLocationRule(PermitResource permit)
	{
		BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		if (buildingDef == null)
		{
			return null;
		}
		return new BuildLocationRule?(buildingDef.BuildLocationRule);
	}

	// Token: 0x06009D44 RID: 40260 RVA: 0x003C6CD8 File Offset: 0x003C4ED8
	public static BuildingDef GetBuildingDef(PermitResource permit)
	{
		BuildingFacadeResource buildingFacadeResource = permit as BuildingFacadeResource;
		if (buildingFacadeResource != null)
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
		else
		{
			ArtableStage artableStage = permit as ArtableStage;
			if (artableStage == null)
			{
				return null;
			}
			BuildingComplete component2 = Assets.GetPrefab(artableStage.prefabId).GetComponent<BuildingComplete>();
			if (component2 == null || !component2)
			{
				return null;
			}
			return component2.Def;
		}
	}

	// Token: 0x04007B2F RID: 31535
	public const float TILE_SIZE_UI = 176f;

	// Token: 0x04007B30 RID: 31536
	public static KleiPermitBuildingAnimateIn buildingAnimateIn;
}
