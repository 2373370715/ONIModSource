using System;
using STRINGS;
using UnityEngine;

public static class LoreBearerUtil
{
	public static void AddLoreTo(GameObject prefabOrGameObject)
	{
		prefabOrGameObject.AddOrGet<LoreBearer>();
	}

	public static void AddLoreTo(GameObject prefabOrGameObject, LoreBearerAction unlockLoreFn)
	{
		KPrefabID component = prefabOrGameObject.GetComponent<KPrefabID>();
		if (component.IsInitialized())
		{
			prefabOrGameObject.AddOrGet<LoreBearer>().Internal_SetContent(unlockLoreFn);
			return;
		}
		prefabOrGameObject.AddComponent<LoreBearer>();
		component.prefabInitFn += delegate(GameObject gameObject)
		{
			gameObject.GetComponent<LoreBearer>().Internal_SetContent(unlockLoreFn);
		};
	}

	public static void AddLoreTo(GameObject prefabOrGameObject, string[] collectionsToUnlockFrom)
	{
		KPrefabID component = prefabOrGameObject.GetComponent<KPrefabID>();
		if (component.IsInitialized())
		{
			prefabOrGameObject.AddOrGet<LoreBearer>().Internal_SetContent(UnlockNextInCollections(collectionsToUnlockFrom));
			return;
		}
		prefabOrGameObject.AddComponent<LoreBearer>();
		component.prefabInitFn += delegate(GameObject gameObject)
		{
			gameObject.GetComponent<LoreBearer>().Internal_SetContent(UnlockNextInCollections(collectionsToUnlockFrom));
		};
	}

	public static LoreBearerAction UnlockSpecificEntry(string unlockId, string searchDisplayText)
	{
		return delegate(InfoDialogScreen screen)
		{
			Game.Instance.unlocks.Unlock(unlockId);
			screen.AddPlainText(searchDisplayText);
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, OpenCodexByLockKeyID(unlockId));
		};
	}

	public static void UnlockNextEmail(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("emails");
		if (text != null)
		{
			string text2 = "SEARCH" + UnityEngine.Random.Range(1, 6);
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_SUCCESS." + text2));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, OpenCodexByLockKeyID(text));
		}
		else
		{
			string text3 = "SEARCH" + UnityEngine.Random.Range(1, 8);
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_FAIL." + text3));
		}
	}

	public static void UnlockNextResearchNote(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("researchnotes");
		if (text != null)
		{
			string text2 = "SEARCH" + UnityEngine.Random.Range(1, 3);
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_TECHNOLOGY_SUCCESS." + text2));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, OpenCodexByLockKeyID(text));
		}
		else
		{
			string text3 = "SEARCH1";
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + text3));
		}
	}

	public static void UnlockNextJournalEntry(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("journals");
		if (text != null)
		{
			string text2 = "SEARCH" + UnityEngine.Random.Range(1, 6);
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS." + text2));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, OpenCodexByLockKeyID(text));
		}
		else
		{
			string text3 = "SEARCH1";
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + text3));
		}
	}

	public static void UnlockNextDimensionalLore(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("dimensionallore", randomize: true);
		if (text != null)
		{
			string text2 = "SEARCH" + UnityEngine.Random.Range(1, 6);
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS." + text2));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, OpenCodexByLockKeyID(text));
		}
		else
		{
			string text3 = "SEARCH1";
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + text3));
		}
	}

	public static void UnlockNextSpaceEntry(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("space");
		if (text != null)
		{
			string text2 = "SEARCH" + UnityEngine.Random.Range(1, 7);
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_SPACEPOI_SUCCESS." + text2));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, OpenCodexByLockKeyID(text));
		}
		else
		{
			string text3 = "SEARCH" + UnityEngine.Random.Range(1, 4);
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_SPACEPOI_FAIL." + text3));
		}
	}

	public static void UnlockNextDeskPodiumEntry(InfoDialogScreen screen)
	{
		if (!Game.Instance.unlocks.IsUnlocked("story_trait_critter_manipulator_parking"))
		{
			Game.Instance.unlocks.Unlock("story_trait_critter_manipulator_parking");
			string text = "SEARCH" + UnityEngine.Random.Range(1, 1);
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_PODIUM." + text));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, OpenCodexByLockKeyID("story_trait_critter_manipulator_parking"));
		}
		else
		{
			string text2 = "SEARCH" + UnityEngine.Random.Range(1, 8);
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_FAIL." + text2));
		}
	}

	public static LoreBearerAction UnlockNextInCollections(string[] collectionsToUnlockFrom)
	{
		return delegate(InfoDialogScreen screen)
		{
			string[] array = collectionsToUnlockFrom;
			foreach (string collectionID in array)
			{
				string text = Game.Instance.unlocks.UnlockNext(collectionID);
				if (text != null)
				{
					screen.AddPlainText(UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS.SEARCH1);
					screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, OpenCodexByLockKeyID(text));
					return;
				}
			}
			string text2 = "SEARCH1";
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + text2));
		};
	}

	public static void NerualVacillator(InfoDialogScreen screen)
	{
		Game.Instance.unlocks.Unlock("neuralvacillator");
		UnlockNextResearchNote(screen);
	}

	public static Action<InfoDialogScreen> OpenCodexByLockKeyID(string key, bool focusContent = false)
	{
		return delegate(InfoDialogScreen dialog)
		{
			dialog.Deactivate();
			ManagementMenu.Instance.OpenCodexToLockId(key, focusContent);
		};
	}

	public static Action<InfoDialogScreen> OpenCodexByEntryID(string id)
	{
		return delegate(InfoDialogScreen dialog)
		{
			dialog.Deactivate();
			ManagementMenu.Instance.OpenCodexToEntry(id);
		};
	}
}
