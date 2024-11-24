using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI;

[DebuggerDisplay("{effect.Id}")]
public class EffectInstance : ModifierInstance<Effect>
{
	public Effect effect;

	public bool shouldSave;

	public StatusItem statusItem;

	public float timeRemaining;

	public EmoteReactable reactable;

	protected Effect[] immunityEffects;

	public EffectInstance(GameObject game_object, Effect effect, bool should_save)
		: base(game_object, effect)
	{
		this.effect = effect;
		shouldSave = should_save;
		DefineEffectImmunities();
		ApplyImmunities();
		ConfigureStatusItem();
		if (effect.showInUI)
		{
			KSelectable component = base.gameObject.GetComponent<KSelectable>();
			if (!component.GetStatusItemGroup().HasStatusItem(statusItem))
			{
				component.AddStatusItem(statusItem, this);
			}
		}
		if (effect.triggerFloatingText && PopFXManager.Instance != null)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, effect.Name, game_object.transform);
		}
		if (effect.emote != null)
		{
			RegisterEmote(effect.emote, effect.emoteCooldown);
		}
		if (!string.IsNullOrEmpty(effect.emoteAnim))
		{
			RegisterEmote(effect.emoteAnim, effect.emoteCooldown);
		}
	}

	protected void DefineEffectImmunities()
	{
		if (immunityEffects == null && effect.immunityEffectsNames != null)
		{
			immunityEffects = new Effect[effect.immunityEffectsNames.Length];
			for (int i = 0; i < immunityEffects.Length; i++)
			{
				immunityEffects[i] = Db.Get().effects.Get(effect.immunityEffectsNames[i]);
			}
		}
	}

	protected void ApplyImmunities()
	{
		if (base.gameObject != null && immunityEffects != null)
		{
			Effects component = base.gameObject.GetComponent<Effects>();
			for (int i = 0; i < immunityEffects.Length; i++)
			{
				component.Remove(immunityEffects[i]);
				component.AddImmunity(immunityEffects[i], effect.IdHash.ToString(), shouldSave: false);
			}
		}
	}

	protected void RemoveImmunities()
	{
		if (base.gameObject != null && immunityEffects != null)
		{
			Effects component = base.gameObject.GetComponent<Effects>();
			for (int i = 0; i < immunityEffects.Length; i++)
			{
				component.RemoveImmunity(immunityEffects[i], effect.IdHash.ToString());
			}
		}
	}

	public void RegisterEmote(string emoteAnim, float cooldown = -1f)
	{
		ReactionMonitor.Instance sMI = base.gameObject.GetSMI<ReactionMonitor.Instance>();
		if (sMI == null)
		{
			return;
		}
		bool flag = cooldown < 0f;
		float globalCooldown = (flag ? 100000f : cooldown);
		EmoteReactable emoteReactable = sMI.AddSelfEmoteReactable(base.gameObject, effect.Name + "_Emote", emoteAnim, flag, Db.Get().ChoreTypes.Emote, globalCooldown, 20f, float.NegativeInfinity, effect.maxInitialDelay, effect.emotePreconditions);
		if (emoteReactable != null)
		{
			emoteReactable.InsertPrecondition(0, NotInATube);
			if (!flag)
			{
				reactable = emoteReactable;
			}
		}
	}

	public void RegisterEmote(Emote emote, float cooldown = -1f)
	{
		ReactionMonitor.Instance sMI = base.gameObject.GetSMI<ReactionMonitor.Instance>();
		if (sMI == null)
		{
			return;
		}
		bool flag = cooldown < 0f;
		float globalCooldown = (flag ? 100000f : cooldown);
		EmoteReactable emoteReactable = sMI.AddSelfEmoteReactable(base.gameObject, effect.Name + "_Emote", emote, flag, Db.Get().ChoreTypes.Emote, globalCooldown, 20f, float.NegativeInfinity, effect.maxInitialDelay, effect.emotePreconditions);
		if (emoteReactable != null)
		{
			emoteReactable.InsertPrecondition(0, NotInATube);
			if (!flag)
			{
				reactable = emoteReactable;
			}
		}
	}

	private bool NotInATube(GameObject go, Navigator.ActiveTransition transition)
	{
		if (transition.navGridTransition.start != NavType.Tube)
		{
			return transition.navGridTransition.end != NavType.Tube;
		}
		return false;
	}

	public override void OnCleanUp()
	{
		if (statusItem != null)
		{
			base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(statusItem);
			statusItem = null;
		}
		if (reactable != null)
		{
			reactable.Cleanup();
			reactable = null;
		}
		RemoveImmunities();
	}

	public float GetTimeRemaining()
	{
		return timeRemaining;
	}

	public bool IsExpired()
	{
		if (effect.duration > 0f)
		{
			return timeRemaining <= 0f;
		}
		return false;
	}

	private void ConfigureStatusItem()
	{
		StatusItem.IconType iconType = (effect.isBad ? StatusItem.IconType.Exclamation : StatusItem.IconType.Info);
		if (!effect.customIcon.IsNullOrWhiteSpace())
		{
			iconType = StatusItem.IconType.Custom;
		}
		statusItem = new StatusItem(effect.Id, effect.Name, effect.description, effect.customIcon, iconType, effect.isBad ? NotificationType.Bad : NotificationType.Neutral, allow_multiples: false, showWorldIcon: effect.showStatusInWorld, render_overlay: OverlayModes.None.ID, status_overlays: 2);
		statusItem.resolveStringCallback = ResolveString;
		statusItem.resolveTooltipCallback = ResolveTooltip;
	}

	private string ResolveString(string str, object data)
	{
		return str;
	}

	private string ResolveTooltip(string str, object data)
	{
		string text = str;
		EffectInstance obj = (EffectInstance)data;
		string text2 = Effect.CreateTooltip(obj.effect, showDuration: false);
		if (!string.IsNullOrEmpty(text2))
		{
			text = text + "\n\n" + text2;
		}
		if (obj.effect.duration > 0f)
		{
			text = text + "\n\n" + string.Format(DUPLICANTS.MODIFIERS.TIME_REMAINING, GameUtil.GetFormattedCycles(GetTimeRemaining()));
		}
		return text;
	}
}
