using System;
using System.Collections.Generic;
using KSerialization;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B97 RID: 15255
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/Traits")]
	public class Traits : KMonoBehaviour, ISaveLoadable
	{
		// Token: 0x0600EAF5 RID: 60149 RVA: 0x0013D141 File Offset: 0x0013B341
		public List<string> GetTraitIds()
		{
			return this.TraitIds;
		}

		// Token: 0x0600EAF6 RID: 60150 RVA: 0x0013D149 File Offset: 0x0013B349
		public void SetTraitIds(List<string> traits)
		{
			this.TraitIds = traits;
		}

		// Token: 0x0600EAF7 RID: 60151 RVA: 0x004CB954 File Offset: 0x004C9B54
		protected override void OnSpawn()
		{
			foreach (string id in this.TraitIds)
			{
				if (Db.Get().traits.Exists(id))
				{
					Trait trait = Db.Get().traits.Get(id);
					this.AddInternal(trait);
				}
			}
			if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 15))
			{
				List<DUPLICANTSTATS.TraitVal> joytraits = DUPLICANTSTATS.JOYTRAITS;
				if (base.GetComponent<MinionIdentity>())
				{
					bool flag = true;
					foreach (DUPLICANTSTATS.TraitVal traitVal in joytraits)
					{
						if (this.HasTrait(traitVal.id))
						{
							flag = false;
						}
					}
					if (flag)
					{
						DUPLICANTSTATS.TraitVal random = joytraits.GetRandom<DUPLICANTSTATS.TraitVal>();
						Trait trait2 = Db.Get().traits.Get(random.id);
						this.Add(trait2);
					}
				}
			}
		}

		// Token: 0x0600EAF8 RID: 60152 RVA: 0x0013D152 File Offset: 0x0013B352
		private void AddInternal(Trait trait)
		{
			if (!this.HasTrait(trait))
			{
				this.TraitList.Add(trait);
				trait.AddTo(this.GetAttributes());
				if (trait.OnAddTrait != null)
				{
					trait.OnAddTrait(base.gameObject);
				}
			}
		}

		// Token: 0x0600EAF9 RID: 60153 RVA: 0x004CBA74 File Offset: 0x004C9C74
		public void Add(Trait trait)
		{
			DebugUtil.Assert(base.IsInitialized() || base.GetComponent<Modifiers>().IsInitialized(), "Tried adding a trait on a prefab, use Modifiers.initialTraits instead!", trait.Name, base.gameObject.name);
			if (trait.ShouldSave)
			{
				this.TraitIds.Add(trait.Id);
			}
			this.AddInternal(trait);
		}

		// Token: 0x0600EAFA RID: 60154 RVA: 0x004CBAD4 File Offset: 0x004C9CD4
		public bool HasTrait(string trait_id)
		{
			bool result = false;
			using (List<Trait>.Enumerator enumerator = this.TraitList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Id == trait_id)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600EAFB RID: 60155 RVA: 0x004CBB34 File Offset: 0x004C9D34
		public bool HasTrait(Trait trait)
		{
			using (List<Trait>.Enumerator enumerator = this.TraitList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == trait)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600EAFC RID: 60156 RVA: 0x0013D18E File Offset: 0x0013B38E
		public void Clear()
		{
			while (this.TraitList.Count > 0)
			{
				this.Remove(this.TraitList[0]);
			}
		}

		// Token: 0x0600EAFD RID: 60157 RVA: 0x004CBB8C File Offset: 0x004C9D8C
		public void Remove(Trait trait)
		{
			for (int i = 0; i < this.TraitList.Count; i++)
			{
				if (this.TraitList[i] == trait)
				{
					this.TraitList.RemoveAt(i);
					this.TraitIds.Remove(trait.Id);
					trait.RemoveFrom(this.GetAttributes());
					return;
				}
			}
		}

		// Token: 0x0600EAFE RID: 60158 RVA: 0x004CBBEC File Offset: 0x004C9DEC
		public bool IsEffectIgnored(Effect effect)
		{
			foreach (Trait trait in this.TraitList)
			{
				if (trait.ignoredEffects != null && Array.IndexOf<string>(trait.ignoredEffects, effect.Id) != -1)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600EAFF RID: 60159 RVA: 0x004CBC5C File Offset: 0x004C9E5C
		public bool IsChoreGroupDisabled(ChoreGroup choreGroup)
		{
			Trait trait;
			return this.IsChoreGroupDisabled(choreGroup, out trait);
		}

		// Token: 0x0600EB00 RID: 60160 RVA: 0x0013D1B2 File Offset: 0x0013B3B2
		public bool IsChoreGroupDisabled(ChoreGroup choreGroup, out Trait disablingTrait)
		{
			return this.IsChoreGroupDisabled(choreGroup.IdHash, out disablingTrait);
		}

		// Token: 0x0600EB01 RID: 60161 RVA: 0x004CBC74 File Offset: 0x004C9E74
		public bool IsChoreGroupDisabled(HashedString choreGroupId)
		{
			Trait trait;
			return this.IsChoreGroupDisabled(choreGroupId, out trait);
		}

		// Token: 0x0600EB02 RID: 60162 RVA: 0x004CBC8C File Offset: 0x004C9E8C
		public bool IsChoreGroupDisabled(HashedString choreGroupId, out Trait disablingTrait)
		{
			foreach (Trait trait in this.TraitList)
			{
				if (trait.disabledChoreGroups != null)
				{
					ChoreGroup[] disabledChoreGroups = trait.disabledChoreGroups;
					for (int i = 0; i < disabledChoreGroups.Length; i++)
					{
						if (disabledChoreGroups[i].IdHash == choreGroupId)
						{
							disablingTrait = trait;
							return true;
						}
					}
				}
			}
			disablingTrait = null;
			return false;
		}

		// Token: 0x0400E62C RID: 58924
		public List<Trait> TraitList = new List<Trait>();

		// Token: 0x0400E62D RID: 58925
		[Serialize]
		private List<string> TraitIds = new List<string>();
	}
}
