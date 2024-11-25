using System;
using System.Collections.Generic;
using UnityEngine;

public class UtilityNetworkManager<NetworkType, ItemType> : IUtilityNetworkMgr
    where NetworkType : UtilityNetwork, new() where ItemType : MonoBehaviour {
    private readonly Dictionary<int, object> endpoints = new Dictionary<int, object>();
    private readonly Dictionary<int, object> items = new Dictionary<int, object>();
    private readonly Dictionary<int, int> links = new Dictionary<int, int>();
    private readonly List<UtilityNetwork> networks;
    private          Action<IList<UtilityNetwork>, ICollection<int>> onNetworksRebuilt;
    protected        UtilityNetworkGridNode[] physicalGrid;
    protected        HashSet<int> physicalNodes;
    private          Queue<int> queued = new Queue<int>();
    private          HashSet<object> queuedVirtualKeys;
    private readonly Dictionary<int, object> semiVirtualLinks = new Dictionary<int, object>();
    private          UtilityNetworkGridNode[] stashedVisualGrid;
    private readonly int tileLayer = -1;
    private readonly Dictionary<object, List<object>> virtualEndpoints = new Dictionary<object, List<object>>();
    private readonly Dictionary<object, List<object>> virtualItems = new Dictionary<object, List<object>>();
    private readonly Dictionary<object, int> virtualKeyToNetworkIdx = new Dictionary<object, int>();
    private          HashSet<int> visitedCells;
    private          HashSet<object> visitedVirtualKeys;
    protected        UtilityNetworkGridNode[] visualGrid;
    protected        HashSet<int> visualNodes;

    public UtilityNetworkManager(int game_width, int game_height, int tile_layer) {
        tileLayer = tile_layer;
        networks  = new List<UtilityNetwork>();
        Initialize(game_width, game_height);
    }

    public bool IsDirty { get; private set; }

    public void ClearCell(int cell, bool is_physical_building) {
        if (Game.IsQuitting()) return;

        var grid        = GetGrid(is_physical_building);
        var nodes       = GetNodes(is_physical_building);
        var connections = grid[cell].connections;
        grid[cell].connections = 0;
        var vector2I = Grid.CellToXY(cell);
        if (vector2I.x > 0 && (connections & UtilityConnections.Left) != 0) {
            var array = grid;
            var num   = Grid.CellLeft(cell);
            array[num].connections = array[num].connections & ~UtilityConnections.Right;
        }

        if (vector2I.x < Grid.WidthInCells - 1 && (connections & UtilityConnections.Right) != 0) {
            var array2 = grid;
            var num2   = Grid.CellRight(cell);
            array2[num2].connections = array2[num2].connections & ~UtilityConnections.Left;
        }

        if (vector2I.y > 0 && (connections & UtilityConnections.Down) != 0) {
            var array3 = grid;
            var num3   = Grid.CellBelow(cell);
            array3[num3].connections = array3[num3].connections & ~UtilityConnections.Up;
        }

        if (vector2I.y < Grid.HeightInCells - 1 && (connections & UtilityConnections.Up) != 0) {
            var array4 = grid;
            var num4   = Grid.CellAbove(cell);
            array4[num4].connections = array4[num4].connections & ~UtilityConnections.Down;
        }

        nodes.Remove(cell);
        if (is_physical_building) {
            IsDirty = true;
            ClearCell(cell, false);
        }
    }

    public void ForceRebuildNetworks() { IsDirty = true; }

    public void AddToNetworks(int cell, object item, bool is_endpoint) {
        if (item != null) {
            if (is_endpoint) {
                if (endpoints.ContainsKey(cell)) {
                    Debug.LogWarning(string.Format("Cell {0} already has a utility network endpoint assigned. Adding {1} will stomp previous endpoint, destroying the object that's already there.",
                                                   cell,
                                                   item));

                    var kmonoBehaviour = endpoints[cell] as KMonoBehaviour;
                    if (kmonoBehaviour != null) Util.KDestroyGameObject(kmonoBehaviour);
                }

                endpoints[cell] = item;
            } else {
                if (items.ContainsKey(cell)) {
                    Debug.LogWarning(string.Format("Cell {0} already has a utility network connector assigned. Adding {1} will stomp previous item, destroying the object that's already there.",
                                                   cell,
                                                   item));

                    var kmonoBehaviour2 = items[cell] as KMonoBehaviour;
                    if (kmonoBehaviour2 != null) Util.KDestroyGameObject(kmonoBehaviour2);
                }

                items[cell] = item;
            }
        }

        IsDirty = true;
    }

    public void RemoveFromNetworks(int cell, object item, bool is_endpoint) {
        if (Game.IsQuitting()) return;

        IsDirty = true;
        if (item != null) {
            if (is_endpoint) {
                endpoints.Remove(cell);
                var networkIdx = physicalGrid[cell].networkIdx;
                if (networkIdx != -1) networks[networkIdx].RemoveItem(item);
            } else {
                var networkIdx2 = physicalGrid[cell].networkIdx;
                physicalGrid[cell].connections = 0;
                physicalGrid[cell].networkIdx  = -1;
                items.Remove(cell);
                Disconnect(cell);
                object item2;
                if (endpoints.TryGetValue(cell, out item2) && networkIdx2 != -1)
                    networks[networkIdx2].DisconnectItem(item2);
            }
        }
    }

    public UtilityNetwork GetNetworkForCell(int cell) {
        UtilityNetwork result = null;
        if (Grid.IsValidCell(cell)                                         &&
            0                             <= physicalGrid[cell].networkIdx &&
            physicalGrid[cell].networkIdx < networks.Count)
            result = networks[physicalGrid[cell].networkIdx];

        return result;
    }

    public UtilityNetwork GetNetworkForDirection(int cell, Direction direction) {
        cell = Grid.GetCellInDirection(cell, direction);
        if (!Grid.IsValidCell(cell)) return null;

        var            utilityNetworkGridNode = GetGrid(true)[cell];
        UtilityNetwork result                 = null;
        if (utilityNetworkGridNode.networkIdx != -1 && utilityNetworkGridNode.networkIdx < networks.Count)
            result = networks[utilityNetworkGridNode.networkIdx];

        return result;
    }

    public virtual void SetConnections(UtilityConnections connections, int cell, bool is_physical_building) {
        var nodes = GetNodes(is_physical_building);
        nodes.Add(cell);
        visualGrid[cell].connections = connections;
        if (is_physical_building) {
            IsDirty = true;
            var connections2 = is_physical_building
                                   ? connections & GetNeighboursAsConnections(cell, nodes)
                                   : connections;

            physicalGrid[cell].connections = connections2;
        }

        Reconnect(cell);
    }

    public UtilityConnections GetConnections(int cell, bool is_physical_building) {
        var grid               = GetGrid(is_physical_building);
        var utilityConnections = grid[cell].connections;
        if (!is_physical_building) {
            grid               =  GetGrid(true);
            utilityConnections |= grid[cell].connections;
        }

        return utilityConnections;
    }

    public UtilityConnections GetDisplayConnections(int cell) {
        UtilityConnections utilityConnections  = 0;
        var                grid                = GetGrid(false);
        var                utilityConnections2 = utilityConnections | grid[cell].connections;
        grid = GetGrid(true);
        return utilityConnections2 | grid[cell].connections;
    }

    public virtual bool CanAddConnection(UtilityConnections new_connection,
                                         int                cell,
                                         bool               is_physical_building,
                                         out string         fail_reason) {
        fail_reason = null;
        return true;
    }

    public void AddConnection(UtilityConnections new_connection, int cell, bool is_physical_building) {
        string text;
        if (CanAddConnection(new_connection, cell, is_physical_building, out text)) {
            if (is_physical_building) IsDirty = true;
            var grid                          = GetGrid(is_physical_building);
            var connections                   = grid[cell].connections;
            grid[cell].connections = connections | new_connection;
        }
    }

    public void StashVisualGrids()   { Array.Copy(visualGrid,        stashedVisualGrid, visualGrid.Length); }
    public void UnstashVisualGrids() { Array.Copy(stashedVisualGrid, visualGrid,        visualGrid.Length); }

    public string GetVisualizerString(int cell) {
        var displayConnections = GetDisplayConnections(cell);
        return GetVisualizerString(displayConnections);
    }

    public string GetVisualizerString(UtilityConnections connections) {
        var text                                                 = "";
        if ((connections & UtilityConnections.Left)  != 0) text  += "L";
        if ((connections & UtilityConnections.Right) != 0) text  += "R";
        if ((connections & UtilityConnections.Up)    != 0) text  += "U";
        if ((connections & UtilityConnections.Down)  != 0) text  += "D";
        if (text                                     == "") text =  "None";
        return text;
    }

    public object GetEndpoint(int cell) {
        object result = null;
        endpoints.TryGetValue(cell, out result);
        return result;
    }

    public void AddNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener) {
        onNetworksRebuilt
            = (Action<IList<UtilityNetwork>, ICollection<int>>)Delegate.Combine(onNetworksRebuilt, listener);
    }

    public void RemoveNetworksRebuiltListener(Action<IList<UtilityNetwork>, ICollection<int>> listener) {
        onNetworksRebuilt
            = (Action<IList<UtilityNetwork>, ICollection<int>>)Delegate.Remove(onNetworksRebuilt, listener);
    }

    public IList<UtilityNetwork> GetNetworks() { return networks; }

    public void Initialize(int game_width, int game_height) {
        networks.Clear();
        physicalGrid       = new UtilityNetworkGridNode[game_width * game_height];
        visualGrid         = new UtilityNetworkGridNode[game_width * game_height];
        stashedVisualGrid  = new UtilityNetworkGridNode[game_width * game_height];
        physicalNodes      = new HashSet<int>();
        visualNodes        = new HashSet<int>();
        visitedCells       = new HashSet<int>();
        visitedVirtualKeys = new HashSet<object>();
        queuedVirtualKeys  = new HashSet<object>();
        for (var i = 0; i < visualGrid.Length; i++) {
            visualGrid[i]   = new UtilityNetworkGridNode { networkIdx = -1, connections = 0 };
            physicalGrid[i] = new UtilityNetworkGridNode { networkIdx = -1, connections = 0 };
        }
    }

    public void Update() {
        if (IsDirty) {
            IsDirty = false;
            for (var i = 0; i < networks.Count; i++) networks[i].Reset(physicalGrid);
            networks.Clear();
            virtualKeyToNetworkIdx.Clear();
            RebuildNetworks(tileLayer, false);
            RebuildNetworks(tileLayer, true);
            if (onNetworksRebuilt != null) onNetworksRebuilt(networks, GetNodes(true));
        }
    }

    protected UtilityNetworkGridNode[] GetGrid(bool is_physical_building) {
        if (!is_physical_building) return visualGrid;

        return physicalGrid;
    }

    private HashSet<int> GetNodes(bool is_physical_building) {
        if (!is_physical_building) return visualNodes;

        return physicalNodes;
    }

    private void QueueCellForVisit(UtilityNetworkGridNode[] grid, int dest_cell, UtilityConnections direction) {
        if (!Grid.IsValidCell(dest_cell)) return;

        if (visitedCells.Contains(dest_cell)) return;

        if (direction != 0 && (grid[dest_cell].connections & direction.InverseDirection()) == 0) return;

        if (Grid.Objects[dest_cell, tileLayer] != null) {
            visitedCells.Add(dest_cell);
            queued.Enqueue(dest_cell);
        }
    }

    public void AddToVirtualNetworks(object key, object item, bool is_endpoint) {
        if (item != null) {
            if (is_endpoint) {
                if (!virtualEndpoints.ContainsKey(key)) virtualEndpoints[key] = new List<object>();
                virtualEndpoints[key].Add(item);
            } else {
                if (!virtualItems.ContainsKey(key)) virtualItems[key] = new List<object>();
                virtualItems[key].Add(item);
            }
        }

        IsDirty = true;
    }

    private unsafe void Reconnect(int cell) {
        var vector2I = Grid.CellToXY(cell);
        var ptr      = stackalloc int[(UIntPtr)16];
        var ptr2     = stackalloc int[(UIntPtr)16];
        var ptr3     = stackalloc int[(UIntPtr)16];
        var num      = 0;
        if (vector2I.y < Grid.HeightInCells - 1) {
            ptr[num]  = Grid.CellAbove(cell);
            ptr2[num] = 4;
            ptr3[num] = 8;
            num++;
        }

        if (vector2I.y > 0) {
            ptr[num]  = Grid.CellBelow(cell);
            ptr2[num] = 8;
            ptr3[num] = 4;
            num++;
        }

        if (vector2I.x > 0) {
            ptr[num]  = Grid.CellLeft(cell);
            ptr2[num] = 1;
            ptr3[num] = 2;
            num++;
        }

        if (vector2I.x < Grid.WidthInCells - 1) {
            ptr[num]  = Grid.CellRight(cell);
            ptr2[num] = 2;
            ptr3[num] = 1;
            num++;
        }

        var connections  = physicalGrid[cell].connections;
        var connections2 = visualGrid[cell].connections;
        for (var i = 0; i < num; i++) {
            var num2                = ptr[i];
            var utilityConnections  = (UtilityConnections)ptr2[i];
            var utilityConnections2 = (UtilityConnections)ptr3[i];
            if ((connections & utilityConnections) != 0) {
                if (physicalNodes.Contains(num2)) {
                    var array = physicalGrid;
                    var num3  = num2;
                    array[num3].connections = array[num3].connections | utilityConnections2;
                }

                if (visualNodes.Contains(num2)) {
                    var array2 = visualGrid;
                    var num4   = num2;
                    array2[num4].connections = array2[num4].connections | utilityConnections2;
                }
            } else if ((connections2 & utilityConnections) != 0 &&
                       (physicalNodes.Contains(num2) || visualNodes.Contains(num2))) {
                var array3 = visualGrid;
                var num5   = num2;
                array3[num5].connections = array3[num5].connections | utilityConnections2;
            }
        }
    }

    public void RemoveFromVirtualNetworks(object key, object item, bool is_endpoint) {
        if (Game.IsQuitting()) return;

        IsDirty = true;
        if (item != null) {
            if (is_endpoint) {
                virtualEndpoints[key].Remove(item);
                if (virtualEndpoints[key].Count == 0) virtualEndpoints.Remove(key);
            } else {
                virtualItems[key].Remove(item);
                if (virtualItems[key].Count == 0) virtualItems.Remove(key);
            }

            var networkForVirtualKey = GetNetworkForVirtualKey(key);
            if (networkForVirtualKey != null) networkForVirtualKey.RemoveItem(item);
        }
    }

    private unsafe void Disconnect(int cell) {
        var vector2I = Grid.CellToXY(cell);
        var num      = 0;
        var ptr      = stackalloc int[(UIntPtr)16];
        var ptr2     = stackalloc int[(UIntPtr)16];
        if (vector2I.y < Grid.HeightInCells - 1) {
            ptr[num]  = Grid.CellAbove(cell);
            ptr2[num] = -9;
            num++;
        }

        if (vector2I.y > 0) {
            ptr[num]  = Grid.CellBelow(cell);
            ptr2[num] = -5;
            num++;
        }

        if (vector2I.x > 0) {
            ptr[num]  = Grid.CellLeft(cell);
            ptr2[num] = -3;
            num++;
        }

        if (vector2I.x < Grid.WidthInCells - 1) {
            ptr[num]  = Grid.CellRight(cell);
            ptr2[num] = -2;
            num++;
        }

        for (var i = 0; i < num; i++) {
            var num2        = ptr[i];
            var num3        = ptr2[i];
            var connections = (int)(physicalGrid[num2].connections & (UtilityConnections)num3);
            physicalGrid[num2].connections = (UtilityConnections)connections;
        }
    }

    private unsafe void RebuildNetworks(int layer, bool is_physical) {
        var grid  = GetGrid(is_physical);
        var nodes = GetNodes(is_physical);
        visitedCells.Clear();
        visitedVirtualKeys.Clear();
        queuedVirtualKeys.Clear();
        queued.Clear();
        var ptr  = stackalloc int[(UIntPtr)16];
        var ptr2 = stackalloc int[(UIntPtr)16];
        foreach (var num in nodes) {
            var utilityNetworkGridNode = grid[num];
            if (!visitedCells.Contains(num)) {
                queued.Enqueue(num);
                visitedCells.Add(num);
                var networkType = Activator.CreateInstance<NetworkType>();
                networkType.id = networks.Count;
                networks.Add(networkType);
                while (queued.Count > 0) {
                    int num2 = queued.Dequeue();
                    utilityNetworkGridNode = grid[num2];
                    object obj  = null;
                    object obj2 = null;
                    if (is_physical) {
                        if (items.TryGetValue(num2, out obj)) {
                            if (obj is IDisconnectable && (obj as IDisconnectable).IsDisconnected()) continue;

                            if (obj != null) networkType.AddItem(obj);
                        }

                        if (endpoints.TryGetValue(num2, out obj2) && obj2 != null) networkType.AddItem(obj2);
                    }

                    grid[num2].networkIdx = networkType.id;
                    if (obj != null && obj2 != null) networkType.ConnectItem(obj2);
                    var vector2I = Grid.CellToXY(num2);
                    var num3     = 0;
                    if (vector2I.x > 0) {
                        ptr[num3]  = Grid.CellLeft(num2);
                        ptr2[num3] = 1;
                        num3++;
                    }

                    if (vector2I.x < Grid.WidthInCells - 1) {
                        ptr[num3]  = Grid.CellRight(num2);
                        ptr2[num3] = 2;
                        num3++;
                    }

                    if (vector2I.y > 0) {
                        ptr[num3]  = Grid.CellBelow(num2);
                        ptr2[num3] = 8;
                        num3++;
                    }

                    if (vector2I.y < Grid.HeightInCells - 1) {
                        ptr[num3]  = Grid.CellAbove(num2);
                        ptr2[num3] = 4;
                        num3++;
                    }

                    for (var i = 0; i < num3; i++) {
                        var num4 = ptr2[i];
                        if ((utilityNetworkGridNode.connections & (UtilityConnections)num4) != 0) {
                            var dest_cell = ptr[i];
                            QueueCellForVisit(grid, dest_cell, (UtilityConnections)num4);
                        }
                    }

                    int dest_cell2;
                    if (links.TryGetValue(num2, out dest_cell2)) QueueCellForVisit(grid, dest_cell2, 0);
                    object obj3;
                    if (semiVirtualLinks.TryGetValue(num2, out obj3) && !visitedVirtualKeys.Contains(obj3)) {
                        visitedVirtualKeys.Add(obj3);
                        virtualKeyToNetworkIdx[obj3] = networkType.id;
                        if (virtualItems.ContainsKey(obj3))
                            foreach (var item in virtualItems[obj3]) {
                                networkType.AddItem(item);
                                networkType.ConnectItem(item);
                            }

                        if (virtualEndpoints.ContainsKey(obj3))
                            foreach (var item2 in virtualEndpoints[obj3]) {
                                networkType.AddItem(item2);
                                networkType.ConnectItem(item2);
                            }

                        foreach (var keyValuePair in semiVirtualLinks)
                            if (keyValuePair.Value == obj3)
                                QueueCellForVisit(grid, keyValuePair.Key, 0);
                    }
                }
            }
        }

        foreach (var keyValuePair2 in virtualItems)
            if (!visitedVirtualKeys.Contains(keyValuePair2.Key)) {
                var networkType2 = Activator.CreateInstance<NetworkType>();
                networkType2.id = networks.Count;
                visitedVirtualKeys.Add(keyValuePair2.Key);
                virtualKeyToNetworkIdx[keyValuePair2.Key] = networkType2.id;
                foreach (var item3 in keyValuePair2.Value) {
                    networkType2.AddItem(item3);
                    networkType2.ConnectItem(item3);
                }

                foreach (var item4 in virtualEndpoints[keyValuePair2.Key]) {
                    networkType2.AddItem(item4);
                    networkType2.ConnectItem(item4);
                }

                networks.Add(networkType2);
            }

        foreach (var keyValuePair3 in virtualEndpoints)
            if (!visitedVirtualKeys.Contains(keyValuePair3.Key)) {
                var networkType3 = Activator.CreateInstance<NetworkType>();
                networkType3.id = networks.Count;
                visitedVirtualKeys.Add(keyValuePair3.Key);
                virtualKeyToNetworkIdx[keyValuePair3.Key] = networkType3.id;
                foreach (var item5 in virtualEndpoints[keyValuePair3.Key]) {
                    networkType3.AddItem(item5);
                    networkType3.ConnectItem(item5);
                }

                networks.Add(networkType3);
            }
    }

    public UtilityNetwork GetNetworkForVirtualKey(object key) {
        int index;
        if (virtualKeyToNetworkIdx.TryGetValue(key, out index)) return networks[index];

        return null;
    }

    public UtilityNetwork GetNetworkByID(int id) {
        UtilityNetwork result                      = null;
        if (0 <= id && id < networks.Count) result = networks[id];
        return result;
    }

    private UtilityConnections GetNeighboursAsConnections(int cell, HashSet<int> nodes) {
        UtilityConnections utilityConnections                                         = 0;
        var                vector2I                                                   = Grid.CellToXY(cell);
        if (vector2I.x > 0 && nodes.Contains(Grid.CellLeft(cell))) utilityConnections |= UtilityConnections.Left;
        if (vector2I.x < Grid.WidthInCells - 1 && nodes.Contains(Grid.CellRight(cell)))
            utilityConnections |= UtilityConnections.Right;

        if (vector2I.y > 0 && nodes.Contains(Grid.CellBelow(cell))) utilityConnections |= UtilityConnections.Down;
        if (vector2I.y < Grid.HeightInCells - 1 && nodes.Contains(Grid.CellAbove(cell)))
            utilityConnections |= UtilityConnections.Up;

        return utilityConnections;
    }

    public void AddSemiVirtualLink(int cell1, object virtualKey) {
        Debug.Assert(virtualKey != null, "Can not use a null key for a virtual network");
        semiVirtualLinks[cell1] = virtualKey;
        IsDirty                 = true;
    }

    public void RemoveSemiVirtualLink(int cell1, object virtualKey) {
        Debug.Assert(virtualKey != null, "Can not use a null key for a virtual network");
        semiVirtualLinks.Remove(cell1);
        IsDirty = true;
    }

    public void AddLink(int cell1, int cell2) {
        links[cell1] = cell2;
        links[cell2] = cell1;
        IsDirty      = true;
    }

    public void RemoveLink(int cell1, int cell2) {
        links.Remove(cell1);
        links.Remove(cell2);
        IsDirty = true;
    }
}