using System;
using KSerialization;
using UnityEngine;

// Token: 0x020016B7 RID: 5815
[AddComponentMenu("KMonoBehaviour/scripts/BuddingTrunk")]
public class BuddingTrunk : KMonoBehaviour
{
	// Token: 0x060077EB RID: 30699 RVA: 0x0030F794 File Offset: 0x0030D994
	protected override void OnSpawn()
	{
		base.OnSpawn();
		PlantBranchGrower.Instance smi = base.gameObject.GetSMI<PlantBranchGrower.Instance>();
		if (smi != null && !smi.IsRunning())
		{
			smi.StartSM();
		}
	}

	// Token: 0x060077EC RID: 30700 RVA: 0x0030F7C4 File Offset: 0x0030D9C4
	public KPrefabID[] GetAndForgetOldSerializedBranches()
	{
		KPrefabID[] array = null;
		if (this.buds != null)
		{
			array = new KPrefabID[this.buds.Length];
			for (int i = 0; i < this.buds.Length; i++)
			{
				HarvestDesignatable harvestDesignatable = (this.buds[i] == null) ? null : this.buds[i].Get();
				array[i] = ((harvestDesignatable == null) ? null : harvestDesignatable.GetComponent<KPrefabID>());
			}
		}
		this.buds = null;
		return array;
	}

	// Token: 0x040059AB RID: 22955
	[Serialize]
	private Ref<HarvestDesignatable>[] buds;
}
