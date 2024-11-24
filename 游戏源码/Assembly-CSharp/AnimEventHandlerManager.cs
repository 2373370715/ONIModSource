using System.Collections.Generic;

public class AnimEventHandlerManager : KMonoBehaviour
{
	private const float HIDE_DISTANCE = 40f;

	private List<AnimEventHandler> handlers;

	public static AnimEventHandlerManager Instance { get; private set; }

	public static void DestroyInstance()
	{
		Instance = null;
	}

	protected override void OnPrefabInit()
	{
		Instance = this;
		handlers = new List<AnimEventHandler>();
	}

	public void Add(AnimEventHandler handler)
	{
		handlers.Add(handler);
	}

	public void Remove(AnimEventHandler handler)
	{
		handlers.Remove(handler);
	}

	private bool IsVisibleToZoom()
	{
		if (Game.MainCamera == null)
		{
			return false;
		}
		return Game.MainCamera.orthographicSize < 40f;
	}

	public void LateUpdate()
	{
		if (!IsVisibleToZoom())
		{
			return;
		}
		Grid.GetVisibleCellRangeInActiveWorld(out var min, out var max);
		foreach (AnimEventHandler handler in handlers)
		{
			if (IsVisible(handler))
			{
				handler.UpdateOffset();
			}
		}
		bool IsVisible(AnimEventHandler handler)
		{
			Grid.CellToXY(handler.GetCachedCell(), out var x, out var y);
			if (x >= min.x && y >= min.y && x < max.x)
			{
				return y < max.y;
			}
			return false;
		}
	}
}
