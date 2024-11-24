using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200169C RID: 5788
public class PedestalArtifactSpawner : KMonoBehaviour
{
	// Token: 0x06007789 RID: 30601 RVA: 0x0030E5D0 File Offset: 0x0030C7D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (GameObject gameObject in this.storage.items)
		{
			if (ArtifactSelector.Instance.GetArtifactType(gameObject.name) == ArtifactType.Terrestrial)
			{
				gameObject.GetComponent<KPrefabID>().AddTag(GameTags.TerrestrialArtifact, true);
			}
		}
		if (this.artifactSpawned)
		{
			return;
		}
		GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab(ArtifactSelector.Instance.GetUniqueArtifactID(ArtifactType.Terrestrial)), base.transform.position);
		gameObject2.SetActive(true);
		gameObject2.GetComponent<KPrefabID>().AddTag(GameTags.TerrestrialArtifact, true);
		this.storage.Store(gameObject2, false, false, true, false);
		this.receptacle.ForceDeposit(gameObject2);
		this.artifactSpawned = true;
	}

	// Token: 0x04005950 RID: 22864
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04005951 RID: 22865
	[MyCmpReq]
	private SingleEntityReceptacle receptacle;

	// Token: 0x04005952 RID: 22866
	[Serialize]
	private bool artifactSpawned;
}
