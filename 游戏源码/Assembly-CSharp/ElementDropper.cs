using System;
using UnityEngine;

// Token: 0x02000D5E RID: 3422
[AddComponentMenu("KMonoBehaviour/scripts/ElementDropper")]
public class ElementDropper : KMonoBehaviour
{
	// Token: 0x06004309 RID: 17161 RVA: 0x000CB535 File Offset: 0x000C9735
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<ElementDropper>(-1697596308, ElementDropper.OnStorageChangedDelegate);
	}

	// Token: 0x0600430A RID: 17162 RVA: 0x000CB54E File Offset: 0x000C974E
	private void OnStorageChanged(object data)
	{
		if (this.storage.GetMassAvailable(this.emitTag) >= this.emitMass)
		{
			this.storage.DropSome(this.emitTag, this.emitMass, false, false, this.emitOffset, true, true);
		}
	}

	// Token: 0x04002DDE RID: 11742
	[SerializeField]
	public Tag emitTag;

	// Token: 0x04002DDF RID: 11743
	[SerializeField]
	public float emitMass;

	// Token: 0x04002DE0 RID: 11744
	[SerializeField]
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x04002DE1 RID: 11745
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04002DE2 RID: 11746
	private static readonly EventSystem.IntraObjectHandler<ElementDropper> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ElementDropper>(delegate(ElementDropper component, object data)
	{
		component.OnStorageChanged(data);
	});
}
