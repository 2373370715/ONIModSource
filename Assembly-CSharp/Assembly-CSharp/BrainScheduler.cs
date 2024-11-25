using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/BrainScheduler")]
public class BrainScheduler : KMonoBehaviour, IRenderEveryTick, ICPULoad {
    // 定义每帧的时间长度，单位为毫秒
    public const float millisecondsPerFrame = 33.33333f;
    // 定义每帧的时间长度，单位为秒
    public const float secondsPerFrame = 0.033333328f;
    // 定义每秒的帧数
    public const float framesPerSecond = 30.000006f;
    // 初始化一个脑组列表，用于管理不同的脑组对象
    private readonly List<BrainGroup> brainGroups = new List<BrainGroup>();
    // 根据全局调优数据判断是否启用异步路径探测
    private bool isAsyncPathProbeEnabled => !TuningData<Tuning>.Get().disableAsyncPathProbes;
    // 获取估计的帧时间，根据当前的全局调优数据返回
    public float GetEstimatedFrameTime() { return TuningData<Tuning>.Get().frameTime; }
    // 调整加载状态，当前实现中总是返回false，表示不进行调整
    public bool AdjustLoad(float currentFrameTime, float frameTimeDelta) { return false; }

    /// <summary>
    /// 每帧渲染时调用的方法。
    /// </summary>
    /// <param name="dt">自上次渲染以来经过的时间。</param>
    public void RenderEveryTick(float dt) {
        // 检查游戏是否正在退出或正在加载场景，如果是，则不执行任何操作。
        if (Game.IsQuitting() || isLoadingScene) return;
    
        // 遍历所有脑组，调用它们的RenderEveryTick方法，传递自上次渲染以来经过的时间。
        foreach (var brainGroup in brainGroups) brainGroup.RenderEveryTick(dt);
    }

    /// <summary>
    /// 调试方法：获取所有大脑组。
    /// </summary>
    /// <returns>返回大脑组的列表。</returns>
    public List<BrainGroup> debugGetBrainGroups() { return brainGroups; }

    /// <summary>
    /// 在 prefab 初始化时调用的方法。
    /// </summary>
    protected override void OnPrefabInit() {
        // 初始化大脑组，添加两种类型的大脑组：Dupe 和 Creature。
        brainGroups.Add(new DupeBrainGroup());
        brainGroups.Add(new CreatureBrainGroup());
        
        // 注册大脑组件，以便在添加或移除大脑时进行处理。
        Components.Brains.Register(OnAddBrain, OnRemoveBrain);
        
        // 将当前实体添加到 CPU 预算的根节点列表中，以便管理其更新频率。
        CPUBudget.AddRoot(this);
        
        // 遍历所有大脑组，将它们添加到 CPU 预算的子节点中，并设置负载均衡阈值。
        foreach (var brainGroup in brainGroups) CPUBudget.AddChild(this, brainGroup, brainGroup.LoadBalanceThreshold());
        
        // 标记当前实体的子节点处理完成，确保所有子节点已被正确添加。
        CPUBudget.FinalizeChildren(this);
    }

    /// <summary>
    /// 当添加一个新的Brain实例时调用此方法，以将其分配到合适的脑组并配置其组件。
    /// </summary>
    /// <param name="brain">要添加的Brain实例。</param>
    private void OnAddBrain(Brain brain) {
        // 初始化一个标志变量以检查Brain是否已被分配
        var test = false;
    
        // 遍历所有脑组，寻找与Brain标签匹配的脑组
        foreach (var brainGroup in brainGroups) {
            // 如果Brain具有当前脑组的标签，则将其添加到该脑组中
            if (brain.HasTag(brainGroup.tag)) {
                brainGroup.AddBrain(brain);
                test = true;
            }
    
            // 尝试获取Brain上的Navigator组件，如果存在，则根据配置设置其异步路径探针任务的执行状态
            var component = brain.GetComponent<Navigator>();
            if (component != null) component.executePathProbeTaskAsync = isAsyncPathProbeEnabled;
        }
    
        // 断言确保每个Brain至少被分配到一个脑组
        DebugUtil.Assert(test);
    }

    /// <summary>
    /// 当移除大脑时触发的事件处理方法。
    /// </summary>
    /// <param name="brain">要移除的大脑对象。</param>
    private void OnRemoveBrain(Brain brain) {
        // 初始化一个标志变量以检查是否找到了匹配标签的大脑组
        var test = false;
        // 遍历所有大脑组，寻找匹配标签的大脑
        foreach (var brainGroup in brainGroups) {
            // 如果大脑具有当前大脑组的标签，则移除该大脑
            if (brain.HasTag(brainGroup.tag)) {
                test = true;
                brainGroup.RemoveBrain(brain);
            }
    
            // 获取大脑上的Navigator组件，如果存在，则禁用其执行路径探测任务的异步功能
            var component = brain.GetComponent<Navigator>();
            if (component != null) component.executePathProbeTaskAsync = false;
        }
    
        // 断言以确保至少找到了一个匹配标签的大脑组并进行了处理
        DebugUtil.Assert(test);
    }

    /// <summary>
    /// 根据大脑组的标签优先化特定的大脑。
    /// </summary>
    /// <param name="brain">需要被优先化的大脑对象。</param>
    public void PrioritizeBrain(Brain brain) {
        // 遍历所有的大脑组，以找到匹配标签的大脑组并优先化该大脑
        foreach (var brainGroup in brainGroups)
            // 检查当前大脑是否具有与大脑组匹配的标签
            if (brain.HasTag(brainGroup.tag))
                // 如果找到匹配的标签，则在该大脑组中优先化大脑
                brainGroup.PrioritizeBrain(brain);
    }

    /// <summary>
    /// 执行强制清理操作时调用的方法。
    /// 该方法确保在强制清理当前对象时，从CPU预算中移除对该对象的引用。
    /// </summary>
    protected override void OnForcedCleanUp() {
        // 从CPU预算中移除当前对象，以释放资源。
        CPUBudget.Remove(this);
        // 调用基类的OnForcedCleanUp方法，执行其他清理操作。
        base.OnForcedCleanUp();
    }

    /// <summary>
    /// 定义一个内部调优类，继承自TuningData<Tuning>，用于系统调优设置。
    /// </summary>
    private class Tuning : TuningData<Tuning> {
        // 禁用异步路径探测功能的开关，设置为true时将禁用异步路径探测。
        public bool disableAsyncPathProbes;
    
        // 设置每帧处理时间的上限，默认值为5毫秒。
        public readonly float frameTime = 5f;
    }

    public abstract class BrainGroup : ICPULoad {
        protected List<Brain> brains = new List<Brain>();
        public    bool        debugFreezeLoadAdustment;
        public    int         debugMaxPriorityBrainCountSeen;
        private   string      decreaseLoadLabel;
        private   string      increaseLoadLabel;
        private   int         nextPathProbeBrain;
        private   int         nextUpdateBrain;

        private readonly WorkItemCollection<Navigator.PathProbeTask, object> pathProbeJob
            = new WorkItemCollection<Navigator.PathProbeTask, object>();

        protected Queue<Brain> priorityBrains = new Queue<Brain>();

        protected BrainGroup(Tag tag) {
            this.tag   = tag;
            probeSize  = InitialProbeSize();
            probeCount = InitialProbeCount();
            var str = tag.ToString();
            increaseLoadLabel = "IncLoad" + str;
            decreaseLoadLabel = "DecLoad" + str;
        }

        public Tag tag        { get; }
        public int BrainCount => brains.Count;
        public int probeSize  { get; private set; }
        public int probeCount { get; private set; }

        public bool AdjustLoad(float currentFrameTime, float frameTimeDelta) {
            if (debugFreezeLoadAdustment) return false;

            var flag = frameTimeDelta > 0f;
            var num  = 0;
            var num2 = Math.Max(probeCount, Math.Min(brains.Count, CPUBudget.coreCount));
            num        += num2 - probeCount;
            probeCount =  num2;
            var num3 = Math.Min(1f, probeCount / (float)CPUBudget.coreCount);
            var num4 = num3             * this.probeSize;
            var num5 = num3             * this.probeSize;
            var num6 = currentFrameTime / num5;
            var num7 = frameTimeDelta   / num6;
            if (num == 0) {
                var num8 = num4 + num7 / CPUBudget.coreCount;
                var num9 = MathUtil.Clamp(MinProbeSize(), IdealProbeSize(), (int)(num8 / num3));
                num       += num9 - probeSize;
                probeSize =  num9;
            }

            if (num == 0) {
                var num10     = Math.Max(1, (int)num3 + (flag ? 1 : -1));
                var probeSize = MathUtil.Clamp(MinProbeSize(), IdealProbeSize(), (int)((num5 + num7) / num10));
                var num11     = Math.Min(brains.Count, num10 * CPUBudget.coreCount);
                num            += num11 - probeCount;
                probeCount     =  num11;
                this.probeSize =  probeSize;
            }

            if (num == 0 && flag) {
                var num12 = probeSize + ProbeSizeStep();
                num       += num12 - probeSize;
                probeSize =  num12;
            }

            if (num >= 0 && num <= 0 && brains.Count > 0) Debug.LogWarning("AdjustLoad() failed");
            return num != 0;
        }

        public abstract float GetEstimatedFrameTime();
        public          void  AddBrain(Brain brain) { brains.Add(brain); }

        public void RemoveBrain(Brain brain) {
            var num = brains.IndexOf(brain);
            if (num != -1) {
                brains.RemoveAt(num);
                OnRemoveBrain(num, ref nextUpdateBrain);
                OnRemoveBrain(num, ref nextPathProbeBrain);
            }

            if (priorityBrains.Contains(brain)) {
                var list = new List<Brain>(priorityBrains);
                list.Remove(brain);
                priorityBrains = new Queue<Brain>(list);
            }
        }

        public void PrioritizeBrain(Brain brain) {
            if (!priorityBrains.Contains(brain)) priorityBrains.Enqueue(brain);
        }

        public void ResetLoad() {
            probeSize  = InitialProbeSize();
            probeCount = InitialProbeCount();
        }

        private void IncrementBrainIndex(ref int brainIndex) {
            brainIndex++;
            if (brainIndex == brains.Count) brainIndex = 0;
        }

        private void ClampBrainIndex(ref int brainIndex) {
            brainIndex = MathUtil.Clamp(0, brains.Count - 1, brainIndex);
        }

        private void OnRemoveBrain(int removedIndex, ref int brainIndex) {
            if (removedIndex < brainIndex) {
                brainIndex--;
                return;
            }

            if (brainIndex == brains.Count) brainIndex = 0;
        }

        private void AsyncPathProbe() {
            pathProbeJob.Reset(null);
            for (var num = 0; num != brains.Count; num++) {
                ClampBrainIndex(ref nextPathProbeBrain);
                var brain = brains[nextPathProbeBrain];
                if (brain.IsRunning()) {
                    var component = brain.GetComponent<Navigator>();
                    if (component != null) {
                        component.executePathProbeTaskAsync          = true;
                        component.PathProber.potentialCellsPerUpdate = probeSize;
                        component.pathProbeTask.Update();
                        pathProbeJob.Add(component.pathProbeTask);
                        if (pathProbeJob.Count == probeCount) break;
                    }
                }

                IncrementBrainIndex(ref nextPathProbeBrain);
            }

            CPUBudget.Start(this);
            GlobalJobManager.Run(pathProbeJob);
            CPUBudget.End(this);
        }

        public void RenderEveryTick(float dt) {
            BeginBrainGroupUpdate();
            var num  = InitialProbeCount();
            var num2 = 0;
            while (num2 != brains.Count && num != 0) {
                ClampBrainIndex(ref nextUpdateBrain);
                debugMaxPriorityBrainCountSeen = Mathf.Max(debugMaxPriorityBrainCountSeen, priorityBrains.Count);
                Brain brain;
                if (AllowPriorityBrains() && priorityBrains.Count > 0)
                    brain = priorityBrains.Dequeue();
                else {
                    brain = brains[nextUpdateBrain];
                    IncrementBrainIndex(ref nextUpdateBrain);
                }

                if (brain.IsRunning()) {
                    brain.UpdateBrain();
                    num--;
                }

                num2++;
            }

            EndBrainGroupUpdate();
        }

        public void AccumulatePathProbeIterations(Dictionary<string, int> pathProbeIterations) {
            foreach (var brain in brains) {
                var component = brain.GetComponent<Navigator>();
                if (!(component == null) && !pathProbeIterations.ContainsKey(brain.name))
                    pathProbeIterations.Add(brain.name, component.PathProber.updateCount);
            }
        }

        protected abstract int   InitialProbeCount();
        protected abstract int   InitialProbeSize();
        protected abstract int   MinProbeSize();
        protected abstract int   IdealProbeSize();
        protected abstract int   ProbeSizeStep();
        public abstract    float LoadBalanceThreshold();
        public abstract    bool  AllowPriorityBrains();

        public virtual void BeginBrainGroupUpdate() {
            if (Game.BrainScheduler.isAsyncPathProbeEnabled) AsyncPathProbe();
        }

        public virtual void EndBrainGroupUpdate() { }
    }

    private class DupeBrainGroup : BrainGroup {
        private bool usePriorityBrain = true;
        public DupeBrainGroup() : base(GameTags.DupeBrain) { }
        protected override int   InitialProbeCount()     { return TuningData<Tuning>.Get().initialProbeCount; }
        protected override int   InitialProbeSize()      { return TuningData<Tuning>.Get().initialProbeSize; }
        protected override int   MinProbeSize()          { return TuningData<Tuning>.Get().minProbeSize; }
        protected override int   IdealProbeSize()        { return TuningData<Tuning>.Get().idealProbeSize; }
        protected override int   ProbeSizeStep()         { return TuningData<Tuning>.Get().probeSizeStep; }
        public override    float GetEstimatedFrameTime() { return TuningData<Tuning>.Get().estimatedFrameTime; }
        public override    float LoadBalanceThreshold()  { return TuningData<Tuning>.Get().loadBalanceThreshold; }
        public override    bool  AllowPriorityBrains()   { return usePriorityBrain; }

        public override void BeginBrainGroupUpdate() {
            base.BeginBrainGroupUpdate();
            usePriorityBrain = !usePriorityBrain;
        }

        public class Tuning : TuningData<Tuning> {
            public readonly float estimatedFrameTime   = 2f;
            public readonly int   idealProbeSize       = 1000;
            public readonly int   initialProbeCount    = 1;
            public readonly int   initialProbeSize     = 1000;
            public readonly float loadBalanceThreshold = 0.1f;
            public readonly int   minProbeSize         = 100;
            public readonly int   probeSizeStep        = 100;
        }
    }

    private class CreatureBrainGroup : BrainGroup {
        public CreatureBrainGroup() : base(GameTags.CreatureBrain) { }
        protected override int   InitialProbeCount()     { return TuningData<Tuning>.Get().initialProbeCount; }
        protected override int   InitialProbeSize()      { return TuningData<Tuning>.Get().initialProbeSize; }
        protected override int   MinProbeSize()          { return TuningData<Tuning>.Get().minProbeSize; }
        protected override int   IdealProbeSize()        { return TuningData<Tuning>.Get().idealProbeSize; }
        protected override int   ProbeSizeStep()         { return TuningData<Tuning>.Get().probeSizeStep; }
        public override    float GetEstimatedFrameTime() { return TuningData<Tuning>.Get().estimatedFrameTime; }
        public override    float LoadBalanceThreshold()  { return TuningData<Tuning>.Get().loadBalanceThreshold; }
        public override    bool  AllowPriorityBrains()   { return true; }

        public class Tuning : TuningData<Tuning> {
            public readonly float estimatedFrameTime   = 1f;
            public readonly int   idealProbeSize       = 300;
            public readonly int   initialProbeCount    = 5;
            public readonly int   initialProbeSize     = 1000;
            public readonly float loadBalanceThreshold = 0.1f;
            public readonly int   minProbeSize         = 100;
            public readonly int   probeSizeStep        = 100;
        }
    }
}