using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C2B RID: 3115
[DebuggerDisplay("{Id}")]
[Serializable]
public class AssignableSlot : Resource
{
	// Token: 0x06003B9C RID: 15260 RVA: 0x000C671F File Offset: 0x000C491F
	public AssignableSlot(string id, string name, bool showInUI = true) : base(id, name)
	{
		this.showInUI = showInUI;
	}

	// Token: 0x06003B9D RID: 15261 RVA: 0x0022B8E8 File Offset: 0x00229AE8
	public AssignableSlotInstance Lookup(GameObject go)
	{
		Assignables component = go.GetComponent<Assignables>();
		if (component != null)
		{
			return component.GetSlot(this);
		}
		return null;
	}

	// Token: 0x040028E9 RID: 10473
	public bool showInUI = true;
}
