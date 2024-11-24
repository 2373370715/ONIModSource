using System;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001F98 RID: 8088
public class OwnablesSecondSideScreenRow : KMonoBehaviour
{
	// Token: 0x17000AEA RID: 2794
	// (get) Token: 0x0600AAC4 RID: 43716 RVA: 0x0010EEAD File Offset: 0x0010D0AD
	// (set) Token: 0x0600AAC3 RID: 43715 RVA: 0x0010EEA4 File Offset: 0x0010D0A4
	public AssignableSlotInstance minionSlotInstance { get; private set; }

	// Token: 0x17000AEB RID: 2795
	// (get) Token: 0x0600AAC6 RID: 43718 RVA: 0x0010EEBE File Offset: 0x0010D0BE
	// (set) Token: 0x0600AAC5 RID: 43717 RVA: 0x0010EEB5 File Offset: 0x0010D0B5
	public Assignable item { get; private set; }

	// Token: 0x0600AAC7 RID: 43719 RVA: 0x004070F4 File Offset: 0x004052F4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.toggle = base.GetComponent<MultiToggle>();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnMultitoggleClicked));
		this.eyeButton.onClick.AddListener(new UnityAction(this.FocusCameraOnAssignedItem));
	}

	// Token: 0x0600AAC8 RID: 43720 RVA: 0x00407158 File Offset: 0x00405358
	public void SetData(AssignableSlotInstance minion, Assignable item_assignable)
	{
		this.minionSlotInstance = minion;
		this.item = item_assignable;
		this.changeAssignmentListenerIDX = this.item.Subscribe(684616645, new Action<object>(this._OnItemAssignationChanged));
		this.destroyListenerIDX = this.item.Subscribe(1969584890, new Action<object>(this._OnRowItemDestroyed));
		this.Refresh();
	}

	// Token: 0x0600AAC9 RID: 43721 RVA: 0x004071C0 File Offset: 0x004053C0
	public void Refresh()
	{
		if (this.item != null)
		{
			this.item.PrefabID();
			string properName = this.item.GetProperName();
			this.nameLabel.text = properName;
			this.icon.sprite = Def.GetUISprite(this.item.gameObject, "ui", false).first;
			bool flag = this.item.IsAssigned() && !this.minionSlotInstance.IsUnassigning() && this.minionSlotInstance.assignable != this.item;
			if (this.item.IsAssigned())
			{
				this.statusLabel.SetText(string.Format(flag ? OwnablesSecondSideScreenRow.ASSIGNED_TO_OTHER : OwnablesSecondSideScreenRow.ASSIGNED_TO_SELF, this.item.assignee.GetProperName()));
			}
			else
			{
				this.statusLabel.SetText(OwnablesSecondSideScreenRow.NOT_ASSIGNED);
			}
			InfoDescription component = this.item.gameObject.GetComponent<InfoDescription>();
			bool flag2 = component != null && !string.IsNullOrEmpty(component.description);
			string simpleTooltip = flag2 ? component.description : properName;
			this.tooltip.SizingSetting = (flag2 ? ToolTip.ToolTipSizeSetting.MaxWidthWrapContent : ToolTip.ToolTipSizeSetting.DynamicWidthNoWrap);
			this.tooltip.SetSimpleTooltip(simpleTooltip);
		}
		else
		{
			this.nameLabel.text = OwnablesSecondSideScreenRow.NO_DATA_MESSAGE;
			this.tooltip.SetSimpleTooltip(null);
		}
		bool flag3 = this.item != null && this.minionSlotInstance != null && !this.minionSlotInstance.IsUnassigning() && this.minionSlotInstance.assignable == this.item;
		this.toggle.ChangeState(flag3 ? 1 : 0);
		this.emptyIcon.gameObject.SetActive(this.item == null);
		this.icon.gameObject.SetActive(this.item != null);
		this.eyeButton.gameObject.SetActive(this.item != null);
		this.statusLabel.gameObject.SetActive(this.item != null);
	}

	// Token: 0x0600AACA RID: 43722 RVA: 0x004073E4 File Offset: 0x004055E4
	public void ClearData()
	{
		if (this.item != null)
		{
			if (this.destroyListenerIDX != -1)
			{
				this.item.Unsubscribe(this.destroyListenerIDX);
			}
			if (this.changeAssignmentListenerIDX != -1)
			{
				this.item.Unsubscribe(this.changeAssignmentListenerIDX);
			}
		}
		this.minionSlotInstance = null;
		this.item = null;
		this.destroyListenerIDX = -1;
		this.changeAssignmentListenerIDX = -1;
		this.Refresh();
	}

	// Token: 0x0600AACB RID: 43723 RVA: 0x0010EEC6 File Offset: 0x0010D0C6
	private void _OnItemAssignationChanged(object o)
	{
		Action<OwnablesSecondSideScreenRow> onRowItemAssigneeChanged = this.OnRowItemAssigneeChanged;
		if (onRowItemAssigneeChanged == null)
		{
			return;
		}
		onRowItemAssigneeChanged(this);
	}

	// Token: 0x0600AACC RID: 43724 RVA: 0x0010EED9 File Offset: 0x0010D0D9
	private void _OnRowItemDestroyed(object o)
	{
		Action<OwnablesSecondSideScreenRow> onRowItemDestroyed = this.OnRowItemDestroyed;
		if (onRowItemDestroyed == null)
		{
			return;
		}
		onRowItemDestroyed(this);
	}

	// Token: 0x0600AACD RID: 43725 RVA: 0x0010EEEC File Offset: 0x0010D0EC
	private void OnMultitoggleClicked()
	{
		Action<OwnablesSecondSideScreenRow> onRowClicked = this.OnRowClicked;
		if (onRowClicked == null)
		{
			return;
		}
		onRowClicked(this);
	}

	// Token: 0x0600AACE RID: 43726 RVA: 0x00407458 File Offset: 0x00405658
	private void FocusCameraOnAssignedItem()
	{
		if (this.item != null)
		{
			GameObject gameObject = this.item.gameObject;
			if (this.item.HasTag(GameTags.Equipped))
			{
				gameObject = this.item.assignee.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
			}
			GameUtil.FocusCamera(gameObject.transform, false);
		}
	}

	// Token: 0x0400862D RID: 34349
	public static string NO_DATA_MESSAGE = UI.UISIDESCREENS.OWNABLESSIDESCREEN.NO_ITEM_FOUND;

	// Token: 0x0400862E RID: 34350
	public static string NOT_ASSIGNED = UI.UISIDESCREENS.OWNABLESSECONDSIDESCREEN.NOT_ASSIGNED;

	// Token: 0x0400862F RID: 34351
	public static string ASSIGNED_TO_SELF = UI.UISIDESCREENS.OWNABLESSECONDSIDESCREEN.ASSIGNED_TO_SELF_STATUS;

	// Token: 0x04008630 RID: 34352
	public static string ASSIGNED_TO_OTHER = UI.UISIDESCREENS.OWNABLESSECONDSIDESCREEN.ASSIGNED_TO_OTHER_STATUS;

	// Token: 0x04008631 RID: 34353
	public KImage icon;

	// Token: 0x04008632 RID: 34354
	public KImage emptyIcon;

	// Token: 0x04008633 RID: 34355
	public LocText nameLabel;

	// Token: 0x04008634 RID: 34356
	public LocText statusLabel;

	// Token: 0x04008635 RID: 34357
	public Button eyeButton;

	// Token: 0x04008636 RID: 34358
	public ToolTip tooltip;

	// Token: 0x04008637 RID: 34359
	public Action<OwnablesSecondSideScreenRow> OnRowItemAssigneeChanged;

	// Token: 0x04008638 RID: 34360
	public Action<OwnablesSecondSideScreenRow> OnRowItemDestroyed;

	// Token: 0x04008639 RID: 34361
	public Action<OwnablesSecondSideScreenRow> OnRowClicked;

	// Token: 0x0400863C RID: 34364
	private MultiToggle toggle;

	// Token: 0x0400863D RID: 34365
	private int changeAssignmentListenerIDX = -1;

	// Token: 0x0400863E RID: 34366
	private int destroyListenerIDX = -1;
}
