using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000A40 RID: 2624
[AddComponentMenu("KMonoBehaviour/scripts/EffectArea")]
public class EffectArea : KMonoBehaviour
{
	// Token: 0x06003037 RID: 12343 RVA: 0x000BF54E File Offset: 0x000BD74E
	protected override void OnPrefabInit()
	{
		this.Effect = Db.Get().effects.Get(this.EffectName);
	}

	// Token: 0x06003038 RID: 12344 RVA: 0x001FB478 File Offset: 0x001F9678
	private void Update()
	{
		int num = 0;
		int num2 = 0;
		Grid.PosToXY(base.transform.GetPosition(), out num, out num2);
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
		{
			int num3 = 0;
			int num4 = 0;
			Grid.PosToXY(minionIdentity.transform.GetPosition(), out num3, out num4);
			if (Math.Abs(num3 - num) <= this.Area && Math.Abs(num4 - num2) <= this.Area)
			{
				minionIdentity.GetComponent<Effects>().Add(this.Effect, true);
			}
		}
	}

	// Token: 0x0400207D RID: 8317
	public string EffectName;

	// Token: 0x0400207E RID: 8318
	public int Area;

	// Token: 0x0400207F RID: 8319
	private Effect Effect;
}
