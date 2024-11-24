using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodexConfigurableConsumerRecipePanel : CodexWidget<CodexConfigurableConsumerRecipePanel>
{
	private LocText title;

	private LocText result_description;

	private Image resultIcon;

	private GameObject ingredient_original;

	private IConfigurableConsumerOption data;

	private GameObject[] _ingredientRows;

	public CodexConfigurableConsumerRecipePanel(IConfigurableConsumerOption data)
	{
		this.data = data;
	}

	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		title = component.GetReference<LocText>("Title");
		result_description = component.GetReference<LocText>("ResultDescription");
		resultIcon = component.GetReference<Image>("ResultIcon");
		ingredient_original = component.GetReference<RectTransform>("IngredientPrefab").gameObject;
		ingredient_original.SetActive(value: false);
		CodexText codexText = new CodexText();
		LocText reference = ingredient_original.GetComponent<HierarchyReferences>().GetReference<LocText>("Name");
		codexText.ConfigureLabel(reference, textStyles);
		Clear();
		if (data != null)
		{
			title.text = data.GetName();
			result_description.text = data.GetDescription();
			result_description.color = Color.black;
			resultIcon.sprite = data.GetIcon();
			IConfigurableConsumerIngredient[] ingredients = data.GetIngredients();
			_ingredientRows = new GameObject[ingredients.Length];
			for (int i = 0; i < _ingredientRows.Length; i++)
			{
				_ingredientRows[i] = CreateIngredientRow(ingredients[i]);
			}
		}
	}

	public GameObject CreateIngredientRow(IConfigurableConsumerIngredient ingredient)
	{
		Tag[] iDSets = ingredient.GetIDSets();
		if (ingredient_original != null && iDSets.Length != 0)
		{
			GameObject gameObject = Util.KInstantiateUI(ingredient_original, ingredient_original.transform.parent.gameObject, force_active: true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			Tuple<Sprite, Color> uISprite = Def.GetUISprite(iDSets[0]);
			component.GetReference<Image>("Icon").sprite = uISprite.first;
			component.GetReference<Image>("Icon").color = uISprite.second;
			component.GetReference<LocText>("Name").text = iDSets[0].ProperName();
			component.GetReference<LocText>("Amount").text = GameUtil.GetFormattedByTag(iDSets[0], ingredient.GetAmount());
			component.GetReference<LocText>("Amount").color = Color.black;
			return gameObject;
		}
		return null;
	}

	public void Clear()
	{
		if (_ingredientRows != null)
		{
			for (int i = 0; i < _ingredientRows.Length; i++)
			{
				Object.Destroy(_ingredientRows[i]);
			}
			_ingredientRows = null;
		}
	}
}
