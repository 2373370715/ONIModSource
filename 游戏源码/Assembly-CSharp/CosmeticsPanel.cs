using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticsPanel : TargetPanel
{
	[SerializeField]
	private GameObject cosmeticSlotContainer;

	[SerializeField]
	private FacadeSelectionPanel selectionPanel;

	[SerializeField]
	private LocText nameLabel;

	[SerializeField]
	private LocText descriptionLabel;

	[SerializeField]
	private KButton editButton;

	[SerializeField]
	private UIMannequin mannequin;

	[SerializeField]
	private Image buildingIcon;

	[SerializeField]
	private Dictionary<ClothingOutfitUtility.OutfitType, GameObject> outfitCategories = new Dictionary<ClothingOutfitUtility.OutfitType, GameObject>();

	[SerializeField]
	private GameObject outfitCategoryButtonPrefab;

	[SerializeField]
	private GameObject outfitCategoryButtonContainer;

	private ClothingOutfitUtility.OutfitType selectedOutfitCategory;

	public override bool IsValidForTarget(GameObject target)
	{
		return true;
	}

	protected override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
		BuildingFacade buildingFacade = selectedTarget.GetComponent<BuildingFacade>();
		MinionIdentity component = selectedTarget.GetComponent<MinionIdentity>();
		selectionPanel.OnFacadeSelectionChanged = null;
		outfitCategoryButtonContainer.SetActive(component != null);
		if (component != null)
		{
			ClothingOutfitTarget outfitTarget = ClothingOutfitTarget.FromMinion(selectedOutfitCategory, component.gameObject);
			selectionPanel.SetOutfitTarget(outfitTarget, selectedOutfitCategory);
			FacadeSelectionPanel facadeSelectionPanel = selectionPanel;
			facadeSelectionPanel.OnFacadeSelectionChanged = (System.Action)Delegate.Combine(facadeSelectionPanel.OnFacadeSelectionChanged, (System.Action)delegate
			{
				if (selectionPanel.SelectedFacade == null || selectionPanel.SelectedFacade == "DEFAULT_FACADE")
				{
					outfitTarget.WriteItems(selectedOutfitCategory, new string[0]);
				}
				else
				{
					outfitTarget.WriteItems(selectedOutfitCategory, ClothingOutfitTarget.FromTemplateId(selectionPanel.SelectedFacade).ReadItems());
				}
				Refresh();
			});
		}
		else if (buildingFacade != null)
		{
			selectionPanel.SetBuildingDef(selectedTarget.GetComponent<Building>().Def.PrefabID, selectedTarget.GetComponent<BuildingFacade>().CurrentFacade);
			selectionPanel.OnFacadeSelectionChanged = null;
			FacadeSelectionPanel facadeSelectionPanel2 = selectionPanel;
			facadeSelectionPanel2.OnFacadeSelectionChanged = (System.Action)Delegate.Combine(facadeSelectionPanel2.OnFacadeSelectionChanged, (System.Action)delegate
			{
				if (selectionPanel.SelectedFacade == null || selectionPanel.SelectedFacade == "DEFAULT_FACADE" || Db.GetBuildingFacades().TryGet(selectionPanel.SelectedFacade).IsNullOrDestroyed())
				{
					buildingFacade.ApplyDefaultFacade(shouldTryAnimate: true);
				}
				else
				{
					buildingFacade.ApplyBuildingFacade(Db.GetBuildingFacades().Get(selectionPanel.SelectedFacade), shouldTryAnimate: true);
				}
				Refresh();
			});
		}
		Refresh();
	}

	public override void OnDeselectTarget(GameObject target)
	{
		base.OnDeselectTarget(target);
	}

	public void Refresh()
	{
		MinionIdentity component = selectedTarget.GetComponent<MinionIdentity>();
		BuildingFacade component2 = selectedTarget.GetComponent<BuildingFacade>();
		if (component != null)
		{
			ClothingOutfitTarget outfit = ClothingOutfitTarget.FromMinion(selectedOutfitCategory, selectedTarget);
			editButton.gameObject.SetActive(value: true);
			mannequin.gameObject.SetActive(value: true);
			mannequin.SetOutfit(outfit);
			buildingIcon.gameObject.SetActive(value: false);
			editButton.ClearOnClick();
			editButton.onClick += OnClickEditOutfit;
			nameLabel.SetText(outfit.ReadName());
			descriptionLabel.SetText("");
		}
		else if (component2 != null)
		{
			editButton.gameObject.SetActive(value: false);
			mannequin.gameObject.SetActive(value: false);
			buildingIcon.gameObject.SetActive(value: true);
			if (component2.CurrentFacade != null && component2.CurrentFacade != "DEFAULT_FACADE" && !Db.GetBuildingFacades().TryGet(component2.CurrentFacade).IsNullOrDestroyed())
			{
				BuildingFacadeResource buildingFacadeResource = Db.GetBuildingFacades().Get(component2.CurrentFacade);
				nameLabel.SetText(buildingFacadeResource.Name);
				descriptionLabel.SetText(buildingFacadeResource.Description);
				buildingIcon.sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(buildingFacadeResource.AnimFile));
			}
			else
			{
				string prefabID = component2.GetComponent<Building>().Def.PrefabID;
				Strings.TryGet("STRINGS.BUILDINGS.PREFABS." + prefabID.ToString().ToUpperInvariant() + ".FACADES.DEFAULT_" + prefabID.ToString().ToUpperInvariant() + ".NAME", out var result);
				if (result == null)
				{
					Strings.TryGet("STRINGS.BUILDINGS.PREFABS." + prefabID.ToString().ToUpperInvariant() + ".NAME", out result);
				}
				Strings.TryGet("STRINGS.BUILDINGS.PREFABS." + prefabID.ToString().ToUpperInvariant() + ".FACADES.DEFAULT_" + prefabID.ToString().ToUpperInvariant() + ".DESC", out var result2);
				if (result2 == null)
				{
					Strings.TryGet("STRINGS.BUILDINGS.PREFABS." + prefabID.ToString().ToUpperInvariant() + ".DESC", out result2);
				}
				nameLabel.SetText((result != null) ? ((string)result) : "");
				descriptionLabel.SetText((result2 != null) ? ((string)result2) : "");
				buildingIcon.sprite = Def.GetUISprite(prefabID).first;
			}
		}
		RefreshOutfitCategories();
		selectionPanel.Refresh();
	}

	public void OnClickEditOutfit()
	{
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot);
		MinionBrowserScreenConfig.MinionInstances(selectedTarget).ApplyAndOpenScreen(delegate
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot);
		});
	}

	private void RefreshOutfitCategories()
	{
		foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, GameObject> outfitCategory in outfitCategories)
		{
			Util.KDestroyGameObject(outfitCategory.Value);
		}
		outfitCategories.Clear();
		_ = new string[2] { "outfit", "atmosuit" };
		Dictionary<ClothingOutfitUtility.OutfitType, string> dictionary = new Dictionary<ClothingOutfitUtility.OutfitType, string>();
		dictionary.Add(ClothingOutfitUtility.OutfitType.Clothing, UI.UISIDESCREENS.BLUEPRINT_TAB.SUBCATEGORY_OUTFIT);
		dictionary.Add(ClothingOutfitUtility.OutfitType.AtmoSuit, UI.UISIDESCREENS.BLUEPRINT_TAB.SUBCATEGORY_ATMOSUIT);
		for (int i = 0; i < 3; i++)
		{
			if (i != 1)
			{
				int idx = i;
				GameObject gameObject = Util.KInstantiateUI(outfitCategoryButtonPrefab, outfitCategoryButtonContainer, force_active: true);
				outfitCategories.Add((ClothingOutfitUtility.OutfitType)idx, gameObject);
				gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(dictionary[(ClothingOutfitUtility.OutfitType)i]);
				MultiToggle component = gameObject.GetComponent<MultiToggle>();
				component.onClick = (System.Action)Delegate.Combine(component.onClick, (System.Action)delegate
				{
					selectedOutfitCategory = (ClothingOutfitUtility.OutfitType)idx;
					Refresh();
					selectionPanel.SelectedOutfitCategory = selectedOutfitCategory;
				});
				component.ChangeState((selectedOutfitCategory == (ClothingOutfitUtility.OutfitType)idx) ? 1 : 0);
			}
		}
	}
}
