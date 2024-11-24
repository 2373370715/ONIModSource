using System;

// Token: 0x02000B8C RID: 2956
public class RoomTypeCategory : Resource
{
	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06003870 RID: 14448 RVA: 0x000C489F File Offset: 0x000C2A9F
	// (set) Token: 0x06003871 RID: 14449 RVA: 0x000C48A7 File Offset: 0x000C2AA7
	public string colorName { get; private set; }

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x06003872 RID: 14450 RVA: 0x000C48B0 File Offset: 0x000C2AB0
	// (set) Token: 0x06003873 RID: 14451 RVA: 0x000C48B8 File Offset: 0x000C2AB8
	public string icon { get; private set; }

	// Token: 0x06003874 RID: 14452 RVA: 0x000C48C1 File Offset: 0x000C2AC1
	public RoomTypeCategory(string id, string name, string colorName, string icon) : base(id, name)
	{
		this.colorName = colorName;
		this.icon = icon;
	}
}
