using System;
using Database;
using UnityEngine;

public readonly struct JoyResponseScreenConfig
{
	public readonly JoyResponseOutfitTarget target;

	public readonly Option<JoyResponseDesignerScreen.GalleryItem> initalSelectedItem;

	public readonly bool isValid;

	private JoyResponseScreenConfig(JoyResponseOutfitTarget target, Option<JoyResponseDesignerScreen.GalleryItem> initalSelectedItem)
	{
		this.target = target;
		this.initalSelectedItem = initalSelectedItem;
		isValid = true;
	}

	public JoyResponseScreenConfig WithInitialSelection(Option<BalloonArtistFacadeResource> initialSelectedItem)
	{
		return new JoyResponseScreenConfig(target, JoyResponseDesignerScreen.GalleryItem.Of(initialSelectedItem));
	}

	public static JoyResponseScreenConfig Minion(GameObject minionInstance)
	{
		return new JoyResponseScreenConfig(JoyResponseOutfitTarget.FromMinion(minionInstance), Option.None);
	}

	public static JoyResponseScreenConfig Personality(Personality personality)
	{
		return new JoyResponseScreenConfig(JoyResponseOutfitTarget.FromPersonality(personality), Option.None);
	}

	public static JoyResponseScreenConfig From(MinionBrowserScreen.GridItem item)
	{
		if (item is MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget)
		{
			return Personality(personalityTarget.personality);
		}
		if (item is MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget)
		{
			return Minion(minionInstanceTarget.minionInstance);
		}
		throw new NotImplementedException();
	}

	public void ApplyAndOpenScreen()
	{
		LockerNavigator.Instance.joyResponseDesignerScreen.GetComponent<JoyResponseDesignerScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.joyResponseDesignerScreen);
	}
}
