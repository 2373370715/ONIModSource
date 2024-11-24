using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/BuddingTrunk")]
public class BuddingTrunk : KMonoBehaviour
{
	[Serialize]
	private Ref<HarvestDesignatable>[] buds;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		PlantBranchGrower.Instance sMI = base.gameObject.GetSMI<PlantBranchGrower.Instance>();
		if (sMI != null && !sMI.IsRunning())
		{
			sMI.StartSM();
		}
	}

	public KPrefabID[] GetAndForgetOldSerializedBranches()
	{
		KPrefabID[] array = null;
		if (buds != null)
		{
			array = new KPrefabID[buds.Length];
			for (int i = 0; i < buds.Length; i++)
			{
				HarvestDesignatable harvestDesignatable = ((buds[i] == null) ? null : buds[i].Get());
				array[i] = ((harvestDesignatable == null) ? null : harvestDesignatable.GetComponent<KPrefabID>());
			}
		}
		buds = null;
		return array;
	}
}
