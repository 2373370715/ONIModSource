using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/GridVisibility")]
public class GridVisibility : KMonoBehaviour {
    public float innerRadius = 16.5f;
    public int   radius      = 18;

    protected override void OnSpawn() {
        Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(transform,
                                                                         OnCellChange,
                                                                         "GridVisibility.OnSpawn");

        OnCellChange();
        var myWorld = gameObject.GetMyWorld();
        if (myWorld != null && !gameObject.HasTag(GameTags.Stored)) myWorld.SetDiscovered();
    }

    private void OnCellChange() {
        if (gameObject.HasTag(GameTags.Dead)) return;

        var num = Grid.PosToCell(this);
        if (!Grid.IsValidCell(num)) return;

        if (!Grid.Revealed[num]) {
            int baseX;
            int baseY;
            Grid.PosToXY(transform.GetPosition(), out baseX, out baseY);
            Reveal(baseX, baseY, radius, innerRadius);
            Grid.Revealed[num] = true;
        }

        FogOfWarMask.ClearMask(num);
    }

    public static void Reveal(int baseX, int baseY, int radius, float innerRadius) {
        int num = Grid.WorldIdx[baseY * Grid.WidthInCells + baseX];
        for (var i = -radius; i <= radius; i++) {
            for (var j = -radius; j <= radius; j++) {
                var num2 = baseY + i;
                var num3 = baseX + j;
                if (num2 >= 0 && Grid.HeightInCells - 1 >= num2 && num3 >= 0 && Grid.WidthInCells - 1 >= num3) {
                    var num4 = num2 * Grid.WidthInCells + num3;
                    if (Grid.Visible[num4] < 255 && num == Grid.WorldIdx[num4]) {
                        var vector = new Vector2(j, i);
                        var num5   = Mathf.Lerp(1f, 0f, (vector.magnitude - innerRadius) / (radius - innerRadius));
                        Grid.Reveal(num4, (byte)(255f * num5));
                    }
                }
            }
        }
    }

    protected override void OnCleanUp() {
        Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(transform, OnCellChange);
    }
}