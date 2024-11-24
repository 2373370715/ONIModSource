using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B32 RID: 15154
	[DebuggerDisplay("{Id}")]
	public class Effect : Modifier
	{
		// Token: 0x0600E938 RID: 59704 RVA: 0x004C5768 File Offset: 0x004C3968
		public Effect(string id, string name, string description, float duration, bool show_in_ui, bool trigger_floating_text, bool is_bad, Emote emote = null, float emote_cooldown = -1f, float max_initial_delay = 0f, string stompGroup = null, string custom_icon = "") : this(id, name, description, duration, show_in_ui, trigger_floating_text, is_bad, emote, max_initial_delay, stompGroup, false, custom_icon, emote_cooldown)
		{
		}

		// Token: 0x0600E939 RID: 59705 RVA: 0x004C5794 File Offset: 0x004C3994
		public Effect(string id, string name, string description, float duration, bool show_in_ui, bool trigger_floating_text, bool is_bad, Emote emote, float max_initial_delay, string stompGroup, bool showStatusInWorld, string custom_icon = "", float emote_cooldown = -1f) : this(id, name, description, duration, null, show_in_ui, trigger_floating_text, is_bad, emote, max_initial_delay, stompGroup, showStatusInWorld, custom_icon, emote_cooldown)
		{
		}

		// Token: 0x0600E93A RID: 59706 RVA: 0x004C57C0 File Offset: 0x004C39C0
		public Effect(string id, string name, string description, float duration, string[] immunityEffects, bool show_in_ui, bool trigger_floating_text, bool is_bad, Emote emote, float max_initial_delay, string stompGroup, bool showStatusInWorld, string custom_icon = "", float emote_cooldown = -1f) : base(id, name, description)
		{
			this.duration = duration;
			this.showInUI = show_in_ui;
			this.triggerFloatingText = trigger_floating_text;
			this.isBad = is_bad;
			this.emote = emote;
			this.emoteCooldown = emote_cooldown;
			this.maxInitialDelay = max_initial_delay;
			this.stompGroup = stompGroup;
			this.customIcon = custom_icon;
			this.showStatusInWorld = showStatusInWorld;
			this.immunityEffectsNames = immunityEffects;
		}

		// Token: 0x0600E93B RID: 59707 RVA: 0x004C5830 File Offset: 0x004C3A30
		public Effect(string id, string name, string description, float duration, bool show_in_ui, bool trigger_floating_text, bool is_bad, string emoteAnim, float emote_cooldown = -1f, string stompGroup = null, string custom_icon = "") : base(id, name, description)
		{
			this.duration = duration;
			this.showInUI = show_in_ui;
			this.triggerFloatingText = trigger_floating_text;
			this.isBad = is_bad;
			this.emoteAnim = emoteAnim;
			this.emoteCooldown = emote_cooldown;
			this.stompGroup = stompGroup;
			this.customIcon = custom_icon;
		}

		// Token: 0x0600E93C RID: 59708 RVA: 0x0013BE49 File Offset: 0x0013A049
		public override void AddTo(Attributes attributes)
		{
			base.AddTo(attributes);
		}

		// Token: 0x0600E93D RID: 59709 RVA: 0x0013BE52 File Offset: 0x0013A052
		public override void RemoveFrom(Attributes attributes)
		{
			base.RemoveFrom(attributes);
		}

		// Token: 0x0600E93E RID: 59710 RVA: 0x0013BE5B File Offset: 0x0013A05B
		public void SetEmote(Emote emote, float emoteCooldown = -1f)
		{
			this.emote = emote;
			this.emoteCooldown = emoteCooldown;
		}

		// Token: 0x0600E93F RID: 59711 RVA: 0x0013BE6B File Offset: 0x0013A06B
		public void AddEmotePrecondition(Reactable.ReactablePrecondition precon)
		{
			if (this.emotePreconditions == null)
			{
				this.emotePreconditions = new List<Reactable.ReactablePrecondition>();
			}
			this.emotePreconditions.Add(precon);
		}

		// Token: 0x0600E940 RID: 59712 RVA: 0x004C5888 File Offset: 0x004C3A88
		public static string CreateTooltip(Effect effect, bool showDuration, string linePrefix = "\n    • ", bool showHeader = true)
		{
			StringEntry stringEntry;
			Strings.TryGet("STRINGS.DUPLICANTS.MODIFIERS." + effect.Id.ToUpper() + ".ADDITIONAL_EFFECTS", out stringEntry);
			string text = (showHeader && (effect.SelfModifiers.Count > 0 || stringEntry != null)) ? DUPLICANTS.MODIFIERS.EFFECT_HEADER.text : "";
			foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
			{
				Attribute attribute = Db.Get().Attributes.TryGet(attributeModifier.AttributeId);
				if (attribute == null)
				{
					attribute = Db.Get().CritterAttributes.TryGet(attributeModifier.AttributeId);
				}
				if (attribute != null && attribute.ShowInUI != Attribute.Display.Never)
				{
					text = text + linePrefix + string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, attribute.Name, attributeModifier.GetFormattedString());
				}
			}
			if (effect.immunityEffectsNames != null)
			{
				text += (string.IsNullOrEmpty(text) ? "" : (linePrefix + linePrefix));
				text += ((showHeader && effect.immunityEffectsNames != null && effect.immunityEffectsNames.Length != 0) ? DUPLICANTS.MODIFIERS.EFFECT_IMMUNITIES_HEADER.text : "");
				foreach (string id in effect.immunityEffectsNames)
				{
					Effect effect2 = Db.Get().effects.TryGet(id);
					if (effect2 != null)
					{
						text = text + linePrefix + string.Format(DUPLICANTS.MODIFIERS.IMMUNITY_FORMAT, effect2.Name);
					}
				}
			}
			if (stringEntry != null)
			{
				text = text + linePrefix + stringEntry;
			}
			if (showDuration && effect.duration > 0f)
			{
				text = text + "\n" + string.Format(DUPLICANTS.MODIFIERS.TIME_TOTAL, GameUtil.GetFormattedCycles(effect.duration, "F1", false));
			}
			return text;
		}

		// Token: 0x0600E941 RID: 59713 RVA: 0x0013BE8C File Offset: 0x0013A08C
		public static string CreateFullTooltip(Effect effect, bool showDuration)
		{
			return string.Concat(new string[]
			{
				effect.Name,
				"\n\n",
				effect.description,
				"\n\n",
				Effect.CreateTooltip(effect, showDuration, "\n    • ", true)
			});
		}

		// Token: 0x0600E942 RID: 59714 RVA: 0x0013BECB File Offset: 0x0013A0CB
		public static void AddModifierDescriptions(GameObject parent, List<Descriptor> descs, string effect_id, bool increase_indent = false)
		{
			Effect.AddModifierDescriptions(descs, effect_id, increase_indent, "STRINGS.DUPLICANTS.ATTRIBUTES.");
		}

		// Token: 0x0600E943 RID: 59715 RVA: 0x004C5A7C File Offset: 0x004C3C7C
		public static void AddModifierDescriptions(List<Descriptor> descs, string effect_id, bool increase_indent = false, string prefix = "STRINGS.DUPLICANTS.ATTRIBUTES.")
		{
			foreach (AttributeModifier attributeModifier in Db.Get().effects.Get(effect_id).SelfModifiers)
			{
				Descriptor item = new Descriptor(Strings.Get(prefix + attributeModifier.AttributeId.ToUpper() + ".NAME") + ": " + attributeModifier.GetFormattedString(), "", Descriptor.DescriptorType.Effect, false);
				if (increase_indent)
				{
					item.IncreaseIndent();
				}
				descs.Add(item);
			}
		}

		// Token: 0x0400E4C6 RID: 58566
		public float duration;

		// Token: 0x0400E4C7 RID: 58567
		public bool showInUI;

		// Token: 0x0400E4C8 RID: 58568
		public bool triggerFloatingText;

		// Token: 0x0400E4C9 RID: 58569
		public bool isBad;

		// Token: 0x0400E4CA RID: 58570
		public bool showStatusInWorld;

		// Token: 0x0400E4CB RID: 58571
		public string customIcon;

		// Token: 0x0400E4CC RID: 58572
		public string[] immunityEffectsNames;

		// Token: 0x0400E4CD RID: 58573
		public string emoteAnim;

		// Token: 0x0400E4CE RID: 58574
		public Emote emote;

		// Token: 0x0400E4CF RID: 58575
		public float emoteCooldown;

		// Token: 0x0400E4D0 RID: 58576
		public float maxInitialDelay;

		// Token: 0x0400E4D1 RID: 58577
		public List<Reactable.ReactablePrecondition> emotePreconditions;

		// Token: 0x0400E4D2 RID: 58578
		public string stompGroup;

		// Token: 0x0400E4D3 RID: 58579
		public int stompPriority;
	}
}
