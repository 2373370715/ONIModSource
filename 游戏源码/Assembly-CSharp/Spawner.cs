using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200199D RID: 6557
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Spawner")]
public class Spawner : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x060088B3 RID: 34995 RVA: 0x000F96FE File Offset: 0x000F78FE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SaveGame.Instance.worldGenSpawner.AddLegacySpawner(this.prefabTag, Grid.PosToCell(this));
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x040066CA RID: 26314
	[Serialize]
	public Tag prefabTag;

	// Token: 0x040066CB RID: 26315
	[Serialize]
	public int units = 1;
}
