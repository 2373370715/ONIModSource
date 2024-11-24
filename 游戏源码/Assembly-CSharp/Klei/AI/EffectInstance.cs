using System;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B33 RID: 15155
	[DebuggerDisplay("{effect.Id}")]
	public class EffectInstance : ModifierInstance<Effect>
	{
		// Token: 0x0600E944 RID: 59716 RVA: 0x004C5B28 File Offset: 0x004C3D28
		public EffectInstance(GameObject game_object, Effect effect, bool should_save) : base(game_object, effect)
		{
			this.effect = effect;
			this.shouldSave = should_save;
			this.DefineEffectImmunities();
			this.ApplyImmunities();
			this.ConfigureStatusItem();
			if (effect.showInUI)
			{
				KSelectable component = base.gameObject.GetComponent<KSelectable>();
				if (!component.GetStatusItemGroup().HasStatusItem(this.statusItem))
				{
					component.AddStatusItem(this.statusItem, this);
				}
			}
			if (effect.triggerFloatingText && PopFXManager.Instance != null)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, effect.Name, game_object.transform, 1.5f, false);
			}
			if (effect.emote != null)
			{
				this.RegisterEmote(effect.emote, effect.emoteCooldown);
			}
			if (!string.IsNullOrEmpty(effect.emoteAnim))
			{
				this.RegisterEmote(effect.emoteAnim, effect.emoteCooldown);
			}
		}

		// Token: 0x0600E945 RID: 59717 RVA: 0x004C5C08 File Offset: 0x004C3E08
		protected void DefineEffectImmunities()
		{
			if (this.immunityEffects == null && this.effect.immunityEffectsNames != null)
			{
				this.immunityEffects = new Effect[this.effect.immunityEffectsNames.Length];
				for (int i = 0; i < this.immunityEffects.Length; i++)
				{
					this.immunityEffects[i] = Db.Get().effects.Get(this.effect.immunityEffectsNames[i]);
				}
			}
		}

		// Token: 0x0600E946 RID: 59718 RVA: 0x004C5C7C File Offset: 0x004C3E7C
		protected void ApplyImmunities()
		{
			if (base.gameObject != null && this.immunityEffects != null)
			{
				Effects component = base.gameObject.GetComponent<Effects>();
				for (int i = 0; i < this.immunityEffects.Length; i++)
				{
					component.Remove(this.immunityEffects[i]);
					component.AddImmunity(this.immunityEffects[i], this.effect.IdHash.ToString(), false);
				}
			}
		}

		// Token: 0x0600E947 RID: 59719 RVA: 0x004C5CF4 File Offset: 0x004C3EF4
		protected void RemoveImmunities()
		{
			if (base.gameObject != null && this.immunityEffects != null)
			{
				Effects component = base.gameObject.GetComponent<Effects>();
				for (int i = 0; i < this.immunityEffects.Length; i++)
				{
					component.RemoveImmunity(this.immunityEffects[i], this.effect.IdHash.ToString());
				}
			}
		}

		// Token: 0x0600E948 RID: 59720 RVA: 0x004C5D5C File Offset: 0x004C3F5C
		public void RegisterEmote(string emoteAnim, float cooldown = -1f)
		{
			ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
			if (smi == null)
			{
				return;
			}
			bool flag = cooldown < 0f;
			float globalCooldown = flag ? 100000f : cooldown;
			EmoteReactable emoteReactable = smi.AddSelfEmoteReactable(base.gameObject, this.effect.Name + "_Emote", emoteAnim, flag, Db.Get().ChoreTypes.Emote, globalCooldown, 20f, float.NegativeInfinity, this.effect.maxInitialDelay, this.effect.emotePreconditions);
			if (emoteReactable == null)
			{
				return;
			}
			emoteReactable.InsertPrecondition(0, new Reactable.ReactablePrecondition(this.NotInATube));
			if (!flag)
			{
				this.reactable = emoteReactable;
			}
		}

		// Token: 0x0600E949 RID: 59721 RVA: 0x004C5E04 File Offset: 0x004C4004
		public void RegisterEmote(Emote emote, float cooldown = -1f)
		{
			ReactionMonitor.Instance smi = base.gameObject.GetSMI<ReactionMonitor.Instance>();
			if (smi == null)
			{
				return;
			}
			bool flag = cooldown < 0f;
			float globalCooldown = flag ? 100000f : cooldown;
			EmoteReactable emoteReactable = smi.AddSelfEmoteReactable(base.gameObject, this.effect.Name + "_Emote", emote, flag, Db.Get().ChoreTypes.Emote, globalCooldown, 20f, float.NegativeInfinity, this.effect.maxInitialDelay, this.effect.emotePreconditions);
			if (emoteReactable == null)
			{
				return;
			}
			emoteReactable.InsertPrecondition(0, new Reactable.ReactablePrecondition(this.NotInATube));
			if (!flag)
			{
				this.reactable = emoteReactable;
			}
		}

		// Token: 0x0600E94A RID: 59722 RVA: 0x0013BEDA File Offset: 0x0013A0DA
		private bool NotInATube(GameObject go, Navigator.ActiveTransition transition)
		{
			return transition.navGridTransition.start != NavType.Tube && transition.navGridTransition.end != NavType.Tube;
		}

		// Token: 0x0600E94B RID: 59723 RVA: 0x004C5EB0 File Offset: 0x004C40B0
		public override void OnCleanUp()
		{
			if (this.statusItem != null)
			{
				base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.statusItem, false);
				this.statusItem = null;
			}
			if (this.reactable != null)
			{
				this.reactable.Cleanup();
				this.reactable = null;
			}
			this.RemoveImmunities();
		}

		// Token: 0x0600E94C RID: 59724 RVA: 0x0013BEFD File Offset: 0x0013A0FD
		public float GetTimeRemaining()
		{
			return this.timeRemaining;
		}

		// Token: 0x0600E94D RID: 59725 RVA: 0x0013BF05 File Offset: 0x0013A105
		public bool IsExpired()
		{
			return this.effect.duration > 0f && this.timeRemaining <= 0f;
		}

		// Token: 0x0600E94E RID: 59726 RVA: 0x004C5F04 File Offset: 0x004C4104
		private void ConfigureStatusItem()
		{
			StatusItem.IconType iconType = this.effect.isBad ? StatusItem.IconType.Exclamation : StatusItem.IconType.Info;
			if (!this.effect.customIcon.IsNullOrWhiteSpace())
			{
				iconType = StatusItem.IconType.Custom;
			}
			string id = this.effect.Id;
			string name = this.effect.Name;
			string description = this.effect.description;
			string customIcon = this.effect.customIcon;
			StatusItem.IconType icon_type = iconType;
			NotificationType notification_type = this.effect.isBad ? NotificationType.Bad : NotificationType.Neutral;
			bool allow_multiples = false;
			bool showStatusInWorld = this.effect.showStatusInWorld;
			this.statusItem = new StatusItem(id, name, description, customIcon, icon_type, notification_type, allow_multiples, OverlayModes.None.ID, 2, showStatusInWorld, null);
			this.statusItem.resolveStringCallback = new Func<string, object, string>(this.ResolveString);
			this.statusItem.resolveTooltipCallback = new Func<string, object, string>(this.ResolveTooltip);
		}

		// Token: 0x0600E94F RID: 59727 RVA: 0x000B1FA8 File Offset: 0x000B01A8
		private string ResolveString(string str, object data)
		{
			return str;
		}

		// Token: 0x0600E950 RID: 59728 RVA: 0x004C5FC4 File Offset: 0x004C41C4
		private string ResolveTooltip(string str, object data)
		{
			string text = str;
			EffectInstance effectInstance = (EffectInstance)data;
			string text2 = Effect.CreateTooltip(effectInstance.effect, false, "\n    • ", true);
			if (!string.IsNullOrEmpty(text2))
			{
				text = text + "\n\n" + text2;
			}
			if (effectInstance.effect.duration > 0f)
			{
				text = text + "\n\n" + string.Format(DUPLICANTS.MODIFIERS.TIME_REMAINING, GameUtil.GetFormattedCycles(this.GetTimeRemaining(), "F1", false));
			}
			return text;
		}

		// Token: 0x0400E4D4 RID: 58580
		public Effect effect;

		// Token: 0x0400E4D5 RID: 58581
		public bool shouldSave;

		// Token: 0x0400E4D6 RID: 58582
		public StatusItem statusItem;

		// Token: 0x0400E4D7 RID: 58583
		public float timeRemaining;

		// Token: 0x0400E4D8 RID: 58584
		public EmoteReactable reactable;

		// Token: 0x0400E4D9 RID: 58585
		protected Effect[] immunityEffects;
	}
}
