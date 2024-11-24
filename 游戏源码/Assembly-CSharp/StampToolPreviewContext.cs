using System;
using UnityEngine;

// Token: 0x02001458 RID: 5208
public class StampToolPreviewContext
{
	// Token: 0x04005116 RID: 20758
	public Transform previewParent;

	// Token: 0x04005117 RID: 20759
	public InterfaceTool tool;

	// Token: 0x04005118 RID: 20760
	public TemplateContainer stampTemplate;

	// Token: 0x04005119 RID: 20761
	public System.Action frameAfterSetupFn;

	// Token: 0x0400511A RID: 20762
	public Action<int> refreshFn;

	// Token: 0x0400511B RID: 20763
	public System.Action onPlaceFn;

	// Token: 0x0400511C RID: 20764
	public Action<string> onErrorChangeFn;

	// Token: 0x0400511D RID: 20765
	public System.Action cleanupFn;
}
