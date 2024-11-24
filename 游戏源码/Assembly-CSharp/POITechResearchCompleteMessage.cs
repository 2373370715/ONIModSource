using KSerialization;
using STRINGS;

public class POITechResearchCompleteMessage : Message
{
	[Serialize]
	public POITechItemUnlocks.Def unlockedItemsdef;

	[Serialize]
	public string popupName;

	[Serialize]
	public string animName;

	public POITechResearchCompleteMessage()
	{
	}

	public POITechResearchCompleteMessage(POITechItemUnlocks.Def unlocked_items)
	{
		unlockedItemsdef = unlocked_items;
		popupName = unlocked_items.PopUpName;
		animName = unlocked_items.animName;
	}

	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	public override string GetMessageBody()
	{
		string text = "";
		for (int i = 0; i < unlockedItemsdef.POITechUnlockIDs.Count; i++)
		{
			TechItem techItem = Db.Get().TechItems.TryGet(unlockedItemsdef.POITechUnlockIDs[i]);
			if (techItem != null)
			{
				text = text + "\n    • " + techItem.Name;
			}
		}
		return string.Format(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.MESSAGEBODY, text);
	}

	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.NAME;
	}

	public override string GetTooltip()
	{
		return string.Format(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.TOOLTIP, popupName);
	}

	public override bool IsValid()
	{
		return unlockedItemsdef != null;
	}

	public override bool ShowDialog()
	{
		EventInfoData eventInfoData = new EventInfoData(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.NAME, GetMessageBody(), animName);
		eventInfoData.AddDefaultOption();
		EventInfoScreen.ShowPopup(eventInfoData);
		Messenger.Instance.RemoveMessage(this);
		return false;
	}

	public override bool ShowDismissButton()
	{
		return false;
	}

	public override NotificationType GetMessageType()
	{
		return NotificationType.Messages;
	}
}
