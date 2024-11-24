using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02001DE3 RID: 7651
public class MaterialSelectionPanel : KScreen, IRender200ms
{
	// Token: 0x06009FF1 RID: 40945 RVA: 0x00107FB7 File Offset: 0x001061B7
	public static void ClearStatics()
	{
		MaterialSelectionPanel.elementsWithTag.Clear();
	}

	// Token: 0x17000A66 RID: 2662
	// (get) Token: 0x06009FF2 RID: 40946 RVA: 0x00107FC3 File Offset: 0x001061C3
	public Tag CurrentSelectedElement
	{
		get
		{
			if (this.materialSelectors.Count == 0)
			{
				return null;
			}
			return this.materialSelectors[0].CurrentSelectedElement;
		}
	}

	// Token: 0x17000A67 RID: 2663
	// (get) Token: 0x06009FF3 RID: 40947 RVA: 0x003D40F4 File Offset: 0x003D22F4
	public IList<Tag> GetSelectedElementAsList
	{
		get
		{
			this.currentSelectedElements.Clear();
			foreach (MaterialSelector materialSelector in this.materialSelectors)
			{
				if (materialSelector.gameObject.activeSelf)
				{
					global::Debug.Assert(materialSelector.CurrentSelectedElement != null);
					this.currentSelectedElements.Add(materialSelector.CurrentSelectedElement);
				}
			}
			return this.currentSelectedElements;
		}
	}

	// Token: 0x17000A68 RID: 2664
	// (get) Token: 0x06009FF4 RID: 40948 RVA: 0x00107FEA File Offset: 0x001061EA
	public PriorityScreen PriorityScreen
	{
		get
		{
			return this.priorityScreen;
		}
	}

	// Token: 0x06009FF5 RID: 40949 RVA: 0x003D4188 File Offset: 0x003D2388
	protected override void OnPrefabInit()
	{
		MaterialSelectionPanel.elementsWithTag.Clear();
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
		for (int i = 0; i < 3; i++)
		{
			MaterialSelector materialSelector = Util.KInstantiateUI<MaterialSelector>(this.MaterialSelectorTemplate, base.gameObject, false);
			materialSelector.selectorIndex = i;
			this.materialSelectors.Add(materialSelector);
		}
		this.materialSelectors[0].gameObject.SetActive(true);
		this.MaterialSelectorTemplate.SetActive(false);
		this.ToggleResearchRequired(false);
		if (this.priorityScreenParent != null)
		{
			this.priorityScreen = Util.KInstantiateUI<PriorityScreen>(this.priorityScreenPrefab.gameObject, this.priorityScreenParent, false);
			this.priorityScreen.InstantiateButtons(new Action<PrioritySetting>(this.OnPriorityClicked), true);
			this.priorityScreenParent.transform.SetAsLastSibling();
		}
		this.gameSubscriptionHandles.Add(Game.Instance.Subscribe(-107300940, delegate(object d)
		{
			this.RefreshSelectors();
		}));
	}

	// Token: 0x06009FF6 RID: 40950 RVA: 0x00107FF2 File Offset: 0x001061F2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.activateOnSpawn = true;
	}

	// Token: 0x06009FF7 RID: 40951 RVA: 0x003D4284 File Offset: 0x003D2484
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		foreach (int id in this.gameSubscriptionHandles)
		{
			Game.Instance.Unsubscribe(id);
		}
		this.gameSubscriptionHandles.Clear();
	}

	// Token: 0x06009FF8 RID: 40952 RVA: 0x003D42EC File Offset: 0x003D24EC
	public void AddSelectAction(MaterialSelector.SelectMaterialActions action)
	{
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.selectMaterialActions = (MaterialSelector.SelectMaterialActions)Delegate.Combine(selector.selectMaterialActions, action);
		});
	}

	// Token: 0x06009FF9 RID: 40953 RVA: 0x00108001 File Offset: 0x00106201
	public void ClearSelectActions()
	{
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.selectMaterialActions = null;
		});
	}

	// Token: 0x06009FFA RID: 40954 RVA: 0x0010802D File Offset: 0x0010622D
	public void ClearMaterialToggles()
	{
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.ClearMaterialToggles();
		});
	}

	// Token: 0x06009FFB RID: 40955 RVA: 0x00108059 File Offset: 0x00106259
	public void ConfigureScreen(Recipe recipe, MaterialSelectionPanel.GetBuildableStateDelegate buildableStateCB, MaterialSelectionPanel.GetBuildableTooltipDelegate buildableTooltipCB)
	{
		this.activeRecipe = recipe;
		this.GetBuildableState = buildableStateCB;
		this.GetBuildableTooltip = buildableTooltipCB;
		this.RefreshSelectors();
	}

	// Token: 0x06009FFC RID: 40956 RVA: 0x003D4320 File Offset: 0x003D2520
	public bool AllSelectorsSelected()
	{
		bool flag = false;
		foreach (MaterialSelector materialSelector in this.materialSelectors)
		{
			flag = (flag || materialSelector.gameObject.activeInHierarchy);
			if (materialSelector.gameObject.activeInHierarchy && materialSelector.CurrentSelectedElement == null)
			{
				return false;
			}
		}
		return flag;
	}

	// Token: 0x06009FFD RID: 40957 RVA: 0x003D43A8 File Offset: 0x003D25A8
	public void RefreshSelectors()
	{
		if (this.activeRecipe == null)
		{
			return;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.gameObject.SetActive(false);
		});
		BuildingDef buildingDef = this.activeRecipe.GetBuildingDef();
		bool flag = this.GetBuildableState(buildingDef);
		string text = this.GetBuildableTooltip(buildingDef);
		if (!flag)
		{
			this.ToggleResearchRequired(true);
			LocText[] componentsInChildren = this.ResearchRequired.GetComponentsInChildren<LocText>();
			componentsInChildren[0].text = "";
			componentsInChildren[1].text = text;
			componentsInChildren[1].color = Constants.NEGATIVE_COLOR;
			if (this.priorityScreen != null)
			{
				this.priorityScreen.gameObject.SetActive(false);
			}
			if (this.buildToolRotateButton != null)
			{
				this.buildToolRotateButton.gameObject.SetActive(false);
				return;
			}
		}
		else
		{
			this.ToggleResearchRequired(false);
			for (int i = 0; i < this.activeRecipe.Ingredients.Count; i++)
			{
				this.materialSelectors[i].gameObject.SetActive(true);
				this.materialSelectors[i].ConfigureScreen(this.activeRecipe.Ingredients[i], this.activeRecipe);
			}
			if (this.priorityScreen != null)
			{
				this.priorityScreen.gameObject.SetActive(true);
				this.priorityScreen.transform.SetAsLastSibling();
			}
			if (this.buildToolRotateButton != null)
			{
				this.buildToolRotateButton.gameObject.SetActive(true);
				this.buildToolRotateButton.transform.SetAsLastSibling();
			}
		}
	}

	// Token: 0x06009FFE RID: 40958 RVA: 0x00108076 File Offset: 0x00106276
	private void UpdateResourceToggleValues()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			if (selector.gameObject.activeSelf)
			{
				selector.RefreshToggleContents();
			}
		});
	}

	// Token: 0x06009FFF RID: 40959 RVA: 0x001080B0 File Offset: 0x001062B0
	private void ToggleResearchRequired(bool state)
	{
		if (this.ResearchRequired == null)
		{
			return;
		}
		this.ResearchRequired.SetActive(state);
	}

	// Token: 0x0600A000 RID: 40960 RVA: 0x003D4554 File Offset: 0x003D2754
	public bool AutoSelectAvailableMaterial()
	{
		bool result = true;
		for (int i = 0; i < this.materialSelectors.Count; i++)
		{
			if (!this.materialSelectors[i].AutoSelectAvailableMaterial())
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x0600A001 RID: 40961 RVA: 0x003D4590 File Offset: 0x003D2790
	public void SelectSourcesMaterials(Building building)
	{
		Tag[] array = null;
		Deconstructable component = building.gameObject.GetComponent<Deconstructable>();
		if (component != null)
		{
			array = component.constructionElements;
		}
		Constructable component2 = building.GetComponent<Constructable>();
		if (component2 != null)
		{
			array = component2.SelectedElementsTags.ToArray<Tag>();
		}
		if (array != null)
		{
			for (int i = 0; i < Mathf.Min(array.Length, this.materialSelectors.Count); i++)
			{
				if (this.materialSelectors[i].ElementToggles.ContainsKey(array[i]))
				{
					this.materialSelectors[i].OnSelectMaterial(array[i], this.activeRecipe, false);
				}
			}
		}
	}

	// Token: 0x0600A002 RID: 40962 RVA: 0x001080CD File Offset: 0x001062CD
	public void ForceSelectPrimaryTag(Tag tag)
	{
		this.materialSelectors[0].OnSelectMaterial(tag, this.activeRecipe, false);
	}

	// Token: 0x0600A003 RID: 40963 RVA: 0x003D4638 File Offset: 0x003D2838
	public static MaterialSelectionPanel.SelectedElemInfo Filter(Tag _materialCategoryTag)
	{
		MaterialSelectionPanel.SelectedElemInfo selectedElemInfo = default(MaterialSelectionPanel.SelectedElemInfo);
		selectedElemInfo.element = null;
		selectedElemInfo.kgAvailable = 0f;
		if (DiscoveredResources.Instance == null || ElementLoader.elements == null || ElementLoader.elements.Count == 0)
		{
			return selectedElemInfo;
		}
		string[] array = _materialCategoryTag.ToString().Split('&', StringSplitOptions.None);
		for (int i = 0; i < array.Length; i++)
		{
			Tag tag = array[i];
			List<Tag> list = null;
			if (!MaterialSelectionPanel.elementsWithTag.TryGetValue(tag, out list))
			{
				list = new List<Tag>();
				foreach (Element element in ElementLoader.elements)
				{
					if (element.tag == tag || element.HasTag(tag))
					{
						list.Add(element.tag);
					}
				}
				foreach (Tag tag2 in GameTags.MaterialBuildingElements)
				{
					if (tag2 == tag)
					{
						foreach (GameObject gameObject in Assets.GetPrefabsWithTag(tag2))
						{
							KPrefabID component = gameObject.GetComponent<KPrefabID>();
							if (component != null && !list.Contains(component.PrefabTag))
							{
								list.Add(component.PrefabTag);
							}
						}
					}
				}
				MaterialSelectionPanel.elementsWithTag[tag] = list;
			}
			foreach (Tag tag3 in list)
			{
				float amount = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag3, true);
				if (amount > selectedElemInfo.kgAvailable)
				{
					selectedElemInfo.kgAvailable = amount;
					selectedElemInfo.element = tag3;
				}
			}
		}
		return selectedElemInfo;
	}

	// Token: 0x0600A004 RID: 40964 RVA: 0x003D486C File Offset: 0x003D2A6C
	public void ToggleShowDescriptorPanels(bool show)
	{
		for (int i = 0; i < this.materialSelectors.Count; i++)
		{
			if (this.materialSelectors[i] != null)
			{
				this.materialSelectors[i].ToggleShowDescriptorsPanel(show);
			}
		}
	}

	// Token: 0x0600A005 RID: 40965 RVA: 0x001080E8 File Offset: 0x001062E8
	private void OnPriorityClicked(PrioritySetting priority)
	{
		this.priorityScreen.SetScreenPriority(priority, false);
	}

	// Token: 0x0600A006 RID: 40966 RVA: 0x001080F7 File Offset: 0x001062F7
	public void Render200ms(float dt)
	{
		this.UpdateResourceToggleValues();
	}

	// Token: 0x04007D39 RID: 32057
	public Dictionary<KToggle, Tag> ElementToggles = new Dictionary<KToggle, Tag>();

	// Token: 0x04007D3A RID: 32058
	private List<MaterialSelector> materialSelectors = new List<MaterialSelector>();

	// Token: 0x04007D3B RID: 32059
	private List<Tag> currentSelectedElements = new List<Tag>();

	// Token: 0x04007D3C RID: 32060
	[SerializeField]
	protected PriorityScreen priorityScreenPrefab;

	// Token: 0x04007D3D RID: 32061
	[SerializeField]
	protected GameObject priorityScreenParent;

	// Token: 0x04007D3E RID: 32062
	[SerializeField]
	protected BuildToolRotateButtonUI buildToolRotateButton;

	// Token: 0x04007D3F RID: 32063
	private PriorityScreen priorityScreen;

	// Token: 0x04007D40 RID: 32064
	public GameObject MaterialSelectorTemplate;

	// Token: 0x04007D41 RID: 32065
	public GameObject ResearchRequired;

	// Token: 0x04007D42 RID: 32066
	private Recipe activeRecipe;

	// Token: 0x04007D43 RID: 32067
	private static Dictionary<Tag, List<Tag>> elementsWithTag = new Dictionary<Tag, List<Tag>>();

	// Token: 0x04007D44 RID: 32068
	private MaterialSelectionPanel.GetBuildableStateDelegate GetBuildableState;

	// Token: 0x04007D45 RID: 32069
	private MaterialSelectionPanel.GetBuildableTooltipDelegate GetBuildableTooltip;

	// Token: 0x04007D46 RID: 32070
	private List<int> gameSubscriptionHandles = new List<int>();

	// Token: 0x02001DE4 RID: 7652
	// (Invoke) Token: 0x0600A00B RID: 40971
	public delegate bool GetBuildableStateDelegate(BuildingDef def);

	// Token: 0x02001DE5 RID: 7653
	// (Invoke) Token: 0x0600A00F RID: 40975
	public delegate string GetBuildableTooltipDelegate(BuildingDef def);

	// Token: 0x02001DE6 RID: 7654
	// (Invoke) Token: 0x0600A013 RID: 40979
	public delegate void SelectElement(Element element, float kgAvailable, float recipe_amount);

	// Token: 0x02001DE7 RID: 7655
	public struct SelectedElemInfo
	{
		// Token: 0x04007D47 RID: 32071
		public Tag element;

		// Token: 0x04007D48 RID: 32072
		public float kgAvailable;
	}
}
