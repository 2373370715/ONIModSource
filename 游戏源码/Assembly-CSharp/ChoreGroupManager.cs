using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200106E RID: 4206
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ChoreGroupManager")]
public class ChoreGroupManager : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x060055C3 RID: 21955 RVA: 0x000D7F4F File Offset: 0x000D614F
	public static void DestroyInstance()
	{
		ChoreGroupManager.instance = null;
	}

	// Token: 0x170004F0 RID: 1264
	// (get) Token: 0x060055C4 RID: 21956 RVA: 0x000D7F57 File Offset: 0x000D6157
	public List<Tag> DefaultForbiddenTagsList
	{
		get
		{
			return this.defaultForbiddenTagsList;
		}
	}

	// Token: 0x170004F1 RID: 1265
	// (get) Token: 0x060055C5 RID: 21957 RVA: 0x000D7F5F File Offset: 0x000D615F
	public Dictionary<Tag, int> DefaultChorePermission
	{
		get
		{
			return this.defaultChorePermissions;
		}
	}

	// Token: 0x060055C6 RID: 21958 RVA: 0x0027F9F8 File Offset: 0x0027DBF8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ChoreGroupManager.instance = this;
		this.ConvertOldVersion();
		foreach (ChoreGroup choreGroup in Db.Get().ChoreGroups.resources)
		{
			if (!this.defaultChorePermissions.ContainsKey(choreGroup.Id.ToTag()))
			{
				this.defaultChorePermissions.Add(choreGroup.Id.ToTag(), 2);
			}
		}
	}

	// Token: 0x060055C7 RID: 21959 RVA: 0x0027FA90 File Offset: 0x0027DC90
	private void ConvertOldVersion()
	{
		foreach (Tag key in this.defaultForbiddenTagsList)
		{
			if (!this.defaultChorePermissions.ContainsKey(key))
			{
				this.defaultChorePermissions.Add(key, -1);
			}
			this.defaultChorePermissions[key] = 0;
		}
		this.defaultForbiddenTagsList.Clear();
	}

	// Token: 0x04003C37 RID: 15415
	public static ChoreGroupManager instance;

	// Token: 0x04003C38 RID: 15416
	[Serialize]
	private List<Tag> defaultForbiddenTagsList = new List<Tag>();

	// Token: 0x04003C39 RID: 15417
	[Serialize]
	private Dictionary<Tag, int> defaultChorePermissions = new Dictionary<Tag, int>();
}
