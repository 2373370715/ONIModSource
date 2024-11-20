namespace Store {
    public sealed class StoreTem : KMonoBehaviour, ISimEveryTick {
        private int index;

        /// <summary>
        ///     每帧运行
        /// </summary>
        /// <param name="dt">自上次更新以来的时间差。</param>
        public void SimEveryTick(float dt) {
            // 获取当前对象是否可用
            var flag = GetComponent<Operational>().IsOperational;
            if (flag) {
                // 获取坐标
                var worldFromPosition = ClusterManager.Instance.GetWorldFromPosition(PosMin());

                // 每帧运行几次
                for (var i = 0; i < 10; i++) {
                    // 获取世界坐标系中的偏移量
                    var worldOffset = worldFromPosition.WorldOffset;

                    // 计算y偏移量
                    worldOffset.y += index / worldFromPosition.WorldSize.X % worldFromPosition.WorldSize.Y;

                    // 计算x偏移量
                    worldOffset.x += index % worldFromPosition.WorldSize.X;

                    // 将坐标转换为单元格位置
                    var cell = Grid.PosToCell(worldOffset);

                    // 堆叠物品
                    if (Grid.Objects[cell, 3]) TryPickup(cell);

                    // 增加索引以进行下一次尝试
                    index++;
                }
            }
        }

        /// <summary>
        ///     尝试捡起指定单元格中的物品。
        /// </summary>
        /// <param name="cell">要捡起物品的单元格索引。</param>
        public void TryPickup(int cell) {
            // 物品在第四个图层
            var gameObject = Grid.Objects[cell, 3];

            // 如果有物品
            if (gameObject) {
                // 最多运行1s
                var num = System.DateTime.Now.Ticks + 10000000L;

                // 获取物品列表
                var objectLayerListItem = gameObject.GetComponent<Pickupable>().objectLayerListItem;

                // 遍历物品列表
                while (objectLayerListItem != null && System.DateTime.Now.Ticks < num) {
                    // 获取当前物品
                    var gameObject2 = objectLayerListItem.gameObject;
                    var component   = gameObject2.GetComponent<Pickupable>();

                    // 监测是否标记为拾取
                    var flag2 = component != null && component.GetComponent<KPrefabID>().HasTag(GameTags.Garbage);

                    // 判断是否是食物
                    var is_food = false;
                    // if (component                                  != null &&
                        // component.Take(component.UnreservedAmount) != null &&
                        // component.GetComponent<PrimaryElement>()   != null)
                        // is_food = component.Take(component.UnreservedAmount).GetComponent<PrimaryElement>().ElementID ==
                                  // SimHashes.Creature;

                    if (flag2 && !is_food) {
                        // 存储
                        Store(component.Take(component.UnreservedAmount));
                    }

                    objectLayerListItem = objectLayerListItem.nextItem;
                }
            }
        }

        /// <summary>
        ///     存储可拾取物品到存储组件中。
        /// </summary>
        /// <param name="pickupable">要存储的可拾取物品。</param>
        public void Store(Pickupable pickupable) {
            if (pickupable) {
                var storage = GetComponent<Storage>();

                // 获取可拾取物品的主要元素
                var primaryElement = pickupable.GetComponent<PrimaryElement>();
                pickupable.PrimaryElement.AddDisease(primaryElement.DiseaseIdx, -primaryElement.DiseaseCount, "");
                pickupable.PrimaryElement.Temperature = 293.15f;

                primaryElement = pickupable.GetComponent<PrimaryElement>();

                if (primaryElement)

                    // 检查有没有可以合并的
                    for (var i = storage.items.Count - 1; i >= 0; i--) {
                        var gameObject = storage.items[i];

                        var otherPickupable = gameObject.GetComponent<Pickupable>();
                        var otherElement    = gameObject.GetComponent<PrimaryElement>();
                        if (otherElement != null) {
                            otherPickupable.PrimaryElement.Temperature         = 293.15f;
                            otherPickupable.PrimaryElement.InternalTemperature = 293.15f;
                        }

                        otherElement = otherPickupable.GetComponent<PrimaryElement>();

                        // 判断是否可合并，且合并后质量未超过最大值
                        var flag3 = primaryElement.ElementID                     == otherElement.ElementID &&
                                    pickupable.GetComponent<ElementChunk>()      != null                   &&
                                    otherPickupable.GetComponent<ElementChunk>() != null                   &&
                                    primaryElement.Mass + otherElement.Mass      <= PrimaryElement.MAX_MASS;

                        if (flag3) {
                            var elementAbsorb      = pickupable.CanAbsorb;
                            var otherElementAbsorb = otherPickupable.CanAbsorb;

                            pickupable.CanAbsorb      = other => true;
                            otherPickupable.CanAbsorb = other => true;
                            var flag4 = otherPickupable.TryAbsorb(pickupable, true, true);

                            pickupable.CanAbsorb      = elementAbsorb;
                            otherPickupable.CanAbsorb = otherElementAbsorb;
                        }
                    }

                // 如果没有找到可合并的物品，直接将可拾取物品存储到组件中。
                storage.Store(pickupable.gameObject, true, false, false);
            }
        }
    }
}