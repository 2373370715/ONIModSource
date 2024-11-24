using System;
using UnityEngine;

// Token: 0x02000CB5 RID: 3253
public abstract class IBuildingConfig
{
	// Token: 0x06003EE9 RID: 16105
	public abstract BuildingDef CreateBuildingDef();

	// Token: 0x06003EEA RID: 16106 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
	}

	// Token: 0x06003EEB RID: 16107
	public abstract void DoPostConfigureComplete(GameObject go);

	// Token: 0x06003EEC RID: 16108 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06003EED RID: 16109 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06003EEE RID: 16110 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void ConfigurePost(BuildingDef def)
	{
	}

	// Token: 0x06003EEF RID: 16111 RVA: 0x000AD332 File Offset: 0x000AB532
	[Obsolete("Implement GetRequiredDlcIds and/or GetForbiddenDlcIds instead")]
	public virtual string[] GetDlcIds()
	{
		return null;
	}

	// Token: 0x06003EF0 RID: 16112 RVA: 0x000AD332 File Offset: 0x000AB532
	public virtual string[] GetRequiredDlcIds()
	{
		return null;
	}

	// Token: 0x06003EF1 RID: 16113 RVA: 0x000AD332 File Offset: 0x000AB532
	public virtual string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06003EF2 RID: 16114 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public virtual bool ForbidFromLoading()
	{
		return false;
	}
}
