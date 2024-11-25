using UnityEngine;

public class BalloonStandCellSensor : Sensor {
    private readonly MinionBrain brain;
    private          int         cell;
    private readonly Navigator   navigator;
    private          int         standCell;

    public BalloonStandCellSensor(Sensors sensors) : base(sensors) {
        navigator = GetComponent<Navigator>();
        brain     = GetComponent<MinionBrain>();
    }

    public override void Update() {
        cell = Grid.InvalidCell;
        var num        = int.MaxValue;
        var pooledList = ListPool<int[], BalloonStandCellSensor>.Allocate();
        var num2       = 50;
        foreach (var num3 in Game.Instance.mingleCellTracker.mingleCells)
            if (brain.IsCellClear(num3)) {
                var navigationCost = navigator.GetNavigationCost(num3);
                if (navigationCost != -1) {
                    if (num3 == Grid.InvalidCell || navigationCost < num) {
                        cell = num3;
                        num  = navigationCost;
                    }

                    if (navigationCost < num2) {
                        var num4           = Grid.CellRight(num3);
                        var num5           = Grid.CellRight(num4);
                        var num6           = Grid.CellLeft(num3);
                        var num7           = Grid.CellLeft(num6);
                        var cavityForCell  = Game.Instance.roomProber.GetCavityForCell(cell);
                        var cavityForCell2 = Game.Instance.roomProber.GetCavityForCell(num7);
                        var cavityForCell3 = Game.Instance.roomProber.GetCavityForCell(num5);
                        if (cavityForCell != null) {
                            if (cavityForCell3        != null                 &&
                                cavityForCell3.handle == cavityForCell.handle &&
                                navigator.NavGrid.NavTable.IsValid(num4)      &&
                                navigator.NavGrid.NavTable.IsValid(num5))
                                pooledList.Add(new[] { num3, num5 });

                            if (cavityForCell2        != null                 &&
                                cavityForCell2.handle == cavityForCell.handle &&
                                navigator.NavGrid.NavTable.IsValid(num6)      &&
                                navigator.NavGrid.NavTable.IsValid(num7))
                                pooledList.Add(new[] { num3, num7 });
                        }
                    }
                }
            }

        if (pooledList.Count > 0) {
            var array = pooledList[Random.Range(0, pooledList.Count)];
            cell      = array[0];
            standCell = array[1];
        } else if (Components.Telepads.Count > 0) {
            var telepad = Components.Telepads.Items[0];
            if (telepad == null || !telepad.GetComponent<Operational>().IsOperational) return;

            var num8 = Grid.PosToCell(telepad.transform.GetPosition());
            num8 = Grid.CellLeft(num8);
            var num9           = Grid.CellRight(num8);
            var num10          = Grid.CellRight(num9);
            var cavityForCell4 = Game.Instance.roomProber.GetCavityForCell(num8) != null;
            var cavityForCell5 = Game.Instance.roomProber.GetCavityForCell(num10);
            if (cavityForCell4                           &&
                cavityForCell5 != null                   &&
                navigator.NavGrid.NavTable.IsValid(num8) &&
                navigator.NavGrid.NavTable.IsValid(num9) &&
                navigator.NavGrid.NavTable.IsValid(num10)) {
                cell      = num8;
                standCell = num10;
            }
        }

        pooledList.Recycle();
    }

    public int GetCell()      { return cell; }
    public int GetStandCell() { return standCell; }
}