using System;
using UnityEngine.UI;

// Token: 0x02001E51 RID: 7761
public class NonDrawingGraphic : Graphic
{
	// Token: 0x0600A2A5 RID: 41637 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void SetMaterialDirty()
	{
	}

	// Token: 0x0600A2A6 RID: 41638 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void SetVerticesDirty()
	{
	}

	// Token: 0x0600A2A7 RID: 41639 RVA: 0x00109818 File Offset: 0x00107A18
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
	}
}
