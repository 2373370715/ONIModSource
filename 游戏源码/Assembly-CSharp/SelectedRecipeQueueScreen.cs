using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FC7 RID: 8135
public class SelectedRecipeQueueScreen : KScreen
{
	// Token: 0x0600AC29 RID: 44073 RVA: 0x0040D7B4 File Offset: 0x0040B9B4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.DecrementButton.onClick = delegate()
		{
			this.target.DecrementRecipeQueueCount(this.selectedRecipe, false);
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
		};
		this.IncrementButton.onClick = delegate()
		{
			this.target.IncrementRecipeQueueCount(this.selectedRecipe);
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
		};
		this.InfiniteButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_FOREVER;
		this.InfiniteButton.onClick += delegate()
		{
			if (this.target.GetRecipeQueueCount(this.selectedRecipe) != ComplexFabricator.QUEUE_INFINITE)
			{
				this.target.SetRecipeQueueCount(this.selectedRecipe, ComplexFabricator.QUEUE_INFINITE);
			}
			else
			{
				this.target.SetRecipeQueueCount(this.selectedRecipe, 0);
			}
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
		};
		this.QueueCount.onEndEdit += delegate()
		{
			base.isEditing = false;
			this.target.SetRecipeQueueCount(this.selectedRecipe, Mathf.RoundToInt(this.QueueCount.currentValue));
			this.RefreshQueueCountDisplay();
			this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
		};
		this.QueueCount.onStartEdit += delegate()
		{
			base.isEditing = true;
			KScreenManager.Instance.RefreshStack();
		};
		MultiToggle multiToggle = this.previousRecipeButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.CyclePreviousRecipe));
		MultiToggle multiToggle2 = this.nextRecipeButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(this.CycleNextRecipe));
	}

	// Token: 0x0600AC2A RID: 44074 RVA: 0x0040D8A4 File Offset: 0x0040BAA4
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.selectedRecipe != null)
		{
			GameObject prefab = Assets.GetPrefab(this.selectedRecipe.results[0].material);
			Equippable equippable = (prefab != null) ? prefab.GetComponent<Equippable>() : null;
			if (equippable != null && equippable.GetBuildOverride() != null)
			{
				this.minionWidget.RemoveEquipment(equippable);
			}
		}
	}

	// Token: 0x0600AC2B RID: 44075 RVA: 0x0040D910 File Offset: 0x0040BB10
	public void SetRecipe(ComplexFabricatorSideScreen owner, ComplexFabricator target, ComplexRecipe recipe)
	{
		this.ownerScreen = owner;
		this.target = target;
		this.selectedRecipe = recipe;
		this.recipeName.text = recipe.GetUIName(false);
		global::Tuple<Sprite, Color> tuple = (recipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.Ingredient) ? Def.GetUISprite(recipe.ingredients[0].material, "ui", false) : Def.GetUISprite(recipe.results[0].material, recipe.results[0].facadeID);
		if (recipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.HEP)
		{
			this.recipeIcon.sprite = owner.radboltSprite;
		}
		else
		{
			this.recipeIcon.sprite = tuple.first;
			this.recipeIcon.color = tuple.second;
		}
		string text = (recipe.time.ToString() + " " + UI.UNITSUFFIXES.SECONDS).ToLower();
		this.recipeMainDescription.SetText(recipe.description);
		this.recipeDuration.SetText(text);
		string simpleTooltip = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.TOOLTIPS.RECIPE_WORKTIME, text);
		this.recipeDurationTooltip.SetSimpleTooltip(simpleTooltip);
		this.RefreshIngredientDescriptors();
		this.RefreshResultDescriptors();
		this.RefreshQueueCountDisplay();
		this.ToggleAndRefreshMinionDisplay();
	}

	// Token: 0x0600AC2C RID: 44076 RVA: 0x0010FE07 File Offset: 0x0010E007
	private void CyclePreviousRecipe()
	{
		this.ownerScreen.CycleRecipe(-1);
	}

	// Token: 0x0600AC2D RID: 44077 RVA: 0x0010FE15 File Offset: 0x0010E015
	private void CycleNextRecipe()
	{
		this.ownerScreen.CycleRecipe(1);
	}

	// Token: 0x0600AC2E RID: 44078 RVA: 0x0010FE23 File Offset: 0x0010E023
	private void ToggleAndRefreshMinionDisplay()
	{
		this.minionWidget.gameObject.SetActive(this.RefreshMinionDisplayAnim());
	}

	// Token: 0x0600AC2F RID: 44079 RVA: 0x0040DA40 File Offset: 0x0040BC40
	private bool RefreshMinionDisplayAnim()
	{
		GameObject prefab = Assets.GetPrefab(this.selectedRecipe.results[0].material);
		if (prefab == null)
		{
			return false;
		}
		Equippable component = prefab.GetComponent<Equippable>();
		if (component == null)
		{
			return false;
		}
		KAnimFile buildOverride = component.GetBuildOverride();
		if (buildOverride == null)
		{
			return false;
		}
		this.minionWidget.SetDefaultPortraitAnimator();
		KAnimFile animFile = buildOverride;
		if (!this.selectedRecipe.results[0].facadeID.IsNullOrWhiteSpace())
		{
			EquippableFacadeResource equippableFacadeResource = Db.GetEquippableFacades().TryGet(this.selectedRecipe.results[0].facadeID);
			if (equippableFacadeResource != null)
			{
				animFile = Assets.GetAnim(equippableFacadeResource.BuildOverride);
			}
		}
		this.minionWidget.UpdateEquipment(component, animFile);
		return true;
	}

	// Token: 0x0600AC30 RID: 44080 RVA: 0x0040DAFC File Offset: 0x0040BCFC
	private void RefreshQueueCountDisplay()
	{
		this.ResearchRequiredContainer.SetActive(!this.selectedRecipe.IsRequiredTechUnlocked());
		bool flag = this.target.GetRecipeQueueCount(this.selectedRecipe) == ComplexFabricator.QUEUE_INFINITE;
		if (!flag)
		{
			this.QueueCount.SetAmount((float)this.target.GetRecipeQueueCount(this.selectedRecipe));
		}
		else
		{
			this.QueueCount.SetDisplayValue("");
		}
		this.InfiniteIcon.gameObject.SetActive(flag);
	}

	// Token: 0x0600AC31 RID: 44081 RVA: 0x0040DB80 File Offset: 0x0040BD80
	private void RefreshResultDescriptors()
	{
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> list = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		list.AddRange(this.GetResultDescriptions(this.selectedRecipe));
		foreach (Descriptor desc in this.target.AdditionalEffectsForRecipe(this.selectedRecipe))
		{
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(desc, null, false));
		}
		if (list.Count > 0)
		{
			this.EffectsDescriptorPanel.gameObject.SetActive(true);
			foreach (KeyValuePair<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> keyValuePair in this.recipeEffectsDescriptorRows)
			{
				Util.KDestroyGameObject(keyValuePair.Value);
			}
			this.recipeEffectsDescriptorRows.Clear();
			foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite in list)
			{
				GameObject gameObject = Util.KInstantiateUI(this.recipeElementDescriptorPrefab, this.EffectsDescriptorPanel.gameObject, true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("Label").SetText(descriptorWithSprite.descriptor.IndentedText());
				component.GetReference<Image>("Icon").sprite = ((descriptorWithSprite.tintedSprite == null) ? null : descriptorWithSprite.tintedSprite.first);
				component.GetReference<Image>("Icon").color = ((descriptorWithSprite.tintedSprite == null) ? Color.white : descriptorWithSprite.tintedSprite.second);
				component.GetReference<RectTransform>("FilterControls").gameObject.SetActive(false);
				component.GetReference<ToolTip>("Tooltip").SetSimpleTooltip(descriptorWithSprite.descriptor.tooltipText);
				this.recipeEffectsDescriptorRows.Add(descriptorWithSprite, gameObject);
			}
		}
	}

	// Token: 0x0600AC32 RID: 44082 RVA: 0x0040DD80 File Offset: 0x0040BF80
	private List<SelectedRecipeQueueScreen.DescriptorWithSprite> GetResultDescriptions(ComplexRecipe recipe)
	{
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> list = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		if (recipe.producedHEP > 0)
		{
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(string.Format("<b>{0}</b>: {1}", UI.FormatAsLink(ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, "HEP"), recipe.producedHEP), string.Format("<b>{0}</b>: {1}", ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, recipe.producedHEP), Descriptor.DescriptorType.Requirement, false), new global::Tuple<Sprite, Color>(Assets.GetSprite("radbolt"), Color.white), false));
		}
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.results)
		{
			GameObject prefab = Assets.GetPrefab(recipeElement.material);
			string formattedByTag = GameUtil.GetFormattedByTag(recipeElement.material, recipeElement.amount, GameUtil.TimeSlice.None);
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPEPRODUCT, recipeElement.facadeID.IsNullOrWhiteSpace() ? recipeElement.material.ProperName() : recipeElement.facadeID.ProperName(), formattedByTag), string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.TOOLTIPS.RECIPEPRODUCT, recipeElement.facadeID.IsNullOrWhiteSpace() ? recipeElement.material.ProperName() : recipeElement.facadeID.ProperName(), formattedByTag), Descriptor.DescriptorType.Requirement, false), Def.GetUISprite(recipeElement.material, recipeElement.facadeID), false));
			Element element = ElementLoader.GetElement(recipeElement.material);
			if (element != null)
			{
				List<SelectedRecipeQueueScreen.DescriptorWithSprite> list2 = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
				foreach (Descriptor desc in GameUtil.GetMaterialDescriptors(element))
				{
					list2.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(desc, null, false));
				}
				foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite in list2)
				{
					descriptorWithSprite.descriptor.IncreaseIndent();
				}
				list.AddRange(list2);
			}
			else
			{
				List<SelectedRecipeQueueScreen.DescriptorWithSprite> list3 = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
				foreach (Descriptor desc2 in GameUtil.GetEffectDescriptors(GameUtil.GetAllDescriptors(prefab, false)))
				{
					list3.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(desc2, null, false));
				}
				foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite2 in list3)
				{
					descriptorWithSprite2.descriptor.IncreaseIndent();
				}
				list.AddRange(list3);
			}
		}
		return list;
	}

	// Token: 0x0600AC33 RID: 44083 RVA: 0x0040E050 File Offset: 0x0040C250
	private void RefreshIngredientDescriptors()
	{
		new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> ingredientDescriptions = this.GetIngredientDescriptions(this.selectedRecipe);
		this.IngredientsDescriptorPanel.gameObject.SetActive(true);
		foreach (KeyValuePair<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> keyValuePair in this.recipeIngredientDescriptorRows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.recipeIngredientDescriptorRows.Clear();
		foreach (SelectedRecipeQueueScreen.DescriptorWithSprite descriptorWithSprite in ingredientDescriptions)
		{
			GameObject gameObject = Util.KInstantiateUI(this.recipeElementDescriptorPrefab, this.IngredientsDescriptorPanel.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(descriptorWithSprite.descriptor.IndentedText());
			component.GetReference<Image>("Icon").sprite = ((descriptorWithSprite.tintedSprite == null) ? null : descriptorWithSprite.tintedSprite.first);
			component.GetReference<Image>("Icon").color = ((descriptorWithSprite.tintedSprite == null) ? Color.white : descriptorWithSprite.tintedSprite.second);
			component.GetReference<RectTransform>("FilterControls").gameObject.SetActive(false);
			component.GetReference<ToolTip>("Tooltip").SetSimpleTooltip(descriptorWithSprite.descriptor.tooltipText);
			this.recipeIngredientDescriptorRows.Add(descriptorWithSprite, gameObject);
		}
	}

	// Token: 0x0600AC34 RID: 44084 RVA: 0x0040E1E8 File Offset: 0x0040C3E8
	private List<SelectedRecipeQueueScreen.DescriptorWithSprite> GetIngredientDescriptions(ComplexRecipe recipe)
	{
		List<SelectedRecipeQueueScreen.DescriptorWithSprite> list = new List<SelectedRecipeQueueScreen.DescriptorWithSprite>();
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			GameObject prefab = Assets.GetPrefab(recipeElement.material);
			string formattedByTag = GameUtil.GetFormattedByTag(recipeElement.material, recipeElement.amount, GameUtil.TimeSlice.None);
			float amount = this.target.GetMyWorld().worldInventory.GetAmount(recipeElement.material, true);
			string formattedByTag2 = GameUtil.GetFormattedByTag(recipeElement.material, amount, GameUtil.TimeSlice.None);
			string text = (amount >= recipeElement.amount) ? string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPERQUIREMENT, prefab.GetProperName(), formattedByTag, formattedByTag2) : ("<color=#F44A47>" + string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPERQUIREMENT, prefab.GetProperName(), formattedByTag, formattedByTag2) + "</color>");
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(text, text, Descriptor.DescriptorType.Requirement, false), Def.GetUISprite(recipeElement.material, "ui", false), Assets.GetPrefab(recipeElement.material).GetComponent<MutantPlant>() != null));
		}
		if (recipe.consumedHEP > 0)
		{
			HighEnergyParticleStorage component = this.target.GetComponent<HighEnergyParticleStorage>();
			list.Add(new SelectedRecipeQueueScreen.DescriptorWithSprite(new Descriptor(string.Format("<b>{0}</b>: {1} / {2}", UI.FormatAsLink(ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, "HEP"), recipe.consumedHEP, component.Particles), string.Format("<b>{0}</b>: {1} / {2}", ITEMS.RADIATION.HIGHENERGYPARITCLE.NAME, recipe.consumedHEP, component.Particles), Descriptor.DescriptorType.Requirement, false), new global::Tuple<Sprite, Color>(Assets.GetSprite("radbolt"), Color.white), false));
		}
		return list;
	}

	// Token: 0x04008747 RID: 34631
	public Image recipeIcon;

	// Token: 0x04008748 RID: 34632
	public LocText recipeName;

	// Token: 0x04008749 RID: 34633
	public LocText recipeMainDescription;

	// Token: 0x0400874A RID: 34634
	public LocText recipeDuration;

	// Token: 0x0400874B RID: 34635
	public ToolTip recipeDurationTooltip;

	// Token: 0x0400874C RID: 34636
	public GameObject IngredientsDescriptorPanel;

	// Token: 0x0400874D RID: 34637
	public GameObject EffectsDescriptorPanel;

	// Token: 0x0400874E RID: 34638
	public KNumberInputField QueueCount;

	// Token: 0x0400874F RID: 34639
	public MultiToggle DecrementButton;

	// Token: 0x04008750 RID: 34640
	public MultiToggle IncrementButton;

	// Token: 0x04008751 RID: 34641
	public KButton InfiniteButton;

	// Token: 0x04008752 RID: 34642
	public GameObject InfiniteIcon;

	// Token: 0x04008753 RID: 34643
	public GameObject ResearchRequiredContainer;

	// Token: 0x04008754 RID: 34644
	private ComplexFabricator target;

	// Token: 0x04008755 RID: 34645
	private ComplexFabricatorSideScreen ownerScreen;

	// Token: 0x04008756 RID: 34646
	private ComplexRecipe selectedRecipe;

	// Token: 0x04008757 RID: 34647
	[SerializeField]
	private GameObject recipeElementDescriptorPrefab;

	// Token: 0x04008758 RID: 34648
	private Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> recipeIngredientDescriptorRows = new Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject>();

	// Token: 0x04008759 RID: 34649
	private Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject> recipeEffectsDescriptorRows = new Dictionary<SelectedRecipeQueueScreen.DescriptorWithSprite, GameObject>();

	// Token: 0x0400875A RID: 34650
	[SerializeField]
	private FullBodyUIMinionWidget minionWidget;

	// Token: 0x0400875B RID: 34651
	[SerializeField]
	private MultiToggle previousRecipeButton;

	// Token: 0x0400875C RID: 34652
	[SerializeField]
	private MultiToggle nextRecipeButton;

	// Token: 0x02001FC8 RID: 8136
	private class DescriptorWithSprite
	{
		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x0600AC3B RID: 44091 RVA: 0x0010FEBA File Offset: 0x0010E0BA
		public Descriptor descriptor { get; }

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x0600AC3C RID: 44092 RVA: 0x0010FEC2 File Offset: 0x0010E0C2
		public global::Tuple<Sprite, Color> tintedSprite { get; }

		// Token: 0x0600AC3D RID: 44093 RVA: 0x0010FECA File Offset: 0x0010E0CA
		public DescriptorWithSprite(Descriptor desc, global::Tuple<Sprite, Color> sprite, bool filterRowVisible = false)
		{
			this.descriptor = desc;
			this.tintedSprite = sprite;
			this.showFilterRow = filterRowVisible;
		}

		// Token: 0x0400875F RID: 34655
		public bool showFilterRow;
	}
}
