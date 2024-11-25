using System;
using System.Collections.Generic;

[Serializable]
public class AttackProperties {
    public enum DamageType {
        Standard
    }

    public enum TargetType {
        Single,
        AreaOfEffect
    }

    public float              aoe_radius = 2f;
    public Weapon             attacker;
    public float              base_damage_max;
    public float              base_damage_min;
    public DamageType         damageType;
    public List<AttackEffect> effects;
    public int                maxHits;
    public TargetType         targetType;
}