using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

namespace Klei.AI;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Effects")]
public class Effects : KMonoBehaviour, ISaveLoadable, ISim1000ms
{
	[Serializable]
	public struct EffectImmunity
	{
		public string giverID;

		public Effect effect;

		public bool shouldSave;

		public EffectImmunity(Effect e, string id, bool save = true)
		{
			giverID = id;
			effect = e;
			shouldSave = save;
		}
	}

	[Serializable]
	public struct SaveLoadImmunities
	{
		public string giverID;

		public string effectID;

		public bool saved;
	}

	[Serializable]
	public struct SaveLoadEffect
	{
		public string id;

		public float timeRemaining;

		public bool saved;
	}

	[Serialize]
	private SaveLoadEffect[] saveLoadEffects;

	[Serialize]
	private SaveLoadImmunities[] saveLoadImmunities;

	private List<EffectInstance> effects = new List<EffectInstance>();

	private List<EffectInstance> effectsThatExpire = new List<EffectInstance>();

	private List<EffectImmunity> effectImmunites = new List<EffectImmunity>();

	protected override void OnPrefabInit()
	{
		autoRegisterSimRender = false;
	}

	protected override void OnSpawn()
	{
		if (this.saveLoadImmunities != null)
		{
			SaveLoadImmunities[] array = this.saveLoadImmunities;
			for (int i = 0; i < array.Length; i++)
			{
				SaveLoadImmunities saveLoadImmunities = array[i];
				if (Db.Get().effects.Exists(saveLoadImmunities.effectID))
				{
					Effect effect = Db.Get().effects.Get(saveLoadImmunities.effectID);
					AddImmunity(effect, saveLoadImmunities.giverID);
				}
			}
		}
		if (saveLoadEffects != null)
		{
			SaveLoadEffect[] array2 = saveLoadEffects;
			for (int i = 0; i < array2.Length; i++)
			{
				SaveLoadEffect saveLoadEffect = array2[i];
				if (Db.Get().effects.Exists(saveLoadEffect.id))
				{
					Effect newEffect = Db.Get().effects.Get(saveLoadEffect.id);
					EffectInstance effectInstance = Add(newEffect, should_save: true);
					if (effectInstance != null)
					{
						effectInstance.timeRemaining = saveLoadEffect.timeRemaining;
					}
				}
			}
		}
		if (effectsThatExpire.Count > 0)
		{
			SimAndRenderScheduler.instance.Add(this, simRenderLoadBalance);
		}
	}

	public EffectInstance Get(string effect_id)
	{
		foreach (EffectInstance effect in effects)
		{
			if (effect.effect.Id == effect_id)
			{
				return effect;
			}
		}
		return null;
	}

	public EffectInstance Get(HashedString effect_id)
	{
		foreach (EffectInstance effect in effects)
		{
			if (effect.effect.IdHash == effect_id)
			{
				return effect;
			}
		}
		return null;
	}

	public EffectInstance Get(Effect effect)
	{
		foreach (EffectInstance effect2 in effects)
		{
			if (effect2.effect == effect)
			{
				return effect2;
			}
		}
		return null;
	}

	public bool HasImmunityTo(Effect effect)
	{
		foreach (EffectImmunity effectImmunite in effectImmunites)
		{
			if (effectImmunite.effect == effect)
			{
				return true;
			}
		}
		return false;
	}

	public EffectInstance Add(string effect_id, bool should_save)
	{
		Effect newEffect = Db.Get().effects.Get(effect_id);
		return Add(newEffect, should_save);
	}

	public EffectInstance Add(HashedString effect_id, bool should_save)
	{
		Effect newEffect = Db.Get().effects.Get(effect_id);
		return Add(newEffect, should_save);
	}

	public EffectInstance Add(Effect newEffect, bool should_save)
	{
		if (HasImmunityTo(newEffect))
		{
			return null;
		}
		Traits component = GetComponent<Traits>();
		if (component != null && component.IsEffectIgnored(newEffect))
		{
			return null;
		}
		Attributes attributes = this.GetAttributes();
		EffectInstance effectInstance = Get(newEffect);
		if (!string.IsNullOrEmpty(newEffect.stompGroup))
		{
			for (int num = effects.Count - 1; num >= 0; num--)
			{
				if (effects[num] != effectInstance && !(effects[num].effect.stompGroup != newEffect.stompGroup) && effects[num].effect.stompPriority > newEffect.stompPriority)
				{
					return null;
				}
			}
			for (int num2 = effects.Count - 1; num2 >= 0; num2--)
			{
				if (effects[num2] != effectInstance && !(effects[num2].effect.stompGroup != newEffect.stompGroup) && effects[num2].effect.stompPriority <= newEffect.stompPriority)
				{
					Remove(effects[num2].effect);
				}
			}
		}
		if (effectInstance == null)
		{
			effectInstance = new EffectInstance(base.gameObject, newEffect, should_save);
			newEffect.AddTo(attributes);
			effects.Add(effectInstance);
			if (newEffect.duration > 0f)
			{
				effectsThatExpire.Add(effectInstance);
				if (effectsThatExpire.Count == 1)
				{
					SimAndRenderScheduler.instance.Add(this, simRenderLoadBalance);
				}
			}
			Trigger(-1901442097, newEffect);
		}
		effectInstance.timeRemaining = newEffect.duration;
		return effectInstance;
	}

	public void Remove(Effect effect)
	{
		Remove(effect.IdHash);
	}

	public void Remove(HashedString effect_id)
	{
		for (int i = 0; i < effectsThatExpire.Count; i++)
		{
			if (effectsThatExpire[i].effect.IdHash == effect_id)
			{
				int index = effectsThatExpire.Count - 1;
				effectsThatExpire[i] = effectsThatExpire[index];
				effectsThatExpire.RemoveAt(index);
				if (effectsThatExpire.Count == 0)
				{
					SimAndRenderScheduler.instance.Remove(this);
				}
				break;
			}
		}
		for (int j = 0; j < effects.Count; j++)
		{
			if (effects[j].effect.IdHash == effect_id)
			{
				Attributes attributes = this.GetAttributes();
				EffectInstance effectInstance = effects[j];
				effectInstance.OnCleanUp();
				Effect effect = effectInstance.effect;
				effect.RemoveFrom(attributes);
				int index2 = effects.Count - 1;
				effects[j] = effects[index2];
				effects.RemoveAt(index2);
				Trigger(-1157678353, effect);
				break;
			}
		}
	}

	public void Remove(string effect_id)
	{
		for (int i = 0; i < effectsThatExpire.Count; i++)
		{
			if (effectsThatExpire[i].effect.Id == effect_id)
			{
				int index = effectsThatExpire.Count - 1;
				effectsThatExpire[i] = effectsThatExpire[index];
				effectsThatExpire.RemoveAt(index);
				if (effectsThatExpire.Count == 0)
				{
					SimAndRenderScheduler.instance.Remove(this);
				}
				break;
			}
		}
		for (int j = 0; j < effects.Count; j++)
		{
			if (effects[j].effect.Id == effect_id)
			{
				Attributes attributes = this.GetAttributes();
				EffectInstance effectInstance = effects[j];
				effectInstance.OnCleanUp();
				Effect effect = effectInstance.effect;
				effect.RemoveFrom(attributes);
				int index2 = effects.Count - 1;
				effects[j] = effects[index2];
				effects.RemoveAt(index2);
				Trigger(-1157678353, effect);
				break;
			}
		}
	}

	public bool HasEffect(HashedString effect_id)
	{
		foreach (EffectInstance effect in effects)
		{
			if (effect.effect.IdHash == effect_id)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasEffect(string effect_id)
	{
		foreach (EffectInstance effect in effects)
		{
			if (effect.effect.Id == effect_id)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasEffect(Effect effect)
	{
		foreach (EffectInstance effect2 in effects)
		{
			if (effect2.effect == effect)
			{
				return true;
			}
		}
		return false;
	}

	public void Sim1000ms(float dt)
	{
		for (int i = 0; i < effectsThatExpire.Count; i++)
		{
			EffectInstance effectInstance = effectsThatExpire[i];
			if (effectInstance.IsExpired())
			{
				Remove(effectInstance.effect);
			}
			effectInstance.timeRemaining -= dt;
		}
	}

	public void AddImmunity(Effect effect, string giverID, bool shouldSave = true)
	{
		if (giverID != null)
		{
			foreach (EffectImmunity effectImmunite in effectImmunites)
			{
				if (effectImmunite.giverID == giverID && effectImmunite.effect == effect)
				{
					return;
				}
			}
		}
		EffectImmunity item = new EffectImmunity(effect, giverID, shouldSave);
		effectImmunites.Add(item);
	}

	public void RemoveImmunity(Effect effect, string ID)
	{
		EffectImmunity item = default(EffectImmunity);
		bool flag = false;
		foreach (EffectImmunity effectImmunite in effectImmunites)
		{
			if (effectImmunite.effect == effect && (ID == null || ID == effectImmunite.giverID))
			{
				item = effectImmunite;
				flag = true;
			}
		}
		if (flag)
		{
			effectImmunites.Remove(item);
		}
	}

	[OnSerializing]
	internal void OnSerializing()
	{
		List<SaveLoadEffect> list = new List<SaveLoadEffect>();
		foreach (EffectInstance effect2 in effects)
		{
			if (effect2.shouldSave)
			{
				SaveLoadEffect saveLoadEffect = default(SaveLoadEffect);
				saveLoadEffect.id = effect2.effect.Id;
				saveLoadEffect.timeRemaining = effect2.timeRemaining;
				saveLoadEffect.saved = true;
				SaveLoadEffect item = saveLoadEffect;
				list.Add(item);
			}
		}
		saveLoadEffects = list.ToArray();
		List<SaveLoadImmunities> list2 = new List<SaveLoadImmunities>();
		foreach (EffectImmunity effectImmunite in effectImmunites)
		{
			if (effectImmunite.shouldSave)
			{
				Effect effect = effectImmunite.effect;
				SaveLoadImmunities saveLoadImmunities = default(SaveLoadImmunities);
				saveLoadImmunities.effectID = effect.Id;
				saveLoadImmunities.giverID = effectImmunite.giverID;
				saveLoadImmunities.saved = true;
				SaveLoadImmunities item2 = saveLoadImmunities;
				list2.Add(item2);
			}
		}
		this.saveLoadImmunities = list2.ToArray();
	}

	public List<SaveLoadImmunities> GetAllImmunitiesForSerialization()
	{
		List<SaveLoadImmunities> list = new List<SaveLoadImmunities>();
		foreach (EffectImmunity effectImmunite in effectImmunites)
		{
			Effect effect = effectImmunite.effect;
			SaveLoadImmunities saveLoadImmunities = default(SaveLoadImmunities);
			saveLoadImmunities.effectID = effect.Id;
			saveLoadImmunities.giverID = effectImmunite.giverID;
			saveLoadImmunities.saved = effectImmunite.shouldSave;
			SaveLoadImmunities item = saveLoadImmunities;
			list.Add(item);
		}
		return list;
	}

	public List<SaveLoadEffect> GetAllEffectsForSerialization()
	{
		List<SaveLoadEffect> list = new List<SaveLoadEffect>();
		foreach (EffectInstance effect in effects)
		{
			SaveLoadEffect saveLoadEffect = default(SaveLoadEffect);
			saveLoadEffect.id = effect.effect.Id;
			saveLoadEffect.timeRemaining = effect.timeRemaining;
			saveLoadEffect.saved = effect.shouldSave;
			SaveLoadEffect item = saveLoadEffect;
			list.Add(item);
		}
		return list;
	}

	public List<EffectInstance> GetTimeLimitedEffects()
	{
		return effectsThatExpire;
	}

	public void CopyEffects(Effects source)
	{
		foreach (EffectInstance effect in source.effects)
		{
			Add(effect.effect, effect.shouldSave).timeRemaining = effect.timeRemaining;
		}
		foreach (EffectInstance item in source.effectsThatExpire)
		{
			Add(item.effect, item.shouldSave).timeRemaining = item.timeRemaining;
		}
	}
}
