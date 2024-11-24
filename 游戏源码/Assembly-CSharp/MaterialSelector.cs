using System;
using System.Collections.Generic;
using Klei;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DEA RID: 7658
public class MaterialSelector : KScreen
{
	// Token: 0x0600A01E RID: 40990 RVA: 0x001081A5 File Offset: 0x001063A5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.toggleGroup = base.GetComponent<ToggleGroup>();
	}

	// Token: 0x0600A01F RID: 40991 RVA: 0x001081B9 File Offset: 0x001063B9
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600A020 RID: 40992 RVA: 0x003D48B8 File Offset: 0x003D2AB8
	public void ClearMaterialToggles()
	{
		this.CurrentSelectedElement = null;
		this.NoMaterialDiscovered.gameObject.SetActive(false);
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			keyValuePair.Value.gameObject.SetActive(false);
			Util.KDestroyGameObject(keyValuePair.Value.gameObject);
		}
		this.ElementToggles.Clear();
	}

	// Token: 0x0600A021 RID: 40993 RVA: 0x003D4950 File Offset: 0x003D2B50
	public static List<Tag> GetValidMaterials(Tag _materialTypeTag, bool omitDisabledElements = false)
	{
		string[] array = _materialTypeTag.ToString().Split('&', StringSplitOptions.None);
		List<Tag> list = new List<Tag>();
		for (int i = 0; i < array.Length; i++)
		{
			Tag tag = array[i];
			foreach (Element element in ElementLoader.elements)
			{
				if ((!element.disabled || !omitDisabledElements) && element.IsSolid && (element.tag == tag || element.HasTag(tag)))
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
		}
		return list;
	}

	// Token: 0x0600A022 RID: 40994 RVA: 0x003D4AC0 File Offset: 0x003D2CC0
	public void ConfigureScreen(Recipe.Ingredient ingredient, Recipe recipe)
	{
		this.activeIngredient = ingredient;
		this.activeRecipe = recipe;
		this.activeMass = ingredient.amount;
		List<Tag> validMaterials = MaterialSelector.GetValidMaterials(ingredient.tag, false);
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			if (!validMaterials.Contains(keyValuePair.Key))
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (Tag key in list)
		{
			this.ElementToggles[key].gameObject.SetActive(false);
			Util.KDestroyGameObject(this.ElementToggles[key].gameObject);
			this.ElementToggles.Remove(key);
		}
		foreach (Tag tag in validMaterials)
		{
			if (!this.ElementToggles.ContainsKey(tag))
			{
				GameObject gameObject = Util.KInstantiate(this.TogglePrefab, this.LayoutContainer, "MaterialSelection_" + tag.ProperName());
				gameObject.transform.localScale = Vector3.one;
				gameObject.SetActive(true);
				KToggle component = gameObject.GetComponent<KToggle>();
				this.ElementToggles.Add(tag, component);
				component.group = this.toggleGroup;
				gameObject.gameObject.GetComponent<ToolTip>().toolTip = tag.ProperName();
			}
		}
		this.ConfigureMaterialTooltips();
		this.RefreshToggleContents();
	}

	// Token: 0x0600A023 RID: 40995 RVA: 0x003D4C98 File Offset: 0x003D2E98
	private void SetToggleBGImage(KToggle toggle, Tag elem)
	{
		if (toggle == this.selectedToggle)
		{
			toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimUIMaterial;
			toggle.GetComponent<ImageToggleState>().SetActive();
			return;
		}
		if (ClusterManager.Instance.activeWorld.worldInventory.GetAmount(elem, true) >= this.activeMass || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
		{
			toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimUIMaterial;
			toggle.GetComponentsInChildren<Image>()[1].color = Color.white;
			toggle.GetComponent<ImageToggleState>().SetInactive();
			return;
		}
		toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimMaterialUIDesaturated;
		toggle.GetComponentsInChildren<Image>()[1].color = new Color(1f, 1f, 1f, 0.6f);
		if (!MaterialSelector.AllowInsufficientMaterialBuild())
		{
			toggle.GetComponent<ImageToggleState>().SetDisabled();
		}
	}

	// Token: 0x0600A024 RID: 40996 RVA: 0x003D4D8C File Offset: 0x003D2F8C
	public void OnSelectMaterial(Tag elem, Recipe recipe, bool focusScrollRect = false)
	{
		KToggle x = this.ElementToggles[elem];
		if (x != this.selectedToggle)
		{
			this.selectedToggle = x;
			if (recipe != null)
			{
				SaveGame.Instance.materialSelectorSerializer.SetSelectedElement(ClusterManager.Instance.activeWorldId, this.selectorIndex, recipe.Result, elem);
			}
			this.CurrentSelectedElement = elem;
			if (this.selectMaterialActions != null)
			{
				this.selectMaterialActions();
			}
			this.UpdateHeader();
			this.SetDescription(elem);
			this.SetEffects(elem);
			if (this.MaterialDescriptionPane != null)
			{
				if (!this.MaterialDescriptionPane.gameObject.activeSelf && !this.MaterialEffectsPane.gameObject.activeSelf)
				{
					this.DescriptorsPanel.SetActive(false);
				}
				else
				{
					this.DescriptorsPanel.SetActive(true);
				}
			}
		}
		if (focusScrollRect && this.ElementToggles.Count > 1)
		{
			List<Tag> list = new List<Tag>();
			foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
			{
				list.Add(keyValuePair.Key);
			}
			list.Sort(new Comparison<Tag>(this.ElementSorter));
			float num = (float)list.IndexOf(elem);
			int constraintCount = this.LayoutContainer.GetComponent<GridLayoutGroup>().constraintCount;
			float num2 = num / (float)constraintCount / (float)Math.Max((list.Count - 1) / constraintCount, 1);
			this.ScrollRect.normalizedPosition = new Vector2(0f, 1f - num2);
		}
		this.RefreshToggleContents();
	}

	// Token: 0x0600A025 RID: 40997 RVA: 0x003D4F30 File Offset: 0x003D3130
	public void RefreshToggleContents()
	{
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			KToggle value = keyValuePair.Value;
			Tag elem = keyValuePair.Key;
			GameObject gameObject = value.gameObject;
			bool flag = DiscoveredResources.Instance.IsDiscovered(elem) || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
			if (gameObject.activeSelf != flag)
			{
				gameObject.SetActive(flag);
			}
			if (flag)
			{
				LocText[] componentsInChildren = gameObject.GetComponentsInChildren<LocText>();
				LocText locText = componentsInChildren[0];
				TMP_Text tmp_Text = componentsInChildren[1];
				Image image = gameObject.GetComponentsInChildren<Image>()[1];
				tmp_Text.text = Util.FormatWholeNumber(ClusterManager.Instance.activeWorld.worldInventory.GetAmount(elem, true));
				locText.text = Util.FormatWholeNumber(this.activeMass);
				GameObject gameObject2 = Assets.TryGetPrefab(keyValuePair.Key);
				if (gameObject2 != null)
				{
					KBatchedAnimController component = gameObject2.GetComponent<KBatchedAnimController>();
					image.sprite = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
				}
				this.SetToggleBGImage(keyValuePair.Value, keyValuePair.Key);
				value.soundPlayer.AcceptClickCondition = (() => this.IsEnoughMass(elem));
				value.ClearOnClick();
				if (this.IsEnoughMass(elem))
				{
					value.onClick += delegate()
					{
						this.OnSelectMaterial(elem, this.activeRecipe, false);
					};
				}
			}
		}
		this.SortElementToggles();
		this.UpdateHeader();
	}

	// Token: 0x0600A026 RID: 40998 RVA: 0x001081CB File Offset: 0x001063CB
	private bool IsEnoughMass(Tag t)
	{
		return ClusterManager.Instance.activeWorld.worldInventory.GetAmount(t, true) >= this.activeMass || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || MaterialSelector.AllowInsufficientMaterialBuild();
	}

	// Token: 0x0600A027 RID: 40999 RVA: 0x003D50EC File Offset: 0x003D32EC
	public bool AutoSelectAvailableMaterial()
	{
		if (this.activeRecipe == null || this.ElementToggles.Count == 0)
		{
			return false;
		}
		Tag previousElement = SaveGame.Instance.materialSelectorSerializer.GetPreviousElement(ClusterManager.Instance.activeWorldId, this.selectorIndex, this.activeRecipe.Result);
		if (previousElement != null)
		{
			KToggle x;
			this.ElementToggles.TryGetValue(previousElement, out x);
			if (x != null && (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || ClusterManager.Instance.activeWorld.worldInventory.GetAmount(previousElement, true) >= this.activeMass))
			{
				this.OnSelectMaterial(previousElement, this.activeRecipe, true);
				return true;
			}
		}
		float num = -1f;
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			list.Add(keyValuePair.Key);
		}
		list.Sort(new Comparison<Tag>(this.ElementSorter));
		if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
		{
			this.OnSelectMaterial(list[0], this.activeRecipe, true);
			return true;
		}
		Tag tag = null;
		foreach (Tag tag2 in list)
		{
			float num2 = ClusterManager.Instance.activeWorld.worldInventory.GetAmount(tag2, true);
			if (MaterialSelector.DeprioritizeAutoSelectElementList.Contains(tag2))
			{
				num2 = Mathf.Min(this.activeMass, num2);
			}
			if (num2 >= this.activeMass && num2 > num)
			{
				num = num2;
				tag = tag2;
			}
		}
		if (tag != null)
		{
			UISounds.PlaySound(UISounds.Sound.Object_AutoSelected);
			string arg = (ElementLoader.GetElement(tag) == null) ? tag.ToString() : tag.Name;
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(MISC.POPFX.RESOURCE_SELECTION_CHANGED, arg), null, Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()), 1.5f, false, false);
			this.OnSelectMaterial(tag, this.activeRecipe, true);
			return true;
		}
		return false;
	}

	// Token: 0x0600A028 RID: 41000 RVA: 0x003D5348 File Offset: 0x003D3548
	private void SortElementToggles()
	{
		bool flag = false;
		int num = -1;
		this.elementsToSort.Clear();
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			if (keyValuePair.Value.gameObject.activeSelf)
			{
				this.elementsToSort.Add(keyValuePair.Key);
			}
		}
		this.elementsToSort.Sort(new Comparison<Tag>(this.ElementSorter));
		for (int i = 0; i < this.elementsToSort.Count; i++)
		{
			int siblingIndex = this.ElementToggles[this.elementsToSort[i]].transform.GetSiblingIndex();
			if (siblingIndex <= num)
			{
				flag = true;
				break;
			}
			num = siblingIndex;
		}
		if (flag)
		{
			foreach (Tag key in this.elementsToSort)
			{
				this.ElementToggles[key].transform.SetAsLastSibling();
			}
		}
		this.UpdateScrollBar();
	}

	// Token: 0x0600A029 RID: 41001 RVA: 0x003D5488 File Offset: 0x003D3688
	private void ConfigureMaterialTooltips()
	{
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			ToolTip component = keyValuePair.Value.gameObject.GetComponent<ToolTip>();
			if (component != null)
			{
				component.toolTip = GameUtil.GetMaterialTooltips(keyValuePair.Key);
			}
		}
	}

	// Token: 0x0600A02A RID: 41002 RVA: 0x003D5504 File Offset: 0x003D3704
	private void UpdateScrollBar()
	{
		if (this.Scrollbar == null)
		{
			return;
		}
		int num = 0;
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			if (keyValuePair.Value.gameObject.activeSelf)
			{
				num++;
			}
		}
		if (this.Scrollbar.activeSelf != num > 5)
		{
			this.Scrollbar.SetActive(num > 5);
		}
		this.ScrollRect.GetComponent<LayoutElement>().minHeight = (float)(74 * ((num <= 5) ? 1 : 2));
	}

	// Token: 0x0600A02B RID: 41003 RVA: 0x003D55B4 File Offset: 0x003D37B4
	private void UpdateHeader()
	{
		if (this.activeIngredient == null)
		{
			return;
		}
		int num = 0;
		foreach (KeyValuePair<Tag, KToggle> keyValuePair in this.ElementToggles)
		{
			if (keyValuePair.Value.gameObject.activeSelf)
			{
				num++;
			}
		}
		LocText componentInChildren = this.Headerbar.GetComponentInChildren<LocText>();
		string[] array = this.activeIngredient.tag.ToString().Split('&', StringSplitOptions.None);
		string text = array[0].ToTag().ProperName();
		for (int i = 1; i < array.Length; i++)
		{
			text = text + " or " + array[i].ToTag().ProperName();
		}
		if (num == 0)
		{
			componentInChildren.text = string.Format(UI.PRODUCTINFO_MISSINGRESOURCES_TITLE, text, GameUtil.GetFormattedMass(this.activeIngredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			string text2 = string.Format(UI.PRODUCTINFO_MISSINGRESOURCES_DESC, text);
			this.NoMaterialDiscovered.text = text2;
			this.NoMaterialDiscovered.gameObject.SetActive(true);
			this.NoMaterialDiscovered.color = Constants.NEGATIVE_COLOR;
			this.BadBG.SetActive(true);
			if (this.Scrollbar != null)
			{
				this.Scrollbar.SetActive(false);
			}
			this.LayoutContainer.SetActive(false);
			return;
		}
		componentInChildren.text = string.Format(UI.PRODUCTINFO_SELECTMATERIAL, text);
		this.NoMaterialDiscovered.gameObject.SetActive(false);
		this.BadBG.SetActive(false);
		this.LayoutContainer.SetActive(true);
		this.UpdateScrollBar();
	}

	// Token: 0x0600A02C RID: 41004 RVA: 0x00108205 File Offset: 0x00106405
	public void ToggleShowDescriptorsPanel(bool show)
	{
		if (this.DescriptorsPanel == null)
		{
			return;
		}
		this.DescriptorsPanel.gameObject.SetActive(show);
	}

	// Token: 0x0600A02D RID: 41005 RVA: 0x003D5778 File Offset: 0x003D3978
	private void SetDescription(Tag element)
	{
		if (this.DescriptorsPanel == null)
		{
			return;
		}
		StringEntry stringEntry = null;
		if (Strings.TryGet(new StringKey("STRINGS.ELEMENTS." + element.ToString().ToUpper() + ".BUILD_DESC"), out stringEntry))
		{
			this.MaterialDescriptionText.text = stringEntry.ToString();
			this.MaterialDescriptionPane.SetActive(true);
			return;
		}
		this.MaterialDescriptionPane.SetActive(false);
	}

	// Token: 0x0600A02E RID: 41006 RVA: 0x003D57F0 File Offset: 0x003D39F0
	private void SetEffects(Tag element)
	{
		if (this.MaterialDescriptionPane == null)
		{
			return;
		}
		List<Descriptor> materialDescriptors = GameUtil.GetMaterialDescriptors(element);
		if (materialDescriptors.Count > 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(ELEMENTS.MATERIAL_MODIFIERS.EFFECTS_HEADER, ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.EFFECTS_HEADER, Descriptor.DescriptorType.Effect);
			materialDescriptors.Insert(0, item);
			this.MaterialEffectsPane.gameObject.SetActive(true);
			this.MaterialEffectsPane.SetDescriptors(materialDescriptors);
			return;
		}
		this.MaterialEffectsPane.gameObject.SetActive(false);
	}

	// Token: 0x0600A02F RID: 41007 RVA: 0x00108227 File Offset: 0x00106427
	public static bool AllowInsufficientMaterialBuild()
	{
		return GenericGameSettings.instance.allowInsufficientMaterialBuild;
	}

	// Token: 0x0600A030 RID: 41008 RVA: 0x003D5878 File Offset: 0x003D3A78
	private int ElementSorter(Tag at, Tag bt)
	{
		GameObject gameObject = Assets.TryGetPrefab(at);
		IHasSortOrder hasSortOrder = (gameObject != null) ? gameObject.GetComponent<IHasSortOrder>() : null;
		GameObject gameObject2 = Assets.TryGetPrefab(bt);
		IHasSortOrder hasSortOrder2 = (gameObject2 != null) ? gameObject2.GetComponent<IHasSortOrder>() : null;
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

	// Token: 0x04007D4F RID: 32079
	public static List<Tag> DeprioritizeAutoSelectElementList = new List<Tag>
	{
		SimHashes.WoodLog.ToString().ToTag(),
		SimHashes.SolidMercury.ToString().ToTag(),
		SimHashes.Lead.ToString().ToTag()
	};

	// Token: 0x04007D50 RID: 32080
	public Tag CurrentSelectedElement;

	// Token: 0x04007D51 RID: 32081
	public Dictionary<Tag, KToggle> ElementToggles = new Dictionary<Tag, KToggle>();

	// Token: 0x04007D52 RID: 32082
	public int selectorIndex;

	// Token: 0x04007D53 RID: 32083
	public MaterialSelector.SelectMaterialActions selectMaterialActions;

	// Token: 0x04007D54 RID: 32084
	public MaterialSelector.SelectMaterialActions deselectMaterialActions;

	// Token: 0x04007D55 RID: 32085
	private ToggleGroup toggleGroup;

	// Token: 0x04007D56 RID: 32086
	public GameObject TogglePrefab;

	// Token: 0x04007D57 RID: 32087
	public GameObject LayoutContainer;

	// Token: 0x04007D58 RID: 32088
	public KScrollRect ScrollRect;

	// Token: 0x04007D59 RID: 32089
	public GameObject Scrollbar;

	// Token: 0x04007D5A RID: 32090
	public GameObject Headerbar;

	// Token: 0x04007D5B RID: 32091
	public GameObject BadBG;

	// Token: 0x04007D5C RID: 32092
	public LocText NoMaterialDiscovered;

	// Token: 0x04007D5D RID: 32093
	public GameObject MaterialDescriptionPane;

	// Token: 0x04007D5E RID: 32094
	public LocText MaterialDescriptionText;

	// Token: 0x04007D5F RID: 32095
	public DescriptorPanel MaterialEffectsPane;

	// Token: 0x04007D60 RID: 32096
	public GameObject DescriptorsPanel;

	// Token: 0x04007D61 RID: 32097
	private KToggle selectedToggle;

	// Token: 0x04007D62 RID: 32098
	private Recipe.Ingredient activeIngredient;

	// Token: 0x04007D63 RID: 32099
	private Recipe activeRecipe;

	// Token: 0x04007D64 RID: 32100
	private float activeMass;

	// Token: 0x04007D65 RID: 32101
	private List<Tag> elementsToSort = new List<Tag>();

	// Token: 0x02001DEB RID: 7659
	// (Invoke) Token: 0x0600A034 RID: 41012
	public delegate void SelectMaterialActions();
}
