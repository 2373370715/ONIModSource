using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001F1F RID: 7967
[AddComponentMenu("KMonoBehaviour/scripts/AccessControlSideScreenDoor")]
public class AccessControlSideScreenDoor : KMonoBehaviour
{
	// Token: 0x0600A802 RID: 43010 RVA: 0x0010D097 File Offset: 0x0010B297
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.leftButton.onClick += this.OnPermissionButtonClicked;
		this.rightButton.onClick += this.OnPermissionButtonClicked;
	}

	// Token: 0x0600A803 RID: 43011 RVA: 0x003FB548 File Offset: 0x003F9748
	private void OnPermissionButtonClicked()
	{
		AccessControl.Permission arg;
		if (this.leftButton.isOn)
		{
			if (this.rightButton.isOn)
			{
				arg = AccessControl.Permission.Both;
			}
			else
			{
				arg = AccessControl.Permission.GoLeft;
			}
		}
		else if (this.rightButton.isOn)
		{
			arg = AccessControl.Permission.GoRight;
		}
		else
		{
			arg = AccessControl.Permission.Neither;
		}
		this.UpdateButtonStates(false);
		this.permissionChangedCallback(this.targetIdentity, arg);
	}

	// Token: 0x0600A804 RID: 43012 RVA: 0x003FB5A4 File Offset: 0x003F97A4
	protected virtual void UpdateButtonStates(bool isDefault)
	{
		ToolTip component = this.leftButton.GetComponent<ToolTip>();
		ToolTip component2 = this.rightButton.GetComponent<ToolTip>();
		if (this.isUpDown)
		{
			component.SetSimpleTooltip(this.leftButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_UP_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_UP_DISABLED);
			component2.SetSimpleTooltip(this.rightButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_DOWN_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_DOWN_DISABLED);
			return;
		}
		component.SetSimpleTooltip(this.leftButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_LEFT_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_LEFT_DISABLED);
		component2.SetSimpleTooltip(this.rightButton.isOn ? UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_RIGHT_ENABLED : UI.UISIDESCREENS.ACCESS_CONTROL_SIDE_SCREEN.GO_RIGHT_DISABLED);
	}

	// Token: 0x0600A805 RID: 43013 RVA: 0x0010D0CD File Offset: 0x0010B2CD
	public void SetRotated(bool rotated)
	{
		this.isUpDown = rotated;
	}

	// Token: 0x0600A806 RID: 43014 RVA: 0x0010D0D6 File Offset: 0x0010B2D6
	public void SetContent(AccessControl.Permission permission, Action<MinionAssignablesProxy, AccessControl.Permission> onPermissionChange)
	{
		this.permissionChangedCallback = onPermissionChange;
		this.leftButton.isOn = (permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoLeft);
		this.rightButton.isOn = (permission == AccessControl.Permission.Both || permission == AccessControl.Permission.GoRight);
		this.UpdateButtonStates(false);
	}

	// Token: 0x0400841B RID: 33819
	public KToggle leftButton;

	// Token: 0x0400841C RID: 33820
	public KToggle rightButton;

	// Token: 0x0400841D RID: 33821
	private Action<MinionAssignablesProxy, AccessControl.Permission> permissionChangedCallback;

	// Token: 0x0400841E RID: 33822
	private bool isUpDown;

	// Token: 0x0400841F RID: 33823
	protected MinionAssignablesProxy targetIdentity;
}
