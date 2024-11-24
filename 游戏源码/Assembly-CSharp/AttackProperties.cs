using System;
using System.Collections.Generic;

// Token: 0x020010B9 RID: 4281
[Serializable]
public class AttackProperties
{
	// Token: 0x04003D4C RID: 15692
	public Weapon attacker;

	// Token: 0x04003D4D RID: 15693
	public AttackProperties.DamageType damageType;

	// Token: 0x04003D4E RID: 15694
	public AttackProperties.TargetType targetType;

	// Token: 0x04003D4F RID: 15695
	public float base_damage_min;

	// Token: 0x04003D50 RID: 15696
	public float base_damage_max;

	// Token: 0x04003D51 RID: 15697
	public int maxHits;

	// Token: 0x04003D52 RID: 15698
	public float aoe_radius = 2f;

	// Token: 0x04003D53 RID: 15699
	public List<AttackEffect> effects;

	// Token: 0x020010BA RID: 4282
	public enum DamageType
	{
		// Token: 0x04003D55 RID: 15701
		Standard
	}

	// Token: 0x020010BB RID: 4283
	public enum TargetType
	{
		// Token: 0x04003D57 RID: 15703
		Single,
		// Token: 0x04003D58 RID: 15704
		AreaOfEffect
	}
}
