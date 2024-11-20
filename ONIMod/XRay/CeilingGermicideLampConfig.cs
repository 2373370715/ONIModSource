using TUNING;
using UnityEngine;

namespace Ray {
    public class CeilingGermicideLampConfig : IBuildingConfig {
        public override BuildingDef CreateBuildingDef() {
            var buildingDef = BuildingTemplates.CreateBuildingDef("XRAY",
                                                                  2,
                                                                  1,
                                                                  "uvlamp_small_kanim",
                                                                  10,
                                                                  10f,
                                                                  new[] { BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0] },
                                                                  new[] { "Metal" },
                                                                  1600f,
                                                                  BuildLocationRule.OnCeiling,
                                                                  DECOR.PENALTY.TIER1,
                                                                  NOISE_POLLUTION.NONE);

            buildingDef.RequiresPowerInput          = false;
            buildingDef.SelfHeatKilowattsWhenActive = 0f;
            buildingDef.AudioCategory               = "Metal";
            buildingDef.Floodable                   = false;
            buildingDef.Overheatable                = false;
            buildingDef.ViewMode                    = OverlayModes.Disease.ID;

            buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
            GeneratedBuildings.RegisterWithOverlay(OverlayScreen.DiseaseIDs, "XRAY");
            return buildingDef;
        }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go) {
            AddVisualizer(go, true);
            var lightShapePreview = go.AddComponent<LightShapePreview>();
            lightShapePreview.lux    = 600;
            lightShapePreview.radius = 8f;
            lightShapePreview.shape  = LightShape.Cone;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag) {
            go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource);
        }

        public override void DoPostConfigureUnderConstruction(GameObject go) { AddVisualizer(go, false); }

        /// <summary>
        /// 在游戏对象配置完成后执行额外的配置逻辑。
        /// </summary>
        /// <param name="go">要进行配置的游戏对象。</param>
        public override void DoPostConfigureComplete(GameObject go) {
            // 定义AOE（区域效应）的左、宽、底、高参数
            int left, wideth, bottom, height;

            // 使用扩展工具计算天花板UV范围的AOE参数
            ExtentsHelpers.CeilingUVExtents(8, 8, out left, out wideth, out bottom, out height);

            // 确保游戏对象包含循环声音组件
            go.AddOrGet<LoopingSounds>();

            // 获取或添加杀菌灯组件
            var XRay = go.AddOrGet<XRay>();

            // 设置杀菌灯的AOE参数
            XRay.left   = left;
            XRay.width  = wideth;
            XRay.bottom = bottom;
            XRay.heigh = height;

            // 添加杀菌灯的可视化效果
            AddVisualizer(go, false);

            // 获取或添加2D光源组件
            var light2D = go.AddOrGet<Light2D>();

            // 设置光源的覆盖颜色、主颜色、范围、角度、方向、偏移、形状、是否绘制覆盖效果和亮度
            light2D.overlayColour = LIGHT2D.CEILINGLIGHT_OVERLAYCOLOR;
            light2D.Color         = new Color(0f, 2f, 2f);
            light2D.Range         = 8f;
            light2D.Angle         = 2.6f;
            light2D.Direction     = LIGHT2D.CEILINGLIGHT_DIRECTION;
            light2D.Offset        = new Vector2(0.55f, 0.65f);
            light2D.shape         = LightShape.Cone;
            light2D.drawOverlay   = true;
            light2D.Lux           = 600;

            // 确保游戏对象包含光源控制器
            go.AddOrGetDef<LightController.Def>();
        }

        private static void AddVisualizer(GameObject prefab, bool movable) {
            int x, y, width, height;
            ExtentsHelpers.CeilingUVExtents(8, 8, out x, out width, out y, out height);

            var stationaryChoreRangeVisualizer = prefab.AddOrGet<StationaryChoreRangeVisualizer>();
            stationaryChoreRangeVisualizer.x                     = x;
            stationaryChoreRangeVisualizer.y                     = y;
            stationaryChoreRangeVisualizer.width                 = width;
            stationaryChoreRangeVisualizer.height                = height;
            stationaryChoreRangeVisualizer.movable               = movable;
            stationaryChoreRangeVisualizer.blocking_tile_visible = true;
            prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go) {
                                                                  go.GetComponent<StationaryChoreRangeVisualizer>()
                                                                    .blocking_cb = cell => Grid.VisibleBlockingCB(cell);
                                                              };
        }
    }
}