using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

namespace Ray {
    [SerializationConfig(memberSerialization: MemberSerialization.OptIn)]
    public class XRay : KMonoBehaviour, ISim200ms {
        public  bool     On;
        public  int      bottom;
        public  int      heigh = 1;
        public  int      left;
        public  int      width = 1;
        public  bool     isMobile;
        private Vector2I lampXY;

        [MyCmpGet]
        public Light2D light;

        public int lightOffsetX;
        public int lightOffsetY;

        [MyCmpGet]
        public Operational operational;

        /// <summary>
        /// 模拟每200毫秒的灯效更新。
        /// </summary>
        /// <param name="dt">自上次更新以来的时间间隔。</param>
        public void Sim200ms(float dt) {
            // 如果是移动平台，计算灯的位置。
            if (isMobile)
                lampXY = Grid.PosToXY(gameObject.transform.position) + new Vector2I(lightOffsetX, lightOffsetY);

            // 检查灯是否应该一直亮着或者当前是操作性的。
            if (On || (operational != null && operational.IsOperational)) {
                // 如果是操作性的，确保其处于激活状态。
                if (operational != null) operational.SetActive(true);

                // 初始化一个集合，用于记录已经处理过的建筑物，避免重复处理。
                var buildingsAlreadySeen = new HashSet<GameObject>();

                // 遍历影响区域内的所有单元格。
                for (var i = bottom; i < bottom + heigh; i++)
                    for (var j = left; j < left + width; j++)

                        // 更新当前单元格的状态，同时传入已经处理过的建筑物集合。
                        UpdateCell(lampXY.X + j, lampXY.Y + i, buildingsAlreadySeen);
            }

            if (operational != null) operational.SetActive(false);
        }

        /// <summary>
        /// 测试两点之间的视线是否无障碍。
        /// </summary>
        /// <param name="x1">起点的x坐标。</param>
        /// <param name="y1">起点的y坐标。</param>
        /// <param name="x2">终点的x坐标。</param>
        /// <param name="y2">终点的y坐标。</param>
        /// <returns>如果两点之间无障碍，则返回true；否则返回false。</returns>
        public static bool TestLineOfSight(int x1, int y1, int x2, int y2) {
            return Grid.TestLineOfSight(x1, y1, x2, y2, Grid.VisibleBlockingCB, true);
        }

        protected override void OnSpawn() {
            base.OnSpawn();
            lampXY = Grid.PosToXY(gameObject.transform.position) + new Vector2I(lightOffsetX, lightOffsetY);
        }

        /// <summary>
        /// 更新指定单元格的杀菌效果。
        /// </summary>
        /// <param name="x">单元格的X坐标。</param>
        /// <param name="y">单元格的Y坐标。</param>
        /// <param name="buildingsAlreadySeen">已处理的建筑物集合，用于优化处理。</param>
        private void UpdateCell(int x, int y, HashSet<GameObject> buildingsAlreadySeen) {
            // 检查灯塔和当前单元格之间是否有视线通视
            if (!TestLineOfSight(lampXY.x, lampXY.y, x, y)) return;

            // 获取单元格索引
            var cell = Grid.XYToCell(x: x, y: y);

            KillGermsInCell(cell);
            KillGermsOnPickupable(cell);
            KillGermsOnConduits(cell);
            KillGermsOnBuildings(cell, buildingsAlreadySeen);
        }

        /// <summary>
        /// 在指定的细胞中消灭病菌。
        /// </summary>
        /// <param name="cell">要处理的细胞索引。</param>
        private void KillGermsInCell(int cell) {
            // 获取当前细胞中的病菌数量
            var count = Grid.DiseaseCount[cell];
            if (count > 0) SimMessages.ModifyDiseaseOnCell(cell, Grid.DiseaseIdx[cell], -count);
        }

        /// <summary>
        /// 减少可拾取物品上的病菌
        /// </summary>
        /// <param name="cell">网格单元索引，表示物体的位置。</param>
        private void KillGermsOnPickupable(int cell) {
            // 获取指定网格单元中的游戏对象。
            var objects = Grid.Objects[cell, 3];
            if (objects == null) return;

            // 获取游戏对象上的Pickupable组件中的objectLayerListItem。
            var objectLayerListItem = objects.GetComponent<Pickupable>().objectLayerListItem;
            while (objectLayerListItem != null) {
                // 获取当前objectLayerListItem关联的Pickupable组件。
                var component = objectLayerListItem.gameObject.GetComponent<Pickupable>();

                // 移动到下一个objectLayerListItem。
                objectLayerListItem = objectLayerListItem.nextItem;
                if (component != null) {
                    // 获取组件的主元素的疾病数量。
                    var diseaseCount = component.PrimaryElement.DiseaseCount;

                    // 减少组件的主元素的疾病数量。
                    component.PrimaryElement.ModifyDiseaseCount(-diseaseCount, "");
                }
            }
        }

        /// <summary>
        /// 管道
        /// </summary>
        /// <param name="cell">管道所在的单元格</param>
        private void KillGermsOnConduits(int cell) {
            // 获取指定单元格对应的管道
            var conduit = Game.Instance.solidConduitFlow.GetConduit(cell);

            // 检查管道是否有效
            if (!conduit.Equals(SolidConduitFlow.Conduit.Invalid())) {
                // 获取管道中的内容物
                var contents = conduit.GetContents(Game.Instance.solidConduitFlow);

                // 获取内容物中的可拾取物品
                var pickupable = Game.Instance.solidConduitFlow.GetPickupable(contents.pickupableHandle);

                // 如果存在可拾取物品，则进行杀菌操作
                if (pickupable != null) {
                    // 获取物品的主要元素中的疾病数量
                    var diseaseCount = pickupable.PrimaryElement.DiseaseCount;

                    // 减少疾病数量，实现杀菌效果
                    pickupable.PrimaryElement.ModifyDiseaseCount(-diseaseCount, "");
                }
            }
        }

        /// <summary>
        /// 在指定的单元格上消除建筑物上的细菌。
        /// </summary>
        /// <param name="cell">要处理的单元格索引。</param>
        /// <param name="buildingsAlreadySeen">一个集合，用于跟踪已经处理过的建筑物，以避免重复处理。</param>
        private void KillGermsOnBuildings(int cell, HashSet<GameObject> buildingsAlreadySeen) {
            // 获取当前单元格上的游戏对象
            var items = Grid.Objects[cell, 1];

            // 检查对象是否存在且未被处理过
            if (items != null && !buildingsAlreadySeen.Contains(items)) {
                // 尝试获取游戏对象上的PrimaryElement组件
                var component = items.GetComponent<PrimaryElement>();

                // 如果组件存在，则减少其疾病计数至0，以模拟杀菌过程
                if (component != null) component.ModifyDiseaseCount(-component.DiseaseCount, "");

                // 将当前处理的游戏对象添加到已处理集合中，以避免重复处理
                buildingsAlreadySeen.Add(items);
            }
        }
    }
}