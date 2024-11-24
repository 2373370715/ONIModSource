using System;
using KSerialization;
using STRINGS;

// Token: 0x02001DFE RID: 7678
public class POITechResearchCompleteMessage : Message
{
	// Token: 0x0600A0B5 RID: 41141 RVA: 0x001082E7 File Offset: 0x001064E7
	public POITechResearchCompleteMessage()
	{
	}

	// Token: 0x0600A0B6 RID: 41142 RVA: 0x00108786 File Offset: 0x00106986
	public POITechResearchCompleteMessage(POITechItemUnlocks.Def unlocked_items)
	{
		this.unlockedItemsdef = unlocked_items;
		this.popupName = unlocked_items.PopUpName;
		this.animName = unlocked_items.animName;
	}

	// Token: 0x0600A0B7 RID: 41143 RVA: 0x001082A2 File Offset: 0x001064A2
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x0600A0B8 RID: 41144 RVA: 0x003D6030 File Offset: 0x003D4230
	public override string GetMessageBody()
	{
		string text = "";
		for (int i = 0; i < this.unlockedItemsdef.POITechUnlockIDs.Count; i++)
		{
			TechItem techItem = Db.Get().TechItems.TryGet(this.unlockedItemsdef.POITechUnlockIDs[i]);
			if (techItem != null)
			{
				text = text + "\n    • " + techItem.Name;
			}
		}
		return string.Format(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.MESSAGEBODY, text);
	}

	// Token: 0x0600A0B9 RID: 41145 RVA: 0x001087B2 File Offset: 0x001069B2
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.NAME;
	}

	// Token: 0x0600A0BA RID: 41146 RVA: 0x001087BE File Offset: 0x001069BE
	public override string GetTooltip()
	{
		return string.Format(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.TOOLTIP, this.popupName);
	}

	// Token: 0x0600A0BB RID: 41147 RVA: 0x001087D5 File Offset: 0x001069D5
	public override bool IsValid()
	{
		return this.unlockedItemsdef != null;
	}

	// Token: 0x0600A0BC RID: 41148 RVA: 0x001087E0 File Offset: 0x001069E0
	public override bool ShowDialog()
	{
		EventInfoData eventInfoData = new EventInfoData(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.NAME, this.GetMessageBody(), this.animName);
		eventInfoData.AddDefaultOption(null);
		EventInfoScreen.ShowPopup(eventInfoData);
		Messenger.Instance.RemoveMessage(this);
		return false;
	}

	// Token: 0x0600A0BD RID: 41149 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public override bool ShowDismissButton()
	{
		return false;
	}

	// Token: 0x0600A0BE RID: 41150 RVA: 0x000AD486 File Offset: 0x000AB686
	public override NotificationType GetMessageType()
	{
		return NotificationType.Messages;
	}

	// Token: 0x04007D90 RID: 32144
	[Serialize]
	public POITechItemUnlocks.Def unlockedItemsdef;

	// Token: 0x04007D91 RID: 32145
	[Serialize]
	public string popupName;

	// Token: 0x04007D92 RID: 32146
	[Serialize]
	public string animName;
}
