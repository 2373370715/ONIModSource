using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001135 RID: 4405
[AddComponentMenu("KMonoBehaviour/scripts/BudUprootedMonitor")]
public class BudUprootedMonitor : KMonoBehaviour
{
	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x06005A1D RID: 23069 RVA: 0x000DAB75 File Offset: 0x000D8D75
	public bool IsUprooted
	{
		get
		{
			return this.uprooted || base.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted);
		}
	}

	// Token: 0x06005A1E RID: 23070 RVA: 0x000DAB91 File Offset: 0x000D8D91
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<BudUprootedMonitor>(-216549700, BudUprootedMonitor.OnUprootedDelegate);
	}

	// Token: 0x06005A1F RID: 23071 RVA: 0x000DABAA File Offset: 0x000D8DAA
	public void SetParentObject(KPrefabID id)
	{
		this.parentObject = new Ref<KPrefabID>(id);
		base.Subscribe(id.gameObject, 1969584890, new Action<object>(this.OnLoseParent));
	}

	// Token: 0x06005A20 RID: 23072 RVA: 0x00293C10 File Offset: 0x00291E10
	private void OnLoseParent(object obj)
	{
		if (!this.uprooted && !base.isNull)
		{
			base.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			this.uprooted = true;
			base.Trigger(-216549700, null);
			if (this.destroyOnParentLost)
			{
				Util.KDestroyGameObject(base.gameObject);
			}
		}
	}

	// Token: 0x06005A21 RID: 23073 RVA: 0x000BFD4F File Offset: 0x000BDF4F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06005A22 RID: 23074 RVA: 0x00293C64 File Offset: 0x00291E64
	public static bool IsObjectUprooted(GameObject plant)
	{
		BudUprootedMonitor component = plant.GetComponent<BudUprootedMonitor>();
		return !(component == null) && component.IsUprooted;
	}

	// Token: 0x04003F98 RID: 16280
	[Serialize]
	public bool canBeUprooted = true;

	// Token: 0x04003F99 RID: 16281
	[Serialize]
	private bool uprooted;

	// Token: 0x04003F9A RID: 16282
	public bool destroyOnParentLost;

	// Token: 0x04003F9B RID: 16283
	public Ref<KPrefabID> parentObject = new Ref<KPrefabID>();

	// Token: 0x04003F9C RID: 16284
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04003F9D RID: 16285
	private static readonly EventSystem.IntraObjectHandler<BudUprootedMonitor> OnUprootedDelegate = new EventSystem.IntraObjectHandler<BudUprootedMonitor>(delegate(BudUprootedMonitor component, object data)
	{
		if (!component.uprooted)
		{
			component.GetComponent<KPrefabID>().AddTag(GameTags.Uprooted, false);
			component.uprooted = true;
			component.Trigger(-216549700, null);
		}
	});
}
