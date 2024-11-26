using System.Collections.Generic;

public class MissionControl
    : GameStateMachine<MissionControl, MissionControl.Instance, IStateMachineTarget, MissionControl.Def> {
    public State            Inoperational;
    public OperationalState Operational;
    public BoolParameter    WorkableRocketsAreInRange;

    /// 初始化状态机的状态和转换规则
    /// <param name="default_state">输出参数，指定状态机的默认状态</param>
    public override void InitializeStates(out BaseState default_state) {
        // 设置默认状态为 Inoperational（不可操作）
        default_state = Inoperational;
    
        // 定义不可操作状态下的事件转换规则
        Inoperational.EventTransition(GameHashes.OperationalChanged, Operational, ValidateOperationalTransition)
                     .EventTransition(GameHashes.UpdateRoom, Operational, ValidateOperationalTransition);
    
        // 定义可操作状态下的事件转换规则和行为
        Operational.EventTransition(GameHashes.OperationalChanged, Inoperational, ValidateOperationalTransition)
                   .EventTransition(GameHashes.UpdateRoom, Operational.WrongRoom, Not(IsInLabRoom))
                   .Enter(OnEnterOperational)
                   .DefaultState(Operational.NoRockets)
                   .Update(delegate(Instance smi, float dt) { smi.UpdateWorkableRockets(null); },
                           UpdateRate.SIM_1000ms);
    
        // 定义房间错误状态下的事件转换规则
        Operational.WrongRoom.EventTransition(GameHashes.UpdateRoom, Operational.NoRockets, IsInLabRoom);
        
        // 定义没有火箭状态的显示和转换规则
        Operational.NoRockets.ToggleStatusItem(Db.Get().BuildingStatusItems.NoRocketsToMissionControlBoost, null)
                   .ParamTransition(WorkableRocketsAreInRange,
                                    Operational.HasRockets,
                                    (smi, inRange) => WorkableRocketsAreInRange.Get(smi));
    
        // 定义有火箭状态的转换规则和行为
        Operational.HasRockets
                   .ParamTransition(WorkableRocketsAreInRange,
                                    Operational.NoRockets,
                                    (smi, inRange) => !WorkableRocketsAreInRange.Get(smi))
                   .ToggleChore(CreateChore, Operational);
    }

    private Chore CreateChore(Instance smi) {
        var   component = smi.master.gameObject.GetComponent<MissionControlWorkable>();
        Chore result = new WorkChore<MissionControlWorkable>(Db.Get().ChoreTypes.Research, component);
        var   randomBoostableSpacecraft = smi.GetRandomBoostableSpacecraft();
        component.TargetSpacecraft = randomBoostableSpacecraft;
        return result;
    }
    /// <summary>
    /// 处理进入操作状态时的行为
    /// </summary>
    /// <param name="smi">The smi instance</param>
    private void OnEnterOperational(Instance smi) {
        // 更新当前实例的工作火箭状态
        smi.UpdateWorkableRockets(null);
        
        // 检查工作范围内的火箭是否可用
        if (WorkableRocketsAreInRange.Get(smi)) {
            // 如果有可用火箭，则进入"有火箭"操作状态
            smi.GoTo(Operational.HasRockets);
            return;
        }
    
        // 如果没有可用火箭，则进入"无火箭"操作状态
        smi.GoTo(Operational.NoRockets);
    }

    private bool ValidateOperationalTransition(Instance smi) {
        var component = smi.GetComponent<Operational>();
        var flag      = smi.IsInsideState(smi.sm.Operational);
        return component != null && flag != component.IsOperational;
    }

    private bool IsInLabRoom(Instance smi) { return smi.roomTracker.IsInCorrectRoom(); }

    public class Def : BaseDef { }

    public new class Instance : GameInstance {
        private readonly List<Spacecraft> boostableSpacecraft = new List<Spacecraft>();

        [MyCmpReq]
        public RoomTracker roomTracker;

        public Instance(IStateMachineTarget master, Def def) : base(master, def) { }

        /// <summary>
        /// 更新可工作的火箭列表。
        /// </summary>
        /// <param name="data">传入的数据对象，未在方法中使用。</param>
        public void UpdateWorkableRockets(object data) {
            // 清空当前可助推的航天器列表
            boostableSpacecraft.Clear();
        
            // 遍历所有航天器，检查它们是否可以被助推
            for (var i = 0; i < SpacecraftManager.instance.GetSpacecraft().Count; i++) {
                // 检查当前航天器是否可以被助推
                if (CanBeBoosted(SpacecraftManager.instance.GetSpacecraft()[i])) {
                    // 初始化标志变量，用于判断当前航天器是否已经被其他任务控制工作台选中
                    var flag = false;
        
                    // 遍历所有任务控制工作台，检查它们是否已经选中了当前航天器
                    foreach (var obj in Components.MissionControlWorkables) {
                        var missionControlWorkable = (MissionControlWorkable)obj;
        
                        // 如果当前任务控制工作台选中了当前航天器，则设置标志为true，并终止内层循环
                        if (!(missionControlWorkable.gameObject == gameObject) &&
                            missionControlWorkable.TargetSpacecraft == SpacecraftManager.instance.GetSpacecraft()[i]) {
                            flag = true;
                            break;
                        }
                    }
        
                    // 如果当前航天器没有被其他任务控制工作台选中，则将其添加到可助推的航天器列表中
                    if (!flag) {
                        boostableSpacecraft.Add(SpacecraftManager.instance.GetSpacecraft()[i]);
                    }
                }
            }
        
            // 更新状态机，指示是否有可工作的火箭在范围内
            sm.WorkableRocketsAreInRange.Set(boostableSpacecraft.Count > 0, smi);
        }

        public Spacecraft GetRandomBoostableSpacecraft() { return boostableSpacecraft.GetRandom(); }

        private bool CanBeBoosted(Spacecraft spacecraft) {
            return spacecraft.controlStationBuffTimeRemaining == 0f &&
                   spacecraft.state                           == Spacecraft.MissionState.Underway;
        }

        public void ApplyEffect(Spacecraft spacecraft) { spacecraft.controlStationBuffTimeRemaining = 600f; }
    }

    public class OperationalState : State {
        public State HasRockets;
        public State NoRockets;
        public State WrongRoom;
    }
}