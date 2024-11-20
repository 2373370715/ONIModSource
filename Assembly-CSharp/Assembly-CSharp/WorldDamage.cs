using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/WorldDamage")]
public class WorldDamage : KMonoBehaviour {
    public enum DamageType {
        Absolute,
        NoBuildingDamage
    }

    private const    float                  SPAWN_DELAY  = 1f;
    private readonly float                  damageAmount = 0.00083333335f;
    private readonly List<int>              expiredCells = new List<int>();
    public           KBatchedAnimController leakEffect;

    [SerializeField]
    private FMODAsset leakSound;

    [SerializeField]
    private EventReference leakSoundMigrated;

    private readonly   Dictionary<int, float> spawnTimes = new Dictionary<int, float>();
    public static      WorldDamage            Instance          { get; private set; }
    public static      void                   DestroyInstance() { Instance = null; }
    protected override void                   OnPrefabInit()    { Instance = this; }

    public void RestoreDamageToValue(int cell, float amount) {
        if (Grid.Damage[cell] > amount) Grid.Damage[cell] = amount;
    }

    public float ApplyDamage(Sim.WorldDamageInfo damage_info) {
        return ApplyDamage(damage_info.gameCell,
                           damageAmount,
                           damage_info.damageSourceOffset,
                           BUILDINGS.DAMAGESOURCES.LIQUID_PRESSURE,
                           UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.LIQUID_PRESSURE);
    }

    /// <summary>
    /// 应用伤害到指定的单元格。
    /// </summary>
    /// <param name="cell">要应用伤害的单元格。</param>
    /// <param name="amount">伤害量。</param>
    /// <param name="src_cell">伤害来源的单元格。</param>
    /// <param name="damageType">伤害类型。</param>
    /// <param name="source_name">伤害来源的名称。</param>
    /// <param name="pop_text">伤害弹出文本。</param>
    /// <returns>返回实际应用的伤害量。</returns>
    public float ApplyDamage(int        cell,
                             float      amount,
                             int        src_cell,
                             DamageType damageType,
                             string     source_name = null,
                             string     pop_text    = null) {
        var result = 0f;

        // 检查目标单元格是否为固体
        if (Grid.Solid[cell]) {
            var num = Grid.Damage[cell];
            result =  Mathf.Min(amount, 1f - num); // 计算实际应用的伤害量
            num    += amount;                      // 更新单元格的总伤害量
            var flag = num > 0.15f;                // 判断伤害是否超过阈值

            // 如果伤害超过阈值且不是无建筑伤害类型
            if (flag && damageType != DamageType.NoBuildingDamage) {
                var gameObject = Grid.Objects[cell, 9]; // 获取单元格中的游戏对象
                if (gameObject != null) {
                    var component = gameObject.GetComponent<BuildingHP>(); // 获取建筑健康组件
                    if (component != null) {
                        if (!component.invincible) {
                            // 如果建筑不是无敌状态
                            var damage = Mathf.RoundToInt(Mathf.Max(component.HitPoints -
                                                                    (1f - num) * component.MaxHitPoints,
                                                                    0f)); // 计算建筑的实际伤害

                            // 触发建筑的伤害事件
                            gameObject.Trigger(-794517298,
                                               new BuildingHP.DamageSourceInfo {
                                                   damage = damage, source = source_name, popString = pop_text
                                               });
                        } else {
                            num = 0f; // 如果建筑无敌，重置伤害量
                        }
                    }
                }
            }

            Grid.Damage[cell] = Mathf.Min(1f, num); // 更新单元格的伤害值
            if (Grid.Damage[cell] >= 1f) {
                DestroyCell(cell); // 如果伤害达到最大值，销毁单元格
            } else if (Grid.IsValidCell(src_cell) && flag) {
                var element = Grid.Element[src_cell]; // 获取伤害来源单元格的元素
                if (element.IsLiquid && Grid.Mass[src_cell] > 1f) {
                    // 如果来源单元格是液体且质量大于1
                    var num2 = cell - src_cell; // 计算目标单元格与来源单元格的相对位置
                    if (num2 == 1 || num2 == -1 || num2 == Grid.WidthInCells || num2 == -Grid.WidthInCells) {
                        var num3     = cell + num2;        // 计算相邻单元格的位置
                        var element2 = Grid.Element[num3]; // 获取相邻单元格的元素
                        if (Grid.IsValidCell(num3)) {
                            if (!element2.IsSolid && // 检查相邻单元格是否为非固体
                                (!element2.IsLiquid ||
                                 (element2.id     == element.id &&
                                  Grid.Mass[num3] <= 100f))      && // 检查相邻单元格是否为非液体或相同类型的液体且质量小于等于100
                                (Grid.Properties[num3] & 2) == 0 && // 检查相邻单元格的属性
                                !spawnTimes.ContainsKey(num3)) {
                                // 检查相邻单元格是否已经记录过
                                spawnTimes[num3] = Time.realtimeSinceStartup; // 记录相邻单元格的时间
                                var idx         = element.idx;                // 获取元素索引
                                var temperature = Grid.Temperature[src_cell]; // 获取来源单元格的温度
                                StartCoroutine(DelayedSpawnFX(src_cell,
                                                              num3,
                                                              num2,
                                                              element,
                                                              idx,
                                                              temperature)); // 启动延迟生成效果的协程
                            }
                        }
                    }
                }
            }
        }

        return result; // 返回实际应用的伤害量
    }

    public float ApplyDamage(int cell, float amount, int src_cell, string source_name = null, string pop_text = null) {
        return ApplyDamage(cell, amount, src_cell, DamageType.Absolute, source_name, pop_text);
    }

    private void ReleaseGO(GameObject go) { go.DeleteObject(); }

    private IEnumerator DelayedSpawnFX(int     src_cell,
                                       int     dest_cell,
                                       int     offset,
                                       Element elem,
                                       ushort  idx,
                                       float   temperature) {
        var seconds = Random.value * 0.25f;
        yield return new WaitForSeconds(seconds);

        var position   = Grid.CellToPosCCC(dest_cell, Grid.SceneLayer.Front);
        var gameObject = GameUtil.KInstantiate(leakEffect.gameObject, position, Grid.SceneLayer.Front);

        var component = gameObject.GetComponent<KBatchedAnimController>();
        component.TintColour    = elem.substance.colour;
        component.onDestroySelf = ReleaseGO;
        SimMessages.AddRemoveSubstance(src_cell,
                                       idx,
                                       CellEventLogger.Instance.WorldDamageDelayedSpawnFX,
                                       -1f,
                                       temperature,
                                       byte.MaxValue,
                                       0);

        if (offset == -1) {
            component.Play("side");
            component.FlipX   = true;
            component.enabled = false;
            component.enabled = true;
            gameObject.transform.SetPosition(gameObject.transform.GetPosition() + Vector3.right * 0.5f);
            FallingWater.instance.AddParticle(dest_cell, idx, 1f, temperature, byte.MaxValue, 0, true);
        } else if (offset == Grid.WidthInCells) {
            gameObject.transform.SetPosition(gameObject.transform.GetPosition() - Vector3.up * 0.5f);
            component.Play("floor");
            component.enabled = false;
            component.enabled = true;
            SimMessages.AddRemoveSubstance(dest_cell,
                                           idx,
                                           CellEventLogger.Instance.WorldDamageDelayedSpawnFX,
                                           1f,
                                           temperature,
                                           byte.MaxValue,
                                           0);
        } else if (offset == -Grid.WidthInCells) {
            component.Play("ceiling");
            component.enabled = false;
            component.enabled = true;
            gameObject.transform.SetPosition(gameObject.transform.GetPosition() + Vector3.up * 0.5f);
            FallingWater.instance.AddParticle(dest_cell, idx, 1f, temperature, byte.MaxValue, 0, true);
        } else {
            component.Play("side");
            component.enabled = false;
            component.enabled = true;
            gameObject.transform.SetPosition(gameObject.transform.GetPosition() - Vector3.right * 0.5f);
            FallingWater.instance.AddParticle(dest_cell, idx, 1f, temperature, byte.MaxValue, 0, true);
        }

        if (CameraController.Instance.IsAudibleSound(gameObject.transform.GetPosition(), leakSoundMigrated))
            SoundEvent.PlayOneShot(leakSoundMigrated, gameObject.transform.GetPosition());

        yield return null;
    }

    private void Update() {
        expiredCells.Clear();
        var realtimeSinceStartup = Time.realtimeSinceStartup;
        foreach (var keyValuePair in spawnTimes)
            if (realtimeSinceStartup - keyValuePair.Value > 1f)
                expiredCells.Add(keyValuePair.Key);

        foreach (var key in expiredCells) spawnTimes.Remove(key);

        expiredCells.Clear();
    }

    public void DestroyCell(int cell) {
        if (Grid.Solid[cell]) SimMessages.Dig(cell);
    }

    public void OnSolidStateChanged(int cell) { Grid.Damage[cell] = 0f; }

    public void OnDigComplete(int    cell,
                              float  mass,
                              float  temperature,
                              ushort element_idx,
                              byte   disease_idx,
                              int    disease_count) {
        var vector  = Grid.CellToPos(cell, CellAlignment.RandomInternal, Grid.SceneLayer.Ore);
        var element = ElementLoader.elements[element_idx];
        Grid.Damage[cell] = 0f;
        Instance.PlaySoundForSubstance(element, vector);
        var num = mass * 0.5f;
        if (num <= 0f) return;

        var gameObject = element.substance.SpawnResource(vector, num, temperature, disease_idx, disease_count);

        var component = gameObject.GetComponent<Pickupable>();
        if (component              != null &&
            component.GetMyWorld() != null &&
            component.GetMyWorld().worldInventory.IsReachable(component))
            PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource,
                                          Mathf.RoundToInt(num) + " " + element.name,
                                          gameObject.transform);
    }

    private void PlaySoundForSubstance(Element element, Vector3 pos) {
        var text = element.substance.GetMiningBreakSound();
        if (text == null) {
            if (element.HasTag(GameTags.RefinedMetal))
                text = "RefinedMetal";
            else if (element.HasTag(GameTags.Metal))
                text = "RawMetal";
            else
                text = "Rock";
        }

        text = "Break_" + text;
        text = GlobalAssets.GetSound(text);
        if (CameraController.Instance && CameraController.Instance.IsAudibleSound(pos, text))
            KFMOD.PlayOneShot(text, CameraController.Instance.GetVerticallyScaledPosition(pos));
    }
}