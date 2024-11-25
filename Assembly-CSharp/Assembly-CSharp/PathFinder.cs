using System;
using System.Collections.Generic;
using System.Diagnostics;

public class PathFinder {
    public static int      InvalidHandle = -1;
    public static int      InvalidIdx    = -1;
    public static int      InvalidCell   = -1;
    public static PathGrid PathGrid;

    private static readonly Func<int, bool> allowPathfindingFloodFillCb = delegate(int cell) {
                                                                              if (Grid.Solid[cell]) return false;

                                                                              if (Grid.AllowPathfinding[cell])
                                                                                  return false;

                                                                              Grid.AllowPathfinding[cell] = true;
                                                                              return true;
                                                                          };

    public static void Initialize() {
        var array                                       = new NavType[11];
        for (var i = 0; i < array.Length; i++) array[i] = (NavType)i;
        PathGrid = new PathGrid(Grid.WidthInCells, Grid.HeightInCells, false, array);
        for (var j = 0; j < Grid.CellCount; j++)
            if (Grid.Visible[j] > 0 || Grid.Spawnable[j] > 0) {
                var pooledList = ListPool<int, PathFinder>.Allocate();
                GameUtil.FloodFillConditional(j, allowPathfindingFloodFillCb, pooledList);
                Grid.AllowPathfinding[j] = true;
                pooledList.Recycle();
            }

        Grid.OnReveal = (Action<int>)Delegate.Combine(Grid.OnReveal, new Action<int>(OnReveal));
    }

    private static void OnReveal(int cell) { }

    public static void UpdatePath(NavGrid             nav_grid,
                                  PathFinderAbilities abilities,
                                  PotentialPath       potential_path,
                                  PathFinderQuery     query,
                                  ref Path            path) {
        Run(nav_grid, abilities, potential_path, query, ref path);
    }

    public static bool ValidatePath(NavGrid nav_grid, PathFinderAbilities abilities, ref Path path) {
        if (!path.IsValid()) return false;

        for (var i = 0; i < path.nodes.Count; i++) {
            var node = path.nodes[i];
            if (i < path.nodes.Count - 1) {
                var node2 = path.nodes[i + 1];
                var num   = node.cell * nav_grid.maxLinksPerCell;
                var flag  = false;
                var link  = nav_grid.Links[num];
                while (link.link != InvalidHandle) {
                    if (link.link     == node2.cell      &&
                        node2.navType == link.endNavType &&
                        node.navType  == link.startNavType) {
                        var potentialPath = new PotentialPath(node.cell, node.navType, PotentialPath.Flags.None);
                        flag = abilities.TraversePath(ref potentialPath,
                                                      node.cell,
                                                      node.navType,
                                                      0,
                                                      link.transitionId,
                                                      false);

                        if (flag) break;
                    }

                    num++;
                    link = nav_grid.Links[num];
                }

                if (!flag) return false;
            }
        }

        return true;
    }

    public static void Run(NavGrid             nav_grid,
                           PathFinderAbilities abilities,
                           PotentialPath       potential_path,
                           PathFinderQuery     query) {
        var invalidCell = InvalidCell;
        var nav_type    = NavType.NumNavTypes;
        query.ClearResult();
        if (!Grid.IsValidCell(potential_path.cell)) return;

        FindPaths(nav_grid, ref abilities, potential_path, query, Temp.Potentials, ref invalidCell, ref nav_type);
        if (invalidCell != InvalidCell) {
            var flag = false;
            var cell = PathGrid.GetCell(invalidCell, nav_type, out flag);
            query.SetResult(invalidCell, cell.cost, nav_type);
        }
    }

    public static void Run(NavGrid             nav_grid,
                           PathFinderAbilities abilities,
                           PotentialPath       potential_path,
                           PathFinderQuery     query,
                           ref Path            path) {
        Run(nav_grid, abilities, potential_path, query);
        if (query.GetResultCell() != InvalidCell) {
            BuildResultPath(query.GetResultCell(), query.GetResultNavType(), ref path);
            return;
        }

        path.Clear();
    }

    private static void BuildResultPath(int path_cell, NavType path_nav_type, ref Path path) {
        if (path_cell != InvalidCell) {
            var flag = false;
            var cell = PathGrid.GetCell(path_cell, path_nav_type, out flag);
            path.Clear();
            path.cost = cell.cost;
            while (path_cell != InvalidCell) {
                path.AddNode(new Path.Node {
                    cell = path_cell, navType = cell.navType, transitionId = cell.transitionId
                });

                path_cell = cell.parent;
                if (path_cell != InvalidCell) cell = PathGrid.GetCell(path_cell, cell.parentNavType, out flag);
            }

            if (path.nodes != null)
                for (var i = 0; i < path.nodes.Count / 2; i++) {
                    var value = path.nodes[i];
                    path.nodes[i] = path.nodes[path.nodes.Count - i - 1];
                    path.nodes[path.nodes.Count                 - i - 1] = value;
                }
        }
    }

    private static void FindPaths(NavGrid                 nav_grid,
                                  ref PathFinderAbilities abilities,
                                  PotentialPath           potential_path,
                                  PathFinderQuery         query,
                                  PotentialList           potentials,
                                  ref int                 result_cell,
                                  ref NavType             result_nav_type) {
        potentials.Clear();
        PathGrid.ResetUpdate();
        PathGrid.BeginUpdate(potential_path.cell, false);
        bool flag;
        var  cell = PathGrid.GetCell(potential_path, out flag);
        AddPotential(potential_path,
                     Grid.InvalidCell,
                     NavType.NumNavTypes,
                     0,
                     0,
                     potentials,
                     PathGrid,
                     ref cell);

        var num = int.MaxValue;
        while (potentials.Count > 0) {
            var keyValuePair = potentials.Next();
            cell = PathGrid.GetCell(keyValuePair.Value, out flag);
            if (cell.cost == keyValuePair.Key) {
                if (cell.navType != NavType.Tube                                   &&
                    query.IsMatch(keyValuePair.Value.cell, cell.parent, cell.cost) &&
                    cell.cost < num) {
                    result_cell     = keyValuePair.Value.cell;
                    num             = cell.cost;
                    result_nav_type = cell.navType;
                    break;
                }

                AddPotentials(nav_grid.potentialScratchPad,
                              keyValuePair.Value,
                              cell.cost,
                              ref abilities,
                              query,
                              nav_grid.maxLinksPerCell,
                              nav_grid.Links,
                              potentials,
                              PathGrid,
                              cell.parent,
                              cell.parentNavType);
            }
        }

        PathGrid.EndUpdate(true);
    }

    public static void AddPotential(PotentialPath potential_path,
                                    int           parent_cell,
                                    NavType       parent_nav_type,
                                    int           cost,
                                    byte          transition_id,
                                    PotentialList potentials,
                                    PathGrid      path_grid,
                                    ref Cell      cell_data) {
        cell_data.cost   = cost;
        cell_data.parent = parent_cell;
        cell_data.SetNavTypes(potential_path.navType, parent_nav_type);
        cell_data.transitionId = transition_id;
        potentials.Add(cost, potential_path);
        path_grid.SetCell(potential_path, ref cell_data);
    }

    [Conditional("ENABLE_PATH_DETAILS")]
    private static void BeginDetailSample(string region_name) { }

    [Conditional("ENABLE_PATH_DETAILS")]
    private static void EndDetailSample(string region_name) { }

    public static bool IsSubmerged(int cell) {
        if (!Grid.IsValidCell(cell)) return false;

        var num = Grid.CellAbove(cell);
        return (Grid.IsValidCell(num)       && Grid.Element[num].IsLiquid) ||
               (Grid.Element[cell].IsLiquid && Grid.IsValidCell(num) && Grid.Solid[num]);
    }

    public static void AddPotentials(PotentialScratchPad     potential_scratch_pad,
                                     PotentialPath           potential,
                                     int                     cost,
                                     ref PathFinderAbilities abilities,
                                     PathFinderQuery         query,
                                     int                     max_links_per_cell,
                                     NavGrid.Link[]          links,
                                     PotentialList           potentials,
                                     PathGrid                path_grid,
                                     int                     parent_cell,
                                     NavType                 parent_nav_type) {
        if (!Grid.IsValidCell(potential.cell)) return;

        var num                     = 0;
        var linksWithCorrectNavType = potential_scratch_pad.linksWithCorrectNavType;
        var num2                    = potential.cell * max_links_per_cell;
        var link                    = links[num2];
        for (var link2 = link.link; link2 != InvalidHandle; link2 = link.link) {
            if (link.startNavType == potential.navType &&
                (parent_cell != link2 || parent_nav_type != link.startNavType))
                linksWithCorrectNavType[num++] = link;

            num2++;
            link = links[num2];
        }

        var num3             = 0;
        var linksInCellRange = potential_scratch_pad.linksInCellRange;
        for (var i = 0; i < num; i++) {
            var link3 = linksWithCorrectNavType[i];
            var link4 = link3.link;
            var flag  = false;
            var cell  = path_grid.GetCell(link4, link3.endNavType, out flag);
            if (flag) {
                var num4  = cost + link3.cost;
                var flag2 = cell.cost == -1;
                var flag3 = num4      < cell.cost;
                if (flag2 || flag3)
                    linksInCellRange[num3++]
                        = new PotentialScratchPad.PathGridCellData { pathGridCell = cell, link = link3 };
            }
        }

        for (var j = 0; j < num3; j++) {
            var pathGridCellData = linksInCellRange[j];
            var link5            = pathGridCellData.link.link;
            pathGridCellData.isSubmerged = IsSubmerged(link5);
            linksInCellRange[j]          = pathGridCellData;
        }

        for (var k = 0; k < num3; k++) {
            var pathGridCellData2 = linksInCellRange[k];
            var link6             = pathGridCellData2.link;
            var link7             = link6.link;
            var pathGridCell      = pathGridCellData2.pathGridCell;
            var num5              = cost + link6.cost;
            var potentialPath     = potential;
            potentialPath.cell    = link7;
            potentialPath.navType = link6.endNavType;
            if (pathGridCellData2.isSubmerged) {
                var submergedPathCostPenalty = abilities.GetSubmergedPathCostPenalty(potentialPath, link6);
                num5 += submergedPathCostPenalty;
            }

            var flags = potentialPath.flags;
            var flag4 = abilities.TraversePath(ref potentialPath,
                                               potential.cell,
                                               potential.navType,
                                               num5,
                                               link6.transitionId,
                                               pathGridCellData2.isSubmerged);

            var flags2 = potentialPath.flags;
            if (flag4)
                AddPotential(potentialPath,
                             potential.cell,
                             potential.navType,
                             num5,
                             link6.transitionId,
                             potentials,
                             path_grid,
                             ref pathGridCell);
        }
    }

    public static void DestroyStatics() {
        PathGrid.OnCleanUp();
        PathGrid = null;
        Temp.Potentials.Clear();
    }

    public struct Cell {
        public  NavType navType                                        => (NavType)(navTypes & 15);
        public  NavType parentNavType                                  => (NavType)(navTypes >> 4);
        public  void    SetNavTypes(NavType type, NavType parent_type) { navTypes = (byte)(type | (parent_type << 4)); }
        public  int     cost;
        public  int     parent;
        public  short   queryId;
        private byte    navTypes;
        public  byte    transitionId;
    }

    public struct PotentialPath {
        public PotentialPath(int cell, NavType nav_type, Flags flags) {
            this.cell  = cell;
            navType    = nav_type;
            this.flags = flags;
        }

        public void    SetFlags(Flags   new_flags) { flags |= new_flags; }
        public void    ClearFlags(Flags new_flags) { flags &= ~new_flags; }
        public bool    HasFlag(Flags    flag)      { return HasAnyFlag(flag); }
        public bool    HasAnyFlag(Flags mask)      { return (flags & mask) > Flags.None; }
        public Flags   flags                       { readonly get; private set; }
        public int     cell;
        public NavType navType;

        [Flags]
        public enum Flags : byte {
            None              = 0,
            HasAtmoSuit       = 1,
            HasJetPack        = 2,
            HasOxygenMask     = 4,
            PerformSuitChecks = 8,
            HasLeadSuit       = 16
        }
    }

    public struct Path {
        public void AddNode(Node node) {
            if (nodes == null) nodes = new List<Node>();
            nodes.Add(node);
        }

        public bool IsValid()    { return nodes != null && nodes.Count > 1; }
        public bool HasArrived() { return nodes != null && nodes.Count > 0; }

        public void Clear() {
            cost = 0;
            if (nodes != null) nodes.Clear();
        }

        public int        cost;
        public List<Node> nodes;

        public struct Node {
            public int     cell;
            public NavType navType;
            public byte    transitionId;
        }
    }

    public class PotentialList {
        private readonly HOTQueue<PotentialPath> queue = new HOTQueue<PotentialPath>();
        public           int Count => queue.Count;
        public           KeyValuePair<int, PotentialPath> Next() { return queue.Dequeue(); }
        public           void Add(int cost, PotentialPath path) { queue.Enqueue(cost, path); }
        public           void Clear() { queue.Clear(); }

        public class PriorityQueue<TValue> {
            private readonly List<KeyValuePair<int, TValue>> _baseHeap;
            public PriorityQueue() { _baseHeap = new List<KeyValuePair<int, TValue>>(); }
            public int  Count                               => _baseHeap.Count;
            public void Enqueue(int priority, TValue value) { Insert(priority, value); }

            public KeyValuePair<int, TValue> Dequeue() {
                var result = _baseHeap[0];
                DeleteRoot();
                return result;
            }

            public KeyValuePair<int, TValue> Peek() {
                if (Count > 0) return _baseHeap[0];

                throw new InvalidOperationException("Priority queue is empty");
            }

            private void ExchangeElements(int pos1, int pos2) {
                var value = _baseHeap[pos1];
                _baseHeap[pos1] = _baseHeap[pos2];
                _baseHeap[pos2] = value;
            }

            private void Insert(int priority, TValue value) {
                var item = new KeyValuePair<int, TValue>(priority, value);
                _baseHeap.Add(item);
                HeapifyFromEndToBeginning(_baseHeap.Count - 1);
            }

            private int HeapifyFromEndToBeginning(int pos) {
                if (pos >= _baseHeap.Count) return -1;

                while (pos > 0) {
                    var num = (pos - 1) / 2;
                    if (_baseHeap[num].Key - _baseHeap[pos].Key <= 0) break;

                    ExchangeElements(num, pos);
                    pos = num;
                }

                return pos;
            }

            private void DeleteRoot() {
                if (_baseHeap.Count <= 1) {
                    _baseHeap.Clear();
                    return;
                }

                _baseHeap[0] = _baseHeap[_baseHeap.Count - 1];
                _baseHeap.RemoveAt(_baseHeap.Count - 1);
                HeapifyFromBeginningToEnd(0);
            }

            private void HeapifyFromBeginningToEnd(int pos) {
                var count = _baseHeap.Count;
                if (pos >= count) return;

                for (;;) {
                    var num                                                               = pos;
                    var num2                                                              = 2 * pos + 1;
                    var num3                                                              = 2 * pos + 2;
                    if (num2 < count && _baseHeap[num].Key - _baseHeap[num2].Key > 0) num = num2;
                    if (num3 < count && _baseHeap[num].Key - _baseHeap[num3].Key > 0) num = num3;
                    if (num == pos) break;

                    ExchangeElements(num, pos);
                    pos = num;
                }
            }

            public void Clear() { _baseHeap.Clear(); }
        }

        private class HOTQueue<TValue> {
            private PriorityQueue<TValue> coldQueue     = new PriorityQueue<TValue>();
            private int                   coldThreshold = int.MinValue;
            private PriorityQueue<TValue> hotQueue      = new PriorityQueue<TValue>();
            private int                   hotThreshold  = int.MinValue;
            public  int                   Count { get; private set; }

            public KeyValuePair<int, TValue> Dequeue() {
                if (hotQueue.Count == 0) {
                    var priorityQueue = hotQueue;
                    hotQueue     = coldQueue;
                    coldQueue    = priorityQueue;
                    hotThreshold = coldThreshold;
                }

                Count--;
                return hotQueue.Dequeue();
            }

            public void Enqueue(int priority, TValue value) {
                if (priority <= hotThreshold)
                    hotQueue.Enqueue(priority, value);
                else {
                    coldQueue.Enqueue(priority, value);
                    coldThreshold = Math.Max(coldThreshold, priority);
                }

                Count++;
            }

            public KeyValuePair<int, TValue> Peek() {
                if (hotQueue.Count == 0) {
                    var priorityQueue = hotQueue;
                    hotQueue     = coldQueue;
                    coldQueue    = priorityQueue;
                    hotThreshold = coldThreshold;
                }

                return hotQueue.Peek();
            }

            public void Clear() {
                Count        = 0;
                hotThreshold = int.MinValue;
                hotQueue.Clear();
                coldThreshold = int.MinValue;
                coldQueue.Clear();
            }
        }
    }

    private class Temp {
        public static readonly PotentialList Potentials = new PotentialList();
    }

    public class PotentialScratchPad {
        public PathGridCellData[] linksInCellRange;
        public NavGrid.Link[]     linksWithCorrectNavType;

        public PotentialScratchPad(int max_links_per_cell) {
            linksWithCorrectNavType = new NavGrid.Link[max_links_per_cell];
            linksInCellRange        = new PathGridCellData[max_links_per_cell];
        }

        public struct PathGridCellData {
            public Cell         pathGridCell;
            public NavGrid.Link link;
            public bool         isSubmerged;
        }
    }
}