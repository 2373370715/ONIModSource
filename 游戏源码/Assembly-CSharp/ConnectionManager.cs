using KSerialization;
using STRINGS;

public class ConnectionManager : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
	[MyCmpAdd]
	private ToggleGeothermalVentConnection toggleable;

	[MyCmpGet]
	private GeothermalVent vent;

	private int toggleIdx;

	private MeterController connectedMeter;

	public bool showButton;

	[Serialize]
	private bool connected;

	[Serialize]
	private bool toggleQueued;

	private static readonly EventSystem.IntraObjectHandler<ConnectionManager> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ConnectionManager>(delegate(ConnectionManager component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	public bool IsConnected
	{
		get
		{
			return connected;
		}
		set
		{
			connected = value;
			connectedMeter.SetPositionPercent(value ? 1f : 0f);
		}
	}

	public bool WaitingForToggle => toggleQueued;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		toggleIdx = toggleable.SetTarget(this);
		Subscribe(493375141, OnRefreshUserMenuDelegate);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (toggleQueued)
		{
			OnMenuToggle();
		}
		connectedMeter = new MeterController(GetComponent<KBatchedAnimController>(), "meter_connected_target", "meter_connected", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalVentConfig.CONNECTED_SYMBOLS);
		connectedMeter.SetPositionPercent(IsConnected ? 1f : 0f);
	}

	public void HandleToggle()
	{
		toggleQueued = false;
		Prioritizable.RemoveRef(base.gameObject);
		OnToggle();
	}

	private void OnToggle()
	{
		IsConnected = !IsConnected;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	private void OnMenuToggle()
	{
		if (!toggleable.IsToggleQueued(toggleIdx))
		{
			if (IsConnected)
			{
				Trigger(2108245096, "BuildingDisabled");
			}
			toggleQueued = true;
			Prioritizable.AddRef(base.gameObject);
		}
		else
		{
			toggleQueued = false;
			Prioritizable.RemoveRef(base.gameObject);
		}
		toggleable.Toggle(toggleIdx);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	private void OnRefreshUserMenu(object data)
	{
		if (showButton)
		{
			bool isConnected = IsConnected;
			bool flag = toggleable.IsToggleQueued(toggleIdx);
			KIconButtonMenu.ButtonInfo buttonInfo = null;
			buttonInfo = (((!isConnected || flag) && !(!isConnected && flag)) ? new KIconButtonMenu.ButtonInfo("action_building_disabled", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.RECONNECT_TITLE, OnMenuToggle, Action.ToggleEnabled, null, null, null, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.RECONNECT_TOOLTIP) : new KIconButtonMenu.ButtonInfo("action_building_disabled", COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.DISCONNECT_TITLE, OnMenuToggle, Action.ToggleEnabled, null, null, null, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.BUTTONS.DISCONNECT_TOOLTIP));
			Game.Instance.userMenu.AddButton(base.gameObject, buttonInfo);
		}
	}

	bool IToggleHandler.IsHandlerOn()
	{
		return IsConnected;
	}
}
