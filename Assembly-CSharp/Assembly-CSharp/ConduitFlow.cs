using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn), DebuggerDisplay("{conduitType}")]
public class ConduitFlow : IConduitFlow {
    [Flags]
    public enum FlowDirections : byte {
        None  = 0,
        Down  = 1,
        Left  = 2,
        Right = 4,
        Up    = 8,
        All   = 15
    }

    public const  float MAX_LIQUID_MASS                          = 10f;
    public const  float MAX_GAS_MASS                             = 1f;
    private const float PERCENT_MAX_MASS_FOR_STATE_CHANGE_DAMAGE = 0.1f;
    public const  float TickRate                                 = 1f;
    public const  float WaitTime                                 = 1f;
    private const int   FLOW_DIRECTION_COUNT                     = 4;

    private readonly WorkItemCollection<BuildNetworkTask, ConduitFlow> build_network_job
        = new WorkItemCollection<BuildNetworkTask, ConduitFlow>();

    public           ConduitType          conduitType;
    private readonly List<ConduitUpdater> conduitUpdaters = new List<ConduitUpdater>();

    private readonly WorkItemCollection<ConnectTask, ConnectContext> connect_job
        = new WorkItemCollection<ConnectTask, ConnectContext>();

    private          bool               dirtyConduitUpdaters;
    private          float              elapsedTime;
    private          GridNode[]         grid;
    private          float              lastUpdateTime = float.NegativeInfinity;
    private readonly float              MaxMass        = 10f;
    private readonly IUtilityNetworkMgr networkMgr;
    private readonly List<Network>      networks     = new List<Network>();
    private readonly HashSet<int>       replacements = new HashSet<int>();

    [Serialize]
    public ConduitContents[] serializedContents;

    [Serialize]
    public int[] serializedIdx;

    public SOAInfo soaInfo = new SOAInfo();

    private readonly WorkItemCollection<UpdateNetworkTask, ConduitFlow> update_networks_job
        = new WorkItemCollection<UpdateNetworkTask, ConduitFlow>();

    [Serialize]
    public SerializedContents[] versionedSerializedContents;

    public ConduitFlow(ConduitType        conduit_type,
                       int                num_cells,
                       IUtilityNetworkMgr network_mgr,
                       float              max_conduit_mass,
                       float              initial_elapsed_time) {
        elapsedTime = initial_elapsed_time;
        conduitType = conduit_type;
        networkMgr  = network_mgr;
        MaxMass     = max_conduit_mass;
        Initialize(num_cells);
        network_mgr.AddNetworksRebuiltListener(OnUtilityNetworksRebuilt);
    }

    public float ContinuousLerpPercent => Mathf.Clamp01((Time.time - lastUpdateTime) / 1f);
    public float DiscreteLerpPercent   => Mathf.Clamp01(elapsedTime                  / 1f);

    public void AddConduitUpdater(Action<float> callback, ConduitFlowPriority priority = ConduitFlowPriority.Default) {
        conduitUpdaters.Add(new ConduitUpdater { priority = priority, callback = callback });
        dirtyConduitUpdaters = true;
    }

    public void RemoveConduitUpdater(Action<float> callback) {
        for (var i = 0; i < conduitUpdaters.Count; i++)
            if (conduitUpdaters[i].callback == callback) {
                conduitUpdaters.RemoveAt(i);
                dirtyConduitUpdaters = true;
                return;
            }
    }

    public bool IsConduitEmpty(int cell_idx) {
        var contents = grid[cell_idx].contents;
        return contents.mass <= 0f;
    }

    public event System.Action    onConduitsRebuilt;
    private static FlowDirections ComputeFlowDirection(int index) { return (FlowDirections)(1 << index); }

    private static FlowDirections ComputeNextFlowDirection(FlowDirections current) {
        switch (current) {
            case FlowDirections.None:
            case FlowDirections.Up:
                return FlowDirections.Down;
            case FlowDirections.Down:
                return FlowDirections.Left;
            case FlowDirections.Left:
                return FlowDirections.Right;
            case FlowDirections.Right:
                return FlowDirections.Up;
        }

        Debug.Assert(false, "multiple bits are set in 'FlowDirections'...can't compute next direction");
        return FlowDirections.Down;
    }

    public static FlowDirections Invert(FlowDirections directions) { return FlowDirections.All & ~directions; }

    public static FlowDirections Opposite(FlowDirections directions) {
        var result = FlowDirections.None;
        if ((directions & FlowDirections.Left) != FlowDirections.None)
            result = FlowDirections.Right;
        else if ((directions & FlowDirections.Right) != FlowDirections.None)
            result = FlowDirections.Left;
        else if ((directions & FlowDirections.Up) != FlowDirections.None)
            result                                                                 = FlowDirections.Down;
        else if ((directions & FlowDirections.Down) != FlowDirections.None) result = FlowDirections.Up;

        return result;
    }

    public void Initialize(int num_cells) {
        grid = new GridNode[num_cells];
        for (var i = 0; i < num_cells; i++) {
            grid[i].conduitIdx          = -1;
            grid[i].contents.element    = SimHashes.Vacuum;
            grid[i].contents.diseaseIdx = byte.MaxValue;
        }
    }

    private void OnUtilityNetworksRebuilt(IList<UtilityNetwork> networks, ICollection<int> root_nodes) {
        RebuildConnections(root_nodes);
        var count = this.networks.Count - networks.Count;
        if (0 < this.networks.Count - networks.Count) this.networks.RemoveRange(networks.Count, count);
        Debug.Assert(this.networks.Count <= networks.Count);
        for (var num = 0; num != networks.Count; num++)
            if (num < this.networks.Count) {
                this.networks[num] = new Network {
                    network = (FlowUtilityNetwork)networks[num], cells = this.networks[num].cells
                };

                this.networks[num].cells.Clear();
            } else
                this.networks.Add(new Network { network = (FlowUtilityNetwork)networks[num], cells = new List<int>() });

        build_network_job.Reset(this);
        foreach (var network in this.networks) build_network_job.Add(new BuildNetworkTask(network, soaInfo.NumEntries));
        GlobalJobManager.Run(build_network_job);
        for (var num2 = 0; num2 != build_network_job.Count; num2++) build_network_job.GetWorkItem(num2).Finish();
    }

    private void RebuildConnections(IEnumerable<int> root_nodes) {
        var connectContext = new ConnectContext(this);
        soaInfo.Clear(this);
        replacements.ExceptWith(root_nodes);
        var layer = conduitType == ConduitType.Gas ? ObjectLayer.GasConduit : ObjectLayer.LiquidConduit;
        foreach (var num in root_nodes) {
            var gameObject = Grid.Objects[num, (int)layer];
            if (!(gameObject == null)) {
                var component = gameObject.GetComponent<global::Conduit>();
                if (!(component != null) || !component.IsDisconnected()) {
                    var conduitIdx = soaInfo.AddConduit(this, gameObject, num);
                    grid[num].conduitIdx = conduitIdx;
                    connectContext.cells.Add(num);
                }
            }
        }

        Game.Instance.conduitTemperatureManager.Sim200ms(0f);
        connect_job.Reset(connectContext);
        var num2 = 256;
        for (var i = 0; i < connectContext.cells.Count; i += num2)
            connect_job.Add(new ConnectTask(i, Mathf.Min(i + num2, connectContext.cells.Count)));

        GlobalJobManager.Run(connect_job);
        connectContext.Finish();
        if (onConduitsRebuilt != null) onConduitsRebuilt();
    }

    private FlowDirections GetDirection(Conduit conduit, Conduit target_conduit) {
        Debug.Assert(conduit.idx        != -1);
        Debug.Assert(target_conduit.idx != -1);
        var conduitConnections = soaInfo.GetConduitConnections(conduit.idx);
        if (conduitConnections.up == target_conduit.idx) return FlowDirections.Up;

        if (conduitConnections.down == target_conduit.idx) return FlowDirections.Down;

        if (conduitConnections.left == target_conduit.idx) return FlowDirections.Left;

        if (conduitConnections.right == target_conduit.idx) return FlowDirections.Right;

        return FlowDirections.None;
    }

    public int ComputeUpdateOrder(int cell) {
        foreach (var network in networks) {
            var num = network.cells.IndexOf(cell);
            if (num != -1) return num;
        }

        return -1;
    }

    public ConduitContents GetContents(int cell) {
        var contents                            = grid[cell].contents;
        var gridNode                            = grid[cell];
        if (gridNode.conduitIdx != -1) contents = soaInfo.GetConduit(gridNode.conduitIdx).GetContents(this);
        if (contents.mass > 0f && contents.temperature <= 0f)
            Debug.LogError(string.Format("unexpected temperature {0}", contents.temperature));

        return contents;
    }

    public void SetContents(int cell, ConduitContents contents) {
        var gridNode = grid[cell];
        if (gridNode.conduitIdx != -1) {
            soaInfo.GetConduit(gridNode.conduitIdx).SetContents(this, contents);
            return;
        }

        grid[cell].contents = contents;
    }

    public static int GetCellFromDirection(int cell, FlowDirections direction) {
        switch (direction) {
            case FlowDirections.Down:
                return Grid.CellBelow(cell);
            case FlowDirections.Left:
                return Grid.CellLeft(cell);
            case FlowDirections.Down | FlowDirections.Left:
                break;
            case FlowDirections.Right:
                return Grid.CellRight(cell);
            default:
                if (direction == FlowDirections.Up) return Grid.CellAbove(cell);

                break;
        }

        return -1;
    }

    public void Sim200ms(float dt) {
        if (dt <= 0f) return;

        elapsedTime += dt;
        if (elapsedTime < 1f) return;

        elapsedTime -= 1f;
        var obj = 1f;
        lastUpdateTime = Time.time;
        soaInfo.BeginFrame(this);
        var pooledList = ListPool<UpdateNetworkTask, ConduitFlow>.Allocate();
        pooledList.Capacity = Mathf.Max(pooledList.Capacity, networks.Count);
        foreach (var network in networks) pooledList.Add(new UpdateNetworkTask(network));
        var num = 0;
        while (num != 4 && pooledList.Count != 0) {
            update_networks_job.Reset(this);
            foreach (var work_item in pooledList) update_networks_job.Add(work_item);
            GlobalJobManager.Run(update_networks_job);
            pooledList.Clear();
            for (var num2 = 0; num2 != update_networks_job.Count; num2++) {
                var workItem = update_networks_job.GetWorkItem(num2);
                if (workItem.continue_updating && num != 3)
                    pooledList.Add(workItem);
                else
                    workItem.Finish(this);
            }

            num++;
        }

        pooledList.Recycle();
        if (dirtyConduitUpdaters) conduitUpdaters.Sort((a, b) => a.priority - b.priority);
        soaInfo.EndFrame(this);
        for (var i = 0; i < conduitUpdaters.Count; i++) conduitUpdaters[i].callback(obj);
    }

    private float ComputeMovableMass(GridNode grid_node) {
        var contents = grid_node.contents;
        if (contents.element == SimHashes.Vacuum) return 0f;

        return contents.movable_mass;
    }

    private bool UpdateConduit(Conduit conduit) {
        var result                  = false;
        var cell                    = soaInfo.GetCell(conduit.idx);
        var gridNode                = grid[cell];
        var num                     = ComputeMovableMass(gridNode);
        var permittedFlowDirections = soaInfo.GetPermittedFlowDirections(conduit.idx);
        var flowDirections          = soaInfo.GetTargetFlowDirection(conduit.idx);
        if (num <= 0f)
            for (var num2 = 0; num2 != 4; num2++) {
                flowDirections = ComputeNextFlowDirection(flowDirections);
                if ((permittedFlowDirections & flowDirections) != FlowDirections.None) {
                    var conduitFromDirection = soaInfo.GetConduitFromDirection(conduit.idx, flowDirections);
                    Debug.Assert(conduitFromDirection.idx != -1);
                    if ((soaInfo.GetSrcFlowDirection(conduitFromDirection.idx) & Opposite(flowDirections)) >
                        FlowDirections.None)
                        soaInfo.SetPullDirection(conduitFromDirection.idx, flowDirections);
                }
            }
        else
            for (var num3 = 0; num3 != 4; num3++) {
                flowDirections = ComputeNextFlowDirection(flowDirections);
                if ((permittedFlowDirections & flowDirections) != FlowDirections.None) {
                    var conduitFromDirection2 = soaInfo.GetConduitFromDirection(conduit.idx, flowDirections);
                    Debug.Assert(conduitFromDirection2.idx != -1);
                    var srcFlowDirection = soaInfo.GetSrcFlowDirection(conduitFromDirection2.idx);
                    var flag             = (srcFlowDirection & Opposite(flowDirections)) > FlowDirections.None;
                    if (srcFlowDirection != FlowDirections.None && !flag)
                        result = true;
                    else {
                        var cell2 = soaInfo.GetCell(conduitFromDirection2.idx);
                        Debug.Assert(cell2 != -1);
                        var contents = grid[cell2].contents;
                        var flag2 = contents.element == SimHashes.Vacuum ||
                                    contents.element == gridNode.contents.element;

                        var effectiveCapacity = contents.GetEffectiveCapacity(MaxMass);
                        var flag3             = flag2 && effectiveCapacity > 0f;
                        var num4              = Mathf.Min(num, effectiveCapacity);
                        if (flag && flag3) soaInfo.SetPullDirection(conduitFromDirection2.idx, flowDirections);
                        if (num4 > 0f && flag3) {
                            soaInfo.SetTargetFlowDirection(conduit.idx, flowDirections);
                            Debug.Assert(gridNode.contents.temperature > 0f);
                            contents.temperature
                                = GameUtil.GetFinalTemperature(gridNode.contents.temperature,
                                                               num4,
                                                               contents.temperature,
                                                               contents.mass);

                            contents.AddMass(num4);
                            contents.element = gridNode.contents.element;
                            var num5 = (int)(num4 / gridNode.contents.mass * gridNode.contents.diseaseCount);
                            if (num5 != 0) {
                                var diseaseInfo
                                    = SimUtil.CalculateFinalDiseaseInfo(gridNode.contents.diseaseIdx,
                                                                        num5,
                                                                        contents.diseaseIdx,
                                                                        contents.diseaseCount);

                                contents.diseaseIdx   = diseaseInfo.idx;
                                contents.diseaseCount = diseaseInfo.count;
                            }

                            grid[cell2].contents = contents;
                            Debug.Assert(num4 <= gridNode.contents.mass);
                            var num6 = gridNode.contents.mass - num4;
                            num -= num4;
                            if (num6 <= 0f) {
                                Debug.Assert(num <= 0f);
                                soaInfo.SetLastFlowInfo(conduit.idx, flowDirections, ref gridNode.contents);
                                gridNode.contents = ConduitContents.Empty;
                            } else {
                                var num7 = (int)(num6 / gridNode.contents.mass * gridNode.contents.diseaseCount);
                                Debug.Assert(num7 >= 0);
                                var contents2 = gridNode.contents;
                                contents2.RemoveMass(num6);
                                contents2.diseaseCount -= num7;
                                gridNode.contents.RemoveMass(num4);
                                gridNode.contents.diseaseCount = num7;
                                if (num7 == 0) gridNode.contents.diseaseIdx = byte.MaxValue;
                                soaInfo.SetLastFlowInfo(conduit.idx, flowDirections, ref contents2);
                            }

                            grid[cell].contents = gridNode.contents;
                            result              = 0f < ComputeMovableMass(gridNode);
                            break;
                        }
                    }
                }
            }

        var srcFlowDirection2 = soaInfo.GetSrcFlowDirection(conduit.idx);
        var pullDirection     = soaInfo.GetPullDirection(conduit.idx);
        if (srcFlowDirection2                             == FlowDirections.None ||
            (Opposite(srcFlowDirection2) & pullDirection) != FlowDirections.None) {
            soaInfo.SetPullDirection(conduit.idx, FlowDirections.None);
            soaInfo.SetSrcFlowDirection(conduit.idx, FlowDirections.None);
            for (var num8 = 0; num8 != 2; num8++) {
                var flowDirections2 = srcFlowDirection2;
                for (var num9 = 0; num9 != 4; num9++) {
                    flowDirections2 = ComputeNextFlowDirection(flowDirections2);
                    var conduitFromDirection3 = soaInfo.GetConduitFromDirection(conduit.idx, flowDirections2);
                    if (conduitFromDirection3.idx != -1 &&
                        (soaInfo.GetPermittedFlowDirections(conduitFromDirection3.idx) & Opposite(flowDirections2)) !=
                        FlowDirections.None) {
                        var cell3     = soaInfo.GetCell(conduitFromDirection3.idx);
                        var contents3 = grid[cell3].contents;
                        var num10     = num8 == 0 ? contents3.movable_mass : contents3.mass;
                        if (0f < num10) {
                            soaInfo.SetSrcFlowDirection(conduit.idx, flowDirections2);
                            break;
                        }
                    }
                }

                if (soaInfo.GetSrcFlowDirection(conduit.idx) != FlowDirections.None) break;
            }
        }

        return result;
    }

    public float GetAmountAllowedForMerging(ConduitContents from, ConduitContents to, float massDesiredtoBeMoved) {
        return Mathf.Min(massDesiredtoBeMoved, MaxMass - to.mass);
    }

    public bool CanMergeContents(ConduitContents from, ConduitContents to, float massToMove) {
        return (from.element == to.element || to.element == SimHashes.Vacuum || massToMove <= 0f) &&
               GetAmountAllowedForMerging(from, to, massToMove) > 0f;
    }

    public float AddElement(int       cell_idx,
                            SimHashes element,
                            float     mass,
                            float     temperature,
                            byte      disease_idx,
                            int       disease_count) {
        if (grid[cell_idx].conduitIdx == -1) return 0f;

        var contents = GetConduit(cell_idx).GetContents(this);
        if (contents.element != element && contents.element != SimHashes.Vacuum && mass > 0f) return 0f;

        var num  = Mathf.Min(mass, MaxMass - contents.mass);
        var num2 = num / mass;
        if (num <= 0f) return 0f;

        contents.temperature = GameUtil.GetFinalTemperature(temperature, num, contents.temperature, contents.mass);
        contents.AddMass(num);
        contents.element = element;
        contents.ConsolidateMass();
        var num3 = (int)(num2 * disease_count);
        if (num3 > 0) {
            var diseaseInfo
                = SimUtil.CalculateFinalDiseaseInfo(disease_idx, num3, contents.diseaseIdx, contents.diseaseCount);

            contents.diseaseIdx   = diseaseInfo.idx;
            contents.diseaseCount = diseaseInfo.count;
        }

        SetContents(cell_idx, contents);
        return num;
    }

    public ConduitContents RemoveElement(int cell, float delta) {
        var conduit = GetConduit(cell);
        if (conduit.idx == -1) return ConduitContents.Empty;

        return RemoveElement(conduit, delta);
    }

    public ConduitContents RemoveElement(Conduit conduit, float delta) {
        var contents = conduit.GetContents(this);
        var num      = Mathf.Min(contents.mass, delta);
        var num2     = contents.mass - num;
        if (num2 <= 0f) {
            conduit.SetContents(this, ConduitContents.Empty);
            return contents;
        }

        var result = contents;
        result.RemoveMass(num2);
        var num3 = (int)(num2 / contents.mass * contents.diseaseCount);
        result.diseaseCount = contents.diseaseCount - num3;
        var contents2 = contents;
        contents2.RemoveMass(num);
        contents2.diseaseCount = num3;
        if (num3 <= 0) {
            contents2.diseaseIdx   = byte.MaxValue;
            contents2.diseaseCount = 0;
        }

        conduit.SetContents(this, contents2);
        return result;
    }

    public FlowDirections GetPermittedFlow(int cell) {
        var conduit = GetConduit(cell);
        if (conduit.idx == -1) return FlowDirections.None;

        return soaInfo.GetPermittedFlowDirections(conduit.idx);
    }

    public bool HasConduit(int cell) { return grid[cell].conduitIdx != -1; }

    public Conduit GetConduit(int cell) {
        var conduitIdx = grid[cell].conduitIdx;
        if (conduitIdx == -1) return Conduit.Invalid;

        return soaInfo.GetConduit(conduitIdx);
    }

    private void DumpPipeContents(int cell, ConduitContents contents) {
        if (contents.element != SimHashes.Vacuum && contents.mass > 0f) {
            SimMessages.AddRemoveSubstance(cell,
                                           contents.element,
                                           CellEventLogger.Instance.ConduitFlowEmptyConduit,
                                           contents.mass,
                                           contents.temperature,
                                           contents.diseaseIdx,
                                           contents.diseaseCount);

            SetContents(cell, ConduitContents.Empty);
        }
    }

    public void EmptyConduit(int cell) {
        if (replacements.Contains(cell)) return;

        DumpPipeContents(cell, grid[cell].contents);
    }

    public void MarkForReplacement(int cell) { replacements.Add(cell); }

    public void DeactivateCell(int cell) {
        grid[cell].conduitIdx = -1;
        SetContents(cell, ConduitContents.Empty);
    }

    [Conditional("CHECK_NAN")]
    private void Validate(ConduitContents contents) {
        if (contents.mass > 0f && contents.temperature <= 0f) Debug.LogError("zero degree pipe contents");
    }

    [OnSerializing]
    private void OnSerializing() {
        var numEntries = soaInfo.NumEntries;
        if (numEntries > 0) {
            versionedSerializedContents = new SerializedContents[numEntries];
            serializedIdx               = new int[numEntries];
            for (var i = 0; i < numEntries; i++) {
                var conduit  = soaInfo.GetConduit(i);
                var contents = conduit.GetContents(this);
                serializedIdx[i]               = soaInfo.GetCell(conduit.idx);
                versionedSerializedContents[i] = new SerializedContents(contents);
            }

            return;
        }

        serializedContents          = null;
        versionedSerializedContents = null;
        serializedIdx               = null;
    }

    [OnSerialized]
    private void OnSerialized() {
        versionedSerializedContents = null;
        serializedContents          = null;
        serializedIdx               = null;
    }

    [OnDeserialized]
    private void OnDeserialized() {
        if (this.serializedContents != null) {
            versionedSerializedContents = new SerializedContents[serializedContents.Length];
            for (var i = 0; i < serializedContents.Length; i++)
                versionedSerializedContents[i] = new SerializedContents(serializedContents[i]);

            serializedContents = null;
        }

        if (versionedSerializedContents == null) return;

        for (var j = 0; j < versionedSerializedContents.Length; j++) {
            var num                = serializedIdx[j];
            var serializedContents = versionedSerializedContents[j];
            var conduitContents = serializedContents.mass <= 0f
                                      ? ConduitContents.Empty
                                      : new ConduitContents(serializedContents.element,
                                                            Math.Min(MaxMass, serializedContents.mass),
                                                            serializedContents.temperature,
                                                            byte.MaxValue,
                                                            0);

            if (0 < serializedContents.diseaseCount || serializedContents.diseaseHash != 0) {
                conduitContents.diseaseIdx = Db.Get().Diseases.GetIndex(serializedContents.diseaseHash);
                conduitContents.diseaseCount
                    = conduitContents.diseaseIdx == byte.MaxValue ? 0 : serializedContents.diseaseCount;
            }

            if (float.IsNaN(conduitContents.temperature)                                           ||
                (conduitContents.temperature <= 0f && conduitContents.element != SimHashes.Vacuum) ||
                10000f < conduitContents.temperature) {
                var vector2I = Grid.CellToXY(num);
                DeserializeWarnings.Instance.PipeContentsTemperatureIsNan
                                   .Warn(string
                                             .Format("Invalid pipe content temperature of {0} detected. Resetting temperature. (x={1}, y={2}, cell={3})",
                                                     conduitContents.temperature,
                                                     vector2I.x,
                                                     vector2I.y,
                                                     num));

                conduitContents.temperature
                    = ElementLoader.FindElementByHash(conduitContents.element).defaultValues.temperature;
            }

            SetContents(num, conduitContents);
        }

        versionedSerializedContents = null;
        this.serializedContents     = null;
        serializedIdx               = null;
    }

    public UtilityNetwork GetNetwork(Conduit conduit) {
        var cell = soaInfo.GetCell(conduit.idx);
        return networkMgr.GetNetworkForCell(cell);
    }

    public void ForceRebuildNetworks() { networkMgr.ForceRebuildNetworks(); }

    public bool IsConduitFull(int cell_idx) {
        var contents = grid[cell_idx].contents;
        return MaxMass - contents.mass <= 0f;
    }

    public void FreezeConduitContents(int conduit_idx) {
        var conduitGO = soaInfo.GetConduitGO(conduit_idx);
        if (conduitGO != null && soaInfo.GetConduit(conduit_idx).GetContents(this).mass > MaxMass * 0.1f)
            conduitGO.Trigger(-700727624);
    }

    public void MeltConduitContents(int conduit_idx) {
        var conduitGO = soaInfo.GetConduitGO(conduit_idx);
        if (conduitGO != null && soaInfo.GetConduit(conduit_idx).GetContents(this).mass > MaxMass * 0.1f)
            conduitGO.Trigger(-1152799878);
    }

    [DebuggerDisplay("{NumEntries}")]
    public class SOAInfo {
        private readonly ConduitJob beginFrameJob = new ConduitJob();
        private readonly List<int>  cells         = new List<int>();
        private readonly ConduitJob clearJob      = new ConduitJob();

        private readonly ConduitTaskDivision<ClearPermanentDiseaseContainer> clearPermanentDiseaseContainer
            = new ConduitTaskDivision<ClearPermanentDiseaseContainer>();

        private readonly List<ConduitConnections>       conduitConnections     = new List<ConduitConnections>();
        private readonly List<GameObject>               conduitGOs             = new List<GameObject>();
        private readonly List<Conduit>                  conduits               = new List<Conduit>();
        private readonly List<bool>                     diseaseContentsVisible = new List<bool>();
        private readonly List<HandleVector<int>.Handle> diseaseHandles         = new List<HandleVector<int>.Handle>();
        private readonly ConduitJob                     endFrameJob            = new ConduitJob();

        private readonly ConduitTaskDivision<FlowThroughVacuum> flowThroughVacuum
            = new ConduitTaskDivision<FlowThroughVacuum>();

        private readonly List<ConduitContents> initialContents = new List<ConduitContents>();

        private readonly ConduitTaskDivision<InitializeContentsTask> initializeContents
            = new ConduitTaskDivision<InitializeContentsTask>();

        private readonly ConduitTaskDivision<InvalidateLastFlow> invalidateLastFlow
            = new ConduitTaskDivision<InvalidateLastFlow>();

        private readonly List<ConduitFlowInfo> lastFlowInfo            = new List<ConduitFlowInfo>();
        private readonly List<FlowDirections>  permittedFlowDirections = new List<FlowDirections>();

        private readonly ConduitTaskDivision<PublishDiseaseToGame> publishDiseaseToGame
            = new ConduitTaskDivision<PublishDiseaseToGame>();

        private readonly ConduitTaskDivision<PublishDiseaseToSim> publishDiseaseToSim
            = new ConduitTaskDivision<PublishDiseaseToSim>();

        private readonly ConduitTaskDivision<PublishTemperatureToGame> publishTemperatureToGame
            = new ConduitTaskDivision<PublishTemperatureToGame>();

        private readonly ConduitTaskDivision<PublishTemperatureToSim> publishTemperatureToSim
            = new ConduitTaskDivision<PublishTemperatureToSim>();

        private readonly List<FlowDirections>              pullDirections    = new List<FlowDirections>();
        private readonly ConduitTaskDivision<ResetConduit> resetConduit      = new ConduitTaskDivision<ResetConduit>();
        private readonly List<FlowDirections>              srcFlowDirections = new List<FlowDirections>();

        private readonly List<HandleVector<int>.Handle> structureTemperatureHandles
            = new List<HandleVector<int>.Handle>();

        private readonly List<FlowDirections>           targetFlowDirections   = new List<FlowDirections>();
        private readonly List<HandleVector<int>.Handle> temperatureHandles     = new List<HandleVector<int>.Handle>();
        private readonly ConduitJob                     updateFlowDirectionJob = new ConduitJob();
        public           int                            NumEntries => conduits.Count;

        public int AddConduit(ConduitFlow manager, GameObject conduit_go, int cell) {
            var count = conduitConnections.Count;
            var item  = new Conduit(count);
            conduits.Add(item);
            conduitConnections.Add(new ConduitConnections { left = -1, right = -1, up = -1, down = -1 });
            var contents = manager.grid[cell].contents;
            initialContents.Add(contents);
            lastFlowInfo.Add(ConduitFlowInfo.DEFAULT);
            var handle = GameComps.StructureTemperatures.GetHandle(conduit_go);
            var handle2 = Game.Instance.conduitTemperatureManager.Allocate(manager.conduitType,
                                                                           count,
                                                                           handle,
                                                                           ref contents);

            var item2 = Game.Instance.conduitDiseaseManager.Allocate(handle2, ref contents);
            cells.Add(cell);
            diseaseContentsVisible.Add(false);
            structureTemperatureHandles.Add(handle);
            temperatureHandles.Add(handle2);
            diseaseHandles.Add(item2);
            conduitGOs.Add(conduit_go);
            permittedFlowDirections.Add(FlowDirections.None);
            srcFlowDirections.Add(FlowDirections.None);
            pullDirections.Add(FlowDirections.None);
            targetFlowDirections.Add(FlowDirections.None);
            return count;
        }

        public void Clear(ConduitFlow manager) {
            if (clearJob.Count == 0) {
                clearJob.Reset(this);
                clearJob.Add(publishTemperatureToSim);
                clearJob.Add(publishDiseaseToSim);
                clearJob.Add(resetConduit);
            }

            clearPermanentDiseaseContainer.Initialize(conduits.Count, manager);
            publishTemperatureToSim.Initialize(conduits.Count, manager);
            publishDiseaseToSim.Initialize(conduits.Count, manager);
            resetConduit.Initialize(conduits.Count, manager);
            clearPermanentDiseaseContainer.Run(this);
            GlobalJobManager.Run(clearJob);
            for (var num = 0; num != conduits.Count; num++)
                Game.Instance.conduitDiseaseManager.Free(diseaseHandles[num]);

            for (var num2 = 0; num2 != conduits.Count; num2++)
                Game.Instance.conduitTemperatureManager.Free(temperatureHandles[num2]);

            cells.Clear();
            diseaseContentsVisible.Clear();
            permittedFlowDirections.Clear();
            srcFlowDirections.Clear();
            pullDirections.Clear();
            targetFlowDirections.Clear();
            conduitGOs.Clear();
            diseaseHandles.Clear();
            temperatureHandles.Clear();
            structureTemperatureHandles.Clear();
            initialContents.Clear();
            lastFlowInfo.Clear();
            conduitConnections.Clear();
            conduits.Clear();
        }

        public Conduit GetConduit(int idx) { return conduits[idx]; }
        public ConduitConnections GetConduitConnections(int idx) { return conduitConnections[idx]; }
        public void SetConduitConnections(int idx, ConduitConnections data) { conduitConnections[idx] = data; }

        public float GetConduitTemperature(int idx) {
            var handle      = temperatureHandles[idx];
            var temperature = Game.Instance.conduitTemperatureManager.GetTemperature(handle);
            Debug.Assert(!float.IsNaN(temperature));
            return temperature;
        }

        public void SetConduitTemperatureData(int idx, ref ConduitContents contents) {
            var handle = temperatureHandles[idx];
            Game.Instance.conduitTemperatureManager.SetData(handle, ref contents);
        }

        public ConduitDiseaseManager.Data GetDiseaseData(int idx) {
            var handle = diseaseHandles[idx];
            return Game.Instance.conduitDiseaseManager.GetData(handle);
        }

        public void SetDiseaseData(int idx, ref ConduitContents contents) {
            var handle = diseaseHandles[idx];
            Game.Instance.conduitDiseaseManager.SetData(handle, ref contents);
        }

        public GameObject GetConduitGO(int idx) { return conduitGOs[idx]; }

        public void ForcePermanentDiseaseContainer(int idx, bool force_on) {
            if (diseaseContentsVisible[idx] != force_on) {
                diseaseContentsVisible[idx] = force_on;
                var gameObject = conduitGOs[idx];
                if (gameObject == null) return;

                gameObject.GetComponent<PrimaryElement>().ForcePermanentDiseaseContainer(force_on);
            }
        }

        public Conduit GetConduitFromDirection(int idx, FlowDirections direction) {
            var conduitConnections = this.conduitConnections[idx];
            switch (direction) {
                case FlowDirections.Down:
                    if (conduitConnections.down == -1) return Conduit.Invalid;

                    return conduits[conduitConnections.down];
                case FlowDirections.Left:
                    if (conduitConnections.left == -1) return Conduit.Invalid;

                    return conduits[conduitConnections.left];
                case FlowDirections.Down | FlowDirections.Left:
                    break;
                case FlowDirections.Right:
                    if (conduitConnections.right == -1) return Conduit.Invalid;

                    return conduits[conduitConnections.right];
                default:
                    if (direction == FlowDirections.Up) {
                        if (conduitConnections.up == -1) return Conduit.Invalid;

                        return conduits[conduitConnections.up];
                    }

                    break;
            }

            return Conduit.Invalid;
        }

        public void BeginFrame(ConduitFlow manager) {
            if (beginFrameJob.Count == 0) {
                beginFrameJob.Reset(this);
                beginFrameJob.Add(initializeContents);
                beginFrameJob.Add(invalidateLastFlow);
            }

            initializeContents.Initialize(conduits.Count, manager);
            invalidateLastFlow.Initialize(conduits.Count, manager);
            GlobalJobManager.Run(beginFrameJob);
        }

        public void EndFrame(ConduitFlow manager) {
            if (endFrameJob.Count == 0) {
                endFrameJob.Reset(this);
                endFrameJob.Add(publishDiseaseToGame);
            }

            publishTemperatureToGame.Initialize(conduits.Count, manager);
            publishDiseaseToGame.Initialize(conduits.Count, manager);
            publishTemperatureToGame.Run(this);
            GlobalJobManager.Run(endFrameJob);
        }

        public void UpdateFlowDirection(ConduitFlow manager) {
            if (updateFlowDirectionJob.Count == 0) {
                updateFlowDirectionJob.Reset(this);
                updateFlowDirectionJob.Add(flowThroughVacuum);
            }

            flowThroughVacuum.Initialize(conduits.Count, manager);
            GlobalJobManager.Run(updateFlowDirectionJob);
        }

        public void ResetLastFlowInfo(int idx) { lastFlowInfo[idx] = ConduitFlowInfo.DEFAULT; }

        public void SetLastFlowInfo(int idx, FlowDirections direction, ref ConduitContents contents) {
            if (lastFlowInfo[idx].direction == FlowDirections.None)
                lastFlowInfo[idx] = new ConduitFlowInfo { direction = direction, contents = contents };
        }

        public ConduitContents GetInitialContents(int         idx) { return initialContents[idx]; }
        public ConduitFlowInfo GetLastFlowInfo(int            idx) { return lastFlowInfo[idx]; }
        public FlowDirections  GetPermittedFlowDirections(int idx) { return permittedFlowDirections[idx]; }

        public void SetPermittedFlowDirections(int idx, FlowDirections permitted) {
            permittedFlowDirections[idx] = permitted;
        }

        public FlowDirections AddPermittedFlowDirections(int idx, FlowDirections delta) {
            var list = permittedFlowDirections;
            return list[idx] |= delta;
        }

        public FlowDirections RemovePermittedFlowDirections(int idx, FlowDirections delta) {
            var list = permittedFlowDirections;
            return list[idx] &= ~delta;
        }

        public FlowDirections GetTargetFlowDirection(int idx) { return targetFlowDirections[idx]; }

        public void SetTargetFlowDirection(int idx, FlowDirections directions) {
            targetFlowDirections[idx] = directions;
        }

        public FlowDirections GetSrcFlowDirection(int idx) { return srcFlowDirections[idx]; }
        public void SetSrcFlowDirection(int idx, FlowDirections directions) { srcFlowDirections[idx] = directions; }
        public FlowDirections GetPullDirection(int idx) { return pullDirections[idx]; }
        public void SetPullDirection(int idx, FlowDirections directions) { pullDirections[idx] = directions; }
        public int GetCell(int idx) { return cells[idx]; }
        public void SetCell(int idx, int cell) { cells[idx] = cell; }

        private abstract class ConduitTask : DivisibleTask<SOAInfo> {
            public ConduitFlow manager;
            public ConduitTask(string name) : base(name) { }
        }

        private class ConduitTaskDivision<Task> : TaskDivision<Task, SOAInfo> where Task : ConduitTask, new() {
            public void Initialize(int conduitCount, ConduitFlow manager) {
                base.Initialize(conduitCount);
                var tasks                                               = this.tasks;
                for (var i = 0; i < tasks.Length; i++) tasks[i].manager = manager;
            }
        }

        private class ConduitJob : WorkItemCollection<ConduitTask, SOAInfo> {
            public void Add<Task>(ConduitTaskDivision<Task> taskDivision) where Task : ConduitTask, new() {
                foreach (var task in taskDivision.tasks) base.Add(task);
            }
        }

        private class ClearPermanentDiseaseContainer : ConduitTask {
            public ClearPermanentDiseaseContainer() : base("ClearPermanentDiseaseContainer") { }

            protected override void RunDivision(SOAInfo soaInfo) {
                for (var num = start; num != end; num++) soaInfo.ForcePermanentDiseaseContainer(num, false);
            }
        }

        private class PublishTemperatureToSim : ConduitTask {
            public PublishTemperatureToSim() : base("PublishTemperatureToSim") { }

            protected override void RunDivision(SOAInfo soaInfo) {
                for (var num = start; num != end; num++) {
                    var handle = soaInfo.temperatureHandles[num];
                    if (handle.IsValid()) {
                        var temperature = Game.Instance.conduitTemperatureManager.GetTemperature(handle);
                        manager.grid[soaInfo.cells[num]].contents.temperature = temperature;
                    }
                }
            }
        }

        private class PublishDiseaseToSim : ConduitTask {
            public PublishDiseaseToSim() : base("PublishDiseaseToSim") { }

            protected override void RunDivision(SOAInfo soaInfo) {
                for (var num = start; num != end; num++) {
                    var handle = soaInfo.diseaseHandles[num];
                    if (handle.IsValid()) {
                        var data = Game.Instance.conduitDiseaseManager.GetData(handle);
                        var num2 = soaInfo.cells[num];
                        manager.grid[num2].contents.diseaseIdx   = data.diseaseIdx;
                        manager.grid[num2].contents.diseaseCount = data.diseaseCount;
                    }
                }
            }
        }

        private class ResetConduit : ConduitTask {
            public ResetConduit() : base("ResetConduitTask") { }

            protected override void RunDivision(SOAInfo soaInfo) {
                for (var num = start; num != end; num++) manager.grid[soaInfo.cells[num]].conduitIdx = -1;
            }
        }

        private class InitializeContentsTask : ConduitTask {
            public InitializeContentsTask() : base("SetInitialContents") { }

            protected override void RunDivision(SOAInfo soaInfo) {
                for (var num = start; num != end; num++) {
                    var num2                                        = soaInfo.cells[num];
                    var conduitContents                             = soaInfo.conduits[num].GetContents(manager);
                    if (conduitContents.mass <= 0f) conduitContents = ConduitContents.Empty;
                    soaInfo.initialContents[num] = conduitContents;
                    manager.grid[num2].contents  = conduitContents;
                }
            }
        }

        private class InvalidateLastFlow : ConduitTask {
            public InvalidateLastFlow() : base("InvalidateLastFlow") { }

            protected override void RunDivision(SOAInfo soaInfo) {
                for (var num = start; num != end; num++) soaInfo.lastFlowInfo[num] = ConduitFlowInfo.DEFAULT;
            }
        }

        private class PublishTemperatureToGame : ConduitTask {
            public PublishTemperatureToGame() : base("PublishTemperatureToGame") { }

            protected override void RunDivision(SOAInfo soaInfo) {
                for (var num = start; num != end; num++)
                    Game.Instance.conduitTemperatureManager.SetData(soaInfo.temperatureHandles[num],
                                                                    ref manager.grid[soaInfo.cells[num]].contents);
            }
        }

        private class PublishDiseaseToGame : ConduitTask {
            public PublishDiseaseToGame() : base("PublishDiseaseToGame") { }

            protected override void RunDivision(SOAInfo soaInfo) {
                for (var num = start; num != end; num++)
                    Game.Instance.conduitDiseaseManager.SetData(soaInfo.diseaseHandles[num],
                                                                ref manager.grid[soaInfo.cells[num]].contents);
            }
        }

        private class FlowThroughVacuum : ConduitTask {
            public FlowThroughVacuum() : base("FlowThroughVacuum") { }

            protected override void RunDivision(SOAInfo soaInfo) {
                for (var num = start; num != end; num++) {
                    var conduit = soaInfo.conduits[num];
                    var cell    = conduit.GetCell(manager);
                    if (manager.grid[cell].contents.element == SimHashes.Vacuum)
                        soaInfo.srcFlowDirections[conduit.idx] = FlowDirections.None;
                }
            }
        }
    }

    [DebuggerDisplay("{priority} {callback.Target.name} {callback.Target} {callback.Method}")]
    public struct ConduitUpdater {
        public ConduitFlowPriority priority;
        public Action<float>       callback;
    }

    [DebuggerDisplay("conduit {conduitIdx}:{contents.element}")]
    public struct GridNode {
        public int             conduitIdx;
        public ConduitContents contents;
    }

    public struct SerializedContents {
        public SerializedContents(SimHashes element,
                                  float     mass,
                                  float     temperature,
                                  byte      disease_idx,
                                  int       disease_count) {
            this.element     = element;
            this.mass        = mass;
            this.temperature = temperature;
            diseaseHash      = disease_idx != byte.MaxValue ? Db.Get().Diseases[disease_idx].id.GetHashCode() : 0;
            diseaseCount     = disease_count;
            if (diseaseCount <= 0) diseaseHash = 0;
        }

        public SerializedContents(ConduitContents src) {
            this = new SerializedContents(src.element, src.mass, src.temperature, src.diseaseIdx, src.diseaseCount);
        }

        public SimHashes element;
        public float     mass;
        public float     temperature;
        public int       diseaseHash;
        public int       diseaseCount;
    }

    [DebuggerDisplay("conduits l:{left}, r:{right}, u:{up}, d:{down}")]
    public struct ConduitConnections {
        public int left;
        public int right;
        public int up;
        public int down;

        public static readonly ConduitConnections DEFAULT
            = new ConduitConnections { left = -1, right = -1, up = -1, down = -1 };
    }

    [DebuggerDisplay("{direction}:{contents.element}")]
    public struct ConduitFlowInfo {
        public FlowDirections  direction;
        public ConduitContents contents;

        public static readonly ConduitFlowInfo DEFAULT
            = new ConduitFlowInfo { direction = FlowDirections.None, contents = ConduitContents.Empty };
    }

    [DebuggerDisplay("conduit {idx}"), Serializable]
    public struct Conduit : IEquatable<Conduit> {
        public Conduit(int idx) { this.idx = idx; }

        public FlowDirections GetPermittedFlowDirections(ConduitFlow manager) {
            return manager.soaInfo.GetPermittedFlowDirections(idx);
        }

        public void SetPermittedFlowDirections(FlowDirections permitted, ConduitFlow manager) {
            manager.soaInfo.SetPermittedFlowDirections(idx, permitted);
        }

        public FlowDirections GetTargetFlowDirection(ConduitFlow manager) {
            return manager.soaInfo.GetTargetFlowDirection(idx);
        }

        public void SetTargetFlowDirection(FlowDirections directions, ConduitFlow manager) {
            manager.soaInfo.SetTargetFlowDirection(idx, directions);
        }

        public ConduitContents GetContents(ConduitFlow manager) {
            var cell     = manager.soaInfo.GetCell(idx);
            var contents = manager.grid[cell].contents;
            var soaInfo  = manager.soaInfo;
            contents.temperature = soaInfo.GetConduitTemperature(idx);
            var diseaseData = soaInfo.GetDiseaseData(idx);
            contents.diseaseIdx   = diseaseData.diseaseIdx;
            contents.diseaseCount = diseaseData.diseaseCount;
            return contents;
        }

        public void SetContents(ConduitFlow manager, ConduitContents contents) {
            var cell = manager.soaInfo.GetCell(idx);
            manager.grid[cell].contents = contents;
            var soaInfo = manager.soaInfo;
            soaInfo.SetConduitTemperatureData(idx, ref contents);
            soaInfo.ForcePermanentDiseaseContainer(idx, contents.diseaseIdx != byte.MaxValue);
            soaInfo.SetDiseaseData(idx, ref contents);
        }

        public ConduitFlowInfo GetLastFlowInfo(ConduitFlow manager) { return manager.soaInfo.GetLastFlowInfo(idx); }

        public ConduitContents GetInitialContents(ConduitFlow manager) {
            return manager.soaInfo.GetInitialContents(idx);
        }

        public                 int     GetCell(ConduitFlow manager) { return manager.soaInfo.GetCell(idx); }
        public                 bool    Equals(Conduit      other)   { return idx == other.idx; }
        public static readonly Conduit Invalid = new Conduit(-1);
        public readonly        int     idx;
    }

    [DebuggerDisplay("{element} M:{mass} T:{temperature}")]
    public struct ConduitContents {
        public float mass         => initial_mass + added_mass - removed_mass;
        public float movable_mass => initial_mass              - removed_mass;

        public ConduitContents(SimHashes element, float mass, float temperature, byte disease_idx, int disease_count) {
            Debug.Assert(!float.IsNaN(temperature));
            this.element     = element;
            initial_mass     = mass;
            added_mass       = 0f;
            removed_mass     = 0f;
            this.temperature = temperature;
            diseaseIdx       = disease_idx;
            diseaseCount     = disease_count;
        }

        public void ConsolidateMass() {
            initial_mass += added_mass;
            added_mass   =  0f;
            initial_mass -= removed_mass;
            removed_mass =  0f;
        }

        public float GetEffectiveCapacity(float maximum_capacity) {
            var mass = this.mass;
            return Mathf.Max(0f, maximum_capacity - mass);
        }

        public void AddMass(float amount) {
            Debug.Assert(0f <= amount);
            added_mass += amount;
        }

        public float RemoveMass(float amount) {
            Debug.Assert(0f <= amount);
            var result = 0f;
            var num    = mass - amount;
            if (num < 0f) {
                amount += num;
                result =  -num;
                Debug.Assert(false);
            }

            removed_mass += amount;
            return result;
        }

        public  SimHashes element;
        private float     initial_mass;
        private float     added_mass;
        private float     removed_mass;
        public  float     temperature;
        public  byte      diseaseIdx;
        public  int       diseaseCount;

        public static readonly ConduitContents Empty = new ConduitContents {
            element      = SimHashes.Vacuum,
            initial_mass = 0f,
            added_mass   = 0f,
            removed_mass = 0f,
            temperature  = 0f,
            diseaseIdx   = byte.MaxValue,
            diseaseCount = 0
        };
    }

    [DebuggerDisplay("{network.ConduitType}:{cells.Count}")]
    private struct Network {
        public List<int>          cells;
        public FlowUtilityNetwork network;
    }

    private struct BuildNetworkTask : IWorkItem<ConduitFlow> {
        public BuildNetworkTask(Network network, int conduit_count) {
            this.network          = network;
            distance_nodes        = QueuePool<DistanceNode, ConduitFlow>.Allocate();
            distances_via_sources = DictionaryPool<int, int, ConduitFlow>.Allocate();
            from_sources          = ListPool<KeyValuePair<int, int>, ConduitFlow>.Allocate();
            distances_via_sinks   = DictionaryPool<int, int, ConduitFlow>.Allocate();
            from_sinks            = ListPool<KeyValuePair<int, int>, ConduitFlow>.Allocate();
            from_sources_graph    = new Graph(network.network);
            from_sinks_graph      = new Graph(network.network);
        }

        public void Finish() {
            distances_via_sinks.Recycle();
            distances_via_sources.Recycle();
            distance_nodes.Recycle();
            from_sources.Recycle();
            from_sinks.Recycle();
            from_sources_graph.Recycle();
            from_sinks_graph.Recycle();
        }

        private void ComputeFlow(ConduitFlow outer) {
            from_sources_graph.Build(outer, network.network.sources, network.network.sinks, true);
            from_sinks_graph.Build(outer, network.network.sinks, network.network.sources, false);
            from_sources_graph.Merge(from_sinks_graph);
            from_sources_graph.BreakCycles();
            from_sources_graph.WriteFlow();
            from_sinks_graph.WriteFlow(true);
        }

        private void ComputeOrder(ConduitFlow outer) {
            foreach (var cell in from_sources_graph.sources)
                distance_nodes.Enqueue(new DistanceNode { cell = cell, distance = 0 });

            using (var enumerator = from_sources_graph.dead_ends.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    var cell2 = enumerator.Current;
                    distance_nodes.Enqueue(new DistanceNode { cell = cell2, distance = 0 });
                }

                goto IL_21D;
            }

            IL_B3:
            var distanceNode = distance_nodes.Dequeue();
            var conduitIdx   = outer.grid[distanceNode.cell].conduitIdx;
            if (conduitIdx != -1) {
                distances_via_sources[distanceNode.cell] = distanceNode.distance;
                var conduitConnections      = outer.soaInfo.GetConduitConnections(conduitIdx);
                var permittedFlowDirections = outer.soaInfo.GetPermittedFlowDirections(conduitIdx);
                if ((permittedFlowDirections & FlowDirections.Up) != FlowDirections.None)
                    distance_nodes.Enqueue(new DistanceNode {
                        cell = outer.soaInfo.GetCell(conduitConnections.up), distance = distanceNode.distance + 1
                    });

                if ((permittedFlowDirections & FlowDirections.Down) != FlowDirections.None)
                    distance_nodes.Enqueue(new DistanceNode {
                        cell = outer.soaInfo.GetCell(conduitConnections.down), distance = distanceNode.distance + 1
                    });

                if ((permittedFlowDirections & FlowDirections.Left) != FlowDirections.None)
                    distance_nodes.Enqueue(new DistanceNode {
                        cell = outer.soaInfo.GetCell(conduitConnections.left), distance = distanceNode.distance + 1
                    });

                if ((permittedFlowDirections & FlowDirections.Right) != FlowDirections.None)
                    distance_nodes.Enqueue(new DistanceNode {
                        cell = outer.soaInfo.GetCell(conduitConnections.right), distance = distanceNode.distance + 1
                    });
            }

            IL_21D:
            if (distance_nodes.Count != 0) goto IL_B3;

            from_sources.AddRange(distances_via_sources);
            from_sources.Sort((a, b) => b.Value - a.Value);
            distance_nodes.Clear();
            foreach (var cell3 in from_sinks_graph.sources)
                distance_nodes.Enqueue(new DistanceNode { cell = cell3, distance = 0 });

            using (var enumerator = from_sinks_graph.dead_ends.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    var cell4 = enumerator.Current;
                    distance_nodes.Enqueue(new DistanceNode { cell = cell4, distance = 0 });
                }

                goto IL_508;
            }

            IL_32A:
            var distanceNode2 = distance_nodes.Dequeue();
            var conduitIdx2   = outer.grid[distanceNode2.cell].conduitIdx;
            if (conduitIdx2 != -1) {
                if (!distances_via_sources.ContainsKey(distanceNode2.cell))
                    distances_via_sinks[distanceNode2.cell] = distanceNode2.distance;

                var conduitConnections2 = outer.soaInfo.GetConduitConnections(conduitIdx2);
                if (conduitConnections2.up != -1 &&
                    (outer.soaInfo.GetPermittedFlowDirections(conduitConnections2.up) & FlowDirections.Down) !=
                    FlowDirections.None)
                    distance_nodes.Enqueue(new DistanceNode {
                        cell = outer.soaInfo.GetCell(conduitConnections2.up), distance = distanceNode2.distance + 1
                    });

                if (conduitConnections2.down != -1 &&
                    (outer.soaInfo.GetPermittedFlowDirections(conduitConnections2.down) & FlowDirections.Up) !=
                    FlowDirections.None)
                    distance_nodes.Enqueue(new DistanceNode {
                        cell = outer.soaInfo.GetCell(conduitConnections2.down), distance = distanceNode2.distance + 1
                    });

                if (conduitConnections2.left != -1 &&
                    (outer.soaInfo.GetPermittedFlowDirections(conduitConnections2.left) & FlowDirections.Right) !=
                    FlowDirections.None)
                    distance_nodes.Enqueue(new DistanceNode {
                        cell = outer.soaInfo.GetCell(conduitConnections2.left), distance = distanceNode2.distance + 1
                    });

                if (conduitConnections2.right != -1 &&
                    (outer.soaInfo.GetPermittedFlowDirections(conduitConnections2.right) & FlowDirections.Left) !=
                    FlowDirections.None)
                    distance_nodes.Enqueue(new DistanceNode {
                        cell = outer.soaInfo.GetCell(conduitConnections2.right), distance = distanceNode2.distance + 1
                    });
            }

            IL_508:
            if (distance_nodes.Count == 0) {
                from_sinks.AddRange(distances_via_sinks);
                from_sinks.Sort((a, b) => a.Value - b.Value);
                network.cells.Capacity = Mathf.Max(network.cells.Capacity, from_sources.Count + from_sinks.Count);
                foreach (var keyValuePair in from_sources) network.cells.Add(keyValuePair.Key);
                foreach (var keyValuePair2 in from_sinks) network.cells.Add(keyValuePair2.Key);
                return;
            }

            goto IL_32A;
        }

        public void Run(ConduitFlow outer) {
            ComputeFlow(outer);
            ComputeOrder(outer);
        }

        private readonly Network                                                  network;
        private readonly QueuePool<DistanceNode, ConduitFlow>.PooledQueue         distance_nodes;
        private readonly DictionaryPool<int, int, ConduitFlow>.PooledDictionary   distances_via_sources;
        private readonly ListPool<KeyValuePair<int, int>, ConduitFlow>.PooledList from_sources;
        private readonly DictionaryPool<int, int, ConduitFlow>.PooledDictionary   distances_via_sinks;
        private readonly ListPool<KeyValuePair<int, int>, ConduitFlow>.PooledList from_sinks;
        private          Graph                                                    from_sources_graph;
        private          Graph                                                    from_sinks_graph;

        [DebuggerDisplay("cell {cell}:{distance}")]
        private struct DistanceNode {
            public int cell;
            public int distance;
        }

        [DebuggerDisplay("vertices:{vertex_cells.Count}, edges:{edges.Count}")]
        private struct Graph {
            public Graph(FlowUtilityNetwork network) {
                conduit_flow   = null;
                vertex_cells   = HashSetPool<int, ConduitFlow>.Allocate();
                edges          = ListPool<Edge, ConduitFlow>.Allocate();
                cycles         = ListPool<Edge, ConduitFlow>.Allocate();
                bfs_traversal  = QueuePool<Vertex, ConduitFlow>.Allocate();
                visited        = HashSetPool<int, ConduitFlow>.Allocate();
                pseudo_sources = ListPool<Vertex, ConduitFlow>.Allocate();
                sources        = HashSetPool<int, ConduitFlow>.Allocate();
                sinks          = HashSetPool<int, ConduitFlow>.Allocate();
                dfs_path       = HashSetPool<DFSNode, ConduitFlow>.Allocate();
                dfs_traversal  = ListPool<DFSNode, ConduitFlow>.Allocate();
                dead_ends      = HashSetPool<int, ConduitFlow>.Allocate();
                cycle_vertices = ListPool<Vertex, ConduitFlow>.Allocate();
            }

            public void Recycle() {
                vertex_cells.Recycle();
                edges.Recycle();
                cycles.Recycle();
                bfs_traversal.Recycle();
                visited.Recycle();
                pseudo_sources.Recycle();
                sources.Recycle();
                sinks.Recycle();
                dfs_path.Recycle();
                dfs_traversal.Recycle();
                dead_ends.Recycle();
                cycle_vertices.Recycle();
            }

            public void Build(ConduitFlow                    conduit_flow,
                              List<FlowUtilityNetwork.IItem> sources,
                              List<FlowUtilityNetwork.IItem> sinks,
                              bool                           are_dead_ends_pseudo_sources) {
                this.conduit_flow = conduit_flow;
                this.sources.Clear();
                for (var i = 0; i < sources.Count; i++) {
                    var cell = sources[i].Cell;
                    if (conduit_flow.grid[cell].conduitIdx != -1) this.sources.Add(cell);
                }

                this.sinks.Clear();
                for (var j = 0; j < sinks.Count; j++) {
                    var cell2 = sinks[j].Cell;
                    if (conduit_flow.grid[cell2].conduitIdx != -1) this.sinks.Add(cell2);
                }

                Debug.Assert(bfs_traversal.Count == 0);
                visited.Clear();
                foreach (var num in this.sources) {
                    bfs_traversal.Enqueue(new Vertex { cell = num, direction = FlowDirections.None });
                    visited.Add(num);
                }

                pseudo_sources.Clear();
                dead_ends.Clear();
                cycles.Clear();
                while (bfs_traversal.Count != 0) {
                    var node = bfs_traversal.Dequeue();
                    vertex_cells.Add(node.cell);
                    var flowDirections = FlowDirections.None;
                    var num2           = 4;
                    if (node.direction != FlowDirections.None) {
                        flowDirections = Opposite(node.direction);
                        num2           = 3;
                    }

                    var conduitIdx = conduit_flow.grid[node.cell].conduitIdx;
                    for (var num3 = 0; num3 != num2; num3++) {
                        flowDirections = ComputeNextFlowDirection(flowDirections);
                        var conduitFromDirection
                            = conduit_flow.soaInfo.GetConduitFromDirection(conduitIdx, flowDirections);

                        var new_node = WalkPath(conduitIdx,
                                                conduitFromDirection.idx,
                                                flowDirections,
                                                are_dead_ends_pseudo_sources);

                        if (new_node.is_valid) {
                            var item = new Edge {
                                vertices = new[] {
                                    new Vertex { cell = node.cell, direction = flowDirections }, new_node
                                }
                            };

                            if (new_node.cell == node.cell)
                                cycles.Add(item);
                            else if (!edges.Any(edge => edge.vertices[0].cell == new_node.cell &&
                                                        edge.vertices[1].cell == node.cell) &&
                                     !edges.Contains(item)) {
                                edges.Add(item);
                                if (visited.Add(new_node.cell)) {
                                    if (IsSink(new_node.cell))
                                        pseudo_sources.Add(new_node);
                                    else
                                        bfs_traversal.Enqueue(new_node);
                                }
                            }
                        }
                    }

                    if (bfs_traversal.Count == 0) {
                        foreach (var item2 in pseudo_sources) bfs_traversal.Enqueue(item2);
                        pseudo_sources.Clear();
                    }
                }
            }

            private bool IsEndpoint(int cell) {
                Debug.Assert(cell != -1);
                return conduit_flow.grid[cell].conduitIdx == -1 ||
                       sources.Contains(cell)                   ||
                       sinks.Contains(cell)                     ||
                       dead_ends.Contains(cell);
            }

            private bool IsSink(int cell) { return sinks.Contains(cell); }

            private bool IsJunction(int cell) {
                Debug.Assert(cell != -1);
                var gridNode = conduit_flow.grid[cell];
                Debug.Assert(gridNode.conduitIdx != -1);
                var conduitConnections = conduit_flow.soaInfo.GetConduitConnections(gridNode.conduitIdx);
                return 2 <
                       JunctionValue(conduitConnections.down) +
                       JunctionValue(conduitConnections.left) +
                       JunctionValue(conduitConnections.up)   +
                       JunctionValue(conduitConnections.right);
            }

            private int JunctionValue(int conduit) {
                if (conduit != -1) return 1;

                return 0;
            }

            private Vertex WalkPath(int            root_conduit,
                                    int            conduit,
                                    FlowDirections direction,
                                    bool           are_dead_ends_pseudo_sources) {
                if (conduit == -1) return Vertex.INVALID;

                int cell;
                for (;;) {
                    cell = conduit_flow.soaInfo.GetCell(conduit);
                    if (IsEndpoint(cell) || IsJunction(cell)) break;

                    direction = Opposite(direction);
                    var flag = true;
                    for (var num = 0; num != 3; num++) {
                        direction = ComputeNextFlowDirection(direction);
                        var conduitFromDirection = conduit_flow.soaInfo.GetConduitFromDirection(conduit, direction);
                        if (conduitFromDirection.idx != -1) {
                            conduit = conduitFromDirection.idx;
                            flag    = false;
                            break;
                        }
                    }

                    if (flag) goto Block_4;
                }

                return new Vertex { cell = cell, direction = direction };

                Block_4:
                if (are_dead_ends_pseudo_sources) {
                    pseudo_sources.Add(new Vertex { cell = cell, direction = ComputeNextFlowDirection(direction) });
                    dead_ends.Add(cell);
                    return Vertex.INVALID;
                }

                var result = default(Vertex);
                result.cell = cell;
                direction   = result.direction = Opposite(ComputeNextFlowDirection(direction));
                return result;
            }

            public void Merge(Graph inverted_graph) {
                using (var enumerator = inverted_graph.edges.GetEnumerator()) {
                    while (enumerator.MoveNext()) {
                        var inverted_edge = enumerator.Current;
                        var candidate     = inverted_edge.Invert();
                        if (!edges.Any(edge => edge.Equals(inverted_edge) || edge.Equals(candidate))) {
                            edges.Add(candidate);
                            vertex_cells.Add(candidate.vertices[0].cell);
                            vertex_cells.Add(candidate.vertices[1].cell);
                        }
                    }
                }

                var num = 1000;
                for (var num2 = 0; num2 != num; num2++) {
                    Debug.Assert(num2 != num - 1);
                    var flag = false;
                    using (var enumerator2 = vertex_cells.GetEnumerator()) {
                        while (enumerator2.MoveNext()) {
                            var cell = enumerator2.Current;
                            if (!IsSink(cell) && !edges.Any(edge => edge.vertices[0].cell == cell)) {
                                var num3 = inverted_graph.edges.FindIndex(inverted_edge =>
                                                                              inverted_edge.vertices[1].cell == cell);

                                if (num3 != -1) {
                                    var edge3 = inverted_graph.edges[num3];
                                    for (var num4 = 0; num4 != edges.Count; num4++) {
                                        var edge2 = edges[num4];
                                        if (edge2.vertices[0].cell == edge3.vertices[0].cell &&
                                            edge2.vertices[1].cell == edge3.vertices[1].cell)
                                            edges[num4] = edge2.Invert();
                                    }

                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (!flag) break;
                }
            }

            public void BreakCycles() {
                visited.Clear();
                foreach (var num in vertex_cells)
                    if (!visited.Contains(num)) {
                        dfs_path.Clear();
                        dfs_traversal.Clear();
                        dfs_traversal.Add(new DFSNode { cell = num, parent = null });
                        while (dfs_traversal.Count != 0) {
                            var dfsnode = dfs_traversal[dfs_traversal.Count - 1];
                            dfs_traversal.RemoveAt(dfs_traversal.Count - 1);
                            var flag = false;
                            for (var parent = dfsnode.parent; parent != null; parent = parent.parent)
                                if (parent.cell == dfsnode.cell) {
                                    flag = true;
                                    break;
                                }

                            if (flag)
                                for (var num2 = edges.Count - 1; num2 != -1; num2--) {
                                    var edge = edges[num2];
                                    if (edge.vertices[0].cell == dfsnode.parent.cell &&
                                        edge.vertices[1].cell == dfsnode.cell) {
                                        cycles.Add(edge);
                                        edges.RemoveAt(num2);
                                    }
                                }
                            else if (visited.Add(dfsnode.cell))
                                foreach (var edge2 in edges)
                                    if (edge2.vertices[0].cell == dfsnode.cell)
                                        dfs_traversal.Add(new DFSNode {
                                            cell = edge2.vertices[1].cell, parent = dfsnode
                                        });
                        }
                    }
            }

            public void WriteFlow(bool cycles_only = false) {
                if (!cycles_only)
                    foreach (var edge in edges) {
                        var vertexIterator = edge.Iter(conduit_flow);
                        while (vertexIterator.IsValid()) {
                            conduit_flow.soaInfo.AddPermittedFlowDirections(conduit_flow.grid[vertexIterator.cell]
                                                                                .conduitIdx,
                                                                            vertexIterator.direction);

                            vertexIterator.Next();
                        }
                    }

                foreach (var edge2 in cycles) {
                    cycle_vertices.Clear();
                    var vertexIterator2 = edge2.Iter(conduit_flow);
                    vertexIterator2.Next();
                    while (vertexIterator2.IsValid()) {
                        cycle_vertices.Add(new Vertex {
                            cell = vertexIterator2.cell, direction = vertexIterator2.direction
                        });

                        vertexIterator2.Next();
                    }

                    if (cycle_vertices.Count > 1) {
                        var i         = 0;
                        var num       = cycle_vertices.Count - 1;
                        var direction = edge2.vertices[0].direction;
                        while (i <= num) {
                            var vertex = cycle_vertices[i];
                            conduit_flow.soaInfo.AddPermittedFlowDirections(conduit_flow.grid[vertex.cell].conduitIdx,
                                                                            Opposite(direction));

                            direction = vertex.direction;
                            i++;
                            var vertex2 = cycle_vertices[num];
                            conduit_flow.soaInfo.AddPermittedFlowDirections(conduit_flow.grid[vertex2.cell].conduitIdx,
                                                                            vertex2.direction);

                            num--;
                        }

                        dead_ends.Add(cycle_vertices[i].cell);
                        dead_ends.Add(cycle_vertices[num].cell);
                    }
                }
            }

            private          ConduitFlow                                     conduit_flow;
            private          HashSetPool<int, ConduitFlow>.PooledHashSet     vertex_cells;
            private          ListPool<Edge, ConduitFlow>.PooledList          edges;
            private readonly ListPool<Edge, ConduitFlow>.PooledList          cycles;
            private readonly QueuePool<Vertex, ConduitFlow>.PooledQueue      bfs_traversal;
            private readonly HashSetPool<int, ConduitFlow>.PooledHashSet     visited;
            private readonly ListPool<Vertex, ConduitFlow>.PooledList        pseudo_sources;
            public readonly  HashSetPool<int, ConduitFlow>.PooledHashSet     sources;
            private readonly HashSetPool<int, ConduitFlow>.PooledHashSet     sinks;
            private readonly HashSetPool<DFSNode, ConduitFlow>.PooledHashSet dfs_path;
            private readonly ListPool<DFSNode, ConduitFlow>.PooledList       dfs_traversal;
            public readonly  HashSetPool<int, ConduitFlow>.PooledHashSet     dead_ends;
            private readonly ListPool<Vertex, ConduitFlow>.PooledList        cycle_vertices;

            [DebuggerDisplay("{cell}:{direction}")]
            public struct Vertex : IEquatable<Vertex> {
                public bool is_valid => cell != -1;
                public bool Equals(Vertex rhs) { return direction == rhs.direction && cell == rhs.cell; }
                public FlowDirections direction;
                public int cell;
                public static readonly Vertex INVALID = new Vertex { direction = FlowDirections.None, cell = -1 };
            }

            [DebuggerDisplay("{vertices[0].cell}:{vertices[0].direction} -> {vertices[1].cell}:{vertices[1].direction}")]
            public struct Edge : IEquatable<Edge> {
                public bool is_valid => vertices != null;

                public bool Equals(Edge rhs) {
                    if (vertices == null) return rhs.vertices == null;

                    return rhs.vertices    != null                &&
                           vertices.Length == rhs.vertices.Length &&
                           vertices.Length == 2                   &&
                           vertices[0].Equals(rhs.vertices[0])    &&
                           vertices[1].Equals(rhs.vertices[1]);
                }

                public Edge Invert() {
                    return new Edge {
                        vertices = new[] {
                            new Vertex { cell = vertices[1].cell, direction = Opposite(vertices[1].direction) },
                            new Vertex { cell = vertices[0].cell, direction = Opposite(vertices[0].direction) }
                        }
                    };
                }

                public VertexIterator Iter(ConduitFlow conduit_flow) { return new VertexIterator(conduit_flow, this); }
                public Vertex[] vertices;
                public static readonly Edge INVALID = new Edge { vertices = null };

                [DebuggerDisplay("{cell}:{direction}")]
                public struct VertexIterator {
                    public VertexIterator(ConduitFlow conduit_flow, Edge edge) {
                        this.conduit_flow = conduit_flow;
                        this.edge         = edge;
                        cell              = edge.vertices[0].cell;
                        direction         = edge.vertices[0].direction;
                    }

                    public void Next() {
                        var conduitIdx           = conduit_flow.grid[cell].conduitIdx;
                        var conduitFromDirection = conduit_flow.soaInfo.GetConduitFromDirection(conduitIdx, direction);
                        Debug.Assert(conduitFromDirection.idx != -1);
                        cell = conduitFromDirection.GetCell(conduit_flow);
                        if (cell == edge.vertices[1].cell) return;

                        direction = Opposite(direction);
                        var flag = false;
                        for (var num = 0; num != 3; num++) {
                            direction = ComputeNextFlowDirection(direction);
                            if (conduit_flow.soaInfo.GetConduitFromDirection(conduitFromDirection.idx, direction).idx !=
                                -1) {
                                flag = true;
                                break;
                            }
                        }

                        Debug.Assert(flag);
                        if (!flag) cell = edge.vertices[1].cell;
                    }

                    public           bool           IsValid() { return cell != edge.vertices[1].cell; }
                    public           int            cell;
                    public           FlowDirections direction;
                    private readonly ConduitFlow    conduit_flow;
                    private readonly Edge           edge;
                }
            }

            [DebuggerDisplay("cell:{cell}, parent:{parent == null ? -1 : parent.cell}")]
            private class DFSNode {
                public int     cell;
                public DFSNode parent;
            }
        }
    }

    private struct ConnectContext {
        public ConnectContext(ConduitFlow outer) {
            this.outer     = outer;
            cells          = ListPool<int, ConduitFlow>.Allocate();
            cells.Capacity = Mathf.Max(cells.Capacity, outer.soaInfo.NumEntries);
        }

        public          void                                  Finish() { cells.Recycle(); }
        public readonly ListPool<int, ConduitFlow>.PooledList cells;
        public readonly ConduitFlow                           outer;
    }

    private struct ConnectTask : IWorkItem<ConnectContext> {
        public ConnectTask(int start, int end) {
            this.start = start;
            this.end   = end;
        }

        public void Run(ConnectContext context) {
            for (var num = start; num != end; num++) {
                var num2       = context.cells[num];
                var conduitIdx = context.outer.grid[num2].conduitIdx;
                if (conduitIdx != -1) {
                    var connections = context.outer.networkMgr.GetConnections(num2, true);
                    if (connections != 0) {
                        var @default = ConduitConnections.DEFAULT;
                        var num3     = num2 - 1;
                        if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Left) != 0)
                            @default.left = context.outer.grid[num3].conduitIdx;

                        num3 = num2 + 1;
                        if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Right) != 0)
                            @default.right = context.outer.grid[num3].conduitIdx;

                        num3 = num2 - Grid.WidthInCells;
                        if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Down) != 0)
                            @default.down = context.outer.grid[num3].conduitIdx;

                        num3 = num2 + Grid.WidthInCells;
                        if (Grid.IsValidCell(num3) && (connections & UtilityConnections.Up) != 0)
                            @default.up = context.outer.grid[num3].conduitIdx;

                        context.outer.soaInfo.SetConduitConnections(conduitIdx, @default);
                    }
                }
            }
        }

        private readonly int start;
        private readonly int end;
    }

    private class UpdateNetworkTask : IWorkItem<ConduitFlow> {
        private readonly Network network;

        public UpdateNetworkTask(Network network) {
            continue_updating = true;
            this.network      = network;
        }

        public bool continue_updating { get; private set; }

        public void Run(ConduitFlow conduit_flow) {
            Debug.Assert(continue_updating);
            continue_updating = false;
            foreach (var num in network.cells) {
                var conduitIdx = conduit_flow.grid[num].conduitIdx;
                if (conduit_flow.UpdateConduit(conduit_flow.soaInfo.GetConduit(conduitIdx))) continue_updating = true;
            }
        }

        public void Finish(ConduitFlow conduit_flow) {
            foreach (var num in network.cells) {
                var contents = conduit_flow.grid[num].contents;
                contents.ConsolidateMass();
                conduit_flow.grid[num].contents = contents;
            }
        }
    }
}