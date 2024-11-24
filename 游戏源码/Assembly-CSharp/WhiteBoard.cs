using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001A47 RID: 6727
public class WhiteBoard : KGameObjectComponentManager<WhiteBoard.Data>, IKComponentManager
{
	// Token: 0x06008C4E RID: 35918 RVA: 0x00362A58 File Offset: 0x00360C58
	public HandleVector<int>.Handle Add(GameObject go)
	{
		return base.Add(go, new WhiteBoard.Data
		{
			keyValueStore = new Dictionary<HashedString, object>()
		});
	}

	// Token: 0x06008C4F RID: 35919 RVA: 0x00362A84 File Offset: 0x00360C84
	protected override void OnCleanUp(HandleVector<int>.Handle h)
	{
		WhiteBoard.Data data = base.GetData(h);
		data.keyValueStore.Clear();
		data.keyValueStore = null;
		base.SetData(h, data);
	}

	// Token: 0x06008C50 RID: 35920 RVA: 0x000FBB3D File Offset: 0x000F9D3D
	public bool HasValue(HandleVector<int>.Handle h, HashedString key)
	{
		return h.IsValid() && base.GetData(h).keyValueStore.ContainsKey(key);
	}

	// Token: 0x06008C51 RID: 35921 RVA: 0x000FBB5C File Offset: 0x000F9D5C
	public object GetValue(HandleVector<int>.Handle h, HashedString key)
	{
		return base.GetData(h).keyValueStore[key];
	}

	// Token: 0x06008C52 RID: 35922 RVA: 0x00362AB4 File Offset: 0x00360CB4
	public void SetValue(HandleVector<int>.Handle h, HashedString key, object value)
	{
		if (!h.IsValid())
		{
			return;
		}
		WhiteBoard.Data data = base.GetData(h);
		data.keyValueStore[key] = value;
		base.SetData(h, data);
	}

	// Token: 0x06008C53 RID: 35923 RVA: 0x00362AE8 File Offset: 0x00360CE8
	public void RemoveValue(HandleVector<int>.Handle h, HashedString key)
	{
		if (!h.IsValid())
		{
			return;
		}
		WhiteBoard.Data data = base.GetData(h);
		data.keyValueStore.Remove(key);
		base.SetData(h, data);
	}

	// Token: 0x02001A48 RID: 6728
	public struct Data
	{
		// Token: 0x0400699E RID: 27038
		public Dictionary<HashedString, object> keyValueStore;
	}
}
