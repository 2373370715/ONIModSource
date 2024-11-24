using KSerialization;
using UnityEngine;

public class ForestTreeSeedMonitor : KMonoBehaviour
{
	[Serialize]
	private bool hasExtraSeedAvailable;

	public bool ExtraSeedAvailable => hasExtraSeedAvailable;

	public void ExtractExtraSeed()
	{
		if (hasExtraSeedAvailable)
		{
			hasExtraSeedAvailable = false;
			Vector3 position = base.transform.position;
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			Util.KInstantiate(Assets.GetPrefab("ForestTreeSeed"), position).SetActive(value: true);
		}
	}

	public void TryRollNewSeed()
	{
		if (!hasExtraSeedAvailable && Random.Range(0, 100) < 5)
		{
			hasExtraSeedAvailable = true;
		}
	}
}
