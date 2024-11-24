using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020010C4 RID: 4292
public class Hit
{
	// Token: 0x060057FB RID: 22523 RVA: 0x000D95DC File Offset: 0x000D77DC
	public Hit(AttackProperties properties, GameObject target)
	{
		this.properties = properties;
		this.target = target;
		this.DeliverHit();
	}

	// Token: 0x060057FC RID: 22524 RVA: 0x000D95F8 File Offset: 0x000D77F8
	private float rollDamage()
	{
		return (float)Mathf.RoundToInt(UnityEngine.Random.Range(this.properties.base_damage_min, this.properties.base_damage_max));
	}

	// Token: 0x060057FD RID: 22525 RVA: 0x002896A0 File Offset: 0x002878A0
	private void DeliverHit()
	{
		Health component = this.target.GetComponent<Health>();
		if (!component)
		{
			return;
		}
		this.target.Trigger(-787691065, this.properties.attacker.GetComponent<FactionAlignment>());
		float num = this.rollDamage();
		AttackableBase component2 = this.target.GetComponent<AttackableBase>();
		num *= 1f + component2.GetDamageMultiplier();
		component.Damage(num);
		if (this.properties.effects == null)
		{
			return;
		}
		Effects component3 = this.target.GetComponent<Effects>();
		if (component3)
		{
			foreach (AttackEffect attackEffect in this.properties.effects)
			{
				if (UnityEngine.Random.Range(0f, 100f) < attackEffect.effectProbability * 100f)
				{
					component3.Add(attackEffect.effectID, true);
				}
			}
		}
	}

	// Token: 0x04003D82 RID: 15746
	private AttackProperties properties;

	// Token: 0x04003D83 RID: 15747
	private GameObject target;
}
