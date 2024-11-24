using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Health")]
public class Health : KMonoBehaviour, ISaveLoadable
{
	public enum HealthState
	{
		Perfect,
		Alright,
		Scuffed,
		Injured,
		Critical,
		Incapacitated,
		Dead,
		Invincible
	}

	[Serialize]
	public bool CanBeIncapacitated;

	[Serialize]
	public HealthState State;

	[Serialize]
	private Death source_of_death;

	public HealthBar healthBar;

	public bool isCritter;

	private bool isCritterPrev;

	private Effects effects;

	private AmountInstance amountInstance;

	public AmountInstance GetAmountInstance => amountInstance;

	public float hitPoints
	{
		get
		{
			return amountInstance.value;
		}
		set
		{
			amountInstance.value = value;
		}
	}

	public float maxHitPoints => amountInstance.GetMax();

	public float percent()
	{
		return hitPoints / maxHitPoints;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Health.Add(this);
		amountInstance = Db.Get().Amounts.HitPoints.Lookup(base.gameObject);
		amountInstance.value = amountInstance.GetMax();
		AmountInstance obj = amountInstance;
		obj.OnDelta = (Action<float>)Delegate.Combine(obj.OnDelta, new Action<float>(OnHealthChanged));
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (State == HealthState.Incapacitated || hitPoints == 0f)
		{
			if (CanBeIncapacitated)
			{
				Incapacitate(GameTags.HitPointsDepleted);
			}
			else
			{
				Kill();
			}
		}
		if (State != HealthState.Incapacitated && State != HealthState.Dead)
		{
			UpdateStatus();
		}
		effects = GetComponent<Effects>();
		UpdateHealthBar();
		UpdateWoundEffects();
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Health.Remove(this);
	}

	public void UpdateHealthBar()
	{
		if (!(NameDisplayScreen.Instance == null))
		{
			bool flag = State == HealthState.Dead || State == HealthState.Incapacitated || hitPoints >= maxHitPoints || base.gameObject.HasTag("HideHealthBar");
			NameDisplayScreen.Instance.SetHealthDisplay(base.gameObject, percent, !flag);
		}
	}

	private void Recover()
	{
		GetComponent<KPrefabID>().RemoveTag(GameTags.HitPointsDepleted);
	}

	public void OnHealthChanged(float delta)
	{
		Trigger(-1664904872, delta);
		if (State != HealthState.Invincible)
		{
			if (hitPoints == 0f && !IsDefeated())
			{
				if (CanBeIncapacitated)
				{
					Incapacitate(GameTags.HitPointsDepleted);
				}
				else
				{
					Kill();
				}
			}
			else
			{
				GetComponent<KPrefabID>().RemoveTag(GameTags.HitPointsDepleted);
			}
		}
		UpdateStatus();
		UpdateWoundEffects();
		UpdateHealthBar();
	}

	[ContextMenu("DoDamage")]
	public void DoDamage()
	{
		Damage(1f);
	}

	public void Damage(float amount)
	{
		if (State != HealthState.Invincible)
		{
			hitPoints = Mathf.Max(0f, hitPoints - amount);
		}
		OnHealthChanged(0f - amount);
	}

	private void UpdateWoundEffects()
	{
		if (!effects)
		{
			return;
		}
		if (isCritter != isCritterPrev)
		{
			if (isCritterPrev)
			{
				effects.Remove("LightWoundsCritter");
				effects.Remove("ModerateWoundsCritter");
				effects.Remove("SevereWoundsCritter");
			}
			else
			{
				effects.Remove("LightWounds");
				effects.Remove("ModerateWounds");
				effects.Remove("SevereWounds");
			}
			isCritterPrev = isCritter;
		}
		string effect_id;
		string effect_id2;
		string effect_id3;
		if (isCritter)
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
		switch (State)
		{
		case HealthState.Scuffed:
			if (!effects.HasEffect(effect_id))
			{
				effects.Add(effect_id, should_save: true);
			}
			effects.Remove(effect_id2);
			effects.Remove(effect_id3);
			break;
		case HealthState.Injured:
			effects.Remove(effect_id);
			if (!effects.HasEffect(effect_id2))
			{
				effects.Add(effect_id2, should_save: true);
			}
			effects.Remove(effect_id3);
			break;
		case HealthState.Critical:
			effects.Remove(effect_id);
			effects.Remove(effect_id2);
			if (!effects.HasEffect(effect_id3))
			{
				effects.Add(effect_id3, should_save: true);
			}
			break;
		case HealthState.Perfect:
		case HealthState.Alright:
		case HealthState.Incapacitated:
		case HealthState.Dead:
			effects.Remove(effect_id);
			effects.Remove(effect_id2);
			effects.Remove(effect_id3);
			break;
		}
	}

	private void UpdateStatus()
	{
		float num = hitPoints / maxHitPoints;
		HealthState healthState = ((State == HealthState.Invincible) ? HealthState.Invincible : ((!(num >= 1f)) ? ((num >= 0.85f) ? HealthState.Alright : ((num >= 0.66f) ? HealthState.Scuffed : (((double)num >= 0.33) ? HealthState.Injured : ((num > 0f) ? HealthState.Critical : ((num != 0f) ? HealthState.Dead : HealthState.Incapacitated))))) : HealthState.Perfect));
		if (State != healthState)
		{
			if (State == HealthState.Incapacitated && healthState != HealthState.Dead)
			{
				Recover();
			}
			if (healthState == HealthState.Perfect)
			{
				Trigger(-1491582671, this);
			}
			State = healthState;
			KSelectable component = GetComponent<KSelectable>();
			if (State != HealthState.Dead && State != 0 && State != HealthState.Alright && !isCritter)
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Hitpoints, Db.Get().CreatureStatusItems.HealthStatus, State);
			}
			else
			{
				component.SetStatusItem(Db.Get().StatusItemCategories.Hitpoints, null);
			}
		}
	}

	public bool IsIncapacitated()
	{
		return State == HealthState.Incapacitated;
	}

	public bool IsDefeated()
	{
		if (State != HealthState.Incapacitated)
		{
			return State == HealthState.Dead;
		}
		return true;
	}

	public void Incapacitate(Tag cause)
	{
		State = HealthState.Incapacitated;
		GetComponent<KPrefabID>().AddTag(cause);
		Damage(hitPoints);
	}

	private void Kill()
	{
		if (base.gameObject.GetSMI<DeathMonitor.Instance>() != null)
		{
			base.gameObject.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Slain);
		}
	}
}
