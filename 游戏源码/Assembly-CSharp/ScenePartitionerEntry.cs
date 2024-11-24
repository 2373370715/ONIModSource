using System;

// Token: 0x0200183E RID: 6206
public class ScenePartitionerEntry
{
	// Token: 0x0600804E RID: 32846 RVA: 0x003343A8 File Offset: 0x003325A8
	public ScenePartitionerEntry(string name, object obj, int x, int y, int width, int height, ScenePartitionerLayer layer, ScenePartitioner partitioner, Action<object> event_callback)
	{
		if (x < 0 || y < 0 || width >= 0)
		{
		}
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
		this.layer = layer.layer;
		this.partitioner = partitioner;
		this.eventCallback = event_callback;
		this.obj = obj;
	}

	// Token: 0x0600804F RID: 32847 RVA: 0x000F4769 File Offset: 0x000F2969
	public void UpdatePosition(int x, int y)
	{
		this.partitioner.UpdatePosition(x, y, this);
	}

	// Token: 0x06008050 RID: 32848 RVA: 0x000F4779 File Offset: 0x000F2979
	public void UpdatePosition(Extents e)
	{
		this.partitioner.UpdatePosition(e, this);
	}

	// Token: 0x06008051 RID: 32849 RVA: 0x000F4788 File Offset: 0x000F2988
	public void Release()
	{
		if (this.partitioner != null)
		{
			this.partitioner.Remove(this);
		}
	}

	// Token: 0x04006144 RID: 24900
	public int x;

	// Token: 0x04006145 RID: 24901
	public int y;

	// Token: 0x04006146 RID: 24902
	public int width;

	// Token: 0x04006147 RID: 24903
	public int height;

	// Token: 0x04006148 RID: 24904
	public int layer;

	// Token: 0x04006149 RID: 24905
	public int queryId;

	// Token: 0x0400614A RID: 24906
	public ScenePartitioner partitioner;

	// Token: 0x0400614B RID: 24907
	public Action<object> eventCallback;

	// Token: 0x0400614C RID: 24908
	public object obj;
}
