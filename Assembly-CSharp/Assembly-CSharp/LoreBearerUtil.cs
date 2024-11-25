﻿using System;
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
			prefabOrGameObject.AddOrGet<LoreBearer>().Internal_SetContent(LoreBearerUtil.UnlockNextInCollections(collectionsToUnlockFrom));
			return;
		}
		prefabOrGameObject.AddComponent<LoreBearer>();
		component.prefabInitFn += delegate(GameObject gameObject)
		{
			gameObject.GetComponent<LoreBearer>().Internal_SetContent(LoreBearerUtil.UnlockNextInCollections(collectionsToUnlockFrom));
		};
	}

		public static LoreBearerAction UnlockSpecificEntry(string unlockId, string searchDisplayText)
	{
		return delegate(InfoDialogScreen screen)
		{
			Game.Instance.unlocks.Unlock(unlockId, true);
			screen.AddPlainText(searchDisplayText);
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(unlockId, false), false);
		};
	}

		public static void UnlockNextEmail(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("emails", false);
		if (text != null)
		{
			string str = "SEARCH" + UnityEngine.Random.Range(1, 6).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_SUCCESS." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
			return;
		}
		string str2 = "SEARCH" + UnityEngine.Random.Range(1, 8).ToString();
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_FAIL." + str2));
	}

		public static void UnlockNextResearchNote(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("researchnotes", false);
		if (text != null)
		{
			string str = "SEARCH" + UnityEngine.Random.Range(1, 3).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_TECHNOLOGY_SUCCESS." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
			return;
		}
		string str2 = "SEARCH1";
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + str2));
	}

		public static void UnlockNextJournalEntry(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("journals", false);
		if (text != null)
		{
			string str = "SEARCH" + UnityEngine.Random.Range(1, 6).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
			return;
		}
		string str2 = "SEARCH1";
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + str2));
	}

		public static void UnlockNextDimensionalLore(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("dimensionallore", true);
		if (text != null)
		{
			string str = "SEARCH" + UnityEngine.Random.Range(1, 6).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
			return;
		}
		string str2 = "SEARCH1";
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + str2));
	}

		public static void UnlockNextSpaceEntry(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("space", false);
		if (text != null)
		{
			string str = "SEARCH" + UnityEngine.Random.Range(1, 7).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_SPACEPOI_SUCCESS." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
			return;
		}
		string str2 = "SEARCH" + UnityEngine.Random.Range(1, 4).ToString();
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_SPACEPOI_FAIL." + str2));
	}

		public static void UnlockNextDeskPodiumEntry(InfoDialogScreen screen)
	{
		if (!Game.Instance.unlocks.IsUnlocked("story_trait_critter_manipulator_parking"))
		{
			Game.Instance.unlocks.Unlock("story_trait_critter_manipulator_parking", true);
			string str = "SEARCH" + UnityEngine.Random.Range(1, 1).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_PODIUM." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID("story_trait_critter_manipulator_parking", false), false);
			return;
		}
		string str2 = "SEARCH" + UnityEngine.Random.Range(1, 8).ToString();
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_FAIL." + str2));
	}

		public static LoreBearerAction UnlockNextInCollections(string[] collectionsToUnlockFrom)
	{
		return delegate(InfoDialogScreen screen)
		{
			foreach (string collectionID in collectionsToUnlockFrom)
			{
				string text = Game.Instance.unlocks.UnlockNext(collectionID, false);
				if (text != null)
				{
					screen.AddPlainText(UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS.SEARCH1);
					screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
					return;
				}
			}
			string str = "SEARCH1";
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + str));
		};
	}

		public static void NerualVacillator(InfoDialogScreen screen)
	{
		Game.Instance.unlocks.Unlock("neuralvacillator", true);
		LoreBearerUtil.UnlockNextResearchNote(screen);
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
			ManagementMenu.Instance.OpenCodexToEntry(id, null);
		};
	}
}
