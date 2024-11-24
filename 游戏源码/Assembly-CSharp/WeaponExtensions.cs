using System;
using UnityEngine;

// Token: 0x020010C6 RID: 4294
public static class WeaponExtensions
{
	// Token: 0x06005805 RID: 22533 RVA: 0x000D96A9 File Offset: 0x000D78A9
	public static Weapon AddWeapon(this GameObject prefab, float base_damage_min, float base_damage_max, AttackProperties.DamageType attackType = AttackProperties.DamageType.Standard, AttackProperties.TargetType targetType = AttackProperties.TargetType.Single, int maxHits = 1, float aoeRadius = 0f)
	{
		Weapon weapon = prefab.AddOrGet<Weapon>();
		weapon.Configure(base_damage_min, base_damage_max, attackType, targetType, maxHits, aoeRadius);
		return weapon;
	}
}
