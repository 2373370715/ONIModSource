using System;
using UnityEngine;

// Token: 0x02000CB2 RID: 3250
[AddComponentMenu("KMonoBehaviour/scripts/BubbleSpawner")]
public class BubbleSpawner : KMonoBehaviour
{
	// Token: 0x06003EE0 RID: 16096 RVA: 0x000C8E64 File Offset: 0x000C7064
	protected override void OnSpawn()
	{
		this.emitMass += (UnityEngine.Random.value - 0.5f) * this.emitVariance * this.emitMass;
		base.OnSpawn();
		base.Subscribe<BubbleSpawner>(-1697596308, BubbleSpawner.OnStorageChangedDelegate);
	}

	// Token: 0x06003EE1 RID: 16097 RVA: 0x002358E0 File Offset: 0x00233AE0
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = this.storage.FindFirst(ElementLoader.FindElementByHash(this.element).tag);
		if (gameObject == null)
		{
			return;
		}
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		if (component.Mass >= this.emitMass)
		{
			gameObject.GetComponent<PrimaryElement>().Mass -= this.emitMass;
			BubbleManager.instance.SpawnBubble(base.transform.GetPosition(), this.initialVelocity, component.ElementID, this.emitMass, component.Temperature);
		}
	}

	// Token: 0x04002AE7 RID: 10983
	public SimHashes element;

	// Token: 0x04002AE8 RID: 10984
	public float emitMass;

	// Token: 0x04002AE9 RID: 10985
	public float emitVariance;

	// Token: 0x04002AEA RID: 10986
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x04002AEB RID: 10987
	public Vector2 initialVelocity;

	// Token: 0x04002AEC RID: 10988
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04002AED RID: 10989
	private static readonly EventSystem.IntraObjectHandler<BubbleSpawner> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<BubbleSpawner>(delegate(BubbleSpawner component, object data)
	{
		component.OnStorageChanged(data);
	});
}
