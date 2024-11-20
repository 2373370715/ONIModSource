using FMODUnity;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class AutoMiner : StateMachineComponent<AutoMiner.Instance>, ISim1000ms {
    private static readonly HashedString HASH_ROTATION = "rotation";

    private static readonly EventSystem.IntraObjectHandler<AutoMiner> OnOperationalChangedDelegate
        = new EventSystem.IntraObjectHandler<AutoMiner>(delegate(AutoMiner component, object data) {
                                                            component.OnOperationalChanged(data);
                                                        });

    private KBatchedAnimController arm_anim_ctrl;
    private GameObject             arm_go;
    private float                  arm_rot  = 45f;
    private int                    dig_cell = Grid.InvalidCell;
    public  int                    height;
    private GameObject             hitEffect;
    private GameObject             hitEffectPrefab;
    private KAnimLink              link;
    private LoopingSounds          looping_sounds;

    [MyCmpReq]
    private MiningSounds mining_sounds;

    [MyCmpReq]
    private Operational operational;

    [MyCmpGet]
    private Rotatable rotatable;

    private          bool           rotate_sound_playing;
    private          EventReference rotateSound;
    private readonly string         rotateSoundName = "AutoMiner_rotate";
    private          bool           rotation_complete;

    [MyCmpGet]
    private KSelectable selectable;

    [MyCmpAdd]
    private Storage storage;

    private readonly float      turn_rate = 180f;
    public           CellOffset vision_offset;
    public           int        width;
    public           int        x;
    public           int        y;
    private          bool       HasDigCell       => dig_cell != Grid.InvalidCell;
    private          bool       RotationComplete => HasDigCell && rotation_complete;

    public void Sim1000ms(float dt) {
        if (!operational.IsOperational) return;

        RefreshDiggableCell();
        operational.SetActive(HasDigCell);
    }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        simRenderLoadBalance = true;
    }

    /// <summary>
    /// 当对象生成时调用的方法
    /// </summary>
    protected override void OnSpawn() {
        // 调用基类的OnSpawn方法
        base.OnSpawn();

        // 获取并设置命中效果的预设
        hitEffectPrefab = Assets.GetPrefab("fx_dig_splash");

        // 获取当前对象的KBatchedAnimController组件
        var component = GetComponent<KBatchedAnimController>();

        // 创建一个新的GameObject作为机械臂，并设置其名称
        var name = component.name + ".gun";
        arm_go = new GameObject(name);

        // 将新创建的机械臂GameObject设置为非活动状态
        arm_go.SetActive(false);

        // 将机械臂GameObject的父对象设置为当前对象的transform
        arm_go.transform.parent = component.transform;

        // 在机械臂GameObject上添加LoopingSounds组件
        looping_sounds = arm_go.AddComponent<LoopingSounds>();

        // 获取旋转声音并设置为机械臂的旋转声音
        var sound = GlobalAssets.GetSound(rotateSoundName);
        rotateSound = RuntimeManager.PathToEventReference(sound);

        // 为机械臂GameObject添加KPrefabID组件并设置其PrefabTag
        arm_go.AddComponent<KPrefabID>().PrefabTag = new Tag(name);

        // 在机械臂GameObject上添加KBatchedAnimController组件并配置其属性
        arm_anim_ctrl             = arm_go.AddComponent<KBatchedAnimController>();
        arm_anim_ctrl.AnimFiles   = new[] { component.AnimFiles[0] };
        arm_anim_ctrl.initialAnim = "gun";
        arm_anim_ctrl.isMovable   = true;
        arm_anim_ctrl.sceneLayer  = Grid.SceneLayer.TransferArm;

        // 隐藏"gun_target"符号
        component.SetSymbolVisiblity("gun_target", false);

        // 获取"gun_target"符号的位置并设置机械臂GameObject的位置
        Vector3 position = component.GetSymbolTransform(new HashedString("gun_target"), out _).GetColumn(3);
        position.z = Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
        arm_go.transform.SetPosition(position);

        // 激活机械臂GameObject
        arm_go.SetActive(true);

        // 创建一个新的KAnimLink实例，链接当前对象和机械臂的动画控制器
        link = new KAnimLink(component, arm_anim_ctrl);

        // 订阅事件，当操作状态改变时调用OnOperationalChangedDelegate方法
        Subscribe(-592767678, OnOperationalChangedDelegate);

        // 调用RotateArm方法，设置机械臂的初始旋转位置
        RotateArm(rotatable.GetRotatedOffset(Quaternion.Euler(0f, 0f, -45f) * Vector3.up), true, 0f);

        // 调用StopDig方法停止挖掘
        StopDig();

        // 启动状态机
        smi.StartSM();
    }

    protected override void OnCleanUp() { base.OnCleanUp(); }

    private void OnOperationalChanged(object data) {
        if (!(bool)data) {
            dig_cell          = Grid.InvalidCell;
            rotation_complete = false;
        }
    }

    public void UpdateRotation(float dt) {
        if (HasDigCell) {
            var a = Grid.CellToPosCCC(dig_cell, Grid.SceneLayer.TileMain);
            a.z = 0f;
            var position = arm_go.transform.GetPosition();
            position.z = 0f;
            var target_dir = Vector3.Normalize(a - position);
            RotateArm(target_dir, false, dt);
        }
    }

    private Element GetTargetElement() {
        if (HasDigCell) return Grid.Element[dig_cell];

        return null;
    }

    public void StartDig() {
        var targetElement = GetTargetElement();
        Trigger(-1762453998, targetElement);
        CreateHitEffect();
        arm_anim_ctrl.Play("gun_digging", KAnim.PlayMode.Loop);
    }

    public void StopDig() {
        Trigger(939543986);
        DestroyHitEffect();
        arm_anim_ctrl.Play("gun", KAnim.PlayMode.Loop);
    }

    /// <summary>
    /// 更新挖掘操作
    /// </summary>
    /// <param name="dt">自上次更新以来的时间差</param>
    public void UpdateDig(float dt) {
        // 如果没有要挖掘的单元格，则不执行任何操作
        if (!HasDigCell) return;

        // 如果旋转动画未完成，则不执行任何操作
        if (!rotation_complete) return;

        // 执行挖掘操作，并根据挖掘进度更新音效
        Diggable.DoDigTick(dig_cell, dt, WorldDamage.DamageType.NoBuildingDamage);
        var percentComplete = Grid.Damage[dig_cell];
        mining_sounds.SetPercentComplete(percentComplete);

        // 计算挖掘位置，并确保其在正确的图层上
        var a = Grid.CellToPosCCC(dig_cell, Grid.SceneLayer.FXFront2);
        a.z = 0f;

        // 获取机械臂的位置，并确保其在正确的图层上
        var position = arm_go.transform.GetPosition();
        position.z = 0f;

        // 计算挖掘位置与机械臂位置之间的距离平方
        var sqrMagnitude = (a - position).sqrMagnitude;

        // 根据位置和距离更新挖掘动画的半径
        arm_anim_ctrl.GetBatchInstanceData().SetClipRadius(position.x, position.y, sqrMagnitude, true);

        // 如果当前挖掘的单元格不再有效，则重置挖掘状态
        if (!ValidDigCell(dig_cell)) {
            dig_cell          = Grid.InvalidCell;
            rotation_complete = false;
        }
    }

    private void CreateHitEffect() {
        if (hitEffectPrefab == null) return;

        if (hitEffect != null) DestroyHitEffect();
        var position = Grid.CellToPosCCC(dig_cell, Grid.SceneLayer.FXFront2);
        hitEffect = GameUtil.KInstantiate(hitEffectPrefab, position, Grid.SceneLayer.FXFront2);
        hitEffect.SetActive(true);
        var component = hitEffect.GetComponent<KBatchedAnimController>();
        component.sceneLayer  = Grid.SceneLayer.FXFront2;
        component.initialMode = KAnim.PlayMode.Loop;
        component.enabled     = false;
        component.enabled     = true;
    }

    private void DestroyHitEffect() {
        if (hitEffectPrefab == null) return;

        if (hitEffect != null) {
            hitEffect.DeleteObject();
            hitEffect = null;
        }
    }

    /// <summary>
    /// 刷新可挖掘单元格的信息。
    /// </summary>
    private void RefreshDiggableCell() {
        // 初始化旋转后的单元格偏移量。
        var rotatedCellOffset = vision_offset;
        if (rotatable) {
            // 如果可旋转，则获取旋转后的单元格偏移量。
            rotatedCellOffset = rotatable.GetRotatedCellOffset(vision_offset);
        }

        // 将当前位置转换为单元格坐标。
        var cell = Grid.PosToCell(transform.gameObject);

        // 根据旋转后的偏移量计算目标单元格。
        var cell2 = Grid.OffsetCell(cell, rotatedCellOffset);

        // 初始化用于存储单元格坐标的变量。
        int num;
        int num2;

        // 将单元格转换为XY坐标。
        Grid.CellToXY(cell2, out num, out num2);

        // 初始化距离和无效单元格变量。
        var num3 = float.MaxValue;
        var num4 = Grid.InvalidCell;

        // 获取单元格的位置。
        var a    = Grid.CellToPos(cell2);
        var flag = false;

        // 遍历所有单元格以找到最近的可挖掘单元格。
        for (var i = 0; i < height; i++) {
            for (var j = 0; j < width; j++) {
                // 计算当前单元格的偏移量。
                var rotatedCellOffset2 = new CellOffset(this.x + j, this.y + i);
                if (rotatable) {
                    // 如果可旋转，则获取旋转后的单元格偏移量。
                    rotatedCellOffset2 = rotatable.GetRotatedCellOffset(rotatedCellOffset2);
                }

                // 根据偏移量计算单元格。
                var num5 = Grid.OffsetCell(cell, rotatedCellOffset2);
                if (Grid.IsValidCell(num5)) {
                    int x;
                    int y;

                    // 将单元格转换为XY坐标。
                    Grid.CellToXY(num5, out x, out y);

                    // 检查单元格是否有效且可挖掘，并且在视线范围内。
                    if (Grid.IsValidCell(num5) &&
                        ValidDigCell(num5)     &&
                        Grid.TestLineOfSight(num, num2, x, y, DigBlockingCB)) {
                        if (num5 == dig_cell) { flag = true; }

                        // 计算当前位置到目标单元格的距离。
                        var b    = Grid.CellToPos(num5);
                        var num6 = Vector3.Distance(a, b);

                        // 更新最近的可挖掘单元格。
                        if (num6 < num3) {
                            num3 = num6;
                            num4 = num5;
                        }
                    }
                }
            }
        }

        // 如果没有找到当前挖掘的单元格，则更新挖掘单元格。
        if (!flag && dig_cell != num4) {
            dig_cell          = num4;
            rotation_complete = false;
        }
    }

    /// <summary>
    /// 验证指定的单元格是否适合进行挖掘操作。
    /// </summary>
    /// <param name="cell">要验证的单元格。</param>
    /// <returns>如果单元格适合进行挖掘操作，则返回true；否则返回false。</returns>
    private static bool ValidDigCell(int cell) {
        // 检查单元格是否有门、是否有基础结构、以及是否在第9层对象中
        var flag = Grid.HasDoor[cell] && Grid.Foundation[cell] && Grid.ObjectLayers[9].ContainsKey(cell);
        if (flag) {
            // 获取第9层对象中的门组件
            var component = Grid.ObjectLayers[9][cell].GetComponent<Door>();
            // 检查门组件是否存在、门是否打开、以及门是否即将关闭
            flag = component != null && component.IsOpen() && !component.IsPendingClose();
        }
    
        // 返回单元格是否为实体、是否没有基础结构或有有效的门、以及元素的硬度是否小于150
        return Grid.Solid[cell] && (!Grid.Foundation[cell] || flag) && Grid.Element[cell].hardness < 150;
    }

    /// <summary>
    /// 检查指定单元格是否可以被挖掘并存在阻塞条件。
    /// </summary>
    /// <param name="cell">要检查的单元格索引。</param>
    /// <returns>如果指定单元格满足挖掘阻塞条件则返回true，否则返回false。</returns>
    public static bool DigBlockingCB(int cell) {
        // 检查单元格中是否有门、基础结构以及特定对象层的存在
        var flag = Grid.HasDoor[cell] && Grid.Foundation[cell] && Grid.ObjectLayers[9].ContainsKey(cell);
        if (flag) {
            // 获取单元格中对象的门组件
            var component = Grid.ObjectLayers[9][cell].GetComponent<Door>();
            // 更新标志，检查门是否打开且不处于关闭待处理状态
            flag = component != null && component.IsOpen() && !component.IsPendingClose();
        }
    
        // 返回是否满足挖掘阻塞条件：基础结构且实体且无门阻塞，或者单元格硬度大于等于150
        return (Grid.Foundation[cell] && Grid.Solid[cell] && !flag) || Grid.Element[cell].hardness >= 150;
    }

    private void RotateArm(Vector3 target_dir, bool warp, float dt) {
        if (rotation_complete) return;

        var num = MathUtil.AngleSigned(Vector3.up, target_dir, Vector3.forward) - arm_rot;
        num               = MathUtil.Wrap(-180f, 180f, num);
        rotation_complete = Mathf.Approximately(num, 0f);
        var num2 = num;
        if (warp)
            rotation_complete = true;
        else
            num2 = Mathf.Clamp(num2, -turn_rate * dt, turn_rate * dt);

        arm_rot                   += num2;
        arm_rot                   =  MathUtil.Wrap(-180f, 180f, arm_rot);
        arm_go.transform.rotation =  Quaternion.Euler(0f, 0f, arm_rot);
        if (!rotation_complete) {
            StartRotateSound();
            looping_sounds.SetParameter(rotateSound, HASH_ROTATION, arm_rot);
            return;
        }

        StopRotateSound();
    }

    private void StartRotateSound() {
        if (!rotate_sound_playing) {
            looping_sounds.StartSound(rotateSound);
            rotate_sound_playing = true;
        }
    }

    private void StopRotateSound() {
        if (rotate_sound_playing) {
            looping_sounds.StopSound(rotateSound);
            rotate_sound_playing = false;
        }
    }

    public class Instance : GameStateMachine<States, Instance, AutoMiner, object>.GameInstance {
        public Instance(AutoMiner master) : base(master) { }
    }

    public class States : GameStateMachine<States, Instance, AutoMiner> {
        public State         off;
        public ReadyStates   on;
        public BoolParameter transferring;

        public override void InitializeStates(out BaseState default_state) {
            default_state = off;
            root.DoNothing();
            off.PlayAnim("off")
               .EventTransition(GameHashes.OperationalChanged,
                                on,
                                smi => smi.GetComponent<Operational>().IsOperational);

            on.DefaultState(on.idle)
              .EventTransition(GameHashes.OperationalChanged,
                               off,
                               smi => !smi.GetComponent<Operational>().IsOperational);

            on.idle.PlayAnim("on")
              .EventTransition(GameHashes.ActiveChanged, on.moving, smi => smi.GetComponent<Operational>().IsActive);

            on.moving.Exit(delegate(AutoMiner.Instance smi) { smi.master.StopRotateSound(); })
              .PlayAnim("working")
              .EventTransition(GameHashes.ActiveChanged, on.idle, smi => !smi.GetComponent<Operational>().IsActive)
              .Update(delegate(AutoMiner.Instance smi, float dt) { smi.master.UpdateRotation(dt); },
                      UpdateRate.SIM_33ms)
              .Transition(on.digging, RotationComplete);

            on.digging.Enter(delegate(AutoMiner.Instance smi) { smi.master.StartDig(); })
              .Exit(delegate(AutoMiner.Instance          smi) { smi.master.StopDig(); })
              .PlayAnim("working")
              .EventTransition(GameHashes.ActiveChanged, on.idle, smi => !smi.GetComponent<Operational>().IsActive)
              .Update(delegate(AutoMiner.Instance smi, float dt) { smi.master.UpdateDig(dt); })
              .Transition(on.moving, Not(RotationComplete));
        }

        public static bool RotationComplete(AutoMiner.Instance smi) { return smi.master.RotationComplete; }

        public class ReadyStates : State {
            public State digging;
            public State idle;
            public State moving;
        }
    }
}