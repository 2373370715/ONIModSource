using System;
using UnityEngine;

// Token: 0x02000B05 RID: 2821
[AddComponentMenu("KMonoBehaviour/scripts/SimpleVent")]
public class SimpleVent : KMonoBehaviour
{
	// Token: 0x060034ED RID: 13549 RVA: 0x000C27E3 File Offset: 0x000C09E3
	protected override void OnPrefabInit()
	{
		base.Subscribe<SimpleVent>(-592767678, SimpleVent.OnChangedDelegate);
		base.Subscribe<SimpleVent>(-111137758, SimpleVent.OnChangedDelegate);
	}

	// Token: 0x060034EE RID: 13550 RVA: 0x000C2807 File Offset: 0x000C0A07
	protected override void OnSpawn()
	{
		this.OnChanged(null);
	}

	// Token: 0x060034EF RID: 13551 RVA: 0x0020C770 File Offset: 0x0020A970
	private void OnChanged(object data)
	{
		if (this.operational.IsFunctional)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, this);
			return;
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
	}

	// Token: 0x040023F1 RID: 9201
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040023F2 RID: 9202
	private static readonly EventSystem.IntraObjectHandler<SimpleVent> OnChangedDelegate = new EventSystem.IntraObjectHandler<SimpleVent>(delegate(SimpleVent component, object data)
	{
		component.OnChanged(data);
	});
}
