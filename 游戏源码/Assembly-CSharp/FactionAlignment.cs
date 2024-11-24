using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020010BD RID: 4285
[AddComponentMenu("KMonoBehaviour/scripts/FactionAlignment")]
public class FactionAlignment : KMonoBehaviour
{
	// Token: 0x1700052D RID: 1325
	// (get) Token: 0x060057D1 RID: 22481 RVA: 0x000D9448 File Offset: 0x000D7648
	// (set) Token: 0x060057D2 RID: 22482 RVA: 0x000D9450 File Offset: 0x000D7650
	[MyCmpAdd]
	public Health health { get; private set; }

	// Token: 0x1700052E RID: 1326
	// (get) Token: 0x060057D3 RID: 22483 RVA: 0x000D9459 File Offset: 0x000D7659
	// (set) Token: 0x060057D4 RID: 22484 RVA: 0x000D9461 File Offset: 0x000D7661
	public AttackableBase attackable { get; private set; }

	// Token: 0x060057D5 RID: 22485 RVA: 0x00288F54 File Offset: 0x00287154
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

	// Token: 0x060057D6 RID: 22486 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x060057D7 RID: 22487 RVA: 0x000D946A File Offset: 0x000D766A
	private void OnDeath(object data)
	{
		this.SetAlignmentActive(false);
	}

	// Token: 0x060057D8 RID: 22488 RVA: 0x00289000 File Offset: 0x00287200
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

	// Token: 0x060057D9 RID: 22489 RVA: 0x000D9473 File Offset: 0x000D7673
	public bool IsAlignmentActive()
	{
		return FactionManager.Instance.GetFaction(this.Alignment).Members.Contains(this);
	}

	// Token: 0x060057DA RID: 22490 RVA: 0x000D9490 File Offset: 0x000D7690
	public bool IsPlayerTargeted()
	{
		return this.targeted;
	}

	// Token: 0x060057DB RID: 22491 RVA: 0x000D9498 File Offset: 0x000D7698
	public void SetPlayerTargetable(bool state)
	{
		this.targetable = (state && this.canBePlayerTargeted);
		if (!state)
		{
			this.SetPlayerTargeted(false);
		}
	}

	// Token: 0x060057DC RID: 22492 RVA: 0x00289058 File Offset: 0x00287258
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

	// Token: 0x060057DD RID: 22493 RVA: 0x002890C0 File Offset: 0x002872C0
	private void UpdateStatusItem()
	{
		if (this.targeted)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OrderAttack, null);
			return;
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.OrderAttack, false);
	}

	// Token: 0x060057DE RID: 22494 RVA: 0x00289110 File Offset: 0x00287310
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

	// Token: 0x060057DF RID: 22495 RVA: 0x000D94B6 File Offset: 0x000D76B6
	public void SwitchAlignment(FactionManager.FactionID newAlignment)
	{
		this.SetAlignmentActive(false);
		this.Alignment = newAlignment;
		this.SetAlignmentActive(true);
		base.Trigger(-971105736, newAlignment);
	}

	// Token: 0x060057E0 RID: 22496 RVA: 0x000D94DE File Offset: 0x000D76DE
	private void OnQueueDestroyObject()
	{
		FactionManager.Instance.GetFaction(this.Alignment).Members.Remove(this);
		Components.FactionAlignments.Remove(this);
	}

	// Token: 0x060057E1 RID: 22497 RVA: 0x0028917C File Offset: 0x0028737C
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

	// Token: 0x04003D5D RID: 15709
	[MyCmpReq]
	public KPrefabID kprefabID;

	// Token: 0x04003D5E RID: 15710
	[SerializeField]
	public bool canBePlayerTargeted = true;

	// Token: 0x04003D5F RID: 15711
	[SerializeField]
	public bool updatePrioritizable = true;

	// Token: 0x04003D60 RID: 15712
	[Serialize]
	private bool alignmentActive = true;

	// Token: 0x04003D61 RID: 15713
	public FactionManager.FactionID Alignment;

	// Token: 0x04003D62 RID: 15714
	[Serialize]
	private bool targeted;

	// Token: 0x04003D63 RID: 15715
	[Serialize]
	private bool targetable = true;

	// Token: 0x04003D64 RID: 15716
	private bool hasBeenRegisterInPriority;

	// Token: 0x04003D65 RID: 15717
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<FactionAlignment>(GameTags.Dead, delegate(FactionAlignment component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x04003D66 RID: 15718
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>(delegate(FactionAlignment component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04003D67 RID: 15719
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> SetPlayerTargetedFalseDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>(delegate(FactionAlignment component, object data)
	{
		component.SetPlayerTargeted(false);
	});

	// Token: 0x04003D68 RID: 15720
	private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>(delegate(FactionAlignment component, object data)
	{
		component.OnQueueDestroyObject();
	});
}
