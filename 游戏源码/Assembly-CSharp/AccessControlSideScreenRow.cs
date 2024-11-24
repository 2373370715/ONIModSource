using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001F20 RID: 7968
public class AccessControlSideScreenRow : AccessControlSideScreenDoor
{
	// Token: 0x0600A808 RID: 43016 RVA: 0x0010D110 File Offset: 0x0010B310
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.defaultButton.onValueChanged += this.OnDefaultButtonChanged;
	}

	// Token: 0x0600A809 RID: 43017 RVA: 0x0010D12F File Offset: 0x0010B32F
	private void OnDefaultButtonChanged(bool state)
	{
		this.UpdateButtonStates(!state);
		if (this.defaultClickedCallback != null)
		{
			this.defaultClickedCallback(this.targetIdentity, !state);
		}
	}

	// Token: 0x0600A80A RID: 43018 RVA: 0x003FB664 File Offset: 0x003F9864
	protected override void UpdateButtonStates(bool isDefault)
	{
		base.UpdateButtonStates(isDefault);
		this.defaultButton.GetComponent<ToolTip>().SetSimpleTooltip(isDefault ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.SET_TO_CUSTOM : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.SET_TO_DEFAULT);
		this.defaultControls.SetActive(isDefault);
		this.customControls.SetActive(!isDefault);
	}

	// Token: 0x0600A80B RID: 43019 RVA: 0x003FB6B8 File Offset: 0x003F98B8
	public void SetMinionContent(MinionAssignablesProxy identity, AccessControl.Permission permission, bool isDefault, Action<MinionAssignablesProxy, AccessControl.Permission> onPermissionChange, Action<MinionAssignablesProxy, bool> onDefaultClick)
	{
		base.SetContent(permission, onPermissionChange);
		if (identity == null)
		{
			global::Debug.LogError("Invalid data received.");
			return;
		}
		if (this.portraitInstance == null)
		{
			this.portraitInstance = Util.KInstantiateUI<CrewPortrait>(this.crewPortraitPrefab.gameObject, this.defaultButton.gameObject, false);
			this.portraitInstance.SetAlpha(1f);
		}
		this.targetIdentity = identity;
		this.portraitInstance.SetIdentityObject(identity, false);
		this.portraitInstance.SetSubTitle(isDefault ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.USING_DEFAULT : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.USING_CUSTOM);
		this.defaultClickedCallback = null;
		this.defaultButton.isOn = !isDefault;
		this.defaultClickedCallback = onDefaultClick;
	}

	// Token: 0x04008420 RID: 33824
	[SerializeField]
	private CrewPortrait crewPortraitPrefab;

	// Token: 0x04008421 RID: 33825
	private CrewPortrait portraitInstance;

	// Token: 0x04008422 RID: 33826
	public KToggle defaultButton;

	// Token: 0x04008423 RID: 33827
	public GameObject defaultControls;

	// Token: 0x04008424 RID: 33828
	public GameObject customControls;

	// Token: 0x04008425 RID: 33829
	private Action<MinionAssignablesProxy, bool> defaultClickedCallback;
}
