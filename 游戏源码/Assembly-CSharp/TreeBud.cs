using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001707 RID: 5895
[AddComponentMenu("KMonoBehaviour/scripts/TreeBud")]
public class TreeBud : KMonoBehaviour
{
	// Token: 0x06007952 RID: 31058 RVA: 0x00313F00 File Offset: 0x00312100
	protected override void OnSpawn()
	{
		base.OnSpawn();
		PlantBranch.Instance smi = base.gameObject.GetSMI<PlantBranch.Instance>();
		if (smi != null && !smi.IsRunning())
		{
			smi.StartSM();
		}
	}

	// Token: 0x06007953 RID: 31059 RVA: 0x000EFCD2 File Offset: 0x000EDED2
	public BuddingTrunk GetAndForgetOldTrunk()
	{
		BuddingTrunk result = (this.buddingTrunk == null) ? null : this.buddingTrunk.Get();
		this.buddingTrunk = null;
		return result;
	}

	// Token: 0x04005AF9 RID: 23289
	[Serialize]
	public Ref<BuddingTrunk> buddingTrunk;
}
