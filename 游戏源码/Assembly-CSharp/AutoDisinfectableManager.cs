using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000993 RID: 2451
[AddComponentMenu("KMonoBehaviour/scripts/AutoDisinfectableManager")]
public class AutoDisinfectableManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06002C7E RID: 11390 RVA: 0x000BCC9E File Offset: 0x000BAE9E
	public static void DestroyInstance()
	{
		AutoDisinfectableManager.Instance = null;
	}

	// Token: 0x06002C7F RID: 11391 RVA: 0x000BCCA6 File Offset: 0x000BAEA6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		AutoDisinfectableManager.Instance = this;
	}

	// Token: 0x06002C80 RID: 11392 RVA: 0x000BCCB4 File Offset: 0x000BAEB4
	public void AddAutoDisinfectable(AutoDisinfectable auto_disinfectable)
	{
		this.autoDisinfectables.Add(auto_disinfectable);
	}

	// Token: 0x06002C81 RID: 11393 RVA: 0x000BCCC2 File Offset: 0x000BAEC2
	public void RemoveAutoDisinfectable(AutoDisinfectable auto_disinfectable)
	{
		auto_disinfectable.CancelChore();
		this.autoDisinfectables.Remove(auto_disinfectable);
	}

	// Token: 0x06002C82 RID: 11394 RVA: 0x001EC0F8 File Offset: 0x001EA2F8
	public void Sim1000ms(float dt)
	{
		for (int i = 0; i < this.autoDisinfectables.Count; i++)
		{
			this.autoDisinfectables[i].RefreshChore();
		}
	}

	// Token: 0x04001DD6 RID: 7638
	private List<AutoDisinfectable> autoDisinfectables = new List<AutoDisinfectable>();

	// Token: 0x04001DD7 RID: 7639
	public static AutoDisinfectableManager Instance;
}
