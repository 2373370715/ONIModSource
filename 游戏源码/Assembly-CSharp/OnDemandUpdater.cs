using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001680 RID: 5760
public class OnDemandUpdater : MonoBehaviour
{
	// Token: 0x06007700 RID: 30464 RVA: 0x000EE29D File Offset: 0x000EC49D
	public static void DestroyInstance()
	{
		OnDemandUpdater.Instance = null;
	}

	// Token: 0x06007701 RID: 30465 RVA: 0x000EE2A5 File Offset: 0x000EC4A5
	private void Awake()
	{
		OnDemandUpdater.Instance = this;
	}

	// Token: 0x06007702 RID: 30466 RVA: 0x000EE2AD File Offset: 0x000EC4AD
	public void Register(IUpdateOnDemand updater)
	{
		if (!this.Updaters.Contains(updater))
		{
			this.Updaters.Add(updater);
		}
	}

	// Token: 0x06007703 RID: 30467 RVA: 0x000EE2C9 File Offset: 0x000EC4C9
	public void Unregister(IUpdateOnDemand updater)
	{
		if (this.Updaters.Contains(updater))
		{
			this.Updaters.Remove(updater);
		}
	}

	// Token: 0x06007704 RID: 30468 RVA: 0x0030C5E0 File Offset: 0x0030A7E0
	private void Update()
	{
		for (int i = 0; i < this.Updaters.Count; i++)
		{
			if (this.Updaters[i] != null)
			{
				this.Updaters[i].UpdateOnDemand();
			}
		}
	}

	// Token: 0x04005901 RID: 22785
	private List<IUpdateOnDemand> Updaters = new List<IUpdateOnDemand>();

	// Token: 0x04005902 RID: 22786
	public static OnDemandUpdater Instance;
}
