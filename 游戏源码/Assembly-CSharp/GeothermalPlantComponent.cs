using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;

public class GeothermalPlantComponent : KMonoBehaviour, ICheckboxListGroupControl, IRelatedEntities
{
	public const string POPUP_DISCOVERED_KANIM = "geothermalplantintro_kanim";

	public const string POPUP_PROGRESS_KANIM = "geothermalplantonline_kanim";

	public const string POPUP_COMPLETE_KANIM = "geothermalplantachievement_kanim";

	string ICheckboxListGroupControl.Title => COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.SIDESCREENS.BRING_ONLINE_TITLE;

	string ICheckboxListGroupControl.Description => COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.SIDESCREENS.BRING_ONLINE_DESC;

	public ICheckboxListGroupControl.ListGroup[] GetData()
	{
		ColonyAchievement activateGeothermalPlant = Db.Get().ColonyAchievements.ActivateGeothermalPlant;
		ICheckboxListGroupControl.CheckboxItem[] array = new ICheckboxListGroupControl.CheckboxItem[activateGeothermalPlant.requirementChecklist.Count];
		for (int i = 0; i < array.Length; i++)
		{
			ICheckboxListGroupControl.CheckboxItem checkboxItem = default(ICheckboxListGroupControl.CheckboxItem);
			bool complete = (checkboxItem.isOn = activateGeothermalPlant.requirementChecklist[i].Success());
			checkboxItem.text = (activateGeothermalPlant.requirementChecklist[i] as VictoryColonyAchievementRequirement).Name();
			checkboxItem.tooltip = activateGeothermalPlant.requirementChecklist[i].GetProgress(complete);
			array[i] = checkboxItem;
		}
		return new ICheckboxListGroupControl.ListGroup[1]
		{
			new ICheckboxListGroupControl.ListGroup(activateGeothermalPlant.Name, array)
		};
	}

	public bool SidescreenEnabled()
	{
		return true;
	}

	public int CheckboxSideScreenSortOrder()
	{
		return 100;
	}

	public static bool GeothermalControllerRepaired()
	{
		return SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired;
	}

	public static bool GeothermalFacilityDiscovered()
	{
		return SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered;
	}

	protected override void OnSpawn()
	{
		Subscribe(-1503271301, OnObjectSelect);
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	public static void DisplayPopup(string title, string desc, HashedString anim, System.Action onDismissCallback, Transform clickFocus = null)
	{
		EventInfoData eventInfoData = new EventInfoData(title, desc, anim);
		if (Components.LiveMinionIdentities.Count >= 2)
		{
			int num = UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Count);
			int num2 = UnityEngine.Random.Range(1, Components.LiveMinionIdentities.Count);
			eventInfoData.minions = new GameObject[2]
			{
				Components.LiveMinionIdentities[num].gameObject,
				Components.LiveMinionIdentities[(num + num2) % Components.LiveMinionIdentities.Count].gameObject
			};
		}
		else if (Components.LiveMinionIdentities.Count == 1)
		{
			eventInfoData.minions = new GameObject[1] { Components.LiveMinionIdentities[0].gameObject };
		}
		eventInfoData.AddDefaultOption(onDismissCallback);
		eventInfoData.clickFocus = clickFocus;
		EventInfoScreen.ShowPopup(eventInfoData);
	}

	protected void RevealAllVentsAndController()
	{
		foreach (WorldGenSpawner.Spawnable item in SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag(true, "GeothermalVentEntity"))
		{
			Grid.CellToXY(item.cell, out var x, out var y);
			GridVisibility.Reveal(x, y + 2, 5, 5f);
		}
		foreach (WorldGenSpawner.Spawnable item2 in SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag(true, "GeothermalControllerEntity"))
		{
			Grid.CellToXY(item2.cell, out var x2, out var y2);
			GridVisibility.Reveal(x2, y2 + 3, 7, 7f);
		}
		SelectTool.Instance.Select(null, skipSound: true);
	}

	protected void OnObjectSelect(object clicked)
	{
		Unsubscribe(-1503271301, OnObjectSelect);
		if (!SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered)
		{
			SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered = true;
			DisplayPopup(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_DISCOVERED_TITLE, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_DISOCVERED_DESC, "geothermalplantintro_kanim", RevealAllVentsAndController);
		}
	}

	public static void OnVentingHotMaterial(int worldid)
	{
		foreach (GeothermalVent item in Components.GeothermalVents.GetItems(worldid))
		{
			if (!item.IsQuestEntombed())
			{
				continue;
			}
			item.SetQuestComplete();
			if (!SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent)
			{
				GeothermalVictorySequence.VictoryVent = item;
				DisplayPopup(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOPLANT_ERRUPTED_TITLE, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOPLANT_ERRUPTED_DESC, "geothermalplantachievement_kanim", delegate
				{
					SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent = true;
				});
				break;
			}
		}
	}

	public List<KSelectable> GetRelatedEntities()
	{
		List<KSelectable> list = new List<KSelectable>();
		int myWorldId = this.GetMyWorldId();
		foreach (GeothermalController item in Components.GeothermalControllers.GetItems(myWorldId))
		{
			list.Add(item.GetComponent<KSelectable>());
		}
		foreach (GeothermalVent item2 in Components.GeothermalVents.GetItems(myWorldId))
		{
			list.Add(item2.GetComponent<KSelectable>());
		}
		return list;
	}
}
