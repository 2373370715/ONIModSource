using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000CCD RID: 3277
[SerializationConfig(MemberSerialization.OptIn)]
public class CircuitSwitch : Switch, IPlayerControlledToggle, ISim33ms
{
	// Token: 0x06003F5E RID: 16222 RVA: 0x000C9275 File Offset: 0x000C7475
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<CircuitSwitch>(-905833192, CircuitSwitch.OnCopySettingsDelegate);
	}

	// Token: 0x06003F5F RID: 16223 RVA: 0x002374C8 File Offset: 0x002356C8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.CircuitOnToggle;
		int cell = Grid.PosToCell(base.transform.GetPosition());
		GameObject gameObject = Grid.Objects[cell, (int)this.objectLayer];
		Wire wire = (gameObject != null) ? gameObject.GetComponent<Wire>() : null;
		if (wire == null)
		{
			this.wireConnectedGUID = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, null);
		}
		this.AttachWire(wire);
		this.wasOn = this.switchedOn;
		this.UpdateCircuit(true);
		base.GetComponent<KBatchedAnimController>().Play(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06003F60 RID: 16224 RVA: 0x00237598 File Offset: 0x00235798
	protected override void OnCleanUp()
	{
		if (this.attachedWire != null)
		{
			this.UnsubscribeFromWire(this.attachedWire);
		}
		bool switchedOn = this.switchedOn;
		this.switchedOn = true;
		this.UpdateCircuit(false);
		this.switchedOn = switchedOn;
	}

	// Token: 0x06003F61 RID: 16225 RVA: 0x002375DC File Offset: 0x002357DC
	private void OnCopySettings(object data)
	{
		CircuitSwitch component = ((GameObject)data).GetComponent<CircuitSwitch>();
		if (component != null)
		{
			this.switchedOn = component.switchedOn;
			this.UpdateCircuit(true);
		}
	}

	// Token: 0x06003F62 RID: 16226 RVA: 0x00237614 File Offset: 0x00235814
	public bool IsConnected()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		GameObject gameObject = Grid.Objects[cell, (int)this.objectLayer];
		return gameObject != null && gameObject.GetComponent<IDisconnectable>() != null;
	}

	// Token: 0x06003F63 RID: 16227 RVA: 0x000C928E File Offset: 0x000C748E
	private void CircuitOnToggle(bool on)
	{
		this.UpdateCircuit(true);
	}

	// Token: 0x06003F64 RID: 16228 RVA: 0x00237658 File Offset: 0x00235858
	public void AttachWire(Wire wire)
	{
		if (wire == this.attachedWire)
		{
			return;
		}
		if (this.attachedWire != null)
		{
			this.UnsubscribeFromWire(this.attachedWire);
		}
		this.attachedWire = wire;
		if (this.attachedWire != null)
		{
			this.SubscribeToWire(this.attachedWire);
			this.UpdateCircuit(true);
			this.wireConnectedGUID = base.GetComponent<KSelectable>().RemoveStatusItem(this.wireConnectedGUID, false);
			return;
		}
		if (this.wireConnectedGUID == Guid.Empty)
		{
			this.wireConnectedGUID = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, null);
		}
	}

	// Token: 0x06003F65 RID: 16229 RVA: 0x000C9297 File Offset: 0x000C7497
	private void OnWireDestroyed(object data)
	{
		if (this.attachedWire != null)
		{
			this.attachedWire.Unsubscribe(1969584890, new Action<object>(this.OnWireDestroyed));
		}
	}

	// Token: 0x06003F66 RID: 16230 RVA: 0x000C928E File Offset: 0x000C748E
	private void OnWireStateChanged(object data)
	{
		this.UpdateCircuit(true);
	}

	// Token: 0x06003F67 RID: 16231 RVA: 0x00237704 File Offset: 0x00235904
	private void SubscribeToWire(Wire wire)
	{
		wire.Subscribe(1969584890, new Action<object>(this.OnWireDestroyed));
		wire.Subscribe(-1735440190, new Action<object>(this.OnWireStateChanged));
		wire.Subscribe(774203113, new Action<object>(this.OnWireStateChanged));
	}

	// Token: 0x06003F68 RID: 16232 RVA: 0x0023775C File Offset: 0x0023595C
	private void UnsubscribeFromWire(Wire wire)
	{
		wire.Unsubscribe(1969584890, new Action<object>(this.OnWireDestroyed));
		wire.Unsubscribe(-1735440190, new Action<object>(this.OnWireStateChanged));
		wire.Unsubscribe(774203113, new Action<object>(this.OnWireStateChanged));
	}

	// Token: 0x06003F69 RID: 16233 RVA: 0x002377B0 File Offset: 0x002359B0
	private void UpdateCircuit(bool should_update_anim = true)
	{
		if (this.attachedWire != null)
		{
			if (this.switchedOn)
			{
				this.attachedWire.Connect();
			}
			else
			{
				this.attachedWire.Disconnect();
			}
		}
		if (should_update_anim && this.wasOn != this.switchedOn)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
			Game.Instance.userMenu.Refresh(base.gameObject);
		}
		this.wasOn = this.switchedOn;
	}

	// Token: 0x06003F6A RID: 16234 RVA: 0x000C92C3 File Offset: 0x000C74C3
	public void Sim33ms(float dt)
	{
		if (this.ToggleRequested)
		{
			this.Toggle();
			this.ToggleRequested = false;
			this.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
	}

	// Token: 0x06003F6B RID: 16235 RVA: 0x000C92F7 File Offset: 0x000C74F7
	public void ToggledByPlayer()
	{
		this.Toggle();
	}

	// Token: 0x06003F6C RID: 16236 RVA: 0x000C92FF File Offset: 0x000C74FF
	public bool ToggledOn()
	{
		return this.switchedOn;
	}

	// Token: 0x06003F6D RID: 16237 RVA: 0x000C9307 File Offset: 0x000C7507
	public KSelectable GetSelectable()
	{
		return base.GetComponent<KSelectable>();
	}

	// Token: 0x17000304 RID: 772
	// (get) Token: 0x06003F6E RID: 16238 RVA: 0x000C930F File Offset: 0x000C750F
	public string SideScreenTitleKey
	{
		get
		{
			return "STRINGS.BUILDINGS.PREFABS.SWITCH.SIDESCREEN_TITLE";
		}
	}

	// Token: 0x17000305 RID: 773
	// (get) Token: 0x06003F6F RID: 16239 RVA: 0x000C9316 File Offset: 0x000C7516
	// (set) Token: 0x06003F70 RID: 16240 RVA: 0x000C931E File Offset: 0x000C751E
	public bool ToggleRequested { get; set; }

	// Token: 0x04002B46 RID: 11078
	[SerializeField]
	public ObjectLayer objectLayer;

	// Token: 0x04002B47 RID: 11079
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04002B48 RID: 11080
	private static readonly EventSystem.IntraObjectHandler<CircuitSwitch> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<CircuitSwitch>(delegate(CircuitSwitch component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04002B49 RID: 11081
	private Wire attachedWire;

	// Token: 0x04002B4A RID: 11082
	private Guid wireConnectedGUID;

	// Token: 0x04002B4B RID: 11083
	private bool wasOn;
}
