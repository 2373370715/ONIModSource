using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001A00 RID: 6656
[SerializationConfig(MemberSerialization.OptOut)]
[AddComponentMenu("KMonoBehaviour/scripts/UnstableGround")]
public class UnstableGround : KMonoBehaviour
{
	// Token: 0x04006864 RID: 26724
	public SimHashes element;

	// Token: 0x04006865 RID: 26725
	public float mass;

	// Token: 0x04006866 RID: 26726
	public float temperature;

	// Token: 0x04006867 RID: 26727
	public byte diseaseIdx;

	// Token: 0x04006868 RID: 26728
	public int diseaseCount;
}
