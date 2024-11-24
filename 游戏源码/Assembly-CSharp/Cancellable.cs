using System;
using UnityEngine;

// Token: 0x020009A9 RID: 2473
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Cancellable")]
public class Cancellable : KMonoBehaviour
{
	// Token: 0x06002D47 RID: 11591 RVA: 0x000BD562 File Offset: 0x000BB762
	protected override void OnPrefabInit()
	{
		base.Subscribe<Cancellable>(2127324410, Cancellable.OnCancelDelegate);
	}

	// Token: 0x06002D48 RID: 11592 RVA: 0x000BD575 File Offset: 0x000BB775
	protected virtual void OnCancel(object data)
	{
		this.DeleteObject();
	}

	// Token: 0x04001E7E RID: 7806
	private static readonly EventSystem.IntraObjectHandler<Cancellable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Cancellable>(delegate(Cancellable component, object data)
	{
		component.OnCancel(data);
	});
}
