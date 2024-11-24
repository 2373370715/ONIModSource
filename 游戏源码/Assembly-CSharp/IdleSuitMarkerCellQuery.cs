public class IdleSuitMarkerCellQuery : PathFinderQuery
{
	private int targetCell;

	private bool isRotated;

	private int markerX;

	public IdleSuitMarkerCellQuery(bool is_rotated, int marker_x)
	{
		targetCell = Grid.InvalidCell;
		isRotated = is_rotated;
		markerX = marker_x;
	}

	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!Grid.PreventIdleTraversal[cell] && Grid.CellToXY(cell).x < markerX != isRotated)
		{
			targetCell = cell;
		}
		return targetCell != Grid.InvalidCell;
	}

	public override int GetResultCell()
	{
		return targetCell;
	}
}
