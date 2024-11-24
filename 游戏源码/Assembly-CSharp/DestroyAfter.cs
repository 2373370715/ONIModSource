using System;
using UnityEngine;

// Token: 0x02000A33 RID: 2611
[AddComponentMenu("KMonoBehaviour/scripts/DestroyAfter")]
public class DestroyAfter : KMonoBehaviour
{
	// Token: 0x06002FC5 RID: 12229 RVA: 0x000BF095 File Offset: 0x000BD295
	protected override void OnSpawn()
	{
		this.particleSystems = base.gameObject.GetComponentsInChildren<ParticleSystem>(true);
	}

	// Token: 0x06002FC6 RID: 12230 RVA: 0x001F9650 File Offset: 0x001F7850
	private bool IsAlive()
	{
		for (int i = 0; i < this.particleSystems.Length; i++)
		{
			if (this.particleSystems[i].IsAlive(false))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002FC7 RID: 12231 RVA: 0x000BF0A9 File Offset: 0x000BD2A9
	private void Update()
	{
		if (this.particleSystems != null && !this.IsAlive())
		{
			this.DeleteObject();
		}
	}

	// Token: 0x0400203E RID: 8254
	private ParticleSystem[] particleSystems;
}
