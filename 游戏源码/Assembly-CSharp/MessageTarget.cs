using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001DFB RID: 7675
[SerializationConfig(MemberSerialization.OptIn)]
public class MessageTarget : ISaveLoadable
{
	// Token: 0x0600A09F RID: 41119 RVA: 0x003D5D80 File Offset: 0x003D3F80
	public MessageTarget(KPrefabID prefab_id)
	{
		this.prefabId.Set(prefab_id);
		this.position = prefab_id.transform.GetPosition();
		this.name = "Unknown";
		KSelectable component = prefab_id.GetComponent<KSelectable>();
		if (component != null)
		{
			this.name = component.GetName();
		}
		prefab_id.Subscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
	}

	// Token: 0x0600A0A0 RID: 41120 RVA: 0x0010860A File Offset: 0x0010680A
	public Vector3 GetPosition()
	{
		if (this.prefabId.Get() != null)
		{
			return this.prefabId.Get().transform.GetPosition();
		}
		return this.position;
	}

	// Token: 0x0600A0A1 RID: 41121 RVA: 0x0010863B File Offset: 0x0010683B
	public KSelectable GetSelectable()
	{
		if (this.prefabId.Get() != null)
		{
			return this.prefabId.Get().transform.GetComponent<KSelectable>();
		}
		return null;
	}

	// Token: 0x0600A0A2 RID: 41122 RVA: 0x00108667 File Offset: 0x00106867
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x0600A0A3 RID: 41123 RVA: 0x003D5DFC File Offset: 0x003D3FFC
	private void OnAbsorbedBy(object data)
	{
		if (this.prefabId.Get() != null)
		{
			this.prefabId.Get().Unsubscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
		}
		KPrefabID component = ((GameObject)data).GetComponent<KPrefabID>();
		component.Subscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
		this.prefabId.Set(component);
	}

	// Token: 0x0600A0A4 RID: 41124 RVA: 0x003D5E70 File Offset: 0x003D4070
	public void OnCleanUp()
	{
		if (this.prefabId.Get() != null)
		{
			this.prefabId.Get().Unsubscribe(-1940207677, new Action<object>(this.OnAbsorbedBy));
			this.prefabId.Set(null);
		}
	}

	// Token: 0x04007D82 RID: 32130
	[Serialize]
	private Ref<KPrefabID> prefabId = new Ref<KPrefabID>();

	// Token: 0x04007D83 RID: 32131
	[Serialize]
	private Vector3 position;

	// Token: 0x04007D84 RID: 32132
	[Serialize]
	private string name;
}
