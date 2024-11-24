using System;
using System.Collections.Generic;
using Klei;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSelector : KScreen
{
	public delegate void SelectMaterialActions();

	public static List<Tag> DeprioritizeAutoSelectElementList = new List<Tag>
	{
		SimHashes.WoodLog.ToString().ToTag(),
		SimHashes.SolidMercury.ToString().ToTag(),
		SimHashes.Lead.ToString().ToTag()
	};

	public Tag CurrentSelectedElement;

	public Dictionary<Tag, KToggle> ElementToggles = new Dictionary<Tag, KToggle>();

	public int selectorIndex;

	public SelectMaterialActions selectMaterialActions;

	public SelectMaterialActions deselectMaterialActions;

	private ToggleGroup toggleGroup;

	public GameObject TogglePrefab;

	public GameObject LayoutContainer;

	public KScrollRect ScrollRect;

	public GameObject Scrollbar;

	public GameObject Headerbar;

	public GameObject BadBG;

	public LocText NoMaterialDiscovered;

	public GameObject MaterialDescriptionPane;

	public LocText MaterialDescriptionText;

	public DescriptorPanel MaterialEffectsPane;

	public GameObject DescriptorsPanel;

	private KToggle selectedToggle;

	private Recipe.Ingredient activeIngredient;

	private Recipe activeRecipe;

	private float activeMass;

	private List<Tag> elementsToSort = new List<Tag>();

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		toggleGroup = GetComponent<ToggleGroup>();
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	public void ClearMaterialToggles()
	{
		CurrentSelectedElement = null;
		NoMaterialDiscovered.gameObject.SetActive(value: false);
		foreach (KeyValuePair<Tag, KToggle> elementToggle in ElementToggles)
		{
			elementToggle.Value.gameObject.SetActive(value: false);
			Util.KDestroyGameObject(elementToggle.Value.gameObject);
		}
		ElementToggles.Clear();
	}

	public static List<Tag> GetValidMaterials(Tag _materialTypeTag, bool omitDisabledElements = false)
	{
		string[] array = _materialTypeTag.ToString().Split('&');
		List<Tag> list = new List<Tag>();
		foreach (Tag tag in array)
		{
			foreach (Element element in ElementLoader.elements)
			{
				if (!(element.disabled && omitDisabledElements) && element.IsSolid && (element.tag == tag || element.HasTag(tag)))
				{
					list.Add(element.tag);
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
					if (component != null && !list.Contains(component.PrefabTag))
					{
						list.Add(component.PrefabTag);
					}
				}
			}
		}
		return list;
	}

	public void ConfigureScreen(Recipe.Ingredient ingredient, Recipe recipe)
	{
		activeIngredient = ingredient;
		activeRecipe = recipe;
		activeMass = ingredient.amount;
		List<Tag> validMaterials = GetValidMaterials(ingredient.tag);
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, KToggle> elementToggle in ElementToggles)
		{
			if (!validMaterials.Contains(elementToggle.Key))
			{
				list.Add(elementToggle.Key);
			}
		}
		foreach (Tag item in list)
		{
			ElementToggles[item].gameObject.SetActive(value: false);
			Util.KDestroyGameObject(ElementToggles[item].gameObject);
			ElementToggles.Remove(item);
		}
		foreach (Tag item2 in validMaterials)
		{
			if (!ElementToggles.ContainsKey(item2))
			{
				GameObject obj = Util.KInstantiate(TogglePrefab, LayoutContainer, "MaterialSelection_" + item2.ProperName());
				obj.transform.localScale = Vector3.one;
				obj.SetActive(value: true);
				KToggle component = obj.GetComponent<KToggle>();
				ElementToggles.Add(item2, component);
				component.group = toggleGroup;
				obj.gameObject.GetComponent<ToolTip>().toolTip = item2.ProperName();
			}
		}
		ConfigureMaterialTooltips();
		RefreshToggleContents();
	}

	private void SetToggleBGImage(KToggle toggle, Tag elem)
	{
		if (toggle == selectedToggle)
		{
			toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimUIMaterial;
			toggle.GetComponent<ImageToggleState>().SetActive();
			return;
		}
		if (ClusterManager.Instance.activeWorld.worldInventory.GetAmount(elem, includeRelatedWorlds: true) >= activeMass || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
		{
			toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimUIMaterial;
			toggle.GetComponentsInChildren<Image>()[1].color = Color.white;
			toggle.GetComponent<ImageToggleState>().SetInactive();
			return;
		}
		toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimMaterialUIDesaturated;
		toggle.GetComponentsInChildren<Image>()[1].color = new Color(1f, 1f, 1f, 0.6f);
		if (!AllowInsufficientMaterialBuild())
		{
			toggle.GetComponent<ImageToggleState>().SetDisabled();
		}
	}

	public void OnSelectMaterial(Tag elem, Recipe recipe, bool focusScrollRect = false)
	{
		KToggle kToggle = null;
		kToggle = ElementToggles[elem];
		if (kToggle != selectedToggle)
		{
			selectedToggle = kToggle;
			if (recipe != null)
			{
				SaveGame.Instance.materialSelectorSerializer.SetSelectedElement(ClusterManager.Instance.activeWorldId, selectorIndex, recipe.Result, elem);
			}
			CurrentSelectedElement = elem;
			if (selectMaterialActions != null)
			{
				selectMaterialActions();
			}
			UpdateHeader();
			SetDescription(elem);
			SetEffects(elem);
			if (MaterialDescriptionPane != null)
			{
				if (!MaterialDescriptionPane.gameObject.activeSelf && !MaterialEffectsPane.gameObject.activeSelf)
				{
					DescriptorsPanel.SetActive(value: false);
				}
				else
				{
					DescriptorsPanel.SetActive(value: true);
				}
			}
		}
		if (focusScrollRect && ElementToggles.Count > 1)
		{
			List<Tag> list = new List<Tag>();
			foreach (KeyValuePair<Tag, KToggle> elementToggle in ElementToggles)
			{
				list.Add(elementToggle.Key);
			}
			list.Sort(ElementSorter);
			int num = list.IndexOf(elem);
			int constraintCount = LayoutContainer.GetComponent<GridLayoutGroup>().constraintCount;
			float num2 = num / constraintCount / Math.Max((list.Count - 1) / constraintCount, 1);
			ScrollRect.normalizedPosition = new Vector2(0f, 1f - num2);
		}
		RefreshToggleContents();
	}

	public void RefreshToggleContents()
	{
		foreach (KeyValuePair<Tag, KToggle> elementToggle in ElementToggles)
		{
			KToggle value = elementToggle.Value;
			Tag elem = elementToggle.Key;
			GameObject gameObject = value.gameObject;
			bool flag = DiscoveredResources.Instance.IsDiscovered(elem) || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
			if (gameObject.activeSelf != flag)
			{
				gameObject.SetActive(flag);
			}
			if (!flag)
			{
				continue;
			}
			LocText[] componentsInChildren = gameObject.GetComponentsInChildren<LocText>();
			LocText locText = componentsInChildren[0];
			LocText obj = componentsInChildren[1];
			Image image = gameObject.GetComponentsInChildren<Image>()[1];
			obj.text = Util.FormatWholeNumber(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(elem, includeRelatedWorlds: true));
			locText.text = Util.FormatWholeNumber(activeMass);
			GameObject gameObject2 = Assets.TryGetPrefab(elementToggle.Key);
			if (gameObject2 != null)
			{
				KBatchedAnimController component = gameObject2.GetComponent<KBatchedAnimController>();
				image.sprite = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0]);
			}
			SetToggleBGImage(elementToggle.Value, elementToggle.Key);
			value.soundPlayer.AcceptClickCondition = () => IsEnoughMass(elem);
			value.ClearOnClick();
			if (IsEnoughMass(elem))
			{
				value.onClick += delegate
				{
					OnSelectMaterial(elem, activeRecipe);
				};
			}
		}
		SortElementToggles();
		UpdateHeader();
	}

	private bool IsEnoughMass(Tag t)
	{
		if (!(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(t, includeRelatedWorlds: true) >= activeMass) && !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive)
		{
			return AllowInsufficientMaterialBuild();
		}
		return true;
	}

	public bool AutoSelectAvailableMaterial()
	{
		if (activeRecipe == null || ElementToggles.Count == 0)
		{
			return false;
		}
		Tag previousElement = SaveGame.Instance.materialSelectorSerializer.GetPreviousElement(ClusterManager.Instance.activeWorldId, selectorIndex, activeRecipe.Result);
		if (previousElement != null)
		{
			ElementToggles.TryGetValue(previousElement, out var value);
			if (value != null && (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || ClusterManager.Instance.activeWorld.worldInventory.GetAmount(previousElement, includeRelatedWorlds: true) >= activeMass))
			{
				OnSelectMaterial(previousElement, activeRecipe, focusScrollRect: true);
				return true;
			}
		}
		float num = -1f;
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, KToggle> elementToggle in ElementToggles)
		{
			list.Add(elementToggle.Key);
		}
		list.Sort(ElementSorter);
		if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
		{
			OnSelectMaterial(list[0], activeRecipe, focusScrollRect: true);
			return true;
		}
		Tag tag = null;
		foreach (Tag item in list)
		{
			float num2 = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(item, includeRelatedWorlds: true);
			if (DeprioritizeAutoSelectElementList.Contains(item))
			{
				num2 = Mathf.Min(activeMass, num2);
			}
			if (num2 >= activeMass && num2 > num)
			{
				num = num2;
				tag = item;
			}
		}
		if (tag != null)
		{
			UISounds.PlaySound(UISounds.Sound.Object_AutoSelected);
			string arg = ((ElementLoader.GetElement(tag) == null) ? tag.ToString() : tag.Name);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(MISC.POPFX.RESOURCE_SELECTION_CHANGED, arg), null, Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
			OnSelectMaterial(tag, activeRecipe, focusScrollRect: true);
			return true;
		}
		return false;
	}

	private void SortElementToggles()
	{
		bool flag = false;
		int num = -1;
		elementsToSort.Clear();
		foreach (KeyValuePair<Tag, KToggle> elementToggle in ElementToggles)
		{
			if (elementToggle.Value.gameObject.activeSelf)
			{
				elementsToSort.Add(elementToggle.Key);
			}
		}
		elementsToSort.Sort(ElementSorter);
		for (int i = 0; i < elementsToSort.Count; i++)
		{
			int siblingIndex = ElementToggles[elementsToSort[i]].transform.GetSiblingIndex();
			if (siblingIndex <= num)
			{
				flag = true;
				break;
			}
			num = siblingIndex;
		}
		if (flag)
		{
			foreach (Tag item in elementsToSort)
			{
				ElementToggles[item].transform.SetAsLastSibling();
			}
		}
		UpdateScrollBar();
	}

	private void ConfigureMaterialTooltips()
	{
		foreach (KeyValuePair<Tag, KToggle> elementToggle in ElementToggles)
		{
			ToolTip component = elementToggle.Value.gameObject.GetComponent<ToolTip>();
			if (component != null)
			{
				component.toolTip = GameUtil.GetMaterialTooltips(elementToggle.Key);
			}
		}
	}

	private void UpdateScrollBar()
	{
		if (Scrollbar == null)
		{
			return;
		}
		int num = 0;
		foreach (KeyValuePair<Tag, KToggle> elementToggle in ElementToggles)
		{
			if (elementToggle.Value.gameObject.activeSelf)
			{
				num++;
			}
		}
		if (Scrollbar.activeSelf != num > 5)
		{
			Scrollbar.SetActive(num > 5);
		}
		ScrollRect.GetComponent<LayoutElement>().minHeight = 74 * ((num <= 5) ? 1 : 2);
	}

	private void UpdateHeader()
	{
		if (activeIngredient == null)
		{
			return;
		}
		int num = 0;
		foreach (KeyValuePair<Tag, KToggle> elementToggle in ElementToggles)
		{
			if (elementToggle.Value.gameObject.activeSelf)
			{
				num++;
			}
		}
		LocText componentInChildren = Headerbar.GetComponentInChildren<LocText>();
		string[] array = activeIngredient.tag.ToString().Split('&');
		string text = array[0].ToTag().ProperName();
		for (int i = 1; i < array.Length; i++)
		{
			text = text + " or " + array[i].ToTag().ProperName();
		}
		if (num == 0)
		{
			componentInChildren.text = string.Format(UI.PRODUCTINFO_MISSINGRESOURCES_TITLE, text, GameUtil.GetFormattedMass(activeIngredient.amount));
			string text2 = string.Format(UI.PRODUCTINFO_MISSINGRESOURCES_DESC, text);
			NoMaterialDiscovered.text = text2;
			NoMaterialDiscovered.gameObject.SetActive(value: true);
			NoMaterialDiscovered.color = Constants.NEGATIVE_COLOR;
			BadBG.SetActive(value: true);
			if (Scrollbar != null)
			{
				Scrollbar.SetActive(value: false);
			}
			LayoutContainer.SetActive(value: false);
		}
		else
		{
			componentInChildren.text = string.Format(UI.PRODUCTINFO_SELECTMATERIAL, text);
			NoMaterialDiscovered.gameObject.SetActive(value: false);
			BadBG.SetActive(value: false);
			LayoutContainer.SetActive(value: true);
			UpdateScrollBar();
		}
	}

	public void ToggleShowDescriptorsPanel(bool show)
	{
		if (!(DescriptorsPanel == null))
		{
			DescriptorsPanel.gameObject.SetActive(show);
		}
	}

	private void SetDescription(Tag element)
	{
		if (!(DescriptorsPanel == null))
		{
			StringEntry result = null;
			if (Strings.TryGet(new StringKey("STRINGS.ELEMENTS." + element.ToString().ToUpper() + ".BUILD_DESC"), out result))
			{
				MaterialDescriptionText.text = result.ToString();
				MaterialDescriptionPane.SetActive(value: true);
			}
			else
			{
				MaterialDescriptionPane.SetActive(value: false);
			}
		}
	}

	private void SetEffects(Tag element)
	{
		if (!(MaterialDescriptionPane == null))
		{
			List<Descriptor> materialDescriptors = GameUtil.GetMaterialDescriptors(element);
			if (materialDescriptors.Count > 0)
			{
				Descriptor item = default(Descriptor);
				item.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.EFFECTS_HEADER, ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.EFFECTS_HEADER);
				materialDescriptors.Insert(0, item);
				MaterialEffectsPane.gameObject.SetActive(value: true);
				MaterialEffectsPane.SetDescriptors(materialDescriptors);
			}
			else
			{
				MaterialEffectsPane.gameObject.SetActive(value: false);
			}
		}
	}

	public static bool AllowInsufficientMaterialBuild()
	{
		return GenericGameSettings.instance.allowInsufficientMaterialBuild;
	}

	private int ElementSorter(Tag at, Tag bt)
	{
		GameObject gameObject = Assets.TryGetPrefab(at);
		IHasSortOrder hasSortOrder = ((gameObject != null) ? gameObject.GetComponent<IHasSortOrder>() : null);
		GameObject gameObject2 = Assets.TryGetPrefab(bt);
		IHasSortOrder hasSortOrder2 = ((gameObject2 != null) ? gameObject2.GetComponent<IHasSortOrder>() : null);
		if (hasSortOrder == null || hasSortOrder2 == null)
		{
			return 0;
		}
		Element element = ElementLoader.GetElement(at);
		Element element2 = ElementLoader.GetElement(bt);
		if (element != null && element2 != null && element.buildMenuSort == element2.buildMenuSort)
		{
			return element.idx.CompareTo(element2.idx);
		}
		return hasSortOrder.sortOrder.CompareTo(hasSortOrder2.sortOrder);
	}
}
