using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020013D4 RID: 5076
public class GridVisibleArea
{
	// Token: 0x17000697 RID: 1687
	// (get) Token: 0x06006800 RID: 26624 RVA: 0x000E41AB File Offset: 0x000E23AB
	public GridArea CurrentArea
	{
		get
		{
			return this.VisibleAreas[0];
		}
	}

	// Token: 0x17000698 RID: 1688
	// (get) Token: 0x06006801 RID: 26625 RVA: 0x000E41B9 File Offset: 0x000E23B9
	public GridArea PreviousArea
	{
		get
		{
			return this.VisibleAreas[1];
		}
	}

	// Token: 0x17000699 RID: 1689
	// (get) Token: 0x06006802 RID: 26626 RVA: 0x000E41C7 File Offset: 0x000E23C7
	public GridArea PreviousPreviousArea
	{
		get
		{
			return this.VisibleAreas[2];
		}
	}

	// Token: 0x1700069A RID: 1690
	// (get) Token: 0x06006803 RID: 26627 RVA: 0x000E41D5 File Offset: 0x000E23D5
	public GridArea CurrentAreaExtended
	{
		get
		{
			return this.VisibleAreasExtended[0];
		}
	}

	// Token: 0x1700069B RID: 1691
	// (get) Token: 0x06006804 RID: 26628 RVA: 0x000E41E3 File Offset: 0x000E23E3
	public GridArea PreviousAreaExtended
	{
		get
		{
			return this.VisibleAreasExtended[1];
		}
	}

	// Token: 0x1700069C RID: 1692
	// (get) Token: 0x06006805 RID: 26629 RVA: 0x000E41F1 File Offset: 0x000E23F1
	public GridArea PreviousPreviousAreaExtended
	{
		get
		{
			return this.VisibleAreasExtended[2];
		}
	}

	// Token: 0x06006806 RID: 26630 RVA: 0x000E41FF File Offset: 0x000E23FF
	public GridVisibleArea()
	{
	}

	// Token: 0x06006807 RID: 26631 RVA: 0x000E422A File Offset: 0x000E242A
	public GridVisibleArea(int padding)
	{
		this._padding = padding;
	}

	// Token: 0x06006808 RID: 26632 RVA: 0x002D5F64 File Offset: 0x002D4164
	public void Update()
	{
		if (!this.debugFreezeVisibleArea)
		{
			this.VisibleAreas[2] = this.VisibleAreas[1];
			this.VisibleAreas[1] = this.VisibleAreas[0];
			this.VisibleAreas[0] = GridVisibleArea.GetVisibleArea();
		}
		if (!this.debugFreezeVisibleAreasExtended)
		{
			this.VisibleAreasExtended[2] = this.VisibleAreasExtended[1];
			this.VisibleAreasExtended[1] = this.VisibleAreasExtended[0];
			this.VisibleAreasExtended[0] = GridVisibleArea.GetVisibleAreaExtended(this._padding);
		}
		foreach (GridVisibleArea.Callback callback in this.Callbacks)
		{
			callback.OnUpdate();
		}
	}

	// Token: 0x06006809 RID: 26633 RVA: 0x002D6054 File Offset: 0x002D4254
	public void AddCallback(string name, System.Action on_update)
	{
		GridVisibleArea.Callback item = new GridVisibleArea.Callback
		{
			Name = name,
			OnUpdate = on_update
		};
		this.Callbacks.Add(item);
	}

	// Token: 0x0600680A RID: 26634 RVA: 0x002D6088 File Offset: 0x002D4288
	public void Run(Action<int> in_view)
	{
		if (in_view != null)
		{
			this.CurrentArea.Run(in_view);
		}
	}

	// Token: 0x0600680B RID: 26635 RVA: 0x002D60A8 File Offset: 0x002D42A8
	public void RunExtended(Action<int> in_view)
	{
		if (in_view != null)
		{
			this.CurrentAreaExtended.Run(in_view);
		}
	}

	// Token: 0x0600680C RID: 26636 RVA: 0x002D60C8 File Offset: 0x002D42C8
	public void Run(Action<int> outside_view, Action<int> inside_view, Action<int> inside_view_second_time)
	{
		if (outside_view != null)
		{
			this.PreviousArea.RunOnDifference(this.CurrentArea, outside_view);
		}
		if (inside_view != null)
		{
			this.CurrentArea.RunOnDifference(this.PreviousArea, inside_view);
		}
		if (inside_view_second_time != null)
		{
			this.PreviousArea.RunOnDifference(this.PreviousPreviousArea, inside_view_second_time);
		}
	}

	// Token: 0x0600680D RID: 26637 RVA: 0x002D6120 File Offset: 0x002D4320
	public void RunExtended(Action<int> outside_view, Action<int> inside_view, Action<int> inside_view_second_time)
	{
		if (outside_view != null)
		{
			this.PreviousAreaExtended.RunOnDifference(this.CurrentAreaExtended, outside_view);
		}
		if (inside_view != null)
		{
			this.CurrentAreaExtended.RunOnDifference(this.PreviousAreaExtended, inside_view);
		}
		if (inside_view_second_time != null)
		{
			this.PreviousAreaExtended.RunOnDifference(this.PreviousPreviousAreaExtended, inside_view_second_time);
		}
	}

	// Token: 0x0600680E RID: 26638 RVA: 0x002D6178 File Offset: 0x002D4378
	public void RunIfVisible(int cell, Action<int> action)
	{
		this.CurrentArea.RunIfInside(cell, action);
	}

	// Token: 0x0600680F RID: 26639 RVA: 0x002D6198 File Offset: 0x002D4398
	public void RunIfVisibleExtended(int cell, Action<int> action)
	{
		this.CurrentAreaExtended.RunIfInside(cell, action);
	}

	// Token: 0x06006810 RID: 26640 RVA: 0x000E425C File Offset: 0x000E245C
	public static GridArea GetVisibleArea()
	{
		return GridVisibleArea.GetVisibleAreaExtended(0);
	}

	// Token: 0x06006811 RID: 26641 RVA: 0x002D61B8 File Offset: 0x002D43B8
	public static GridArea GetVisibleAreaExtended(int padding)
	{
		GridArea result = default(GridArea);
		Camera mainCamera = Game.MainCamera;
		if (mainCamera != null)
		{
			Vector3 vector = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, mainCamera.transform.GetPosition().z));
			Vector3 vector2 = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, mainCamera.transform.GetPosition().z));
			vector.x += (float)padding;
			vector.y += (float)padding;
			vector2.x -= (float)padding;
			vector2.y -= (float)padding;
			if (CameraController.Instance != null)
			{
				Vector2I vector2I;
				Vector2I vector2I2;
				CameraController.Instance.GetWorldCamera(out vector2I, out vector2I2);
				result.SetExtents(Math.Max((int)(vector2.x - 0.5f), vector2I.x), Math.Max((int)(vector2.y - 0.5f), vector2I.y), Math.Min((int)(vector.x + 1.5f), vector2I2.x + vector2I.x), Math.Min((int)(vector.y + 1.5f), vector2I2.y + vector2I.y));
			}
			else
			{
				result.SetExtents(Math.Max((int)(vector2.x - 0.5f), 0), Math.Max((int)(vector2.y - 0.5f), 0), Math.Min((int)(vector.x + 1.5f), Grid.WidthInCells), Math.Min((int)(vector.y + 1.5f), Grid.HeightInCells));
			}
		}
		return result;
	}

	// Token: 0x04004E7C RID: 20092
	private GridArea[] VisibleAreas = new GridArea[3];

	// Token: 0x04004E7D RID: 20093
	private GridArea[] VisibleAreasExtended = new GridArea[3];

	// Token: 0x04004E7E RID: 20094
	private List<GridVisibleArea.Callback> Callbacks = new List<GridVisibleArea.Callback>();

	// Token: 0x04004E7F RID: 20095
	public bool debugFreezeVisibleArea;

	// Token: 0x04004E80 RID: 20096
	public bool debugFreezeVisibleAreasExtended;

	// Token: 0x04004E81 RID: 20097
	private readonly int _padding;

	// Token: 0x020013D5 RID: 5077
	public struct Callback
	{
		// Token: 0x04004E82 RID: 20098
		public System.Action OnUpdate;

		// Token: 0x04004E83 RID: 20099
		public string Name;
	}
}
