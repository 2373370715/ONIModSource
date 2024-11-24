using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class FacadeSelectionPanel : KMonoBehaviour
{
	private struct FacadeToggle
	{
		public string id { get; set; }

		public GameObject gameObject { get; set; }

		public MultiToggle multiToggle { get; set; }

		public FacadeToggle(string buildingFacadeID, string buildingPrefabID, GameObject gameObject)
		{
			id = buildingFacadeID;
			this.gameObject = gameObject;
			gameObject.SetActive(value: true);
			multiToggle = gameObject.GetComponent<MultiToggle>();
			multiToggle.onClick = null;
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<UIMannequin>("Mannequin").gameObject.SetActive(value: false);
			component.GetReference<Image>("FGImage").SetAlpha(1f);
			Sprite sprite;
			string simpleTooltip;
			string dlcId;
			if (buildingFacadeID != "DEFAULT_FACADE")
			{
				BuildingFacadeResource buildingFacadeResource = Db.GetBuildingFacades().Get(buildingFacadeID);
				sprite = Def.GetUISpriteFromMultiObjectAnim(Assets.GetAnim(buildingFacadeResource.AnimFile));
				simpleTooltip = KleiItemsUI.GetTooltipStringFor(buildingFacadeResource);
				dlcId = buildingFacadeResource.GetDlcIdFrom();
			}
			else
			{
				GameObject prefab = Assets.GetPrefab(buildingPrefabID);
				Building component2 = prefab.GetComponent<Building>();
				StringEntry result;
				string text = (Strings.TryGet("STRINGS.BUILDINGS.PREFABS." + buildingPrefabID.ToUpperInvariant() + ".FACADES.DEFAULT_" + buildingPrefabID.ToUpperInvariant() + ".NAME", out result) ? ((string)result) : ((!(component2 != null)) ? prefab.GetProperName() : component2.Def.Name));
				StringEntry result2;
				string text2 = (Strings.TryGet("STRINGS.BUILDINGS.PREFABS." + buildingPrefabID.ToUpperInvariant() + ".FACADES.DEFAULT_" + buildingPrefabID.ToUpperInvariant() + ".DESC", out result2) ? ((string)result2) : ((!(component2 != null)) ? "" : component2.Def.Desc));
				sprite = Def.GetUISprite(buildingPrefabID).first;
				simpleTooltip = KleiItemsUI.WrapAsToolTipTitle(text) + "\n" + text2;
				dlcId = null;
			}
			component.GetReference<Image>("FGImage").sprite = sprite;
			this.gameObject.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
			Image reference = component.GetReference<Image>("DlcBanner");
			if (DlcManager.IsDlcId(dlcId))
			{
				reference.gameObject.SetActive(value: true);
				reference.color = DlcManager.GetDlcBannerColor(dlcId);
			}
			else
			{
				reference.gameObject.SetActive(value: false);
			}
		}

		public FacadeToggle(string outfitID, GameObject gameObject, ClothingOutfitUtility.OutfitType outfitType)
		{
			id = outfitID;
			this.gameObject = gameObject;
			gameObject.SetActive(value: true);
			multiToggle = gameObject.GetComponent<MultiToggle>();
			multiToggle.onClick = null;
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			UIMannequin reference = component.GetReference<UIMannequin>("Mannequin");
			reference.gameObject.SetActive(value: true);
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
			if (outfitID != "DEFAULT_FACADE" && ClothingOutfitTarget.FromTemplateId(outfitID).impl is ClothingOutfitTarget.DatabaseAuthoredTemplate databaseAuthoredTemplate)
			{
				dlcId = databaseAuthoredTemplate.resource.GetDlcIdFrom();
			}
			Image reference2 = component.GetReference<Image>("DlcBanner");
			if (DlcManager.IsDlcId(dlcId))
			{
				reference2.gameObject.SetActive(value: true);
				reference2.color = DlcManager.GetDlcBannerColor(dlcId);
			}
			else
			{
				reference2.gameObject.SetActive(value: false);
			}
			Vector2 sizeDelta = new Vector2(0f, 0f);
			if (outfitType == ClothingOutfitUtility.OutfitType.AtmoSuit)
			{
				sizeDelta = new Vector2(-16f, -16f);
			}
			reference.rectTransform().sizeDelta = sizeDelta;
		}
	}

	private enum ConfigType
	{
		BuildingFacade,
		MinionOutfit
	}

	[SerializeField]
	private GameObject togglePrefab;

	[SerializeField]
	private RectTransform toggleContainer;

	[SerializeField]
	private bool usesScrollRect;

	[SerializeField]
	private LayoutElement scrollRect;

	private Dictionary<string, FacadeToggle> activeFacadeToggles = new Dictionary<string, FacadeToggle>();

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

	private ConfigType currentConfigType;

	private string _selectedFacade;

	public const string DEFAULT_FACADE_ID = "DEFAULT_FACADE";

	private int GridLayoutConstraintCount
	{
		get
		{
			if (gridLayout != null)
			{
				return gridLayout.constraintCount;
			}
			return 3;
		}
	}

	public ClothingOutfitUtility.OutfitType SelectedOutfitCategory
	{
		get
		{
			return selectedOutfitCategory;
		}
		set
		{
			selectedOutfitCategory = value;
			Refresh();
		}
	}

	public string SelectedBuildingDefID => selectedBuildingDefID;

	public string SelectedFacade
	{
		get
		{
			return _selectedFacade;
		}
		set
		{
			if (_selectedFacade != value)
			{
				_selectedFacade = value;
				switch (currentConfigType)
				{
				case ConfigType.BuildingFacade:
					RefreshTogglesForBuilding();
					break;
				case ConfigType.MinionOutfit:
					RefreshTogglesForOutfit(selectedOutfitCategory);
					break;
				}
				if (OnFacadeSelectionChanged != null)
				{
					OnFacadeSelectionChanged();
				}
			}
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		gridLayout = toggleContainer.GetComponent<GridLayoutGroup>();
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		getMoreButton.ClearOnClick();
		getMoreButton.onClick += LockerMenuScreen.Instance.ShowInventoryScreen;
	}

	public void SetBuildingDef(string defID, string currentFacadeID = null)
	{
		currentConfigType = ConfigType.BuildingFacade;
		ClearToggles();
		selectedBuildingDefID = defID;
		SelectedFacade = ((currentFacadeID == null) ? "DEFAULT_FACADE" : currentFacadeID);
		RefreshTogglesForBuilding();
		if (hideWhenEmpty)
		{
			base.gameObject.SetActive(Assets.GetBuildingDef(defID).AvailableFacades.Count != 0);
		}
	}

	public void SetOutfitTarget(ClothingOutfitTarget outfitTarget, ClothingOutfitUtility.OutfitType outfitType)
	{
		currentConfigType = ConfigType.MinionOutfit;
		ClearToggles();
		SelectedFacade = outfitTarget.OutfitId;
		base.gameObject.SetActive(value: true);
	}

	private void ClearToggles()
	{
		foreach (KeyValuePair<string, FacadeToggle> activeFacadeToggle in activeFacadeToggles)
		{
			pooledFacadeToggles.Add(activeFacadeToggle.Value.gameObject);
			activeFacadeToggle.Value.gameObject.SetActive(value: false);
		}
		activeFacadeToggles.Clear();
	}

	public void Refresh()
	{
		switch (currentConfigType)
		{
		case ConfigType.MinionOutfit:
			RefreshTogglesForOutfit(selectedOutfitCategory);
			break;
		case ConfigType.BuildingFacade:
			RefreshTogglesForBuilding();
			break;
		}
		getMoreButton.gameObject.SetActive(showGetMoreButton);
		if (useDummyPlaceholder)
		{
			for (int i = 0; i < dummyGridPlaceholders.Count; i++)
			{
				dummyGridPlaceholders[i].SetActive(value: false);
			}
			int num = 0;
			for (int j = 0; j < toggleContainer.transform.childCount; j++)
			{
				if (toggleContainer.GetChild(j).gameObject.activeInHierarchy)
				{
					num++;
				}
			}
			getMoreButton.transform.SetAsLastSibling();
			if (num % GridLayoutConstraintCount != 0)
			{
				for (int k = 0; k < GridLayoutConstraintCount - 1; k++)
				{
					dummyGridPlaceholders[k].SetActive(k < GridLayoutConstraintCount - num % GridLayoutConstraintCount);
					dummyGridPlaceholders[k].transform.SetAsLastSibling();
				}
			}
		}
		else
		{
			getMoreButton.transform.SetAsLastSibling();
		}
	}

	private void RefreshTogglesForOutfit(ClothingOutfitUtility.OutfitType outfitType)
	{
		IEnumerable<ClothingOutfitTarget> enumerable = from outfit in ClothingOutfitTarget.GetAllTemplates()
			where outfit.OutfitType == outfitType
			select outfit;
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, FacadeToggle> toggle in activeFacadeToggles)
		{
			if (!enumerable.Any((ClothingOutfitTarget match) => match.OutfitId == toggle.Key))
			{
				list.Add(toggle.Key);
			}
		}
		foreach (string item in list)
		{
			pooledFacadeToggles.Add(activeFacadeToggles[item].gameObject);
			activeFacadeToggles[item].gameObject.SetActive(value: false);
			activeFacadeToggles.Remove(item);
		}
		list.Clear();
		AddDefaultOutfitToggle();
		enumerable = enumerable.StableSort((ClothingOutfitTarget a, ClothingOutfitTarget b) => a.OutfitId.CompareTo(b.OutfitId));
		foreach (ClothingOutfitTarget item2 in enumerable)
		{
			if (!item2.DoesContainLockedItems())
			{
				AddNewOutfitToggle(item2.OutfitId);
			}
		}
		foreach (KeyValuePair<string, FacadeToggle> activeFacadeToggle in activeFacadeToggles)
		{
			activeFacadeToggle.Value.multiToggle.ChangeState((SelectedFacade != null && SelectedFacade == activeFacadeToggle.Key) ? 1 : 0);
		}
		RefreshHeight();
	}

	private void RefreshTogglesForBuilding()
	{
		BuildingDef buildingDef = Assets.GetBuildingDef(selectedBuildingDefID);
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, FacadeToggle> activeFacadeToggle in activeFacadeToggles)
		{
			if (!buildingDef.AvailableFacades.Contains(activeFacadeToggle.Key))
			{
				list.Add(activeFacadeToggle.Key);
			}
		}
		foreach (string item in list)
		{
			pooledFacadeToggles.Add(activeFacadeToggles[item].gameObject);
			activeFacadeToggles[item].gameObject.SetActive(value: false);
			activeFacadeToggles.Remove(item);
		}
		list.Clear();
		AddDefaultBuildingFacadeToggle();
		foreach (string availableFacade in buildingDef.AvailableFacades)
		{
			PermitResource permitResource = Db.Get().Permits.TryGet(availableFacade);
			if (permitResource != null && permitResource.IsUnlocked())
			{
				AddNewBuildingToggle(availableFacade);
			}
		}
		foreach (KeyValuePair<string, FacadeToggle> activeFacadeToggle2 in activeFacadeToggles)
		{
			activeFacadeToggle2.Value.multiToggle.ChangeState((SelectedFacade == activeFacadeToggle2.Key) ? 1 : 0);
		}
		activeFacadeToggles["DEFAULT_FACADE"].gameObject.transform.SetAsFirstSibling();
		RefreshHeight();
	}

	private void RefreshHeight()
	{
		if (usesScrollRect)
		{
			LayoutElement component = scrollRect.GetComponent<LayoutElement>();
			component.minHeight = 58 * ((activeFacadeToggles.Count <= 5) ? 1 : 2);
			component.preferredHeight = component.minHeight;
		}
	}

	private void AddDefaultBuildingFacadeToggle()
	{
		AddNewBuildingToggle("DEFAULT_FACADE");
	}

	private void AddDefaultOutfitToggle()
	{
		AddNewOutfitToggle("DEFAULT_FACADE", setAsFirstSibling: true);
	}

	private void AddNewBuildingToggle(string facadeID)
	{
		if (!activeFacadeToggles.ContainsKey(facadeID))
		{
			GameObject gameObject = null;
			if (pooledFacadeToggles.Count > 0)
			{
				gameObject = pooledFacadeToggles[0];
				pooledFacadeToggles.RemoveAt(0);
			}
			else
			{
				gameObject = Util.KInstantiateUI(togglePrefab, toggleContainer.gameObject);
			}
			FacadeToggle newToggle = new FacadeToggle(facadeID, selectedBuildingDefID, gameObject);
			MultiToggle multiToggle = newToggle.multiToggle;
			multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, (System.Action)delegate
			{
				SelectFacade(newToggle.id);
			});
			activeFacadeToggles.Add(newToggle.id, newToggle);
		}
	}

	private void AddNewOutfitToggle(string outfitID, bool setAsFirstSibling = false)
	{
		if (activeFacadeToggles.ContainsKey(outfitID))
		{
			if (setAsFirstSibling)
			{
				activeFacadeToggles[outfitID].gameObject.transform.SetAsFirstSibling();
			}
			return;
		}
		GameObject gameObject = null;
		if (pooledFacadeToggles.Count > 0)
		{
			gameObject = pooledFacadeToggles[0];
			pooledFacadeToggles.RemoveAt(0);
		}
		else
		{
			gameObject = Util.KInstantiateUI(togglePrefab, toggleContainer.gameObject);
		}
		FacadeToggle newToggle = new FacadeToggle(outfitID, gameObject, selectedOutfitCategory);
		MultiToggle multiToggle = newToggle.multiToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, (System.Action)delegate
		{
			SelectFacade(newToggle.id);
		});
		activeFacadeToggles.Add(newToggle.id, newToggle);
		if (setAsFirstSibling)
		{
			activeFacadeToggles[outfitID].gameObject.transform.SetAsFirstSibling();
		}
	}

	private void SelectFacade(string id)
	{
		SelectedFacade = id;
	}
}
