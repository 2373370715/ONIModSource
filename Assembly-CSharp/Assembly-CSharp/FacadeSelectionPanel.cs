﻿using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class FacadeSelectionPanel : KMonoBehaviour
{
			private int GridLayoutConstraintCount
	{
		get
		{
			if (this.gridLayout != null)
			{
				return this.gridLayout.constraintCount;
			}
			return 3;
		}
	}

				public ClothingOutfitUtility.OutfitType SelectedOutfitCategory
	{
		get
		{
			return this.selectedOutfitCategory;
		}
		set
		{
			this.selectedOutfitCategory = value;
			this.Refresh();
		}
	}

			public string SelectedBuildingDefID
	{
		get
		{
			return this.selectedBuildingDefID;
		}
	}

				public string SelectedFacade
	{
		get
		{
			return this._selectedFacade;
		}
		set
		{
			if (this._selectedFacade != value)
			{
				this._selectedFacade = value;
				FacadeSelectionPanel.ConfigType configType = this.currentConfigType;
				if (configType != FacadeSelectionPanel.ConfigType.BuildingFacade)
				{
					if (configType == FacadeSelectionPanel.ConfigType.MinionOutfit)
					{
						this.RefreshTogglesForOutfit(this.selectedOutfitCategory);
					}
				}
				else
				{
					this.RefreshTogglesForBuilding();
				}
				if (this.OnFacadeSelectionChanged != null)
				{
					this.OnFacadeSelectionChanged();
				}
			}
		}
	}

		protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.gridLayout = this.toggleContainer.GetComponent<GridLayoutGroup>();
	}

		protected override void OnSpawn()
	{
		base.OnSpawn();
		this.getMoreButton.ClearOnClick();
		this.getMoreButton.onClick += LockerMenuScreen.Instance.ShowInventoryScreen;
	}

		public void SetBuildingDef(string defID, string currentFacadeID = null)
	{
		this.currentConfigType = FacadeSelectionPanel.ConfigType.BuildingFacade;
		this.ClearToggles();
		this.selectedBuildingDefID = defID;
		this.SelectedFacade = ((currentFacadeID == null) ? "DEFAULT_FACADE" : currentFacadeID);
		this.RefreshTogglesForBuilding();
		if (this.hideWhenEmpty)
		{
			base.gameObject.SetActive(Assets.GetBuildingDef(defID).AvailableFacades.Count != 0);
		}
	}

		public void SetOutfitTarget(ClothingOutfitTarget outfitTarget, ClothingOutfitUtility.OutfitType outfitType)
	{
		this.currentConfigType = FacadeSelectionPanel.ConfigType.MinionOutfit;
		this.ClearToggles();
		this.SelectedFacade = outfitTarget.OutfitId;
		base.gameObject.SetActive(true);
	}

		private void ClearToggles()
	{
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair in this.activeFacadeToggles)
		{
			this.pooledFacadeToggles.Add(keyValuePair.Value.gameObject);
			keyValuePair.Value.gameObject.SetActive(false);
		}
		this.activeFacadeToggles.Clear();
	}

		public void Refresh()
	{
		FacadeSelectionPanel.ConfigType configType = this.currentConfigType;
		if (configType != FacadeSelectionPanel.ConfigType.BuildingFacade)
		{
			if (configType == FacadeSelectionPanel.ConfigType.MinionOutfit)
			{
				this.RefreshTogglesForOutfit(this.selectedOutfitCategory);
			}
		}
		else
		{
			this.RefreshTogglesForBuilding();
		}
		this.getMoreButton.gameObject.SetActive(this.showGetMoreButton);
		if (this.useDummyPlaceholder)
		{
			for (int i = 0; i < this.dummyGridPlaceholders.Count; i++)
			{
				this.dummyGridPlaceholders[i].SetActive(false);
			}
			int num = 0;
			for (int j = 0; j < this.toggleContainer.transform.childCount; j++)
			{
				if (this.toggleContainer.GetChild(j).gameObject.activeInHierarchy)
				{
					num++;
				}
			}
			this.getMoreButton.transform.SetAsLastSibling();
			if (num % this.GridLayoutConstraintCount != 0)
			{
				for (int k = 0; k < this.GridLayoutConstraintCount - 1; k++)
				{
					this.dummyGridPlaceholders[k].SetActive(k < this.GridLayoutConstraintCount - num % this.GridLayoutConstraintCount);
					this.dummyGridPlaceholders[k].transform.SetAsLastSibling();
				}
				return;
			}
		}
		else
		{
			this.getMoreButton.transform.SetAsLastSibling();
		}
	}

		private void RefreshTogglesForOutfit(ClothingOutfitUtility.OutfitType outfitType)
	{
		IEnumerable<ClothingOutfitTarget> enumerable = from outfit in ClothingOutfitTarget.GetAllTemplates()
		where outfit.OutfitType == outfitType
		select outfit;
		List<string> list = new List<string>();
		using (Dictionary<string, FacadeSelectionPanel.FacadeToggle>.Enumerator enumerator = this.activeFacadeToggles.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> toggle = enumerator.Current;
				if (!enumerable.Any((ClothingOutfitTarget match) => match.OutfitId == toggle.Key))
				{
					list.Add(toggle.Key);
				}
			}
		}
		foreach (string key in list)
		{
			this.pooledFacadeToggles.Add(this.activeFacadeToggles[key].gameObject);
			this.activeFacadeToggles[key].gameObject.SetActive(false);
			this.activeFacadeToggles.Remove(key);
		}
		list.Clear();
		this.AddDefaultOutfitToggle();
		enumerable = enumerable.StableSort((ClothingOutfitTarget a, ClothingOutfitTarget b) => a.OutfitId.CompareTo(b.OutfitId));
		foreach (ClothingOutfitTarget clothingOutfitTarget in enumerable)
		{
			if (!clothingOutfitTarget.DoesContainLockedItems())
			{
				this.AddNewOutfitToggle(clothingOutfitTarget.OutfitId, false);
			}
		}
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair in this.activeFacadeToggles)
		{
			keyValuePair.Value.multiToggle.ChangeState((this.SelectedFacade != null && this.SelectedFacade == keyValuePair.Key) ? 1 : 0);
		}
		this.RefreshHeight();
	}

		private void RefreshTogglesForBuilding()
	{
		BuildingDef buildingDef = Assets.GetBuildingDef(this.selectedBuildingDefID);
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair in this.activeFacadeToggles)
		{
			if (!buildingDef.AvailableFacades.Contains(keyValuePair.Key))
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (string key in list)
		{
			this.pooledFacadeToggles.Add(this.activeFacadeToggles[key].gameObject);
			this.activeFacadeToggles[key].gameObject.SetActive(false);
			this.activeFacadeToggles.Remove(key);
		}
		list.Clear();
		this.AddDefaultBuildingFacadeToggle();
		foreach (string text in buildingDef.AvailableFacades)
		{
			PermitResource permitResource = Db.Get().Permits.TryGet(text);
			if (permitResource != null && permitResource.IsUnlocked())
			{
				this.AddNewBuildingToggle(text);
			}
		}
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair2 in this.activeFacadeToggles)
		{
			keyValuePair2.Value.multiToggle.ChangeState((this.SelectedFacade == keyValuePair2.Key) ? 1 : 0);
		}
		this.activeFacadeToggles["DEFAULT_FACADE"].gameObject.transform.SetAsFirstSibling();
		this.RefreshHeight();
	}

		private void RefreshHeight()
	{
		if (this.usesScrollRect)
		{
			LayoutElement component = this.scrollRect.GetComponent<LayoutElement>();
			component.minHeight = (float)(58 * ((this.activeFacadeToggles.Count <= 5) ? 1 : 2));
			component.preferredHeight = component.minHeight;
		}
	}

		private void AddDefaultBuildingFacadeToggle()
	{
		this.AddNewBuildingToggle("DEFAULT_FACADE");
	}

		private void AddDefaultOutfitToggle()
	{
		this.AddNewOutfitToggle("DEFAULT_FACADE", true);
	}

		private void AddNewBuildingToggle(string facadeID)
	{
		if (this.activeFacadeToggles.ContainsKey(facadeID))
		{
			return;
		}
		GameObject gameObject;
		if (this.pooledFacadeToggles.Count > 0)
		{
			gameObject = this.pooledFacadeToggles[0];
			this.pooledFacadeToggles.RemoveAt(0);
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.togglePrefab, this.toggleContainer.gameObject, false);
		}
		FacadeSelectionPanel.FacadeToggle newToggle = new FacadeSelectionPanel.FacadeToggle(facadeID, this.selectedBuildingDefID, gameObject);
		MultiToggle multiToggle = newToggle.multiToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SelectFacade(newToggle.id);
		}));
		this.activeFacadeToggles.Add(newToggle.id, newToggle);
	}

		private void AddNewOutfitToggle(string outfitID, bool setAsFirstSibling = false)
	{
		if (this.activeFacadeToggles.ContainsKey(outfitID))
		{
			if (setAsFirstSibling)
			{
				this.activeFacadeToggles[outfitID].gameObject.transform.SetAsFirstSibling();
			}
			return;
		}
		GameObject gameObject;
		if (this.pooledFacadeToggles.Count > 0)
		{
			gameObject = this.pooledFacadeToggles[0];
			this.pooledFacadeToggles.RemoveAt(0);
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.togglePrefab, this.toggleContainer.gameObject, false);
		}
		FacadeSelectionPanel.FacadeToggle newToggle = new FacadeSelectionPanel.FacadeToggle(outfitID, gameObject, this.selectedOutfitCategory);
		MultiToggle multiToggle = newToggle.multiToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SelectFacade(newToggle.id);
		}));
		this.activeFacadeToggles.Add(newToggle.id, newToggle);
		if (setAsFirstSibling)
		{
			this.activeFacadeToggles[outfitID].gameObject.transform.SetAsFirstSibling();
		}
	}

		private void SelectFacade(string id)
	{
		this.SelectedFacade = id;
	}

		[SerializeField]
	private GameObject togglePrefab;

		[SerializeField]
	private RectTransform toggleContainer;

		[SerializeField]
	private bool usesScrollRect;

		[SerializeField]
	private LayoutElement scrollRect;

		private Dictionary<string, FacadeSelectionPanel.FacadeToggle> activeFacadeToggles = new Dictionary<string, FacadeSelectionPanel.FacadeToggle>();

		private List<GameObject> pooledFacadeToggles = new List<GameObject>();

		[SerializeField]
	private KButton getMoreButton;

		[SerializeField]
	private bool showGetMoreButton;

		[SerializeField]
	private bool hideWhenEmpty = true;

		[SerializeField]
	private bool useDummyPlaceholder;

		private GridLayoutGroup gridLayout;

		[SerializeField]
	private List<GameObject> dummyGridPlaceholders;

		public System.Action OnFacadeSelectionChanged;

		private ClothingOutfitUtility.OutfitType selectedOutfitCategory;

		private string selectedBuildingDefID;

		private FacadeSelectionPanel.ConfigType currentConfigType;

		private string _selectedFacade;

		public const string DEFAULT_FACADE_ID = "DEFAULT_FACADE";

		private struct FacadeToggle
	{
				public FacadeToggle(string buildingFacadeID, string buildingPrefabID, GameObject gameObject)
		{
			this.id = buildingFacadeID;
			this.gameObject = gameObject;
			gameObject.SetActive(true);
			this.multiToggle = gameObject.GetComponent<MultiToggle>();
			this.multiToggle.onClick = null;
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<UIMannequin>("Mannequin").gameObject.SetActive(false);
			component.GetReference<Image>("FGImage").SetAlpha(1f);
			Sprite sprite;
			string simpleTooltip;
			string dlcId;
			if (buildingFacadeID != "DEFAULT_FACADE")
			{
				BuildingFacadeResource buildingFacadeResource = Db.GetBuildingFacades().Get(buildingFacadeID);
				sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(buildingFacadeResource.AnimFile), "ui", false, "");
				simpleTooltip = KleiItemsUI.GetTooltipStringFor(buildingFacadeResource);
				dlcId = buildingFacadeResource.GetDlcIdFrom();
			}
			else
			{
				GameObject prefab = Assets.GetPrefab(buildingPrefabID);
				Building component2 = prefab.GetComponent<Building>();
				StringEntry entry;
				string text;
				if (Strings.TryGet(string.Concat(new string[]
				{
					"STRINGS.BUILDINGS.PREFABS.",
					buildingPrefabID.ToUpperInvariant(),
					".FACADES.DEFAULT_",
					buildingPrefabID.ToUpperInvariant(),
					".NAME"
				}), out entry))
				{
					text = entry;
				}
				else if (component2 != null)
				{
					text = component2.Def.Name;
				}
				else
				{
					text = prefab.GetProperName();
				}
				StringEntry entry2;
				string str;
				if (Strings.TryGet(string.Concat(new string[]
				{
					"STRINGS.BUILDINGS.PREFABS.",
					buildingPrefabID.ToUpperInvariant(),
					".FACADES.DEFAULT_",
					buildingPrefabID.ToUpperInvariant(),
					".DESC"
				}), out entry2))
				{
					str = entry2;
				}
				else if (component2 != null)
				{
					str = component2.Def.Desc;
				}
				else
				{
					str = "";
				}
				sprite = Def.GetUISprite(buildingPrefabID, "ui", false).first;
				simpleTooltip = KleiItemsUI.WrapAsToolTipTitle(text) + "\n" + str;
				dlcId = null;
			}
			component.GetReference<Image>("FGImage").sprite = sprite;
			this.gameObject.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
			Image reference = component.GetReference<Image>("DlcBanner");
			if (DlcManager.IsDlcId(dlcId))
			{
				reference.gameObject.SetActive(true);
				reference.color = DlcManager.GetDlcBannerColor(dlcId);
				return;
			}
			reference.gameObject.SetActive(false);
		}

				public FacadeToggle(string outfitID, GameObject gameObject, ClothingOutfitUtility.OutfitType outfitType)
		{
			this.id = outfitID;
			this.gameObject = gameObject;
			gameObject.SetActive(true);
			this.multiToggle = gameObject.GetComponent<MultiToggle>();
			this.multiToggle.onClick = null;
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			UIMannequin reference = component.GetReference<UIMannequin>("Mannequin");
			reference.gameObject.SetActive(true);
			component.GetReference<Image>("FGImage").SetAlpha(0f);
			ToolTip component2 = this.gameObject.GetComponent<ToolTip>();
			component2.SetSimpleTooltip("");
			if (outfitID != "DEFAULT_FACADE")
			{
				ClothingOutfitTarget outfit = ClothingOutfitTarget.FromTemplateId(outfitID);
				component.GetReference<UIMannequin>("Mannequin").SetOutfit(outfit);
				component2.SetSimpleTooltip(GameUtil.ApplyBoldString(outfit.ReadName()));
			}
			else
			{
				component.GetReference<UIMannequin>("Mannequin").ClearOutfit(outfitType);
				component2.SetSimpleTooltip(GameUtil.ApplyBoldString(UI.OUTFIT_NAME.NONE));
			}
			string dlcId = null;
			if (outfitID != "DEFAULT_FACADE")
			{
				ClothingOutfitTarget.Implementation impl = ClothingOutfitTarget.FromTemplateId(outfitID).impl;
				if (impl is ClothingOutfitTarget.DatabaseAuthoredTemplate)
				{
					ClothingOutfitTarget.DatabaseAuthoredTemplate databaseAuthoredTemplate = (ClothingOutfitTarget.DatabaseAuthoredTemplate)impl;
					dlcId = databaseAuthoredTemplate.resource.GetDlcIdFrom();
				}
			}
			Image reference2 = component.GetReference<Image>("DlcBanner");
			if (DlcManager.IsDlcId(dlcId))
			{
				reference2.gameObject.SetActive(true);
				reference2.color = DlcManager.GetDlcBannerColor(dlcId);
			}
			else
			{
				reference2.gameObject.SetActive(false);
			}
			Vector2 sizeDelta = new Vector2(0f, 0f);
			if (outfitType == ClothingOutfitUtility.OutfitType.AtmoSuit)
			{
				sizeDelta = new Vector2(-16f, -16f);
			}
			reference.rectTransform().sizeDelta = sizeDelta;
		}

								public string id { readonly get; set; }

								public GameObject gameObject { readonly get; set; }

								public MultiToggle multiToggle { readonly get; set; }
	}

		private enum ConfigType
	{
				BuildingFacade,
				MinionOutfit
	}
}
