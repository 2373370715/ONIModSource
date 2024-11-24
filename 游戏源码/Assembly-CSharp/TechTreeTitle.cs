using System;
using UnityEngine;

// Token: 0x020017AE RID: 6062
public class TechTreeTitle : Resource
{
	// Token: 0x170007ED RID: 2029
	// (get) Token: 0x06007CD4 RID: 31956 RVA: 0x000F230E File Offset: 0x000F050E
	public Vector2 center
	{
		get
		{
			return this.node.center;
		}
	}

	// Token: 0x170007EE RID: 2030
	// (get) Token: 0x06007CD5 RID: 31957 RVA: 0x000F231B File Offset: 0x000F051B
	public float width
	{
		get
		{
			return this.node.width;
		}
	}

	// Token: 0x170007EF RID: 2031
	// (get) Token: 0x06007CD6 RID: 31958 RVA: 0x000F2328 File Offset: 0x000F0528
	public float height
	{
		get
		{
			return this.node.height;
		}
	}

	// Token: 0x06007CD7 RID: 31959 RVA: 0x000F2335 File Offset: 0x000F0535
	public TechTreeTitle(string id, ResourceSet parent, string name, ResourceTreeNode node) : base(id, parent, name)
	{
		this.node = node;
	}

	// Token: 0x04005E7D RID: 24189
	public string desc;

	// Token: 0x04005E7E RID: 24190
	private ResourceTreeNode node;
}
