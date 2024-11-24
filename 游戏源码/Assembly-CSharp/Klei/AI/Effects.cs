using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B34 RID: 15156
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/Effects")]
	public class Effects : KMonoBehaviour, ISaveLoadable, ISim1000ms
	{
		// Token: 0x0600E951 RID: 59729 RVA: 0x0013BF2B File Offset: 0x0013A12B
		protected override void OnPrefabInit()
		{
			this.autoRegisterSimRender = false;
		}

		// Token: 0x0600E952 RID: 59730 RVA: 0x004C6040 File Offset: 0x004C4240
		protected override void OnSpawn()
		{
			if (this.saveLoadImmunities != null)
			{
				foreach (Effects.SaveLoadImmunities saveLoadImmunities in this.saveLoadImmunities)
				{
					if (Db.Get().effects.Exists(saveLoadImmunities.effectID))
					{
						Effect effect = Db.Get().effects.Get(saveLoadImmunities.effectID);
						this.AddImmunity(effect, saveLoadImmunities.giverID, true);
					}
				}
			}
			if (this.saveLoadEffects != null)
			{
				foreach (Effects.SaveLoadEffect saveLoadEffect in this.saveLoadEffects)
				{
					if (Db.Get().effects.Exists(saveLoadEffect.id))
					{
						Effect newEffect = Db.Get().effects.Get(saveLoadEffect.id);
						EffectInstance effectInstance = this.Add(newEffect, true);
						if (effectInstance != null)
						{
							effectInstance.timeRemaining = saveLoadEffect.timeRemaining;
						}
					}
				}
			}
			if (this.effectsThatExpire.Count > 0)
			{
				SimAndRenderScheduler.instance.Add(this, this.simRenderLoadBalance);
			}
		}

		// Token: 0x0600E953 RID: 59731 RVA: 0x004C6144 File Offset: 0x004C4344
		public EffectInstance Get(string effect_id)
		{
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.effect.Id == effect_id)
				{
					return effectInstance;
				}
			}
			return null;
		}

		// Token: 0x0600E954 RID: 59732 RVA: 0x004C61AC File Offset: 0x004C43AC
		public EffectInstance Get(HashedString effect_id)
		{
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.effect.IdHash == effect_id)
				{
					return effectInstance;
				}
			}
			return null;
		}

		// Token: 0x0600E955 RID: 59733 RVA: 0x004C6214 File Offset: 0x004C4414
		public EffectInstance Get(Effect effect)
		{
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.effect == effect)
				{
					return effectInstance;
				}
			}
			return null;
		}

		// Token: 0x0600E956 RID: 59734 RVA: 0x004C6270 File Offset: 0x004C4470
		public bool HasImmunityTo(Effect effect)
		{
			using (List<Effects.EffectImmunity>.Enumerator enumerator = this.effectImmunites.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect == effect)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600E957 RID: 59735 RVA: 0x004C62CC File Offset: 0x004C44CC
		public EffectInstance Add(string effect_id, bool should_save)
		{
			Effect newEffect = Db.Get().effects.Get(effect_id);
			return this.Add(newEffect, should_save);
		}

		// Token: 0x0600E958 RID: 59736 RVA: 0x004C62F4 File Offset: 0x004C44F4
		public EffectInstance Add(HashedString effect_id, bool should_save)
		{
			Effect newEffect = Db.Get().effects.Get(effect_id);
			return this.Add(newEffect, should_save);
		}

		// Token: 0x0600E959 RID: 59737 RVA: 0x004C631C File Offset: 0x004C451C
		public EffectInstance Add(Effect newEffect, bool should_save)
		{
			if (this.HasImmunityTo(newEffect))
			{
				return null;
			}
			Traits component = base.GetComponent<Traits>();
			if (component != null && component.IsEffectIgnored(newEffect))
			{
				return null;
			}
			Attributes attributes = this.GetAttributes();
			EffectInstance effectInstance = this.Get(newEffect);
			if (!string.IsNullOrEmpty(newEffect.stompGroup))
			{
				for (int i = this.effects.Count - 1; i >= 0; i--)
				{
					if (this.effects[i] != effectInstance && !(this.effects[i].effect.stompGroup != newEffect.stompGroup) && this.effects[i].effect.stompPriority > newEffect.stompPriority)
					{
						return null;
					}
				}
				for (int j = this.effects.Count - 1; j >= 0; j--)
				{
					if (this.effects[j] != effectInstance && !(this.effects[j].effect.stompGroup != newEffect.stompGroup) && this.effects[j].effect.stompPriority <= newEffect.stompPriority)
					{
						this.Remove(this.effects[j].effect);
					}
				}
			}
			if (effectInstance == null)
			{
				effectInstance = new EffectInstance(base.gameObject, newEffect, should_save);
				newEffect.AddTo(attributes);
				this.effects.Add(effectInstance);
				if (newEffect.duration > 0f)
				{
					this.effectsThatExpire.Add(effectInstance);
					if (this.effectsThatExpire.Count == 1)
					{
						SimAndRenderScheduler.instance.Add(this, this.simRenderLoadBalance);
					}
				}
				base.Trigger(-1901442097, newEffect);
			}
			effectInstance.timeRemaining = newEffect.duration;
			return effectInstance;
		}

		// Token: 0x0600E95A RID: 59738 RVA: 0x0013BF34 File Offset: 0x0013A134
		public void Remove(Effect effect)
		{
			this.Remove(effect.IdHash);
		}

		// Token: 0x0600E95B RID: 59739 RVA: 0x004C64D4 File Offset: 0x004C46D4
		public void Remove(HashedString effect_id)
		{
			int i = 0;
			while (i < this.effectsThatExpire.Count)
			{
				if (this.effectsThatExpire[i].effect.IdHash == effect_id)
				{
					int index = this.effectsThatExpire.Count - 1;
					this.effectsThatExpire[i] = this.effectsThatExpire[index];
					this.effectsThatExpire.RemoveAt(index);
					if (this.effectsThatExpire.Count == 0)
					{
						SimAndRenderScheduler.instance.Remove(this);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			for (int j = 0; j < this.effects.Count; j++)
			{
				if (this.effects[j].effect.IdHash == effect_id)
				{
					Attributes attributes = this.GetAttributes();
					EffectInstance effectInstance = this.effects[j];
					effectInstance.OnCleanUp();
					Effect effect = effectInstance.effect;
					effect.RemoveFrom(attributes);
					int index2 = this.effects.Count - 1;
					this.effects[j] = this.effects[index2];
					this.effects.RemoveAt(index2);
					base.Trigger(-1157678353, effect);
					return;
				}
			}
		}

		// Token: 0x0600E95C RID: 59740 RVA: 0x004C6608 File Offset: 0x004C4808
		public void Remove(string effect_id)
		{
			int i = 0;
			while (i < this.effectsThatExpire.Count)
			{
				if (this.effectsThatExpire[i].effect.Id == effect_id)
				{
					int index = this.effectsThatExpire.Count - 1;
					this.effectsThatExpire[i] = this.effectsThatExpire[index];
					this.effectsThatExpire.RemoveAt(index);
					if (this.effectsThatExpire.Count == 0)
					{
						SimAndRenderScheduler.instance.Remove(this);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			for (int j = 0; j < this.effects.Count; j++)
			{
				if (this.effects[j].effect.Id == effect_id)
				{
					Attributes attributes = this.GetAttributes();
					EffectInstance effectInstance = this.effects[j];
					effectInstance.OnCleanUp();
					Effect effect = effectInstance.effect;
					effect.RemoveFrom(attributes);
					int index2 = this.effects.Count - 1;
					this.effects[j] = this.effects[index2];
					this.effects.RemoveAt(index2);
					base.Trigger(-1157678353, effect);
					return;
				}
			}
		}

		// Token: 0x0600E95D RID: 59741 RVA: 0x004C673C File Offset: 0x004C493C
		public bool HasEffect(HashedString effect_id)
		{
			using (List<EffectInstance>.Enumerator enumerator = this.effects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect.IdHash == effect_id)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600E95E RID: 59742 RVA: 0x004C67A0 File Offset: 0x004C49A0
		public bool HasEffect(string effect_id)
		{
			using (List<EffectInstance>.Enumerator enumerator = this.effects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect.Id == effect_id)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600E95F RID: 59743 RVA: 0x004C6804 File Offset: 0x004C4A04
		public bool HasEffect(Effect effect)
		{
			using (List<EffectInstance>.Enumerator enumerator = this.effects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.effect == effect)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600E960 RID: 59744 RVA: 0x004C6860 File Offset: 0x004C4A60
		public void Sim1000ms(float dt)
		{
			for (int i = 0; i < this.effectsThatExpire.Count; i++)
			{
				EffectInstance effectInstance = this.effectsThatExpire[i];
				if (effectInstance.IsExpired())
				{
					this.Remove(effectInstance.effect);
				}
				effectInstance.timeRemaining -= dt;
			}
		}

		// Token: 0x0600E961 RID: 59745 RVA: 0x004C68B4 File Offset: 0x004C4AB4
		public void AddImmunity(Effect effect, string giverID, bool shouldSave = true)
		{
			if (giverID != null)
			{
				foreach (Effects.EffectImmunity effectImmunity in this.effectImmunites)
				{
					if (effectImmunity.giverID == giverID && effectImmunity.effect == effect)
					{
						return;
					}
				}
			}
			Effects.EffectImmunity effectImmunity2 = new Effects.EffectImmunity(effect, giverID, shouldSave);
			this.effectImmunites.Add(effectImmunity2);
			base.Trigger(1152870979, effectImmunity2);
		}

		// Token: 0x0600E962 RID: 59746 RVA: 0x004C6944 File Offset: 0x004C4B44
		public void RemoveImmunity(Effect effect, string ID)
		{
			Effects.EffectImmunity effectImmunity = default(Effects.EffectImmunity);
			bool flag = false;
			foreach (Effects.EffectImmunity effectImmunity2 in this.effectImmunites)
			{
				if (effectImmunity2.effect == effect && (ID == null || ID == effectImmunity2.giverID))
				{
					effectImmunity = effectImmunity2;
					flag = true;
				}
			}
			if (flag)
			{
				this.effectImmunites.Remove(effectImmunity);
				base.Trigger(964452195, effectImmunity);
			}
		}

		// Token: 0x0600E963 RID: 59747 RVA: 0x004C69DC File Offset: 0x004C4BDC
		[OnSerializing]
		internal void OnSerializing()
		{
			List<Effects.SaveLoadEffect> list = new List<Effects.SaveLoadEffect>();
			foreach (EffectInstance effectInstance in this.effects)
			{
				if (effectInstance.shouldSave)
				{
					Effects.SaveLoadEffect item = new Effects.SaveLoadEffect
					{
						id = effectInstance.effect.Id,
						timeRemaining = effectInstance.timeRemaining,
						saved = true
					};
					list.Add(item);
				}
			}
			this.saveLoadEffects = list.ToArray();
			List<Effects.SaveLoadImmunities> list2 = new List<Effects.SaveLoadImmunities>();
			foreach (Effects.EffectImmunity effectImmunity in this.effectImmunites)
			{
				if (effectImmunity.shouldSave)
				{
					Effect effect = effectImmunity.effect;
					Effects.SaveLoadImmunities item2 = new Effects.SaveLoadImmunities
					{
						effectID = effect.Id,
						giverID = effectImmunity.giverID,
						saved = true
					};
					list2.Add(item2);
				}
			}
			this.saveLoadImmunities = list2.ToArray();
		}

		// Token: 0x0600E964 RID: 59748 RVA: 0x004C6B18 File Offset: 0x004C4D18
		public List<Effects.SaveLoadImmunities> GetAllImmunitiesForSerialization()
		{
			List<Effects.SaveLoadImmunities> list = new List<Effects.SaveLoadImmunities>();
			foreach (Effects.EffectImmunity effectImmunity in this.effectImmunites)
			{
				Effect effect = effectImmunity.effect;
				Effects.SaveLoadImmunities item = new Effects.SaveLoadImmunities
				{
					effectID = effect.Id,
					giverID = effectImmunity.giverID,
					saved = effectImmunity.shouldSave
				};
				list.Add(item);
			}
			return list;
		}

		// Token: 0x0600E965 RID: 59749 RVA: 0x004C6BB0 File Offset: 0x004C4DB0
		public List<Effects.SaveLoadEffect> GetAllEffectsForSerialization()
		{
			List<Effects.SaveLoadEffect> list = new List<Effects.SaveLoadEffect>();
			foreach (EffectInstance effectInstance in this.effects)
			{
				Effects.SaveLoadEffect item = new Effects.SaveLoadEffect
				{
					id = effectInstance.effect.Id,
					timeRemaining = effectInstance.timeRemaining,
					saved = effectInstance.shouldSave
				};
				list.Add(item);
			}
			return list;
		}

		// Token: 0x0600E966 RID: 59750 RVA: 0x0013BF42 File Offset: 0x0013A142
		public List<EffectInstance> GetTimeLimitedEffects()
		{
			return this.effectsThatExpire;
		}

		// Token: 0x0600E967 RID: 59751 RVA: 0x004C6C44 File Offset: 0x004C4E44
		public void CopyEffects(Effects source)
		{
			foreach (EffectInstance effectInstance in source.effects)
			{
				this.Add(effectInstance.effect, effectInstance.shouldSave).timeRemaining = effectInstance.timeRemaining;
			}
			foreach (EffectInstance effectInstance2 in source.effectsThatExpire)
			{
				this.Add(effectInstance2.effect, effectInstance2.shouldSave).timeRemaining = effectInstance2.timeRemaining;
			}
		}

		// Token: 0x0400E4DA RID: 58586
		[Serialize]
		private Effects.SaveLoadEffect[] saveLoadEffects;

		// Token: 0x0400E4DB RID: 58587
		[Serialize]
		private Effects.SaveLoadImmunities[] saveLoadImmunities;

		// Token: 0x0400E4DC RID: 58588
		private List<EffectInstance> effects = new List<EffectInstance>();

		// Token: 0x0400E4DD RID: 58589
		private List<EffectInstance> effectsThatExpire = new List<EffectInstance>();

		// Token: 0x0400E4DE RID: 58590
		private List<Effects.EffectImmunity> effectImmunites = new List<Effects.EffectImmunity>();

		// Token: 0x02003B35 RID: 15157
		[Serializable]
		public struct EffectImmunity
		{
			// Token: 0x0600E969 RID: 59753 RVA: 0x0013BF73 File Offset: 0x0013A173
			public EffectImmunity(Effect e, string id, bool save = true)
			{
				this.giverID = id;
				this.effect = e;
				this.shouldSave = save;
			}

			// Token: 0x0400E4DF RID: 58591
			public string giverID;

			// Token: 0x0400E4E0 RID: 58592
			public Effect effect;

			// Token: 0x0400E4E1 RID: 58593
			public bool shouldSave;
		}

		// Token: 0x02003B36 RID: 15158
		[Serializable]
		public struct SaveLoadImmunities
		{
			// Token: 0x0400E4E2 RID: 58594
			public string giverID;

			// Token: 0x0400E4E3 RID: 58595
			public string effectID;

			// Token: 0x0400E4E4 RID: 58596
			public bool saved;
		}

		// Token: 0x02003B37 RID: 15159
		[Serializable]
		public struct SaveLoadEffect
		{
			// Token: 0x0400E4E5 RID: 58597
			public string id;

			// Token: 0x0400E4E6 RID: 58598
			public float timeRemaining;

			// Token: 0x0400E4E7 RID: 58599
			public bool saved;
		}
	}
}
