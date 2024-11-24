﻿using System;
using System.Linq;
using UnityEngine;

public readonly struct MinionBrowserScreenConfig
{
	public MinionBrowserScreenConfig(MinionBrowserScreen.GridItem[] items, Option<MinionBrowserScreen.GridItem> defaultSelectedItem)
	{
		this.items = items;
		this.defaultSelectedItem = defaultSelectedItem;
		this.isValid = true;
	}

	public static MinionBrowserScreenConfig Personalities(Option<Personality> defaultSelectedPersonality = default(Option<Personality>))
	{
		MinionBrowserScreen.GridItem.PersonalityTarget[] items = (from personality in Db.Get().Personalities.GetAll(true, false)
		select MinionBrowserScreen.GridItem.Of(personality)).ToArray<MinionBrowserScreen.GridItem.PersonalityTarget>();
		Option<MinionBrowserScreen.GridItem> option = defaultSelectedPersonality.AndThen<MinionBrowserScreen.GridItem>((Personality personality) => items.FirstOrDefault((MinionBrowserScreen.GridItem.PersonalityTarget item) => item.personality == personality));
		if (option.IsNone() && items.Length != 0)
		{
			option = items[0];
		}
		MinionBrowserScreen.GridItem[] array = items;
		return new MinionBrowserScreenConfig(array, option);
	}

	public static MinionBrowserScreenConfig MinionInstances(Option<GameObject> defaultSelectedMinionInstance = default(Option<GameObject>))
	{
		MinionBrowserScreen.GridItem.MinionInstanceTarget[] items = (from minionIdentity in Components.MinionIdentities.Items
		select MinionBrowserScreen.GridItem.Of(minionIdentity.gameObject)).ToArray<MinionBrowserScreen.GridItem.MinionInstanceTarget>();
		Option<MinionBrowserScreen.GridItem> option = defaultSelectedMinionInstance.AndThen<MinionBrowserScreen.GridItem>((GameObject minionInstance) => items.FirstOrDefault((MinionBrowserScreen.GridItem.MinionInstanceTarget item) => item.minionInstance == minionInstance));
		if (option.IsNone() && items.Length != 0)
		{
			option = items[0];
		}
		MinionBrowserScreen.GridItem[] array = items;
		return new MinionBrowserScreenConfig(array, option);
	}

	public void ApplyAndOpenScreen(System.Action onClose = null)
	{
		LockerNavigator.Instance.duplicantCatalogueScreen.GetComponent<MinionBrowserScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.duplicantCatalogueScreen, onClose);
	}

	public readonly MinionBrowserScreen.GridItem[] items;

	public readonly Option<MinionBrowserScreen.GridItem> defaultSelectedItem;

	public readonly bool isValid;
}
