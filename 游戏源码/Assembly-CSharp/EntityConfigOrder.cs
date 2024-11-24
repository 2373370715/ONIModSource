using System;

// Token: 0x0200129B RID: 4763
public class EntityConfigOrder : Attribute
{
	// Token: 0x0600620A RID: 25098 RVA: 0x000DFFDB File Offset: 0x000DE1DB
	public EntityConfigOrder(int sort_order)
	{
		this.sortOrder = sort_order;
	}

	// Token: 0x040045CA RID: 17866
	public int sortOrder;
}
