﻿using System;
using KSerialization;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/FactionAlignment")]
public class FactionAlignment : KMonoBehaviour
{
			[MyCmpAdd]
	public Health health { get; private set; }

			public AttackableBase attackable { get; private set; }

	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.health = base.GetComponent<Health>();
		this.attackable = base.GetComponent<AttackableBase>();
		Components.FactionAlignments.Add(this);
		base.Subscribe<FactionAlignment>(493375141, FactionAlignment.OnRefreshUserMenuDelegate);
		base.Subscribe<FactionAlignment>(2127324410, FactionAlignment.SetPlayerTargetedFalseDelegate);
		base.Subscribe<FactionAlignment>(1502190696, FactionAlignment.OnQueueDestroyObjectDelegate);
		if (this.alignmentActive)
		{
			FactionManager.Instance.GetFaction(this.Alignment).Members.Add(this);
		}
		GameUtil.SubscribeToTags<FactionAlignment>(this, FactionAlignment.OnDeadTagAddedDelegate, true);
		this.SetPlayerTargeted(this.targeted);
		this.UpdateStatusItem();
	}

	protected override void OnPrefabInit()
	{
	}

	private void OnDeath(object data)
	{
		this.SetAlignmentActive(false);
	}

	public void SetAlignmentActive(bool active)
	{
		this.SetPlayerTargetable(active);
		this.alignmentActive = active;
		if (active)
		{
			FactionManager.Instance.GetFaction(this.Alignment).Members.Add(this);
			return;
		}
		FactionManager.Instance.GetFaction(this.Alignment).Members.Remove(this);
	}

	public bool IsAlignmentActive()
	{
		return FactionManager.Instance.GetFaction(this.Alignment).Members.Contains(this);
	}

	public bool IsPlayerTargeted()
	{
		return this.targeted;
	}

	public void SetPlayerTargetable(bool state)
	{
		this.targetable = (state && this.canBePlayerTargeted);
		if (!state)
		{
			this.SetPlayerTargeted(false);
		}
	}

	public void SetPlayerTargeted(bool state)
	{
		this.targeted = (this.canBePlayerTargeted && state && this.targetable);
		if (state)
		{
			if (!Components.PlayerTargeted.Items.Contains(this))
			{
				Components.PlayerTargeted.Add(this);
			}
			this.SetPrioritizable(true);
		}
		else
		{
			Components.PlayerTargeted.Remove(this);
			this.SetPrioritizable(false);
		}
		this.UpdateStatusItem();
	}

	private void UpdateStatusItem()
	{
		if (this.targeted)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OrderAttack, null);
			return;
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.OrderAttack, false);
	}

	private void SetPrioritizable(bool enable)
	{
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (component == null || !this.updatePrioritizable)
		{
			return;
		}
		if (enable && !this.hasBeenRegisterInPriority)
		{
			Prioritizable.AddRef(base.gameObject);
			this.hasBeenRegisterInPriority = true;
			return;
		}
		if (!enable && component.IsPrioritizable() && this.hasBeenRegisterInPriority)
		{
			Prioritizable.RemoveRef(base.gameObject);
			this.hasBeenRegisterInPriority = false;
		}
	}

	public void SwitchAlignment(FactionManager.FactionID newAlignment)
	{
		this.SetAlignmentActive(false);
		this.Alignment = newAlignment;
		this.SetAlignmentActive(true);
		base.Trigger(-971105736, newAlignment);
	}

	private void OnQueueDestroyObject()
	{
		FactionManager.Instance.GetFaction(this.Alignment).Members.Remove(this);
		Components.FactionAlignments.Remove(this);
	}

	private void OnRefreshUserMenu(object data)
	{
		if (this.Alignment == FactionManager.FactionID.Duplicant)
		{
			return;
		}
		if (!this.canBePlayerTargeted)
		{
			return;
		}
		if (!this.IsAlignmentActive())
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (!this.targeted) ? new KIconButtonMenu.ButtonInfo("action_attack", UI.USERMENUACTIONS.ATTACK.NAME, delegate()
		{
			this.SetPlayerTargeted(true);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ATTACK.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_attack", UI.USERMENUACTIONS.CANCELATTACK.NAME, delegate()
		{
			this.SetPlayerTargeted(false);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELATTACK.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	[MyCmpReq]
	public KPrefabID kprefabID;

	[SerializeField]
	public bool canBePlayerTargeted = true;

	[SerializeField]
	public bool updatePrioritizable = true;

	[Serialize]
	private bool alignmentActive = true;

	public FactionManager.FactionID Alignment;

	[Serialize]
	private bool targeted;

	[Serialize]
	private bool targetable = true;

	private bool hasBeenRegisterInPriority;

	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<FactionAlignment>(GameTags.Dead, delegate(FactionAlignment component, object data)
	{
		component.OnDeath(data);
	});

	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>(delegate(FactionAlignment component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> SetPlayerTargetedFalseDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>(delegate(FactionAlignment component, object data)
	{
		component.SetPlayerTargeted(false);
	});

	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>(delegate(FactionAlignment component, object data)
	{
		component.OnQueueDestroyObject();
	});
}
