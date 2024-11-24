using System;

// Token: 0x0200183F RID: 6207
public class ScenePartitionerLayer
{
	// Token: 0x06008052 RID: 32850 RVA: 0x000F479E File Offset: 0x000F299E
	public ScenePartitionerLayer(HashedString name, int layer)
	{
		this.name = name;
		this.layer = layer;
	}

	// Token: 0x0400614D RID: 24909
	public HashedString name;

	// Token: 0x0400614E RID: 24910
	public int layer;

	// Token: 0x0400614F RID: 24911
	public Action<int, object> OnEvent;
}
