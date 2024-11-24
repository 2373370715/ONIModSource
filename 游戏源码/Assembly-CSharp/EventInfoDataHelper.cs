using System;
using UnityEngine;

public class EventInfoDataHelper
{
	public enum PopupType
	{
		NONE = -1,
		BEGIN,
		NORMAL,
		COMPLETE
	}

	public static EventInfoData GenerateStoryTraitData(string titleText, string descriptionText, string buttonText, string animFileName, PopupType popupType, string buttonTooltip = null, GameObject[] minions = null, System.Action callback = null)
	{
		EventInfoData eventInfoData = new EventInfoData(titleText, descriptionText, animFileName);
		eventInfoData.minions = minions;
		if ((uint)popupType <= 1u || popupType != PopupType.COMPLETE)
		{
			eventInfoData.showCallback = delegate
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound("StoryTrait_Activation_Popup"));
			};
		}
		else
		{
			eventInfoData.showCallback = delegate
			{
				MusicManager.instance.PlaySong("Stinger_StoryTraitUnlock");
			};
		}
		EventInfoData.Option option = eventInfoData.AddOption(buttonText);
		option.callback = callback;
		option.tooltip = buttonTooltip;
		return eventInfoData;
	}
}
