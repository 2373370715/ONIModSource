using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

/// <summary>
///     完成建筑的基类，继承自 Building。
/// </summary>
public class BuildingComplete : Building {
    /// <summary>
    ///     用于处理被埋葬状态变化事件的委托。
    /// </summary>
    private static readonly EventSystem.IntraObjectHandler<BuildingComplete> OnEntombedChange
        = new EventSystem.IntraObjectHandler<BuildingComplete>(delegate(BuildingComplete component, object data) {
                                                                   component.OnEntombedChanged();
                                                               });

    /// <summary>
    ///     用于处理对象替换事件的委托。
    /// </summary>
    private static readonly EventSystem.IntraObjectHandler<BuildingComplete> OnObjectReplacedDelegate
        = new EventSystem.IntraObjectHandler<BuildingComplete>(delegate(BuildingComplete component, object data) {
                                                                   component.OnObjectReplaced(data);
                                                               });

    /// <summary>
    ///     记录游戏中见过的最低温度。
    /// </summary>
    public static float MinKelvinSeen = float.MaxValue;

    /// <summary>
    ///     建筑创建的时间。
    /// </summary>
    [Serialize]
    public float creationTime = -1f;

    private bool hasSpawnedKComponents;

    /// <summary>
    ///     建筑是否可作为艺术品。
    /// </summary>
    public bool isArtable;

    /// <summary>
    ///     建筑是否需要手动操作。
    /// </summary>
    public bool isManuallyOperated;

    /// <summary>
    ///     建筑的修饰器组件。
    /// </summary>
    [MyCmpReq]
    private Modifiers modifiers;

    /// <summary>
    ///     建筑的预制体ID组件。
    /// </summary>
    [MyCmpGet]
    public KPrefabID prefabid;

    /// <summary>
    ///     建筑的主要元素。
    /// </summary>
    public PrimaryElement primaryElement;

    /// <summary>
    ///     建筑的区域属性修饰器列表。
    /// </summary>
    public List<AttributeModifier> regionModifiers = new List<AttributeModifier>();

    /// <summary>
    ///     被替换的地砖层。
    /// </summary>
    private ObjectLayer replacingTileLayer = ObjectLayer.NumLayers;

    /// <summary>
    ///     场景分区器的入口句柄。
    /// </summary>
    private HandleVector<int>.Handle scenePartitionerEntry;

    /// <summary>
    ///     检查建筑是否被替换过。
    /// </summary>
    /// <returns>是否被替换过。</returns>
    private bool WasReplaced() { return replacingTileLayer != ObjectLayer.NumLayers; }

    /// <summary>
    ///     初始化预制件时调用的方法。
    /// </summary>
    protected override void OnPrefabInit() {
        base.OnPrefabInit();

        // 设置位置
        var position = transform.GetPosition();
        position.z = Grid.GetLayerZ(Def.SceneLayer);
        transform.SetPosition(position);

        // 设置层级
        gameObject.SetLayerRecursively(LayerMask.NameToLayer("Default"));

        // 设置动画控制器偏移
        var component                                                 = GetComponent<KBatchedAnimController>();
        var component2                                                = GetComponent<Rotatable>();
        if (component != null && component2 == null) component.Offset = Def.GetVisualizerOffset();

        // 设置碰撞器偏移
        var component3 = GetComponent<KBoxCollider2D>();
        if (component3 != null) {
            var visualizerOffset = Def.GetVisualizerOffset();
            component3.offset += new Vector2(visualizerOffset.x, visualizerOffset.y);
        }

        // 添加属性和属性修饰器
        var attributes = this.GetAttributes();
        foreach (var attribute in Def.attributes) attributes.Add(attribute);
        foreach (var attributeModifier in Def.attributeModifiers) {
            var attribute2 = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId);
            if (attributes.Get(attribute2) == null) attributes.Add(attribute2);
            attributes.Add(attributeModifier);
        }

        foreach (var attributeInstance in attributes) {
            var item = new AttributeModifier(attributeInstance.Id, attributeInstance.GetTotalValue());
            regionModifiers.Add(item);
        }

        // 添加热效应和结构温度管理
        if (Def.SelfHeatKilowattsWhenActive != 0f || Def.ExhaustKilowattsWhenActive != 0f)
            gameObject.AddOrGet<KBatchedAnimHeatPostProcessingEffect>();

        if (Def.UseStructureTemperature) GameComps.StructureTemperatures.Add(gameObject);

        // 订阅对象替换和被埋葬事件
        Subscribe(1606648047, OnObjectReplacedDelegate);
        if (Def.Entombable) Subscribe(-1089732772, OnEntombedChange);
    }

    /// <summary>
    ///     处理被埋葬状态变化事件。
    /// </summary>
    private void OnEntombedChanged() {
        if (gameObject.HasTag(GameTags.Entombed)) {
            Components.EntombedBuildings.Add(this);
            return;
        }

        Components.EntombedBuildings.Remove(this);
    }

    /// <summary>
    ///     更新建筑的位置。
    /// </summary>
    public override void UpdatePosition() {
        base.UpdatePosition();
        GameScenePartitioner.Instance.UpdatePosition(scenePartitionerEntry, GetExtents());
    }

    /// <summary>
    ///     处理对象替换事件。
    /// </summary>
    /// <param name="data">事件数据。</param>
    private void OnObjectReplaced(object data) {
        var replaceCallbackParameters = (Constructable.ReplaceCallbackParameters)data;
        replacingTileLayer = replaceCallbackParameters.TileLayer;
    }

    /// <summary>
    ///     实例化时调用的方法。
    /// </summary>
    protected override void OnSpawn() {
        base.OnSpawn();
        this.primaryElement = GetComponent<PrimaryElement>();
        var cell = Grid.PosToCell(transform.GetPosition());
        if (Def.IsFoundation)
            foreach (var num in PlacementCells) {
                Grid.Foundation[num] = true;
                Game.Instance.roomProber.SolidChangedEvent(num, false);
            }

        if (Grid.IsValidCell(cell)) {
            var position = Grid.CellToPosCBC(cell, Def.SceneLayer);
            transform.SetPosition(position);
        }

        if (this.primaryElement != null) {
            if (this.primaryElement.Mass == 0f) this.primaryElement.Mass = Def.Mass[0];
            var temperature                                              = this.primaryElement.Temperature;
            if (temperature > 0f && !float.IsNaN(temperature) && !float.IsInfinity(temperature))
                MinKelvinSeen = Mathf.Min(MinKelvinSeen, temperature);

            var primaryElement = this.primaryElement;
            primaryElement.setTemperatureCallback
                = (PrimaryElement.SetTemperatureCallback)Delegate.Combine(primaryElement.setTemperatureCallback,
                                                                          new PrimaryElement.
                                                                              SetTemperatureCallback(OnSetTemperature));
        }

        if (!this.gameObject.HasTag(GameTags.RocketInSpace)) {
            Def.MarkArea(cell, Orientation, Def.ObjectLayer, gameObject);
            if (Def.IsTilePiece) {
                Def.MarkArea(cell, Orientation, Def.TileLayer, gameObject);
                Def.RunOnArea(cell,
                              Orientation,
                              delegate(int c) { TileVisualizer.RefreshCell(c, Def.TileLayer, Def.ReplacementLayer); });
            }
        }

        RegisterBlockTileRenderer();
        if (Def.PreventIdleTraversalPastBuilding)
            for (var j = 0; j < PlacementCells.Length; j++)
                Grid.PreventIdleTraversal[PlacementCells[j]] = true;

        Components.BuildingCompletes.Add(this);
        BuildingConfigManager.Instance.AddBuildingCompleteKComponents(this.gameObject, Def.Tag);
        hasSpawnedKComponents = true;
        scenePartitionerEntry
            = GameScenePartitioner.Instance.Add(name,
                                                this,
                                                GetExtents(),
                                                GameScenePartitioner.Instance.completeBuildings,
                                                null);

        if (prefabid.HasTag(GameTags.TemplateBuilding)) Components.TemplateBuildings.Add(this);
        var attributes = this.GetAttributes();
        if (attributes != null) {
            var component = GetComponent<Deconstructable>();
            if (component != null) {
                var k = 1;
                while (k < component.constructionElements.Length) {
                    var tag     = component.constructionElements[k];
                    var element = ElementLoader.GetElement(tag);
                    if (element != null)
                        using (var enumerator = element.attributeModifiers.GetEnumerator()) {
                            while (enumerator.MoveNext()) {
                                var modifier = enumerator.Current;
                                attributes.Add(modifier);
                            }
                        }

                    var gameObject = Assets.TryGetPrefab(tag);
                    if (gameObject != null) {
                        var component2 = gameObject.GetComponent<PrefabAttributeModifiers>();
                        if (component2 != null)
                            foreach (var modifier2 in component2.descriptors)
                                attributes.Add(modifier2);
                    }

                    k++;
                }
            }
        }

        BuildingInventory.Instance.RegisterBuilding(this);
    }

    /// <summary>
    ///     设置建筑的创建时间。
    /// </summary>
    /// <param name="time">创建时间。</param>
    public void SetCreationTime(float time) { creationTime = time; }

    /// <summary>
    ///     获取检查声音。
    /// </summary>
    /// <returns>检查声音的路径。</returns>
    private string GetInspectSound() {
        return GlobalAssets.GetSound("AI_Inspect_" + GetComponent<KPrefabID>().PrefabTag.Name);
    }

    /// <summary>
    ///     清理资源时调用的方法。
    /// </summary>
    protected override void OnCleanUp() {
        if (Game.quitting) return;

        GameScenePartitioner.Instance.Free(ref scenePartitionerEntry);
        if (hasSpawnedKComponents)
            BuildingConfigManager.Instance.DestroyBuildingCompleteKComponents(gameObject, Def.Tag);

        if (Def.UseStructureTemperature) GameComps.StructureTemperatures.Remove(gameObject);
        base.OnCleanUp();
        if (!WasReplaced() && gameObject.GetMyWorldId() != 255) {
            var cell = Grid.PosToCell(this);
            Def.UnmarkArea(cell, Orientation, Def.ObjectLayer, gameObject);
            if (Def.IsTilePiece) {
                Def.UnmarkArea(cell, Orientation, Def.TileLayer, gameObject);
                Def.RunOnArea(cell,
                              Orientation,
                              delegate(int c) { TileVisualizer.RefreshCell(c, Def.TileLayer, Def.ReplacementLayer); });
            }

            if (Def.IsFoundation)
                foreach (var num in PlacementCells) {
                    Grid.Foundation[num] = false;
                    Game.Instance.roomProber.SolidChangedEvent(num, false);
                }

            if (Def.PreventIdleTraversalPastBuilding)
                for (var j = 0; j < PlacementCells.Length; j++)
                    Grid.PreventIdleTraversal[PlacementCells[j]] = false;
        }

        if (WasReplaced() && Def.IsTilePiece && replacingTileLayer != Def.TileLayer) {
            var cell2 = Grid.PosToCell(this);
            Def.UnmarkArea(cell2, Orientation, Def.TileLayer, gameObject);
            Def.RunOnArea(cell2,
                          Orientation,
                          delegate(int c) { TileVisualizer.RefreshCell(c, Def.TileLayer, Def.ReplacementLayer); });
        }

        Components.BuildingCompletes.Remove(this);
        Components.EntombedBuildings.Remove(this);
        Components.TemplateBuildings.Remove(this);
        UnregisterBlockTileRenderer();
        BuildingInventory.Instance.UnregisterBuilding(this);
        Trigger(-21016276, this);
    }

    /// <summary>
    ///     处理温度变化事件。
    /// </summary>
    /// <param name="primary_element">主要元素。</param>
    /// <param name="temperature">新温度。</param>
    private void OnSetTemperature(PrimaryElement primary_element, float temperature) {
        MinKelvinSeen = Mathf.Min(MinKelvinSeen, temperature);
    }
}