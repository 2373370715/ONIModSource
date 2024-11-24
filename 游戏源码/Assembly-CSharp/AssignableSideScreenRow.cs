using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001F2B RID: 7979
[AddComponentMenu("KMonoBehaviour/scripts/AssignableSideScreenRow")]
public class AssignableSideScreenRow : KMonoBehaviour
{
	// Token: 0x0600A865 RID: 43109 RVA: 0x003FCD04 File Offset: 0x003FAF04
	public void Refresh(object data = null)
	{
		if (!this.sideScreen.targetAssignable.CanAssignTo(this.targetIdentity))
		{
			this.currentState = AssignableSideScreenRow.AssignableState.Disabled;
			this.assignmentText.text = UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.DISABLED;
		}
		else if (this.sideScreen.targetAssignable.assignee == this.targetIdentity)
		{
			this.currentState = AssignableSideScreenRow.AssignableState.Selected;
			this.assignmentText.text = UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.ASSIGNED;
		}
		else
		{
			bool flag = false;
			KMonoBehaviour kmonoBehaviour = this.targetIdentity as KMonoBehaviour;
			if (kmonoBehaviour != null)
			{
				Ownables component = kmonoBehaviour.GetComponent<Ownables>();
				if (component != null)
				{
					AssignableSlotInstance[] slots = component.GetSlots(this.sideScreen.targetAssignable.slot);
					if (slots != null && slots.Length != 0)
					{
						AssignableSlotInstance assignableSlotInstance = slots.FindFirst((AssignableSlotInstance s) => !s.IsAssigned());
						if (assignableSlotInstance == null)
						{
							assignableSlotInstance = slots[0];
						}
						if (assignableSlotInstance != null && assignableSlotInstance.IsAssigned())
						{
							this.currentState = AssignableSideScreenRow.AssignableState.AssignedToOther;
							this.assignmentText.text = assignableSlotInstance.assignable.GetProperName();
							flag = true;
						}
					}
				}
				Equipment component2 = kmonoBehaviour.GetComponent<Equipment>();
				if (component2 != null)
				{
					AssignableSlotInstance[] slots2 = component2.GetSlots(this.sideScreen.targetAssignable.slot);
					if (slots2 != null && slots2.Length != 0)
					{
						AssignableSlotInstance assignableSlotInstance2 = slots2.FindFirst((AssignableSlotInstance s) => !s.IsAssigned());
						if (assignableSlotInstance2 == null)
						{
							assignableSlotInstance2 = slots2[0];
						}
						if (assignableSlotInstance2 != null && assignableSlotInstance2.IsAssigned())
						{
							this.currentState = AssignableSideScreenRow.AssignableState.AssignedToOther;
							this.assignmentText.text = assignableSlotInstance2.assignable.GetProperName();
							flag = true;
						}
					}
				}
			}
			if (!flag)
			{
				this.currentState = AssignableSideScreenRow.AssignableState.Unassigned;
				this.assignmentText.text = UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGNED;
			}
		}
		this.toggle.ChangeState((int)this.currentState);
	}

	// Token: 0x0600A866 RID: 43110 RVA: 0x0010D67F File Offset: 0x0010B87F
	protected override void OnCleanUp()
	{
		if (this.refreshHandle == -1)
		{
			Game.Instance.Unsubscribe(this.refreshHandle);
		}
		base.OnCleanUp();
	}

	// Token: 0x0600A867 RID: 43111 RVA: 0x003FCEF0 File Offset: 0x003FB0F0
	public void SetContent(IAssignableIdentity identity_object, Action<IAssignableIdentity> selectionCallback, AssignableSideScreen assignableSideScreen)
	{
		if (this.refreshHandle == -1)
		{
			Game.Instance.Unsubscribe(this.refreshHandle);
		}
		this.refreshHandle = Game.Instance.Subscribe(-2146166042, delegate(object o)
		{
			if (this != null && this.gameObject != null && this.gameObject.activeInHierarchy)
			{
				this.Refresh(null);
			}
		});
		this.toggle = base.GetComponent<MultiToggle>();
		this.sideScreen = assignableSideScreen;
		this.targetIdentity = identity_object;
		if (this.portraitInstance == null)
		{
			this.portraitInstance = Util.KInstantiateUI<CrewPortrait>(this.crewPortraitPrefab.gameObject, base.gameObject, false);
			this.portraitInstance.transform.SetSiblingIndex(1);
			this.portraitInstance.SetAlpha(1f);
		}
		this.toggle.onClick = delegate()
		{
			selectionCallback(this.targetIdentity);
		};
		this.portraitInstance.SetIdentityObject(identity_object, false);
		base.GetComponent<ToolTip>().OnToolTip = new Func<string>(this.GetTooltip);
		this.Refresh(null);
	}

	// Token: 0x0600A868 RID: 43112 RVA: 0x003FCFF4 File Offset: 0x003FB1F4
	private string GetTooltip()
	{
		ToolTip component = base.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		if (this.targetIdentity != null && !this.targetIdentity.IsNull())
		{
			AssignableSideScreenRow.AssignableState assignableState = this.currentState;
			if (assignableState != AssignableSideScreenRow.AssignableState.Selected)
			{
				if (assignableState != AssignableSideScreenRow.AssignableState.Disabled)
				{
					component.AddMultiStringTooltip(string.Format(UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.ASSIGN_TO_TOOLTIP, this.targetIdentity.GetProperName()), null);
				}
				else
				{
					component.AddMultiStringTooltip(string.Format(UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.DISABLED_TOOLTIP, this.targetIdentity.GetProperName()), null);
				}
			}
			else
			{
				component.AddMultiStringTooltip(string.Format(UI.UISIDESCREENS.ASSIGNABLESIDESCREEN.UNASSIGN_TOOLTIP, this.targetIdentity.GetProperName()), null);
			}
		}
		return "";
	}

	// Token: 0x0400845B RID: 33883
	[SerializeField]
	private CrewPortrait crewPortraitPrefab;

	// Token: 0x0400845C RID: 33884
	[SerializeField]
	private LocText assignmentText;

	// Token: 0x0400845D RID: 33885
	public AssignableSideScreen sideScreen;

	// Token: 0x0400845E RID: 33886
	private CrewPortrait portraitInstance;

	// Token: 0x0400845F RID: 33887
	[MyCmpReq]
	private MultiToggle toggle;

	// Token: 0x04008460 RID: 33888
	public IAssignableIdentity targetIdentity;

	// Token: 0x04008461 RID: 33889
	public AssignableSideScreenRow.AssignableState currentState;

	// Token: 0x04008462 RID: 33890
	private int refreshHandle = -1;

	// Token: 0x02001F2C RID: 7980
	public enum AssignableState
	{
		// Token: 0x04008464 RID: 33892
		Selected,
		// Token: 0x04008465 RID: 33893
		AssignedToOther,
		// Token: 0x04008466 RID: 33894
		Unassigned,
		// Token: 0x04008467 RID: 33895
		Disabled
	}
}
