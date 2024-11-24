using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialSelectionPanel : KScreen, IRender200ms
{
	public delegate bool GetBuildableStateDelegate(BuildingDef def);

	public delegate string GetBuildableTooltipDelegate(BuildingDef def);

	public delegate void SelectElement(Element element, float kgAvailable, float recipe_amount);

	public struct SelectedElemInfo
	{
		public Tag element;

		public float kgAvailable;
	}

	public Dictionary<KToggle, Tag> ElementToggles = new Dictionary<KToggle, Tag>();

	private List<MaterialSelector> materialSelectors = new List<MaterialSelector>();

	private List<Tag> currentSelectedElements = new List<Tag>();

	[SerializeField]
	protected PriorityScreen priorityScreenPrefab;

	[SerializeField]
	protected GameObject priorityScreenParent;

	[SerializeField]
	protected BuildToolRotateButtonUI buildToolRotateButton;

	private PriorityScreen priorityScreen;

	public GameObject MaterialSelectorTemplate;

	public GameObject ResearchRequired;

	private Recipe activeRecipe;

	private static Dictionary<Tag, List<Tag>> elementsWithTag = new Dictionary<Tag, List<Tag>>();

	private GetBuildableStateDelegate GetBuildableState;

	private GetBuildableTooltipDelegate GetBuildableTooltip;

	private List<int> gameSubscriptionHandles = new List<int>();

	public Tag CurrentSelectedElement
	{
		get
		{
			if (materialSelectors.Count == 0)
			{
				return null;
			}
			return materialSelectors[0].CurrentSelectedElement;
		}
	}

	public IList<Tag> GetSelectedElementAsList
	{
		get
		{
			currentSelectedElements.Clear();
			foreach (MaterialSelector materialSelector in materialSelectors)
			{
				if (materialSelector.gameObject.activeSelf)
				{
					Debug.Assert(materialSelector.CurrentSelectedElement != null);
					currentSelectedElements.Add(materialSelector.CurrentSelectedElement);
				}
			}
			return currentSelectedElements;
		}
	}

	public PriorityScreen PriorityScreen => priorityScreen;

	public static void ClearStatics()
	{
		elementsWithTag.Clear();
	}

	protected override void OnPrefabInit()
	{
		elementsWithTag.Clear();
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
		for (int i = 0; i < 3; i++)
		{
			MaterialSelector materialSelector = Util.KInstantiateUI<MaterialSelector>(MaterialSelectorTemplate, base.gameObject);
			materialSelector.selectorIndex = i;
			materialSelectors.Add(materialSelector);
		}
		materialSelectors[0].gameObject.SetActive(value: true);
		MaterialSelectorTemplate.SetActive(value: false);
		ToggleResearchRequired(state: false);
		if (priorityScreenParent != null)
		{
			priorityScreen = Util.KInstantiateUI<PriorityScreen>(priorityScreenPrefab.gameObject, priorityScreenParent);
			priorityScreen.InstantiateButtons(OnPriorityClicked);
			priorityScreenParent.transform.SetAsLastSibling();
		}
		gameSubscriptionHandles.Add(Game.Instance.Subscribe(-107300940, delegate
		{
			RefreshSelectors();
		}));
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		activateOnSpawn = true;
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		foreach (int gameSubscriptionHandle in gameSubscriptionHandles)
		{
			Game.Instance.Unsubscribe(gameSubscriptionHandle);
		}
		gameSubscriptionHandles.Clear();
	}

	public void AddSelectAction(MaterialSelector.SelectMaterialActions action)
	{
		materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.selectMaterialActions = (MaterialSelector.SelectMaterialActions)Delegate.Combine(selector.selectMaterialActions, action);
		});
	}

	public void ClearSelectActions()
	{
		materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.selectMaterialActions = null;
		});
	}

	public void ClearMaterialToggles()
	{
		materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.ClearMaterialToggles();
		});
	}

	public void ConfigureScreen(Recipe recipe, GetBuildableStateDelegate buildableStateCB, GetBuildableTooltipDelegate buildableTooltipCB)
	{
		activeRecipe = recipe;
		GetBuildableState = buildableStateCB;
		GetBuildableTooltip = buildableTooltipCB;
		RefreshSelectors();
	}

	public bool AllSelectorsSelected()
	{
		bool flag = false;
		foreach (MaterialSelector materialSelector in materialSelectors)
		{
			flag = flag || materialSelector.gameObject.activeInHierarchy;
			if (materialSelector.gameObject.activeInHierarchy && materialSelector.CurrentSelectedElement == null)
			{
				return false;
			}
		}
		return flag;
	}

	public void RefreshSelectors()
	{
		if (activeRecipe == null || !base.gameObject.activeInHierarchy)
		{
			return;
		}
		materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			selector.gameObject.SetActive(value: false);
		});
		BuildingDef buildingDef = activeRecipe.GetBuildingDef();
		bool num = GetBuildableState(buildingDef);
		string text = GetBuildableTooltip(buildingDef);
		if (!num)
		{
			ToggleResearchRequired(state: true);
			LocText[] componentsInChildren = ResearchRequired.GetComponentsInChildren<LocText>();
			componentsInChildren[0].text = "";
			componentsInChildren[1].text = text;
			componentsInChildren[1].color = Constants.NEGATIVE_COLOR;
			if (priorityScreen != null)
			{
				priorityScreen.gameObject.SetActive(value: false);
			}
			if (buildToolRotateButton != null)
			{
				buildToolRotateButton.gameObject.SetActive(value: false);
			}
			return;
		}
		ToggleResearchRequired(state: false);
		for (int i = 0; i < activeRecipe.Ingredients.Count; i++)
		{
			materialSelectors[i].gameObject.SetActive(value: true);
			materialSelectors[i].ConfigureScreen(activeRecipe.Ingredients[i], activeRecipe);
		}
		if (priorityScreen != null)
		{
			priorityScreen.gameObject.SetActive(value: true);
			priorityScreen.transform.SetAsLastSibling();
		}
		if (buildToolRotateButton != null)
		{
			buildToolRotateButton.gameObject.SetActive(value: true);
			buildToolRotateButton.transform.SetAsLastSibling();
		}
	}

	private void UpdateResourceToggleValues()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		materialSelectors.ForEach(delegate(MaterialSelector selector)
		{
			if (selector.gameObject.activeSelf)
			{
				selector.RefreshToggleContents();
			}
		});
	}

	private void ToggleResearchRequired(bool state)
	{
		if (!(ResearchRequired == null))
		{
			ResearchRequired.SetActive(state);
		}
	}

	public bool AutoSelectAvailableMaterial()
	{
		bool result = true;
		for (int i = 0; i < materialSelectors.Count; i++)
		{
			if (!materialSelectors[i].AutoSelectAvailableMaterial())
			{
				result = false;
			}
		}
		return result;
	}

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
			array = component2.SelectedElementsTags.ToArray();
		}
		if (array == null)
		{
			return;
		}
		for (int i = 0; i < Mathf.Min(array.Length, materialSelectors.Count); i++)
		{
			if (materialSelectors[i].ElementToggles.ContainsKey(array[i]))
			{
				materialSelectors[i].OnSelectMaterial(array[i], activeRecipe);
			}
		}
	}

	public void ForceSelectPrimaryTag(Tag tag)
	{
		materialSelectors[0].OnSelectMaterial(tag, activeRecipe);
	}

	public static SelectedElemInfo Filter(Tag _materialCategoryTag)
	{
		SelectedElemInfo result = default(SelectedElemInfo);
		result.element = null;
		result.kgAvailable = 0f;
		if (DiscoveredResources.Instance == null || ElementLoader.elements == null || ElementLoader.elements.Count == 0)
		{
			return result;
		}
		string[] array = _materialCategoryTag.ToString().Split('&');
		foreach (Tag tag in array)
		{
			List<Tag> value = null;
			if (!elementsWithTag.TryGetValue(tag, out value))
			{
				value = new List<Tag>();
				foreach (Element element in ElementLoader.elements)
				{
					if (element.tag == tag || element.HasTag(tag))
					{
						value.Add(element.tag);
					}
				}
				foreach (Tag materialBuildingElement in GameTags.MaterialBuildingElements)
				{
					if (!(materialBuildingElement == tag))
					{
						continue;
					}
					foreach (GameObject item in Assets.GetPrefabsWithTag(materialBuildingElement))
					{
						KPrefabID component = item.GetComponent<KPrefabID>();
						if (component != null && !value.Contains(component.PrefabTag))
						{
							value.Add(component.PrefabTag);
						}
					}
				}
				elementsWithTag[tag] = value;
			}
			foreach (Tag item2 in value)
			{
				float amount = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(item2, includeRelatedWorlds: true);
				if (amount > result.kgAvailable)
				{
					result.kgAvailable = amount;
					result.element = item2;
				}
			}
		}
		return result;
	}

	public void ToggleShowDescriptorPanels(bool show)
	{
		for (int i = 0; i < materialSelectors.Count; i++)
		{
			if (materialSelectors[i] != null)
			{
				materialSelectors[i].ToggleShowDescriptorsPanel(show);
			}
		}
	}

	private void OnPriorityClicked(PrioritySetting priority)
	{
		priorityScreen.SetScreenPriority(priority);
	}

	public void Render200ms(float dt)
	{
		UpdateResourceToggleValues();
	}
}
