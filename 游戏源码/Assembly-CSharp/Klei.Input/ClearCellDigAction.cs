using Klei.Actions;

namespace Klei.Input;

[Action("Clear Cell")]
public class ClearCellDigAction : DigAction
{
	public override void Dig(int cell, int distFromOrigin)
	{
		if (Grid.Solid[cell] && !Grid.Foundation[cell])
		{
			SimMessages.Dig(cell, -1, skipEvent: true);
		}
	}

	protected override void EntityDig(IDigActionEntity digActionEntity)
	{
		digActionEntity?.Dig();
	}
}
