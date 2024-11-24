using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000C73 RID: 3187
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/BuildingEnabledButton")]
public class BuildingEnabledButton : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
	// Token: 0x170002CF RID: 719
	// (get) Token: 0x06003D2E RID: 15662 RVA: 0x000C7A08 File Offset: 0x000C5C08
	// (set) Token: 0x06003D2F RID: 15663 RVA: 0x00230958 File Offset: 0x0022EB58
	public bool IsEnabled
	{
		get
		{
			return this.Operational != null && this.Operational.GetFlag(BuildingEnabledButton.EnabledFlag);
		}
		set
		{
			this.Operational.SetFlag(BuildingEnabledButton.EnabledFlag, value);
			Game.Instance.userMenu.Refresh(base.gameObject);
			this.buildingEnabled = value;
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.BuildingDisabled, !this.buildingEnabled, null);
			base.Trigger(1088293757, this.buildingEnabled);
		}
	}

	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x06003D30 RID: 15664 RVA: 0x000C7A2A File Offset: 0x000C5C2A
	public bool WaitingForDisable
	{
		get
		{
			return this.IsEnabled && this.Toggleable.IsToggleQueued(this.ToggleIdx);
		}
	}

	// Token: 0x06003D31 RID: 15665 RVA: 0x000C7A47 File Offset: 0x000C5C47
	protected override void OnPrefabInit()
	{
		this.ToggleIdx = this.Toggleable.SetTarget(this);
		base.Subscribe<BuildingEnabledButton>(493375141, BuildingEnabledButton.OnRefreshUserMenuDelegate);
	}

	// Token: 0x06003D32 RID: 15666 RVA: 0x000C7A6C File Offset: 0x000C5C6C
	protected override void OnSpawn()
	{
		this.IsEnabled = this.buildingEnabled;
		if (this.queuedToggle)
		{
			this.OnMenuToggle();
		}
	}

	// Token: 0x06003D33 RID: 15667 RVA: 0x000C7A88 File Offset: 0x000C5C88
	public void HandleToggle()
	{
		this.queuedToggle = false;
		Prioritizable.RemoveRef(base.gameObject);
		this.OnToggle();
	}

	// Token: 0x06003D34 RID: 15668 RVA: 0x000C7AA2 File Offset: 0x000C5CA2
	public bool IsHandlerOn()
	{
		return this.IsEnabled;
	}

	// Token: 0x06003D35 RID: 15669 RVA: 0x000C7AAA File Offset: 0x000C5CAA
	private void OnToggle()
	{
		this.IsEnabled = !this.IsEnabled;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06003D36 RID: 15670 RVA: 0x002309D0 File Offset: 0x0022EBD0
	private void OnMenuToggle()
	{
		if (!this.Toggleable.IsToggleQueued(this.ToggleIdx))
		{
			if (this.IsEnabled)
			{
				base.Trigger(2108245096, "BuildingDisabled");
			}
			this.queuedToggle = true;
			Prioritizable.AddRef(base.gameObject);
		}
		else
		{
			this.queuedToggle = false;
			Prioritizable.RemoveRef(base.gameObject);
		}
		this.Toggleable.Toggle(this.ToggleIdx);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06003D37 RID: 15671 RVA: 0x00230A54 File Offset: 0x0022EC54
	private void OnRefreshUserMenu(object data)
	{
		bool isEnabled = this.IsEnabled;
		bool flag = this.Toggleable.IsToggleQueued(this.ToggleIdx);
		KIconButtonMenu.ButtonInfo button;
		if ((isEnabled && !flag) || (!isEnabled && flag))
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", UI.USERMENUACTIONS.ENABLEBUILDING.NAME, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, UI.USERMENUACTIONS.ENABLEBUILDING.TOOLTIP, true);
		}
		else
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", UI.USERMENUACTIONS.ENABLEBUILDING.NAME_OFF, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, UI.USERMENUACTIONS.ENABLEBUILDING.TOOLTIP_OFF, true);
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x040029A9 RID: 10665
	[MyCmpAdd]
	private Toggleable Toggleable;

	// Token: 0x040029AA RID: 10666
	[MyCmpReq]
	private Operational Operational;

	// Token: 0x040029AB RID: 10667
	private int ToggleIdx;

	// Token: 0x040029AC RID: 10668
	[Serialize]
	private bool buildingEnabled = true;

	// Token: 0x040029AD RID: 10669
	[Serialize]
	private bool queuedToggle;

	// Token: 0x040029AE RID: 10670
	public static readonly Operational.Flag EnabledFlag = new Operational.Flag("building_enabled", Operational.Flag.Type.Functional);

	// Token: 0x040029AF RID: 10671
	private static readonly EventSystem.IntraObjectHandler<BuildingEnabledButton> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<BuildingEnabledButton>(delegate(BuildingEnabledButton component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
