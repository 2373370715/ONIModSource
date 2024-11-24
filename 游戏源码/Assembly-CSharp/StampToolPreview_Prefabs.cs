using System;
using Database;
using TemplateClasses;
using UnityEngine;

public class StampToolPreview_Prefabs : IStampToolPreviewPlugin
{
	public void Setup(StampToolPreviewContext context)
	{
		if (!context.stampTemplate.elementalOres.IsNullOrDestroyed())
		{
			foreach (Prefab elementalOre in context.stampTemplate.elementalOres)
			{
				SpawnPrefab(context, elementalOre);
			}
		}
		if (!context.stampTemplate.otherEntities.IsNullOrDestroyed())
		{
			foreach (Prefab otherEntity in context.stampTemplate.otherEntities)
			{
				SpawnPrefab(context, otherEntity);
			}
		}
		if (!context.stampTemplate.buildings.IsNullOrDestroyed())
		{
			foreach (Prefab building in context.stampTemplate.buildings)
			{
				SpawnPrefab(context, building);
			}
		}
		if (context.stampTemplate.elementalOres.IsNullOrDestroyed())
		{
			return;
		}
		foreach (Prefab elementalOre2 in context.stampTemplate.elementalOres)
		{
			SpawnPrefab(context, elementalOre2);
		}
	}

	public static void SpawnPrefab(StampToolPreviewContext context, Prefab prefabInfo)
	{
		GameObject gameObject = Assets.TryGetPrefab(prefabInfo.id);
		if (gameObject.IsNullOrDestroyed())
		{
			return;
		}
		if (!gameObject.GetComponent<Building>().IsNullOrDestroyed())
		{
			Building component = gameObject.GetComponent<Building>();
			if (component.Def.IsTilePiece)
			{
				SpawnPrefab_Tile(context, prefabInfo, component);
			}
			else
			{
				SpawnPrefab_Building(context, prefabInfo, component);
			}
		}
		else
		{
			SpawnPrefab_Default(context, prefabInfo, gameObject);
		}
	}

	public static void SpawnPrefab_Tile(StampToolPreviewContext context, Prefab prefabInfo, Building buildingPrefab)
	{
		TextureAtlas textureAtlas = buildingPrefab.Def.BlockTilePlaceAtlas;
		if (textureAtlas == null)
		{
			textureAtlas = buildingPrefab.Def.BlockTileAtlas;
		}
		if (textureAtlas == null || textureAtlas.items == null || textureAtlas.items.Length < 0)
		{
			return;
		}
		StampToolPreviewUtil.MakeQuad(out var gameObject, out var meshRenderer, 1.5f, textureAtlas.items[0].uvBox);
		gameObject.name = $"TilePlacer {buildingPrefab.PrefabID()}";
		gameObject.transform.SetParent(context.previewParent.transform, worldPositionStays: false);
		gameObject.transform.SetLocalPosition(new Vector2(prefabInfo.location_x, (float)prefabInfo.location_y + Grid.HalfCellSizeInMeters));
		Material material = StampToolPreviewUtil.MakeMaterial(textureAtlas.texture);
		material.name = $"Tile ({buildingPrefab.PrefabID()}) ({material.name})";
		meshRenderer.material = material;
		context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn, (System.Action)delegate
		{
			if (!gameObject.IsNullOrDestroyed())
			{
				UnityEngine.Object.Destroy(gameObject);
			}
			if (!material.IsNullOrDestroyed())
			{
				UnityEngine.Object.Destroy(material);
			}
		});
		context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn, (Action<string>)delegate(string error)
		{
			if (!meshRenderer.IsNullOrDestroyed())
			{
				meshRenderer.material.color = ((error != null) ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK);
			}
		});
	}

	public static void SpawnPrefab_Building(StampToolPreviewContext context, Prefab prefabInfo, Building buildingPrefab)
	{
		int num = LayerMask.NameToLayer("Place");
		GameObject original = ((!buildingPrefab.Def.BuildingPreview.IsNullOrDestroyed()) ? buildingPrefab.Def.BuildingPreview : BuildingLoader.Instance.CreateBuildingPreview(buildingPrefab.Def));
		Building spawn = GameUtil.KInstantiate(original, Vector3.zero, Grid.SceneLayer.Building, null, num).GetComponent<Building>();
		context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn, (System.Action)delegate
		{
			if (!spawn.IsNullOrDestroyed())
			{
				UnityEngine.Object.Destroy(spawn.gameObject);
			}
		});
		Rotatable component = spawn.GetComponent<Rotatable>();
		if (component != null)
		{
			component.SetOrientation(prefabInfo.rotationOrientation);
		}
		KBatchedAnimController kanim = spawn.GetComponent<KBatchedAnimController>();
		if (kanim != null)
		{
			kanim.visibilityType = KAnimControllerBase.VisibilityType.Always;
			kanim.isMovable = true;
			kanim.Offset = buildingPrefab.Def.GetVisualizerOffset();
			kanim.name = kanim.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
			kanim.TintColour = StampToolPreviewUtil.COLOR_OK;
			kanim.SetLayer(num);
		}
		spawn.transform.SetParent(context.previewParent.transform, worldPositionStays: false);
		spawn.transform.SetLocalPosition(new Vector2(prefabInfo.location_x, prefabInfo.location_y));
		context.frameAfterSetupFn = (System.Action)Delegate.Combine(context.frameAfterSetupFn, (System.Action)delegate
		{
			if (!spawn.IsNullOrDestroyed())
			{
				spawn.gameObject.SetActive(value: false);
				spawn.gameObject.SetActive(value: true);
				if (!kanim.IsNullOrDestroyed())
				{
					string text = "";
					if (((uint)prefabInfo.connections & (true ? 1u : 0u)) != 0)
					{
						text += "L";
					}
					if (((uint)prefabInfo.connections & 2u) != 0)
					{
						text += "R";
					}
					if (((uint)prefabInfo.connections & 4u) != 0)
					{
						text += "U";
					}
					if (((uint)prefabInfo.connections & 8u) != 0)
					{
						text += "D";
					}
					if (text == "")
					{
						text = "None";
					}
					if (kanim != null && kanim.HasAnimation(text))
					{
						string text2 = text + "_place";
						bool flag = kanim.HasAnimation(text2);
						kanim.Play(flag ? text2 : text, KAnim.PlayMode.Loop);
					}
				}
			}
		});
		context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn, (Action<string>)delegate(string error)
		{
			if (!kanim.IsNullOrDestroyed())
			{
				Color color = ((error != null) ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK);
				if (buildingPrefab.Def.SceneLayer == Grid.SceneLayer.Backwall)
				{
					color.a = 0.2f;
				}
				kanim.TintColour = color;
			}
		});
		BuildingFacade component2 = spawn.GetComponent<BuildingFacade>();
		if (component2 != null && !prefabInfo.facadeId.IsNullOrWhiteSpace())
		{
			BuildingFacadeResource buildingFacadeResource = Db.GetBuildingFacades().TryGet(prefabInfo.facadeId);
			if (buildingFacadeResource != null && buildingFacadeResource.IsUnlocked())
			{
				component2.ApplyBuildingFacade(buildingFacadeResource);
			}
		}
	}

	public static void SpawnPrefab_Default(StampToolPreviewContext context, Prefab prefabInfo, GameObject prefab)
	{
		KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
		if (component == null)
		{
			return;
		}
		string name = prefab.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
		int layer = LayerMask.NameToLayer("Place");
		GameObject spawn = new GameObject(name);
		spawn.SetActive(value: false);
		KBatchedAnimController kanim = spawn.AddComponent<KBatchedAnimController>();
		if (!component.IsNullOrDestroyed())
		{
			kanim.AnimFiles = component.AnimFiles;
			kanim.visibilityType = KAnimControllerBase.VisibilityType.Always;
			kanim.isMovable = true;
			kanim.name = name;
			kanim.TintColour = StampToolPreviewUtil.COLOR_OK;
			kanim.SetLayer(layer);
		}
		spawn.transform.SetParent(context.previewParent.transform, worldPositionStays: false);
		OccupyArea component2 = prefab.GetComponent<OccupyArea>();
		int num;
		if (component2.IsNullOrDestroyed() || component2._UnrotatedOccupiedCellsOffsets.Length == 0)
		{
			num = 0;
		}
		else
		{
			int num2 = int.MaxValue;
			int num3 = int.MinValue;
			CellOffset[] unrotatedOccupiedCellsOffsets = component2._UnrotatedOccupiedCellsOffsets;
			for (int i = 0; i < unrotatedOccupiedCellsOffsets.Length; i++)
			{
				CellOffset cellOffset = unrotatedOccupiedCellsOffsets[i];
				if (cellOffset.x < num2)
				{
					num2 = cellOffset.x;
				}
				if (cellOffset.x > num3)
				{
					num3 = cellOffset.x;
				}
			}
			num = num3 - num2 + 1;
		}
		if (num != 0 && num % 2 == 0)
		{
			spawn.transform.SetLocalPosition(new Vector2((float)prefabInfo.location_x + Grid.HalfCellSizeInMeters, prefabInfo.location_y));
		}
		else
		{
			spawn.transform.SetLocalPosition(new Vector2(prefabInfo.location_x, prefabInfo.location_y));
		}
		context.frameAfterSetupFn = (System.Action)Delegate.Combine(context.frameAfterSetupFn, (System.Action)delegate
		{
			if (!spawn.IsNullOrDestroyed())
			{
				spawn.gameObject.SetActive(value: false);
				spawn.gameObject.SetActive(value: true);
				if (!kanim.IsNullOrDestroyed())
				{
					kanim.Play("place", KAnim.PlayMode.Loop);
				}
			}
		});
		context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn, (System.Action)delegate
		{
			if (!spawn.IsNullOrDestroyed())
			{
				UnityEngine.Object.Destroy(spawn.gameObject);
			}
		});
		context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn, (Action<string>)delegate(string error)
		{
			if (!kanim.IsNullOrDestroyed())
			{
				kanim.TintColour = ((error != null) ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK);
			}
		});
	}
}
