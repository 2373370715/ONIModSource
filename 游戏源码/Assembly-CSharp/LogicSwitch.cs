using System;
using System.Collections;
using KSerialization;
using UnityEngine;

// Token: 0x02000E65 RID: 3685
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicSwitch : Switch, IPlayerControlledToggle, ISim33ms
{
	// Token: 0x060049C7 RID: 18887 RVA: 0x000CFC97 File Offset: 0x000CDE97
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicSwitch>(-905833192, LogicSwitch.OnCopySettingsDelegate);
	}

	// Token: 0x060049C8 RID: 18888 RVA: 0x0025953C File Offset: 0x0025773C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.wasOn = this.switchedOn;
		this.UpdateLogicCircuit();
		base.GetComponent<KBatchedAnimController>().Play(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060049C9 RID: 18889 RVA: 0x000BFD4F File Offset: 0x000BDF4F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060049CA RID: 18890 RVA: 0x00259590 File Offset: 0x00257790
	private void OnCopySettings(object data)
	{
		LogicSwitch component = ((GameObject)data).GetComponent<LogicSwitch>();
		if (component != null && this.switchedOn != component.switchedOn)
		{
			this.switchedOn = component.switchedOn;
			this.UpdateVisualization();
			this.UpdateLogicCircuit();
		}
	}

	// Token: 0x060049CB RID: 18891 RVA: 0x000CFCB0 File Offset: 0x000CDEB0
	protected override void Toggle()
	{
		base.Toggle();
		this.UpdateVisualization();
		this.UpdateLogicCircuit();
	}

	// Token: 0x060049CC RID: 18892 RVA: 0x002595D8 File Offset: 0x002577D8
	private void UpdateVisualization()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (this.wasOn != this.switchedOn)
		{
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
		this.wasOn = this.switchedOn;
	}

	// Token: 0x060049CD RID: 18893 RVA: 0x000CA11E File Offset: 0x000C831E
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060049CE RID: 18894 RVA: 0x0025965C File Offset: 0x0025785C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSwitchStatusActive : Db.Get().BuildingStatusItems.LogicSwitchStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x060049CF RID: 18895 RVA: 0x000CFCC4 File Offset: 0x000CDEC4
	public void Sim33ms(float dt)
	{
		if (this.ToggleRequested)
		{
			this.Toggle();
			this.ToggleRequested = false;
			this.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
	}

	// Token: 0x060049D0 RID: 18896 RVA: 0x000CFCF8 File Offset: 0x000CDEF8
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x060049D1 RID: 18897 RVA: 0x000CFD0E File Offset: 0x000CDF0E
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060049D2 RID: 18898 RVA: 0x000C92F7 File Offset: 0x000C74F7
	public void ToggledByPlayer()
	{
		this.Toggle();
	}

	// Token: 0x060049D3 RID: 18899 RVA: 0x000C92FF File Offset: 0x000C74FF
	public bool ToggledOn()
	{
		return this.switchedOn;
	}

	// Token: 0x060049D4 RID: 18900 RVA: 0x000C9307 File Offset: 0x000C7507
	public KSelectable GetSelectable()
	{
		return base.GetComponent<KSelectable>();
	}

	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x060049D5 RID: 18901 RVA: 0x000CFD1D File Offset: 0x000CDF1D
	public string SideScreenTitleKey
	{
		get
		{
			return "STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.SIDESCREEN_TITLE";
		}
	}

	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x060049D6 RID: 18902 RVA: 0x000CFD24 File Offset: 0x000CDF24
	// (set) Token: 0x060049D7 RID: 18903 RVA: 0x000CFD2C File Offset: 0x000CDF2C
	public bool ToggleRequested { get; set; }

	// Token: 0x04003353 RID: 13139
	public static readonly HashedString PORT_ID = "LogicSwitch";

	// Token: 0x04003354 RID: 13140
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003355 RID: 13141
	private static readonly EventSystem.IntraObjectHandler<LogicSwitch> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicSwitch>(delegate(LogicSwitch component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04003356 RID: 13142
	private bool wasOn;

	// Token: 0x04003357 RID: 13143
	private System.Action firstFrameCallback;
}
