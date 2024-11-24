using System;
using KSerialization;
using STRINGS;

// Token: 0x02000DB8 RID: 3512
public class ConnectionManager : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
	// Token: 0x17000358 RID: 856
	// (get) Token: 0x06004508 RID: 17672 RVA: 0x000CC9BE File Offset: 0x000CABBE
	// (set) Token: 0x06004509 RID: 17673 RVA: 0x000CC9C6 File Offset: 0x000CABC6
	public bool IsConnected
	{
		get
		{
			return this.connected;
		}
		set
		{
			this.connected = value;
			this.connectedMeter.SetPositionPercent(value ? 1f : 0f);
		}
	}

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x0600450A RID: 17674 RVA: 0x000CC9E9 File Offset: 0x000CABE9
	public bool WaitingForToggle
	{
		get
		{
			return this.toggleQueued;
		}
	}

	// Token: 0x0600450B RID: 17675 RVA: 0x000CC9F1 File Offset: 0x000CABF1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.toggleIdx = this.toggleable.SetTarget(this);
		base.Subscribe<ConnectionManager>(493375141, ConnectionManager.OnRefreshUserMenuDelegate);
	}

	// Token: 0x0600450C RID: 17676 RVA: 0x0024A408 File Offset: 0x00248608
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.toggleQueued)
		{
			this.OnMenuToggle();
		}
		this.connectedMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_connected_target", "meter_connected", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalVentConfig.CONNECTED_SYMBOLS);
		this.connectedMeter.SetPositionPercent(this.IsConnected ? 1f : 0f);
	}

	// Token: 0x0600450D RID: 17677 RVA: 0x000CCA1C File Offset: 0x000CAC1C
	public void HandleToggle()
	{
		this.toggleQueued = false;
		Prioritizable.RemoveRef(base.gameObject);
		this.OnToggle();
	}

	// Token: 0x0600450E RID: 17678 RVA: 0x000CCA36 File Offset: 0x000CAC36
	private void OnToggle()
	{
		this.IsConnected = !this.IsConnected;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x0600450F RID: 17679 RVA: 0x0024A46C File Offset: 0x0024866C
	private void OnMenuToggle()
	{
		if (!this.toggleable.IsToggleQueued(this.toggleIdx))
		{
			if (this.IsConnected)
			{
				base.Trigger(2108245096, "BuildingDisabled");
			}
			this.toggleQueued = true;
			Prioritizable.AddRef(base.gameObject);
		}
		else
		{
			this.toggleQueued = false;
			Prioritizable.RemoveRef(base.gameObject);
		}
		this.toggleable.Toggle(this.toggleIdx);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06004510 RID: 17680 RVA: 0x0024A4F0 File Offset: 0x002486F0
	private void OnRefreshUserMenu(object data)
	{
		if (!this.showButton)
		{
			return;
		}
		bool isConnected = this.IsConnected;
		bool flag = this.toggleable.IsToggleQueued(this.toggleIdx);
		KIconButtonMenu.ButtonInfo button;
		if ((isConnected && !flag) || (!isConnected && flag))
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.DISCONNECT_TITLE, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.DISCONNECT_TOOLTIP, true);
		}
		else
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.RECONNECT_TITLE, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.RECONNECT_TOOLTIP, true);
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06004511 RID: 17681 RVA: 0x000CCA5C File Offset: 0x000CAC5C
	bool IToggleHandler.IsHandlerOn()
	{
		return this.IsConnected;
	}

	// Token: 0x04002F88 RID: 12168
	[MyCmpAdd]
	private ToggleGeothermalVentConnection toggleable;

	// Token: 0x04002F89 RID: 12169
	[MyCmpGet]
	private GeothermalVent vent;

	// Token: 0x04002F8A RID: 12170
	private int toggleIdx;

	// Token: 0x04002F8B RID: 12171
	private MeterController connectedMeter;

	// Token: 0x04002F8C RID: 12172
	public bool showButton;

	// Token: 0x04002F8D RID: 12173
	[Serialize]
	private bool connected;

	// Token: 0x04002F8E RID: 12174
	[Serialize]
	private bool toggleQueued;

	// Token: 0x04002F8F RID: 12175
	private static readonly EventSystem.IntraObjectHandler<ConnectionManager> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ConnectionManager>(delegate(ConnectionManager component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
