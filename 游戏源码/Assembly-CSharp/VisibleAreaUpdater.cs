using System;

// Token: 0x02001A31 RID: 6705
public class VisibleAreaUpdater
{
	// Token: 0x06008BD5 RID: 35797 RVA: 0x000FB67C File Offset: 0x000F987C
	public VisibleAreaUpdater(Action<int> outside_view_first_time_cb, Action<int> inside_view_first_time_cb, string name)
	{
		this.OutsideViewFirstTimeCallback = outside_view_first_time_cb;
		this.InsideViewFirstTimeCallback = inside_view_first_time_cb;
		this.UpdateCallback = new Action<int>(this.InternalUpdateCell);
		this.Name = name;
	}

	// Token: 0x06008BD6 RID: 35798 RVA: 0x000FB6AB File Offset: 0x000F98AB
	public void Update()
	{
		if (CameraController.Instance != null && this.VisibleArea == null)
		{
			this.VisibleArea = CameraController.Instance.VisibleArea;
			this.VisibleArea.Run(this.InsideViewFirstTimeCallback);
		}
	}

	// Token: 0x06008BD7 RID: 35799 RVA: 0x000FB6E3 File Offset: 0x000F98E3
	private void InternalUpdateCell(int cell)
	{
		this.OutsideViewFirstTimeCallback(cell);
		this.InsideViewFirstTimeCallback(cell);
	}

	// Token: 0x06008BD8 RID: 35800 RVA: 0x000FB6FD File Offset: 0x000F98FD
	public void UpdateCell(int cell)
	{
		if (this.VisibleArea != null)
		{
			this.VisibleArea.RunIfVisible(cell, this.UpdateCallback);
		}
	}

	// Token: 0x04006937 RID: 26935
	private GridVisibleArea VisibleArea;

	// Token: 0x04006938 RID: 26936
	private Action<int> OutsideViewFirstTimeCallback;

	// Token: 0x04006939 RID: 26937
	private Action<int> InsideViewFirstTimeCallback;

	// Token: 0x0400693A RID: 26938
	private Action<int> UpdateCallback;

	// Token: 0x0400693B RID: 26939
	private string Name;
}
