using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020013E2 RID: 5090
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Health")]
public class Health : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170006AF RID: 1711
	// (get) Token: 0x0600687C RID: 26748 RVA: 0x000E4797 File Offset: 0x000E2997
	public AmountInstance GetAmountInstance
	{
		get
		{
			return this.amountInstance;
		}
	}

	// Token: 0x170006B0 RID: 1712
	// (get) Token: 0x0600687D RID: 26749 RVA: 0x000E479F File Offset: 0x000E299F
	// (set) Token: 0x0600687E RID: 26750 RVA: 0x000E47AC File Offset: 0x000E29AC
	public float hitPoints
	{
		get
		{
			return this.amountInstance.value;
		}
		set
		{
			this.amountInstance.value = value;
		}
	}

	// Token: 0x170006B1 RID: 1713
	// (get) Token: 0x0600687F RID: 26751 RVA: 0x000E47BA File Offset: 0x000E29BA
	public float maxHitPoints
	{
		get
		{
			return this.amountInstance.GetMax();
		}
	}

	// Token: 0x06006880 RID: 26752 RVA: 0x000E47C7 File Offset: 0x000E29C7
	public float percent()
	{
		return this.hitPoints / this.maxHitPoints;
	}

	// Token: 0x06006881 RID: 26753 RVA: 0x002D7214 File Offset: 0x002D5414
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Health.Add(this);
		this.amountInstance = Db.Get().Amounts.HitPoints.Lookup(base.gameObject);
		this.amountInstance.value = this.amountInstance.GetMax();
		AmountInstance amountInstance = this.amountInstance;
		amountInstance.OnDelta = (Action<float>)Delegate.Combine(amountInstance.OnDelta, new Action<float>(this.OnHealthChanged));
	}

	// Token: 0x06006882 RID: 26754 RVA: 0x002D7290 File Offset: 0x002D5490
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.State == Health.HealthState.Incapacitated || this.hitPoints == 0f)
		{
			if (this.CanBeIncapacitated)
			{
				this.Incapacitate(GameTags.HitPointsDepleted);
			}
			else
			{
				this.Kill();
			}
		}
		if (this.State != Health.HealthState.Incapacitated && this.State != Health.HealthState.Dead)
		{
			this.UpdateStatus();
		}
		this.effects = base.GetComponent<Effects>();
		this.UpdateHealthBar();
		this.UpdateWoundEffects();
	}

	// Token: 0x06006883 RID: 26755 RVA: 0x000E47D6 File Offset: 0x000E29D6
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Health.Remove(this);
	}

	// Token: 0x06006884 RID: 26756 RVA: 0x002D7304 File Offset: 0x002D5504
	public void UpdateHealthBar()
	{
		if (NameDisplayScreen.Instance == null)
		{
			return;
		}
		bool flag = this.State == Health.HealthState.Dead || this.State == Health.HealthState.Incapacitated || this.hitPoints >= this.maxHitPoints || base.gameObject.HasTag("HideHealthBar");
		NameDisplayScreen.Instance.SetHealthDisplay(base.gameObject, new Func<float>(this.percent), !flag);
	}

	// Token: 0x06006885 RID: 26757 RVA: 0x000E47E9 File Offset: 0x000E29E9
	private void Recover()
	{
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.HitPointsDepleted);
	}

	// Token: 0x06006886 RID: 26758 RVA: 0x002D7378 File Offset: 0x002D5578
	public void OnHealthChanged(float delta)
	{
		base.Trigger(-1664904872, delta);
		if (this.State != Health.HealthState.Invincible)
		{
			if (this.hitPoints == 0f && !this.IsDefeated())
			{
				if (this.CanBeIncapacitated)
				{
					this.Incapacitate(GameTags.HitPointsDepleted);
				}
				else
				{
					this.Kill();
				}
			}
			else
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.HitPointsDepleted);
			}
		}
		this.UpdateStatus();
		this.UpdateWoundEffects();
		this.UpdateHealthBar();
	}

	// Token: 0x06006887 RID: 26759 RVA: 0x000E47FB File Offset: 0x000E29FB
	[ContextMenu("DoDamage")]
	public void DoDamage()
	{
		this.Damage(1f);
	}

	// Token: 0x06006888 RID: 26760 RVA: 0x000E4808 File Offset: 0x000E2A08
	public void Damage(float amount)
	{
		if (this.State != Health.HealthState.Invincible)
		{
			this.hitPoints = Mathf.Max(0f, this.hitPoints - amount);
		}
		this.OnHealthChanged(-amount);
	}

	// Token: 0x06006889 RID: 26761 RVA: 0x002D73F4 File Offset: 0x002D55F4
	private void UpdateWoundEffects()
	{
		if (!this.effects)
		{
			return;
		}
		if (this.isCritter != this.isCritterPrev)
		{
			if (this.isCritterPrev)
			{
				this.effects.Remove("LightWoundsCritter");
				this.effects.Remove("ModerateWoundsCritter");
				this.effects.Remove("SevereWoundsCritter");
			}
			else
			{
				this.effects.Remove("LightWounds");
				this.effects.Remove("ModerateWounds");
				this.effects.Remove("SevereWounds");
			}
			this.isCritterPrev = this.isCritter;
		}
		string effect_id;
		string effect_id2;
		string effect_id3;
		if (this.isCritter)
		{
			effect_id = "LightWoundsCritter";
			effect_id2 = "ModerateWoundsCritter";
			effect_id3 = "SevereWoundsCritter";
		}
		else
		{
			effect_id = "LightWounds";
			effect_id2 = "ModerateWounds";
			effect_id3 = "SevereWounds";
		}
		switch (this.State)
		{
		case Health.HealthState.Perfect:
		case Health.HealthState.Alright:
		case Health.HealthState.Incapacitated:
		case Health.HealthState.Dead:
			this.effects.Remove(effect_id);
			this.effects.Remove(effect_id2);
			this.effects.Remove(effect_id3);
			break;
		case Health.HealthState.Scuffed:
			if (!this.effects.HasEffect(effect_id))
			{
				this.effects.Add(effect_id, true);
			}
			this.effects.Remove(effect_id2);
			this.effects.Remove(effect_id3);
			return;
		case Health.HealthState.Injured:
			this.effects.Remove(effect_id);
			if (!this.effects.HasEffect(effect_id2))
			{
				this.effects.Add(effect_id2, true);
			}
			this.effects.Remove(effect_id3);
			return;
		case Health.HealthState.Critical:
			this.effects.Remove(effect_id);
			this.effects.Remove(effect_id2);
			if (!this.effects.HasEffect(effect_id3))
			{
				this.effects.Add(effect_id3, true);
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600688A RID: 26762 RVA: 0x002D75B0 File Offset: 0x002D57B0
	private void UpdateStatus()
	{
		float num = this.hitPoints / this.maxHitPoints;
		Health.HealthState healthState;
		if (this.State == Health.HealthState.Invincible)
		{
			healthState = Health.HealthState.Invincible;
		}
		else if (num >= 1f)
		{
			healthState = Health.HealthState.Perfect;
		}
		else if (num >= 0.85f)
		{
			healthState = Health.HealthState.Alright;
		}
		else if (num >= 0.66f)
		{
			healthState = Health.HealthState.Scuffed;
		}
		else if ((double)num >= 0.33)
		{
			healthState = Health.HealthState.Injured;
		}
		else if (num > 0f)
		{
			healthState = Health.HealthState.Critical;
		}
		else if (num == 0f)
		{
			healthState = Health.HealthState.Incapacitated;
		}
		else
		{
			healthState = Health.HealthState.Dead;
		}
		if (this.State != healthState)
		{
			if (this.State == Health.HealthState.Incapacitated && healthState != Health.HealthState.Dead)
			{
				this.Recover();
			}
			if (healthState == Health.HealthState.Perfect)
			{
				base.Trigger(-1491582671, this);
			}
			this.State = healthState;
			KSelectable component = base.GetComponent<KSelectable>();
			if (this.State != Health.HealthState.Dead && this.State != Health.HealthState.Perfect && this.State != Health.HealthState.Alright && !this.isCritter)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Hitpoints, Db.Get().CreatureStatusItems.HealthStatus, this.State);
				return;
			}
			component.SetStatusItem(Db.Get().StatusItemCategories.Hitpoints, null, null);
		}
	}

	// Token: 0x0600688B RID: 26763 RVA: 0x000E4833 File Offset: 0x000E2A33
	public bool IsIncapacitated()
	{
		return this.State == Health.HealthState.Incapacitated;
	}

	// Token: 0x0600688C RID: 26764 RVA: 0x000E483E File Offset: 0x000E2A3E
	public bool IsDefeated()
	{
		return this.State == Health.HealthState.Incapacitated || this.State == Health.HealthState.Dead;
	}

	// Token: 0x0600688D RID: 26765 RVA: 0x000E4854 File Offset: 0x000E2A54
	public void Incapacitate(Tag cause)
	{
		this.State = Health.HealthState.Incapacitated;
		base.GetComponent<KPrefabID>().AddTag(cause, false);
		this.Damage(this.hitPoints);
	}

	// Token: 0x0600688E RID: 26766 RVA: 0x000E4876 File Offset: 0x000E2A76
	private void Kill()
	{
		if (base.gameObject.GetSMI<DeathMonitor.Instance>() != null)
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Slain);
		}
	}

	// Token: 0x04004EBE RID: 20158
	[Serialize]
	public bool CanBeIncapacitated;

	// Token: 0x04004EBF RID: 20159
	[Serialize]
	public Health.HealthState State;

	// Token: 0x04004EC0 RID: 20160
	[Serialize]
	private Death source_of_death;

	// Token: 0x04004EC1 RID: 20161
	public HealthBar healthBar;

	// Token: 0x04004EC2 RID: 20162
	public bool isCritter;

	// Token: 0x04004EC3 RID: 20163
	private bool isCritterPrev;

	// Token: 0x04004EC4 RID: 20164
	private Effects effects;

	// Token: 0x04004EC5 RID: 20165
	private AmountInstance amountInstance;

	// Token: 0x020013E3 RID: 5091
	public enum HealthState
	{
		// Token: 0x04004EC7 RID: 20167
		Perfect,
		// Token: 0x04004EC8 RID: 20168
		Alright,
		// Token: 0x04004EC9 RID: 20169
		Scuffed,
		// Token: 0x04004ECA RID: 20170
		Injured,
		// Token: 0x04004ECB RID: 20171
		Critical,
		// Token: 0x04004ECC RID: 20172
		Incapacitated,
		// Token: 0x04004ECD RID: 20173
		Dead,
		// Token: 0x04004ECE RID: 20174
		Invincible
	}
}
