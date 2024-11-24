using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CC9 RID: 7369
public class FacadeSelectionPanel : KMonoBehaviour
{
	// Token: 0x17000A2A RID: 2602
	// (get) Token: 0x060099E1 RID: 39393 RVA: 0x00104210 File Offset: 0x00102410
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

	// Token: 0x17000A2B RID: 2603
	// (get) Token: 0x060099E3 RID: 39395 RVA: 0x0010423C File Offset: 0x0010243C
	// (set) Token: 0x060099E2 RID: 39394 RVA: 0x0010422D File Offset: 0x0010242D
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

	// Token: 0x17000A2C RID: 2604
	// (get) Token: 0x060099E4 RID: 39396 RVA: 0x00104244 File Offset: 0x00102444
	public string SelectedBuildingDefID
	{
		get
		{
			return this.selectedBuildingDefID;
		}
	}

	// Token: 0x17000A2D RID: 2605
	// (get) Token: 0x060099E5 RID: 39397 RVA: 0x0010424C File Offset: 0x0010244C
	// (set) Token: 0x060099E6 RID: 39398 RVA: 0x003B714C File Offset: 0x003B534C
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

	// Token: 0x060099E7 RID: 39399 RVA: 0x00104254 File Offset: 0x00102454
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.gridLayout = this.toggleContainer.GetComponent<GridLayoutGroup>();
	}

	// Token: 0x060099E8 RID: 39400 RVA: 0x0010426D File Offset: 0x0010246D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.getMoreButton.ClearOnClick();
		this.getMoreButton.onClick += LockerMenuScreen.Instance.ShowInventoryScreen;
	}

	// Token: 0x060099E9 RID: 39401 RVA: 0x003B71A8 File Offset: 0x003B53A8
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

	// Token: 0x060099EA RID: 39402 RVA: 0x0010429B File Offset: 0x0010249B
	public void SetOutfitTarget(ClothingOutfitTarget outfitTarget, ClothingOutfitUtility.OutfitType outfitType)
	{
		this.currentConfigType = FacadeSelectionPanel.ConfigType.MinionOutfit;
		this.ClearToggles();
		this.SelectedFacade = outfitTarget.OutfitId;
		base.gameObject.SetActive(true);
	}

	// Token: 0x060099EB RID: 39403 RVA: 0x003B7208 File Offset: 0x003B5408
	private void ClearToggles()
	{
		foreach (KeyValuePair<string, FacadeSelectionPanel.FacadeToggle> keyValuePair in this.activeFacadeToggles)
		{
			this.pooledFacadeToggles.Add(keyValuePair.Value.gameObject);
			keyValuePair.Value.gameObject.SetActive(false);
		}
		this.activeFacadeToggles.Clear();
	}

	// Token: 0x060099EC RID: 39404 RVA: 0x003B7290 File Offset: 0x003B5490
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

	// Token: 0x060099ED RID: 39405 RVA: 0x003B73C0 File Offset: 0x003B55C0
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

	// Token: 0x060099EE RID: 39406 RVA: 0x003B75E4 File Offset: 0x003B57E4
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

	// Token: 0x060099EF RID: 39407 RVA: 0x001042C3 File Offset: 0x001024C3
	private void RefreshHeight()
	{
		if (this.usesScrollRect)
		{
			LayoutElement component = this.scrollRect.GetComponent<LayoutElement>();
			component.minHeight = (float)(58 * ((this.activeFacadeToggles.Count <= 5) ? 1 : 2));
			component.preferredHeight = component.minHeight;
		}
	}

	// Token: 0x060099F0 RID: 39408 RVA: 0x001042FF File Offset: 0x001024FF
	private void AddDefaultBuildingFacadeToggle()
	{
		this.AddNewBuildingToggle("DEFAULT_FACADE");
	}

	// Token: 0x060099F1 RID: 39409 RVA: 0x0010430C File Offset: 0x0010250C
	private void AddDefaultOutfitToggle()
	{
		this.AddNewOutfitToggle("DEFAULT_FACADE", true);
	}

	// Token: 0x060099F2 RID: 39410 RVA: 0x003B77EC File Offset: 0x003B59EC
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

	// Token: 0x060099F3 RID: 39411 RVA: 0x003B78B4 File Offset: 0x003B5AB4
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

	// Token: 0x060099F4 RID: 39412 RVA: 0x0010431A File Offset: 0x0010251A
	private void SelectFacade(string id)
	{
		this.SelectedFacade = id;
	}

	// Token: 0x0400781B RID: 30747
	[SerializeField]
	private GameObject togglePrefab;

	// Token: 0x0400781C RID: 30748
	[SerializeField]
	private RectTransform toggleContainer;

	// Token: 0x0400781D RID: 30749
	[SerializeField]
	private bool usesScrollRect;

	// Token: 0x0400781E RID: 30750
	[SerializeField]
	private LayoutElement scrollRect;

	// Token: 0x0400781F RID: 30751
	private Dictionary<string, FacadeSelectionPanel.FacadeToggle> activeFacadeToggles = new Dictionary<string, FacadeSelectionPanel.FacadeToggle>();

	// Token: 0x04007820 RID: 30752
	private List<GameObject> pooledFacadeToggles = new List<GameObject>();

	// Token: 0x04007821 RID: 30753
	[SerializeField]
	private KButton getMoreButton;

	// Token: 0x04007822 RID: 30754
	[SerializeField]
	private bool showGetMoreButton;

	// Token: 0x04007823 RID: 30755
	[SerializeField]
	private bool hideWhenEmpty = true;

	// Token: 0x04007824 RID: 30756
	[SerializeField]
	private bool useDummyPlaceholder;

	// Token: 0x04007825 RID: 30757
	private GridLayoutGroup gridLayout;

	// Token: 0x04007826 RID: 30758
	[SerializeField]
	private List<GameObject> dummyGridPlaceholders;

	// Token: 0x04007827 RID: 30759
	public System.Action OnFacadeSelectionChanged;

	// Token: 0x04007828 RID: 30760
	private ClothingOutfitUtility.OutfitType selectedOutfitCategory;

	// Token: 0x04007829 RID: 30761
	private string selectedBuildingDefID;

	// Token: 0x0400782A RID: 30762
	private FacadeSelectionPanel.ConfigType currentConfigType;

	// Token: 0x0400782B RID: 30763
	private string _selectedFacade;

	// Token: 0x0400782C RID: 30764
	public const string DEFAULT_FACADE_ID = "DEFAULT_FACADE";

	// Token: 0x02001CCA RID: 7370
	private struct FacadeToggle
	{
		// Token: 0x060099F6 RID: 39414 RVA: 0x003B79C0 File Offset: 0x003B5BC0
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

		// Token: 0x060099F7 RID: 39415 RVA: 0x003B7BEC File Offset: 0x003B5DEC
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

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x060099F8 RID: 39416 RVA: 0x00104348 File Offset: 0x00102548
		// (set) Token: 0x060099F9 RID: 39417 RVA: 0x00104350 File Offset: 0x00102550
		public string id { readonly get; set; }

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x060099FA RID: 39418 RVA: 0x00104359 File Offset: 0x00102559
		// (set) Token: 0x060099FB RID: 39419 RVA: 0x00104361 File Offset: 0x00102561
		public GameObject gameObject { readonly get; set; }

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x060099FC RID: 39420 RVA: 0x0010436A File Offset: 0x0010256A
		// (set) Token: 0x060099FD RID: 39421 RVA: 0x00104372 File Offset: 0x00102572
		public MultiToggle multiToggle { readonly get; set; }
	}

	// Token: 0x02001CCB RID: 7371
	private enum ConfigType
	{
		// Token: 0x04007831 RID: 30769
		BuildingFacade,
		// Token: 0x04007832 RID: 30770
		MinionOutfit
	}
}
