using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001054 RID: 4180
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/CO2")]
public class CO2 : KMonoBehaviour
{
	// Token: 0x06005550 RID: 21840 RVA: 0x000D7A69 File Offset: 0x000D5C69
	public void StartLoop()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.Play("exhale_pre", KAnim.PlayMode.Once, 1f, 0f);
		component.Play("exhale_loop", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06005551 RID: 21841 RVA: 0x000D7AA6 File Offset: 0x000D5CA6
	public void TriggerDestroy()
	{
		base.GetComponent<KBatchedAnimController>().Play("exhale_pst", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04003BCE RID: 15310
	[Serialize]
	[NonSerialized]
	public Vector3 velocity = Vector3.zero;

	// Token: 0x04003BCF RID: 15311
	[Serialize]
	[NonSerialized]
	public float mass;

	// Token: 0x04003BD0 RID: 15312
	[Serialize]
	[NonSerialized]
	public float temperature;

	// Token: 0x04003BD1 RID: 15313
	[Serialize]
	[NonSerialized]
	public float lifetimeRemaining;
}
