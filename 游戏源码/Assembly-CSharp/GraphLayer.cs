using System;
using UnityEngine;

// Token: 0x02001CEE RID: 7406
[RequireComponent(typeof(GraphBase))]
[AddComponentMenu("KMonoBehaviour/scripts/GraphLayer")]
public class GraphLayer : KMonoBehaviour
{
	// Token: 0x17000A38 RID: 2616
	// (get) Token: 0x06009AA6 RID: 39590 RVA: 0x0010490C File Offset: 0x00102B0C
	public GraphBase graph
	{
		get
		{
			if (this.graph_base == null)
			{
				this.graph_base = base.GetComponent<GraphBase>();
			}
			return this.graph_base;
		}
	}

	// Token: 0x040078D4 RID: 30932
	[MyCmpReq]
	private GraphBase graph_base;
}
