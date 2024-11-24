using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020010C5 RID: 4293
[AddComponentMenu("KMonoBehaviour/scripts/Weapon")]
public class Weapon : KMonoBehaviour
{
	// Token: 0x060057FE RID: 22526 RVA: 0x002897A0 File Offset: 0x002879A0
	public void Configure(float base_damage_min, float base_damage_max, AttackProperties.DamageType attackType = AttackProperties.DamageType.Standard, AttackProperties.TargetType targetType = AttackProperties.TargetType.Single, int maxHits = 1, float aoeRadius = 0f)
	{
		this.properties = new AttackProperties();
		this.properties.base_damage_min = base_damage_min;
		this.properties.base_damage_max = base_damage_max;
		this.properties.maxHits = maxHits;
		this.properties.damageType = attackType;
		this.properties.aoe_radius = aoeRadius;
		this.properties.attacker = this;
	}

	// Token: 0x060057FF RID: 22527 RVA: 0x000D961B File Offset: 0x000D781B
	public void AddEffect(string effectID = "WasAttacked", float probability = 1f)
	{
		if (this.properties.effects == null)
		{
			this.properties.effects = new List<AttackEffect>();
		}
		this.properties.effects.Add(new AttackEffect(effectID, probability));
	}

	// Token: 0x06005800 RID: 22528 RVA: 0x00289804 File Offset: 0x00287A04
	public int AttackArea(Vector3 centerPoint)
	{
		Vector3 b = Vector3.zero;
		this.alignment = base.GetComponent<FactionAlignment>();
		if (this.alignment == null)
		{
			return 0;
		}
		List<GameObject> list = new List<GameObject>();
		foreach (Health health in Components.Health.Items)
		{
			if (!(health.gameObject == base.gameObject) && !health.IsDefeated())
			{
				FactionAlignment component = health.GetComponent<FactionAlignment>();
				if (!(component == null) && component.IsAlignmentActive() && FactionManager.Instance.GetDisposition(this.alignment.Alignment, component.Alignment) == FactionManager.Disposition.Attack)
				{
					b = health.transform.GetPosition();
					b.z = centerPoint.z;
					if (Vector3.Distance(centerPoint, b) <= this.properties.aoe_radius)
					{
						list.Add(health.gameObject);
					}
				}
			}
		}
		this.AttackTargets(list.ToArray());
		return list.Count;
	}

	// Token: 0x06005801 RID: 22529 RVA: 0x000D9651 File Offset: 0x000D7851
	public void AttackTarget(GameObject target)
	{
		this.AttackTargets(new GameObject[]
		{
			target
		});
	}

	// Token: 0x06005802 RID: 22530 RVA: 0x000D9663 File Offset: 0x000D7863
	public void AttackTargets(GameObject[] targets)
	{
		if (this.properties == null)
		{
			global::Debug.LogWarning(string.Format("Attack properties not configured. {0} cannot attack with weapon.", base.gameObject.name));
			return;
		}
		new Attack(this.properties, targets);
	}

	// Token: 0x06005803 RID: 22531 RVA: 0x000D9695 File Offset: 0x000D7895
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.properties.attacker = this;
	}

	// Token: 0x04003D84 RID: 15748
	[MyCmpReq]
	private FactionAlignment alignment;

	// Token: 0x04003D85 RID: 15749
	public AttackProperties properties;
}
