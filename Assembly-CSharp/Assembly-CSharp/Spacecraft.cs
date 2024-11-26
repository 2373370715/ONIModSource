using Database;
using KSerialization;
using STRINGS;
using TUNING;

[SerializationConfig(MemberSerialization.OptIn)]
public class Spacecraft {
    public enum MissionState {
        Grounded,
        Launching,
        Underway,
        WaitingToLand,
        Landing,
        Destroyed
    }

    [Serialize]
    public float controlStationBuffTimeRemaining;

    [Serialize]
    public int id = -1;

    [Serialize]
    private float missionDuration;

    [Serialize]
    private float missionElapsed;

    [Serialize]
    public Ref<LaunchConditionManager> refLaunchConditions = new Ref<LaunchConditionManager>();

    [Serialize]
    public string rocketName = UI.STARMAP.DEFAULT_NAME;

    [Serialize]
    public MissionState state;

    public Spacecraft(LaunchConditionManager launchConditions) { this.launchConditions = launchConditions; }
    public Spacecraft() { }

    public LaunchConditionManager launchConditions {
        get => refLaunchConditions.Get();
        set => refLaunchConditions.Set(value);
    }

    public void SetRocketName(string newName) {
        rocketName = newName;
        UpdateNameOnRocketModules();
    }

    public string GetRocketName() { return rocketName; }

    public void UpdateNameOnRocketModules() {
        foreach (var gameObject in AttachableBuilding.GetAttachedNetwork(launchConditions
                                                                             .GetComponent<AttachableBuilding>())) {
            var component = gameObject.GetComponent<RocketModule>();
            if (component != null) component.SetParentRocketName(rocketName);
        }
    }

    public bool HasInvalidID()               { return id == -1; }
    public void SetID(int             id)    { this.id    = id; }
    public void SetState(MissionState state) { this.state = state; }

    public void BeginMission(SpaceDestination destination) {
        // 剩余时间
        missionElapsed = 0f;

        // 发射总时间
        missionDuration = destination.OneBasedDistance *
                          ROCKETRY.MISSION_DURATION_SCALE /
                          GetPilotNavigationEfficiency();

        SetState(MissionState.Launching);
    }

    /// <summary>
    /// 获取飞行员的导航效率。
    /// </summary>
    /// <returns>返回飞行员的导航效率。</returns>
    private float GetPilotNavigationEfficiency() {
        // 初始化导航效率为1.0
        var num = 1f;
        
        // 检查是否是非机器人的飞行员控制
        if (!launchConditions.GetComponent<CommandModule>().robotPilotControlled) {
            // 获取存储的随从信息
            var storedMinionInfo = launchConditions.GetComponent<MinionStorage>().GetStoredMinionInfo();
            // 如果没有存储的随从信息，则直接返回1.0
            if (storedMinionInfo.Count < 1) return 1f;
    
            // 获取存储随从的身份信息
            var component = storedMinionInfo[0].serializedMinion.Get().GetComponent<StoredMinionIdentity>();
            // 获取太空导航属性ID
            var b         = Db.Get().Attributes.SpaceNavigation.Id;
            
            // 遍历随从的技能掌握情况
            using (var enumerator = component.MasteryBySkillID.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    var keyValuePair = enumerator.Current;
                    // 遍历当前技能的所有属性提升
                    foreach (var skillPerk in Db.Get().Skills.Get(keyValuePair.Key).perks)
                        // 检查技能所需的DLC是否在当前存档中全部激活
                        if (SaveLoader.Instance.IsAllDlcActiveForCurrentSave(skillPerk.requiredDlcIds)) {
                            var skillAttributePerk = skillPerk as SkillAttributePerk;
                            // 如果当前属性提升与太空导航相关，则增加导航效率
                            if (skillAttributePerk != null && skillAttributePerk.modifier.AttributeId == b)
                                num += skillAttributePerk.modifier.Value;
                        }
                }
    
                // 返回计算出的导航效率
                return num;
            }
        }
    
        // 如果是机器人的飞行员控制，检查数据银行存储情况
        var component2 = launchConditions.GetComponent<RoboPilotModule>();
        // 如果数据银行存储量达到或超过1，增加导航效率
        if (component2 != null && component2.GetDataBanksStored() >= 1f) num += component2.FlightEfficiencyModifier();
        
        // 返回最终的导航效率
        return num;
    }

    public void ForceComplete() { missionElapsed = missionDuration; }

    /// <summary>
    /// 根据给定的时间增量推进任务进度。
    /// </summary>
    /// <param name="deltaTime">自上次更新以来经过的时间量。</param>
    public void ProgressMission(float deltaTime) {
        // 仅当任务状态为进行中时，才推进任务进度
        if (state == MissionState.Underway) {
            // 累加经过的时间
            missionElapsed += deltaTime;

            // 如果控制站增益时间剩余大于0，则加快任务进度并减少剩余增益时间
            if (controlStationBuffTimeRemaining > 0f) {
                // 增益效果使任务进度加快
                missionElapsed += deltaTime * 0.20000005f;

                // 减少剩余的增益时间
                controlStationBuffTimeRemaining -= deltaTime;
            } else {
                // 如果增益时间已用完，将剩余增益时间设置为0
                controlStationBuffTimeRemaining = 0f;
            }

            // 如果任务进度超过任务持续时间，则完成任务
            if (missionElapsed > missionDuration) { CompleteMission(); }
        }
    }

    public float GetTimeLeft() { return missionDuration - missionElapsed; }
    public float GetDuration() { return missionDuration; }

    public void CompleteMission() {
        SpacecraftManager.instance.PushReadyToLandNotification(this);
        SetState(MissionState.WaitingToLand);
        Land();
    }

    private void Land() {
        launchConditions.Trigger(-1165815793, SpacecraftManager.instance.GetSpacecraftDestination(id));
        foreach (var gameObject in AttachableBuilding.GetAttachedNetwork(launchConditions
                                                                             .GetComponent<AttachableBuilding>()))
            if (gameObject != launchConditions.gameObject)
                gameObject.Trigger(-1165815793, SpacecraftManager.instance.GetSpacecraftDestination(id));
    }

    public void TemporallyTear() {
        SpacecraftManager.instance.hasVisitedWormHole = true;
        var launchConditions = this.launchConditions;
        for (var i = launchConditions.rocketModules.Count - 1; i >= 0; i--) {
            var component = launchConditions.rocketModules[i].GetComponent<Storage>();
            if (component != null) component.ConsumeAllIgnoringDisease();
            var component2 = launchConditions.rocketModules[i].GetComponent<MinionStorage>();
            if (component2 != null) {
                var storedMinionInfo = component2.GetStoredMinionInfo();
                for (var j = storedMinionInfo.Count - 1; j >= 0; j--)
                    component2.DeleteStoredMinion(storedMinionInfo[j].id);
            }

            Util.KDestroyGameObject(launchConditions.rocketModules[i].gameObject);
        }
    }

    public void GenerateName() { SetRocketName(GameUtil.GenerateRandomRocketName()); }
}