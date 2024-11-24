using System;
using System.Linq;
using UnityEngine;

public readonly struct MinionBrowserScreenConfig
{
	public readonly MinionBrowserScreen.GridItem[] items;

	public readonly Option<MinionBrowserScreen.GridItem> defaultSelectedItem;

	public readonly bool isValid;

	public MinionBrowserScreenConfig(MinionBrowserScreen.GridItem[] items, Option<MinionBrowserScreen.GridItem> defaultSelectedItem)
	{
		this.items = items;
		this.defaultSelectedItem = defaultSelectedItem;
		isValid = true;
	}

	public static MinionBrowserScreenConfig Personalities(Option<Personality> defaultSelectedPersonality = default(Option<Personality>))
	{
		MinionBrowserScreen.GridItem.PersonalityTarget[] items = (from personality in Db.Get().Personalities.GetAll(onlyEnabledMinions: true, onlyStartingMinions: false)
			select MinionBrowserScreen.GridItem.Of(personality)).ToArray();
		Option<MinionBrowserScreen.GridItem> option = defaultSelectedPersonality.AndThen((Func<Personality, MinionBrowserScreen.GridItem>)((Personality personality) => items.FirstOrDefault((MinionBrowserScreen.GridItem.PersonalityTarget item) => item.personality == personality)));
		if (option.IsNone() && items.Length != 0)
		{
			option = items[0];
		}
		MinionBrowserScreen.GridItem[] array = items;
		return new MinionBrowserScreenConfig(array, option);
	}

	public static MinionBrowserScreenConfig MinionInstances(Option<GameObject> defaultSelectedMinionInstance = default(Option<GameObject>))
	{
		MinionBrowserScreen.GridItem.MinionInstanceTarget[] items = Components.MinionIdentities.Items.Select((MinionIdentity minionIdentity) => MinionBrowserScreen.GridItem.Of(minionIdentity.gameObject)).ToArray();
		Option<MinionBrowserScreen.GridItem> option = defaultSelectedMinionInstance.AndThen((Func<GameObject, MinionBrowserScreen.GridItem>)((GameObject minionInstance) => items.FirstOrDefault((MinionBrowserScreen.GridItem.MinionInstanceTarget item) => item.minionInstance == minionInstance)));
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
}
