using KSerialization;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/TreeBud")]
public class TreeBud : KMonoBehaviour
{
	[Serialize]
	public Ref<BuddingTrunk> buddingTrunk;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		PlantBranch.Instance sMI = base.gameObject.GetSMI<PlantBranch.Instance>();
		if (sMI != null && !sMI.IsRunning())
		{
			sMI.StartSM();
		}
	}

	public BuddingTrunk GetAndForgetOldTrunk()
	{
		BuddingTrunk result = ((buddingTrunk == null) ? null : buddingTrunk.Get());
		buddingTrunk = null;
		return result;
	}
}
