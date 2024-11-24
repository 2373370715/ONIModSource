using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x02001386 RID: 4998
public class GeothermalPlantComponent : KMonoBehaviour, ICheckboxListGroupControl, IRelatedEntities
{
	// Token: 0x17000666 RID: 1638
	// (get) Token: 0x060066BE RID: 26302 RVA: 0x000E32C8 File Offset: 0x000E14C8
	string ICheckboxListGroupControl.Title
	{
		get
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.SIDESCREENS.BRING_ONLINE_TITLE;
		}
	}

	// Token: 0x17000667 RID: 1639
	// (get) Token: 0x060066BF RID: 26303 RVA: 0x000E32D4 File Offset: 0x000E14D4
	string ICheckboxListGroupControl.Description
	{
		get
		{
			return COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.SIDESCREENS.BRING_ONLINE_DESC;
		}
	}

	// Token: 0x060066C0 RID: 26304 RVA: 0x002D0474 File Offset: 0x002CE674
	public ICheckboxListGroupControl.ListGroup[] GetData()
	{
		ColonyAchievement activateGeothermalPlant = Db.Get().ColonyAchievements.ActivateGeothermalPlant;
		ICheckboxListGroupControl.CheckboxItem[] array = new ICheckboxListGroupControl.CheckboxItem[activateGeothermalPlant.requirementChecklist.Count];
		for (int i = 0; i < array.Length; i++)
		{
			ICheckboxListGroupControl.CheckboxItem checkboxItem = default(ICheckboxListGroupControl.CheckboxItem);
			bool flag = activateGeothermalPlant.requirementChecklist[i].Success();
			checkboxItem.isOn = flag;
			checkboxItem.text = (activateGeothermalPlant.requirementChecklist[i] as VictoryColonyAchievementRequirement).Name();
			checkboxItem.tooltip = activateGeothermalPlant.requirementChecklist[i].GetProgress(flag);
			array[i] = checkboxItem;
		}
		return new ICheckboxListGroupControl.ListGroup[]
		{
			new ICheckboxListGroupControl.ListGroup(activateGeothermalPlant.Name, array, null, null)
		};
	}

	// Token: 0x060066C1 RID: 26305 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x060066C2 RID: 26306 RVA: 0x000CECD9 File Offset: 0x000CCED9
	public int CheckboxSideScreenSortOrder()
	{
		return 100;
	}

	// Token: 0x060066C3 RID: 26307 RVA: 0x000E32E0 File Offset: 0x000E14E0
	public static bool GeothermalControllerRepaired()
	{
		return SaveGame.Instance.ColonyAchievementTracker.GeothermalControllerRepaired;
	}

	// Token: 0x060066C4 RID: 26308 RVA: 0x000E32F1 File Offset: 0x000E14F1
	public static bool GeothermalFacilityDiscovered()
	{
		return SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered;
	}

	// Token: 0x060066C5 RID: 26309 RVA: 0x000E3302 File Offset: 0x000E1502
	protected override void OnSpawn()
	{
		base.Subscribe(-1503271301, new Action<object>(this.OnObjectSelect));
	}

	// Token: 0x060066C6 RID: 26310 RVA: 0x000BFD4F File Offset: 0x000BDF4F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060066C7 RID: 26311 RVA: 0x002D0530 File Offset: 0x002CE730
	public static void DisplayPopup(string title, string desc, HashedString anim, System.Action onDismissCallback, Transform clickFocus = null)
	{
		EventInfoData eventInfoData = new EventInfoData(title, desc, anim);
		if (Components.LiveMinionIdentities.Count >= 2)
		{
			int num = UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Count);
			int num2 = UnityEngine.Random.Range(1, Components.LiveMinionIdentities.Count);
			eventInfoData.minions = new GameObject[]
			{
				Components.LiveMinionIdentities[num].gameObject,
				Components.LiveMinionIdentities[(num + num2) % Components.LiveMinionIdentities.Count].gameObject
			};
		}
		else if (Components.LiveMinionIdentities.Count == 1)
		{
			eventInfoData.minions = new GameObject[]
			{
				Components.LiveMinionIdentities[0].gameObject
			};
		}
		eventInfoData.AddDefaultOption(onDismissCallback);
		eventInfoData.clickFocus = clickFocus;
		EventInfoScreen.ShowPopup(eventInfoData);
	}

	// Token: 0x060066C8 RID: 26312 RVA: 0x002D05FC File Offset: 0x002CE7FC
	protected void RevealAllVentsAndController()
	{
		foreach (WorldGenSpawner.Spawnable spawnable in SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag(true, new Tag[]
		{
			"GeothermalVentEntity"
		}))
		{
			int baseX;
			int num;
			Grid.CellToXY(spawnable.cell, out baseX, out num);
			GridVisibility.Reveal(baseX, num + 2, 5, 5f);
		}
		foreach (WorldGenSpawner.Spawnable spawnable2 in SaveGame.Instance.worldGenSpawner.GetSpawnablesWithTag(true, new Tag[]
		{
			"GeothermalControllerEntity"
		}))
		{
			int baseX2;
			int num2;
			Grid.CellToXY(spawnable2.cell, out baseX2, out num2);
			GridVisibility.Reveal(baseX2, num2 + 3, 7, 7f);
		}
		SelectTool.Instance.Select(null, true);
	}

	// Token: 0x060066C9 RID: 26313 RVA: 0x002D070C File Offset: 0x002CE90C
	protected void OnObjectSelect(object clicked)
	{
		base.Unsubscribe(-1503271301, new Action<object>(this.OnObjectSelect));
		if (SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered)
		{
			return;
		}
		SaveGame.Instance.ColonyAchievementTracker.GeothermalFacilityDiscovered = true;
		GeothermalPlantComponent.DisplayPopup(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_DISCOVERED_TITLE, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOTHERMAL_DISOCVERED_DESC, "geothermalplantintro_kanim", new System.Action(this.RevealAllVentsAndController), null);
	}

	// Token: 0x060066CA RID: 26314 RVA: 0x002D0784 File Offset: 0x002CE984
	public static void OnVentingHotMaterial(int worldid)
	{
		using (List<GeothermalVent>.Enumerator enumerator = Components.GeothermalVents.GetItems(worldid).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GeothermalVent vent = enumerator.Current;
				if (vent.IsQuestEntombed())
				{
					vent.SetQuestComplete();
					if (!SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent)
					{
						GeothermalVictorySequence.VictoryVent = vent;
						GeothermalPlantComponent.DisplayPopup(COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOPLANT_ERRUPTED_TITLE, COLONY_ACHIEVEMENTS.ACTIVATEGEOTHERMALPLANT.POPUPS.GEOPLANT_ERRUPTED_DESC, "geothermalplantachievement_kanim", delegate
						{
							SaveGame.Instance.ColonyAchievementTracker.GeothermalClearedEntombedVent = true;
							if (!Db.Get().ColonyAchievements.ActivateGeothermalPlant.IsValidForSave())
							{
								GeothermalVictorySequence.Start(vent);
							}
						}, null);
						break;
					}
				}
			}
		}
	}

	// Token: 0x060066CB RID: 26315 RVA: 0x002D084C File Offset: 0x002CEA4C
	public List<KSelectable> GetRelatedEntities()
	{
		List<KSelectable> list = new List<KSelectable>();
		int myWorldId = this.GetMyWorldId();
		foreach (GeothermalController geothermalController in Components.GeothermalControllers.GetItems(myWorldId))
		{
			list.Add(geothermalController.GetComponent<KSelectable>());
		}
		foreach (GeothermalVent geothermalVent in Components.GeothermalVents.GetItems(myWorldId))
		{
			list.Add(geothermalVent.GetComponent<KSelectable>());
		}
		return list;
	}

	// Token: 0x04004D38 RID: 19768
	public const string POPUP_DISCOVERED_KANIM = "geothermalplantintro_kanim";

	// Token: 0x04004D39 RID: 19769
	public const string POPUP_PROGRESS_KANIM = "geothermalplantonline_kanim";

	// Token: 0x04004D3A RID: 19770
	public const string POPUP_COMPLETE_KANIM = "geothermalplantachievement_kanim";
}
