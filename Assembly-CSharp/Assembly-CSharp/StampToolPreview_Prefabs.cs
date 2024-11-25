using System;
using TemplateClasses;
using UnityEngine;
using Object = UnityEngine.Object;

public class StampToolPreview_Prefabs : IStampToolPreviewPlugin {
    public void Setup(StampToolPreviewContext context) {
        if (!context.stampTemplate.elementalOres.IsNullOrDestroyed())
            foreach (var prefabInfo in context.stampTemplate.elementalOres)
                SpawnPrefab(context, prefabInfo);

        if (!context.stampTemplate.otherEntities.IsNullOrDestroyed())
            foreach (var prefabInfo2 in context.stampTemplate.otherEntities)
                SpawnPrefab(context, prefabInfo2);

        if (!context.stampTemplate.buildings.IsNullOrDestroyed())
            foreach (var prefabInfo3 in context.stampTemplate.buildings)
                SpawnPrefab(context, prefabInfo3);

        if (!context.stampTemplate.elementalOres.IsNullOrDestroyed())
            foreach (var prefabInfo4 in context.stampTemplate.elementalOres)
                SpawnPrefab(context, prefabInfo4);
    }

    public static void SpawnPrefab(StampToolPreviewContext context, Prefab prefabInfo) {
        var gameObject = Assets.TryGetPrefab(prefabInfo.id);
        if (gameObject.IsNullOrDestroyed()) return;

        if (gameObject.GetComponent<Building>().IsNullOrDestroyed()) {
            SpawnPrefab_Default(context, prefabInfo, gameObject);
            return;
        }

        var component = gameObject.GetComponent<Building>();
        if (component.Def.IsTilePiece) {
            SpawnPrefab_Tile(context, prefabInfo, component);
            return;
        }

        SpawnPrefab_Building(context, prefabInfo, component);
    }

    public static void SpawnPrefab_Tile(StampToolPreviewContext context, Prefab prefabInfo, Building buildingPrefab) {
        var textureAtlas                       = buildingPrefab.Def.BlockTilePlaceAtlas;
        if (textureAtlas == null) textureAtlas = buildingPrefab.Def.BlockTileAtlas;
        if (textureAtlas == null || textureAtlas.items == null || textureAtlas.items.Length < 0) return;

        GameObject   gameObject;
        MeshRenderer meshRenderer;
        StampToolPreviewUtil.MakeQuad(out gameObject, out meshRenderer, 1.5f, textureAtlas.items[0].uvBox);
        gameObject.name = string.Format("TilePlacer {0}", buildingPrefab.PrefabID());
        gameObject.transform.SetParent(context.previewParent.transform, false);
        gameObject.transform.SetLocalPosition(new Vector2(prefabInfo.location_x,
                                                          prefabInfo.location_y + Grid.HalfCellSizeInMeters));

        var material = StampToolPreviewUtil.MakeMaterial(textureAtlas.texture);
        material.name         = string.Format("Tile ({0}) ({1})", buildingPrefab.PrefabID(), material.name);
        meshRenderer.material = material;
        context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn,
                                                            new System.Action(delegate {
                                                                                  if (!gameObject.IsNullOrDestroyed())
                                                                                      Object.Destroy(gameObject);

                                                                                  if (!material.IsNullOrDestroyed())
                                                                                      Object.Destroy(material);
                                                                              }));

        context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn,
                                                                   new Action<string>(delegate(string error) {
                                                                       if (meshRenderer.IsNullOrDestroyed())
                                                                           return;

                                                                       meshRenderer.material.color
                                                                           = error != null
                                                                                 ? StampToolPreviewUtil
                                                                                     .COLOR_ERROR
                                                                                 : StampToolPreviewUtil
                                                                                     .COLOR_OK;
                                                                   }));
    }

    public static void SpawnPrefab_Building(StampToolPreviewContext context,
                                            Prefab                  prefabInfo,
                                            Building                buildingPrefab) {
        var        num = LayerMask.NameToLayer("Place");
        GameObject original;
        if (buildingPrefab.Def.BuildingPreview.IsNullOrDestroyed())
            original = BuildingLoader.Instance.CreateBuildingPreview(buildingPrefab.Def);
        else
            original = buildingPrefab.Def.BuildingPreview;

        var spawn = GameUtil.KInstantiate(original, Vector3.zero, Grid.SceneLayer.Building, null, num)
                            .GetComponent<Building>();

        context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn,
                                                            new System.Action(delegate {
                                                                                  if (spawn.IsNullOrDestroyed()) return;

                                                                                  Object.Destroy(spawn.gameObject);
                                                                              }));

        var component = spawn.GetComponent<Rotatable>();
        if (component != null) component.SetOrientation(prefabInfo.rotationOrientation);
        var kanim = spawn.GetComponent<KBatchedAnimController>();
        if (kanim != null) {
            kanim.visibilityType = KAnimControllerBase.VisibilityType.Always;
            kanim.isMovable      = true;
            kanim.Offset         = buildingPrefab.Def.GetVisualizerOffset();
            kanim.name           = kanim.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
            kanim.TintColour     = StampToolPreviewUtil.COLOR_OK;
            kanim.SetLayer(num);
        }

        spawn.transform.SetParent(context.previewParent.transform, false);
        spawn.transform.SetLocalPosition(new Vector2(prefabInfo.location_x, prefabInfo.location_y));
        context.frameAfterSetupFn = (System.Action)Delegate.Combine(context.frameAfterSetupFn,
                                                                    new System.Action(delegate {
                                                                        if (spawn.IsNullOrDestroyed()) return;

                                                                        spawn.gameObject.SetActive(false);
                                                                        spawn.gameObject.SetActive(true);
                                                                        if (kanim.IsNullOrDestroyed()) return;

                                                                        var text = "";
                                                                        if ((prefabInfo.connections & 1) != 0)
                                                                            text += "L";

                                                                        if ((prefabInfo.connections & 2) != 0)
                                                                            text += "R";

                                                                        if ((prefabInfo.connections & 4) != 0)
                                                                            text += "U";

                                                                        if ((prefabInfo.connections & 8) != 0)
                                                                            text += "D";

                                                                        if (text == "") text = "None";
                                                                        if (kanim != null &&
                                                                            kanim.HasAnimation(text)) {
                                                                            var text2 = text + "_place";
                                                                            var flag
                                                                                = kanim.HasAnimation(text2);

                                                                            kanim.Play(flag ? text2 : text,
                                                                             KAnim.PlayMode.Loop);
                                                                        }
                                                                    }));

        context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn,
                                                                   new Action<string>(delegate(string error) {
                                                                       if (kanim.IsNullOrDestroyed()) return;

                                                                       var c = error != null
                                                                                   ? StampToolPreviewUtil
                                                                                       .COLOR_ERROR
                                                                                   : StampToolPreviewUtil.COLOR_OK;

                                                                       if (buildingPrefab.Def.SceneLayer ==
                                                                           Grid.SceneLayer.Backwall)
                                                                           c.a = 0.2f;

                                                                       kanim.TintColour = c;
                                                                   }));

        var component2 = spawn.GetComponent<BuildingFacade>();
        if (component2 != null && !prefabInfo.facadeId.IsNullOrWhiteSpace()) {
            var buildingFacadeResource = Db.GetBuildingFacades().TryGet(prefabInfo.facadeId);
            if (buildingFacadeResource != null && buildingFacadeResource.IsUnlocked())
                component2.ApplyBuildingFacade(buildingFacadeResource);
        }
    }

    public static void SpawnPrefab_Default(StampToolPreviewContext context, Prefab prefabInfo, GameObject prefab) {
        var component = prefab.GetComponent<KBatchedAnimController>();
        if (component == null) return;

        var name  = prefab.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
        var layer = LayerMask.NameToLayer("Place");
        var spawn = new GameObject(name);
        spawn.SetActive(false);
        var kanim = spawn.AddComponent<KBatchedAnimController>();
        if (!component.IsNullOrDestroyed()) {
            kanim.AnimFiles      = component.AnimFiles;
            kanim.visibilityType = KAnimControllerBase.VisibilityType.Always;
            kanim.isMovable      = true;
            kanim.name           = name;
            kanim.TintColour     = StampToolPreviewUtil.COLOR_OK;
            kanim.SetLayer(layer);
        }

        spawn.transform.SetParent(context.previewParent.transform, false);
        var component2 = prefab.GetComponent<OccupyArea>();
        int num;
        if (component2.IsNullOrDestroyed() || component2._UnrotatedOccupiedCellsOffsets.Length == 0)
            num = 0;
        else {
            var num2 = int.MaxValue;
            var num3 = int.MinValue;
            foreach (var cellOffset in component2._UnrotatedOccupiedCellsOffsets) {
                if (cellOffset.x < num2) num2 = cellOffset.x;
                if (cellOffset.x > num3) num3 = cellOffset.x;
            }

            num = num3 - num2 + 1;
        }

        if (num != 0 && num % 2 == 0)
            spawn.transform.SetLocalPosition(new Vector2(prefabInfo.location_x + Grid.HalfCellSizeInMeters,
                                                         prefabInfo.location_y));
        else
            spawn.transform.SetLocalPosition(new Vector2(prefabInfo.location_x, prefabInfo.location_y));

        context.frameAfterSetupFn = (System.Action)Delegate.Combine(context.frameAfterSetupFn,
                                                                    new System.Action(delegate {
                                                                        if (spawn.IsNullOrDestroyed()) return;

                                                                        spawn.gameObject.SetActive(false);
                                                                        spawn.gameObject.SetActive(true);
                                                                        if (kanim.IsNullOrDestroyed()) return;

                                                                        kanim.Play("place",
                                                                         KAnim.PlayMode.Loop);
                                                                    }));

        context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn,
                                                            new System.Action(delegate {
                                                                                  if (spawn.IsNullOrDestroyed()) return;

                                                                                  Object.Destroy(spawn.gameObject);
                                                                              }));

        context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn,
                                                                   new Action<string>(delegate(string error) {
                                                                       if (kanim.IsNullOrDestroyed()) return;

                                                                       kanim.TintColour
                                                                           = error != null
                                                                                 ? StampToolPreviewUtil
                                                                                     .COLOR_ERROR
                                                                                 : StampToolPreviewUtil
                                                                                     .COLOR_OK;
                                                                   }));
    }
}