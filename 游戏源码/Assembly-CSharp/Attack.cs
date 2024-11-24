using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020010B8 RID: 4280
public class Attack
{
	// Token: 0x060057CD RID: 22477 RVA: 0x000D9403 File Offset: 0x000D7603
	public Attack(AttackProperties properties, GameObject[] targets)
	{
		this.properties = properties;
		this.targets = targets;
		this.RollHits();
	}

	// Token: 0x060057CE RID: 22478 RVA: 0x00288F00 File Offset: 0x00287100
	private void RollHits()
	{
		int num = 0;
		while (num < this.targets.Length && num <= this.properties.maxHits - 1)
		{
			if (this.targets[num] != null)
			{
				new Hit(this.properties, this.targets[num]);
			}
			num++;
		}
	}

	// Token: 0x04003D49 RID: 15689
	private AttackProperties properties;

	// Token: 0x04003D4A RID: 15690
	private GameObject[] targets;

	// Token: 0x04003D4B RID: 15691
	public List<Hit> Hits;
}
