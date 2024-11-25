using System.Collections.Generic;

public class DoorTransitionLayer : TransitionDriver.InterruptOverrideLayer {
    private readonly List<INavDoor> doors = new List<INavDoor>();
    public DoorTransitionLayer(Navigator navigator) : base(navigator) { }

    private bool AreAllDoorsOpen() {
        foreach (var navDoor in doors)
            if (navDoor != null && !navDoor.IsOpen())
                return false;

        return true;
    }

    protected override bool IsOverrideComplete() { return base.IsOverrideComplete() && AreAllDoorsOpen(); }

    public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition) {
        if (doors.Count > 0) return;

        var cell  = Grid.PosToCell(navigator);
        var cell2 = Grid.OffsetCell(cell, transition.x, transition.y);
        AddDoor(cell2);
        if (navigator.CurrentNavType != NavType.Tube) AddDoor(Grid.CellAbove(cell2));
        for (var i = 0; i < transition.navGridTransition.voidOffsets.Length; i++) {
            var cell3 = Grid.OffsetCell(cell, transition.navGridTransition.voidOffsets[i]);
            AddDoor(cell3);
        }

        if (doors.Count == 0) return;

        if (!AreAllDoorsOpen()) {
            base.BeginTransition(navigator, transition);
            transition.anim  = navigator.NavGrid.GetIdleAnim(navigator.CurrentNavType);
            transition.start = originalTransition.start;
            transition.end   = originalTransition.start;
        }

        foreach (var navDoor in doors) navDoor.Open();
    }

    public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition) {
        base.EndTransition(navigator, transition);
        if (doors.Count == 0) return;

        foreach (var navDoor in doors)
            if (!navDoor.IsNullOrDestroyed())
                navDoor.Close();

        doors.Clear();
    }

    private void AddDoor(int cell) {
        var door = GetDoor(cell);
        if (!door.IsNullOrDestroyed() && !doors.Contains(door)) doors.Add(door);
    }

    private INavDoor GetDoor(int cell) {
        if (!Grid.HasDoor[cell]) return null;

        var gameObject = Grid.Objects[cell, 1];
        if (gameObject != null) {
            var navDoor                  = gameObject.GetComponent<INavDoor>();
            if (navDoor == null) navDoor = gameObject.GetSMI<INavDoor>();
            if (navDoor != null && navDoor.isSpawned) return navDoor;
        }

        return null;
    }
}