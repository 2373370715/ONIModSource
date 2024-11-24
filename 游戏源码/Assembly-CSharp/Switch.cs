using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000FE0 RID: 4064
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Switch")]
public class Switch : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
	// Token: 0x170004B8 RID: 1208
	// (get) Token: 0x06005291 RID: 21137 RVA: 0x000C92FF File Offset: 0x000C74FF
	public bool IsSwitchedOn
	{
		get
		{
			return this.switchedOn;
		}
	}

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x06005292 RID: 21138 RVA: 0x00275858 File Offset: 0x00273A58
	// (remove) Token: 0x06005293 RID: 21139 RVA: 0x00275890 File Offset: 0x00273A90
	public event Action<bool> OnToggle;

	// Token: 0x06005294 RID: 21140 RVA: 0x000D5D3D File Offset: 0x000D3F3D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.switchedOn = this.defaultState;
	}

	// Token: 0x06005295 RID: 21141 RVA: 0x002758C8 File Offset: 0x00273AC8
	protected override void OnSpawn()
	{
		this.openToggleIndex = this.openSwitch.SetTarget(this);
		if (this.OnToggle != null)
		{
			this.OnToggle(this.switchedOn);
		}
		if (this.manuallyControlled)
		{
			base.Subscribe<Switch>(493375141, Switch.OnRefreshUserMenuDelegate);
		}
		this.UpdateSwitchStatus();
	}

	// Token: 0x06005296 RID: 21142 RVA: 0x000C92F7 File Offset: 0x000C74F7
	public void HandleToggle()
	{
		this.Toggle();
	}

	// Token: 0x06005297 RID: 21143 RVA: 0x000C92FF File Offset: 0x000C74FF
	public bool IsHandlerOn()
	{
		return this.switchedOn;
	}

	// Token: 0x06005298 RID: 21144 RVA: 0x000D5D51 File Offset: 0x000D3F51
	private void OnMinionToggle()
	{
		if (!DebugHandler.InstantBuildMode)
		{
			this.openSwitch.Toggle(this.openToggleIndex);
			return;
		}
		this.Toggle();
	}

	// Token: 0x06005299 RID: 21145 RVA: 0x000D5D72 File Offset: 0x000D3F72
	protected virtual void Toggle()
	{
		this.SetState(!this.switchedOn);
	}

	// Token: 0x0600529A RID: 21146 RVA: 0x00275920 File Offset: 0x00273B20
	protected virtual void SetState(bool on)
	{
		if (this.switchedOn != on)
		{
			this.switchedOn = on;
			this.UpdateSwitchStatus();
			if (this.OnToggle != null)
			{
				this.OnToggle(this.switchedOn);
			}
			if (this.manuallyControlled)
			{
				Game.Instance.userMenu.Refresh(base.gameObject);
			}
		}
	}

	// Token: 0x0600529B RID: 21147 RVA: 0x0027597C File Offset: 0x00273B7C
	protected virtual void OnRefreshUserMenu(object data)
	{
		LocString loc_string = this.switchedOn ? BUILDINGS.PREFABS.SWITCH.TURN_OFF : BUILDINGS.PREFABS.SWITCH.TURN_ON;
		LocString loc_string2 = this.switchedOn ? BUILDINGS.PREFABS.SWITCH.TURN_OFF_TOOLTIP : BUILDINGS.PREFABS.SWITCH.TURN_ON_TOOLTIP;
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_power", loc_string, new System.Action(this.OnMinionToggle), global::Action.ToggleEnabled, null, null, null, loc_string2, true), 1f);
	}

	// Token: 0x0600529C RID: 21148 RVA: 0x002759F8 File Offset: 0x00273BF8
	protected virtual void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.SwitchStatusActive : Db.Get().BuildingStatusItems.SwitchStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x040039AE RID: 14766
	[SerializeField]
	public bool manuallyControlled = true;

	// Token: 0x040039AF RID: 14767
	[SerializeField]
	public bool defaultState = true;

	// Token: 0x040039B0 RID: 14768
	[Serialize]
	protected bool switchedOn = true;

	// Token: 0x040039B1 RID: 14769
	[MyCmpAdd]
	private Toggleable openSwitch;

	// Token: 0x040039B2 RID: 14770
	private int openToggleIndex;

	// Token: 0x040039B4 RID: 14772
	private static readonly EventSystem.IntraObjectHandler<Switch> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Switch>(delegate(Switch component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
