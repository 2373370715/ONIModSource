using System;
using UnityEngine;

// Token: 0x02001CC1 RID: 7361
public class EventInfoDataHelper
{
	// Token: 0x060099C5 RID: 39365 RVA: 0x003B6634 File Offset: 0x003B4834
	public static EventInfoData GenerateStoryTraitData(string titleText, string descriptionText, string buttonText, string animFileName, EventInfoDataHelper.PopupType popupType, string buttonTooltip = null, GameObject[] minions = null, System.Action callback = null)
	{
		EventInfoData eventInfoData = new EventInfoData(titleText, descriptionText, animFileName);
		eventInfoData.minions = minions;
		if (popupType <= EventInfoDataHelper.PopupType.NORMAL || popupType != EventInfoDataHelper.PopupType.COMPLETE)
		{
			eventInfoData.showCallback = delegate()
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound("StoryTrait_Activation_Popup", false));
			};
		}
		else
		{
			eventInfoData.showCallback = delegate()
			{
				MusicManager.instance.PlaySong("Stinger_StoryTraitUnlock", false);
			};
		}
		EventInfoData.Option option = eventInfoData.AddOption(buttonText, null);
		option.callback = callback;
		option.tooltip = buttonTooltip;
		return eventInfoData;
	}

	// Token: 0x02001CC2 RID: 7362
	public enum PopupType
	{
		// Token: 0x040077FB RID: 30715
		NONE = -1,
		// Token: 0x040077FC RID: 30716
		BEGIN,
		// Token: 0x040077FD RID: 30717
		NORMAL,
		// Token: 0x040077FE RID: 30718
		COMPLETE
	}
}
