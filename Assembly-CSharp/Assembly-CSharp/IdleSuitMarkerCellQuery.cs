public class IdleSuitMarkerCellQuery : PathFinderQuery {
    private readonly bool isRotated;
    private readonly int  markerX;
    private          int  targetCell;

    public IdleSuitMarkerCellQuery(bool is_rotated, int marker_x) {
        targetCell = Grid.InvalidCell;
        isRotated  = is_rotated;
        markerX    = marker_x;
    }

    public override bool IsMatch(int cell, int parent_cell, int cost) {
        if (!Grid.PreventIdleTraversal[cell] && Grid.CellToXY(cell).x < markerX != isRotated) targetCell = cell;
        return targetCell != Grid.InvalidCell;
    }

    public override int GetResultCell() { return targetCell; }
}